

export class EngineService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }


    GetAllData() {
        return this.HttpClient.get<string>('/Engine/GetData');
    }

    GetCarModel(katashiki: String) {
        return this.HttpClient.get<JSON>('/Engine/getcarmodel/' + katashiki);
    }

    GetAll() {
        return this.HttpClient.get<string>('/Engine/GetAll');
    }

    PostData(katashiki: string, FrameCode: string, EnginePrefix: string) {
        return this.HttpClient.post('/Engine/create', { katashiki, FrameCode, EnginePrefix});
    }

    UpdateData(katashikiValidationId: string, katashiki: string, FrameCode: string, EnginePrefix: string) {
        return this.HttpClient.post('/Engine/edit/' + katashikiValidationId, { katashiki, FrameCode, EnginePrefix });
    }

    DeleteData(katashikiValidationId: string) {
        return this.HttpClient.post('/Engine/delete/' + katashikiValidationId, {});
    }

}