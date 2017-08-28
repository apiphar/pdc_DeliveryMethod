export class DeliveryLegPageViewModel {
    viewModels: DeliveryLegViewModel[];
    deliveryLegLocations: DeliveryLegLocationViewModel[];
    cityLegCodes: DeliveryLegCityListViewModel[];
}

export class DeliveryLegViewModel {
    deliveryLegCode: string;
    name: string;
    locationFrom: string;
    locationFromString: string;
    locationTo: string;
    locationToString: string;
    cityLegCode: string;
    cityLegCodeString: string;
    bufferMinutes: number;
    needSJKB: string;
}

export class DeliveryLegPostPutViewModel {
    deliveryLegCode: string;
    name: string;
    locationFrom: string;
    locationTo: string;
    cityLegCode: string;
    bufferMinutes: number;
    needSJKB: boolean;
}

export class DeliveryLegLocationViewModel {
    locationCode: string;
    name: string;
}

export class DeliveryLegCityListViewModel {
    cityLegCode: string;
    cityLegName: string;
}

export class DeliveryLegService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    // Get all on load
    getAll() {
        return this.HttpClient.get<DeliveryLegPageViewModel>('api/v1/DeliveryLegApi');
    }

    // Send data to database
    sendData(deliveryLegPostPutViewModel: DeliveryLegPostPutViewModel) {
        return this.HttpClient.post('api/v1/DeliveryLegApi/SendData', deliveryLegPostPutViewModel);
    }

    // Update data to database
    updateData(deliveryLegPostPutViewModel: DeliveryLegPostPutViewModel) {
        return this.HttpClient.put('api/v1/DeliveryLegApi/UpdateData', deliveryLegPostPutViewModel);
    }

    // Delete data from database
    deleteData(deliveryLegCode: string) {
        return this.HttpClient.delete('api/v1/DeliveryLegApi/Delete/' + deliveryLegCode);
    }

}