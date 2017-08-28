import * as Angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';

export class BrandController implements angular.IController {
    static $inject = ['BrandService', '$rootScope'];

    brand: Service.BrandService;
    $rootScope: angular.IRootScopeService;
    brandModel: BrandModel;

    dataBrand: any;
    jsonBrand = {};

    showHideButton: boolean;
    delId: string;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;
    Search: {};

    loader: boolean = true;
    constructor(brand: Service.BrandService, rootScope: angular.IRootScopeService) {
        this.brand = brand;
        this.brandModel = new BrandModel();
        this.$rootScope = rootScope;
        this.showHideButton = false;
    }

    Download(result: any) {
        let tempData = [];
        Angular.forEach(result, (data) => {
            tempData.push(data.brandCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "brand";
        info.title = "Brand";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    Upload() {
        let info: any = {};
        info.master = "brand";
        info.title = "Brand";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

    reset(myForm: angular.IFormController) {
        this.showHideButton = false;
        this.brandModel.brandCode = null;
        this.brandModel.name = null;
        this.Search = {};
        if (myForm) {
            myForm.$setPristine();
            myForm.$setUntouched();   
        }
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

    refreshData(myForm?: angular.IFormController) {
        this.brand.getDataBrand().then(response => {
            this.dataBrand = response.data;
            this.reset(myForm);
            this.loader = false;
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
        this.brandModel.brandCode = data.brandCode;
        this.brandModel.name = data.name;
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        Angular.forEach(json, (value, key) => {
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

    postBrand(myForm: angular.IFormController) {
        this.jsonBrand["Kode Brand"] = this.brandModel.brandCode;
        this.jsonBrand["Brand"] = this.brandModel.name;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonBrand)),
            () => {
                if (_.find(this.dataBrand, (data: any) => {
                    return <any>data.brandCode.toLocaleLowerCase() == this.brandModel.brandCode.toLocaleLowerCase();
                })) {
                    Alertify.error("Kode Brand telah terdaftar");
                    return;
                }
                this.createData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    createData(myForm: angular.IFormController) {
        this.brand.postData(this.brandModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal disimpan");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data berhasil disimpan");
                }
            }
        )
    }

    updateBrand(myForm: angular.IFormController) {
        this.jsonBrand["Kode Brand"] = this.brandModel.brandCode;
        this.jsonBrand["Brand"] = this.brandModel.name;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonBrand)),
            () => {
                this.updateData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(myForm: angular.IFormController) {
        this.brand.updateData(this.brandModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal disimpan");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data berhasil disimpan");
                }
            }
        )
    }

    deleteBrand(data, myForm: angular.IFormController) {
        this.jsonBrand["Kode Brand"] = data.brandCode;
        this.jsonBrand["Brand"] = data.name;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON('delete', this.jsonBrand)),
            () => {
                this.deleteData(data.brandCode, myForm);
                this.reset(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(id, myForm: angular.IFormController) {
        this.brand.deleteData(id).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal dihapus");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data berhasil dihapus");
                }
            }
        )
    }
}

export class BrandModel {
    brandCode: string;
    name: string;
}

let Brand = {
    controller: BrandController,
    controllerAs: "me",
    template: require("./Brand.html")
};

export { Brand };