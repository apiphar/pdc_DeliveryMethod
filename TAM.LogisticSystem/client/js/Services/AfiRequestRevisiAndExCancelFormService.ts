import * as Component from '../components';
export class AfiRequestRevisiAndExCancelFormService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }
    UpdateAFIRevisi(Data: Component.DataInsertRevisiModel) {
        return this.HttpClient.post('api/v1/AfiRequestRevisiAndExCancelFormApi/UpdateAFIRevisi', Data);
    }
    UpdateAFIExCancel(Data: Component.DataInsertCanceModelForm) {
        return this.HttpClient.post('api/v1/AfiRequestRevisiAndExCancelFormApi/UpdateAFIExCancel', Data);
    }
    getRegionAndRegionAFI() {
        return this.HttpClient.get("api/v1/AfiRequestRevisiAndExCancelFormApi/GetAllRegion");
    }
}