
import * as Component from '../components';
export class SPULineMasterService {
    static $inject = ['$http'];
    HttpClient: angular.IHttpService;
    
    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
        let a = new Date();
    }

    getData() {
        return this.HttpClient.get('/api/v1/SPULineMasterApi/SPULineMaster');
    }
    
    postData(SPU: Component.SPULineMasterInsert) {
        return this.HttpClient.post('/api/v1/SPULineMasterApi/PostData', SPU );
    } 

    updateSPULineMaster(SPULineMaster: Component.SPULineMasterInsert) {
        return this.HttpClient.post('/api/v1/SPULineMasterApi/UpdateSPULineMaster/' + SPULineMaster.spuLineId, SPULineMaster);
    } 

    deleteSPULineMaster(id: string) {
        return this.HttpClient.delete('/api/v1/SPULineMasterApi/DeleteSPULineMaster/' + id);
    }
}