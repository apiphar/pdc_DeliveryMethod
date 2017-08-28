export class GenerateJamBreakPageViewModel {
    breakHourTemplates: BreakHourTemplateViewModel[];
    location: BreakHourLocationViewModel[];
}

export class BreakHourTemplateViewModel {
    breakHourTemplateCode: string;
    description: string;
}

export class BreakHourLocationViewModel {
    locationCode: string;
    locationName: string;
}

export class LocationBreakHour {
    breakHourTemplateCode: string;
    location: BreakHourLocationViewModel;
    validFrom: Date;
    validTo: Date;
}

export class GenerateJamBreakService {
    static $inject = ['$http'];

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }
    httpClient: angular.IHttpService;

    // Get all
    getAll() {
        return this.httpClient.get<GenerateJamBreakPageViewModel>('../api/v1/GenerateJamBreakApi');
    }

    checkDuplicate(idleTime: LocationBreakHour) {
        return this.httpClient.post('../api/v1/GenerateJamBreakApi/CheckDuplicate', idleTime);
    }

    // Generate idleTime
    generateData(idleTime: LocationBreakHour) {
        return this.httpClient.post('../api/v1/GenerateJamBreakApi/GenerateData', idleTime);
    }
}