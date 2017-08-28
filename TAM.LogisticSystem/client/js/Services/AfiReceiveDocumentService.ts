import * as Angular from 'angular';
import * as Component from '../components';
export class AfiReceiveDocumentService {
    $inject = ['$http'];

    HttpClient: Angular.IHttpService;

    constructor($http: Angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAllDocument(frameNo: string) {
        return this.HttpClient.get('/api/v1/AfiReceiveDocumentApi/'+frameNo);
    }

    receiveDocument(RCU: Component.ReceiveDocumentUpdate) {
        return this.HttpClient.post('/api/v1/AfiReceiveDocumentApi/ReceiveDocument', RCU);
    }
}