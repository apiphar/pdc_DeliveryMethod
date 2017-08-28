import * as angular from "angular";

export class ManufacturingService{
    static $inject = ["$http"];

    HttpService: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpService = $http;
    }

    getAll(){
        return this.HttpService.get<ManufacturingViewModel[]>("/api/v1/ManufacturingApi");
    }

    updateData(data: ManufacturingViewModel) {
        return this.HttpService.post<any>("/api/v1/ManufacturingApi/Update",data);
    }

    save(data: ManufacturingViewModel) {
        return this.HttpService.post<any>("/api/v1/ManufacturingApi", data);
    }

    delete(data: ManufacturingViewModel) {
        return this.HttpService.delete("/api/v1/ManufacturingApi/" + data.plantCode);
    }
}

export class ManufacturingViewModel {
    country: string;
    name: string;
    plantCode: string;
}



