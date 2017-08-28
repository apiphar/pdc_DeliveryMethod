

export class MasterLeadTimeService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }


    GetData() {
        return this.HttpClient.get<string>('/masterLeadTime/GetData');
    }

    GetDropDownLocation() {
        return this.HttpClient.get<string>('/masterLeadTime/GetDropDownLocation');
    }

    GetAll() {
        return this.HttpClient.get<string>('/masterLeadTime/GetAll');
    }

    GetKodeRute(RoutingMasterCode: String) {
        return this.HttpClient.get<JSON>('/masterLeadTime/GetKodeRute/' + RoutingMasterCode);
    }



    PostData(LocationCode: string, RoutingMasterCode: string, LeadMinutes: number) {
        return this.HttpClient.post('/masterLeadTime/create', { LocationCode, RoutingMasterCode, LeadMinutes});
    }

    UpdateData(LocationCode: string, RoutingMasterCode: string, LeadMinutes: number) {
        return this.HttpClient.post('/masterLeadTime/edit/' + LocationCode + '/' + RoutingMasterCode, {LeadMinutes});
    }

    DeleteData(LocationCode: string) {
        return this.HttpClient.post('/masterLeadTime/delete/' + LocationCode, {});
    }

    checkLocationCodeAndRoutingCode(LocationCode: string, RoutingMasterCode: string) {
        return this.HttpClient.get('/masterLeadTime/check-location-code-and-routing-code/' + LocationCode + '/' + RoutingMasterCode);
    }



}