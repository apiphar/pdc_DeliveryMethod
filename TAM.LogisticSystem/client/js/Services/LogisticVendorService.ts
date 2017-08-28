

export class LogisticVendorService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }


    getAllData() {
        return this.HttpClient.get<DeliveryVendorViewModel[]>('/logistic-vendor/API/get-all');
    }

    getAllLocation() {
        return this.HttpClient.get<DeliveryVendorLocationViewModel[]>('/logistic-vendor/API/get-location');
    }


    createData(deliveryVendorViewModel: DeliveryVendorCreateModel) {
        console.log(deliveryVendorViewModel);
        return this.HttpClient.post<number>('/logistic-vendor/API/create', deliveryVendorViewModel);
    }

    updateData(deliveryVendorViewModel: DeliveryVendorCreateModel) {
        return this.HttpClient.post<number>('/logistic-vendor/API/edit', deliveryVendorViewModel);
    }

    deleteData(deliveryVendorCode: string) {
        return this.HttpClient.delete<number>('/logistic-vendor/API/delete/' + deliveryVendorCode);
    }
}

export class DeliveryVendorViewModel {
    deliveryVendorCode: string;
    name: string;
    address: string;
    locationCode: string;
    sapCode: string;
    account: string;
 
}

export class DeliveryVendorCreateModel {
    deliveryVendorCode: string;
    name: string;
    address: string;
    location: DeliveryVendorLocationViewModel;
    sapCode: string;
    account: string;
    
}

export class DeliveryVendorLocationViewModel {
    locationCode: string;
    name: string;
}