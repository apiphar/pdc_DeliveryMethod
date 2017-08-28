import * as Angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';
export class AfiHOApprovalController implements Angular.IController {
    $inject = ['AfiHOApprovalService'];

    afiHOApprovalService: Service.AfiHOApprovalService;

    branchData: any;
    hoApprovalData: any = null;
    searchSubmission: SearchSubmission;
    cbMain: boolean;
    altInputFormats: any = ['M!/d!/yyyy'];
    loader: boolean = true;
    dateOptionsSampai: any = {};
    dateOptions: any = {};
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
    constructor(AfiHOApprovalService: Service.AfiHOApprovalService) {
        this.afiHOApprovalService = AfiHOApprovalService;
        this.searchSubmission = new SearchSubmission();
    }

    $onInit() {
        this.getAllBranch().then(response => {
            this.branchData = response.data;
        });
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

    getAllBranch() {
        return this.afiHOApprovalService.getAllBranch();
    }

    getTrueData() {
        return _.filter(this.hoApprovalData, ['select', true]);
    }

    checkboxChanged() {
        let count: number = _.filter(this.hoApprovalData, ['select', true]).length;
        if (count == this.hoApprovalData.length) {
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
    returnToOutletDialog() {
        Alertify.confirm(
            "Konfirmasi", "Anda yakin akan mengembalikan ke Outlet?",
            () => {
                this.returnToOutlet();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
    returnToOutlet() {
        let trueData = this.getTrueData();
        if (trueData.length != 0) {
            this.afiHOApprovalService.returnToOutlet(trueData).then(
                response => {
                    Alertify.success(response.data);
                    this.hoApprovalData = null;
                    this.cbSelect.indeterminate = false;
                    this.cbMain = false;
                }
            ).catch(response => {
                if (response.status == "400") {
                    Alertify.error(response.data);
                    return;
                }
                Alertify.error("Koneksi server bermasalah");
            });
        } else {
            Alertify.error("Silahkan pilih salah satu data");
        }
    }
    processToTamDialog() {
        Alertify.confirm(
            "Konfirmasi","Anda yakin akan melanjutkan proses ke TAM?",
            () => {
                this.processToTam();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
    processToTam() {
        let trueData = this.getTrueData();
        if (trueData.length != 0) {
            this.afiHOApprovalService.processToTam(trueData).then(
                response => {
                    Alertify.success(response.data);
                    this.hoApprovalData = null;
                    this.cbSelect.indeterminate = false;
                    this.cbMain = false;
                }
            ).catch(response => {
                if (response.status == "400") {
                    Alertify.error(response.data);
                    return;
                }
                Alertify.error("Koneksi server bermasalah");
            });
        } else {
            Alertify.error("Silahkan pilih salah satu data");
        }
    }

    selectAll() {
        var flag = this.cbMain;
        Angular.forEach(this.hoApprovalData, function (data) {
            data.select = flag;
        });
    }

    doSearch() {
        this.loader = false;
        this.cbSelect.indeterminate = false;
        this.cbMain = false;
        this.afiHOApprovalService.getSubmission(this.searchSubmission).then(
            response => {
                this.hoApprovalData = response.data;
                this.loader = true;
                Angular.forEach(this.hoApprovalData, function (data) {
                    data.select = false;
                });
            }).catch(response => {
                this.loader = true;
                if (response.status == "400") {
                    Alertify.error(response.data);
                    return;
                }
                Alertify.error("Koneksi server bermasalah");
            });
    }

    convertForDatePicker(tanggal:Date) {
        return new Date(Date.UTC(tanggal.getFullYear(), tanggal.getMonth(), tanggal.getDate()));
    }

    tanggalChanged() {
        if (this.searchSubmission.tanggalPengajuan == null) {
            this.dateOptionsSampai.minDate = null;
            return;
        }
        this.dateOptionsSampai.minDate = new Date(this.searchSubmission.tanggalPengajuan).setDate(this.searchSubmission.tanggalPengajuan.getDate()+1);
        this.searchSubmission.tanggalPengajuan = this.convertForDatePicker(this.searchSubmission.tanggalPengajuan);
    }
    tanggalChangedSampai() {
        if (this.searchSubmission.sampai == null) {
            this.dateOptions.maxDate = null;
            return;
        }
        this.dateOptions.maxDate = new Date(this.searchSubmission.sampai).setDate(this.searchSubmission.sampai.getDate()-1);
        this.searchSubmission.sampai = this.convertForDatePicker(this.searchSubmission.sampai);
    }
    
}

export class SearchSubmission {
    frameNo: string;
    type: string;
    branch: any;
    tanggalPengajuan?: Date;
    sampai?: Date;
    statusPengajuan: string;
}

export class AfiHOApproval implements Angular.IComponentOptions {
    controller = AfiHOApprovalController;
    controllerAs = 'me';

    template =  require('./AfiHOApproval.html');
}