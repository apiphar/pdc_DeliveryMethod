﻿import * as moment from 'moment';
import * as Service from '../services';
import * as alertify from 'alertifyjs';

export class MasterKalenderLiburKerjaController implements angular.IController {
    static $inject = ['MasterKalenderLiburKerjaService'];

    masterKalenderLiburKerjaService: Service.MasterKalenderLiburKerjaService;
    locations: any;
    years: any;
    selectedYear: number;
    firstDayOfYear: any;
    lastDayOfYear: any;
    saturdays = [];
    sundays = [];
    myMonth: any;
    selectedDates = [];
    selectedLocation : string;
    chkSaturday: boolean;
    chkSunday: boolean;
    locationCode: string;
    holiday = {
        Name: '',
        LocationCode: ''
    };

    constructor(HolidayService: Service.MasterKalenderLiburKerjaService) {
        this.masterKalenderLiburKerjaService = HolidayService;

        this.firstDayOfYear = moment().startOf('year');
        this.lastDayOfYear = moment().endOf('year');

        
    }

    $onInit() {
        this.loadLocations();
        //this.loadYears();
    }


    //dapetin data kalender awal
    loadLocations() {
        this.masterKalenderLiburKerjaService.populateLocations().then(response => {
            this.locations = response.data;
            console.log(this.locations);
        });
    }


    //loadYears() {
    //    this.masterKalenderLiburKerjaService.populateYears().then(response => {
    //        this.years = response.data;
    //    });
    //}

    setMonthRange(event, month) {
        console.log('this.selectedYear = ' + this.selectedYear);
        console.log(moment().set({ 'year': this.selectedYear, 'month': 0, 'date': 1 }));
        console.log(moment().set({ 'year': this.selectedYear, 'month': 11, 'date': 31 }));
        //if (month.isBefore(moment().startOf('year')) || month.isAfter(moment().endOf('year'))) {
        if (month.isBefore(this.firstDayOfYear) || month.isAfter(this.lastDayOfYear)) {
            //if (month.isBefore(moment().set({ 'year': this.selectedYear, 'month': 0, 'date': 1 })) || month.isAfter(moment().set({ 'year': this.selectedYear, 'month': 11, 'date': 31 }))) {
            event.preventDefault();
        }
    }

    setCalendar() {
        console.log('this.selectedYear = ' + this.selectedYear);
        this.selectedDates = [];
        this.saturdays = this.getSaturdayInYear(moment());
        this.sundays = this.getSundayInYear(moment());

        var tempFirstDayOfYear = moment().year(this.selectedYear).month(0).date(1).hour(0).minute(0).second(0);
        var tempLastDayOfYear = moment().year(this.selectedYear).month(11).date(31).hour(0).minute(0).second(0);

        this.myMonth = tempFirstDayOfYear;
        this.firstDayOfYear = tempFirstDayOfYear;
        this.lastDayOfYear = tempLastDayOfYear;
    }

    //untuk checkbox sabtu
    selectDeselectSaturday() {
        //var saturdays = this.getSaturdayInYear(2017);

        if (this.chkSaturday) {
            this.selectedDates = this.selectedDates.concat(this.saturdays);
        } else {
            var selected = this.selectedDates;

            for (var i = 0; i < this.selectedDates.length; i++) {
                var date1 = new Date(selected[i]).getTime();

                for (var j = 0; j < this.saturdays.length; j++) {
                    var date2 = new Date(this.saturdays[j]).getTime();

                    if (date1 === date2) {
                        selected.splice(i, 1);
                        i--;
                        break;
                    }
                }
            }

            this.selectedDates = selected;
        }
    }

    //untuk checkbox sunday
    selectDeselectSunday() {
        //var sundays = this.getSundayInYear(2017);

        if (this.chkSunday) {
            this.selectedDates = this.selectedDates.concat(this.sundays);
        } else {
            var selected = this.selectedDates;

            for (var i = 0; i < this.selectedDates.length; i++) {
                var date1 = new Date(selected[i]).getTime();

                for (var j = 0; j < this.sundays.length; j++) {
                    var date2 = new Date(this.sundays[j]).getTime();

                    if (date1 === date2) {
                        selected.splice(i, 1);
                        i--;
                        break;
                    }
                }
            }

            this.selectedDates = selected;
        }
    }

    //dapetin hari-hari sabtu
    getSaturdayInYear(year) {
        var date = new Date(year, 0, 1);
        while (date.getDay() != 6) {
            date.setDate(date.getDate() + 1);
        }
        var days = [];
        while (date.getFullYear() == year) {
            var m = date.getMonth() + 1;
            var d = date.getDate();
            days.push(moment(
                year + '-' +
                (m < 10 ? '0' + m : m) + '-' +
                (d < 10 ? '0' + d : d)
            ));
            date.setDate(date.getDate() + 7);
        }
        return days;
    }

    //dapetin hari-hari minggu
    getSundayInYear(year) {
        var date = new Date(year, 0, 1);
        while (date.getDay() != 0) {
            date.setDate(date.getDate() + 1);
        }
        var days = [];
        while (date.getFullYear() == year) {
            var m = date.getMonth() + 1;
            var d = date.getDate();
            days.push(moment(
                year + '-' +
                (m < 10 ? '0' + m : m) + '-' +
                (d < 10 ? '0' + d : d)
            ));
            date.setDate(date.getDate() + 7);
        }
        return days;
    }


    //simpan data kalender
    saveKalender() {
        if (this.selectedLocation === undefined || this.selectedDates.length === 0) {
            alertify.error("Data Tidak Valid");
        }
        else {
            alertify.confirm("Konfirmasi", "Konfirmasi Data?",
                () => {
                    //this.masterKalenderLiburKerjaService.saveData(this.selectedLocation, this.selectedDates).then(response => {
                    console.log(this.selectedDates);
                    //});
                    alertify.success('Berhasil');
                },
                () => {
                    alertify.error('Gagal');
                });
        }
        

        
    }

    resetForm(form: angular.IFormController) {
        form.$setPristine;
        form.$setUntouched;
        this.selectedLocation = undefined;
        this.chkSaturday = undefined;
        this.chkSunday = undefined;
       
        
    }
}

let MasterKalenderLiburKerja = {
    controller: MasterKalenderLiburKerjaController,
    controllerAs: "me",
    template: require("./MasterKalenderLiburKerja.html")
}

export { MasterKalenderLiburKerja }
