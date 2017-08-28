
import * as Component from '../components'
export class LogUploadDownloadService {
    static $inject = ['$http'];
    HttpClient: angular.IHttpService;
    
    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAllLogUploadDownload() {
        return this.HttpClient.get('/api/v1/LogUploadDownloadApi/GetAllLogUploadDownload');
    }
}