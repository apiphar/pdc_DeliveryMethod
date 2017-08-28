
import * as Component from '../components';
export class PIOLineMasterService {
    static $inject = ['$http'];
    HttpClient: angular.IHttpService;
    
    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getData() {
        return this.HttpClient.get('/api/v1/PIOLineMasterApi/PIOLineMaster');
    }
    
    postData(Region:Component.PIOLineMasterInsert) {
        return this.HttpClient.post('/api/v1/PIOLineMasterApi/PostData', Region );
    } 

    updatePIOLineMaster(pioLineMaster: Component.PIOLineMasterInsert) {
        return this.HttpClient.post('/api/v1/PIOLineMasterApi/UpdatePIOLineMaster/' + pioLineMaster.pioLineDictionaryId, pioLineMaster);
    } 

    deletePIOLineMaster(id: string) {
        return this.HttpClient.delete('/api/v1/PIOLineMasterApi/DeletePIOLineMaster/' + id);
    }
}