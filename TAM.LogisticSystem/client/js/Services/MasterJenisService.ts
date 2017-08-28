export class MasterJenisService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }
    //to get all data
    getAllJenisData() {
        return this.httpClient.get<MasterJenisViewModel[]>('api/v1/MasterJenisAPI');
    }
    //to delete selected data
    deleteJenisData(afiCarTypeCode: string) {
        return this.httpClient.delete('api/v1/MasterJenisAPI/' + afiCarTypeCode);
    }
    //to insert data
    createJenisData(afiCarTypeCode: string, jenis: string, model: string) {
        return this.httpClient.post('api/v1/MasterJenisAPI', { afiCarTypeCode, jenis, model });
    }
    //to update selected data
    updateJenisData(afiCarTypeCode: string, jenis: string, model: string) {
        return this.httpClient.post('api/v1/MasterJenisAPI/' + afiCarTypeCode, { afiCarTypeCode, jenis, model });
    }
}

export class MasterJenisViewModel {
    afiCarTypeCode: string;
    jenis: string;
    model: string;
}