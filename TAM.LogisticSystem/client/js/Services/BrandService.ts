import * as Component from '../components'

export class BrandService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getDataBrand() {
        return this.HttpClient.get<string>('/Brand/GetDataBrand');
    }

    postData(brandModel: Component.BrandModel) {
        return this.HttpClient.post('/Brand/Create', brandModel);
    }

    updateData(brandModel: Component.BrandModel) {
        return this.HttpClient.post('/Brand/Edit/' + brandModel.brandCode, brandModel);
    }

    deleteData(brandCode: String) {
        return this.HttpClient.delete('/Brand/Delete/' + brandCode);
    }
}