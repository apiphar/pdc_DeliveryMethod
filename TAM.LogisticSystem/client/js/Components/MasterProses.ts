import * as angular from 'angular';
import * as Service from '../services';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';
import * as Lodash from 'lodash';

export class MasterProsesController implements angular.IController {
    static $inject = ['MasterProsesService', 'LeadTimeByService', '$timeout'];

    _httpState: boolean = false;
    masterProsesService: Service.MasterProsesService;
    leadTimeByService: Service.LeadTimeByService;
    masterProsesDataList;
    processLeadTimeByEnumDataList;
    masterProsesDataActive: MasterProsesData;
    masterProsesDataDelete: MasterProsesData;
    componentState = {
        edit: false,
        delete: false,
        page: false
    };
    masterProsesForm: angular.IFormController;
    search: MasterProsesSearch = {
        name: undefined,
        processMasterCode: undefined,
        bufferTime: undefined,
        isScan: [],
        processLeadTimeByEnumId: []
    };
    order = {
        name: 'processMasterCode',
        direction: false
    }

    currentPage = 1;
    maxSize = 5;
    pageSizes = [5, 10, 15, 20, 25];
    pageSize = 5;
    timeout: angular.ITimeoutService;

    constructor(service: Service.MasterProsesService, leadTimeByService: Service.LeadTimeByService, timeout: angular.ITimeoutService) {
        this.masterProsesService = service;
        this.leadTimeByService = leadTimeByService;
        this.timeout = timeout;
    }

    $onInit() {
        this.fetchprocessLeadTimeByEnumDataList();
        this.fetchMasterProsesDataList();
        this.deactivateEdit();
    }

    defaultMasterProsesSearch() {
        return {
            name: undefined,
            processMasterCode: undefined,
            bufferTime: undefined,
            isScan: [],
            processLeadTimeByEnumId: []
        };
    }

    // Reset container data list
    defaultMasterProsesData() {
        return {
            processMasterCode: null,
            name: null,
            isScan: undefined,
            processLeadTimeByEnumId: undefined,
            bufferMinutes: 0,
            swappingPoint: false
        };
    }

    // fetching Lead Time By data list
    fetchprocessLeadTimeByEnumDataList() {
        this.leadTimeByService.getRoutingLeadTimeByList().then(response => {
            this.processLeadTimeByEnumDataList = response.data;
            this.defaultSearchCustom();
            this.masterProsesDataActive = this.defaultMasterProsesData();
            this.masterProsesDataDelete = this.defaultMasterProsesData();
        });
    }

    // fetching Master Proses data list
    fetchMasterProsesDataList() {
        this.masterProsesService.getMasterProsesList().then(response => {
            this.masterProsesDataList = response.data;
            this.componentState.page = true;

            this.parseLeadTimeByOrder();
        });
    }

    _parseLeadTimeByOrderThrottle = 0;
    parseLeadTimeByOrder() {
        if (!this.processLeadTimeByEnumDataList) {
            if (this._parseLeadTimeByOrderThrottle >= 20) return;

            this.timeout(this.parseLeadTimeByOrder, 100);
            this._parseLeadTimeByOrderThrottle++;
        }

        Lodash.each(this.masterProsesDataList, (item) => {
            item._parsedOrder = false;
            Lodash.each(this.processLeadTimeByEnumDataList, (leadTime) => {
                let id = 'orderLeadTime' + leadTime.processLeadTimeByEnumId;
                if (item.processLeadTimeByEnumId == leadTime.processLeadTimeByEnumId) item[id] = 1;
                else item[id] = 0;
            });
        });
    }

    // parsing confirmation data for alertify + mustachejs
    getConfirmationMessage() {
        let activeData = this.masterProsesDataActive;
        if (this.componentState.delete) activeData = this.masterProsesDataDelete;

        let grid = [
            { key: 'Kode Proses', value: activeData.processMasterCode },
            { key: 'Nama Proses', value: activeData.name },
            { key: 'Buffer Time', value: activeData.bufferMinutes + ' menit' },
            { key: 'Proses Scan', value: activeData.isScan ? 'Ya' : 'Tidak' },
            { key: 'Lead Time Mengikuti', value: this.getLeadTime(activeData.processLeadTimeByEnumId).name }
        ];

        let title = '';
        if (this.componentState.edit) return { grid, update: true }
        else if (this.componentState.delete) return { grid, delete: true }
        else if (!this.componentState.delete) return { grid, insert: true }
        else return {};
    }

    // check kode proses avaibility
    _timer: any;
    kodeProsesAvailability: boolean = true;
    checkKodeProsesAvailability() {
        if (this.componentState.edit) {
            this.kodeProsesAvailability = true;
            return;
        }
        if (this._timer != undefined) this.timeout.cancel(this._timer);

        let self = this;
        this._timer = this.timeout(() => {
            if (self == undefined) return;

            let found = Lodash.find(self.masterProsesDataList, { 'processMasterCode': self.masterProsesDataActive.processMasterCode });
            if (self.masterProsesDataActive.processMasterCode == '') self.kodeProsesAvailability = true;
            else if (found == undefined) self.kodeProsesAvailability = true;
            else self.kodeProsesAvailability = false;
        }, 375);
    }

    // setting order by grid
    setOrderBy(name: string, direction = undefined) {
        if (this.order.name == name) this.order.direction = (direction == undefined) ? !this.order.direction : direction;
        else {
            this.order.name = name;
            this.order.direction = (direction == undefined) ? false : direction;
        }
    }

    // activating edit mode
    actionEdit(data: MasterProsesData, key: number) {
        angular.copy(data, this.masterProsesDataActive);
        this.activateEdit(key);
    }

    // confirmation delete
    actionDelete(data: MasterProsesData) {
        angular.copy(data, this.masterProsesDataDelete);
        this.activateDelete();

        Alertify.confirm(
            'Konfirmasi',
            Mustache.render(require('./alertify/MasterAlertify.html'), this.getConfirmationMessage()),
            () => { this.actionConfirmDelete(); },
            () => { this.deactivateDelete(); }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    actionSaveAddEdit(form: angular.IFormController) {
        if (this.componentState.edit) return this.actionSaveEdit(form);
        else return this.actionSaveAdd(form);
    }

    // confirmation create new data
    actionSaveAdd(form: angular.IFormController) {
        if (this.componentState.edit) return;

        if (!this.validationAction(form)) return;

        this.masterProsesForm = form;
        Alertify.confirm(
            'Konfirmasi',
            Mustache.render(require('./alertify/MasterAlertify.html'), this.getConfirmationMessage()),
            () => { this.saveAdd(); },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // confirmation updating data
    actionSaveEdit(form: angular.IFormController) {
        if (!this.componentState.edit) return;

        if (!this.validationAction(form)) return;

        this.masterProsesForm = form;
        Alertify.confirm(
            'Konfirmasi',
            Mustache.render(require('./alertify/MasterAlertify.html'), this.getConfirmationMessage()),
            () => { this.saveEdit(); },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // validation action save and edit
    validationObjects: any = [
        {
            _id: 'kodeProses',
            required: 'Kode Proses harus di isi',
            pattern: 'Kode Proses harus berisi alphanumeric'
        },
        {
            _id: 'namaProses',
            required: 'Nama Proses harus di isi'
        },
        {
            _id: 'bufferMinutes',
            required: 'Buffer Time harus di isi'
        }
    ];
    validationAction(form: angular.IFormController) {
        if (this.isFormValid(form)) return true;

        let objects = this.validationObjects;
        for (let i = 0; i < objects.length; i++) {
            let item = objects[i];
            let error = form[item._id].$error;
            if (error) {
                for (let e in error) {
                    let msg = item[e];
                    if (error[e] && msg) {
                        Alertify.error(msg);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    // saving new data
    saveAdd() {
        if (this.componentState.edit) return;
        this._httpState = true;

        this.masterProsesService.postMasterProses(this.masterProsesDataActive)
            .then(
                () => {
                    this.masterProsesDataList.push(this.masterProsesDataActive);
                    this.parseLeadTimeByOrder();
                    Alertify.success('Data tersimpan');
                    this.resetDataActive();
                },
                () => {
                    Alertify.error('Data gagal tersimpan');
                    this.resetDataActive();
                }
            )
            .catch(response => {
                if (response.status == '400') {
                    Alertify.error(response.data);
                    return;
                }

                if (response.status == '500') {
                    Alertify.error('Koneksi server bermasalah');
                    return;
                }
            });
    }

    // saving edited data
    saveEdit() {
        if (!this.componentState.edit) return;
        this._httpState = true;

        this.masterProsesService.putMasterProses(this.masterProsesDataActive)
            .then(
                () => {
                    Alertify.success('Data berhasil disimpan');
                    angular.forEach(this.masterProsesDataList, (value, key) => {
                        if (value.processMasterCode == this.masterProsesDataActive.processMasterCode) {
                            this.masterProsesDataList[key].bufferMinutes = this.masterProsesDataActive.bufferMinutes;
                            this.masterProsesDataList[key].swappingPoint = this.masterProsesDataActive.swappingPoint;
                            this.masterProsesDataList[key].isScan = this.masterProsesDataActive.isScan;
                            this.masterProsesDataList[key].name = this.masterProsesDataActive.name;
                            this.masterProsesDataList[key].processLeadTimeByEnumId = this.masterProsesDataActive.processLeadTimeByEnumId;
                        }
                    });
                    this.deactivateEdit();
                    this.resetDataActive();
                },
                () => {
                    Alertify.error('Data gagal disimpan');
                    this.deactivateEdit();
                    this.resetDataActive();
                }
            )
            .catch(response => {
                if (response.status == '400') {
                    Alertify.error(response.data);
                    return;
                }

                if (response.status == '500') {
                    Alertify.error('Koneksi server bermasalah');
                    return;
                }
            });
    }

    // cancel editing
    actionCancelEdit(form: angular.IFormController) {
        this.masterProsesForm = form;
        this.deactivateEdit();
        this.resetDataActive();
    }

    // deleting data
    actionConfirmDelete() {
        if (!this.componentState.delete) return;
        this._httpState = true;

        this.masterProsesService.deleteMasterProses(this.masterProsesDataDelete)
            .then(
                () => {
                    angular.forEach(this.masterProsesDataList, (value, key) => {
                        if (value.processMasterCode == this.masterProsesDataDelete.processMasterCode) {
                            this.masterProsesDataList.splice(key, 1);
                            Alertify.success('Data berhasil dihapus');
                            this.deactivateDelete(true);
                        }
                    });
                },
                () => {
                    Alertify.error('Data tidak bisa dihapus');
                    this.deactivateDelete(true);
                }
            )
            .catch(response => {
                if (response.status == '400') {
                    Alertify.error(response.data);
                    return;
                }

                if (response.status == '500') {
                    Alertify.error('Koneksi server bermasalah');
                    return;
                }
            });
    }

    // get Lead Time By detail by LeadTimeById
    getLeadTime(id: number): RoutingLeadTimeByData {
        if (!this.processLeadTimeByEnumDataList) return null;

        let leadtime = null;
        for (let i = 0; i < this.processLeadTimeByEnumDataList.length; i++) {
            if (this.processLeadTimeByEnumDataList[i].processLeadTimeByEnumId == id) {
                leadtime = this.processLeadTimeByEnumDataList[i];
                break;
            }
        }

        return leadtime;
    }

    // check form validation, if valid submit button clickable
    isFormValid(form: angular.IFormController) {
        return angular.equals(form.$error, {});
    }

    isInputValid(input) {
        return angular.equals(input.$error, {});
    }

    // reset
    resetDataActive() {
        this.masterProsesForm.$setPristine();
        this.masterProsesDataActive = this.defaultMasterProsesData();
        this.masterProsesDataDelete = this.defaultMasterProsesData();
        this.search = this.defaultMasterProsesSearch();
        this.defaultSearchCustom();
        this._httpState = false;
    }

    // turn on edit state
    activateEdit(key) {
        this.componentState.delete = false;
        this.componentState.edit = true;
    }

    // turn on delete state
    activateDelete() {
        this.componentState.delete = true;
    }

    // turn off edit state
    deactivateEdit() {
        this.componentState.edit = false;
    }

    // turn off delete state
    deactivateDelete(careForEdit: boolean = false) {
        this.componentState.delete = false

        if (careForEdit && this.componentState.edit && this.masterProsesDataDelete.processMasterCode == this.masterProsesDataActive.processMasterCode) {
            this.masterProsesDataActive = this.defaultMasterProsesData();
            this.deactivateEdit();
        }

        this.masterProsesDataDelete = this.defaultMasterProsesData();
        this.search = this.defaultMasterProsesSearch();
        this.defaultSearchCustom();
        this._httpState = false;
    }

    // fetch default search
    defaultSearchCustom() {
        this.search.isScan[1] = true;
        this.search.isScan[0] = true;

        angular.forEach(this.processLeadTimeByEnumDataList, (value) => {
            this.search.processLeadTimeByEnumId[value.processLeadTimeByEnumId] = true;
        });
    }
}

// filter for checklist master proses
export function masterProsesDataListFilter() {
    return (items, component) => {
        if (!items) return [];

        let filtered = [];
        angular.forEach(items, (value, key) => {
            let _string = true;
            let _isScan = true;
            let _routingLeadTimeBy = true;

            if (component.search.processMasterCode && value.processMasterCode.toLowerCase().indexOf(component.search.processMasterCode.toLowerCase()) == -1) _string = false;
            if (component.search.name && value.name.toLowerCase().indexOf(component.search.name.toLowerCase()) == -1) _string = false;
            if (component.search.bufferMinutes && value.bufferMinutes.toString().toLowerCase().indexOf(component.search.bufferMinutes.toString().toLowerCase()) == -1) _string = false;

            _isScan = component.search.isScan[(value.isScan == true ? 1 : 0)];
            _routingLeadTimeBy = component.search.processLeadTimeByEnumId[value.processLeadTimeByEnumId];

            if (_string && _isScan && _routingLeadTimeBy) filtered.push(value);
        });

        return filtered;
    }
}

// interface master proses data
export interface MasterProsesData {
    processMasterCode: string,
    name: string,
    isScan: boolean,
    processLeadTimeByEnumId: number,
    bufferMinutes: number,
    swappingPoint: boolean
}

// interface routing lead time by data
export interface RoutingLeadTimeByData {
    processLeadTimeByEnumId: number,
    name: string
}

// interface master proses search
export interface MasterProsesSearch {
    processMasterCode: string,
    name: string,
    bufferTime: number,
    isScan: Array<boolean>,
    processLeadTimeByEnumId: Array<boolean>
}


export class MasterProsesComponent implements angular.IComponentOptions {
    controller = MasterProsesController;
    controllerAs = 'masterProses';

    template = require('./MasterProses.html');
    bindings = {};
}