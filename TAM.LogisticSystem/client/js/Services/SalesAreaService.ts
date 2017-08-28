

export class SalesAreaService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get('api/v1/salesarea');
    }

    PostData(SalesAreaCode:string, Description: String) {
        return this.HttpClient.post('api/v1/salesarea', { SalesAreaCode, Description });
    }

    UpdateData(SalesAreaCode: string, Description: String) {
        return this.HttpClient.post('api/v1/salesarea/' + SalesAreaCode, { Description });
    }

    DeleteData(SalesAreaCode: string) {
        return this.HttpClient.delete('api/v1/salesarea/' + SalesAreaCode);
    }
}
