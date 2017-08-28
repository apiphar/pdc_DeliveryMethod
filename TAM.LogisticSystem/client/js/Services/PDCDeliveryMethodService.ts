import * as angular from "angular";

export class PDCDeliveryMethodService {
    static $inject = ["$http"];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getId(locationCode: string, branchCode: string) {
        return this.HttpClient.get<string>('/api/v1/PDCDeliveryMethodApi/GetId/' + locationCode + '/' + branchCode, {});
    }

    getAll() {
        return this.HttpClient.get<PDCDeliveryViewModel[]>("/api/v1/PDCDeliveryMethodApi/GetAllData");
    }

    getBranch() {
        return this.HttpClient.get<PDCBranchViewModel[]>("/api/v1/PDCDeliveryMethodApi/GetBranches");
    }

    getLocation() {
        return this.HttpClient.get<PDCLocationViewModel[]>("/api/v1/PDCDeliveryMethodApi/GetLocations");
    }

    getDeliveryMethod() {
        return this.HttpClient.get<PDCDeliveryMethodViewModel[]>("/api/v1/PDCDeliveryMethodApi/GetDeliveries");
    }


    // Send data
    postData(viewModel: PDCDeliveryCreateViewModel[]) {
        return this.HttpClient.post("../api/v1/PDCDeliveryMethodApi/Create", viewModel);
    }

    // Delete data by code
    deleteData(viewModel: PDCDeliveryDeleteViewModel) {
        return this.HttpClient.delete("../api/v1/PDCDeliveryMethodApi/Delete", { params: viewModel });
    }
}

export class PDCDeliveryViewModel {
    branchCode: string;
    locationCode: string;
    deliveryMethodCode: string;
    locationData: string;
    branchData: string;
    deliveryMethodData: string;
}

export class PDCDeliveryTempViewModel {
    branchCode: string;
    locationCode: string;
    deliveryMethodCode: string;
    ordering: number;
    deliveryMethodName: string;
    branchName: string;
    locationName: string;
    locationData: any;
    branchData: any;
    deliveryMethodData: any;
}


export class PDCDeliveryCreateViewModel {
    branchCode: any;
    locationCode: any;
    deliveryMethodCode: any;
    ordering: number;
}

export class PDCDeliveryDeleteViewModel {
    branchCode: string;
    locationCode: string;
    deliveryMethodCode: string;
    locationData: any;
    branchData: any;
    deliveryMethodData: any;
}

export class PDCDeliveryDeleteDetailModel {
    branchCode: string;
    branchName: string
    locationCode: string;
    locationName: string
    deliveryMethodCode: string;
    deliveryMethodName: string;
    ordering: number;
}

export class PDCBranchViewModel {
    branchCode: string;
    branchName: string;
    branchData: any;
}

export class PDCLocationViewModel {
    locationCode: string;
    name: string;
    locationData: any;
}

export class PDCDeliveryMethodViewModel {
    deliveryMethodCode: string;
    name: string;
    deliveryMethodData: string;
}