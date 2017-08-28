
import * as Component from '../components';
export class GesekNoRangkaService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    } 

    //check data apakah ada dan sudah terdaftar di database atau belum
    checkDataByFrameNo(frameNo: string) {
        return this.HttpClient.get<GesekNoRangkaData>("/api/v1/GesekNoRangkaApi/CheckDataByFrameNo/" + frameNo);
    }
    //simpan data
    saveGesekan(data: GesekNoRangkaInputModel) {
        return this.HttpClient.post('/api/v1/GesekNoRangkaApi/SaveGesekan', data);
    }
    //Get Location data
    getLocation() {
        return this.HttpClient.get<GesekNoRangkaLocationViewModel>("/api/v1/GesekNoRangkaApi/GetLocationData");
    }
}

export class GesekNoRangkaData {
    vehicleId: number;
    katashiki: string;
    suffix: string;
    color: string;
    jenis: string;
    model: string;
    branchCode: string;
    branchName: string;
    jumlahGesek: number;
    lokasi: string;
}

export class GesekNoRangkaConfirmation {
    frameNumber: string;
    jumlahGesek: number;
}

export class GesekNoRangkaInputModel {
    vehicleId: number;
    locationCode: string;
}

export class GesekNoRangkaLocationViewModel {
    locationCode: string;
    name: string;
}