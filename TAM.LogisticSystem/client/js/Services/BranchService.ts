import * as Component from '../components'

export class BranchService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) { 
        this.HttpClient = $http;
    }

    getDataBranch() {
        return this.HttpClient.get<string>('/Branch/GetDataBranch');
    }

    getDataSalesArea() {
        return this.HttpClient.get<string>('/Branch/GetDataSalesArea');
    }

    getDataDestination() {
        return this.HttpClient.get<string>('/Branch/GetDataDestination');
    }

    getDataRegion() {
        return this.HttpClient.get<string>('/Branch/GetDataRegion');
    }

    getDataCompany() {
        return this.HttpClient.get<string>('/Branch/GetDataCompany');
    }

    getDataLocation() {
        return this.HttpClient.get<string>('/Branch/GetDataLocation/');
    }

    //getDataLocation2(locationcode: string) {
    //    return this.HttpClient.get<string>('/Branch/GetDataLocation2/' + locationcode);
    //}

    getDataCluster() {
        return this.HttpClient.get<string>('/Branch/GetDataCluster'); 
    }

    postData(branchModel: Component.BranchModel) {
        return this.HttpClient.post('/Branch/Create', branchModel);
    }

    updateData(branchModel: Component.BranchModel ) {
        return this.HttpClient.post('/Branch/Edit/' + branchModel.branchCode, branchModel);
    }

    deleteData(branchCode: String) {
        return this.HttpClient.delete('/Branch/Delete/' + branchCode);
    }
} 