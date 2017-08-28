
import * as Component from '../components'

export class CarTypeService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAllCarType() {
        return this.HttpClient.get<string>('api/v1/cartype/GetAllCarType');
    } 

    getAllCarSeries() {
        return this.HttpClient.get<string>('api/v1/cartype/GetAllCarSeries');
    }

    getAllAfiCarType() {
        return this.HttpClient.get<string>('api/v1/cartype/getAllAfiCarType');
    }

    get() {
        return this.HttpClient.get<string>('api/v1/cartype');
    }

    postData(cartypeModel: Component.CartypeModel ) {
        return this.HttpClient.post('api/v1/cartype', cartypeModel);
    }

    updateData(cartypeModel: Component.CartypeModel) {
        return this.HttpClient.post('api/v1/cartype/update', cartypeModel);
    }

    deleteData(cartypeModel: Component.CartypeModel) {
        return this.HttpClient.delete('api/v1/cartype', cartypeModel );
    }
}