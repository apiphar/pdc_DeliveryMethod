import * as angular from 'angular';
import * as Service from '../Services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as Mustache from 'mustache';
import * as moment from 'moment';

export class PIOLineMasterController implements angular.IController{
    static $inject = ['PIOLineMasterService', '$rootScope'];

    PIOLineMasterService: Service.PIOLineMasterService;
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
    pioLineMaster: PIOLineMasterInsert;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;

    loader: boolean = true;
    btnDisabled: boolean = false;
    constructor(PIOLineMasterService: Service.PIOLineMasterService, rootScope: angular.IRootScopeService) {
        this.PIOLineMasterService = PIOLineMasterService;
        this.pioLineMaster = new PIOLineMasterInsert();
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
     * Delete PIO from the database
     * @param data
     * @param form
     */
    deletePIO(data,form:angular.IFormController) {
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
                this.deleteData(data.pioLineDictionaryId,form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.pioLineDictionaryId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "PIOLineDictionary";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "PIOLineDictionary";
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
        this.PIOLineMasterService.getData().then((response:any) => {
            this.data = response.data.pioLineMaster;
            angular.forEach(this.data, (data) => {
                data.locationString = data.location.locationCode + ' - ' + data.location.name;
                data.taktSecondsString = this.convertToHMSToString(data.taktSeconds);
            });
            this.location = response.data.location;
            this.loader = false;
        });
    }
    btnDisableCheck(form: angular.IFormController) {
        return form.$invalid || this.btnDisabled;
    }
    order(orderString:string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    clear(form:angular.IFormController) {
        //field
        this.pioLineMaster = new PIOLineMasterInsert();
        this.jam = 0; this.menit = 0; this.detik = 0;
        this.buttonState = 0;
        form.$setPristine();
        form.$setUntouched();
    }

    saveData(form:angular.IFormController) {
        this.pioLineMaster.taktSeconds = (this.jam * 3600) + (this.menit * 60) + this.detik;
        this.PIOLineMasterService.postData(this.pioLineMaster).then(
            response => {
                this.refresh();
                this.clear(form);
                this.btnDisabled = false;
                alertify.success(response.data);
            }).catch(
            response => {
                this.btnDisabled = false;
                if (response.status == "400") {
                    if (typeof (response.data) == "object") {
                        alertify.error(response.data[0]);
                    } else
                        alertify.error(response.data);
                    return;
                }
                alertify.error("Internal Server Error");
            });
    }
    /**
     * fungsi untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
     * @param action insert,update,delete (salah satu) *case insensitive
     * @param json json data -> { label : value , label2 : value2 }
     */
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
    getPIOLineMasterName(PIOLineMasterCode) {
        let PIOLineMaster:any = _.find(this.data, ['PIOLineMasterCode', PIOLineMasterCode]);
        return PIOLineMaster.name;
    }
    postData(form:angular.IFormController) {
        let json = {};
        json["Lokasi"] = this.pioLineMaster.location.locationCode + " - " + this.pioLineMaster.location.name;
        json["Line"] = this.pioLineMaster.lineNumber;
        json["TAKT time"] = this.jam + " Jam " + this.menit + " Menit " + this.detik + " Detik";
        json["Jumlah Pos"] = this.pioLineMaster.post;

        
        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                if (_.find(this.data, (data: any) => {
                    return <any>data.lineNumber.toLowerCase() == this.pioLineMaster.lineNumber.toLowerCase();
                })) {
                    alertify.error("Line sudah terdaftar");
                    return;
                }
                this.btnDisabled = true;
                this.saveData(form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
    
    deleteData(id,form:angular.IFormController) {
        this.PIOLineMasterService.deletePIOLineMaster(id).then(response => {
            this.refresh();
            this.btnDisabled = false;
            this.clear(form);
            alertify.success(response.data);
        }).catch(
            response => {
                this.btnDisabled = false;
                if (response.status == "400") {
                    if (typeof (response.data) == "object") {
                        alertify.error(response.data[0]);
                    } else
                        alertify.error(response.data);
                    return;
                }
                alertify.error("Internal Server Error");
            });
        
    }
    convertToHMSToString(seconds: number): string {
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
        this.pioLineMaster.pioLineDictionaryId = data.pioLineDictionaryId;
        this.convertToHMS(data.taktSeconds);
        this.pioLineMaster.location = data.location;
        this.pioLineMaster.lineNumber = data.lineNumber;
        this.pioLineMaster.post = data.post;
        this.buttonState = 1;
    }

    selectDelete(data) {
        this.delId = data;
    } 

    updateData(form: angular.IFormController) {
        this.pioLineMaster.taktSeconds = (this.jam * 3600) + (this.menit * 60) + this.detik;
        this.PIOLineMasterService.updatePIOLineMaster(this.pioLineMaster).then(response => {
            this.refresh();
            this.clear(form);
            this.btnDisabled = false;
            alertify.success(response.data);
        }).catch(
            response => {
                this.btnDisabled = false;
                if (response.status == "400") {
                    if (typeof (response.data) == "object") {
                        alertify.error(response.data[0]);
                    } else
                        alertify.error(response.data);
                    return;
                }
                alertify.error("Internal Server Error");
            });
    }

    updatePIOLineMaster(form:angular.IFormController) {
        let json = {};
        json["Lokasi"] = this.pioLineMaster.location.locationCode + " - " + this.pioLineMaster.location.name;
        json["Line"] = this.pioLineMaster.lineNumber;
        json["TAKT time"] = this.jam + " Jam "+this.menit+" Menit "+this.detik+" Detik";
        json["Jumlah Pos"] = this.pioLineMaster.post;
        
        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", json)),
            () => {
                if (_.find(this.data, (row: any) => {
                    return row.lineNumber == this.pioLineMaster.lineNumber && row.pioLineDictionaryId != this.pioLineMaster.pioLineDictionaryId;
                })) {
                    alertify.error("Line sudah terdaftar");
                    return;
                }
                this.btnDisabled = true;
                this.updateData(form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
}

export class PIOLineMasterInsert{
    pioLineDictionaryId: number;
    location: any = null;
    lineNumber: string = null;
    taktSeconds: number = null;
    post: number = null;
}


export class PIOLineMaster implements angular.IComponentOptions {
    controller = PIOLineMasterController;
    controllerAs = 'me';

    template = require('./PIOLineMaster.html');

}