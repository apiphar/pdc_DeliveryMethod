

export class LocationTypeService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }
    //get all data location type
    getAllLocationType() {
        return this.httpClient.get<LocationTypeModel[]>('/api/v1/LocationTypeApi');
    }
    //add new location type
    postLocationType(locationTypeModel: LocationTypeModel) {
        return this.httpClient.post('/api/v1/LocationTypeApi', locationTypeModel);
    }
    //udpate location type
    updateLocationType(locationTypeModel: LocationTypeModel) {
        return this.httpClient.post('/api/v1/LocationTypeApi/edit/', locationTypeModel);
    }
    //delete location type
    deleteLocation(locationTypeCode: string) {
        
        return this.httpClient.delete('/api/v1/LocationTypeApi/delete/' + locationTypeCode);
    }
}
export class LocationTypeModel {
    locationTypeCode: string;
    name: string;
    hasResponsibility: boolean;
    needSjkbTarikan: boolean;
    tanggungJawab: string;
    sjkb: string;
}
export class LocationTypeModelConfirmation {
    kodeLokasi: string;
    tipeLokasi: string;
    tanggungJawab: string;
    sjkb: string;
}