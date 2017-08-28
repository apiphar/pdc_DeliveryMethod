export class VesselDepartPageViewModel {
    viewModels: VesselDepartViewModel[];
    unitLists: UnitListViewModel[];
}

export class VesselDepartViewModel {
    voyageNumber: string;
    vendor: string;
    vessel: string;
    estimatedTimeDeparture: Date;
    capacity: number;
    loaded: number;
    assigned: number;
    preBookPorted: number;
    preBookNotPorted: number;
    voyageStatus: string;
    unitListId: number;
    totalUnit: number;
}

export class VesselDepartSendViewModel {
    voyageNumber: string;
    unitListId: number;
}

export class UnitListViewModel {
    frameNo: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    pdcIn: string;
    pdcInString: string;
    customerAssign: boolean;
    requestedPdd: Date;
    requestedPddString: string;
    status: string;
    voyageNumber: string;
}

export class VesselDepartService {
    static $inject = ['$http'];

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }
    HttpClient: angular.IHttpService;

    //Get all on load
    getAll() {
        return this.HttpClient.get<VesselDepartPageViewModel>('../api/v1/VesselDepartApi');
    }

    // Get all unit data on load
    getUnitList(voyageNumber: string) {
        return this.HttpClient.get<UnitListViewModel[]>('../api/v1/VesselDepartApi/UnitList/' + voyageNumber);
    }

    // Update data on server
    departVoyage(vessel: VesselDepartSendViewModel) {
        return this.HttpClient.put('../api/v1/VesselDepartApi/DepartVessel', vessel);
    }
}