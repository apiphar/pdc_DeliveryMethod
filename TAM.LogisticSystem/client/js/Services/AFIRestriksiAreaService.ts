export class AFIRestriksiAreaService {
    static $inject = ['$http'];

    $http: angular.IHttpService;

    constructor(http: angular.IHttpService) {
        this.$http = http;
    }

    //get all the require data for the view
    getAllData() {
        return this.$http.get<AFIRestriksiAreaGetAllModel>('/api/v1/AFIRestriksiAreaAPI');
    }

    //get all the require data for the detail view
    getAllDetailData(regionCode: string, validFrom: Date, validTo: Date) {
        return this.$http.get<AFIRestriksiAreaGetAllModel>('/api/v1/AFIRestriksiAreaAPI/Detail', { params: { regionCode: regionCode, validFrom: validFrom, validTo: validTo } });
    }

    //insert the data into database
    create(afiRestriksiAreaInsertModel: AFIRestriksiAreaInsertModel) {
        return this.$http.post<AFIRestriksiAreaInsertModel>('/api/v1/AFIRestriksiAreaAPI', afiRestriksiAreaInsertModel);
    }

    //update the data in the database
    update(afiRestriksiAreaUpdateModel: AFIRestriksiAreaUpdateModel) {
        return this.$http.put<AFIRestriksiAreaUpdateModel>('/api/v1/AFIRestriksiAreaAPI', afiRestriksiAreaUpdateModel);
    }

    //delete the data in the database
    delete(afiRegionRestrictionId: number) {
        return this.$http.delete('/api/v1/AFIRestriksiAreaAPI/' + afiRegionRestrictionId);
    }
}

export class AFIRestriksiAreaViewModel {
    afiRegionRestrictionId: number;
    regionCode: string;
    name: string;
    regionString: string;
    isLocked: boolean;
    validFrom: Date;
    validFromString: string;
    validTo: Date;
    validToString: string;
}

export class AFIRestriksiAreaRegionModel {
    regionCode: string;
    name: string;
}

export class AFIRestriksiAreaGetAllModel {
    afiRegionsRestriction: AFIRestriksiAreaViewModel[];
    regions: AFIRestriksiAreaRegionModel[];
}

export class AFIRestriksiAreaInsertModel {
    regionCode: string;
    isLocked: boolean;
    validFrom: Date;
    validTo: Date;
}

export class AFIRestriksiAreaUpdateModel {
    afiRegionRestrictionId: number;
    regionCode: string;
    isLocked: boolean;
    validFrom: Date;
    validTo: Date;
}

export class AFIRestriksiAreaParentDataModel {
    regionCode: string;
    validFrom: Date;
    validTo: Date
}