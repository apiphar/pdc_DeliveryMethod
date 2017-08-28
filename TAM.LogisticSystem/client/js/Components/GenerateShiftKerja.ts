import * as angular from 'angular';
import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as moment from 'moment';

export class GenerateShiftKerjaController implements angular.IController {
    static $inject = ['GenerateShiftKerjaService'];

    constructor(generateShiftKerjaService: service.GenerateShiftKerjaService) {
        this.generateShiftKerjaService = generateShiftKerjaService;
    }
    generateShiftKerjaService: service.GenerateShiftKerjaService;

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
    generateShiftKerjaPage: service.GenerateShiftKerjaPageViewModel;
    workingTime: service.LocationWorkHourViewModel;
    jsonAlertify = {};

    // Other Property
    workingDictionaryRadio: boolean;
    workingDescription: string;
    search = {};

    // onInit
    $onInit() {
        this.workingTime = new service.LocationWorkHourViewModel();
        this.workingDictionaryRadio = false;
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
        this.dateOverOptions.minDate = this.workingTime.validFrom;
        this.workingTime.validTo = this.workingTime.validFrom;
    }

    // Fill form on radio button clicked
    fillForm(idleDictionary: service.WorkHourTemplateViewModel) {
        this.workingTime.workHourTemplateCode = idleDictionary.workHourTemplateCode;
        this.workingDescription = idleDictionary.description;
    }

    // Get all
    getAll() {
        return this.generateShiftKerjaService.getAll().then(response => {
            this.generateShiftKerjaPage = response.data as service.GenerateShiftKerjaPageViewModel;
        })
    }

    disableButton() {
        if (this.workingTime.workHourTemplateCode == undefined || this.workingTime.locationCode == undefined || this.workingTime.validFrom == undefined || this.workingTime.validTo == undefined) {
            return true;
        }
        return false;
    }

    // Send data
    generateData(form: angular.IFormController) {
        this.jsonAlertify['Kode Pola'] = this.workingTime.workHourTemplateCode;
        this.jsonAlertify['Keterangan'] = this.workingDescription;
        this.jsonAlertify['Lokasi'] = this.workingTime.locationCode;
        this.jsonAlertify['Tanggal'] = moment(this.workingTime.validFrom).format('DD-MMM-YYYY') + ' s/d ' + moment(this.workingTime.validTo).format('DD-MMM-YYYY');
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.jsonAlertify)),
            () => {
                this.generateShiftKerjaService.generateData(this.workingTime).then(() => {
                    alertify.success('Data berhasil di-generate');
                    this.setPristine(form);
                }).catch(() => {
                    alertify.error('Gagal generate jam break karena data tidak valid atau sudah digenerate');
                });
            },
            () => {

            }

        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        // TIE: START

        // Angular.forEach(json, (value, key) => {
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });
        // TIE: END

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
        this.workingTime = new service.LocationWorkHourViewModel();
        this.workingDescription = null;
        this.workingDictionaryRadio = false;
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
    }
}

let GenerateShiftKerjaComponent = {
    template: require('./GenerateShiftKerja.html'),
    controller: GenerateShiftKerjaController,
    controllerAs: 'me'
}

export { GenerateShiftKerjaComponent }