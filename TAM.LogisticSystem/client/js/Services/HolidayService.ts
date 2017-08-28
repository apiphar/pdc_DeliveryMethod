import * as moment from 'moment';
export class HolidayService {
    static $inject = ['$http'];
    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }


    getData() {
        return this.httpClient.get<HolidayData[]>('/api/v1/HolidayApi/GetData');
    }
 

    saveAddData(addedKalender: HolidayData[]) {
        return this.httpClient.post('/api/v1/HolidayApi/SaveAddData/', addedKalender);
        
    }

    saveDelData(deletedKalender: HolidayData[]) {
        return this.httpClient.post('/api/v1/HolidayApi/SaveDelData/', deletedKalender);
    }

    populateLocations() {
        return this.httpClient.get<HolidayData[]>('/api/v1/HolidayApi/PopulateLocations');
    }

    populateYears() {
        return this.httpClient.get('/api/v1/HolidayApi/PopulateYears');
    }


}

export class HolidayData {
    locationCode: string;
    holidayDate: Date;
    name: string;
    stringDate: string;
    dateDate: Date;
    constructor(){
        this.holidayDate = new Date();
    }
}

export class DateKalender {
    locationCode: string;
    holidayDate: Date;
}



