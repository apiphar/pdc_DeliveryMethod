

export class PolaRangkaianTahapAwalPenerapanService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    // untuk request data ke server
    getData() {
        return this.HttpClient.get('api/v1/polarangkaiantahapawalpenerapan');
    }

    // untuk insert data dengan http post ke server
    postData(routingDictionaryHeadCode, carType) {
        return this.HttpClient.post('api/v1/polarangkaiantahapawalpenerapan', { routingDictionaryHeadCode , carType});
    }
}