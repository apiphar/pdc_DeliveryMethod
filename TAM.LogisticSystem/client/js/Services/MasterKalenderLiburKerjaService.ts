

﻿

export class MasterKalenderLiburKerjaService {
    static $inject = ['$http'];
    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    

    saveData(locationCode: string, selectedDates: any) {
        return this.HttpClient.post('api/MasterKalenderLiburKerjaApi/', { locationCode, selectedDates});
    }

    populateLocations() {
        return this.HttpClient.get('api/MasterKalenderLiburKerjaApi/')
    }

    populateYears() {
        return this.HttpClient.get('/MasterKalenderLiburKerjaApi/PopulateYears')
    }
}


