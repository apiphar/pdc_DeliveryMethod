import * as angular from 'angular';
import * as service from '../services'
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as _ from 'lodash';

export class MasterGroupDealerContoller implements angular.IController {
    static $inject = ['MasterGroupDealerService', '$rootScope'];

    constructor(masterGroupDealerService: service.MasterGroupDealerService, root: angular.IRootScopeService) {
        this.masterGroupDealerService = masterGroupDealerService;
        this.$rootScope = root;
    }

    $onInit() {
        this.getAllTableData();
        this.isEdit = false;
        this.masterGroupDealerModel = new service.MasterGroupDealerModel();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.getAllTableData();
        })
    }

    //private read only
    masterGroupDealerService: service.MasterGroupDealerService;
    $rootScope: angular.IRootScopeService;

    //model
    masterGroupDealerModel: service.MasterGroupDealerModel;
    masterGroupDealerModels: service.MasterGroupDealerModel[];
    searchMasterGroupDealerModels: service.MasterGroupDealerModel[];
    jsonMasterGroupDealer = {};

    //variable
    isEdit: boolean;
    pageState: boolean = true;
    groupDealerCodeRegex = '[a-zA-Z0-9]+';
    groupDealerRegex = '[a-zA-Z0-9\\s-&/\',.]+';
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
            tempData.push(data.kodeGroupDealer);
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
        this.masterGroupDealerService.getAllTableData().then(response => {
            let self = this;
            this.masterGroupDealerModels = response.data;
            this.loader = false;
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    updateSelected(updateData: any) {
        this.isEdit = true;
        this.masterGroupDealerModel = new service.MasterGroupDealerModel();
        this.masterGroupDealerModel.groupDealer = updateData.groupDealer;
        this.masterGroupDealerModel.kodeGroupDealer = updateData.kodeGroupDealer;
    }

    deleteSelected(deletedId: string, form: angular.IFormController) {
        let deletedModel = _.find(this.masterGroupDealerModels, { 'kodeGroupDealer': deletedId });
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("delete", this.confirmationData(deletedModel))),
            () => {
                this.masterGroupDealerService.deleteData(deletedId).then(response => {
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

    createData(form: angular.IFormController) {
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("insert", this.confirmationData(this.masterGroupDealerModel))),
            () => {
                this.masterGroupDealerService.createData(this.masterGroupDealerModel).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.getAllTableData();
                    this.reset(form);
                }).catch(response => {
                    if (response.data == "Kode Group Dealer sudah terdaftar") {
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
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.confirmationData(this.masterGroupDealerModel))),
            () => {
                this.masterGroupDealerService.updateData(this.masterGroupDealerModel).then(response => {
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

    confirmationData(masterGroupDealerModel: any) {
        this.jsonMasterGroupDealer['Kode Group Dealer'] = masterGroupDealerModel.kodeGroupDealer;
        this.jsonMasterGroupDealer['Group Dealer'] = masterGroupDealerModel.groupDealer;
        return this.jsonMasterGroupDealer;
    }

    reset(form: angular.IFormController) {
        this.isEdit = false;
        this.masterGroupDealerModel = new service.MasterGroupDealerModel();
        this.masterGroupDealerModel.kodeGroupDealer = '';
        this.masterGroupDealerModel.groupDealer = '';
        this.getAllTableData();
        this.search = {};
        form.$setPristine();
        form.$setUntouched();
    }
}

let masterGroupDealer = {
    controller: MasterGroupDealerContoller,
    controllerAs: 'me',
    template: require('./MasterGroupDealer.html')
}

export { masterGroupDealer }