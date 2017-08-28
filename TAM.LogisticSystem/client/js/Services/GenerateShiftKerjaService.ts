export class GenerateShiftKerjaPageViewModel {
    workHourTemplates: WorkHourTemplateViewModel[];
    locationCodes: string[];
}

export class WorkHourTemplateViewModel {
    workHourTemplateCode: string;
    description: string;
}

export class LocationWorkHourViewModel {
    workHourTemplateCode: string;
    locationCode: string;
    validFrom: Date;
    validTo: Date;
}

export class GenerateShiftKerjaService {
    static $inject = ['$http'];

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }
    httpClient: angular.IHttpService;

    // Get all
    getAll() {
        return this.httpClient.get<GenerateShiftKerjaPageViewModel>('../api/v1/GenerateShiftKerjaApi');
    }

    // Send Data
    generateData(workingTime: LocationWorkHourViewModel) {
        return this.httpClient.post('../api/v1/GenerateShiftKerjaApi/GenerateData', workingTime);
    }
}