import * as angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
export class UploadDownloadController implements angular.IController {
    static $inject = ['UploadDownloadService', '$rootScope'];

    rootService: angular.IRootScopeService;
    UploadDownloadService: Service.UploadDownloadService;
    master: string;
    tipe: string;
    data: any;
    searchString: any;
    dateFrom: any;
    dateTo: any;
    buttonState: boolean = true;
    dateColumn: any;
    lastLog: any = [];
    errorCount: number = 0;
    modul: string;
    fileInput: any;
    txtFile: string;
    title: string;


    //pagination
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20,25];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    totalSpan: number;
    loader: boolean = false;
    pageState: boolean = false;
    constructor(UploadDownloadService: Service.UploadDownloadService,RootService:angular.IRootScopeService) {
        this.UploadDownloadService = UploadDownloadService;
        this.rootService = RootService;
    }

    $onInit() {
        this.loader = true;
        this.rootService.$on("UploadDownload", (event, result, info) => { 
            this.UploadDownloadService.checkTable(info.master).then(
                response => {
                    if (response.data != true) {
                        alertify.error("Wrong Table!!");
                        this.buttonState = false;
                        return;
                    }
                    this.loader = false;
                }
            );
            let tempData;
            tempData = result;
            if (Array.isArray(result)) {
                let temp: any = {};
                temp.firstProp = result;
                tempData = temp;
            }
            this.tipe = info.tipe;
            this.master = info.master;
            this.title = info.title!=null?info.title:info.master;
            this.pageState = true;
            if (this.tipe == "3") {
                this.UploadDownloadService.filter(this.master, tempData).then(
                    response => {
                        this.getData(response.data, this.master);
                        this.loader = false;
                    }
                );
            }
        });
        this.getLastLog();
        this.modul = "Master";
    }

    modalPopup() {
        this.UploadDownloadService.getLastLog(this.master).then(response => {
            this.lastLog = response.data;
            if (this.lastLog.status.toLowerCase() == "sukses")
                this.lastLog.statusState = true;
            else
                this.lastLog.statusState = false;
            this.lastLog.prosess = this.lastLog.isUploadProcess ? "Upload" : "Download";
            alertify.alert('Status Proses Terakhir', mustache.render(require('./alertify/LogModal.html'), this.lastLog));
        });

    }

    kembali() {
        this.pageState = false;
        this.data = null;
        this.dateColumn = null;
        this.rootService.$emit("Kembali");
    }

    refresh() {
        this.data = null;
        this.dateColumn = null;
        this.fileInput = null;
        this.txtFile = null;
    }
    //to filter by Date
    filter() {
        var filterDate: FilterDate = new FilterDate();
        if (this.dateTo != null && this.dateFrom==null) {
            angular.forEach(this.dateTo, (value, key) => {
                filterDate.field.push(key);
                filterDate.DateFrom.push(null);
                filterDate.DateTo.push(value);
            });
        }
        if (this.dateFrom != null && this.dateTo==null) {
            angular.forEach(this.dateFrom, (value, key) => {
                filterDate.field.push(key);
                filterDate.DateTo.push(null);
                filterDate.DateFrom.push(value);
            });
        }
        angular.forEach(this.dateFrom, (value, key) => {
            angular.forEach(this.dateTo, (valueTo, keyTo) => {
                if (key == keyTo) {

                    if (value > valueTo) {
                        alertify.error(keyTo+" tanggal mulai harus lebih besar");
                        return;
                    }
                    filterDate.field.push(keyTo);
                    filterDate.DateFrom.push(value);
                    filterDate.DateTo.push(valueTo);
                }
            })
        })

        if (filterDate.field.length != 0)
            this.UploadDownloadService.filterByDate(this.master, filterDate).then(
                response =>
                {
                    this.data = response.data;
                    this.totalItems = this.data.length;
                }
            );
    }

    getDateColumn(master) {
        this.UploadDownloadService.getColumnDate(master).then(
            response => {
                if ((<any>response).data.length > 0)
                    this.dateColumn = response.data
            }
        );
    }
    
    process() {
        this.loader = true;
        if (this.tipe == "3") {
            this.UploadDownloadService.GetDataByMaster(this.master).then(
                response => {
                    this.getDateColumn(this.master);
                    this.modul = "Master";
                    this.data = response.data;
                    this.totalItems = this.data.length;
                    this.loader = false;
                }
            ).catch(
                resp => {
                    this.loader = false;
                    alertify.error("Error While Processing Data");
                }
           );
        }
    }

    getData(result,master) {
        if (this.tipe == "3") {
            this.loader = true;
            this.getDateColumn(master);
            this.data = result;
            this.totalItems = this.data.length;
            this.loader = false;
        }
    }
    
    getLastLog() {
        this.UploadDownloadService.getLastLog(this.master).then(
            response =>
            {
                this.lastLog = response.data;
                this.lastLog.prosess = this.lastLog.isUploadProcess ? "Upload" : "Download";
            }
        );
    }
    
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }
    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            // angular's default (non-strict) internal comparator
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    }

    setPage(pageNo) {
        this.currentPage = pageNo;
    }

    downloadData(result) {
        this.UploadDownloadService.download(this.master,this.title, result).then(
            response => {
                alertify.success(response.data).set('notifier', 'delay', 5);;
                this.getLastLog();
            }
        );
    }

    download(result) {
        alertify.confirm("Download", "Anda yakin ingin download?", () => {
            this.downloadData(result);
        }, () => { });
        
    }

    search(result) {
        this.totalItems = result.length;
        this.setPage(1);
    }

    upload() {
        this.loader = true; this.data = null;
        let file: File = this.fileInput;
        this.txtFile = this.fileInput.name;

        this.UploadDownloadService.upload(this.master, file).then(
            response => {
                this.data = response.data;
                this.errorCount  = _.reject(this.data, ['messageError', null]).length;
                this.totalItems = this.data.length;
                this.modul = "Master";
                this.loader = false;
            }
        ).catch(response => {
            if (response.status == "400") {
                alertify.error(response.data);
                return;
            }
            alertify.error("Internal Server Error");
            this.loader = false;
        });
    }

    saveUploadData(result) {
        this.UploadDownloadService.saveUpload(this.master,this.title, result).then(
            response => {
                alertify.success(response.data);
                this.getLastLog();
                this.data = null
                this.txtFile = null;
            }
        ).catch(
            response => {
                alertify.error("Error while saving data");
                this.data = null;
            }
        );
    }
    saveUpload(result) {
        alertify.confirm("Save Upload", "Anda yakin ingin Simpan?", () => {
            this.saveUploadData(result);
        }, () => { });
        
    }
}

export class FilterDate {
    field: string[] = [];
    DateFrom?: Date[] = [];
    DateTo?: Date[] = [];
}

export class UploadDownload implements angular.IComponentOptions {
    controller = UploadDownloadController;
    controllerAs = 'me';

    template = require('./UploadDownload.html');

}