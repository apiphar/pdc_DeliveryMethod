

export class LegPriceMasterService {
    static $inject = ['$http'];

    httpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.httpClient = $http;
    }

    getData() {
        return this.httpClient.get<string>('/legpricemaster/GetData');
    }

    getAllCityLegCost() {
        return this.httpClient.get<LegPriceMasterViewModel[]>('/api/v1/LegPriceMasterApi/GetAllCityLegCost');
    }

    getAllKodeVendor() {
        return this.httpClient.get<LegPriceMasterDeliveryVendorModel[]>('/api/v1/LegPriceMasterApi/GetAllDeliveryVendor');
    }

    getAllCityLeg() {
        return this.httpClient.get<LegPriceMasterCityLegModel[]>('/api/v1/LegPriceMasterApi/GetAllCityLeg');
    }

    getDeliveryMethod() {
        return this.httpClient.get<LegPriceMasterDeliveryMethodModel[]>('api/v1/LegPriceMasterApi/GetDeliveryMethod');
    }

    getAllCarSeries() {
        return this.httpClient.get<LegPriceMasterCarSeriesModel[]>('api/v1/LegPriceMasterApi/GetAllCarSeries');
    }

    getAllCurrency() {
        return this.httpClient.get<LegPriceMasterCurrencyModel[]>('api/v1/LegPriceMasterApi/GetAllCurrency');
    }

    getAll() {
        return this.httpClient.get<string>('/legpricemaster/GetAll');
    }
    postData(legPriceMasterCreateModel: LegPriceMasterCreateModel) {
        return this.httpClient.post<number>('/api/v1/LegPriceMasterApi/Create', legPriceMasterCreateModel);
    }
    updateData(legPriceMasterCreateModel: LegPriceMasterCreateModel) {
        return this.httpClient.post<number>('/api/v1/LegPriceMasterApi/Edit/' + legPriceMasterCreateModel.cityLegCostCode, legPriceMasterCreateModel);
    }
    deleteData(cityLegCostCode: string) {
        return this.httpClient.delete('/api/v1/LegPriceMasterApi/Delete/' + cityLegCostCode);
    }

}

export class LegPriceMasterViewModel {
    carSeriesCode: string;
    carSeriesName: string;
    carSeriesNameView: string;
    cityLegCode: string;
    cityLegName: string;
    cityLegNameView: string;

    cityLegCostCode: string; 

    createdAt: Date; 

    createdBy: string; 

    currencySymbol: string; 
    currencyName: string;
    currencyNameView: string;

    deliveryMethodCode: string;
    deliveryMethodName: string;
    deliveryMethodNameView: string;

    deliveryVendorCode: string;
    deliveryVendorName: string;
    deliveryVendorNameView: string;

    needAdditionalCityLegCostCode: string;

    nominal: number;

    updatedAt: Date;

    updatedBy: string;

    validDate: Date;
    validDateView: Date;
}

export class LegPriceMasterCreateModel {
    cityLegCostCode: string;
    deliveryVendor: LegPriceMasterDeliveryVendorModel;
    cityLeg: LegPriceMasterCityLegModel;
    deliveryMethod: LegPriceMasterDeliveryMethodModel;
    carSeries: LegPriceMasterCarSeriesModel;
    currency: LegPriceMasterCurrencyModel;
    nominal: number;
    validDate: Date;
    needAdditionalCityLegCostCode: string;
}

export class LegPriceMasterDeliveryVendorModel {
    deliveryVendorCode: string;
    deliveryVendorName: string;   
}

export class LegPriceMasterCityLegModel {
    cityLegCode: string;
    cityLegName: string;
}

export class LegPriceMasterDeliveryMethodModel {
    deliveryMethodCode: string;
    deliveryMethodName: string;
}

export class LegPriceMasterCarSeriesModel {
    carSeriesCode: string;
    carSeriesName: string;
}

export class LegPriceMasterCurrencyModel {
    currencySymbol: string;
    currencyName: string;
}
