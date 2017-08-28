import * as Service from '../services';
import * as Component from '../components';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
export class AfiReturnToOutletController implements angular.IController {
    $inject = ['AfiReturnToOutletService','$rootScope'];
    AfiReturnToOutletService: Service.AfiReturnToOutletService;
    $rootScope: angular.IRootScopeService;
    Branch: any;
    SearchData: AFISearchDataReturnToOutlet;
    Data: any;
    radio: any;
    loader: boolean = true;
    altInputFormats: any = ['M!/d!/yyyy'];
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
    constructor(AfiReturnToOutletService: Service.AfiReturnToOutletService, $rootScope: angular.IRootScopeService) {
        this.AfiReturnToOutletService = AfiReturnToOutletService;
        this.$rootScope = $rootScope;
    }

    $onInit() {
        this.SearchData = new AFISearchDataReturnToOutlet();
        this.$rootScope.$on("ReturnToOutletKembali", (event) => {
            this.Data = null;
            this.SearchData = new AFISearchDataReturnToOutlet();
            this.pageState = true;
            this.radio = null;
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
    Cari() {
        this.loader = false;
        this.Data = {};
        this.AfiReturnToOutletService.GetRequest(this.SearchData).then(
            response => 
            {
                this.Data = response.data;
                this.loader = true;
            }
        );
    }

    Process() {
        if (this.radio==null) {
            alertify.error("Silahkan pilih salah satu data");
            return;
        }
        this.pageState = false;
        let dataChosen: any = _.find(this.Data, (data: any) => { return data.afiApplicationId == this.radio; });
        if (dataChosen.tipePengajuanName.toLowerCase() == "normal") {
            this.$rootScope.$emit("ReturnToOutlet", dataChosen);
        } else if (dataChosen.tipePengajuanName.toLowerCase().indexOf("rev") >=0) {
            this.$rootScope.$emit("ReturnToOutletRevisi", dataChosen);
        } else if (dataChosen.tipePengajuanName.toLowerCase().indexOf(".r") >= 0) {
            this.$rootScope.$emit("ReturnToOutletExCancel", dataChosen);
        }
    }
    
    
    convertForDatePicker(tanggal: Date) {
        return new Date(Date.UTC(tanggal.getFullYear(), tanggal.getMonth(), tanggal.getDate()));
    }

    tanggalChanged() {
        if (this.SearchData.tanggalPengajuan == null) {
            this.dateOptionsSampai.minDate = null;
            return;
        }
        this.dateOptionsSampai.minDate = new Date(this.SearchData.tanggalPengajuan).setDate(this.SearchData.tanggalPengajuan.getDate() + 1);
        this.SearchData.tanggalPengajuan = this.convertForDatePicker(this.SearchData.tanggalPengajuan);
    }
    tanggalChangedSampai() {
        if (this.SearchData.sampai == null) {
            this.dateOptions.maxDate = null;
            return;
        }
        this.dateOptions.maxDate = new Date(this.SearchData.sampai).setDate(this.SearchData.sampai.getDate() - 1);
        this.SearchData.sampai = this.convertForDatePicker(this.SearchData.sampai);
    }
}

export class AfiReturnToOutlet implements angular.IComponentOptions {
    controller = AfiReturnToOutletController;
    controllerAs = 'me';

    template =  require('./AfiReturnToOutlet.html');
}
export class AFISearchDataReturnToOutlet {
    frameNumber: string = null;
    tanggalPengajuan?: Date = null;
    sampai?: Date = null;
    rbStatus: string = null;
}
