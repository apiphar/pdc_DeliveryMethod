import * as angular from 'angular';
import * as service from '../services'
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as _ from 'lodash';

export class MasterCityLocationContoller implements angular.IController {
    static $inject = ['MasterCityLocationService', '$rootScope'];

    constructor(masterCityLocationService: service.MasterCityLocationService, root: angular.IRootScopeService) {
        this.masterCityLocationService = masterCityLocationService;
        this.$rootScope = root;
    }

    $onInit() {
        this.getAllTableData();
        this.isEdit = false;
        this.masterCityLocationModel = new service.MasterCityLocationModel();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.getAllTableData();
        })
    }

    //private read only
    masterCityLocationService: service.MasterCityLocationService;
    $rootScope: angular.IRootScopeService;

    //model
    masterCityLocationModel: service.MasterCityLocationModel;
    masterCityLocationModels: service.MasterCityLocationModel[];
    searchMasterCityLocationModels: service.MasterCityLocationModel[];
    jsonMasterCityLocation = {};

    //variable
    isEdit: boolean;
    pageState: boolean = true;
    cityLocationCodeRegex = '[a-zA-Z0-9]+';
    cityLocationRegex = '[a-zA-Z0-9\\s-&/\',.]+';
    loader: boolean = true;
    search = {};

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.kodeCityLocation);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "DealerType";
        info.title = "Group Dealer";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "DealerType";
        info.title = "Group Dealer";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

    getAllTableData() {
        this.masterCityLocationService.getAllTableData().then(response => {
            let self = this;
            this.masterCityLocationModels = response.data;
            this.loader = false;
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    updateSelected(updateData: any) {
        this.isEdit = true;
        this.masterCityLocationModel = new service.MasterCityLocationModel();
        this.masterCityLocationModel.cityLocation = updateData.cityLocation;
        this.masterCityLocationModel.kodeCityLocation = updateData.kodeCityLocation;
    }

    deleteSelected(deletedId: string, form: angular.IFormController) {
        let deletedModel = _.find(this.masterCityLocationModels, { 'kodeCityLocation': deletedId });
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("delete", this.confirmationData(deletedModel))),
            () => {
                this.masterCityLocationService.deleteData(deletedId).then(response => {
                    this.getAllTableData();
                    alertify.success("Data berhasil di hapus");
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    }
                    else {
                        alertify.error("Data gagal dihapus");
                    }
                }).finally(() => {
                    this.reset(form);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    createData(form: angular.IFormController) {
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("insert", this.confirmationData(this.masterCityLocationModel))),
            () => {
                this.masterCityLocationService.createData(this.masterCityLocationModel).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.getAllTableData();
                    this.reset(form);
                }).catch(response => {
                    if (response.data == "Kode City Location sudah terdaftar") {
                        alertify.error(response.data);
                    }
                    if (response.data == "Data tidak valid") {
                        alertify.error(response.data);
                    }
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(form: angular.IFormController) {
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.confirmationData(this.masterCityLocationModel))),
            () => {
                this.masterCityLocationService.updateData(this.masterCityLocationModel).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.getAllTableData();
                    this.reset(form);
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    }
                }).finally(() => {
                    this.reset(form);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    confirmationData(masterCityLocationModel: any) {
        this.jsonMasterCityLocation['Kode Group Dealer'] = masterCityLocationModel.kodeCityLocation;
        this.jsonMasterCityLocation['Group Dealer'] = masterCityLocationModel.cityLocation;
        return this.jsonMasterCityLocation;
    }

    reset(form: angular.IFormController) {
        this.isEdit = false;
        this.masterCityLocationModel = new service.MasterCityLocationModel();
        this.masterCityLocationModel.kodeCityLocation = '';
        this.masterCityLocationModel.cityLocation = '';
        this.getAllTableData();
        this.search = {};
        form.$setPristine();
        form.$setUntouched();
    }

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

}

let masterCityLocation = {
    controller: MasterCityLocationContoller,
    controllerAs: 'me',
    template: require('./MasterCityLocation.html')
}

export { masterCityLocation }