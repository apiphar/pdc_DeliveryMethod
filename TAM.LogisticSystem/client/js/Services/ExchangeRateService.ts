

export class ExchangeRateService {

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getExchangeRates() {
        return this.HttpClient.get<string>('/ExchangeRateApi/GetExchangeRates');
    }

    getCurrencys() {
        return this.HttpClient.get<string>('/ExchangeRateApi/GetCurrencys');
    }

    postData(CurrencySymbol: String, ValidFrom: Date, ValidUntil: Date, ToRupiah: number, CreatedAt: Date, UpdatedAt: Date) {
        return this.HttpClient.post('/ExchangeRateApi/Create', { CurrencySymbol, ValidFrom, ValidUntil, ToRupiah, CreatedAt, UpdatedAt });
    }

    updateData(ExchangeRateId: number, CurrencySymbol: String, ValidFrom: Date, ValidUntil: Date, ToRupiah: number, CreatedAt: Date, UpdatedAt: Date) {
        return this.HttpClient.post('/ExchangeRateApi/edit/' + ExchangeRateId, { CurrencySymbol, ValidFrom, ValidUntil, ToRupiah, CreatedAt, UpdatedAt });
    } 

    deleteData(CurencyRateId: number) {
        return this.HttpClient.delete('/ExchangeRateApi/delete/' + CurencyRateId);
    }
}