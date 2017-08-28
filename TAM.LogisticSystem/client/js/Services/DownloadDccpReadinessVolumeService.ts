
export class DownloadDccpReadinessVolumeService {
    static $inject = ['$http'];
    httpService: angular.IHttpService;
    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }
    //get download link.. return file
    downloadExcel(date: DownloadDate) {
        return this.httpService.get<Download>('/api/v1/DownloadDccpReadinessVolumeApi/', { params:date });
    }
    
}
export class Download {
    guid: string;
    count: number;
}
export class DownloadDate {
    constructor() {
        this.date = new Date();
    }
    date: Date;
}