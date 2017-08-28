import * as angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';
export class KonfigurasiGesekanController implements angular.IController {
    $inject = ['KonfigurasiGesekanService'];

    KonfigurasiGesekanService: Service.KonfigurasiGesekanService;

    ExistsScratch: any;
    tempExistsScratch: any;
    Dealer: any;
    CarModel: any;
    Loader: boolean = true;
    cbSelect: boolean;
    cbSelectDealer: boolean;
    jumlahGesekan: number;
    cbShow: boolean = true;

    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;


    constructor(KonfigurasiGesekanService: Service.KonfigurasiGesekanService) {
        this.KonfigurasiGesekanService = KonfigurasiGesekanService;
    }

    $onInit() {
        this.GetExistsScratch();
        this.GetCarModel();
        this.GetDealerBranch();
        
    }
    ShowAll() {
        if (this.cbShow==false) {
            this.ExistsScratch = _.filter(this.ExistsScratch, ['jumlahGesek', null]);
            this.totalItems = this.ExistsScratch.length;
        } else {
            this.ExistsScratch = this.tempExistsScratch;
            this.totalItems = this.ExistsScratch.length;
        }
    }

    camelize(str) :string{
        return str.replace(/(?:^\w|[A-Z]|\b\w|\s+)/g, function (match, index) {
            if (+match === 0) return ""; // or if (/\s+/.test(match)) for white spaces
            return index == 0 ? match.toLowerCase() : match.toUpperCase();
        });
    }

    IsExists(branchName: string, carModelName: string): boolean {
        let name: string = this.camelize(carModelName);
        let data: any = _.filter(this.ExistsScratch, [name, null]);
        return _.find(data, ['dealer', branchName]) ? false : true ;
    }

    GetCarModelList(): string[] {
        var tempCarModelCode = [];
        angular.forEach(_.filter(this.CarModel, ['select', true]),
            function (data: any) {
                tempCarModelCode.push(data.name);
            }
        );
        return tempCarModelCode;
    }

    createBranchCarModel(branchCode, carModelCode):string[]{
        var branchCarModel = []
        branchCarModel.push(branchCode);
        branchCarModel.push(carModelCode);
        return branchCarModel;
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

    SaveConfig() {
        let scratch: ScratchData = new ScratchData();
        let tempBranchCode = [], tempBranchCodeUpdate = [];
        let tempCarModel= _.filter(this.CarModel, ['select', true]);
        let Dealer = _.filter(this.Dealer, ['select', true]);
        let InsertData = [], UpdateData = [];

        if (Dealer.length == 0) {
            Alertify.error("Please Choose Dealer First!");
        } else if (tempCarModel.length == 0) {
            Alertify.error("Please Choose Car Model First!");
        } else {
            angular.forEach(Dealer, (data: any)=> {
                angular.forEach(_.filter(data.branch, ['select', true]),
                    (branch: any) =>{
                        angular.forEach(tempCarModel, (carModel: any) =>{
                            if (!this.IsExists(branch.branchName, carModel.name)) {
                                InsertData.push(this.createBranchCarModel(branch.branchCode, carModel.carModelCode));
                            }
                            else
                                UpdateData.push(this.createBranchCarModel(branch.branchCode, carModel.carModelCode));
                        });
                    }
                );
            });
            scratch.InsertData = InsertData;
            scratch.UpdateData = UpdateData;
            scratch.jumlahGesekan = this.jumlahGesekan;
            this.KonfigurasiGesekanService.SaveConfig(scratch).then(
                response => {
                        Alertify.success("Success Save Configuration");
                        this.GetExistsScratch();
                        this.ShowAll();
                }
            ).catch(response => {
                if (response.status == "400") {
                    Alertify.error(response.data);
                }
            });
        }
    }


    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }


    GetCarModel() {
        this.KonfigurasiGesekanService.GetCarModel().then(
            response =>
            {
                this.CarModel = response.data;
            }
        );
    }
    
    GetExistsScratch() {
        this.KonfigurasiGesekanService.GetExistsScratch().then(
            response => {
                this.ExistsScratch = response.data;
                this.Loader = false;
                this.totalItems = this.ExistsScratch.length;
                this.tempExistsScratch = response.data;
            }
        );
    }

    GetDealerBranch() {
        this.KonfigurasiGesekanService.GetDealerBranch().then(
            response => {
                this.Dealer = response.data;
            }
        );
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }
    setPage(pageNo) {
        this.currentPage = pageNo;
    };

    selectAllModel() {
        var flag = this.cbSelect;
        angular.forEach(this.CarModel, function (data) {
            data.select = flag;
        });
    }

    selectAllDealer() {
        var flag = this.cbSelectDealer;
        angular.forEach(this.Dealer, data => {
            data.select = flag;
            this.selectAllBranch(data);
        },this);
    }

    selectAllBranch(Data) {
        var Branch:any = _.find(this.Dealer, Data);
        angular.forEach(Branch.branch, function (data) {
            data.select = Data.select;
        });
    }

    selectParent(Data) {
        var Dealer: any = _.find(this.Dealer, ['dealerCode', Data.dealerCode]);
        var Branch: any = _.filter(Dealer.branch, ['select', true]);
        if (Branch.length == 0)
            Dealer.select = false;
        else {
            Dealer.select = true;
        }
    }
}

export class ScratchData {
    UpdateData: string[];
    InsertData: string[];
    jumlahGesekan: number;
}

export class BranchCarModel {
    branchCode: string;
    carModelCode: string;
}

export class KonfigurasiGesekan implements angular.IComponentOptions {
    controller = KonfigurasiGesekanController;
    controllerAs = 'me';

    template = require('./KonfigurasiGesekan.html');
}