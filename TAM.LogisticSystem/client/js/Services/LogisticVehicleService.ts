
import * as Component from '../components';

export class LogisticVehicleService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getDataDeliveryMethod() {
        return this.HttpClient.get<string>('/LogisticVehicle/GetDataDeliveryMethod');
    } 

    getDataDeliveryMethodType() {
        return this.HttpClient.get<any>('/LogisticVehicle/GetDataDeliveryMethodType');
    }

    postData(logisticVehicleModel: Component.LogisticVehicleModel) {
        return this.HttpClient.post('/LogisticVehicle/create', logisticVehicleModel);
    }

    updateData(logisticVehicleModel: Component.LogisticVehicleModel) {
        return this.HttpClient.post('/LogisticVehicle/edit/' + logisticVehicleModel.deliveryMethodCode, logisticVehicleModel);
    }

    deleteData(id: string) {
        return this.HttpClient.delete('/LogisticVehicle/delete/' + id);
    }
}