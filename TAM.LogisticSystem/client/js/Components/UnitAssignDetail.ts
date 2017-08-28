import * as angular from 'angular';
import * as service from '../services/UnitAssignDetailService';
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as moment from 'moment';

class UnitAsssignDetailController implements angular.IController {

    static $inject = ["UnitAssignDetailService", "$rootScope"];

    constructor(AssignByUnitDetailService: service.UnitAssignDetailService, root: angular.IRootScopeService) {
        this.unitAssignDetailService = AssignByUnitDetailService;
        this.root = root;
        this.showDetail = false;
    }

    //private readonly style
    unitAssignDetailService: service.UnitAssignDetailService;
    root: angular.IRootScopeService

    //Data Types
    showDetail: boolean;
    unitListDetailByVoyage: service.UnitListByVoyage[];
    searchTable = {};

    $onInit() {
        this.root.$on("Delivery_AssignByUnit-ShowDetail", (event, data) => {
            this.showDetail = data;
        });

        this.root.$on("Delivery_AssignByUnit-Voyage", (event, data) => {
            this.unitAssignDetailService.getUnitList(data).then(response => {
                this.unitListDetailByVoyage = response.data as service.UnitListByVoyage[];

                this.totalItems = response.data.length;

                lodash.each(this.unitListDetailByVoyage, function (Q) {
                    Q.customerAssignModel = (Q.customerAssign) ? "Ya" : "Tidak";
                });
            })
        });
    }

    //Send Trigger to show the component of assign(back to the form)
    showAssignByUnit() {
        this.searchTable = {};
        this.root.$broadcast("ShowAssign", true);
        this.showDetail = false;
        this.unitListDetailByVoyage = new Array<service.UnitListByVoyage>();
    };

    //Paging Section
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
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

export class UnitAsssignDetail implements angular.IComponentOptions {
    controller = UnitAsssignDetailController;
    controllerAs = 'me';
    template = require("./UnitAssignDetail.html");
}
