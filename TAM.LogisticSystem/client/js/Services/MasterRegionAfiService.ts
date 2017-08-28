
export class MasterRegionAfiService {
    static $inject = ['$http'];
    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {

        this.httpClient = $http;
    }

    getAllRegionAfiData() {
        return this.httpClient.get('/api/v1/masterregionafiapi/getalldataafi/');
    }

    getAllRegionData() {
        return this.httpClient.get('/api/v1/masterregionafiapi/getalldataregion/');
    }
    getPostCode() {
        return this.httpClient.get('/api/v1/masterregionafiapi/getpostcode/');
    }

    addRegionAfiData(addRegion: Region) {
        return this.httpClient.post('/api/v1/masterregionafiapi/AddData/', addRegion);
    }
    //deleteRegionAfiData(Delete: PostRegion) {
    //    return this.httpClient.post('/api/v1/masterregionafiapi/delete/', Delete);
    //}
    deleteRegionAfiData(regionCode: string) {
        return this.httpClient.delete('/api/v1/masterregionafiapi/delete/' + regionCode);
    }
    updateRegionAfiData(Update: PostRegion) {
        return this.httpClient.post('/api/v1/masterregionafiapi/UpdateRegionAfi/', Update);
    }

}

export class RegionAfi {
    afiRegionCode: string;
    name: string;
}
export class PostCode {
    postCode: string;
}

class PostRegion {
    postCode: string;
    afiRegionCode: string;
}
export class Region {
    regionCode: string;
    kota: string;
    kelurahan: string;
    postCode: string;
    afiRegionCode: string;
    afiRegionName: string;
    afiRegion: string;
}