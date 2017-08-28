export class WorkingDictionaryServices {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAll() {
        return this.HttpClient.get<string>('/api/v1/WorkingDictionaryapi/GetData');
    }
    deleteData(idleCode) {
        return this.HttpClient.delete<string>('/api/v1/WorkingDictionaryapi/DeleteWorkHour/' + idleCode);
    }
    postDataWork(breakHour) {
        return this.HttpClient.post('/api/v1/WorkingDictionaryapi/InsertUpdateWorkHour', breakHour);
    }
}