import * as compo from '../Components'
export class SchemeTable {
    scheme: number;
    bm: number;
    constructor(_scheme: number, _bm: number) {
        this.scheme = _scheme;
        this.bm = _bm;
    }
}
export class TariffService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    Add(HSCode: string, PPH: number, PPn: number, PPnBM: number, EffectiveDateFrom: Date, schemeTable: Array<SchemeTable>) {
        return this.HttpClient
            .post('/api/v1/tariff/', { HSCode, PPH, PPn, PPnBM, EffectiveDateFrom,schemeTable});
    }

    Update(tariffId,HSCode: string, PPH: number, PPn: number, PPnBM: number, EffectiveDateFrom: Date, Scheme,BM) {
        return this.HttpClient
            .post('/api/v1/tariff/' + tariffId, { HSCode, PPH, PPn, PPnBM, EffectiveDateFrom, Scheme, BM });
    }

    GetData() {
        return this.HttpClient.get<string>('/api/v1/tariff/');
    }

    DeleteData(TariffId: number) {
        return this.HttpClient.delete('/api/v1/tariff/' + TariffId);
    }
}