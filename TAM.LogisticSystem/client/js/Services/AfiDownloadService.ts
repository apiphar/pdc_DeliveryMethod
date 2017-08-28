import * as Angular from 'angular';
import * as Component from '../components';
export class AfiDownloadService {
    $inject = ['$http'];

    HttpClient: Angular.IHttpService;

    constructor($http: Angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAllBranch() {
        return this.HttpClient.get('/api/v1/AfiDownloadApi/GetAllBranch');
    }

    download(data) {
        return this.HttpClient.post('/api/v1/AfiDownloadApi/Download',data);
    }
    getSubmission(search: Component.searchSubmissionDownload) {
        return this.HttpClient.post('/api/v1/AfiDownloadApi/GetSubmission', search);
    }
}