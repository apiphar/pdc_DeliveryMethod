

export class IdleDictionaryServices {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAll() {
        return this.HttpClient.get<string>('/api/v1/IdleDictionaryapi/GetData');
    }
    deleteData(idleCode) {
        return this.HttpClient.delete<string>('/api/v1/IdleDictionaryapi/DeleteIdleDictionary/' + idleCode);
    }
    postDataBreak(breakHour) {
        console.log(breakHour);
        return this.HttpClient.post('/api/v1/IdleDictionaryapi/InsertUpdateIdleDictionary',  breakHour );
    }
    
}