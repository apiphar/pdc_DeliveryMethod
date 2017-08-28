import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as moment from 'moment';
import * as service from '../services/DeliveryShippingScheduleService';

class DeliveryShippingScheduleDestinationListController implements angular.IController {
    static $inject = ['DeliveryShippingScheduleService', '$rootScope'];

    deliveryShippingScheduleService: service.DeliveryShippingScheduleService;
    rootScope: angular.IRootScopeService;

    // Destination cities
    destinationCities: service.DeliveryShippingScheduleDestinationCityModel[] = new Array<service.DeliveryShippingScheduleDestinationCityModel>();
    deliveryShippingScheduleViewModel: service.DeliveryShippingScheduleViewModel;

    // Destination form model
    tempDestinationCities: service.DeliveryShippingScheduleVoyageDestinationCityModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
    tempDestinationCity: service.DeliveryShippingScheduleVoyageDestinationCityModel;
    destinationCityForm: service.DeliveryShippingScheduleDestinationCityFormModel = new service.DeliveryShippingScheduleDestinationCityFormModel;
    //destinationCity: service.DeliveryShippingScheduleDestinationCityModel;
    //estimatedArrival: Date;

    //// Property
    voyageDestinationCities: service.DeliveryShippingScheduleVoyageDestinationCityModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
    voyageDestinationCity: service.DeliveryShippingScheduleVoyageDestinationCityModel;
    voyageForm: service.DeliveryShippingScheduleVoyageFormModel = new service.DeliveryShippingScheduleVoyageFormModel;

    //// Voyage destination source locations property
    sourceLocations: service.DeliveryShippingScheduleLocationModel[];
    voyageDestinationsSourceLocations: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel[];
    voyageDestinationSourceLocations: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel[];
    voyageDestinationSourceLocationsView: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel[];

    // Toggle component visibility
    showDestinationCity: boolean = false;
    voyageNumber: string = null;
    destinationEdit: boolean = false;

    // Timepicker
    hourStep: number = 1;
    minuteStep: number = 1;

    // Pagination
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string = "locationName";
    totalItems: number;

    constructor(deliveryShippingScheduleService: service.DeliveryShippingScheduleService, rootScope: angular.IRootScopeService) {
        this.deliveryShippingScheduleService = deliveryShippingScheduleService;
        this.rootScope = rootScope;
    }

    $onInit() {
        this.rootScope.$on("showDestinationCity", (event, data) => {
            this.showDestinationCity = data;
        });
        
        this.rootScope.$on("voyageNumber", (event, data) => {
            this.voyageNumber = data;
        });

        this.rootScope.$on("sourceLocations", (event, data) => {
            this.sourceLocations = new Array<service.DeliveryShippingScheduleLocationModel>();
            this.sourceLocations = data;
        });

        this.rootScope.$on("deliveryShippingScheduleViewModel", (event, data) => {
            this.deliveryShippingScheduleViewModel = data;
        });
        
        this.rootScope.$on("destinationCities", (event, data) => {
            console.log(data);
            this.destinationCities = new Array<service.DeliveryShippingScheduleDestinationCityModel>();
            this.destinationCities = data;
        });

        this.rootScope.$on("voyageDestinationCities", (event, data) => {
            this.voyageDestinationCities = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
            this.voyageDestinationCities = data;
            this.tempDestinationCities = data;
            let self = this;
            angular.forEach(this.tempDestinationCities, function (value, key) {
                value.estimatedArrivalDateToString = moment(value.estimatedTimeOfArrival).format("D-MMM-YYYY").toString();
                value.estimatedArrivalTimeToString = moment(value.estimatedTimeOfArrival).format("HH:mm").toString();
            });
        });

        this.rootScope.$on("voyageForm", (event, data) => {
            this.voyageDestinationCities = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
            this.voyageForm = data;
            let self = this;
            angular.forEach(this.tempDestinationCities, function (value, key) {
                value.viewOnly = moment(self.voyageForm.departureDate).isSameOrBefore(value.estimatedTimeOfArrival);
            });
        });

        this.rootScope.$on("resetDestinationCityForm", (event, data) => {
            this.destinationCityForm = data;
        });

        this.rootScope.$on("closeDestinationCity", (event, data) => {
            this.showDestinationCity = data;
        });


        this.rootScope.$on("resetvoyageDestinationCities", (event, data) => {
            this.tempDestinationCities = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
            this.voyageDestinationCities = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
            this.destinationEdit = false;
        });

        this.destinationCityForm.destinationCity = new service.DeliveryShippingScheduleDestinationCityModel;

        //this.rootScope.$on("voyageDestinationsSourceLocations", (event, data) => {
        //    this.voyageDestinationsSourceLocations = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
        //    this.voyageDestinationsSourceLocations = data;
        //});
    }

    addDestinationCity(form: angular.IFormController) {
            let lastId = 1;
            if (this.tempDestinationCities[0] !== undefined) {
                lastId = _.maxBy(this.tempDestinationCities, function (o) { return o.tempVoyageNodeId; }).tempVoyageNodeId + 1;
            }
            this.tempDestinationCity = new service.DeliveryShippingScheduleVoyageDestinationCityModel();
            this.tempDestinationCity.voyageNodeId = 0;
            this.tempDestinationCity.tempVoyageNodeId = lastId;
            this.tempDestinationCity.cityForShipmentCode = this.destinationCityForm.destinationCity.cityForShipmentCode;
            this.tempDestinationCity.cityName = this.destinationCityForm.destinationCity.name;
            this.tempDestinationCity.estimatedTimeOfArrival = this.destinationCityForm.estimatedTimeOfArrival;
            this.tempDestinationCity.estimatedArrivalDateToString = moment(this.destinationCityForm.estimatedTimeOfArrival).format("D-MMM-YYYY").toString();
            this.tempDestinationCity.estimatedArrivalTimeToString = moment(this.destinationCityForm.estimatedTimeOfArrival).format("HH:mm").toString();
            this.tempDestinationCity.viewOnly = true;
            this.tempDestinationCities.push(this.tempDestinationCity);
        this.resetForm(form);

        this.rootScope.$broadcast("tempDestinationCities", this.tempDestinationCities);
    }

    editDestinationCity(data: service.DeliveryShippingScheduleVoyageDestinationCityModel) {
        console.log(data);
        this.tempDestinationCity = new service.DeliveryShippingScheduleVoyageDestinationCityModel();
        this.destinationEdit = true;
        this.destinationCityForm.destinationCity.cityForShipmentCode = data.cityForShipmentCode;
        this.destinationCityForm.destinationCity.name = data.cityName;
        this.destinationCityForm.estimatedTimeOfArrival = new Date(moment(data.estimatedTimeOfArrival).format("D-MMM-YYYY HH:mm"));
        this.tempDestinationCity.estimatedArrivalDateToString = moment(data.estimatedTimeOfArrival).format("D-MMM-YYYY").toString();
        this.tempDestinationCity.estimatedArrivalTimeToString = moment(data.estimatedTimeOfArrival).format("HH:mm").toString();
        this.tempDestinationCity = new service.DeliveryShippingScheduleVoyageDestinationCityModel();
        this.tempDestinationCity.voyageNodeId = data.voyageNodeId;
        this.tempDestinationCity.tempVoyageNodeId = data.tempVoyageNodeId;
    }

    deleteDestinationCity(data: service.DeliveryShippingScheduleVoyageDestinationCityModel) {
        if (data.tempVoyageNodeId !== 0 && data.tempVoyageNodeId !== null && data.tempVoyageNodeId !== undefined) {
            _.remove(this.tempDestinationCities, function (o) { return o.tempVoyageNodeId == data.tempVoyageNodeId });
        }
        else {
            _.remove(this.tempDestinationCities, function (o) { return o.voyageNodeId == data.voyageNodeId });
        }
    }

    updateDestinationCity(form: angular.IFormController) {
            let self = this;
            angular.forEach(this.tempDestinationCities, function (value, key) {
                console.log(value.voyageNodeId, self.tempDestinationCity.voyageNodeId);
                if (value.tempVoyageNodeId === self.tempDestinationCity.tempVoyageNodeId && self.tempDestinationCity.tempVoyageNodeId !== 0 && self.tempDestinationCity.tempVoyageNodeId !== undefined && self.tempDestinationCity.tempVoyageNodeId !== null) {
                    value.cityForShipmentCode = self.destinationCityForm.destinationCity.cityForShipmentCode;
                    value.cityName = self.destinationCityForm.destinationCity.name;
                    value.estimatedTimeOfArrival = self.destinationCityForm.estimatedTimeOfArrival;
                    value.estimatedArrivalDateToString = moment(self.destinationCityForm.estimatedTimeOfArrival).format("D-MMM-YYYY").toString();
                    value.estimatedArrivalTimeToString = moment(self.destinationCityForm.estimatedTimeOfArrival).format("HH:mm").toString();
                }
                else if (value.voyageNodeId === self.tempDestinationCity.voyageNodeId) {
                    value.cityForShipmentCode = self.destinationCityForm.destinationCity.cityForShipmentCode;
                    value.cityName = self.destinationCityForm.destinationCity.name;
                    value.estimatedTimeOfArrival = self.destinationCityForm.estimatedTimeOfArrival;
                    value.estimatedArrivalDateToString = moment(self.destinationCityForm.estimatedTimeOfArrival).format("D-MMM-YYYY").toString();
                    value.estimatedArrivalTimeToString = moment(self.destinationCityForm.estimatedTimeOfArrival).format("HH:mm").toString();
                }
            });
        this.resetForm(form);

        this.rootScope.$broadcast("tempDestinationCities", this.tempDestinationCities);
    }

    resetForm(form: angular.IFormController) {
        
        this.destinationCityForm.destinationCity = new service.DeliveryShippingScheduleDestinationCityModel;
        this.destinationCityForm.estimatedTimeOfArrival = undefined;
        this.destinationEdit = false;
        form.$setPristine();
        form.$setUntouched();
    }

    showSourceLocationPanel(tempVoyageNodeId: number, voyageNodeId: number) {
        this.voyageDestinationSourceLocationsView = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
        this.rootScope.$broadcast("showSourceLocation", true);
        this.rootScope.$broadcast("tempVoyageNodeId", tempVoyageNodeId);
        this.rootScope.$broadcast("voyageNodeId", voyageNodeId);
        this.rootScope.$broadcast("voyageDestinationSourceLocations", this.deliveryShippingScheduleViewModel.voyagesDestinationsSourceLocations);
        this.rootScope.$broadcast("deliveryShippingScheduleViewModel", this.deliveryShippingScheduleViewModel);
        this.rootScope.$broadcast("voyageNumber", this.voyageNumber);
        this.rootScope.$broadcast("sourceLocations", this.sourceLocations);
        this.rootScope.$broadcast("tempDestinationCities", this.tempDestinationCities);
        if (voyageNodeId !== 0 && voyageNodeId !== null && voyageNodeId !== undefined) {
            this.voyageDestinationSourceLocationsView = _.filter(this.deliveryShippingScheduleViewModel.voyagesDestinationsSourceLocations, ["voyageNodeId", voyageNodeId]);
            this.rootScope.$broadcast("voyageDestinationSourceLocationsView", this.voyageDestinationSourceLocationsView);
        }
        else {
            this.voyageDestinationSourceLocationsView = _.filter(this.deliveryShippingScheduleViewModel.voyagesDestinationsSourceLocations, ["tempVoyageNodeId", tempVoyageNodeId]);
            this.rootScope.$broadcast("voyageDestinationSourceLocationsView", this.voyageDestinationSourceLocationsView);
        }
    }

    checkDestinationCity() {
        if (_.find(this.tempDestinationCities, ['cityForShipmentCode', this.destinationCityForm.destinationCity.cityForShipmentCode]) !== undefined) {
            this.destinationCityForm.errorMessageDestinationCity = "Destination City tidak boleh sama !";
            this.destinationCityForm.validateForm = true;
        }
        else {
            this.destinationCityForm.errorMessageDestinationCity = undefined;
            this.destinationCityForm.validateForm = false;
        }
    }

    checkEstimatedArrival() {
        if (moment(this.destinationCityForm.estimatedTimeOfArrival).isSameOrBefore(this.voyageForm.departureDate)) {
            this.destinationCityForm.errorMessageestimatedTimeOfArrival = "Estimation Time Arrival tidak boleh lebih kecil dari tanggal hari ini";
            this.destinationCityForm.validateForm = true;
        }
        else {
            this.destinationCityForm.errorMessageestimatedTimeOfArrival = undefined;
            this.destinationCityForm.validateForm = false;
        }
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
}

let DeliveryShippingScheduleDestinationList = {
    controller: DeliveryShippingScheduleDestinationListController,
    controllerAs: "me",
    template: require('./DeliveryShippingScheduleDestinationList.html')
}

export { DeliveryShippingScheduleDestinationList }