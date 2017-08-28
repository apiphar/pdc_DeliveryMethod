import * as Angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';
import * as Moment from 'moment';
export class AfiDownloadController implements Angular.IController {
    $inject = ['AfiDownloadService'];

    AfiDownloadService: Service.AfiDownloadService;

    branchData: any;
    submissionData: any;
    searchSubmission: searchSubmissionDownload;
    cbMain: boolean;
    revisiState: boolean = false;
    altInputFormats: any = ['M!/d!/yyyy'];
    dateOptionsSampai: any = {};
    dateOptions: any = {};
    loader: boolean = true;
    //pagination
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20,25];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;
    cbSelect: any = document.getElementById("cbSelect");
    constructor(AfiDownloadService: Service.AfiDownloadService) {
        this.AfiDownloadService = AfiDownloadService;
        this.searchSubmission = new searchSubmissionDownload();
    }

    $onInit() {
        this.getAllBranch();
    }

    hideRevisi() {
        this.revisiState = false;
    }
    showRevisi() {
        this.revisiState = true;
    }

    getAllBranch() {
        this.AfiDownloadService.getAllBranch().then(response => {
            this.branchData = response.data;
        });
    }
    getTrueData() {
        return _.filter(this.submissionData, ['select', true]);
    }
    downloadDialog() {
        Alertify.confirm(
            "Konfirmasi", "Anda yakin akan ingin mendownload?",
            () => {
                this.download()
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
    download() {
        this.AfiDownloadService.download(this.getTrueData()).then(
            response => {
                if (response.data) {
                    var content = response.data;
                    var blob = new Blob([content], { type: 'text/plain' });
                    var a = document.createElement("a");
                    a.href = (window.URL).createObjectURL(blob);
                    a.download = this.searchSubmission.statusPengajuan+"_" + Moment().format("YYYYMMDD_HHmm") +".txt";
                    a.click();
                    Alertify.success("Data berhasil terdownload");
                    this.submissionData = null;
                    this.cbSelect.indeterminate = false;
                    this.cbMain = false;
                } else
                    Alertify.error("Silahkan pilih data terlebih dahulu")
            }
        );
    }

    selectAll() {
        var flag = this.cbMain;
        Angular.forEach(this.submissionData, function (data) {
            data.select = flag;
        });
    }
    checkboxChanged() {
        let count: number = _.filter(this.submissionData, ['select', true]).length;
        if (count == this.submissionData.length) {
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

    doSearch() {
        this.loader = false;
        this.cbSelect.indeterminate = false;
        this.cbMain = false;
        this.AfiDownloadService.getSubmission(this.searchSubmission).then(
            response => {
                this.loader = true;
                this.submissionData = response.data;
                Angular.forEach(this.submissionData, function (data) {
                    data.select = false;
                });
        });
    }

    convertForDatepicker(tanggal: Date) {
        return new Date(Date.UTC(tanggal.getFullYear(), tanggal.getMonth(), tanggal.getDate()));
    }

    tanggalChanged() {
        if (this.searchSubmission.tanggalPengajuan == null) {
            this.dateOptionsSampai.minDate = null;
            return;
        }
        this.dateOptionsSampai.minDate = new Date(this.searchSubmission.tanggalPengajuan).setDate(this.searchSubmission.tanggalPengajuan.getDate() + 1);
        this.searchSubmission.tanggalPengajuan = this.convertForDatepicker(this.searchSubmission.tanggalPengajuan)
    }
    tanggalChangedSampai() {
        if (this.searchSubmission.sampai == null) {
            this.dateOptions.maxDate = null;
            return;
        }
        this.dateOptions.maxDate = new Date(this.searchSubmission.sampai).setDate(this.searchSubmission.sampai.getDate() - 1);
        this.searchSubmission.sampai = this.convertForDatepicker(this.searchSubmission.sampai);
    }
    
}

export class searchSubmissionDownload {
    frameNo: string;
    quantity: number;
    type: string;
    branch: any;
    tanggalPengajuan?: Date;
    sampai?: Date;
    statusPengajuan: string;
    revisi: string;
}

export class AfiDownload implements Angular.IComponentOptions {
    controller = AfiDownloadController;
    controllerAs = 'me';

    template =  require('./AfiDownload.html');
}