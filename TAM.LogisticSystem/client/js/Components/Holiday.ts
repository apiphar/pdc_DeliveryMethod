import * as moment from 'moment';
import * as angular from 'angular';
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as mustache from 'mustache';



export class HolidayController implements angular.IController {
    static $inject = ['HolidayService', '$scope'];

    constructor(holidayService: Service.HolidayService, $scope: angular.IScope) {
        this.holidayService = holidayService;
        this.$scope = $scope;
        this.firstDayOfYear = moment().startOf('year');
        this.lastDayOfYear = moment().endOf('year');

    }

    holidayService: Service.HolidayService;
    $scope: angular.IScope;
    locations: Service.HolidayData[];
    years: any;

    selectedLocation: string;
    selectedDate: Date;
    selectedDateString: string;


    selectedYear: number;
    firstDayOfYear: any;
    lastDayOfYear: any;

    saturdays = [];
    sundays = [];
    myMonth: any;
    selectedDates = [];


    jsonKalender = {};

    option: any;

    mainArray: Service.HolidayData[];

    addedKalender: Service.HolidayData[];
    deletedKalender: Service.HolidayData[];

    finalAdd: Service.HolidayData[];
    finalDelete: Service.HolidayData[];

    holidayKalender: Service.HolidayData;

    chkSaturday: boolean;
    chkSunday: boolean;

    checkedSat: boolean;
    checkedSun: boolean;


    $onInit() {
        this.loadLocations();
        this.loadYears();
        this.getAllKalenderData();
        this.holidayKalender = new Service.HolidayData;
        this.addedKalender = new Array<Service.HolidayData>();
        this.deletedKalender = new Array<Service.HolidayData>();

    }

    //untuk pagination
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

        this.totalItems = data.length;
        this.setPage(1);

    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    //pagination end



    //get data lengkap lokasi untuk select
    loadLocations() {
        this.holidayService.populateLocations().then(response => {
            this.locations = response.data as Service.HolidayData[];

        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah")
            }
        });
    }

    //get 5 tahun opsi kalender untuk select
    loadYears() {
        this.holidayService.populateYears().then(response => {
            this.years = response.data;
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah")
            }
        });
    }

    //get Holiday data dari Db untuk isi mainArray
    getAllKalenderData() {
        this.holidayService.getData().then(response => {
            this.mainArray = response.data as Service.HolidayData[];

            this.totalItems = this.mainArray.length;



            angular.forEach(this.mainArray, (data) => {

                data.stringDate = moment(data.holidayDate).format("DD-MMM-YYYY");
                data.dateDate = new Date(data.holidayDate);
            });

        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah")
            }
        });
    }


    changingLocation(result) {
        this.search(result);
        this.setCalendar(result);
    }

    //untuk set kalender, dan nilai untuk fungsi lain
    setCalendar(data) {
        if (this.selectedYear !== undefined) {
            this.search(data);
            this.getAllKalenderData();
            this.saturdays = this.getSaturdayInYear(this.selectedYear);
            this.sundays = this.getSundayInYear(this.selectedYear);
            this.addedKalender = new Array<Service.HolidayData>();
            this.deletedKalender = new Array<Service.HolidayData>();

            var tempFirstDayOfYear = moment().year(this.selectedYear).month(0).date(1).hour(0).minute(0).second(0);
            var tempLastDayOfYear = moment().year(this.selectedYear).month(11).date(31).hour(0).minute(0).second(0);

            this.checkSaturday();
            this.checkSunday();


            this.selectedDate = new Date(this.selectedYear, 0, 1);
            this.option.minDate = new Date(this.selectedYear, 0, 1);
            this.option.maxDate = new Date(this.selectedYear, 11, 31);
        }



    }

    //function cekbox sabtu
    selectDeselectSaturday() {
        if (this.chkSaturday === true) {

            angular.forEach(this.saturdays, (data) => {

                if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'holidayDate': data }) === undefined
                    && _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': data }) === undefined) {

                    let sabtu = new Service.HolidayData();
                    sabtu.holidayDate = data;
                    sabtu.locationCode = this.selectedLocation;
                    sabtu.name = this.findLocationName(this.selectedLocation);
                    sabtu.dateDate = new Date(data);

                    this.mainArray.push(sabtu);

                    this.addedKalender.push(sabtu);

                }

            });


        }
        if (this.chkSaturday === false) {
            angular.forEach(this.saturdays, (data) => {
                if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': data }) !== undefined) {

                    let sabtu = _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': data })
                    _.remove(this.mainArray, sabtu);
                    this.deletedKalender.push(sabtu);
                }

                if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'holidayDate': data }) !== undefined) {
                    let sabtu = _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'holidayDate': data })
                    _.remove(this.mainArray, sabtu);
                    this.deletedKalender.push(sabtu);

                }

            });
        }
    }

    //dapetin hari sabtu setahun
    getSaturdayInYear(year) {

        var date = new Date(year, 0, 1);
        while (date.getDay() != 6) {
            date.setDate(date.getDate() + 1);
        }
        var days = new Array<Date>();
        while (date.getFullYear() == year) {
            var m = date.getMonth() + 1;
            var d = date.getDate();
            days.push(moment(
                year + '-' +
                (m < 10 ? '0' + m : m) + '-' +
                (d < 10 ? '0' + d : d)
            ).toDate());
            date.setDate(date.getDate() + 7);
        }
        return days;
    }

    //fungsi cekbox minggu
    selectDeselectSunday() {
        if (this.chkSunday === true) {

            angular.forEach(this.sundays, (data) => {

                if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'holidayDate': data }) === undefined
                    && _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': data }) === undefined) {

                    let minggu = new Service.HolidayData();
                    minggu.holidayDate = data;
                    minggu.locationCode = this.selectedLocation;
                    minggu.name = this.findLocationName(this.selectedLocation);
                    minggu.dateDate = new Date(data);

                    this.mainArray.push(minggu);

                    this.addedKalender.push(minggu);

                }
            });


        }

        if (this.chkSunday === false) {
            angular.forEach(this.sundays, (data) => {
                if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': data }) !== undefined) {


                    let minggu = _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': data })
                    _.remove(this.mainArray, minggu);
                    this.deletedKalender.push(minggu);

                }

                if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'holidayDate': data }) !== undefined) {
                    let minggu = _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'holidayDate': data })

                    _.remove(this.mainArray, minggu);
                    this.deletedKalender.push(minggu);
                }

            });
        }
    }

    //dapetin tanggal hari minggu setahun
    getSundayInYear(year) {
        var date = new Date(year, 0, 1);
        while (date.getDay() != 0) {
            date.setDate(date.getDate() + 1);
        }
        var days = new Array<Date>();
        while (date.getFullYear() == year) {
            var m = date.getMonth() + 1;
            var d = date.getDate();
            days.push(moment(
                year + '-' +
                (m < 10 ? '0' + m : m) + '-' +
                (d < 10 ? '0' + d : d)
            ).toDate());
            date.setDate(date.getDate() + 7);
        }
        return days;
    }


    //cek cekbox sabtu
    checkSaturday() {
        this.chkSaturday = true;
        angular.forEach(this.saturdays, (date) => {
            if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': date }) !== undefined
                || _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'stringDate': date }) !== undefined) {


            }

            else {
                this.chkSaturday = false;

            }
        });

    }

    //cek cekbox minggu
    //cek cekbox sabtu
    checkSunday() {
        this.chkSunday = true;
        angular.forEach(this.sundays, (date) => {
            if (_.find(this.mainArray, { 'locationCode': this.selectedLocation, 'dateDate': date }) !== undefined
                || _.find(this.mainArray, { 'locationCode': this.selectedLocation, 'stringDate': date }) !== undefined) {

            }

            else {
                this.chkSunday = false;

            }
        });

    }

    //convert data ke json
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

    //simpan ke database
    saveDate(result, form) {
        this.finalAdd = this.addedKalender;
        this.finalDelete = this.deletedKalender;
        //alertify.confirm(
        //    "Konfirmasi",
        //    "Apakah anda yakin untuk mengubah data?",
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonKalender)),
            () => {
                if (this.addedKalender !== undefined) {

                    this.holidayService.saveAddData(this.finalAdd).then(response => {

                        this.addedKalender = new Array<Service.HolidayData>();
                        this.deletedKalender = new Array<Service.HolidayData>();
                        this.getAllKalenderData();
                        this.resetAll(form);
                    }).catch(response => {
                        console.log(response.status);
                        if (response.status == "400") {
                            alertify.error(response.data);
                        }
                        if (response.status == "500") {
                            alertify.error("Koneksi ke server bermasalah")
                        }
                        if (response.status == "200") {
                            alertify.success("Data berhasil disimpan");
                        }
                    });
                }

                if (this.deletedKalender !== undefined) {

                    this.holidayService.saveDelData(this.finalDelete).then(response => {

                        alertify.success("Data berhasil disimpan");
                        this.addedKalender = new Array<Service.HolidayData>();
                        this.deletedKalender = new Array<Service.HolidayData>();
                        this.getAllKalenderData();
                        this.resetAll(form);

                    }).catch(response => {
                        console.log(response.status);
                        if (response.status == "400") {
                            alertify.error(response.data);
                        }
                        if (response.status == "500") {
                            alertify.error("Koneksi ke server bermasalah")
                        }
                        if (response.status == "200") {
                            alertify.success("Data berhasil disimpan");
                        }
                    });
                }
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //cari nama locationCode
    findLocationName(data: string) {

        let location = _.find(this.locations, { 'locationCode': data });

        return location.name;
    }

    //reset tampilan
    resetAll(form: angular.IFormController) {


        this.holidayKalender = new Service.HolidayData;
        this.selectedLocation = undefined;
        this.selectedYear = undefined;
        this.chkSaturday = undefined;
        this.chkSunday = undefined;
        this.totalItems = this.mainArray.length;
        this.getAllKalenderData();
        this.addedKalender = undefined;
        this.deletedKalender = undefined;
        form.$setPristine();
        form.$setUntouched();


    }

    //tambah data ke array Add
    appendNewDate(result) {
        let resultSize = result;
        if (this.selectedLocation === undefined || this.selectedYear === undefined) {
            alertify.error("Lokasi dan Tahun tidak boleh kosong");
        }
        else {

            this.holidayKalender.holidayDate = this.selectedDate;
            this.holidayKalender.locationCode = this.selectedLocation;
            this.holidayKalender.name = this.findLocationName(this.selectedLocation);
            this.holidayKalender.dateDate = new Date(this.selectedDate);
            let test = moment(this.holidayKalender.holidayDate).format("DD-MMM-YYYY");




            if (_.find(this.mainArray, { 'locationCode': this.holidayKalender.locationCode, 'stringDate': test }) === undefined
                && _.find(this.mainArray, { 'locationCode': this.holidayKalender.locationCode, 'holidayDate': this.holidayKalender.holidayDate }) === undefined) {

                this.mainArray.push(this.holidayKalender);
                this.addedKalender.push(this.holidayKalender);
                this.checkSaturday();
                this.checkSunday();
            }

            else {
                alertify.error("Tanggal sudah terdaftar");
            }



            this.holidayKalender = new Service.HolidayData;
        }



    }

    //tambah data ke array Delete
    deleteOldDate(data, result, form) {
        let resultSize = result;

        this.jsonKalender["Tanggal"] = moment(data.holidayDate).format("DD-MMM-YYYY");
        this.jsonKalender["Lokasi"] = this.findLocationName(data.locationCode);

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonKalender)),
            () => {
                _.remove(this.mainArray, data);

                alertify.success("Data berhasil terhapus");
                this.deletedKalender.push(data);
                this.checkSaturday();
                this.checkSunday();
                this.search(resultSize);

                this.$scope.$apply();

                this.mainArray = this.mainArray;

            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }
}

let Holiday = {
    controller: HolidayController,
    controllerAs: 'vm',

    template: require('./Holiday.html')
}

export { Holiday }