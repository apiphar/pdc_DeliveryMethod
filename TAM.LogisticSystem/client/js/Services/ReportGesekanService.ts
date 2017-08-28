
import * as Component from '../components';
export class ReportGesekanService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    } 

    getReportGesekan(data:Component.ReportGesekanModel) {
        return this.HttpClient.post('/api/v1/ReportGesekanApi/ReportGesekan',data);
    }

    download(data: any[]) {
        return this.HttpClient.post("/api/v1/ReportGesekanApi/Download/", data);
    }
}