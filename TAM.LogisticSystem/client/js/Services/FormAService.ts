

export class FormAService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAll() {
        return this.HttpClient.get<string>('/FormA/GetAll');
    }

    getFrameNumber(frameNumber: string) {
        return this.HttpClient.get<string>('/FormA/GetFrameNumber/' + frameNumber);
    } 

    updateData(frameNumber: string, formADate: Date, formANumber: string) {
        return this.HttpClient.post('/FormA/Edit/' + frameNumber, { formADate, formANumber });
    }
}