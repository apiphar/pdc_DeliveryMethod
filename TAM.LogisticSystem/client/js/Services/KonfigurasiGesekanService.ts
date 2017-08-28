
import * as Component from '../components';
export class KonfigurasiGesekanService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    } 

    GetExistsScratch() {
        return this.HttpClient.get('/api/v1/KonfigurasiGesekanApi/ExistsScratch');
    }    

    GetDealerBranch() {
        return this.HttpClient.get('/api/v1/KonfigurasiGesekanApi/DealerBranch');
    }    

    GetCarModel() {
        return this.HttpClient.get('/api/v1/KonfigurasiGesekanApi/CarModel');
    }    

    SaveConfig(Data: Component.ScratchData) {
        return this.HttpClient.post('/api/v1/KonfigurasiGesekanApi/SaveConfiguration', Data);
    }
}