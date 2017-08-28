import * as angular from 'angular';
import * as service from '../services';
import * as moment from 'moment';


class DeliveryUnitLoadingDetailController implements angular.IController {

    static $inject = ['deliveryUnitLoadingDetailService','$rootScope'];

    constructor(deliveryUnitLoadingDetailService: service.DeliveryUnitLoadingDetailService, $rootScope: angular.IRootScopeService) {
        this.deliveryUnitLoadingDetailService = deliveryUnitLoadingDetailService;
        this.$rootScope = $rootScope;
    }



    //property
    deliveryUnitLoadingDetailService: service.DeliveryUnitLoadingDetailService;
    searchInput: service.SearchData;
    $rootScope: angular.IRootScopeService;
    //service
    unitLoadingDetailModel: service.DeliveryUnitLoadingDetailModel[];
    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;
    voyageNumber: string;
    loader: boolean;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    flag: boolean;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };


    //method


    //set date to string for filter
    convertDateToString() {
        angular.forEach(this.unitLoadingDetailModel, data => {
            if (data.requestedDeliveryTime != null) {
                data.dateTemp = moment(data.requestedDeliveryTime).format('DD-MMM-YYYY ');
            }
            if (data.estimatedPDCIn != null) {
                data.dateTempEPDC = moment(data.estimatedPDCIn).format('DD-MMM-YYYY ');
            }    
        })
    }

    //get All Car Detail where status != depart
    getAlltUnitLoadingDetail(voyageNumber: string) {
        this.deliveryUnitLoadingDetailService.getAlltUnitLoadingDetail(voyageNumber).then(result => {
            this.unitLoadingDetailModel = result.data as service.DeliveryUnitLoadingDetailModel[];
            this.convertDateToString();
            this.totalItems = this.unitLoadingDetailModel.length;
            this.loader = false;
            this.order('dateTempEPDC');
        }).catch(result => {
        })
    }

    //back to unitloading menu
    back() {
        this.$rootScope.$broadcast('flag', this.voyageNumber);
        this.flag = false;
    }

    //clear Search on Table Grid
    clearingTable() {
        this.searchInput = null;
        this.searchInput = new service.SearchData();
        this.unitLoadingDetailModel = new Array<service.DeliveryUnitLoadingDetailModel>();
    }

    //get voyage Numebr
    onBroadcast() {
        this.$rootScope.$on('voyageNumber', (event, data)=> {
            this.voyageNumber = data;
            this.flag = true;
            this.getAlltUnitLoadingDetail(this.voyageNumber);
            this.loader = true;
            this.clearingTable();        
        })
       
    }


    $onInit() {
        this.onBroadcast();  
    }
}

let DeliveryUnitLoadingDetail = {
    controller: DeliveryUnitLoadingDetailController,
    controllerAs: 'me',
    template: require('./DeliveryUnitLoadingDetail.html')
}

export { DeliveryUnitLoadingDetail }