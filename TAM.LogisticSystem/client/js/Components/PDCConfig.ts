import * as angular from 'angular';
import * as Service from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';

export class PDCConfigController implements angular.IController {
    static $inject = ['PDCConfigService', '$rootScope'];

    $rootScope: angular.IRootScopeService;
    //declare IFormController
    MyForm: angular.IFormController;

    data: any;
    dataPDC: any;
    locationCodes: any;

    pdcConfigId: number;
    maintenanceDay: number;
    carCarrierQuotaPerDay: number;
    nonCarCarrierQuotaPerDay: number;

    locationCodeMessage: boolean;

    leadDayPreDeliveryService: number;

    dayPreDelivery: number = 0;
    hoursPreDelivery: number = 0;
    minutePreDelivery: number = 0;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;

    created: boolean = true;
    edited: boolean = false;
    disablePDC: boolean = false;

    jsonPDCConfig = {
        "Location": null,
        "Periode Maintenance": null,
        "Unit Car Carrier": null,
        "Unit Non Car Carrier": null,
        "Lead Time Persiapan Pengiriman": null
    }

    PDCConfig: Service.PDCConfigService;

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

    constructor(pdcConfig: Service.PDCConfigService, rootScope: angular.IRootScopeService) {
        this.PDCConfig = pdcConfig;
        this.$rootScope = rootScope;
    }
    
    //set current page
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

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.pdcConfigId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "pdcConfig";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "pdcConfig";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

    refreshData() {
        this.locationCodeMessage = false;
        this.PDCConfig.getPDCconfig().then(response => {
            this.data = response.data;
        });

        this.PDCConfig.getPDC().then(response => {
            this.dataPDC = response.data;
        });
    }

    clearForm(Form: angular.IFormController) {
        this.locationCodes = undefined;
        this.maintenanceDay = null;
        this.carCarrierQuotaPerDay = null;
        this.nonCarCarrierQuotaPerDay = null;
        this.dayPreDelivery = 0;
        this.hoursPreDelivery = 0;
        this.minutePreDelivery = 0;
        this.locationCodeMessage = false;
        this.disablePDC = false;
        this.created = true;
        this.edited = false;
        Form.$setPristine();
    }

    clearFormForCancelDelete(Form: angular.IFormController) {
        this.locationCodes = undefined;
        this.maintenanceDay = null;
        this.carCarrierQuotaPerDay = null;
        this.nonCarCarrierQuotaPerDay = null;
        this.dayPreDelivery = 0;
        this.hoursPreDelivery = 0;
        this.minutePreDelivery = 0;
        this.locationCodeMessage = false;
        this.disablePDC = false;
        this.created = true;
        this.edited = false;
    }

    $onInit() {
        this.refreshData();
        this.$rootScope.$on("Kembali", (event)=>{
            this.pageState = true;
            this.refreshData();
        })
    }

    selectEdit(data) {
        this.getAllSelectedData(data);
        this.disablePDC = true;
        this.edited = true;
        this.created = false;
    }

    getAllSelectedData(data) {
        this.pdcConfigId = data.pdcConfigId
        this.locationCodes = _.find(this.dataPDC, ['locationCode', data.locationCode]);
        this.locationCodes = _.find(this.dataPDC, ['name', data.name]);
        this.maintenanceDay = data.maintenanceDay;
        this.carCarrierQuotaPerDay = data.carCarrierQuotaPerDay;
        this.nonCarCarrierQuotaPerDay = data.nonCarCarrierQuotaPerDay;;

        //Modulus For LeadMinutePreparation
        var minutes2 = data.leadDayPreDeliveryService % 60;
        var hours2 = ((data.leadDayPreDeliveryService - minutes2) / 60) % 24;
        var days2 = (((data.leadDayPreDeliveryService - minutes2) / 60) - (((data.leadDayPreDeliveryService - minutes2) / 60) % 24)) / 24;
        //Convert to days,Hours & Minute
        this.dayPreDelivery = days2; //day
        this.hoursPreDelivery = hours2; //Hours
        this.minutePreDelivery = minutes2; //Minutes
    }

    selectDelete(data, MyForm: angular.IFormController) {
        this.getAllSelectedData(data);
    }

    checkLocationCode() {
        this.PDCConfig.GetId(this.locationCodes["locationCode"]).then(response => {
            if (response.data["locationCode"] === undefined) {
                this.locationCodeMessage = false;
            }
            else {
                this.locationCodeMessage = true;
            }
        });
       
    }

    // insert to database
    addData(Form: angular.IFormController) {
        var hariToMenit = 1440;
        var jamToMenit = 60;
        var convertTime = (this.dayPreDelivery * hariToMenit) + (this.hoursPreDelivery * jamToMenit) + this.minutePreDelivery;
        this.leadDayPreDeliveryService = convertTime;

        let check = true
        if (this.locationCodeMessage == true ) {
            alertify.error("PDC sudah terdaftar");
            check = false;
        }
        if (this.locationCodes == undefined) {
            alertify.error("PDC harus dipilih");
            check = false;
        }
        else {
            this.checkLocationCode();
            this.jsonPDCConfig["Location"] = this.locationCodes["locationCode"] + ' - ' + this.locationCodes["name"];
            this.jsonPDCConfig["Periode Maintenance"] = this.maintenanceDay + ' Hari ';
            this.jsonPDCConfig["Unit Car Carrier"] = this.carCarrierQuotaPerDay + ' Unit ';
            this.jsonPDCConfig["Unit Non Car Carrier"] = this.nonCarCarrierQuotaPerDay + ' Unit ';
            this.jsonPDCConfig["Lead Time Persiapan Pengiriman"] = this.dayPreDelivery + ' Hari ' + this.hoursPreDelivery + ' Jam ' + this.minutePreDelivery + ' Menit ';
        }
        if (this.maintenanceDay > 99) {
            alertify.error("Durasi Maintenance tidak boleh melebihi dari 2 digit");
            check = false;
        }
        if (this.dayPreDelivery > 99) {
            alertify.error("Hari Lead Time tidak boleh melebihi dari 2 digit");
            check = false;
        }
        if (this.hoursPreDelivery > 23) {
            alertify.error("Jam Lead Time tidak boleh lebih dari 23 jam");
            check = false;
        }
        if (this.minutePreDelivery > 59) {
            alertify.error("Menit Lead Time tidak boleh lebih dari 59 menit");
            check = false;
        }

        if (check)
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonPDCConfig)),
            () => {
                if (this.locationCodeMessage == true) {
                    alertify.error("PDC sudah terdaftar");
                    check = false;
                } else {
                    this.PDCConfig.postData(this.locationCodes["locationCode"], this.maintenanceDay, this.carCarrierQuotaPerDay, this.nonCarCarrierQuotaPerDay, this.leadDayPreDeliveryService).then(response => {
                        alertify.success("Data tersimpan");
                    }).catch(response => {
                        if (response.status == "500") {
                            alertify.error("Error ketika menyimpan data");
                        } else {
                            alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.refreshData();
                        this.setPage(this.currentPage);
                        this.clearForm(Form);
                    });
                }
                },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(Form: angular.IFormController) {
        var hariToMenit = 1440;
        var jamToMenit = 60;
        var convertTime = (this.dayPreDelivery * hariToMenit) + (this.hoursPreDelivery * jamToMenit) + this.minutePreDelivery;
        this.leadDayPreDeliveryService = convertTime;

        this.jsonPDCConfig["Location"] = this.locationCodes["locationCode"] + ' - ' + this.locationCodes["name"];
        this.jsonPDCConfig["Periode Maintenance"] = this.maintenanceDay + ' Hari ';
        this.jsonPDCConfig["Unit Car Carrier"] = this.carCarrierQuotaPerDay + ' Unit ';
        this.jsonPDCConfig["Unit Non Car Carrier"] = this.nonCarCarrierQuotaPerDay + ' Unit ';
        this.jsonPDCConfig["Lead Time Persiapan Pengiriman"] = this.dayPreDelivery + ' Hari ' + this.hoursPreDelivery + ' Jam ' + this.minutePreDelivery + ' Menit ';

        let check = true
        if (this.dayPreDelivery > 99) {
            alertify.error("Hari Lead Time tidak boleh melebihi dari 2 digit");
            check = false;
        }
        if (this.hoursPreDelivery > 23) {
            alertify.error("Jam Lead Time tidak boleh lebih dari 23 jam");
            check = false;
        }
        if (this.minutePreDelivery > 59) {
            alertify.error("Menit Lead Time tidak boleh lebih dari 59 menit");
            check = false;
        }

        if(check)
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonPDCConfig)),
            () => {
                    this.PDCConfig.updateData(this.pdcConfigId, this.maintenanceDay, this.carCarrierQuotaPerDay, this.nonCarCarrierQuotaPerDay, this.leadDayPreDeliveryService).then(response => {
                        alertify.success("Data berhasil disimpan");
                    }).catch(response => {
                        if (response.status == "500") {
                            alertify.error("Error ketika menupdate data");
                        } else {
                            alertify.error(response.data);
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
        //this.getAllSelectedData(data);
        this.pdcConfigId = data.pdcConfigId
        var minutes = data.leadDayPreDeliveryService % 60;
        var hours = ((data.leadDayPreDeliveryService - minutes) / 60) % 24;
        var days = (((data.leadDayPreDeliveryService - minutes) / 60) - (((data.leadDayPreDeliveryService - minutes) / 60) % 24)) / 24;

        this.jsonPDCConfig["Location"] = data.locationCode + ' - ' + data.name;
        this.jsonPDCConfig["Periode Maintenance"] = data.maintenanceDayResult + ' Hari ';
        this.jsonPDCConfig["Unit Car Carrier"] = data.carCarrierQuotaPerDayResult + ' Unit ';
        this.jsonPDCConfig["Unit Non Car Carrier"] = data.nonCarCarrierQuotaPerDayResult + ' Unit ';
        this.jsonPDCConfig["Lead Time Persiapan Pengiriman"] = days  + ' Hari ' + hours + ' Jam ' + minutes + ' Menit '

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonPDCConfig)),
            () => {
                this.PDCConfig.deleteData(this.pdcConfigId).then(response => {
                    alertify.success("Data berhasil dihapus");
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Error ketika menghapus data");
                    } else {
                        alertify.error(response.data);
                    }
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                });
            },
            () => {}
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
}


export class PDCConfigComponent implements angular.IComponentOptions {
    controller = PDCConfigController;
    controllerAs = 'me';

    template = require('./PDCConfig.html');
    bindings = {
        greet: '@'
    };
}