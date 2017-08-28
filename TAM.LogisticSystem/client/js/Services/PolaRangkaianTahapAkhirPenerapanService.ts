

export class PolaRangkaianTahapAkhirPenerapanService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    // untuk request data ke server
    getData() {
        return this.HttpClient.get('api/v1/polarangkaiantahapakhirpenerapan');
    }

    // untuk insert data dengan http post ke server
    postData(routingDictionaryTailCode, carType, branch) {
        return this.HttpClient.post('api/v1/polarangkaiantahapakhirpenerapan', { routingDictionaryTailCode , carType, branch });
    }
}