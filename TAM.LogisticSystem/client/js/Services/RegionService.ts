
import * as Component from '../components'
export class RegionService {
    static $inject = ['$http'];
    HttpClient: angular.IHttpService;
    
    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getData() {
        return this.HttpClient.get('/api/v1/regionApi/GetData');
    }

    updateRegion(Region: Component.RegionInsert) {
        return this.HttpClient.post('/api/v1/regionApi/UpdateRegion/' + Region.regionCode, Region);
    } 

    deleteRegion(id: string) {
        return this.HttpClient.delete('/api/v1/regionApi/DeleteRegion/' + id);
    }
}