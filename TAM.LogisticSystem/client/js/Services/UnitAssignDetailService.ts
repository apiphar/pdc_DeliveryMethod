import * as angular from 'angular';



export class UnitAssignDetailService {
    static $inject = ['$http'];

    constructor(httpService: angular.IHttpService) {
        this.HttpService = httpService;
    }

    //private readonly
    HttpService: angular.IHttpService;


    getUnitList(voyage: string) {
        return this.HttpService.get<UnitListByVoyage[]>("/api/v1/UnitAssignApi/ViewDetail/" + voyage);
    }

}

export class UnitListByVoyage {
    frameNumber: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    customerAssign: boolean;
    requestedPDD: Date;
    location: string;
    status: string;
    requestedPDDString: string;
    voyage: string;
    customerAssignModel: string;
    etd: Date;
    etdString: string;
}











