

export class PolaRangkaianTahapAkhirService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    // untuk request data dari server
    getData() {
        return this.HttpClient.get('api/v1/polarangkaiantahapakhir');
    }

    // untuk insert data header dan detail routing dictionary ke server
    postData(Header, Detail) {
        return this.HttpClient.post('api/v1/polarangkaiantahapakhir', { Header, Detail });
    }

    // untuk delete data dari header
    DeleteDataHeader(routingDictionaryTailCode) {
        return this.HttpClient.delete('api/v1/polarangkaiantahapakhir/' + routingDictionaryTailCode );
    }
}
