import * as angular from 'angular';
import * as Service from '../Services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as Mustache from 'mustache';
import * as moment from 'moment';

export class SPULineMasterController implements angular.IController{
    static $inject = ['SPULineMasterService', '$rootScope'];

    SPULineMasterService: Service.SPULineMasterService;
    buttonState: number = 0;
    jam: number;
    menit: number;
    detik: number;
    location: any;
    $rootScope: angular.IRootScopeService;

    data: any;
    delId: string;
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20,25];
    pageSize: number = 5;
    spuLineMaster: SPULineMasterInsert;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;

    loader: boolean = true;
    btnDisabled: boolean = false;
    constructor(SPULineMasterService: Service.SPULineMasterService, rootScope: angular.IRootScopeService) {
        this.SPULineMasterService = SPULineMasterService;
        this.spuLineMaster = new SPULineMasterInsert();
        this.$rootScope = rootScope;
    }

    $onInit() {
        this.refresh();
        this.jam = 0; this.menit=0; this.detik = 0;
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refresh();
        });
    }
   /**
    * Delete SPU from the database
    * @param data
    * @param form
    */
    deleteSPU(data,form:angular.IFormController) {
        let json = {};
        json["Lokasi"] = data.locationString;
        json["Line"] = data.lineNumber;
        json["TAKT time"] = data.taktSecondsString;
        json["Jumlah Pos"] = data.post;

        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", json)),
            () => {
                this.btnDisabled = true;
                this.deleteData(data.spuLineId,form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.spuLineId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "SPULineDictionary";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "SPULineDictionary";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
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
        this.setPage(1);
    }
    refresh() {
        this.SPULineMasterService.getData().then((response:any) => {
            this.data = response.data.spuLineMaster;
            angular.forEach(this.data, (data) => {
                data.locationString = data.location.locationCode + ' - ' + data.location.name;
                data.taktSecondsString = this.convertToHMSToString(data.taktSeconds);
            });
            this.location = response.data.location;
            this.loader = false;
        });
    }

    order(orderString:string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }
    btnDisableCheck(form: angular.IFormController) {
        return form.$invalid ||this.btnDisabled;
    }
    clear(form:angular.IFormController) {
        //field
        this.spuLineMaster = new SPULineMasterInsert();
        this.jam = 0; this.menit = 0; this.detik = 0;
        this.buttonState = 0;
        form.$setPristine();
        form.$setUntouched();
    }

    saveData(form:angular.IFormController) {
        this.spuLineMaster.taktSeconds = (this.jam * 3600) + (this.menit * 60) + this.detik;
        this.SPULineMasterService.postData(this.spuLineMaster).then(
            response => {
                this.refresh();
                this.clear(form);
                this.btnDisabled = false;
                alertify.success(response.data);
            }).catch(
            response => {
                this.btnDisabled = false;
                if (response.status == "400") {
                    alertify.error(response.data);
                    return;
                }

                if (response.status == "500") {
                    alertify.error("Koneksi ke server bermasalah");
                }
            });
    }
    /**
     * fungsi untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
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

    getSPULineMasterName(SPULineMasterCode) {
        let SPULineMaster:any = _.find(this.data, ['SPULineMasterCode', SPULineMasterCode]);
        return SPULineMaster.name;
    }
    postData(form:angular.IFormController) {
        let json = {};
        json["Lokasi"] = this.spuLineMaster.location?this.spuLineMaster.location.locationCode + " - " + this.spuLineMaster.location.name:'';
        json["Line"] = this.spuLineMaster.lineNumber;
        json["TAKT time"] = this.jam + " Jam " + this.menit + " Menit " + this.detik + " Detik";
        json["Jumlah Pos"] = this.spuLineMaster.post;
        
        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                if (_.find(this.data, (data: any) => {
                    return <any>data.lineNumber.toLowerCase() == this.spuLineMaster.lineNumber.toLowerCase() && <any>data.location.locationCode.toLowerCase() == this.spuLineMaster.location.locationCode.toLowerCase();
                })) {
                    alertify.error("Lokasi dan Line sudah terdaftar");
                    return;
                }
                this.btnDisabled = true;
                this.saveData(form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
    
    deleteData(id,form:angular.IFormController) {
        this.SPULineMasterService.deleteSPULineMaster(id).then(response => {
            this.btnDisabled = false;
            this.refresh();
            this.clear(form);
            alertify.success(response.data);
        }).catch(
            response => {
                this.btnDisabled = false;
                if (response.status == "400") {
                    alertify.error(response.data);
                    return;
                }
                if (response.status == "500") {
                    alertify.error("Koneksi ke server bermasalah");
                }
            });
        
    }
    convertToHMSToString(seconds: number) : string{
        let detik: number = seconds % 60;
        let tempMenit: number = (seconds - detik) / 60;
        let menit: number = tempMenit % 60;
        let jam: number = (tempMenit - menit) / 60;
        return jam.toString() + ' Jam ' + menit.toString() + ' Menit ' + detik.toString() + ' Detik'; 
    }
    convertToHMS(seconds: number) {
        let detik: number = seconds % 60;
        let tempMenit: number = (seconds - detik) / 60;
        let menit: number = tempMenit % 60;
        let jam: number = (tempMenit - menit) / 60;
        this.detik = detik;
        this.menit = menit;
        this.jam = jam;
    }

    selectUpdate(data) {
        this.spuLineMaster.spuLineId = data.spuLineId;
        this.convertToHMS(data.taktSeconds);
        this.spuLineMaster.location = data.location;
        this.spuLineMaster.lineNumber = data.lineNumber;
        this.spuLineMaster.post = data.post;
        this.buttonState = 1;
    }

    selectDelete(data) {
        this.delId = data;
    } 

    updateData(form: angular.IFormController) {
        this.spuLineMaster.taktSeconds = (this.jam * 3600) + (this.menit * 60) + this.detik;
        this.SPULineMasterService.updateSPULineMaster(this.spuLineMaster).then(response => {
            this.refresh();
            this.clear(form);
            this.btnDisabled = false;
            alertify.success(response.data);
        }).catch(response => {
            this.btnDisabled = false;
            if (response.status == "400") {
                alertify.error(response.data);
                return;
            }
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    updateSPULineMaster(form:angular.IFormController) {
        let json = {};
        json["Lokasi"] = this.spuLineMaster.location ? this.spuLineMaster.location.locationCode + " - " + this.spuLineMaster.location.name : '';
        json["Line"] = this.spuLineMaster.lineNumber;
        json["TAKT time"] = this.jam + " Jam " + this.menit + " Menit " + this.detik + " Detik" ;
        json["Jumlah Pos"] = this.spuLineMaster.post;
        
        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", json)),
            () => {
                if (_.find(this.data, (row: any) => {
                    return row.lineNumber == this.spuLineMaster.lineNumber && row.spuLineId != this.spuLineMaster.spuLineId && row.location.locationCode == this.spuLineMaster.location.locationCode;
                })) {
                    alertify.error("Lokasi dan Line sudah terdaftar");
                    return;
                }
                this.btnDisabled = true;
                this.updateData(form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
}

export class SPULineMasterInsert{
    spuLineId: number;
    location: any = null;
    lineNumber: string = null;
    taktSeconds: number = null;
    post: number = null;
}


export class SPULineMaster implements angular.IComponentOptions {
    controller = SPULineMasterController;
    controllerAs = 'me';

    template = require('./SPULineMaster.html');

}