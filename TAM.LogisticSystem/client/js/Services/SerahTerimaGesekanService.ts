import * as angular from 'angular';
import * as Component from '../components';
export class SerahTerimaGesekanService {
    $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }
    //get all serah terima gesekan when scratch hand over number == null
    serahTerimaGesekan() {
        return this.HttpClient.get<SerahTerimaGesekanViewModel[]>("/api/v1/SerahTerimaGesekanApi/SerahTerimaGesekan");
    }
    //save scratch hand over number and update scratch and download excel file
    generate(dataInput: SerahTerimaGesekanInputViewModel) {
        return this.HttpClient.post("/api/v1/SerahTerimaGesekanApi/Generate", dataInput);
    }
}

export class SerahTerimaGesekanViewModel {
    scratchId: number;
    vehicleId: number;
    frameNumber: string;
    tanggalGesek: Date;
    jumlahGesek: number;
    lokasi: string;
    katashiki: string;
    suffix: string;
    modelName: string;
    color: string;
    branch: string;
    customerAssign: boolean;
    requestedPDD: Date;
    select: boolean = false;
}

export class SerahTerimaGesekanExcelViewModel {
    frameNumber: string;
    tanggalGesek: Date;
    jumlahGesek: number;
    lokasi: string;
    katashiki: string;
    suffix: string;
    modelName: string;
    color: string;
    branch: string;
    customerAssign: boolean;
    requestedPDD: Date;
}

export class SerahTerimaGesekanInputViewModel {
    vehicleId: number[]=[];
    noSurat: string;
    tanggal: Date;
    excelModel: SerahTerimaGesekanExcelViewModel[]=[];
}