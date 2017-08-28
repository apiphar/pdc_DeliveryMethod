import * as angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as moment from 'moment';
export class LogUploadDownloadController implements angular.IController{
    static $inject = ['LogUploadDownloadService'];

    LogUploadDownloadService: Service.LogUploadDownloadService;
    data: any;


    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20,25];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    monthNames:string[] = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
    

    constructor(LogUploadDownloadService: Service.LogUploadDownloadService) {
        this.LogUploadDownloadService = LogUploadDownloadService;
    }

    $onInit() {
        this.refresh();
    }

    refresh() {
        this.LogUploadDownloadService.getAllLogUploadDownload().then(
            response => {
                this.data = response.data;
                angular.forEach(this.data, (data) => {
                    data.process = data.isUploadProcess ? "Upload" : "Download";
                });
                this.totalItems = this.data.length;
            }
        );
    }

    ConvertDateTime(date:string) {
        let d: Date = new Date(date);
        return d;
    }

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            // angular's default (non-strict) internal comparator
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };

    setPage(pageNo) {
        this.currentPage = pageNo;
    };

    search(data) {
        this.totalItems = data.length;
        this.setPage(1);
        
    }
    order(orderString:string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }
}
export class LogUploadDownload implements angular.IComponentOptions {
    controller = LogUploadDownloadController;
    controllerAs = 'me';

    template = require('./LogUploadDownload.html');

}