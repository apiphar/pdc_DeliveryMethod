import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as moment from 'moment';
import * as _ from 'lodash';
import * as service from '../Services';

export class AFIRestriksiAreaController {
    static $inject = ['AFIRestriksiAreaService', '$rootScope'];

    root: angular.IRootScopeService;

    afiRestriksiAreaService: service.AFIRestriksiAreaService;
    afiRegionsRestriction: service.AFIRestriksiAreaViewModel[];
    afiRegionRestriction: service.AFIRestriksiAreaViewModel;
    provinces: service.AFIRestriksiAreaRegionModel[];

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;

    isUpdate: boolean = false;
    isDetail: boolean = false;
    searchTable = {};
    jsonConfirmData = {};
    dateOptionsValidFrom = {
        minDate: new Date()
    }
    dateOptionsValidTo = {
        minDate: new Date()
    }

    constructor(afiRestriksiAreaService: service.AFIRestriksiAreaService, root: angular.IRootScopeService) {
        this.afiRestriksiAreaService = afiRestriksiAreaService;
        this.root = root;
    }

    $onInit() {
        this.getData();
        this.root.$on('isDetail', (event, data) => {
            this.isDetail = data;
        });
    }

    //get all data for view
    getData() {
        this.afiRestriksiAreaService.getAllData().then(response => {
            this.afiRegionsRestriction = response.data.afiRegionsRestriction;
            this.generateRegionString();
            this.generateValidFromString();
            this.generateValidToString();
            this.provinces = response.data.regions;
            this.totalItems = this.afiRegionsRestriction.length;
        }).catch(response => {
            if (response.status === 500) {
                alertify.error('Koneksi ke server bermasalah');
            }
        });
    }

    //generate region code + string for view
    generateRegionString() {
        angular.forEach(this.afiRegionsRestriction, (data) => {
            data.regionString = data.regionCode + ' - ' + data.name;
        });
    }

    //convert valid from date into string with dd-MMM-yyyy format
    generateValidFromString() {
        angular.forEach(this.afiRegionsRestriction, (data) => {
            data.validFromString = moment(data.validFrom).format('DD-MMM-YYYY');
        });
    }

    //convert valid to date into string with dd-MMM-yyyy format
    generateValidToString() {
        angular.forEach(this.afiRegionsRestriction, (data) => {
            data.validToString = moment(data.validTo).format('DD-MMM-YYYY');
        });
    }

    //get detail of row data
    getDetail(data: service.AFIRestriksiAreaViewModel) {
        this.isDetail = true;
        let parentData: service.AFIRestriksiAreaParentDataModel = {
            regionCode: data.regionCode,
            validFrom: data.validFrom,
            validTo: data.validTo
        };
        this.root.$broadcast('parentData', parentData);
    }

    //trigger event on selecting valid from date
    validFromChange() {
        this.dateOptionsValidTo.minDate = new Date(this.afiRegionRestriction.validFrom);
    }

    //check inputed date range
    isDateSliced() {
        let existedData = _.filter(this.afiRegionsRestriction, ['regionCode', this.afiRegionRestriction.regionCode]);
        if (existedData.length > 0) {
            let validFrom = new Date(this.afiRegionRestriction.validFrom);
            let validTo = new Date(this.afiRegionRestriction.validTo);
            let isSliced = 0;
            angular.forEach(existedData, (data) => {
                var startDate = new Date(data.validFrom);
                var endDate = new Date(data.validTo);
                if ((validFrom.valueOf() >= startDate.valueOf() && validFrom.valueOf() <= endDate.valueOf()) || (validTo.valueOf() >= startDate.valueOf() && validTo.valueOf() <= endDate.valueOf())) {
                    isSliced += 1;
                }
            });
            if (isSliced > 0) {
                return false;
            }
        }
        return true;
    }

    //insert data into database
    create(form: angular.IFormController) {
        this.jsonConfirmData['Provinsi'] = this.afiRegionRestriction.regionCode + ' - ' + (_.find(this.provinces, ['regionCode', this.afiRegionRestriction.regionCode])).name;
        this.jsonConfirmData['Is Locked'] = this.afiRegionRestriction.isLocked === true ? 'Ya' : 'Tidak';
        this.jsonConfirmData['Valid From'] = moment(this.afiRegionRestriction.validFrom).format('DD-MMM-YYYY');
        this.jsonConfirmData['Valid To'] = moment(this.afiRegionRestriction.validTo).format('DD-MMM-YYYY');
        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.jsonConfirmData)),
            () => {
                if (this.isDateSliced() === false) {
                    alertify.error('Batas waktu yang diinput harus di luar batas waktu sebelumnya');
                }
                else {
                    let newData: service.AFIRestriksiAreaInsertModel = {
                        regionCode: this.afiRegionRestriction.regionCode,
                        isLocked: this.afiRegionRestriction.isLocked,
                        validFrom: this.afiRegionRestriction.validFrom,
                        validTo: this.afiRegionRestriction.validTo
                    };
                    this.afiRestriksiAreaService.create(newData).then(response => {
                        this.afiRegionsRestriction = undefined;
                        this.getData();
                        this.resetForm(form);
                        alertify.success('Data berhasil disimpan');
                    }).catch(response => {
                        if (response.status === 500) {
                            alertify.error('Koneksi ke server bermasalah');
                            return false;
                        }
                        if (response.status === 400) {
                            alertify.error(response.data);
                            return false;
                        }
                        alertify.error('Data gagal disimpan');
                        return false;
                    });
                }
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        return true;
    }

    //bind the selected row data to the form
    setUpdate(data: service.AFIRestriksiAreaViewModel) {
        this.isUpdate = true;
        this.afiRegionRestriction = new service.AFIRestriksiAreaViewModel();
        this.afiRegionRestriction.afiRegionRestrictionId = data.afiRegionRestrictionId;
        this.afiRegionRestriction.regionCode = data.regionCode;
        this.afiRegionRestriction.isLocked = data.isLocked;
        this.afiRegionRestriction.validFrom = new Date(data.validFrom);
        this.dateOptionsValidFrom.minDate = new Date(data.validFrom);
        this.dateOptionsValidTo.minDate = new Date(data.validFrom);
        this.afiRegionRestriction.validTo = new Date(data.validTo);
    }

    //update the data in the database based on the form data
    update(form: angular.IFormController) {
        this.jsonConfirmData['Provinsi'] = this.afiRegionRestriction.regionCode + ' - ' + (_.find(this.provinces, ['regionCode', this.afiRegionRestriction.regionCode])).name;
        this.jsonConfirmData['Is Locked'] = this.afiRegionRestriction.isLocked === true ? 'Ya' : 'Tidak';
        this.jsonConfirmData['Valid From'] = moment(this.afiRegionRestriction.validFrom).format('DD-MMM-YYYY');
        this.jsonConfirmData['Valid To'] = moment(this.afiRegionRestriction.validTo).format('DD-MMM-YYYY');
        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('update', this.jsonConfirmData)),
            () => {
                let newData: service.AFIRestriksiAreaUpdateModel = {
                    afiRegionRestrictionId: this.afiRegionRestriction.afiRegionRestrictionId,
                    regionCode: this.afiRegionRestriction.regionCode,
                    isLocked: this.afiRegionRestriction.isLocked,
                    validFrom: this.afiRegionRestriction.validFrom,
                    validTo: this.afiRegionRestriction.validTo
                };
                this.afiRestriksiAreaService.update(newData).then(response => {
                    this.afiRegionsRestriction = undefined;
                    this.getData();
                    this.resetForm(form);
                    alertify.success('Data berhasil disimpan');
                }).catch(response => {
                    if (response.status === 500) {
                        alertify.error('Koneksi ke server bermasalah');
                        return false;
                    }
                    if (response.status === 400) {
                        alertify.error(response.data);
                        return false;
                    }
                    alertify.error('Data gagal disimpan');
                    return false;
                });

            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        return false;
    }

    //delete the data of the selected row
    delete(data: service.AFIRestriksiAreaViewModel, form: angular.IFormController) {
        this.jsonConfirmData['Provinsi'] = data.regionCode + ' - ' + (_.find(this.provinces, ['regionCode', data.regionCode])).name;
        this.jsonConfirmData['Is Locked'] = data.isLocked === true ? 'Ya' : 'Tidak';
        this.jsonConfirmData['Valid From'] = moment(data.validFrom).format('DD-MMM-YYYY');
        this.jsonConfirmData['Valid To'] = moment(data.validTo).format('DD-MMM-YYYY');
        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', this.jsonConfirmData)),
            () => {
                this.afiRestriksiAreaService.delete(data.afiRegionRestrictionId).then(response => {
                    alertify.success('Data berhasil dihapus');
                    this.afiRegionsRestriction = undefined;
                    this.getData();
                    this.resetForm(form);

                }).catch(response => {
                    if (response.status === 500) {
                        alertify.error('Koneksi ke server bermasalah');
                        return false;
                    }
                    alertify.error('Data gagal dihapus');
                    return false;
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //reset the form
    resetForm(form: angular.IFormController) {
        this.afiRegionRestriction = new service.AFIRestriksiAreaViewModel();
        this.isUpdate = false;
        this.jsonConfirmData = {};
        this.searchTable = {};
        this.dateOptionsValidFrom.minDate = new Date();
        this.dateOptionsValidTo.minDate = new Date();
        form.$setPristine();
        form.$setUntouched();
    }

    //convert json object tfor laertify pop up
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

    //ordering table data
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    //search the data based on input in the data grid
    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }
}

let AFIRestriksiArea = {
    template: require('./AFIRestriksiArea.html'),
    controller: AFIRestriksiAreaController,
    controllerAs: 'me',
}

export { AFIRestriksiArea };