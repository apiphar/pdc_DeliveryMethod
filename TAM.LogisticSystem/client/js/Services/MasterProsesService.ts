
import { MasterProsesData } from '../components/MasterProses';

export class MasterProsesService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }

    // fetching Master Proses data
    getMasterProsesList() {
        return this.httpClient.get<MasterProsesData>('/api/v1/masterproses');
    }

    // create new Master Proses data
    postMasterProses(model: MasterProsesData) {

        model.name = model.name.toUpperCase();
        model.processMasterCode = model.processMasterCode.toUpperCase();

        return this.httpClient.post('/api/v1/masterproses', model);
    }

    // updating Master Proses data
    putMasterProses(model: MasterProsesData) {

        model.name = model.name.toUpperCase();
        model.processMasterCode = model.processMasterCode.toUpperCase();

        return this.httpClient.put('/api/v1/masterproses/' + model.processMasterCode, model);
    }

    // deleting Master Proses data
    deleteMasterProses(model: MasterProsesData) {

        model.processMasterCode = model.processMasterCode.toUpperCase();

        return this.httpClient.delete('/api/v1/masterproses/' + model.processMasterCode);
    }
}