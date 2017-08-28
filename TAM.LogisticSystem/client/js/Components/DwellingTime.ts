import * as angular from 'angular';
import * as services from '../Services';
import * as mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as Lodash from 'lodash';

export class DwellingTimeController implements angular.IController {
    static $inject = ['DwellingTimeService'];

    dwellingTimeService: services.DwellingTimeService;
    dwellingTimeViewModel: services.DwellingTimeViewModel[];
    singleDwellingTimeViewModel: services.DwellingTimeViewModel;
    leadTime = new services.LeadTimeViewModel;
    tempTime = new services.LeadTimeViewModel;

    locationData: services.GetDwellingLocationViewModel[];

    dwellingId: number;
    locationFrom: string;
    locationTo: string;
    locationNameFrom: string;
    locationNameTo: string;
    leadMinutes: number;
    totalLeadTime: number;
    checkStatusCombobox: boolean;
    stringError: string;

    editCheck: boolean;

    jsonDwellingTime = {};
    searchData = {};

    constructor(dwellingTimeService: services.DwellingTimeService) {
        this.dwellingTimeService = dwellingTimeService;
    }

    $onInit() {
        this.refreshData();
        this.tempTime.day = 0;
        this.tempTime.hour = 0;
        this.tempTime.minute = 0;
    }

    //Get all delivery lead data
    refreshData() {
        this.dwellingTimeService.getDwellingData().then(response => {
            this.dwellingTimeViewModel = response.data;
            this.totalItems = this.dwellingTimeViewModel.length;
        });
        //Get LocationFrom & LocationTo
        this.dwellingTimeService.getLocationCode().then(response => {
            this.locationData = response.data;
        });
    }

    addData(DwellingTimeForm) {
        this.checkLeadTime();
        this.calculateTotalLeadTimeMinutes();
        this.jsonDwellingTime["Kode Lokasi Asal"] = this.locationFrom;
        this.jsonDwellingTime["Lokasi Asal"] = this.findLocation(this.locationFrom);
        this.jsonDwellingTime["Kode Lokasi Tujuan"] = this.locationTo;
        this.jsonDwellingTime["Lokasi Tujuan"] = this.findLocation(this.locationTo);
        this.jsonDwellingTime["Lead Time"] = this.tempTime.day + ' Hari ' + this.tempTime.hour + ' Jam ' + this.tempTime.minute + ' Menit';
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDwellingTime)),
            () => {
                this.dwellingTimeService.postDwellingData(this.locationFrom, this.locationTo, this.totalLeadTime).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(DwellingTimeForm);
                }).catch(response => {
                    Alertify.error(response.data);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //update selecting Dwelling time data
    updateData(DwellingTimeForm) {
        this.checkLeadTime();
        this.calculateTotalLeadTimeMinutes();
        this.jsonDwellingTime["Kode Lokasi Asal"] = this.locationFrom;
        this.jsonDwellingTime["Lokasi Asal"] = this.findLocation(this.locationFrom);
        this.jsonDwellingTime["Kode Lokasi Tujuan"] = this.locationTo;
        this.jsonDwellingTime["Lokasi Tujuan"] = this.findLocation(this.locationTo);
        this.jsonDwellingTime["Lead Time"] = this.tempTime.day + ' Hari ' + this.tempTime.hour + ' Jam ' + this.tempTime.minute + ' Menit';
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonDwellingTime)),
            () => {
                this.dwellingTimeService.updateDwellingData(this.locationFrom, this.locationTo, this.totalLeadTime).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(DwellingTimeForm);
                }).catch(response => {
                    Alertify.error(response.data);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //delete selecting delivery lead data
    deleteData(data) {
        this.separateTotalLeadTimeMinutes(data.leadMinutes);
        this.jsonDwellingTime["Kode Lokasi Asal"] = data.locationFrom;
        this.jsonDwellingTime["Lokasi Asal"] = data.locationNameFrom;
        this.jsonDwellingTime["Kode Lokasi Tujuan"] = data.locationTo;
        this.jsonDwellingTime["Lokasi Tujuan"] = data.locationNameTo;
        this.jsonDwellingTime["Lead Time"] = this.leadTime.day + ' Hari ' + this.leadTime.hour + ' Jam ' + this.leadTime.minute + ' Menit';
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonDwellingTime)),
            () => {
                this.dwellingTimeService.deleteDwellingData(data.locationFrom, data.locationTo).then(response => {
                    Alertify.success("Data berhasil dihapus");
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(data);
                }).catch(response => {
                    Alertify.error("Data gagal dihapus");
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Selecting Data
    selectEdit(data) {
        this.dwellingId = data.dwellingId;
        this.locationFrom = data.locationFrom;
        this.locationTo = data.locationTo;
        this.locationNameFrom = data.locationNameFrom;
        this.locationNameTo = data.locationNameTo;
        this.separateTotalLeadTimeMinutes(data.leadMinutes);
        this.tempTime.day = this.leadTime.day;
        this.tempTime.hour = this.leadTime.hour;
        this.tempTime.minute = this.leadTime.minute;
        this.editCheck = true;
        this.checkStatusCombobox = true;
        this.stringError = "";
    }

    // Other
    reset(form: angular.IFormController) {
        this.locationFrom = null;
        this.locationTo = null;
        this.leadMinutes = null;
        this.tempTime.day = 0;
        this.tempTime.hour = 0;
        this.tempTime.minute = 0;
        this.editCheck = false;
        this.stringError = null;
        this.searchData = {};
        this.checkStatusCombobox = false;
    }

    //Convert to mustache JSON
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

    //to find Lokasi Asal & Lokasi Tujuan alertify
    findLocation(locationCode: string) {
        let tempLocationModel = Lodash.find(this.locationData, ['locationCode', locationCode]);
        if (tempLocationModel == undefined) {
            return '';
        }
        return tempLocationModel.name
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


    //untuk validasi combobox jika kombinasi sama
    checkCombobox() {
        let findFromTo = Lodash.find(this.dwellingTimeViewModel, { 'locationFrom': this.locationFrom, 'locationTo': this.locationTo });
        this.stringError = "";
        if (findFromTo != undefined) {
            this.stringError = "Kombinasi Lokasi Awal dan Lokasi Tujuan sudah terdaftar";
            return true;
        }
        if (this.locationFrom == this.locationTo && (this.locationFrom != undefined || this.locationTo != undefined)) {
            this.stringError = "Kode Lokasi Awal dan Kode Lokasi Tujuan tidak boleh sama";
            return true;
        }
        if (this.locationFrom == undefined && this.locationTo == undefined) {
            this.stringError = "Kode Lokasi Awal dan Kode Lokasi Tujuan tidak boleh kosong";
            return true;
        }
        else {
            this.stringError = "";
            return false;
        }
    }

    //to disable button if form mandatory empty
    disableButton() {
        if (this.locationFrom == undefined || this.locationTo == undefined || this.checkCombobox() == true
            || this.tempTime.day == undefined || this.tempTime.hour == undefined || this.tempTime.minute == undefined) {
            return true;
        }
        return false;
    }

    //to disable button if form mandatory empty
    disableUpdate() {
        if (this.tempTime.day == undefined || this.tempTime.hour == undefined || this.tempTime.minute == undefined) {
            return true;
        }
        return false;
    }
}
let DwellingTimeComponent = {
    controller: DwellingTimeController,
    controllerAs: 'me',
    template: require("./DwellingTime.html")
}

export { DwellingTimeComponent };