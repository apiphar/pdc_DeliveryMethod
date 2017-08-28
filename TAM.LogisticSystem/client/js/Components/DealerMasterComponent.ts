import * as angular from 'angular';
import * as Service from '../Services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';

export class DealerMasterController implements angular.IController {
    static $inject = ['DealerMasterService', '$rootScope'];

    constructor(dealerMaster: Service.DealerMasterService, root: angular.IRootScopeService) {
        this.dealerMaster = dealerMaster;
        this.root = root;
    }
    dealerMaster: Service.DealerMasterService;
    root: angular.IRootScopeService

    // Model property
    dealerMasterViewModels: Service.DealerMasterViewModel[];
    dealerTypeCodes: Service.DealerMasterTypeCode[];
    dealerMasterSend: Service.DealerMasterViewModel;

    // Other property
    pageState: boolean = true;
    onUpdateClick: boolean = false;
    codePattern: string = '^[a-zA-Z0-9]+';
    namePattern: string = '^[a-zA-Z0-9\\s-.&,\'/]+';
    search = {};

    $onInit() {
        this.refreshData();
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
        this.dealerMasterSend = new Service.DealerMasterViewModel();
    }

    refreshData() {
        this.getAll();
    }

    getAll() {
        return this.dealerMaster.getAll().then(response => {
            this.dealerMasterViewModels = response.data.viewModels as Service.DealerMasterViewModel[];
            this.dealerTypeCodes = response.data.dealerTypeCodes as Service.DealerMasterTypeCode[];
            this.convertDealerType();
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    convertDealerType() {
        angular.forEach(this.dealerMasterViewModels, viewModel => {
            viewModel.dealerTypeString = viewModel.dealerTypeCode.dealerTypeCode + ' - ' + viewModel.dealerTypeCode.dealerTypeName;
        });
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.dealerCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Dealer";
        info.title = "Dealer";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    fillData(item: Service.DealerMasterViewModel) {
        let tempData = new Service.DealerMasterViewModel();
        tempData.dealerCode = item.dealerCode;
        tempData.dealerName = item.dealerName;
        tempData.dealerAddress = item.dealerAddress;
        tempData.dealerTypeCode = item.dealerTypeCode;
        return tempData;
    }

    fillForm(item: Service.DealerMasterViewModel) {
        this.dealerMasterSend = this.fillData(item);
        this.onUpdateClick = true;
    }

    disableButton() {
        if (this.dealerMasterSend.dealerTypeCode == null) {
            return true;
        }
        return false;
    }

    confirmationData(item: Service.DealerMasterViewModel) {
        let jsonData = {};
        jsonData['Kode Dealer'] = item.dealerCode;
        jsonData['Dealer'] = item.dealerName;
        jsonData['Alamat Dealer'] = item.dealerAddress;
        jsonData['Kode Group Dealer'] = item.dealerTypeCode.dealerTypeCode + ' - ' + item.dealerTypeCode.dealerTypeName;
        return jsonData;
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

    updateData(form: angular.IFormController) {
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('update', this.confirmationData(this.dealerMasterSend))),
            () => {
                this.dealerMaster.updateData(this.dealerMasterSend).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.refreshData();
                    this.setPristine(form);
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    setPristine(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.dealerMasterSend = new Service.DealerMasterViewModel();
        this.dealerMasterSend.dealerCode = null;
        this.dealerMasterSend.dealerName = null;
        this.dealerMasterSend.dealerAddress = null;
        this.onUpdateClick = false;
        this.search = {};
    }

    // Pagination
    pageSizes = [5, 10, 15, 20, 25];
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
}

export class DealerMasterComponent implements angular.IComponentOptions {
    controller = DealerMasterController;
    controllerAs = 'me';

    template = require('./DealerMaster.html');
    bindings = {
        greet: '@',
        //ftoken: '@'
    };
}