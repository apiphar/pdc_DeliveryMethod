import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as service from '../services/DeliveryShippingScheduleService';

class DeliveryShippingScheduleSourceLocationController implements angular.IController {
    static $inject = ['DeliveryShippingScheduleService', '$rootScope'];

    deliveryShippingScheduleService: service.DeliveryShippingScheduleService;
    rootScope: angular.IRootScopeService;

    sourceLocations: service.DeliveryShippingScheduleLocationModel[];
    voyageDestinationSourceLocations: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
    voyageDestinationSourceLocation: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel;

    tempSourceLocations: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
    tempSourceLocation: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel;
    tempVoyageNodeId: number;
    voyageNodeId: number;
    sourceLocationForm: service.DeliveryShippingScheduleSourceLocationFormModel = new service.DeliveryShippingScheduleSourceLocationFormModel();
    tempSourceLocationsView: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();

    voyageForm: service.DeliveryShippingScheduleVoyageFormModel = new service.DeliveryShippingScheduleVoyageFormModel();
    tempDestinationCities: service.DeliveryShippingScheduleVoyageDestinationCityModel[] = new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>();
    resetVoyageForm: service.DeliveryShippingScheduleVoyageFormModel = new service.DeliveryShippingScheduleVoyageFormModel();
    resetDestinationCityForm: service.DeliveryShippingScheduleDestinationCityFormModel = new service.DeliveryShippingScheduleDestinationCityFormModel();
    resetSourceLocationForm: service.DeliveryShippingScheduleSourceLocationFormModel = new service.DeliveryShippingScheduleSourceLocationFormModel();

    saveModel: service.DeliveryShippingScheduleSaveModel;

    // Toggle component visibility
    showSourceLocation: boolean = false;
    sourceLocationEdit: boolean = false;

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
        this.sourceLocationForm.sourceLocation = new service.DeliveryShippingScheduleLocationModel;
        this.rootScope.$on("showSourceLocation", (event, data) => {
            this.showSourceLocation = data;
        });

        this.rootScope.$on("sourceLocations", (event, data) => {
            this.sourceLocations = new Array<service.DeliveryShippingScheduleLocationModel>();
            this.sourceLocations = data;
        });

        this.rootScope.$on("voyageDestinationSourceLocations", (event, data) => {
            this.tempSourceLocations = data;
        });

        this.rootScope.$on("voyageDestinationSourceLocationsView", (event, data) => {
            console.log(data);
            this.tempSourceLocationsView = data;
        });

        this.rootScope.$on("tempVoyageNodeId", (event, data) => {
            this.tempVoyageNodeId = data;
        });

        this.rootScope.$on("voyageNodeId", (event, data) => {
            this.voyageNodeId = data;
        });

        this.rootScope.$on("voyageForm", (event, data) => {
            this.voyageForm = data;
        });

        this.rootScope.$on("tempDestinationCities", (event, data) => {
            this.tempDestinationCities = data;
        });

        this.rootScope.$on("resetvoyageSourceLocations", (event, data) => {
            this.sourceLocationForm = this.resetSourceLocationForm;
            this.tempSourceLocations = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
            this.tempSourceLocationsView = new Array<service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel>();
            this.showSourceLocation = false;
        });

    }

    addSourceLocation(form: angular.IFormController) {
        let lastId = 1;
        if (this.tempSourceLocations[0] !== undefined) {
            lastId = _.maxBy(this.tempSourceLocations, function (o) { return o.tempVoyageNodeSourceId; }).tempVoyageNodeSourceId + 1;
        }
        this.tempSourceLocation = new service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel();
        this.tempSourceLocation.tempVoyageNodeId = this.tempVoyageNodeId;
        this.tempSourceLocation.voyageNodeId = this.voyageNodeId;
        this.tempSourceLocation.tempVoyageNodeSourceId = lastId;
        this.tempSourceLocation.voyageNodeSourceId = 0;
        this.tempSourceLocation.locationCode = this.sourceLocationForm.sourceLocation.locationCode;
        this.tempSourceLocation.locationName = this.sourceLocationForm.sourceLocation.locationName;
        this.tempSourceLocation.capacity = this.sourceLocationForm.capacity;
        this.tempSourceLocations.push(this.tempSourceLocation);
        console.log(this.tempSourceLocations);
        if (this.voyageNodeId !== 0 && this.voyageNodeId !== null && this.voyageNodeId !== undefined) {
            this.tempSourceLocationsView = _.filter(this.tempSourceLocations, ["voyageNodeId", this.voyageNodeId]);
        }
        else {
            this.tempSourceLocationsView = _.filter(this.tempSourceLocations, ["tempVoyageNodeId", this.tempVoyageNodeId]);
        }

        this.resetForm(form);

        this.rootScope.$broadcast("tempSourceLocations", this.tempSourceLocations);
    }

    save(form: angular.IFormController) {
        this.saveModel = new service.DeliveryShippingScheduleSaveModel();
        this.saveModel.voyageForm = this.voyageForm;
        this.saveModel.tempDestinationCities = this.tempDestinationCities;
        this.saveModel.tempSourceLocations = this.tempSourceLocations;

        alertify.confirm("Konfirmasi", "Apakah anda yakin untuk menambahkan data", () => {
            this.deliveryShippingScheduleService.save(this.saveModel).then(response => {
                alertify.success("Data tersimpan");
                this.resetForm(form);
                this.reset();
            }).catch(response => {
                alertify.error("Data gagal disimpan");
            });
        }, () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        
    }

    editSourceLocation(data: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel) {
        this.sourceLocationEdit = true;
        this.tempSourceLocation = new service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel();
        this.sourceLocationForm.sourceLocation = new service.DeliveryShippingScheduleLocationModel;
        this.sourceLocationForm.sourceLocation.locationCode = data.locationCode;
        this.sourceLocationForm.sourceLocation.locationName = data.locationName;
        this.tempSourceLocation.tempVoyageNodeId = data.tempVoyageNodeId;
        this.tempSourceLocation.voyageNodeId = data.voyageNodeId;

        this.tempSourceLocation.tempVoyageNodeSourceId = data.tempVoyageNodeSourceId;
        this.tempSourceLocation.voyageNodeSourceId = data.voyageNodeSourceId;
        this.sourceLocationForm.capacity = data.capacity;
    }

    resetForm(form: angular.IFormController) {
        this.sourceLocationEdit = false;
        this.sourceLocationForm.sourceLocation = new service.DeliveryShippingScheduleLocationModel;
        this.sourceLocationForm.capacity = undefined;
        form.$setPristine();
        form.$setUntouched();
    }

    updateSourceLocation(form: angular.IFormController) {
            let self = this;
            angular.forEach(this.tempSourceLocations, function (value, key) {
                if (value.tempVoyageNodeSourceId === self.tempSourceLocation.tempVoyageNodeSourceId && self.tempSourceLocation.tempVoyageNodeSourceId !== 0 && self.tempSourceLocation.tempVoyageNodeSourceId !== undefined && self.tempSourceLocation.tempVoyageNodeSourceId !== null) {
                    value.locationCode = self.sourceLocationForm.sourceLocation.locationCode;
                    value.locationName = self.sourceLocationForm.sourceLocation.locationName;
                    value.capacity = self.sourceLocationForm.capacity;
                }
                else if (value.voyageNodeSourceId === self.tempSourceLocation.voyageNodeSourceId) {
                    value.locationCode = self.sourceLocationForm.sourceLocation.locationCode;
                    value.locationName = self.sourceLocationForm.sourceLocation.locationName;
                    value.capacity = self.sourceLocationForm.capacity;
                }
            });
        this.resetForm(form);
    }

    reset() {
        this.rootScope.$broadcast("resetvoyageSourceLocations", new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>());
        this.rootScope.$broadcast("resetvoyageDestinationCities", new Array<service.DeliveryShippingScheduleVoyageDestinationCityModel>());
        this.rootScope.$broadcast("closeDestinationCity", false);
        this.rootScope.$broadcast("resetVoyageForm", this.resetVoyageForm);
        this.rootScope.$broadcast("resetDestinationCityForm", this.resetDestinationCityForm);
    }

    deleteSourceLocation(data: service.DeliveryShippingScheduleVoyageDestinationSourceLocationModel) {
        if (data.tempVoyageNodeSourceId !== 0 && data.tempVoyageNodeSourceId !== null && data.tempVoyageNodeSourceId !== undefined) {
            _.remove(this.tempSourceLocations, function (o) { return o.tempVoyageNodeSourceId == data.tempVoyageNodeSourceId });
            _.remove(this.tempSourceLocationsView, function (o) { return o.tempVoyageNodeSourceId == data.tempVoyageNodeSourceId });
        }
        else {
            _.remove(this.tempSourceLocations, function (o) { return o.voyageNodeSourceId == data.voyageNodeSourceId });
            _.remove(this.tempSourceLocationsView, function (o) { return o.voyageNodeSourceId == data.voyageNodeSourceId });
        }
    }

    checkSourceLocation() {
        if (_.find(this.tempSourceLocationsView, ['locationCode', this.sourceLocationForm.sourceLocation.locationCode]) !== undefined) {
            this.sourceLocationForm.errorMessageSourceLocation = "Source Location tidak boleh sama !";
            this.sourceLocationForm.validateForm = true;
        }
        else {
            this.sourceLocationForm.errorMessageSourceLocation = undefined;
            this.sourceLocationForm.validateForm = false;
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

let DeliveryShippingScheduleSourceLocation = {
    controller: DeliveryShippingScheduleSourceLocationController,
    controllerAs: "me",
    template: require('./DeliveryShippingScheduleSourceLocation.html')
}

export { DeliveryShippingScheduleSourceLocation }