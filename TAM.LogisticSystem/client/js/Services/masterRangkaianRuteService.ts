

export class masterRangkaianRuteService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetRoutingDictionary() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetRoutingDictionary');
    }

    GetVehicle() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetVehicle');
    }

    GetBranch() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetBranch');
    }

    GetDealer() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetDealer');
    }

    GetRoutingDictionaryDetail(RoutingDictionaryId: number) {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetRoutingDictionaryDetail/' + RoutingDictionaryId);
    }

    GetLocation() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetLocation');
    }

    GetDeliveryMethod() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetDeliveryMethod');
    }

    GetRoutingMaster() {
        return this.HttpClient.get<string>('/masterrangkaianruteapi/GetRoutingMaster');
    }


    PostDataHeader(BranchCode: string, Katashiki: string, Suffix: string) {
        return this.HttpClient.post('/masterrangkaianruteapi/createHeader', { BranchCode, Katashiki, Suffix });
    }

    UpdateDataHeader(RoutingDictionaryId: number, BranchCode: string, Katashiki: string, Suffix: string) {
        return this.HttpClient.post('/masterrangkaianruteapi/editHeader/' + RoutingDictionaryId, { BranchCode, Katashiki, Suffix });
    }

    DeleteDataHeader(RoutingDictionaryId: number) {
        return this.HttpClient.post('/masterrangkaianruteapi/deleteHeader/' + RoutingDictionaryId, {});
    }

    PostDataDetail(RoutingDictionaryId: number, LocationCode: string, Ordering: number, DeliveryMethodCode: string, RoutingMasterCode: string) {
        return this.HttpClient.post('/masterrangkaianruteapi/createDetail', { RoutingDictionaryId, LocationCode, Ordering, DeliveryMethodCode, RoutingMasterCode });
    }

    UpdateDataDetail(RoutingDictionaryDetailId: number, LocationCode: string, Ordering: number, DeliveryMethodCode: string, RoutingMasterCode: string) {
        return this.HttpClient.post('/masterrangkaianruteapi/editDetail/' + RoutingDictionaryDetailId, { LocationCode, Ordering, DeliveryMethodCode, RoutingMasterCode });
    }

    DeleteDataDetail(RoutingDictionaryDetailId: number) {
        return this.HttpClient.post('/masterrangkaianruteapi/deleteDetail/' + RoutingDictionaryDetailId, {});
    }
}