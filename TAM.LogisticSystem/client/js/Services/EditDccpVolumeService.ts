
import * as services from '../services';
export class EditDccpVolumeService {
    static $inject = ["$http", "DccpReadinessVolumeService"];
    constructor(httpClient: angular.IHttpService, drs: services.DccpReadinessVolumeService) {
        this.httpClient = httpClient;
        this.dccpReadinessService = drs;
    }

    dccpReadinessService: services.DccpReadinessVolumeService;
    
    updateData(DccpModel: services.DccpReadinessVolumeModel) {
       return this.httpClient.post<any>("/api/v1/DccpReadinessVolumeApi", DccpModel);
    }
    httpClient: angular.IHttpService;
}

export class DccpEditModel {
    id: number; 
    tanggal: string;
    asal: string;
    tujuan: string;
    trip: number;
    load: number;
    shift: string;
    qty: number;
    adjust?: number;
    estimasi?: number;
}