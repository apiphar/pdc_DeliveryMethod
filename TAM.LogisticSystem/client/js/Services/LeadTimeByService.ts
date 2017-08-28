

export class LeadTimeByService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }

    getRoutingLeadTimeByList() {
        return this.httpClient.get<any>('/api/v1/leadtimeby');
    }
}