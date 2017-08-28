

export class ConfigurationPlanningService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAll() {
        return this.HttpClient.get<string>('/ConfigurationPlanningApi/GetAll');
    }

    getRoutingMaster() {
        return this.HttpClient.get<string>('/ConfigurationPlanningApi/GetRoutingMaster');
    }

    updateData(routingMasterMCCP: String, routingMasterDCCP: String, mccp: Boolean, dccp: Boolean) {
        return this.HttpClient.post('/ConfigurationPlanningApi/Edit/' + routingMasterMCCP + '/' + routingMasterDCCP + '/' + mccp + '/' + dccp, {});
    } 

    deleteData(mccp: Boolean, dccp: Boolean) {
        return this.HttpClient.post('/ConfigurationPlanningApi/delete/' + mccp + '/' + dccp, {});
    } 

}