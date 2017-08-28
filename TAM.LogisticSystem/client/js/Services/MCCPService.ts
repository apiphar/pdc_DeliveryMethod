

export class MCCPService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }

    getData() {
        return this.httpClient.get('/api/v1/MCCPapi/GetData');
    }
    create(Baris, LokAsal, LokTujuan, Keterangan, SqlField, DealerCode, BranchCode) {
        return this.httpClient.post('/api/v1/MCCPapi/', { Baris, LokAsal, LokTujuan, Keterangan, SqlField, DealerCode, BranchCode});
    }
    update(Baris, LokAsal, LokTujuan, Keterangan, SqlField, DealerCode, BranchCode) {
        return this.httpClient.post('/api/v1/MCCPapi/' + Baris, { LokAsal, LokTujuan, Keterangan, SqlField, DealerCode, BranchCode });
    }
    delete(Baris:number) {
        return this.httpClient.delete('/api/v1/MCCPapi/' + Baris);
    }
    getLocationData() {
        return this.httpClient.get('/api/v1/MCCPapi/GetLocationData');
    }
    getDealerData() {
        return this.httpClient.get('/api/v1/MCCPapi/GetDealerData');
    }
    getBranchData() {
        return this.httpClient.get('/api/v1/MCCPapi/GetBranchData');
    }
}