
import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as moment from 'moment';
import * as mustache from 'mustache';
import * as angular from 'angular';

class VesselArrivalController implements angular.IController {
    static $inject = ['VesselArrivalService'];

    constructor(vesselArrivalService: service.VesselArrivalService) {
        this.vesselArrivalService = vesselArrivalService;
    }
    vesselArrivalService: service.VesselArrivalService;

    // Model Property
    vesselArrivalPageViewModel: service.VesselArrivalPageViewModel;
    vesselArrivalViewModel: service.VesselArrivalViewModel;
    voyageDestinationViewModel: service.VoyageDestinationViewModel;
    jsonData = {};

    // DateTime Property
    arrivalDate: Date;
    datePopup: boolean = false;
    altInputFormats = ['M!/d!/yyyy'];
    dateOptions = {
        formatYear: 'yy',
        minDate: new Date(),
        startingDay: 1
    }
    arrivalTime: Date;

    // Other Property
    voyageFound: boolean = false;
    voyagePattern: string = '^[a-zA-Z0-9]{1,16}$';
    search: {};

    $onInit() {
        this.getAll();
    }

    // Get all data
    getAll() {
        return this.vesselArrivalService.GetAll().then(response => {
            this.vesselArrivalPageViewModel = response.data as service.VesselArrivalPageViewModel;
            for (let i in this.vesselArrivalPageViewModel.unitLists) {
                let tempPdcIn = this.vesselArrivalPageViewModel.unitLists[i].pdcIn;
                let tempPdd = this.vesselArrivalPageViewModel.unitLists[i].requestedPdd;
                this.vesselArrivalPageViewModel.unitLists[i].pdcInString = moment(tempPdcIn).format('DD-MMM-YYYY');
                this.vesselArrivalPageViewModel.unitLists[i].requestedPddString = moment(tempPdd).format('DD-MMM-YYYY');
            }
        });
    }

    // Search specific voyage
    searchVoyage(voyageNumber: string) {
        let tempVoyage = voyageNumber;
        if (tempVoyage != undefined) {
            tempVoyage = tempVoyage.toUpperCase();
        }
        this.vesselArrivalViewModel = _.find(this.vesselArrivalPageViewModel.viewModels, ['voyageNumber', tempVoyage]);
        if (this.vesselArrivalViewModel == null) {
            alertify.error('"Voyage No." tidak ditemukan');
            this.voyageFound = false;
            return;
        }
        this.voyageFound = true;
    }

    // Open date picker popup on click
    openDatePicker() {
        this.datePopup = true;
    }

    // Convert dari textbox Date dan textbox Time jadi satu value
    convertToDateTime(arrivalDate: Date, arrivalTime: Date) {
        if (arrivalDate == undefined || arrivalTime == undefined) {
            // TIE: START
            // return;
            return 0;
            // TIE: END
        }
        let arrivalDateTime = new Date(arrivalDate.getFullYear(), arrivalDate.getMonth(), arrivalDate.getDate(), arrivalTime.getHours(), arrivalTime.getMinutes(), arrivalTime.getSeconds());
        return arrivalDateTime;
    }

    getCityName() {
        let tempModel = _.find(this.vesselArrivalPageViewModel.cityLists, ['cityId', this.voyageDestinationViewModel.destinationCity]);
        return tempModel.cityName;
    }

    disableButton() {
        if (this.voyageFound == false || this.arrivalDate == undefined || this.arrivalTime == undefined || this.voyageDestinationViewModel.destinationCity == undefined) {
            return true;
        }
        return false;
    }

    // Send data ke VesselArrivalService
    sendData(arrivalDate: Date, arrivalTime: Date, form: angular.IFormController) {
        // TIE: START
        // this.voyageDestinationViewModel.estimatedTimeArrival = this.convertToDateTime(arrivalDate, arrivalTime);
        // TIE: END
        let tempModel = this.voyageDestinationViewModel;
        this.fillJsonData();
        alertify.confirm("Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonData)),
            () => {
                this.vesselArrivalService.createNewVoyageDestination(tempModel).then(response => {
                    if (response.data == "DUPLICATE") {
                        alertify.error('Voyage No. telah terdaftar');
                        return;
                    }
                    alertify.success('Data tersimpan');
                    this.setPristine(form);
                }).catch(response => {
                    alertify.error('Gagal mengirim data ke database');
                });
            },
            () => { }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    fillJsonData() {
        this.jsonData['Voyage Number'] = this.voyageDestinationViewModel.voyageNumber;
        this.jsonData['Vendor'] = this.vesselArrivalViewModel.vendor;
        this.jsonData['Vessel'] = this.vesselArrivalViewModel.vessel;
        this.jsonData['Destination City'] = this.getCityName();
        this.jsonData['Estimated Arrival Time'] = moment(this.voyageDestinationViewModel.estimatedTimeArrival).format('DD-MMM-YYYY, HH:mm');
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin data ingin disimpan?";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data :";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data :";
        convertResult["grid"] = tempJson;
        return convertResult;
    }

    // ClearForm
    setPristine(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.clearData();
    }

    clearData() {
        this.voyageDestinationViewModel = new service.VoyageDestinationViewModel();
        this.voyageDestinationViewModel.voyageNumber = '';
        this.vesselArrivalViewModel = new service.VesselArrivalViewModel();
        this.arrivalDate = null;
        this.arrivalTime = new Date();
        this.voyageFound = false;
        this.search = null;
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

    getTotalItems(items: number) {
        if (this.voyageFound == false) {
            return 1;
        }
        return items;
    }
}

let VesselArrivalComponent = {
    template: require('./VesselArrival.html'),
    controller: VesselArrivalController,
    controllerAs: 'me'
}

export { VesselArrivalComponent }