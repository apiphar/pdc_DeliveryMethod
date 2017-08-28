import * as service from '../services';
import * as moment from 'moment';
import * as alertify from 'alertifyjs';

class VesselUnitListController implements angular.IController {
    static $inject = ['vesselDepartService', '$rootScope'];

    constructor(vesselUnitListService: service.VesselDepartService, root: angular.IRootScopeService) {
        this.vesselDepartService = vesselUnitListService;
        this.root = root;
    }
    vesselDepartService: service.VesselDepartService;
    root: angular.IRootScopeService;

    unitListViewModel: service.UnitListViewModel[];
    search = {};
    pageState: boolean = false;

    $onInit() {
        this.root.$on("Details", (event, data) => {
            this.pageState = true;
            this.getUnitList(data);
        });
    }

    getUnitList(voyageNumber: string) {
        return this.vesselDepartService.getUnitList(voyageNumber).then(response => {
            this.unitListViewModel = response.data as service.UnitListViewModel[];
            for (let i in this.unitListViewModel) {
                let tempPdcString = moment(this.unitListViewModel[i].pdcIn).format('DD-MMM-YYYY');
                this.unitListViewModel[i].pdcInString = tempPdcString;
                let tempDateString = moment(this.unitListViewModel[i].requestedPdd).format('DD-MMM-YYYY');
                this.unitListViewModel[i].requestedPddString = tempDateString;
            }
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    kembali() {
        this.pageState = false;
        this.search = {};
        this.root.$emit("Kembali");
    }

    // Pagination
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;
    
    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }
}

let VesselUnitListComponent = {
    template: require('./VesselUnitList.html'),
    controllerAs: 'me',
    controller: VesselUnitListController
}

export { VesselUnitListComponent }