

export class KodeShiftService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get<string>('/api/v1/kodeShiftapi');
    }
    PostData(shiftCode: string, description: string) {
        return this.HttpClient.post('/api/v1/kodeShiftapi/', { shiftCode, description });
    }
    UpdateData(shiftCode: string, description: string) {
        return this.HttpClient.post('/api/v1/kodeShiftapi/' + shiftCode, { description });
    }

    DeleteData(shiftCode: string) {
        return this.HttpClient.delete('/api/v1/kodeShiftapi/' + shiftCode, {});
    }

}