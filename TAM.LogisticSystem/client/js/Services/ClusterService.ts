
import * as Component from '../components'

export class ClusterService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) { 
        this.HttpClient = $http;
    }

    getDataCluster() {
        return this.HttpClient.get<string>('/Cluster/GetDataCluster');
    }

    postData(clusterModel: Component.ClusterModel) {
        return this.HttpClient.post('/Cluster/Create', clusterModel);
    }

    updateData(clusterModel: Component.ClusterModel) {
        return this.HttpClient.post('/Cluster/Edit/' + clusterModel.aS400ClusterCode, clusterModel);
    }

    deleteData(id: string) {
        return this.HttpClient.delete('/Cluster/Delete/' + id);
    }
}