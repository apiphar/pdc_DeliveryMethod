import * as Angular from 'Angular'

export class MaintenanceWaktuBreakService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getMaintenanceWaktuBreak() {
        return this.HttpClient.get<string>('/MaintenanceWaktuBreakApi/GetMaintenanceWaktuBreak');
    }

    getLocation() {
        return this.HttpClient.get<string>('/MaintenanceWaktuBreakApi/GetLocation');
    }

    getShift() {
        return this.HttpClient.get<string>('/MaintenanceWaktuBreakApi/GetShift');
    }

    postData(locationCode: String, shiftCode: String, dateFrom: Date, dateTo: Date) {
        return this.HttpClient.post('/MaintenanceWaktuBreakApi/Create', { locationCode, shiftCode, dateFrom, dateTo });
    }

    updateData(idleTimeCustomId: number, locationCode: String, shiftCode: String, dateFrom: Date, dateTo: Date) {
        return this.HttpClient.post('/MaintenanceWaktuBreakApi/edit/' + idleTimeCustomId, { locationCode, shiftCode, dateFrom, dateTo });
    }

    deleteData(idleTimeCustomId: number) {
        return this.HttpClient.post('/MaintenanceWaktuBreakApi/delete/' + idleTimeCustomId, {});
    } 
}