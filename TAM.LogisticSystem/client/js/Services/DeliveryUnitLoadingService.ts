



export class DeliveryUnitLoadingService {
    static $inject = ['$http'];
    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    httpService: angular.IHttpService;    

    // get All Voyage Data
    getUnitLoadingModels() {
        return this.httpService.get<UnitLoadingModel[]>('/api/v1/DeliveryUnitLoadingApi/GetUnitLoadingModels');
    }

    //get all frame number based in Voyage number
    getFrameNumbers(voyageNumber: string) {
        return this.httpService.get<InputFrameNumber[]>('/api/v1/DeliveryUnitLoadingApi/GetFrameNumbers/' + voyageNumber);
    }

    //update status Frame Number based on Voyage Number to Loaded
    updateDataLoaded(updateFrameNumber: UpdateFrameNumber) {
        return this.httpService.put('/api/v1/DeliveryUnitLoadingApi/UpdateFrameNumber', updateFrameNumber);
    }
}

export class UnitLoadingModel {
    capacity: number;
    voyageNumber: string; 
    vessel: string;
    vendor: string;
    estimationDeparture: Date;
    totalUnitAssign: number;
    totalUnitPreBookedInPorted: number;
    totalUnitPreBookedNotPorted: number;
}

export class InputFrameNumber {
    frameNumber: string;
    vehicleId: number;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    customerAssign: boolean;
    requestedDeliveryTime: Date;
    status: string;
    dateTemp: string;
    estimatedPDCIn: Date;
    dateTempEPDC: string;
}

export class SearchDataPreview {
    frameNumber: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    customerAssign: boolean;
    requestedDeliveryTime: Date;
    status: string;
    dateTemp: string;
    dateTempEPDC: string;
}

export class UpdateFrameNumber {
    vehicleId: number[];
    voyageNumber: string;
}


