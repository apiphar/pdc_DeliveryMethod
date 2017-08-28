


export class MasterRitasePriceService {
    static $inject = ['$http'];
    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }
    

    //get All Ritase Data
    getAllRitaseData() {
        return this.httpService.get<MasterRitasePriceViewModel[]>('/api/v1/MasterRitasePriceApi/');
    }

    //delete Ritase Data by id
    deleteData(id: number) {
        return this.httpService.delete('/api/v1/MasterRitasePriceApi/DeleteData/'+id);
    }

    //get all delivery vendor code
    getDeliveryVendor() {
        return this.httpService.get<DeliveryVendor[]>('/api/v1/MasterRitasePriceApi/GetDeliveryVendor');
    }


    //get all delivery method code
    getDeliveryMethod() {
        return this.httpService.get<DeliveryMethod[]>('/api/v1/MasterRitasePriceApi/GetDeliveryMethod');
    }

    //get all city leg code
    getCityLeg() {
        return this.httpService.get<CityLeg[]>('/api/v1/MasterRitasePriceApi/GetCityLegCode');
    }

    getCurrencySymbol() {
        return this.httpService.get<CurrencySymbol[]>('/api/v1/MasterRitasePriceApi/GetCurrencySymbol');
    }


    //add ritase data
    addData(data: MasterRitasePriceInput) {
        return this.httpService.post('/api/v1/MasterRitasePriceApi/AddData', data);
    }

    //update data
    updateData(data: MasterRitasePriceViewModel) {
        return this.httpService.put('/api/v1/MasterRitasePriceApi/UpdateData', data);
    }

    //property
    httpService: angular.IHttpService;
}

export class TryModell {
    Check: string;
    Numb: number;
}

export class MasterRitasePriceInput {
    cityLegCode: string;
    currencySymbol: string;
    deliveryMethodCode: string;
    deliveryVendorCode: string;
    isSingleTrip: boolean;
    nominal: number;
    validDate: Date;
}

export class DeliveryVendor {
    deliveryVendorCode: string;
}

export class CityLeg {
    cityLegCode: string;
}

export class DeliveryMethod {
    deliveryMethodCode: string;
}

export class CurrencySymbol {
    currencySymbol: string;
}


export class MasterRitasePriceViewModel {
    cityLegCode: string;
    cityLegRitaseCostId: number;
    currencySymbol: string;
    deliveryMethodCode: string;
    deliveryVendorCode: string;
    isSingleTrip: string;
    nominal: number;
    validDate: Date;
}

export class Search {
    cityLegCode: string;
    cityLegRitaseCostId: number;
    currencySymbol: string;
    deliveryMethodCode: string;
    deliveryVendorCode: string;
    isSingleTrip: string;
    nominal: number;
    validDate: Date;
}

export class TripList {
    code: number;
    name: string;

}