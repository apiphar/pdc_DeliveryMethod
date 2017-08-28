import * as angular from 'angular';

export class UnitAssignService {
    static $inject = ['$http'];

    //private readonly
    HttpService: angular.IHttpService;

    constructor(httpService: angular.IHttpService) {
        this.HttpService = httpService;
    }

    getAllVoyage(data: string) {
        return this.HttpService.get<UnitAssignDataModel>("/api/v1/UnitAssignApi/GetAllVoyage/" + data);
    }

    saveData(data: UnitAssignDataModel) {
        return this.HttpService.post<any>("/api/v1/UnitAssignApi", data);
    }

}


export class UnitAssignDataModel {
    allUnit: UnitAssignUnitListModel[];
    allVoyage: UnitAssignVoyageModel;
}

export class UnitAssignUnitListModel {
    frameNumber: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    customerAssign: boolean;
    requestedPDD: Date;
    statusId: number;
    vehicleId: number;
    requestedPDDString: string;
    customerAssignModel: string;
    voyage: string;
    status: string;
    etd: Date;
    etdString: string;
    VoyageNodeSourceId: number;
}

export class UnitAssignVoyageModel {
    CapacityVessel: number;
    Voyage: string;
    Vendor: string;
    Vessel: string;
    EstimationTimeDeparture: Date;
    TotalUnitAssign: number;
    TotalUnitPreBookedInPorted: number;
    TotalUnitPreBookedNotPorted: number;
    TotalAll: number;
}







