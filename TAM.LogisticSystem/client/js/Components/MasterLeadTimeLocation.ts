import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as mustache from 'mustache';
import * as service from '../services';

export class Controller implements angular.IController {
    static $inject = ['masterLeadTimeLocationService'];

    masterLeadTimeLocationService: service.MasterLeadTimeLocationService;
    masterLeadTimeLocationsData: service.MasterLeadTimeLocationViewModel[];
    masterLeadTimeLocationData: service.MasterLeadTimeLocationViewModel;
    routeComboBoxData: service.MasterLeadTimeLocationRouteComboBoxModel[];
    locationComboBoxData: service.MasterLeadTimeLocationLocationComboBoxModel[];
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    isUpdating: boolean;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    jsonConfirmData = {};
    searchTable = {};

    $onInit() {
        this.getMasterLeadTimeLocationData();
        this.getLocations();
        this.getRoutes();
        this.isUpdating = false;
        this.masterLeadTimeLocationData = new service.MasterLeadTimeLocationViewModel();
        this.masterLeadTimeLocationData.day = 0;
        this.masterLeadTimeLocationData.hour = 0;
        this.masterLeadTimeLocationData.minute = 0;
    }

    constructor(masterLeadTimeLocationService: service.MasterLeadTimeLocationService) {
        this.masterLeadTimeLocationService = masterLeadTimeLocationService;
    }

    //get all master lead time location data
    getMasterLeadTimeLocationData() {
        this.masterLeadTimeLocationService.getMasterLeadTimeLocationData().then(response => {
            this.masterLeadTimeLocationsData = response.data as service.MasterLeadTimeLocationViewModel[];
            this.convertLeadTime();
            this.generateTimeString();
            this.generateLocationString();
        });
    }

    //generate time string
    generateTimeString() {
        angular.forEach(this.masterLeadTimeLocationsData, (masterLeadTimeData) => {
            let hariString = '';
            let jamString = '';
            let menitString = '';
            hariString = masterLeadTimeData.day + ' Hari ';
            jamString = masterLeadTimeData.hour + ' Jam ';
            menitString = masterLeadTimeData.minute + ' Menit';
            masterLeadTimeData.leadMinutesString = (hariString + jamString + menitString).trim();
        });
    }

    //generate location string
    generateLocationString() {
        angular.forEach(this.masterLeadTimeLocationsData, (masterLeadTimeData) => {
            masterLeadTimeData.locationString = masterLeadTimeData.locationCode + ' - ' + masterLeadTimeData.locationName;
        });
    }

    //count day, hour, and minute of lead time into minute
    countLeadTime(day: number, hour: number, minute: number) {
        let leadMinute = (day * 24 * 60) + (hour * 60) + minute;
        return leadMinute;
    }

    //convert lead minute to day, hour, and minute
    convertLeadTime() {
        angular.forEach(this.masterLeadTimeLocationsData, (masterLeadTimeData) => {
            masterLeadTimeData.day = Math.floor(masterLeadTimeData.leadMinutes / 60 / 24);
            masterLeadTimeData.hour = Math.floor(masterLeadTimeData.leadMinutes / 60 % 24);
            masterLeadTimeData.minute = Math.floor(masterLeadTimeData.leadMinutes % 60);
        });
    }

    //get all locations code
    getLocations() {
        this.masterLeadTimeLocationService.getLocations().then(response => {
            this.locationComboBoxData = response.data as service.MasterLeadTimeLocationLocationComboBoxModel[];
        });
    }

    //get all route code and its name
    getRoutes() {
        this.masterLeadTimeLocationService.getRoutes().then(response => {
            this.routeComboBoxData = response.data as service.MasterLeadTimeLocationRouteComboBoxModel[];
        });
    }

    //convert time to 0 if the field is empty
    checkLeadTime() {
        if (this.masterLeadTimeLocationData.day == null) {
            this.masterLeadTimeLocationData.day = 0;
        }
        if (this.masterLeadTimeLocationData.hour == null) {
            this.masterLeadTimeLocationData.hour = 0;
        }
        if (this.masterLeadTimeLocationData.minute == null) {
            this.masterLeadTimeLocationData.minute = 0;
        }
    }

    //insert new master lead time location data
    createNewMasterLeadTimeLocationData(form: angular.IFormController) {
        this.checkLeadTime();
        this.jsonConfirmData['Lokasi'] = this.masterLeadTimeLocationData.locationCode;
        this.jsonConfirmData['Kode Rute'] = this.masterLeadTimeLocationData.processMasterCode;
        this.jsonConfirmData['Lead Time'] = this.masterLeadTimeLocationData.day + ' Hari ' + this.masterLeadTimeLocationData.hour + ' Jam ' + this.masterLeadTimeLocationData.minute + ' Menit';
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.jsonConfirmData)),
            () => {
                let masterLeadTimeLocationInsertUpdateData: service.MasterLeadTimeLocationInsertUpdateModel = {
                    processMasterCode: this.masterLeadTimeLocationData.processMasterCode,
                    leadMinutes: this.countLeadTime(this.masterLeadTimeLocationData.day, this.masterLeadTimeLocationData.hour, this.masterLeadTimeLocationData.minute),
                    locationCode: this.masterLeadTimeLocationData.locationCode
                };
                this.masterLeadTimeLocationService.createNewRoutingLocationLeadTimeData(masterLeadTimeLocationInsertUpdateData).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.getMasterLeadTimeLocationData();
                    this.resetForm(form);
                }).catch(response => {
                    if (response.status === 400) {
                        alertify.error(response.data);
                    }
                    else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //bind data from the selected row to the form
    setUpdateRow(masterLeadTimeLocationViewModel: service.MasterLeadTimeLocationViewModel) {
        this.masterLeadTimeLocationData = masterLeadTimeLocationViewModel;
        this.isUpdating = true;
    }

    //update data on selected row
    updateRow(form: angular.IFormController) {
        this.checkLeadTime();
        this.jsonConfirmData['Lokasi'] = this.masterLeadTimeLocationData.locationCode;
        this.jsonConfirmData['Kode Rute'] = this.masterLeadTimeLocationData.processMasterCode;
        this.jsonConfirmData['Lead Time'] = this.masterLeadTimeLocationData.day + ' Hari ' + this.masterLeadTimeLocationData.hour + ' Jam ' + this.masterLeadTimeLocationData.minute + ' Menit';
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('update', this.jsonConfirmData)),
            () => {
                let masterLeadTimeLocationInsertUpdateData: service.MasterLeadTimeLocationInsertUpdateModel = {
                    processMasterCode: this.masterLeadTimeLocationData.processMasterCode,
                    leadMinutes: this.countLeadTime(this.masterLeadTimeLocationData.day, this.masterLeadTimeLocationData.hour, this.masterLeadTimeLocationData.minute),
                    locationCode: this.masterLeadTimeLocationData.locationCode
                };
                this.masterLeadTimeLocationService.updateRow(masterLeadTimeLocationInsertUpdateData).then(response => {
                    this.isUpdating = false;
                    alertify.success('Data berhasil disimpan');
                    this.getMasterLeadTimeLocationData();
                    this.resetForm(form);
                }).catch(response => {
                    if (response.status === 400) {
                        alertify.error(response.data);
                    }
                    else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }


    //delete data on selected row
    deleteRow(masterLeadTimeLocationViewModel: service.MasterLeadTimeLocationViewModel, form: angular.IFormController) {
        this.jsonConfirmData['Lokasi'] = masterLeadTimeLocationViewModel.locationCode;
        this.jsonConfirmData['Kode Rute'] = masterLeadTimeLocationViewModel.processMasterCode;
        this.jsonConfirmData['Nama Rute'] = masterLeadTimeLocationViewModel.routeName;
        this.jsonConfirmData['Lead Time'] = masterLeadTimeLocationViewModel.leadMinutesString;
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', this.jsonConfirmData)),
            () => {
                this.masterLeadTimeLocationService.deleteRow(masterLeadTimeLocationViewModel.locationCode, masterLeadTimeLocationViewModel.processMasterCode).then(response => {
                    alertify.success('Data berhasil dihapus');
                    this.getMasterLeadTimeLocationData();
                    this.resetForm(form);
                }).catch(response => {
                    if (response.data === null) {
                        alertify.error(response.data);
                    }
                    else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //clean the form
    resetForm(form: angular.IFormController) {
        this.masterLeadTimeLocationData = new service.MasterLeadTimeLocationViewModel();
        this.masterLeadTimeLocationData.day = 0;
        this.masterLeadTimeLocationData.hour = 0;
        this.masterLeadTimeLocationData.minute = 0;
        this.searchTable = {};
        this.jsonConfirmData = {};
        this.isUpdating = false;
        form.$setPristine();
        form.$setUntouched();
    }

    //convert data to json for alertify modal
    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["insert"] = 1;
        else if (action.toLowerCase() == "update") convertResult["update"] = 1;
        else if (action.toLowerCase() == "delete") convertResult["delete"] = 1;

        convertResult["grid"] = tempJson;
        return convertResult;
    }

    //set current page
    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }
}

let MasterLeadTimeLocation = {
    template: require('./MasterLeadTimeLocation.html'),
    controller: Controller,
    controllerAs: 'me'
}

export { MasterLeadTimeLocation }