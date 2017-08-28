
export class CityMasterService {
    static $inject = ['$http']; 

    httpService: angular.IHttpService;

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }
    //to get all master city data
    getAllData() {
        return this.httpService.get<CityMasterViewModel[]>("api/v1/CityMasterAPI");
    }
    //to delete one master city data
    deleteData(cityCode: string) {
        return this.httpService.delete("api/v1/CityMasterAPI/" + cityCode);
    }
    //to insert one master city data
    createData(cityCode : string, name: string) {
        return this.httpService.post("api/v1/CityMasterAPI", { cityCode, name });
    }
    //to update one master city data
    updateData(cityCode: string, name: string) {
        return this.httpService.post("api/v1/CityMasterAPI/" + cityCode, { cityCode, name });
    }
}

export class CityMasterViewModel {
    cityCode: string;
    name: string;
}