

export class MasterWarnaVehicleService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }
    //get all data location type
    getAllVehicleColor() {
        return this.httpClient.get<MasterWarnaVehicleViewModel[]>('/api/v1/MasterWarnaVehicleApi');
    }

    getAllKodeBrand() {
        return this.httpClient.get<MasterWarnaVehicleBrandModel[]>('/api/v1/MasterWarnaVehicleApi/GetAllKodeBrand');
    }

    getAllKodeModel() {
        return this.httpClient.get<MasterWarnaVehicleModelModel[]>('/api/v1/MasterWarnaVehicleApi/GetAllKodeModel');
    }

    getAllKodeWarna() {
        return this.httpClient.get<MasterWarnaVehicleColorModel[]>('/api/v1/MasterWarnaVehicleApi/GetAllKodeWarna');
    }

    //add new location type
    postVehicleColor(vehicleColorModel: MasterWarnaVehicleCreateModel) {
        return this.httpClient.post('/api/v1/MasterWarnaVehicleApi', vehicleColorModel);
    }
    //udpate location type
    updateVehicleColor(vehicleColorModel: MasterWarnaVehicleCreateModel) {
        return this.httpClient.post('/api/v1/MasterWarnaVehicleApi/edit/', vehicleColorModel);
    }
    //delete location type
    deleteVehicleColor(kodeWarnaVehicle: string) {
        return this.httpClient.delete('/api/v1/MasterWarnaVehicleApi/delete/' + kodeWarnaVehicle);
    }
}

export class MasterWarnaVehicleViewModel {
    kodeWarnaVehicle: string;
    kodeBrand: string;
    brandName: string;
    brandDetail: string;
    kodeModel: string;
    modelName: string;
    modelDetail: string;
    kodeWarna: string;
    deskripsiWarnaInd: string;
    deskripsiWarnaEng: string;
}

export class MasterWarnaVehicleCreateModel {
    kodeWarnaVehicle: string;
    brand: MasterWarnaVehicleBrandModel;
    model: MasterWarnaVehicleModelModel;
    warna: MasterWarnaVehicleColorModel;
}

export class MasterWarnaVehicleBrandModel {
    kodeBrand: string;
    brandName: string;
}

export class MasterWarnaVehicleModelModel {
    kodeBrand: string;
    kodeModel: string;
    modelName: string;
}

export class MasterWarnaVehicleColorModel {
    kodeWarna: string;
    deskripsiWarnaInd: string;
    deskripsiWarnaEng: string;
}
