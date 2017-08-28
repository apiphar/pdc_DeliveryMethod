

export class SPUDefaultLeadTimeConfigurationPageViewModel {
    locations: SPUDefaultLeadTimeConfigurationLocationModel[];
    spuDefaultLeadTimes: [{
        leadTime: number,
        routingMasterCode: string,
        locationName: string,
        processStatus: string,
        locationCode: string,
        processForLineId: number
    }];
    processStatusList: ProcessStatusViewModels[];
}

export class SPUDefaultLeadTimeConfigurationLocationModel {
    locationCode: string;
    locationName: string;
}

export class ProcessStatusViewModels {
    processForLineId: number;
    name: string;
}

export class SPULeadTimeViewModel {
    day: number = 0;
    hour: number = 0;
    minute: number = 0;
}

export class SPUDefaultLeadTimeConfigurationUpdateFormViewModel {
    routingMasterCode: string;
    locationCode: string;
    processForLineId: number;
    totalLeadTimeMinutes: number;
}

export class SPUCreateDefaultLeadTimeConfigurationViewModel {
    locationCode: string;
    processForLineId: number;
    totalLeadTimeMinutes: number;
}

export class SPUUpdateDefaultLeadTimeConfigurationViewModel {
    routingMasterCode: string;
    oldLocationCode: string;
    newLocationCode: string;
    oldProcessForLineId: number;
    newProcessForLineId: number;
    totalLeadTimeMinutes: number;
}

export class SPUDeleteDefaultLeadTimeConfigurationViewModel {
    routingMasterCode: string;
    locationCode: string;
    processForLineId: number;
}

export class SPUDefaultLeadTimeConfigurationService {
    static $inject = ["$http"];
    
    constructor(httpClient: angular.IHttpService) {
        this.httpClient = httpClient;
    }

    httpClient: angular.IHttpService;

    // Retrieve the SPUDefaultLeadTimeConfigurationPageViewModel from the server
    getData() {
        return this.httpClient.get<SPUDefaultLeadTimeConfigurationPageViewModel>("/api/v1/SPUDefaultLeadTimeConfigurationapi/Get");
    }

    // Call the create a new SPU Default Lead Time method in the SPUDefaultLeadTimeConfiguration controller
    createSPUDefaultLeadTimeConfiguration(submitSPUDefaultLeadTimeConfiguration: SPUCreateDefaultLeadTimeConfigurationViewModel) {
        return this.httpClient.post("/api/v1/SPUDefaultLeadTimeConfigurationapi/Create", submitSPUDefaultLeadTimeConfiguration);
    }

    // Call the update SPU Default Lead Time method in the SPUDefaultLeadTimeConfiguration controller
    updateSPUDefaultLeadTimeConfiguration(submitSPUDefaultLeadTimeConfiguration: SPUUpdateDefaultLeadTimeConfigurationViewModel) {
        return this.httpClient.post("/api/v1/SPUDefaultLeadTimeConfigurationapi/Update/", submitSPUDefaultLeadTimeConfiguration);
    }

    // Call the delete SPU Default Lead Time method in the SPUDefaultLeadTimeConfiguration controller
    deleteSPUDefaultLeadTimeConfiguration(spuDeleteDefaultLeadTimeConfiguration:SPUDeleteDefaultLeadTimeConfigurationViewModel) {
        return this.httpClient.delete("/api/v1/SPUDefaultLeadTimeConfigurationapi/Delete", { params: spuDeleteDefaultLeadTimeConfiguration });
    }
}