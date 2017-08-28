import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as moment from 'moment';
import * as service from '../services/DeliveryShippingScheduleService';

class DeliveryShippingScheduleDestinationFormController implements angular.IController {
    static $inject = ['DeliveryShippingScheduleService', '$rootScope'];

    deliveryShippingScheduleService: service.DeliveryShippingScheduleService;
    rootScope: angular.IRootScopeService;

    deliveryShippingScheduleViewModel: service.DeliveryShippingScheduleViewModel;

    resetPristineVoyageForm: angular.IFormController;
    voyageForm: service.DeliveryShippingScheduleVoyageFormModel = new service.DeliveryShippingScheduleVoyageFormModel();
    voyageDestinationCitiesForm: service.DeliveryShippingScheduleVoyageDestinationCityModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();

    foundVoyage: service.DeliveryShippingScheduleVoyageModel;
    currentDate: Date = moment().toDate();
    // Timepicker
    hourStep: number = 1;
    minuteStep: number = 1;

    constructor(deliveryShippingScheduleService: service.DeliveryShippingScheduleService, rootScope: angular.IRootScopeService) {
        this.deliveryShippingScheduleService = deliveryShippingScheduleService;
        this.rootScope = rootScope;
    }

    $onInit() {
        this.rootScope.$on("resetVoyageForm", (event, data) => {
            this.voyageForm = new service.DeliveryShippingScheduleVoyageFormModel();
            this.voyageForm.validateDepartureDate = false;
            this.voyageForm.showAddButton = true;
            this.resetPristineVoyageForm.$setPristine();
            this.resetPristineVoyageForm.$setUntouched();
            this.voyageDestinationCitiesForm = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
            this.init();
        });
        this.voyageForm.validateDepartureDate = false;

        this.init();
    }

    init() {
        // Get all needed data
        this.deliveryShippingScheduleViewModel = new service.DeliveryShippingScheduleViewModel();
        this.deliveryShippingScheduleService.init().then(response => {
            this.deliveryShippingScheduleViewModel = response.data as service.DeliveryShippingScheduleViewModel;

            // Send data to other component
            this.rootScope.$broadcast("destinationCities", this.deliveryShippingScheduleViewModel.destinationCities);
            this.rootScope.$broadcast("sourceLocations", this.deliveryShippingScheduleViewModel.sourceLocations);
            this.rootScope.$broadcast("voyageDestinationCities", this.deliveryShippingScheduleViewModel.voyagesDestinationCities);
            this.rootScope.$broadcast("voyageDestinationsSourceLocations", this.deliveryShippingScheduleViewModel.voyagesDestinationsSourceLocations);
            this.rootScope.$broadcast("existVoyage", false);
            console.log(this.deliveryShippingScheduleViewModel);
        });
    }

    searchVoyage(form: angular.IFormController) {
        this.foundVoyage = new service.DeliveryShippingScheduleVoyageModel();
        this.foundVoyage = _.find(this.deliveryShippingScheduleViewModel.voyages, ["voyageNumber", this.voyageForm.voyageNumber]);
        let voyageFormVoyageNumber = this.voyageForm.voyageNumber;
        if (this.foundVoyage !== undefined) {
            this.voyageForm.showAddButton = false;
            this.rootScope.$broadcast("voyageNumber", this.voyageForm.voyageNumber);
            this.voyageDestinationCitiesForm = _.filter(this.deliveryShippingScheduleViewModel.voyagesDestinationCities, ["voyageNumber", this.voyageForm.voyageNumber]);
            this.voyageForm.vendor = _.find(this.deliveryShippingScheduleViewModel.vendors, ["deliveryVendorCode", this.foundVoyage.deliveryVendorCode]);
            this.voyageForm.vessel = _.find(this.deliveryShippingScheduleViewModel.vessels, ["deliveryVendorVehicleId", this.foundVoyage.deliveryVendorVehicleId]);
            this.voyageForm.port = _.find(this.deliveryShippingScheduleViewModel.ports, ["locationCode", this.foundVoyage.departureLocationCode]);
            this.voyageForm.departureDate = new Date(this.foundVoyage.departureDate);
            this.showDestinationCityPanel();
        }
        else {
            this.reset();
            this.voyageForm.voyageNumber = voyageFormVoyageNumber;
        }
        this.resetPristineVoyageForm = form;
        this.broadcastForm();
    }

    resetForm(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.voyageForm = new service.DeliveryShippingScheduleVoyageFormModel();;
    }

    reset() {
        this.rootScope.$broadcast("resetvoyageSourceLocations", new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>());
        this.rootScope.$broadcast("resetvoyageDestinationCities", new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>());
        this.rootScope.$broadcast("closeDestinationCity", false);
        this.rootScope.$broadcast("resetVoyageForm", new service.DeliveryShippingScheduleVoyageFormModel());
        this.rootScope.$broadcast("resetDestinationCityForm", new service.DeliveryShippingScheduleDestinationCityFormModel());
    }

    checkEstimatedDeparture() {
        this.currentDate = moment().toDate();
        if (moment(this.voyageForm.departureDate).isSameOrBefore(this.currentDate)) {
            this.voyageForm.errorMessage = "Estimation Time Departure tidak boleh lebih kecil dari tanggal hari ini";
            this.voyageForm.validateDepartureDate = true;
        }
        else {
            this.voyageForm.errorMessage = undefined;
            this.voyageForm.validateDepartureDate = false;
        }
    }

    showDestinationCityPanel() {
        this.voyageForm.showAddButton = false;
        this.rootScope.$broadcast("showDestinationCity", true);
        this.rootScope.$broadcast("voyageDestinationCities", this.voyageDestinationCitiesForm);
        this.rootScope.$broadcast("deliveryShippingScheduleViewModel", this.deliveryShippingScheduleViewModel);
        this.rootScope.$broadcast("sourceLocations", this.deliveryShippingScheduleViewModel.sourceLocations);
        this.broadcastForm();
    }

    broadcastForm() {
        this.rootScope.$broadcast("voyageForm", this.voyageForm);
    }
}

let DeliveryShippingScheduleDestinationForm = {
    controller: DeliveryShippingScheduleDestinationFormController,
    controllerAs: "me",
    template: require('./DeliveryShippingScheduleDestinationForm.html')
}

export { DeliveryShippingScheduleDestinationForm }