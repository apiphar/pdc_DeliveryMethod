

export class MasterModelService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get<string>('/mastermodel/GetDataMasterModel');
    }

    GetDropdownBrand() {
        return this.HttpClient.get<string>('/mastermodel/GetDropdownBrand');
    }

    GetDropdownManufacturing() {
        return this.HttpClient.get<string>('/mastermodel/GetDropdownManufacturing');
    }

    PostData(MasterModelId: String, Name: String, BrandCode: string, PlantCode: string) {
        return this.HttpClient.post('/mastermodel/create', { MasterModelId,Name, BrandCode, PlantCode });
    }

    UpdateData(MasterModelId: String, Name: String, BrandCode: string, PlantCode: string  ) {
        return this.HttpClient.post('/mastermodel/edit/' + MasterModelId, { MasterModelId, Name, BrandCode,PlantCode });
    }

    DeleteData(MasterModelId: String) {
        return this.HttpClient.delete('/mastermodel/delete/' + MasterModelId, {});
    }

    CekPola(MasterModelId: string) {
        return this.HttpClient.get<number>('/mastermodel/cekpola/' + MasterModelId);
    }
}