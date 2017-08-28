
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';
export class ReportGesekanController implements angular.IController{
    $inject = ['ReportGesekanService'];

    ReportGesekanService: Service.ReportGesekanService;
    reportGesekan: ReportGesekanModel;
    reportList: any;
    altInputFormats: any = ['M!/d!/yyyy'];
    loader: boolean;

    //pagination
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    constructor(ReportGesekanService: Service.ReportGesekanService) {
        this.ReportGesekanService = ReportGesekanService;
        this.loader = true;
    }

    $onInit() {
        this.reportGesekan = new ReportGesekanModel();
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
        this.totalItems = data.result.length;
        this.setPage(1);

    }
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    download() {
        this.ReportGesekanService.download(this.reportList).then(response => {
            Alertify.success("Success Download Data");
            window.open(window.location.origin + '/api/v1/ReportGesekanApi/Download/' + response.data);
        });
    }
    getRepostGesekan() {
        if (this.reportGesekan.tanggalFrom > this.reportGesekan.tanggalTo) {
            Alertify.error("Date to must be later than or equal to Date from");
            return;
        }
        this.ReportGesekanService.getReportGesekan(this.reportGesekan).then(respoonse => {
            this.reportList = respoonse.data;
            this.loader = true;
        });
    }
    
}

export class ReportGesekan implements angular.IComponentOptions {
    controller = ReportGesekanController;
    controllerAs = 'me';

    template = require('./ReportGesekan.html');
}

export class ReportGesekanModel {
    frameNumber: string;
    tanggalFrom?: Date;
    tanggalTo?: Date;
}