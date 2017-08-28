
import * as alertify from 'alertifyjs';

export class BranchViewModel {
    branchCode: string;
    branchName: string;
}

export class VehicleViewModel {
    frameNumber: string;
    model: string;
    series: string;
    katashiki: string;
    suffix: string;
}

export class UIStandarizationController implements angular.IController {
    constructor() {
        let firstBranch: BranchViewModel = {
            branchCode: "JKT48",
            branchName: "Jakarta"
        }
        let secondBranch: BranchViewModel = {
            branchCode: "AKB45",
            branchName: "Akihabara"
        }
        this.branches = new Array<BranchViewModel>();
        this.branches.push(firstBranch, secondBranch);

        let vehicle1: VehicleViewModel = {
            frameNumber: "asd123",
            model: "Avanza",
            series: "Avanza Extreme",
            katashiki: "AVZ89009AB",
            suffix: "01"
        }
        let vehicle2: VehicleViewModel = {
            frameNumber: "sdf435",
            model: "Agya",
            series: "Agya Turbo",
            katashiki: "AGY100001AB",
            suffix: "00"
        }
        let vehicle3: VehicleViewModel = {
            frameNumber: "sdf435",
            model: "Agya",
            series: "Agya Turbo",
            katashiki: "AGY100001AB",
            suffix: "00"
        }
        let vehicle4: VehicleViewModel = {
            frameNumber: "sdf435",
            model: "Agya",
            series: "Agya Turbo",
            katashiki: "AGY100001AB",
            suffix: "00"
        }
        let vehicle5: VehicleViewModel = {
            frameNumber: "sdf435",
            model: "Agya",
            series: "Agya Turbo",
            katashiki: "AGY100001AB",
            suffix: "00"
        }
        let vehicle6: VehicleViewModel = {
            frameNumber: "sdf435",
            model: "Agya",
            series: "Agya Turbo",
            katashiki: "AGY100001AB",
            suffix: "00"
        }
        this.vehicles = new Array<VehicleViewModel>();
        this.vehicles.push(vehicle1, vehicle2, vehicle3, vehicle4, vehicle5, vehicle6);
        this.totalItems = this.vehicles.length;
    }

    hourStep = 1;
    minuteStep = 1;

    deliveryDate: Date;

    branches: BranchViewModel[];
    vehicles: VehicleViewModel[];

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;


    submitFromOutside(formName: angular.IFormController) {
        alertify.confirm('Confirm Message');

        if (formName.$valid === true) {
            this.submitForm(formName);
        }
    }

    submitForm(formName: angular.IFormController) {
        console.log("Success");
    }

    //set current page
    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

}

let uiStandarization = {
    controller: UIStandarizationController,
    controllerAs: "me",
    template: require("./UIStandarization.html")
};

export { uiStandarization };