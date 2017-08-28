import * as Component from '../components';
export class AfiRequestRevisiAndExCancelService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }
    
    GetRequest(data: Component.AFISearchData) {
        return this.HttpClient.post('api/v1/AfiRequestRevisiAndExCancelApi/GetRequest', data);
    }
}