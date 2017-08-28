import * as angular from 'angular';
import * as Component from '../components';
export class AfiRequestUploadService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    Upload(file: any) {
        var fd = new FormData();
        fd.append('file', file);
        return this.HttpClient.post<AfiRequestUploadViewModel[]>('/api/v1/AfiRequestUploadApi/Upload', fd, {
            headers: { 'Content-Type': undefined }
        });
    }
    SaveAfiRequest(data) {
        return this.HttpClient.post('/api/v1/AfiRequestUploadApi/SaveAfiRequest', data);
    }
}

export class AfiRequestUploadViewModel {
    frameNumber: string;
    customerName: string;
    ktp: string;
    address1: string;
    address2: string;
    address3: string;
    province: string;
    city: string;
    postCode: string;
    regionAFI: string;
    effectiveDate: Date;
    color: string;
}