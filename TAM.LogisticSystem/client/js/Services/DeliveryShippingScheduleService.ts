import * as angular from 'angular';

export class DeliveryShippingScheduleService {
    static $inject = ['$http'];

    http: angular.IHttpService;

    constructor(http: angular.IHttpService) {
        this.http = http;
    }

    init() {
        return this.http.get<DeliveryShippingScheduleViewModel>("api/v1/DeliveryShippingScheduleApi");
    }

    save(saveModel: DeliveryShippingScheduleSaveModel) {
        return this.http.post("api/v1/DeliveryShippingScheduleApi", saveModel);
    }

    saveDestination(voyageNumber: string, destinationCity: string, estimatedTimeArrivalDate: Date) {
        return this.http.post("api/v1/DeliveryShippingScheduleApi/voyage-destination/create", {voyageNumber,destinationCity,estimatedTimeArrivalDate});
    }

    updateDestination(voyageDestinationId : number,voyageNumber: string, destinationCity: string, estimatedTimeArrivalDate: Date) {
        return this.http.post("api/v1/DeliveryShippingScheduleApi/voyage-destination/edit/" + voyageDestinationId, { voyageNumber, destinationCity, estimatedTimeArrivalDate });
    }

    deleteDestination(voyageDestinationId: number) {
        return this.http.delete("api/v1/DeliveryShippingScheduleApi/voyage-destination/delete/" + voyageDestinationId);
    }

    deleteSourceLocation() {

    }
}

export class DeliveryShippingScheduleViewModel {
    vendors: DeliveryShippingScheduleVendorModel[];
    vessels: DeliveryShippingScheduleVesselModel[];
    ports: DeliveryShippingScheduleLocationModel[];
    destinationCities: DeliveryShippingScheduleDestinationCityModel[];
    sourceLocations: DeliveryShippingScheduleLocationModel[];
    voyages: DeliveryShippingScheduleVoyageModel[];
    voyagesDestinationCities: DeliveryShippingScheduleVoyageDestinationCityModel[];
    voyagesDestinationsSourceLocations: DeliveryShippingScheduleVoyageDestinationSourceLocationModel[];
}

export class DeliveryShippingScheduleVendorModel {
    deliveryVendorCode: string;
    deliveryVendorName: string;
}

export class DeliveryShippingScheduleVesselModel {
    deliveryVendorCode: string;
    deliveryVendorVehicleId: number;
    policeNumberOrVesselName: string;
}

export class DeliveryShippingScheduleLocationModel {
    locationCode: string;
    locationName: string;
}

export class DeliveryShippingScheduleDestinationCityModel {
    cityForShipmentCode: string;
    name: string;
}

export class DeliveryShippingScheduleVoyageModel {
    voyageNumber: string;
    deliveryVendorCode: string;
    deliveryVendorName: string;
    deliveryVendorVehicleId: number;
    policeNumberOrVesselName: string;
    departureLocationCode: string;
    departureLocationName: string;
    departureDate: Date;
}

export class DeliveryShippingScheduleVoyageDestinationCityModel {
    voyageNumber: string;
    voyageNodeId: number;
    tempVoyageNodeId: number;
    cityForShipmentCode: string;
    cityName: string;
    estimatedTimeOfArrival: Date;
    estimatedArrivalDateToString: string;
    estimatedArrivalTimeToString: string;
    viewOnly: boolean;
}

export class DeliveryShippingScheduleVoyageDestinationSourceLocationModel {
    voyageNodeId: number;
    tempVoyageNodeId: number;
    voyageNodeSourceId: number;
    tempVoyageNodeSourceId: number;
    locationCode: string;
    locationName: string;
    capacity: number;
}

export class DeliveryShippingScheduleVoyageFormModel {
    voyageNumber: string;
    vendor: DeliveryShippingScheduleVendorModel;
    vessel: DeliveryShippingScheduleVesselModel;
    port: DeliveryShippingScheduleLocationModel;
    departureDate: Date;
    errorMessage: string;
    validateDepartureDate: boolean;
    showAddButton: boolean = true;
}

export class DeliveryShippingScheduleDestinationCityFormModel {
    destinationCity: DeliveryShippingScheduleDestinationCityModel;
    estimatedTimeOfArrival: Date;
    errorMessageestimatedTimeOfArrival: string;
    errorMessageDestinationCity: string;
    validateForm: boolean;
}

export class DeliveryShippingScheduleSourceLocationFormModel {
    sourceLocation: DeliveryShippingScheduleLocationModel;
    capacity: number;
    errorMessageSourceLocation: string;
    validateForm: boolean;
}

export class DeliveryShippingScheduleSaveModel {
    voyageForm: DeliveryShippingScheduleVoyageFormModel;
    tempDestinationCities: DeliveryShippingScheduleVoyageDestinationCityModel[];
    tempSourceLocations: DeliveryShippingScheduleVoyageDestinationSourceLocationModel[];
}