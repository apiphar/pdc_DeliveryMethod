

export class GeneratePolaRangkaianRuteService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get('api/v1/generatepolarangkaianrute');
    }
    
    PostData(joinData, validFrom) {
        return this.HttpClient.post('api/v1/generatepolarangkaianrute', { joinData , validFrom });
    }
}