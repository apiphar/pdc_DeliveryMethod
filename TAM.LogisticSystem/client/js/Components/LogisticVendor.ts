
import * as lodash from 'lodash';
import * as angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';


export class LogisticVendorController implements angular.IController {
    static $inject = ['LogisticVendorService', '$rootScope'];

    LogisticVendor: Service.LogisticVendorService;
    dataTable: Service.DeliveryVendorViewModel[];
    showUpdate: boolean;

    deliveryVendorCreateModel: Service.DeliveryVendorCreateModel;
    tempDeliveryVendor: Service.DeliveryVendorViewModel;

    searchFilter = {};

    tempDeliveryVendorCode: string;
    tempName: string;
    tempAddress: string;
    tempLocation: Service.DeliveryVendorLocationViewModel[];
    tempSapCode: string;
    tempAccount: string;

    dropDownLocation: Service.DeliveryVendorLocationViewModel[];

    success: boolean;
    isError: number;

    regexCode: RegExp = /^[a-zA-Z0-9]+$/;
    regexName: RegExp = /^[a-zA-Z0-9\s\-.,&\'\/]+$/;

    // Paging Sorting Searching upload downlod
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;
    $rootScope: angular.IRootScopeService;

    jsonLogisticVendor: {};
    validationFlag: boolean = true;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    }



    constructor(LogisticVendor: Service.LogisticVendorService, rootScope: angular.IRootScopeService) {
        this.searchFilter = {};
        this.LogisticVendor = LogisticVendor;
        this.showUpdate = false;

        this.$rootScope = rootScope;
    }

    $onInit() {
        this.deliveryVendorCreateModel = new Service.DeliveryVendorCreateModel;
        this.jsonLogisticVendor = {};
        this.refreshData();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
    }

    //validateVendorCode() {

    //    if (this.deliveryVendorCreateModel.deliveryVendorCode !== null) {
    //        this.tempDeliveryVendor = lodash.find(this.dataTable, {
    //            'deliveryVendorCode': this.deliveryVendorCreateModel.deliveryVendorCode
    //        });
    //    }

    //    if (this.deliveryVendorCreateModel.deliveryVendorCode === null || this.deliveryVendorCreateModel.deliveryVendorCode === undefined) {
    //        this.validationFlag = true;
    //    } else if (!this.deliveryVendorCreateModel.deliveryVendorCode.match(/^[\w\.\,\-\&\/\s]+$/)) {
    //        this.validationFlag = true;
    //    } else if (this.tempDeliveryVendor !== undefined) {
    //        this.validationFlag = false;
    //    } else {
    //        this.validationFlag = true;
    //    }
    //    console.log(this.validationFlag);
    //}

    reset() {
        this.showUpdate = false;
        this.validationFlag = true;
        this.deliveryVendorCreateModel = new Service.DeliveryVendorCreateModel;
        this.deliveryVendorCreateModel.account = null;
        this.deliveryVendorCreateModel.deliveryVendorCode = null;
        this.deliveryVendorCreateModel.address = null;
        this.deliveryVendorCreateModel.location = null;
        this.deliveryVendorCreateModel.name = null;
        this.deliveryVendorCreateModel.sapCode = null;
        this.searchFilter = {};

    }
    refreshData(Form?: angular.IFormController) {
        this.LogisticVendor.getAllData().then(response => {
            this.dataTable = response.data;
            this.totalItems = this.dataTable.length;
        });
        this.LogisticVendor.getAllLocation().then(response => {
            this.dropDownLocation = response.data;
        });
        if (Form) {
            Form.$setPristine();
            Form.$setUntouched();
        }
        this.reset();
    }

    selectEdit(data) {
        this.showUpdate = true;
        this.getAllSelectedData(data);
    }

    selectDelete(data, Form: angular.IFormController) {
        this.getAllSelectedDeleteData(data);
        this.deleteConfirmation('hapus', Form);
    }

    getAllSelectedData(data) {
        this.deliveryVendorCreateModel.location = new Service.DeliveryVendorLocationViewModel;
        this.deliveryVendorCreateModel.deliveryVendorCode = data["deliveryVendorCode"];
        this.deliveryVendorCreateModel.name = data["name"];
        this.deliveryVendorCreateModel.address = data["address"];
        this.deliveryVendorCreateModel.sapCode = data["sapCode"];
        this.deliveryVendorCreateModel.account = data["account"];
        let temp = lodash.find(this.dropDownLocation, ["locationCode", data["locationCode"]]);
        this.deliveryVendorCreateModel.location.locationCode = temp["locationCode"];
        this.deliveryVendorCreateModel.location.name = temp["name"];
    }
    getAllSelectedDeleteData(data) {
        this.tempLocation = new Array<Service.DeliveryVendorLocationViewModel>();
        this.tempDeliveryVendorCode = data["deliveryVendorCode"];
        this.tempName = data["name"];
        this.tempAddress = data["address"];
        this.tempSapCode = data["sapCode"];
        this.tempAccount = data["account"];
        let temp = lodash.find(this.dropDownLocation, ["locationCode", data["locationCode"]]);
        this.tempLocation["locationCode"] = temp["locationCode"];
        this.tempLocation["name"] = temp["name"];
    }

    tempCode: any;
    confirmationData(type) {
        let data = {
            deliveryVendorCode: this.deliveryVendorCreateModel.deliveryVendorCode,
            name: this.deliveryVendorCreateModel.name,
            address: this.deliveryVendorCreateModel.address === null ? '\xa0' : this.deliveryVendorCreateModel.address,
            sapCode: this.deliveryVendorCreateModel.sapCode === null ? '\xa0' : this.deliveryVendorCreateModel.sapCode,
            account: this.deliveryVendorCreateModel.account === null ? '\xa0' : this.deliveryVendorCreateModel.account,
            locationCode: this.deliveryVendorCreateModel.location['locationCode'] + " - " + this.deliveryVendorCreateModel.location['name']
        };
        return data;
    }
    confirmationDeleteData(type) {
        let data = {
            deliveryVendorCode: this.tempDeliveryVendorCode,
            name: this.tempName,
            address: this.tempAddress === null ? '\xa0' : this.tempAddress,
            sapCode: this.tempSapCode === null ? '\xa0' : this.tempSapCode,
            account: this.tempAccount === null ? '\xa0' : this.tempAccount,
            locationCode: this.tempLocation['locationCode'] + " - " + this.tempLocation['name']
        };
        this.tempCode = this.tempDeliveryVendorCode;
        return data;
    }

    //type : 'save' , 'edit' , or 'delete'
    confirmation(type: string, Form: angular.IFormController) {
        this.jsonLogisticVendor["Kode Vendor"] = this.deliveryVendorCreateModel.deliveryVendorCode;
        this.jsonLogisticVendor["Vendor"] = this.deliveryVendorCreateModel.name;
        this.jsonLogisticVendor["Alamat Vendor"] = this.deliveryVendorCreateModel.address;
        this.jsonLogisticVendor["Kode Vendor SAP"] = this.deliveryVendorCreateModel.sapCode;
        this.jsonLogisticVendor["Vendor Account"] = this.deliveryVendorCreateModel.account;
        this.jsonLogisticVendor["Kode Lokasi"] = this.deliveryVendorCreateModel.location['locationCode'] + " - " + this.deliveryVendorCreateModel.location['name'];

        if (this.deliveryVendorCreateModel.deliveryVendorCode !== null) {
            this.tempDeliveryVendor = lodash.find(this.dataTable, {
                'deliveryVendorCode': this.deliveryVendorCreateModel.deliveryVendorCode
            });
        }

        if (this.deliveryVendorCreateModel.deliveryVendorCode === null || this.deliveryVendorCreateModel.deliveryVendorCode === undefined) {
            this.validationFlag = true;
        } else if (!this.deliveryVendorCreateModel.deliveryVendorCode.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validationFlag = true;
        } else if (this.tempDeliveryVendor !== undefined) {
            this.validationFlag = false;
        } else {
            this.validationFlag = true;
        }

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON(type, this.jsonLogisticVendor)),
                () => {
                    if (type === 'simpan') {
                        this.createData(Form);
                    }
                    else if (type === 'ubah') {
                        this.updateData(Form);
                    }
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }
    deleteConfirmation(type: string, Form: angular.IFormController) {
        this.jsonLogisticVendor["Kode Vendor"] = this.tempDeliveryVendorCode;
        this.jsonLogisticVendor["Vendor"] = this.tempName;
        this.jsonLogisticVendor["Alamat Vendor"] = this.tempAddress;
        this.jsonLogisticVendor["Kode Vendor SAP"] = this.tempSapCode;
        this.jsonLogisticVendor["Vendor Account"] = this.tempAccount;
        this.jsonLogisticVendor["Kode Lokasi"] = this.tempLocation['locationCode'] + " - " + this.tempLocation['name'];
        this.tempCode = this.tempDeliveryVendorCode;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON(type, this.jsonLogisticVendor)),
            () => {
                this.deleteData(Form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    createData(Form: angular.IFormController) {
        this.LogisticVendor.createData(this.deliveryVendorCreateModel).then(response => {
            this.isError = response.data;

            if (this.isError == 1) {
                Alertify.error("Data gagal disimpan");
            } else if (this.isError == 2) {
                Alertify.error('Kode Vendor telah terdaftar');
            } else {
                this.refreshData(Form);
                Alertify.success('Data berhasil disimpan');
            }
        });
    }

    updateData(Form: angular.IFormController) {
        this.LogisticVendor.updateData(this.deliveryVendorCreateModel).then(response => {
            

            this.isError = response.data;

            if (this.isError == 1) {
                Alertify.error("Data gagal disimpan");
            } else {
                this.refreshData(Form);
                Alertify.success('Data berhasil disimpan');
            }
        });
    }

    deleteData(Form: angular.IFormController) {
        this.LogisticVendor.deleteData(this.tempCode).then(response => {
            this.isError = response.data;
            if (this.isError == 1) {
                Alertify.error("Data gagal dihapus");
            } else {
                this.refreshData(Form);
                Alertify.success('Data berhasil dihapus');
            }
        }).catch(() => {
            Alertify.error("Data gagal dihapus");
        });
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
    Download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.deliveryVendorCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "deliveryVendor";
        info.title = "Logistic Vendor";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }
    Upload() {

        let info: any = {};
        info.master = "deliveryVendor";
        info.title = "Logistic Vendor";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }


}
export class LogisticVendorComponent implements angular.IComponentOptions {
    controller = LogisticVendorController;
    controllerAs = 'me';

    template = require('./LogisticVendor.html');
}

