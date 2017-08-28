import * as angular from 'angular';
import * as Service from '../Services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as Mustache from 'mustache';

export class RegionController implements angular.IController {
    static $inject = ['RegionService', '$rootScope'];

    RegionService: Service.RegionService;
    buttonState: number = 0;

    $rootScope: angular.IRootScopeService;

    data: any;
    delId: string;
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    region: RegionInsert;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;
    searchString: Object = {};

    loader: boolean = true;
    btnDisabled: boolean = false;

    isUpdate: boolean = false;

    constructor(RegionService: Service.RegionService, rootScope: angular.IRootScopeService) {
        this.RegionService = RegionService;
        this.region = new RegionInsert();
        this.$rootScope = rootScope;
    }

    $onInit() {
        this.refresh();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refresh();
        });
    }

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.regionCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Region";
        info.title = "Region";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            // angular's default (non-strict) internal comparator
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };

    setPage(pageNo) {
        this.currentPage = pageNo;
    };

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);

    }
    refresh() {
        this.RegionService.getData().then(response => {
            this.data = response.data;
            this.loader = false;
        });
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }
    btnDisableCheck(form: angular.IFormController) {
        return form.$invalid || this.btnDisabled;
    }
    clear(form: angular.IFormController) {
        this.isUpdate = false;
        this.region.regionCode = null;
        this.region.parentCode = null;
        this.region.name = null;
        this.region.regionType = null;
        this.region.postCode = null;
        this.buttonState = 0;
        this.searchString = {};
        form.$setPristine();
        form.$setUntouched();
    }

    /**
     * untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
     * @param action insert,update,delete (salah satu) *case insensitive
     * @param json json data -> { label : value , label2 : value2 }
     */
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

    getRegionName(regionCode) {
        let region: any = _.find(this.data, ['regionCode', regionCode]);
        return region.name;
    }

    selectUpdate(data) {
        this.isUpdate = true;
        this.buttonState = 1;
        this.region.regionCode = data.regionCode
        this.region.name = data.name;
        this.region.regionType = data.type;
        this.region.parentCode = _.find(this.data, ['regionCode', data.parentRegionCode]);
        this.region.postCode = data.postCode;
    }

    selectDelete(data) {
        this.delId = data;
    }

    updateData(form: angular.IFormController) {
        this.RegionService.updateRegion(this.region).then(response => {
            this.refresh();
            this.clear(form);
            this.btnDisabled = false;
            alertify.success(response.data);
        }).catch(
            response => {
                this.btnDisabled = false;
                if (response.status == "400") {
                    alertify.error(response.data);
                }
                if (response.status == "500") {
                    alertify.error("Koneksi ke server bermasalah");
                }
            });
    }

    updateRegion(form: angular.IFormController) {
        let json = {};
        json["Kode Region"] = this.region.regionCode;
        json["Tipe Region"] = this.region.regionType;
        json["Region"] = this.region.name;
        json["Parent Region"] = this.region.parentCode ? this.region.parentCode.regionCode + ' - ' + this.region.parentCode.name : null;
        json["Kode Pos"] = this.region.postCode;

        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", json)),
            () => {
                this.btnDisabled = true;
                this.updateData(form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
}

export class RegionInsert {
    regionCode: string = null;
    regionType: string = null;
    name: string = null;
    parentCode: any = null;
    postCode: string = null;
}


export class Region implements angular.IComponentOptions {
    controller = RegionController;
    controllerAs = 'me';

    template = require('./Region.html');

}