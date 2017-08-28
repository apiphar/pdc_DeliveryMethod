import * as angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';

export class AfiRequestUploadController implements angular.IController {
    $inject = ['AfiRequestUploadService'];

    AfiRequestUploadService: Service.AfiRequestUploadService;
    directory: any;
    data: any;
    cbMain: boolean;
    inputFile: any;
    cbSelect: any = document.getElementById("cbSelect");
    isError: boolean = true;


    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;

    loader: boolean = false;
    constructor(AfiRequestUploadService: Service.AfiRequestUploadService) {
        this.AfiRequestUploadService = AfiRequestUploadService;
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
    upload() {
        let file: any = this.inputFile;
        this.directory = file.name;
        this.loader = true;
        this.AfiRequestUploadService.Upload(file).then(
            response => {
                this.loader = false;
                this.data = response.data;
                var nullObject = _.find(this.data, (item:any) => {
                    return item.errorDescription != null;
                });
                if (nullObject != null) {
                    this.isError = true;
                    return;
                }
                this.isError = false;
            }
        ).catch(
            response => {
                if (response.status == "400") {
                    Alertify.error(response.data);
                }
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
                this.data = null;
                this.loader = false;
            });
    }
    convertDate(tanggalString: string): Date{
        let tanggal:Date = new Date(tanggalString);
        return new Date(Date.UTC(tanggal.getFullYear(), tanggal.getMonth(), tanggal.getDate()));
    }

    getTrueData() {
        let data = _.filter(this.data, ['select', true]);
        angular.forEach(data, (item:any) => {
            item.effectiveDate = this.convertDate(item.effectiveDate);
        });
        return data;
    }
    saveDialog() {
        let trueData: any = this.getTrueData();
        console.log(trueData);
        Alertify.confirm(
            "Konfirmasi","Apakah anda yakin ingin menyimpan data?"
            ,
            () => {
                this.save();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
    save() {
        let trueData: any = this.getTrueData();
        if (trueData.length != 0) {
            this.AfiRequestUploadService.SaveAfiRequest(trueData).then(
                response => {
                    this.data = null;
                    this.cbMain = false;
                    this.directory = null;
                    Alertify.success(response.data);
                }
            ).catch(
                response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                    }
                    if (response.status == "500") {
                        this.data = null;
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
        } else {
            Alertify.error("Silahkan pilih salah satu data");
        }
    }
    checkboxChanged() {
        let count: number = _.filter(this.data, ['select', true]).length;
        if (count == this.data.length) {
            this.cbSelect.indeterminate = false;
            this.cbMain = true;
            return;
        }
        if (count == 0) {
            this.cbSelect.indeterminate = false;
            return;
        }
        this.cbMain = false;
        this.cbSelect.indeterminate = true;
    }
    selectAll() {
        var flag = this.cbMain;
        angular.forEach(this.data, function (data) {
            data.select = flag;
        });
    }
}

export class AfiRequestUpload implements angular.IComponentOptions {
    controller = AfiRequestUploadController;
    controllerAs = 'me';

    template =  require('./AfiRequestUpload.html');
}