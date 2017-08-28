

export class PolaRangkaianTahapAwalService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    // untuk request data dari server
    getData() {
        return this.HttpClient.get('api/v1/polarangkaiantahapawal');
    }

    // untuk insert data header dan detail routing dictionary ke server
    postData(Header, Detail) {
        return this.HttpClient.post('api/v1/polarangkaiantahapawal', { Header, Detail });
    }

    // untuk delete data dari header
    DeleteDataHeader(routingDictionaryHeadCode) {
        return this.HttpClient.delete('api/v1/polarangkaiantahapawal/' + routingDictionaryHeadCode );
    }
}
