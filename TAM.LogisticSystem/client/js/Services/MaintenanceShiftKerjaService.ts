

export class MaintenanceShiftKerjaService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }
    getData() {
        return this.HttpClient.get<MaintenanceShiftKerjaFullModel>("api/v1/MaintenanceShiftKerjaApi");
    }

    postData(data: MaintenanceShiftKerjaPostViewModel) {
        return this.HttpClient.post('api/v1/MaintenanceShiftKerjaApi', data);
    }
    updateData(data: MaintenanceShiftKerjaPostViewModel) {
        return this.HttpClient.put('api/v1/MaintenanceShiftKerjaApi', data);
    }
    deleteData(id: number) {
        return this.HttpClient.delete('api/v1/MaintenanceShiftKerjaApi/'+ id);
    }
}   
export class MaintenanceShiftKerjaFullModel {
    maintenanceShiftKerjaFullModel: MaintenanceShiftKerjaPostViewModel[];
    shiftModel: ShiftModel[];
    lokasiModel: LokasiModel[];
}
export class MaintenanceShiftKerjaPostViewModel {
    locationWorkHourId: number;
    locationCode: string; 
    shiftCode: string;
    finish: Date;
    start: Date;
    penampungDateTo: string;
    penampungDateFrom: string;
}
export class ShiftModel {
    shiftCode: string;
    description: string;
}
export class LokasiModel {
    locationCode: string;
    nama: string;
}