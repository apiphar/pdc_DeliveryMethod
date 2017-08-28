

export class MasterModelSeriesService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get<string>('/mastermodelseries/GetDataMasterSeries');
    }

    GetDropdownCarModel() {
        return this.HttpClient.get<string>('/mastermodelseries/GetDropdownCarModel');
    }

   
    PostData(CarSeriesCode: String, Name: String, CarModelCode: String) {
        return this.HttpClient.post('/mastermodelseries/create', { CarSeriesCode, Name, CarModelCode });
    }

    UpdateData(CarSeriesCode: String, Name: String, CarModelCode: String) {
        return this.HttpClient.post('/mastermodelseries/edit/' + CarSeriesCode, { CarSeriesCode, Name, CarModelCode });
    }

    DeleteData(CarSeriesCode: String) {
        return this.HttpClient.delete('/mastermodelseries/delete/' + CarSeriesCode, {});
    }
    CekPola(CarSeriesCode: string) {
        return this.HttpClient.get<number>('/mastermodelseries/cekpola/' + CarSeriesCode);
    }
}