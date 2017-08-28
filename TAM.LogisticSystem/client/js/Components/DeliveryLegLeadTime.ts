import * as angular from 'angular';
import * as $ from 'jquery';
import * as Lodash from 'lodash';
import * as Alertify from 'alertifyjs';
import * as service from '../Services';
import * as mustache from 'mustache';

class DeliveryLegLeadTimeController implements angular.IController {
    static $inject = ["DeliveryLegLeadTimeService", '$rootScope'];

    deliveryLegLeadTimeService: service.DeliveryLegLeadTimeService;

    deliveryLegLeadTimeViewModel: service.DeliveryLegLeadTimeViewModel[];
    singleDeliveryLegLeadTimeViewModel: service.DeliveryLegLeadTimeViewModel;
    locationViewModel: service.GetLocationLeadTimeViewModel;
    deliveryMethodCodeViewModel: service.GetDeliveryMethodViewModel[];
    filterDeliveryMethodCodeViewModel: service.GetDeliveryMethodViewModel[];
    ccDetaildeliveryMethodCodeViewModel: service.GetDeliveryMethodViewModel[];
    scDetaildeliveryMethodCodeViewModel: service.GetDeliveryMethodViewModel[];

    leadTime = new service.LeadTimeViewModel();
    tempTime = new service.LeadTimeViewModel;
    deliveryMethodCode: string;
    tempDeliveryMethodCode: string;
    tempDeliveryMethodAlertify: string;
    deliveryLeadTimeId: number;
    leadMinutes: number;
    locationFrom: string;
    locationTo: string;
    totalLeadTime: number;
    deleteDay: number;
    deleteHour: number;
    deleteMinute: number;
    parentDeliveryMethodCode: string;

    flagCC: boolean = false;
    flagSC: boolean = false;
    pageState: boolean = true;

    editCheck: boolean;
    jsonDeliveryLeadTime = {};
    searchData = {};
    deliveryLegCode = document.getElementById('receivedDeliveryLegCode').innerHTML;
    root: angular.IRootScopeService;

    constructor(deliveryLegLeadTimeService: service.DeliveryLegLeadTimeService, root: angular.IRootScopeService) {
        this.deliveryLegLeadTimeService = deliveryLegLeadTimeService;
        this.root = root;
    }

    $onInit() {
        this.refreshData();
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
        this.tempTime.day = 0;
        this.tempTime.hour = 0;
        this.tempTime.minute = 0;
    }

    flagClearCC() {
        this.tempDeliveryMethodCode = undefined;
        this.flagCC = true;
        this.flagSC = false;
    }

    flagClearSC() {
        this.tempDeliveryMethodCode = undefined;
        this.flagSC = true;
        this.flagCC = false;
    }

    //detail kode moda (deliverymethodcode)
    detailMethod() {
        this.filterDeliveryMethodCodeViewModel = Lodash.filter(this.deliveryMethodCodeViewModel, ['parentDeliveryMethodCode', null]);
        if (this.deliveryMethodCode == 'CCRT' || this.deliveryMethodCode == 'CCST' || this.deliveryMethodCode == 'CCU' || this.deliveryMethodCode == 'CC') {
            this.ccDetaildeliveryMethodCodeViewModel = Lodash.filter(this.deliveryMethodCodeViewModel, ['parentDeliveryMethodCode', 'CC']);
            this.flagClearCC();
            return;
        }
        if (this.deliveryMethodCode == 'SCRT' || this.deliveryMethodCode == 'SCST' || this.deliveryMethodCode == 'SCU' || this.deliveryMethodCode == 'SC') {
            this.scDetaildeliveryMethodCodeViewModel = Lodash.filter(this.deliveryMethodCodeViewModel, ['parentDeliveryMethodCode', 'SC']);
            this.flagClearSC();
            return;
        }
        this.flagCC = false;
        this.flagSC = false;
        return;
    }

    checkDeliveryMethod() {
        if (this.deliveryMethodCode == 'CC' || this.deliveryMethodCode == 'SC') {
            this.tempDeliveryMethodAlertify = this.tempDeliveryMethodCode;
        }
        else {
            this.tempDeliveryMethodAlertify = this.deliveryMethodCode;
        }
    }

    checkSelectDeliveryMethod() {
        this.detailMethod();
        let temp: service.GetDeliveryMethodViewModel = Lodash.find(this.deliveryMethodCodeViewModel, ['deliveryMethodCode', this.deliveryMethodCode]);
        if (temp.parentDeliveryMethodCode != null) {
            this.deliveryMethodCode = temp.parentDeliveryMethodCode;
            this.tempDeliveryMethodCode = temp.deliveryMethodCode;
            if (this.deliveryMethodCode == 'CC') {
                this.flagCC = true;
                return;
            }
            if (this.deliveryMethodCode == 'SC') {
                this.flagSC = true;
                return;
            }
            this.flagCC = false;
            this.flagSC = false;
        }
        else {
            this.deliveryMethodCode = temp.deliveryMethodCode;
        }

    }

    //Get all delivery lead data
    refreshData() {
        this.deliveryLegLeadTimeService.getAllDeliveryLeadData(this.deliveryLegCode).then(response => {
            this.deliveryLegLeadTimeViewModel = response.data;
            this.totalItems = this.deliveryLegLeadTimeViewModel.length;
        });
        //Get semua kode moda
        this.deliveryLegLeadTimeService.getDeliveryMethodCode().then(response => {
            this.deliveryMethodCodeViewModel = response.data;
            this.detailMethod();
        });
        //Get LocationFrom & LocationTo berdasarkan deliveryLegCode
        this.deliveryLegLeadTimeService.getLocation(this.deliveryLegCode).then(response => {
            this.locationViewModel = response.data;
        }).catch(response => {
            if (response.status == "400") {
                Alertify.error(response.data);
                return;
            }
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    showDeliveryMethod(deliveryMethodCode: string) {
        let tempMethodModel = Lodash.find(this.deliveryMethodCodeViewModel, ['deliveryMethodCode', deliveryMethodCode]);
        if (tempMethodModel == undefined) {
            return '';
        }
        return tempMethodModel.deliveryMethodCode + ' - ' + tempMethodModel.deliveryMethodName;
    }

    //kirim data ke master alertify untuk Insert & Update
    jsonAlertify() {
        this.jsonDeliveryLeadTime["Kode Delivery Leg"] = this.deliveryLegCode;
        this.jsonDeliveryLeadTime["Lokasi Asal"] = this.locationViewModel.locationFrom;
        this.jsonDeliveryLeadTime["Lokasi Tujuan"] = this.locationViewModel.locationTo;
        this.jsonDeliveryLeadTime["Kode Moda"] = this.tempDeliveryMethodAlertify;
        this.jsonDeliveryLeadTime["Lead Time"] = this.tempTime.day + ' Hari ' + this.tempTime.hour + ' Jam ' + this.tempTime.minute + ' Menit';
    }

    //kirim data ke master alertify untuk Delete
    jsonDeleteAlertify(data) {
        this.jsonDeliveryLeadTime["Kode Delivery Leg"] = this.deliveryLegCode;
        this.jsonDeliveryLeadTime["Lokasi Asal"] = this.locationViewModel.locationFrom;
        this.jsonDeliveryLeadTime["Lokasi Tujuan"] = this.locationViewModel.locationTo;
        this.jsonDeliveryLeadTime["Kode Moda"] = data.deliveryMethodCode;
        this.jsonDeliveryLeadTime["Lead Time"] = this.leadTime.day + ' Hari ' + this.leadTime.hour + ' Jam ' + this.leadTime.minute + ' Menit';
    }

    //insert delivery lead data
    addData(DeliveryLegLeadTimeForm) {
        this.checkDeliveryMethod();
        this.checkLeadTime();
        this.calculateTotalLeadTimeMinutes();
        this.jsonAlertify();
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryLeadTime)),
            () => {
                this.deliveryLegLeadTimeService.postDeliveryLeadData(this.deliveryLegCode, this.tempDeliveryMethodAlertify, this.totalLeadTime).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.reset(DeliveryLegLeadTimeForm);
                    this.refreshData();
                    this.setPage(this.currentPage);
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //update selecting delivery lead data
    updateData(DeliveryLegLeadTimeForm) {
        this.checkDeliveryMethod();
        this.checkLeadTime();
        this.calculateTotalLeadTimeMinutes();
        this.jsonAlertify();
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonDeliveryLeadTime)),
            () => {
                this.deliveryLegLeadTimeService.updatePlafondData(this.deliveryLeadTimeId, this.locationFrom, this.locationTo, this.tempDeliveryMethodAlertify, this.totalLeadTime).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.reset(DeliveryLegLeadTimeForm);
                    this.refreshData();
                    this.setPage(this.currentPage);
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //delete selecting delivery lead data
    deleteData(data, DeliveryLegLeadTimeForm) {
        this.checkLeadTime();
        this.calculateTotalLeadTimeMinutes();
        this.separateTotalLeadTimeMinutes(data.leadMinutes);
        this.jsonDeleteAlertify(data);
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonDeliveryLeadTime)),
            () => {
                this.deliveryLegLeadTimeService.deleteDeliveryLeadData(data.deliveryLeadTimeId).then(response => {
                    Alertify.success("Data berhasil dihapus");
                    this.reset(DeliveryLegLeadTimeForm);
                    this.refreshData();
                    this.setPage(this.currentPage);
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.deliveryLeadTimeId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "DeliveryLeadTime";
        info.title = "Delivery Leg Lead Time";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "DeliveryLeadTime";
        info.title = "Delivery Leg Lead Time"
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }
    //convert Mustache to JSON
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

    selectEdit(data) {
        this.deliveryLeadTimeId = data.deliveryLeadTimeId;
        this.locationFrom = data.locationFrom;
        this.locationTo = data.locationTo;
        this.deliveryMethodCode = data.deliveryMethodCode;
        this.checkSelectDeliveryMethod();
        this.leadMinutes = data.leadMinutes;
        this.separateTotalLeadTimeMinutes(data.leadMinutes);
        this.tempTime.day = this.leadTime.day;
        this.tempTime.hour = this.leadTime.hour;
        this.tempTime.minute = this.leadTime.minute;
        this.editCheck = true;
    }



    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
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

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    // Other
    reset(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.locationFrom = undefined;
        this.locationTo = undefined;
        this.deliveryMethodCode = undefined;
        this.leadMinutes = undefined;
        this.tempTime.day = 0;
        this.tempTime.hour = 0;
        this.tempTime.minute = 0;
        this.tempDeliveryMethodCode = undefined;
        this.searchData = {};
        this.editCheck = false;
    }

    // Check the lead time day, hour, and minute. If some of them are undefined/not assigned by the user in the textbox, this method 
    // will assign them to 0
    checkLeadTime() {
        this.tempTime.day = this.tempTime.day === undefined ? 0 : this.tempTime.day;
        this.tempTime.hour = this.tempTime.hour === undefined ? 0 : this.tempTime.hour;
        this.tempTime.minute = this.tempTime.minute === undefined ? 0 : this.tempTime.minute;
    }

    // Get the total lead time minutes by calculating the lead time days, hours, and minutes
    calculateTotalLeadTimeMinutes() {
        this.totalLeadTime = (this.tempTime.day * 1440) +
            (this.tempTime.hour * 60) +
            this.tempTime.minute;
    }
    // Separate the total time minutes by days, hours, and minutes and append the values to their own property respectively
    separateTotalLeadTimeMinutes(leadTime: number) {
        this.leadTime.day = Math.floor(leadTime / 24 / 60);
        this.leadTime.hour = Math.floor(leadTime / 60 % 24);
        this.leadTime.minute = Math.floor(leadTime % 60);

        return this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit";
    }
}

let DeliveryLegLeadTimeComponent = {
    controller: DeliveryLegLeadTimeController,
    controllerAs: 'me',
    template: require("./DeliveryLegLeadTime.html")
}

export { DeliveryLegLeadTimeComponent };