
import * as Component from '../components';
export class AfiReturnToOutletService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    } 
    UpdateAFINormal(data: Component.DataInsertROModelForm) {
        return this.HttpClient.post('api/v1/AfiReturnToOutletFormApi/UpdateAFINormal', data);
    }
    UpdateAFIRevisi(data: Component.DataInsertROModelForm) {
        return this.HttpClient.post('api/v1/AfiReturnToOutletFormApi/UpdateAFIRevisi', data);
    }

    UpdateAFIExCancel(data: Component.DataInsertROModelForm) {
        return this.HttpClient.post('api/v1/AfiReturnToOutletFormApi/UpdateAFIExCancel', data);
    }
    checkDataByFrame(id: string) {
        return this.HttpClient.get("api/v1/AfiReturnToOutletFormApi/CheckDataByFrame/"+id);
    }
    getRegionAndRegionAFI() {
        return this.HttpClient.get("api/v1/AfiReturnToOutletFormApi/GetAllRegion");
    }
    GetFormData(id: string) {
        return this.HttpClient.get('api/v1/AfiReturnToOutletFormApi/GetFormData/' + id);
    }
    GetRequest(data: Component.AFISearchData) {
        return this.HttpClient.post('api/v1/AfiReturnToOutletApi/GetRequest', data);
    }

}