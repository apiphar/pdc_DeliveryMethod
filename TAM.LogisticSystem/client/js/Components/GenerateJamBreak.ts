import * as angular from 'angular';
import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as moment from 'moment';
import * as _ from 'lodash';

class GenerateJamBreakController implements angular.IController {
    static $inject = ['GenerateJamBreakService'];

    constructor(generateJamBreakService: service.GenerateJamBreakService) {
        this.generateJamBreakService = generateJamBreakService;
    }
    generateJamBreakService: service.GenerateJamBreakService

    // Datetime Property
    dateStartPopup: boolean;
    dateOverPopup: boolean;
    altInputFormats = ['M!/d!/yyyy'];
    dateStartOptions = {
        formatYear: 'yy',
        minDate: new Date(),
        startingDay: 1
    }
    dateOverOptions = {
        formatYear: 'yy',
        minDate: new Date(),
        startingDay: 1
    }

    // Model Property
    generateJamBreakPage: service.GenerateJamBreakPageViewModel;
    idleTime: service.LocationBreakHour;
    jsonAlertify = {};

    // Other Property
    idleDictionaryRadio: boolean;
    idleDescription: string;
    search = {};

    // onInit
    $onInit() {
        this.idleTime = new service.LocationBreakHour();
        this.idleDictionaryRadio = false;
        this.getAll();
    }

    // Open Date Picker
    openDateStart() {
        this.dateStartPopup = true;
    }
    openDateOver() {
        this.dateOverPopup = true;
    }

    // On dateStart Change
    dateStartChange() {
        this.dateOverOptions.minDate = this.idleTime.validFrom;
        this.idleTime.validTo = this.idleTime.validFrom;
    }

    // Fill form on radio button clicked
    fillForm(idleDictionary: service.BreakHourTemplateViewModel) {
        this.idleTime.breakHourTemplateCode = idleDictionary.breakHourTemplateCode;
        this.idleDescription = idleDictionary.description;
    }

    // Get all
    getAll() {
        return this.generateJamBreakService.getAll().then(response => {
            this.generateJamBreakPage = response.data as service.GenerateJamBreakPageViewModel;
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    disableButton() {
        if (this.idleTime.breakHourTemplateCode == undefined || this.idleTime.location == undefined || this.idleTime.validFrom == undefined || this.idleTime.validTo == undefined) {
            return true;
        }
        return false;
    }

    toUTC(dateTime: Date) {
        let year = dateTime.getFullYear();
        let month = dateTime.getMonth();
        let day = dateTime.getDate();
        let utcDate: Date = new Date(Date.UTC(year, month, day));
        return utcDate;
    }

    // Send data
    generateData(form: angular.IFormController) {
        this.jsonAlertify['Kode Pola'] = this.idleTime.breakHourTemplateCode;
        this.jsonAlertify['Keterangan'] = this.idleDescription;
        this.jsonAlertify['Lokasi'] = this.idleTime.location.locationCode + ' - ' + this.idleTime.location.locationName;
        this.jsonAlertify['Tanggal'] = moment(this.idleTime.validFrom).format('DD-MMM-YYYY') + ' s/d ' + moment(this.idleTime.validTo).format('DD-MMM-YYYY');

        this.idleTime.validFrom = this.toUTC(this.idleTime.validFrom);
        this.idleTime.validTo = this.toUTC(this.idleTime.validTo);

        let onDuplicate = 'Akan ada perubahan pada data. ';
        let normalMessage = 'Apakah anda yakin ingin menyimpan data?</strong>';
        let message: string = '';
        this.generateJamBreakService.checkDuplicate(this.idleTime).then(() => {
            message = '<strong>' + normalMessage;
        }).catch(response => {
            if (response.data == 'DUPLICATE') {
                message = '<strong>' + onDuplicate + normalMessage;
            }
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        }).finally(() => {
            alertify.confirm(
                'Konfirmasi',
                message + mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('', this.jsonAlertify)),
                () => {
                    this.generateJamBreakService.generateData(this.idleTime).then(() => {
                        alertify.success('Data berhasil di-generate');
                        this.setPristine(form);
                    }).catch(response => {
                        if (response.status == "500") {
                            alertify.error("Koneksi ke server bermasalah");
                        } else {
                            alertify.error('Data gagal di-generate');
                        }
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        });
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

    // Clear Form
    setPristine(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.idleTime = new service.LocationBreakHour();
        this.idleDescription = null;
        this.idleDictionaryRadio = false;
        this.search = {};
    }

    // Pagination
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
        this.setPage(1);
    }

    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }
}

let GenerateJamBreakComponent = {
    controller: GenerateJamBreakController,
    controllerAs: 'me',
    template: require('./GenerateJamBreak.html')
}

export { GenerateJamBreakComponent }