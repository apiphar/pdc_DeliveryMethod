import * as angular from 'angular';
import * as Component from '../components'
export class UploadDownloadService {
    static $inject = ['$http'];
    HttpClient: angular.IHttpService;
    
    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    checkTable(master) {
        return this.HttpClient.get("/api/v1/UploadDownloadApi/CheckTable/" + master);
    }
    
    GetDataByMaster(master) {
        return this.HttpClient.get("/api/v1/UploadDownloadApi/DataByMaster/"+master);
    }

    getColumnDate(master) {
        return this.HttpClient.get("/api/v1/UploadDownloadApi/GetColumnDate/"+master);
    }

    filterByDate(master, filterDate: Component.FilterDate) {
        return this.HttpClient.post("/api/v1/UploadDownloadApi/FilterByDate/"+master, filterDate);
    }

    filter(master, Data: string[]) {
        return this.HttpClient.post("/api/v1/UploadDownloadApi/Filter/"+master, Data);
    }
   
    upload(master,file: any) {
        var fd = new FormData();
        fd.append('file', file);
        return this.HttpClient.post('/api/v1/UploadDownloadApi/Upload/'+master, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    }

    download(master,title, Data) {
        let jsonstring: string = JSON.stringify(Data);
        return this.HttpClient.post('/api/v1/UploadDownloadApi/Download/' + master+'/'+title, { jsonstring});
    }

    saveUpload(master,title, Data) {
        let jsonstring: string = JSON.stringify(Data);
        return this.HttpClient.post('/api/v1/UploadDownloadApi/SaveUpload/' + master+'/'+title,  { jsonstring });
    }

    getLastLog(master) {
        return this.HttpClient.get('/api/v1/LogUploadDownloadApi/GetLastLog/' + master);
    }
}