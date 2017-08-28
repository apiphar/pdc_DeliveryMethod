export class DeliveryLegLeadTimeService {
    static $inject = ["$http"];

    httpService: angular.IHttpService;
    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }
    //to get all delivery lead time data
    getAllDeliveryLeadData(deliveryLegCode: string) {
        return this.httpService.get<DeliveryLegLeadTimeViewModel[]>("/api/v1/DeliveryLegLeadTimeAPI/" + deliveryLegCode);
    }
    //to get delivery all delivery method code
    getDeliveryMethodCode() {
        return this.httpService.get<GetDeliveryMethodViewModel[]>("../../api/v1/DeliveryLegLeadTimeAPI/GetDeliveryMethodCode");
    }
    //to get LocationFrom & LocationTo
    getLocation(deliveryLegCode: string) {
        return this.httpService.get<GetLocationLeadTimeViewModel>("/api/v1/DeliveryLegLeadTimeAPI/GetLocation/" + deliveryLegCode);
    }
    //to insert one delivery lead data
    postDeliveryLeadData(deliveryLegCode: string, deliveryMethodCode: string, leadMinutes: number) {
        return this.httpService.post("../../api/v1/DeliveryLegLeadTimeAPI/", { deliveryLegCode, deliveryMethodCode, leadMinutes });
    }
    //to delete one delivery lead data
    deleteDeliveryLeadData(deliveryLeadTimeId: number) {
        return this.httpService.delete("/api/v1/DeliveryLegLeadTimeAPI/" + deliveryLeadTimeId);
    }
    //to update one delivery lead data
    updatePlafondData(deliveryLeadTimeId: number, locationFrom: string, locationTo: string, deliveryMethodCode: string, leadMinutes: number) {
        return this.httpService.post("../../api/v1/DeliveryLegLeadTimeAPI/" + deliveryLeadTimeId, { deliveryMethodCode, locationFrom, locationTo, leadMinutes });
    }
}

export class DeliveryLegLeadTimeViewModel {
    deliveryLeadTimeId: number;
    deliveryMethodCode: string;
    leadMinutes: number;
    deliveryLegCode: string;
    parentDeliveryMethodCode: string;
    day: number;
    hour: number;
    minute: number;
    waktu: string;
}

export class GetLocationLeadTimeViewModel {
    locationCode: string;
    locationFrom: string;
    locationTo: string;
    nameLocationFrom: string;
    nameLocationTo: string;
    deliveryLegCode: string;
    deliveryLegName: string;
}
export class GetDeliveryMethodViewModel {
    deliveryMethodCode: string;
    deliveryMethodName: string;
    parentDeliveryMethodCode: string;
}

export class LeadTimeViewModel {
    day: number;
    hour: number;
    minute: number;
}