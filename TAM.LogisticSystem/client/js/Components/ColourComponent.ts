import * as Service from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as angular from 'angular';
import * as lodash from 'lodash';

export class ColourController implements angular.IController {
    static $inject = ['ColourService','$rootScope'];

    colour: Service.ColourService;
    dataTable: any;
    dropDownColorType: JSON;
    showUpdate: boolean;

    colorCode: string;
    $rootScope: angular.IRootScopeService;
    colorType: string;
    indonesianName: string;
    englishName: string;
    

    pageState: boolean = true;
    isLoading: boolean = true;
    searchString = {};
    jsonModel = {};
    //PAGING
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
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

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    constructor(colour: Service.ColourService, rootScope: angular.IRootScopeService) {
        this.colour = colour;
        this.showUpdate = false;
        this.$rootScope = rootScope;
    }

    getData() {
        this.colour.getAllData().then(response => {
            this.dataTable = response.data;
            this.totalItems = this.dataTable.length;
            this.isLoading = false;
        });
    }
    $onInit() {
        this.getData();
        this.colorType = "Interior";
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.getData();
        });
    }

    cancel(form: angular.IFormController) {
        this.colorCode = null;
        this.indonesianName = null;
        this.englishName = null;
        form.$setPristine();
        form.$setUntouched();
        this.showUpdate = false;
        this.searchString = {};
        this.colorType = "Interior";
    }
    bindUpdate(data) {
        this.colorCode = data["colorCode"];
        this.colorType = data["colorType"];
        this.indonesianName = data["indonesianName"];
        this.englishName = data["englishName"];
        this.showUpdate = true;
    }

    //update the data
    update(form: angular.IFormController) {
        
        this.colorCode = this.colorCode.toUpperCase();
        this.englishName = this.englishName.toUpperCase();
        this.indonesianName = this.indonesianName.toUpperCase();
        this.jsonModel["Kode Warna"] = this.colorCode;
        this.jsonModel["Tipe Warna"] = this.colorType.substring(0, 3).toUpperCase();
        this.jsonModel["Deskripsi Warna(Ind)"] = this.indonesianName;
        this.jsonModel["Deskripsi Warna(Eng)"] = this.englishName;

        alertify.confirm("Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonModel)),
            Q => {
                this.dataTable = null;
                this.isLoading = true;
                this.colour.updateData(this.colorCode, this.colorType, this.indonesianName, this.englishName).then(response => {
                    this.getData();
                    this.cancel(form);
                    alertify.success("Data berhasil disimpan");
                }).catch(response => {
                    if (response.status == "400") {
                        alertify.error(response.data);
                        this.getData();
                        return;
                    }
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                        this.getData();
                        return;
                    }
                    
                });
            },
            () => {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
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

        result = lodash.filter(result, ['colorType', this.colorType]);
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.colorCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = this.colorType + "color";
        info.title = this.colorType+" Color";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }
    



}
export class ColourComponent implements angular.IComponentOptions {
    controller = ColourController;
    controllerAs = 'me';

    template = require('./Colour.html');
}