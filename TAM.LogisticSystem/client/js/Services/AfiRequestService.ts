import * as Angular from 'angular';
import * as Component from '../components';
export class AfiRequestService {
    $inject = ['$http'];

    HttpClient: Angular.IHttpService;

    constructor($http:Angular.IHttpService) {
        this.HttpClient = $http;
    }

    checkDataByFrame(FrameNumber:string) {
        return this.HttpClient.get('/api/v1/AfiRequestApi/CheckDataByFrame/'+FrameNumber);
    }

    getRegionAndRegionAFI() {
        return this.HttpClient.get('/api/v1/AfiRequestApi/GetRegionAndRegionAFI');
    }

    insertAfi(dataInsert: Component.DataInsertModel) {
        return this.HttpClient.post('/api/v1/AfiRequestApi/InsertAfi', dataInsert);
    }
}