import * as angular from "angular";

export class PDILeadTimeConfigurationViewModel {
    pdiLeadTimeId: number;
    locationCode: string;
    locationName: string;
    locationString: string;
    katashiki: string;
    suffix: string;
    taktSeconds: number;
    post: number;
    taktTimeString: string;
}

export class PDILeadTimeConfigurationKatashikiSuffixPairViewModel {
    katashiki: string;
    suffix: string;
}

export class PDILeadTimeConfigurationLocationViewModel {
    locationCode: string;
    locationName: string;
}

export class SubmitPDILeadTimeConfigurationFormViewModel {
    pdiLeadTimeId: number;
    locationCode: string;
    totalTaktTimes: number;
    katashiki: string;
    suffix: string;
    post: number;
}

export class UpdatePDILeadTimeConfigurationFormViewModel {
    pdiLeadTimeId: number;
    locationCode: string;
    totalTaktTimes: number;
    post: number;
}

export class PDILeadTimeConfigurationTaktTimesViewModel {
    hour: number;
    minute: number;
    second: number;
}

export class PDILeadTimeConfigurationCarModelViewModel {
    carModelCode: string;
    carModelName: string;
    carSeries: PDILeadTimeConfigurationCarSeriesViewModel[];
    isChecked: boolean;
    isExpand: boolean;
}

export class PDILeadTimeConfigurationCarSeriesViewModel {
    carSeriesCode: string;
    carModelCode: string;
    carSeriesName: string;
    carTypeKatashikis: PDILeadTimeConfigurationCarTypeKatashikiViewModel[];
    isChecked: boolean;
    isExpand: boolean;
}

export class PDILeadTimeConfigurationCarTypeViewModel {
    katashiki: string;
    suffix: string;
    carSeriesCode: string;
}

export class PDILeadTimeConfigurationCarTypeKatashikiViewModel {
    katashiki: string;
    suffixes: PDILeadTimeConfigurationCarTypeSuffixViewModel[];
    carSeriesCode: string;
    isChecked: boolean;
    isExpand: boolean;
}

export class PDILeadTimeConfigurationKatashikiModel {
    katashiki: string;
    carSeriesCode: string;
}

export class PDILeadTimeConfigurationCarTypeSuffixViewModel {
    katashiki: string;
    suffix: string;
    isChecked: boolean;
}

export class PDILeadTimeConfigurationAllGetModel {
    pdiLeadTimeConfigurationViewModels: PDILeadTimeConfigurationViewModel[];
    pdiLeadTimeConfigurationCarModels: PDILeadTimeConfigurationCarModelViewModel[];
    pdiLeadTimeConfigurationCarSeries: PDILeadTimeConfigurationCarSeriesViewModel[];
    pdiLeadTimeConfigurationCarTypes: PDILeadTimeConfigurationCarTypeViewModel[];
    pdiLeadTimeConfigurationKatashikis: PDILeadTimeConfigurationKatashikiModel[];
    pdiLeadTimeConfigurationLocations: PDILeadTimeConfigurationLocationViewModel[];
}

export class PDILeadTimeConfigurationService {
    static $inject = ["$http"];

    constructor(httpService: angular.IHttpService) {
        this.HttpService = httpService;
    }

    HttpService: angular.IHttpService;

    // Get the all data needed for the application
    getPDILeadTimeConfigurations() {
        return this.HttpService
            .get<PDILeadTimeConfigurationAllGetModel>("api/v1/PDILeadTimeConfigurationApi/PDILeadTimeConfigurations");
    }

    // Create a several new PDI Lead Time Configuration data
    createPDILeadTimeConfigurations(newPDILeadTimeConfigs: SubmitPDILeadTimeConfigurationFormViewModel[]) {
        return this.HttpService
            .post("api/v1/PDILeadTimeConfigurationApi", newPDILeadTimeConfigs);
    }

    // Update a PDI Lead Time Configuration data based on the PDIKatsuDictionaryDetailId
    updatePDILeadTimeConfiguration(updatedPDILeadTimeConfig: UpdatePDILeadTimeConfigurationFormViewModel) {
        return this.HttpService
            .put("api/v1/PDILeadTimeConfigurationApi/" + updatedPDILeadTimeConfig.pdiLeadTimeId, updatedPDILeadTimeConfig);
    }

    // Delete a PDI Lead Time Configuration data based on the PDIKatsuDictionaryDetailId
    deletePDILeadTimeConfiguration(pdiLeadTimeId: number) {
        return this.HttpService
            .delete("api/v1/PDILeadTimeConfigurationApi/" + pdiLeadTimeId);
    }
}