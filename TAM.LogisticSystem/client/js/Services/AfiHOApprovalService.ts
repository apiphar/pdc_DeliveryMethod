import * as Angular from 'angular';
import * as Component from '../components';
export class AfiHOApprovalService {
    $inject = ['$http'];

    HttpClient: Angular.IHttpService;

    constructor($http: Angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAllBranch() {
        return this.HttpClient.get('/api/v1/AfiHOApprovalApi/GetAllBranch');
    }

    processToTam(data) {
        return this.HttpClient.post('/api/v1/AfiHOApprovalApi/ProcessToTam',data);
    }

    returnToOutlet(data) {
        return this.HttpClient.post('/api/v1/AfiHOApprovalApi/ReturnToOutlet', data);
    }

    getSubmission(search: Component.SearchSubmission) {
        return this.HttpClient.post('/api/v1/AfiHOApprovalApi/GetAFIHoApproval', search);
    }
}