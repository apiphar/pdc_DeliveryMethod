export class LocationService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    //Get Semua Data
    getAllData() {
        return this.HttpClient.get<LocationPageViewModel>('/api/v1/lokasi/GetAllData');
    }

    //Insert
    postData(locationCode, locationName, alamat, locationTypeCode, cityForLegCode, cityForShipmentCode, cetakSJKB) {
        return this.HttpClient.post<number>('/api/v1/lokasi/', { locationCode, locationName, alamat, locationTypeCode, cityForLegCode, cityForShipmentCode, cetakSJKB });
    }

    //Update
    updateData(locationCode, locationName, alamat, locationTypeCode, cityForLegCode, cityForShipmentCode, cetakSJKB) {
        return this.HttpClient.post<number>('/api/v1/lokasi/edit', { locationCode, locationName, alamat, locationTypeCode, cityForLegCode, cityForShipmentCode, cetakSJKB });
    }

    //Delete
    deleteData(locationCode: string) {
        return this.HttpClient.delete<number>('/api/v1/lokasi/' + locationCode);
    }
}
//Class untuk semua data yang perlu di GET
export class LocationPageViewModel {
    location: LocationViewModel[];
    locationType: string;
    cityForLeg: string;
    cityForShipment: string;
}

//Class View Model
export class LocationViewModel {
    locationCode: string;
    locationName: string;
    alamat: string;
    cityForLegCode: string;
    cityForLegName: string;
    cityForLegView: string;
    cityForShipmentCode: string;
    cityForShipmentName: string;
    cityForShipmentView: string;
    locationTypeCode: string;
    locationTypeName: string;
    locationTypeView: string;
    cetakSJKB: boolean;
}