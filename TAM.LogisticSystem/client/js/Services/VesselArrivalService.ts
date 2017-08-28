

export class VesselArrivalPageViewModel {
    unitLists: VesselArrivalUnitList[];
    cityLists: CityList[];
    viewModels: VesselArrivalViewModel[];
}

export class VesselArrivalViewModel {
    voyageNumber: string;
    vendor: string;
    vessel: string;
}

export class VoyageDestinationViewModel {
    voyageNumber: string;
    destinationCity: string;
    estimatedTimeArrival: Date;
}

export class CityList {
    cityId: number;
    cityName: string;
}

export class VesselArrivalUnitList {
    frameNo: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    pdcIn: Date;
    pdcInString;
    customerAssign: boolean;
    requestedPdd: Date;
    requestedPddString;
    voyageNumber: string;
}

export class VesselArrivalService {
    static $inject = ['$http'];

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }
    HttpClient: angular.IHttpService;

    // Get all
    GetAll() {
        return this.HttpClient.get<VesselArrivalPageViewModel>('../api/v1/VesselArrivalApi');
    }

    // POST data
    createNewVoyageDestination(voyageDestinationViewModel: VoyageDestinationViewModel) {
        return this.HttpClient.post('../api/v1/VesselArrivalApi', voyageDestinationViewModel);
    }
}