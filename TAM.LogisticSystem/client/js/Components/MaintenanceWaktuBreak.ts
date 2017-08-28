import * as angular from 'angular';
import * as Service from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as _ from 'lodash';

export class MaintenanceWaktuBreakController implements angular.IController {
    static $inject = ['MaintenanceWaktuBreakService', '$rootScope'];

    $rootScope: angular.IRootScopeService;
    //declare IFormController
    MyForm: angular.IFormController;

    data: any;
    dataLocation: any;
    dataShift: any;

    idleTimeCustomId: number;
    dateFrom: Date
    dateTo: Date;
    hourStep = 1;
    minuteStep = 1;

    locations: any; 
    shifts: any;

    created: boolean;
    edited: boolean;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;
    loader: boolean = true;

    jsonMaintenanceWaktuBreak = {
        "Location": null,
        "Shift": null,
        "Mulai Break": null,
        "Selesai Break": null,
    }

    MaintenanceWaktuBreak: Service.MaintenanceWaktuBreakService;

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

    constructor(maintenanceWaktuBreak: Service.MaintenanceWaktuBreakService, rootScope: angular.IRootScopeService) {
        this.MaintenanceWaktuBreak = maintenanceWaktuBreak;
        this.$rootScope = rootScope;      
    }

    dateOption = {
        minDate: new Date()
    }

    dateChange(MyForm: angular.IFormController) {
        this.dateOption.minDate = moment(this.dateFrom).add(0, 'minutes').toDate();
        MyForm.$setUntouched;
    }

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.idleTimeCustomId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "idleTimeCustom";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "idleTimeCustom";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

    //set current page
    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    refreshData() {
        this.MaintenanceWaktuBreak.getMaintenanceWaktuBreak().then(response => {
            this.data = response.data;
        });

        this.MaintenanceWaktuBreak.getLocation().then(response => {
            this.dataLocation = response.data;
        });

        this.MaintenanceWaktuBreak.getShift().then(response => {
            this.dataShift = response.data;
        });

        this.dateFrom = null;
        this.dateTo = null;
        this.loader = false;
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    clearForm(MyForm: angular.IFormController) {
        this.locations = undefined;
        this.shifts = undefined;
        this.dateFrom = null;
        this.dateTo = null;
        this.edited = false;
        this.created = true;
        MyForm.$setPristine();
    }

    $onInit() {
        this.refreshData();
        this.edited = false;
        this.created = true;
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        })
    }

    selectEdit(data) {
        this.getAllSelectedData(data);
        this.edited = true;
        this.created = false;
    }

    selectDelete(data, MyForm: angular.IFormController) {
        this.getAllSelectedData(data);
    }

    getAllSelectedData(data) {
        this.idleTimeCustomId = data.idleTimeCustomId
        this.locations = _.find(this.dataLocation, ['locationCode', data.locationCode]);
        this.locations = _.find(this.dataLocation, ['name', data.name]);
        this.shifts = _.find(this.dataShift, ['shiftCode', data.shiftCode]);
        this.shifts = _.find(this.dataShift, ['description', data.description]);

        var dateFrom_ = new Date(data["dateFrom"]);
        this.dateFrom = moment(new Date(dateFrom_)).toDate();

        var dateTo_ = new Date(data["dateTo"]);
        this.dateTo = moment(new Date(dateTo_)).toDate();
    }

    // insert to database
    addData(Form: angular.IFormController) {        
        this.jsonMaintenanceWaktuBreak["Mulai Break"] = moment(this.dateFrom).format('YYYY/MM/D H:mm');
        this.jsonMaintenanceWaktuBreak["Selesai Break"] = moment(this.dateTo).format('YYYY/MM/D H:mm');

        let check = true
        if (this.locations == undefined) {
            alertify.error("Lokasi harus dipilih");
            check = false;
        } else {
            this.jsonMaintenanceWaktuBreak["Location"] = this.locations["locationCode"] + ' - ' + this.locations["name"];
        }
        if (this.shifts == undefined) {
            alertify.error("Shift harus dipilih");
            check = false;
        } else {
            this.jsonMaintenanceWaktuBreak["Shift"] = this.shifts["shiftCode"] + ' - ' + this.shifts["description"];
        }
        if (this.dateFrom > this.dateTo) {
            alertify.error("Tanggal & Jam Mulai Break tidak boleh lebih besar dari Selesai Break");
            check = false;
        }

        if(check)
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonMaintenanceWaktuBreak)),
            () => {
                this.MaintenanceWaktuBreak.postData(this.locations["locationCode"], this.shifts["shiftCode"], this.dateFrom, this.dateTo).then(response => {
                    alertify.success("Data tersimpan");
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Error ketika menyimpan data");
                    } else {
                        alertify.error("Gagal menyimpan");
                    }
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.clearForm(Form);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(Form: angular.IFormController) {     
        this.jsonMaintenanceWaktuBreak["Mulai Break"] = moment(this.dateFrom).format('YYYY/MM/D H:mm');
        this.jsonMaintenanceWaktuBreak["Selesai Break"] = moment(this.dateTo).format('YYYY/MM/D H:mm');

        let check = true
        if (this.locations == undefined) {
            alertify.error("Lokasi harus dipilih");
            check = false;
        } else {
            this.jsonMaintenanceWaktuBreak["Location"] = this.locations["locationCode"] + ' - ' + this.locations["name"];
        }
        if (this.shifts == undefined ) {
            alertify.error("Shift harus dipilih");
            check = false;
        } else {
            this.jsonMaintenanceWaktuBreak["Shift"] = this.shifts["shiftCode"] + ' - ' + this.shifts["description"];
        }
        if (this.dateFrom > this.dateTo) {
            alertify.error("Tanggal & Jam Mulai Break tidak boleh lebih besar dari Selesai Break");
            check = false;
        }

        if (check)
            alertify.confirm(
                "Konfirmasi",
                mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonMaintenanceWaktuBreak)),
                () => {
                    this.MaintenanceWaktuBreak.updateData(this.idleTimeCustomId, this.locations["locationCode"], this.shifts["shiftCode"], this.dateFrom, this.dateTo).then(response => {
                        alertify.success("Data berhasil disimpan");
                    }).catch(response => {
                        if (response.status == "500") {
                            alertify.error("Error ketika menyimpan data");
                        } else {
                            alertify.error("Gagal menyimpan");
                        }
                    }).finally(() => {
                        this.refreshData();
                        this.setPage(this.currentPage);
                        this.clearForm(Form);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(data, Form: angular.IFormController) {
        this.idleTimeCustomId = data.idleTimeCustomId

        this.jsonMaintenanceWaktuBreak["Location"] = data.locationCode + ' - ' + data.name;
        this.jsonMaintenanceWaktuBreak["Shift"] = data.shiftCode + ' - ' + data.description;
        this.jsonMaintenanceWaktuBreak["Mulai Break"] = moment(data.dateFrom).format('YYYY/MM/D H:mm');
        this.jsonMaintenanceWaktuBreak["Selesai Break"] = moment(data.dateTo).format('YYYY/MM/D H:mm');

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonMaintenanceWaktuBreak)),
            () => {
                this.MaintenanceWaktuBreak.deleteData(this.idleTimeCustomId).then(response => {
                    alertify.success("Data berhasil dihapus");
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Error ketika menghapus data");
                    } else {
                        alertify.error("Gagal menghapus data");
                    }
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.clearForm(Form);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
}

export class MaintenanceWaktuBreakComponent implements angular.IComponentOptions {
    controller = MaintenanceWaktuBreakController;
    controllerAs = 'me';

    template = require('./MaintenanceWaktuBreak.html');
    bindings = {
        greet: '@'
    };
}