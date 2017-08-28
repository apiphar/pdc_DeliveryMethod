

export class RoutingProductionLeadTimeService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }


    GetData() {
        return this.HttpClient.get<string>('/routingProductionLeadTime/GetData');
    }

    GetKodeRute(RoutingMasterCode: String) {
        return this.HttpClient.get<JSON>('/routingProductionLeadTime/GetKodeRute/' + RoutingMasterCode);
    }

    GetRoutingMasterCode() {
        return this.HttpClient.get<string>('/routingProductionLeadTime/GetRoutingMasterCode');
    }
    GetDropDownLocationCode() {
        return this.HttpClient.get<string>('/routingProductionLeadTime/GetDropDownLocationCode');
    }

    GetSuffix(katashiki: string) {
        return this.HttpClient.get<string>('/routingProductionLeadTime/getsuffix/' + katashiki);
    }

    GetCarModel(katashiki: String, suffix: String) {
        return this.HttpClient.get<JSON>('/routingProductionLeadTime/getcarmodel/' + katashiki + '/' + suffix);
    }

    GetKatashiki() {
        return this.HttpClient.get<string>('/routingProductionLeadTime/GetKatashiki');
    }



    PostData(LocationCode: string, Katashiki: string, Suffix: string, RoutingMasterCode: string, Ordering: number, LeadMinutes: number) {
        return this.HttpClient.post('/routingProductionLeadTime/create', { LocationCode, Katashiki, Suffix, RoutingMasterCode, Ordering, LeadMinutes });
    }

    UpdateData(RoutingDictionaryProductionId: number, LocationCode: string, Katashiki: string, Suffix: string, RoutingMasterCode: string, Ordering: number, LeadMinutes: number) {
        return this.HttpClient.post('/routingProductionLeadTime/edit/' + RoutingDictionaryProductionId, { LocationCode, Katashiki, Suffix, RoutingMasterCode, Ordering, LeadMinutes });
    }

    DeleteData(RoutingDictionaryProductionId: number) {
        return this.HttpClient.post('/routingProductionLeadTime/delete/' + RoutingDictionaryProductionId, {});
    }



}