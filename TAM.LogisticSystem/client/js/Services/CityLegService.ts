export class CityLegPageViewModel {
    viewModels: CityLegViewModel[];
    cityList: CityLegLocation[];
}

export class CityLegViewModel {
    cityLegCode: string;
    cityLegName: string;
    cityFrom: string;
    cityFromGrid: string;
    cityTo: string;
    cityToGrid: string;
    calculatingSwappingCost: string;
}

export class CityLegSendViewModel {
    cityLegCode: string;
    cityLegName: string;
    cityFrom: string;
    cityFromGrid: string;
    cityTo: string;
    cityToGrid: string;
    calculatingSwappingCost: boolean;
}

export class CityLegLocation {
    cityForLegCode: string;
    cityName: string;
}

export class CityLegService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    // Get all data on load
    getAll() {
        return this.HttpClient.get<CityLegPageViewModel>('../api/v1/CityLegApi');
    }

    // Send data
    sendData(viewModel: CityLegSendViewModel) {
        return this.HttpClient.post('../api/v1/CityLegApi/Create', viewModel);
    }

    // Update data
    updateData(viewModel: CityLegSendViewModel) {
        return this.HttpClient.put('../api/v1/CityLegApi/Update', viewModel);
    }

    // Delete data by code
    deleteData(cityLegCode: string) {
        return this.HttpClient.delete('../api/v1/CityLegApi/Delete/' + cityLegCode);
    }
}




