
export class MasterPlafondService {
    static $inject = ["$http"];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    httpService: angular.IHttpService;

    //to get all plafond data
    getAllPlafondData() {
        return this.httpService.get<MasterPlafondViewModel[]>("/api/v1/MasterPlafondAPI");
    }
    //untuk mendapatkan company code yang belum di insert ke table PlafondMaster
    getCompanyCode() {
        return this.httpService.get<CodeCompanyViewModel[]>("api/v1/MasterPlafondAPI/GetCompanyKode");
    }
    //to insert one plafond data
    postPlafondData(kodeCompany: string, plafond: number) {
        return this.httpService.post("/api/v1/MasterPlafondAPI/", { kodeCompany, plafond });
    }
    //to delete one plafond data
    deletePlafondData(plafondMasterId: number) {
        return this.httpService.delete("/api/v1/MasterPlafondAPI/" + plafondMasterId);
    }
    //to update one plafond data
    updatePlafondData(plafondMasterId: number, plafond: number) {
        return this.httpService.post("api/v1/MasterPlafondAPI/" + plafondMasterId, plafond);
    }

}

export class MasterPlafondViewModel {
    plafondMasterId: number;
    kodeCompany: string;
    plafond: number;
}

export class CodeCompanyViewModel {
    kodeCompany: string;
    name: string;
}