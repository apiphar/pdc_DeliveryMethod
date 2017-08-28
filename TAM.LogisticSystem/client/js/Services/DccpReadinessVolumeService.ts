
export class DccpReadinessVolumeService {
    static $inject = ["$http"];
    constructor(httpClient: angular.IHttpService) {
        this.httpClient = httpClient;
    }
    httpClient: angular.IHttpService;

    //delete selected dccp service
    deleteData(id: number) {
        return this.httpClient.delete("/api/v1/DccpReadinessVolumeApi/" + id);
    }
    //get all dccp service
    getData() {
        return this.httpClient.get<DccpReadinessVolumeModel[]>("/api/v1/DccpReadinessVolumeApi");
    }
    //update selected dccp service
    updateData(DccpModel: DccpReadinessVolumeModel) {
        return this.httpClient.post<any>("/api/v1/DccpReadinessVolumeApi", DccpModel);
    }
}
export class DccpReadinessVolumeModel {
    id: number;
    tanggal: string;
    asal: string;
    tujuan: string;
    trip: number;
    load: number;
    shift: string;
    qty: number;
    adjust: number;
    estimasi: number;
}