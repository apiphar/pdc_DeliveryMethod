

export class PIODefaultLeadTimeConfigurationPageViewModel {
    locations: PIODefaultLeadTimeConfigurationLocationModel[];
    pioDefaultLeadTimes: [{
        leadTime: number,
        leadTimeToString: string,
        routingMasterCode: string,
        locationName: string,
        processStatus: string,
        locationCode: string,
        processForLineId: number
    }];
    processStatusList: ProcessStatusViewModel[];
}

export class PIODefaultLeadTimeConfigurationLocationModel {
    locationCode: string;
    locationName: string;
}

export class ProcessStatusViewModel {
    processForLineId: number;
    name: string;
}

export class PIOLeadTimeViewModel {
    day: number;
    hour: number;
    minute: number;
}

export class PIODefaultLeadTimeConfigurationUpdateFormViewModel {
    routingMasterCode: string;
    locationCode: string;
    processForLineId: number;
    totalLeadTimeMinutes: number;
}

export class PIOCreateDefaultLeadTimeConfigurationViewModel {
    locationCode: string;
    processForLineId: number;
    totalLeadTimeMinutes: number;
}

export class PIOUpdateDefaultLeadTimeConfigurationViewModel {
    routingMasterCode: string;
    oldLocationCode: string;
    newLocationCode: string;
    oldProcessForLineId: number;
    newProcessForLineId: number;
    totalLeadTimeMinutes: number;
}

export class PIODeleteDefaultLeadTimeConfigurationViewModel {
    routingMasterCode: string;
    locationCode: string;
    processForLineId: number;
}

export class PIODefaultLeadTimeConfigurationService {
    static $inject = ["$http"];
    
    constructor(httpClient: angular.IHttpService) {
        this.httpClient = httpClient;
    }

    httpClient: angular.IHttpService;

    // Retrieve the PIODefaultLeadTimeConfigurationPageViewModel from the server
    getData() {
        return this.httpClient.get<PIODefaultLeadTimeConfigurationPageViewModel>("/api/v1/PIODefaultLeadTimeConfigurationApi");
    }

    // Call the create a new PIO Default Lead Time method in the PIODefaultLeadTimeConfiguration controller
    createPIODefaultLeadTimeConfiguration(submitPIODefaultLeadTimeConfiguration: PIOCreateDefaultLeadTimeConfigurationViewModel) {
        return this.httpClient.post("/api/v1/PIODefaultLeadTimeConfigurationApi/Create", submitPIODefaultLeadTimeConfiguration);
    }

    // Call the update PIO Default Lead Time method in the PIODefaultLeadTimeConfiguration controller
    updatePIODefaultLeadTimeConfiguration(submitPIODefaultLeadTimeConfiguration: PIOUpdateDefaultLeadTimeConfigurationViewModel) {
        return this.httpClient.post("/api/v1/PIODefaultLeadTimeConfigurationApi/Update/", submitPIODefaultLeadTimeConfiguration);
    }

    // Call the delete PIO Default Lead Time method in the PIODefaultLeadTimeConfiguration controller
    deletePIODefaultLeadTimeConfiguration(pioDeleteDefaultLeadTimeConfiguration: PIODeleteDefaultLeadTimeConfigurationViewModel) {
        return this.httpClient.delete("/api/v1/PIODefaultLeadTimeConfigurationApi", { params: pioDeleteDefaultLeadTimeConfiguration });
    }
}