import * as angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';

export class LogisticVehicleController implements angular.IController
{
    static $inject = ['LogisticVehicleService', '$rootScope'];

    logisticVehicle: Service.LogisticVehicleService;
    $rootScope: angular.IRootScopeService;
    logisticVehicleModel: LogisticVehicleModel;

    dataDeliveryMethod: any;  
    dataDeliveryMethodType;
      
    showHideButton: boolean = false;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;
    Search: any = {};
    needSJKBValidationText: string;

    loader: boolean = true;
    constructor(logisticVehicle: Service.LogisticVehicleService, rootScope: angular.IRootScopeService) {
        this.logisticVehicle = logisticVehicle;
        this.$rootScope = rootScope;
        this.showHideButton = false;
        this.logisticVehicleModel = new LogisticVehicleModel();
    }

    Download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {  
            tempData.push(data.deliveryMethodCode);  
        }); 
        this.pageState = false;
        let info: any = {};
        info.master = "DeliveryMethod";
        info.title = "Delivery Method";  
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    Upload() {
        let info: any = {};
        info.master = "DeliveryMethod";
        info.title = "Delivery Method";  
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }
    
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

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };

    reset(myForm: angular.IFormController) {
        this.showHideButton = false;
        this.logisticVehicleModel.deliveryMethodCode = null;
        this.logisticVehicleModel.name = null;
        this.logisticVehicleModel.deliveryMethodTypeId = undefined;     
        this.logisticVehicleModel.needSJKBValidation = undefined;
        this.Search = {};
        if (myForm) {
            myForm.$setPristine();
        }
    }

    refreshData(myForm?: angular.IFormController) {
        this.logisticVehicle.getDataDeliveryMethod().then(response => {
            this.dataDeliveryMethod = response.data;
            this.reset(myForm);
            this.loader = false;

            _.each(this.dataDeliveryMethod, (item) => {
                item.needSJKBValidationText = (item.needSJKBValidation ? 'Ya' : 'Tidak');
            });
        });
         
        this.logisticVehicle.getDataDeliveryMethodType().then(response => {
            this.dataDeliveryMethodType = response.data;            
        });
    }

    $onInit() {
        this.refreshData();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
    }

    selectEdit(data) {
        this.showHideButton = true;
        this.logisticVehicleModel.deliveryMethodCode = data.deliveryMethodCode;
        this.logisticVehicleModel.name = data.name;
        this.logisticVehicleModel.deliveryMethodTypeId = data.deliveryMethodTypeId;
        this.logisticVehicleModel.deliveryMethodTypeName = data.deliveryMethodTypeName;
        this.logisticVehicleModel.needSJKBValidation = data.needSJKBValidation;
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin untuk menambahkan data :";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data :";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data :";
        convertResult["grid"] = tempJson;
        return convertResult;
    }

    postVehicle(myForm: angular.IFormController) {
        let json = {};
        let moda = _.find(this.dataDeliveryMethodType, { deliveryMethodTypeId: this.logisticVehicleModel.deliveryMethodTypeId });
        json["Kode Moda"] = this.logisticVehicleModel.deliveryMethodCode;
        json["Deskripsi Moda"] = this.logisticVehicleModel.name;
        json["Tipe Moda"] = (moda as any).deliveryMethodTypeName;
        json["Menggunakan Validasi SJKB"] = this.logisticVehicleModel.needSJKBValidation ? "Ya" : "Tidak";

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                if (_.find(this.dataDeliveryMethod, (data: any) => {
                    return <any>data.deliveryMethodCode.toLocaleLowerCase() == this.logisticVehicleModel.deliveryMethodCode.toLocaleLowerCase();
                })) {
                    Alertify.error("Kode moda sudah terdaftar");
                    return;
                }
                this.createData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    createData(myForm: angular.IFormController) {
        this.logisticVehicle.postData(this.logisticVehicleModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Gagal menyimpan data");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data Berhasil disimpan");
                }
            }
        )
    }

    updateVehicle(myForm: angular.IFormController) {
        let json = {};
        let moda = _.find(this.dataDeliveryMethodType, { deliveryMethodTypeId: this.logisticVehicleModel.deliveryMethodTypeId });
        json["Kode Moda"] = this.logisticVehicleModel.deliveryMethodCode;
        json["Deskripsi Moda"] = this.logisticVehicleModel.name;
        json["Tipe Moda"] = (moda as any).deliveryMethodTypeName;
        json["Menggunakan Validasi SJKB"] = this.logisticVehicleModel.needSJKBValidation ? "Ya" : "Tidak";

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", json)),
            () => {
                this.updateData(myForm);
                myForm.$setPristine();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(myForm: angular.IFormController) {
        this.logisticVehicle.updateData(this.logisticVehicleModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Gagal mengubah data");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data Berhasil diubah");
                }
            }
        )
    }

    deleteVehicle(data, myForm: angular.IFormController) {
        this.reset(myForm);
        let json = {};
        json["Kode Moda"] = data.deliveryMethodCode;
        json["Deskripsi Moda"] = data.name;
        json["Tipe Moda"] = data.deliveryMethodTypeName;
        json["Menggunakan Validasi SJKB"] = data.needSJKBValidation ? "Ya" : "Tidak";

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON('delete', json)),
            () => {
                this.deleteData(data.deliveryMethodCode, myForm);
                myForm.$setPristine();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(id, myForm: angular.IFormController) {
        this.logisticVehicle.deleteData(id).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Gagal menghapus data");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data Berhasil dihapus");
                }
            }
        )
    }
}

export class LogisticVehicleModel {
    deliveryMethodCode: string;
    name: string;
    deliveryMethodTypeId: number;
    deliveryMethodTypeName: string;
    needSJKBValidation: boolean;
}

export interface DeliveryMethodTypeByData {
    deliveryMethodTypeId: number,
    name: string
    deliveryMethodTypeName: string;
}

export class LogisticVehicle implements angular.IComponentOptions {
    controller = LogisticVehicleController;
    controllerAs = 'me';

    template = require('./LogisticVehicle.html');
}