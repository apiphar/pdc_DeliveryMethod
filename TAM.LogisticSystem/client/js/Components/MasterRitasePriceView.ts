
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as Service from '../services';

class MasterRitasePriceViewController implements angular.IController {

    static $inject = ['masterRitasePriceService','$rootScope'];

    constructor(masterRitasePriceService: Service.MasterRitasePriceService, root: angular.IRootScopeService) {
        this.masterRitasePriceService = masterRitasePriceService;
        this.root = root;
    }



    //property
    masterRitasePriceService: Service.MasterRitasePriceService;
    masterRitasePriceViewModels: Service.MasterRitasePriceViewModel[];
    confirmRitaseModel: Service.MasterRitasePriceViewModel;
    searchInput: Service.Search;
    root: angular.IRootScopeService;
    flag: boolean; //buat flag add form dan edit form
    flagRefresh: boolean;



    //function
    $onInit() {
        this.flag = true;
        this.broadcastFlag();
        this.getAllRitasePriceService();


    }

    //broadcastFlag
    broadcastFlag() {
        this.root.$broadcast('flag', this.flag);
    }

    //get all ritase data
    getAllRitasePriceService() {
        this.masterRitasePriceService.getAllRitaseData().then(result => {
            this.masterRitasePriceViewModels = result.data as Service.MasterRitasePriceViewModel[];
            angular.forEach(this.masterRitasePriceViewModels, result => {
                if (result.isSingleTrip.toString() === "true") {
                    console.log("True");
                    result.isSingleTrip = "Round Trip";
                } else if (result.isSingleTrip.toString() === "false") {
                    result.isSingleTrip = "Single Trip";
                }

            })
            this.totalItems = this.masterRitasePriceViewModels.length;
            console.log("berhasil");


        }).catch(e => {
            console.log("gagal");
        })
    }



    //delete ritase data by id
    deleteData(data) {
        this.confirmRitaseModel = data;
        alertify.confirm(
            "Anda Serius mau menghapus " + data.cityLegRitaseCostId+ " ?",
            () => {
                this.masterRitasePriceService.deleteData(data.cityLegRitaseCostId).then(message => {
                    console.log("succses hapus");
                    this.getAllRitasePriceService();
                }).catch(message => {
                    console.log("gagal hapus");
                })
                
            },
            () => {


            });

    }


    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

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

}

let MasterRitasePriceView = {
    controller: MasterRitasePriceViewController,
    controllerAs: 'me',
    template: require("./MasterRitasePriceView.html")
}

export { MasterRitasePriceView };
