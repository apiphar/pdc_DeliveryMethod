

export class PDCConfigService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getPDCconfig() {
        return this.HttpClient.get<string>('/PDCConfigApi/GetPDCConfig');
    }

    getPDC() {
        return this.HttpClient.get<string>('/PDCConfigApi/GetPDC');
    }

   GetId(locationCode: string) {
       return this.HttpClient.get<string>('/PDCConfigApi/GetId/' + locationCode);
    }

    postData(locationCode: String, maintenanceDay: number, carCarrierQuotaPerDay: number, nonCarCarrierQuotaPerDay: number, leadDayPreDeliveryService: number) {
        return this.HttpClient.post('PDCConfigApi/create', { locationCode, maintenanceDay, carCarrierQuotaPerDay, nonCarCarrierQuotaPerDay, leadDayPreDeliveryService});
    }

    updateData(pdcConfigId: number, maintenanceDay: number, carCarrierQuotaPerDay: number, nonCarCarrierQuotaPerDay: number, leadDayPreDeliveryService: number) {
        return this.HttpClient.post('/PDCConfigApi/edit/' + pdcConfigId, {  maintenanceDay, carCarrierQuotaPerDay, nonCarCarrierQuotaPerDay, leadDayPreDeliveryService});
    }

    deleteData(pdcConfigId: number) {
        return this.HttpClient.post('/PDCConfigApi/delete/' + pdcConfigId, {});
    } 
}