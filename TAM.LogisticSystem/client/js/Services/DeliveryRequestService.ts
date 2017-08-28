export class DeliveryRequestService {
    static $inject = ["$http"];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    } 

    httpService: angular.IHttpService;

    //Get semua data
    getAllDeliveryRequestPageView() {
        return this.httpService.get<DeliveryRequestPageViewModel>("api/v1/DeliveryRequestApi/GetAllDeliveryRequestPageView");
    }

    //Get Sequential Number
    getSequentialNumber(branchCode: string, tipeDR: string) {
        return this.httpService.get<number>("api/v1/DeliveryRequestApi/GetSequentialNumber/" + branchCode + "/" + tipeDR);
    }

    //Generate Confirmation Code
    getConfirmationCode() {
        return this.httpService.get<string>("api/v1/DeliveryRequestApi/GetConfirmationCode");
    }

    //Generate Return Pdc Date
    getReturnPdcDate(returnPdcDateModel: ReturnPdcDateModel) {
        return this.httpService.get<Date>("api/v1/DeliveryRequestApi/GetReturnPdcDate", {
            params: returnPdcDateModel
        });
    }

    //Create Delivery Request - Normal
    createDeliveryNormalRequest(deliveryRequestNormalCreateModel: DeliveryRequestNormalCreateModel) {
        return this.httpService.post<number>("api/v1/DeliveryRequestApi/Normal", deliveryRequestNormalCreateModel);
    }

    //Create Delivery Request - Self Pick
    createDeliverySelfPickRequest(deliveryRequestSelfPickCreateModel: DeliveryRequestSelfPickCreateModel) {
        return this.httpService.post<number>("api/v1/DeliveryRequestApi/SelfPick", deliveryRequestSelfPickCreateModel);
    }

    //Create Delivery Request - Direct Delivery
    createDeliveryDirectDeliveryRequest(deliveryRequestDirectDeliveryCreateModel: DeliveryRequestDirectDeliveryCreateModel) {
        return this.httpService.post<number>("api/v1/DeliveryRequestApi/DirectDelivery", deliveryRequestDirectDeliveryCreateModel);
    }

    //Create Delivery Request - Transit To Others - Self Pick To Others
    createDeliveryTransitToOthersSelfPickToOthersRequest(deliveryRequestTransitToOthersSelfPickToOthersCreateModel: DeliveryRequestTransitToOthersSelfPickToOthersCreateModel) {
        return this.httpService.post<number>("api/v1/DeliveryRequestApi/TransitToOthers/SelfPickToOthers", deliveryRequestTransitToOthersSelfPickToOthersCreateModel);
    }

    //Create Delivery Request - Transit To Others - Normal - Return To PDC // Return To Others PDC
    createDeliveryTransitToOthersNormalReturnToPdcRequest(deliveryRequestTransitToOthersNormalReturnToPdcCreateModel: DeliveryRequestTransitToOthersNormalReturnToPdcCreateModel) {
        return this.httpService.post<number>("api/v1/DeliveryRequestApi/TransitToOthers/Normal/ReturnToPDC", deliveryRequestTransitToOthersNormalReturnToPdcCreateModel);
    }

    //Create Delivery Request - Transit To Others - Normal - Self Pick From Others
    createDeliveryTransitToOthersNormalSelfPickFromOthersRequest(deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel: DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel) {
        return this.httpService.post<number>("api/v1/DeliveryRequestApi/TransitToOthers/Normal/SelfPickFromOthers", deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel);
    }
}

//Class untuk Date Picker Popup
export class PopUp {
    opened: boolean;
}

//Class untuk generate Transit Return Date
export class ReturnPdcDateModel {
    date: Date;
    leadTimeDay: number;
    leadTimeHour: number;
    leadTimeMinute: number;
}

//Class Location (Combo Box)
export class DeliveryRequestLocationModel {
    locationCode: string;
    locationTypeCode: string;
    locationType: string;
}

//Class Location Name (Combo Box)
export class DeliveryRequestLocationNameModel {
    locationCode: string;
    locationTypeCode: string;
    locationName: string;
}

//Class Other PDC Location (Combo Box)
export class DeliveryRequestOtherPdcLocationModel {
    otherPdcLocation: string;
    otherPdcLocationCode: string;
}

//Class Delivery Request
export class DeliveryRequestModel {
    deliveryRequestNumber: string;
    createdAt: Date;
    vehicleId: number;
    frameNumber: string;
    deliveryRequestType: string;
    cancelledAt: string;
    branchCode: string;
    deliveryRequestTypeEnumId: number;
    sequentialNumber: number;
}

//Class Car
export class DeliveryRequestCarModel {
    frameNumber: string;
    vehicleId: number;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    customerAssign: boolean;
    requestedPdd: Date;
    estimatedPdcIn: Date;
    warna: string;
    branch: string;
    branchCode: string;
    posisiTerakhir: string;
    lokasiTerakhir: string;
    scanTime: string;
}

//Class untuk semua data yang perlu di GET
export class DeliveryRequestPageViewModel {
    deliveryRequest: DeliveryRequestModel[];
    deliveryRequestCar: DeliveryRequestCarModel[];
    deliveryRequestLocationType: DeliveryRequestLocationModel[];
    deliveryRequestLocationName: DeliveryRequestLocationNameModel[];
    deliveryRequestLocationAddress: DeliveryRequestTransitToOthersModel[];
    deliveryRequestOtherPdcLocation: DeliveryRequestOtherPdcLocationModel[];
}

//Class View Model
export class DeliveryRequestViewModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestCar: DeliveryRequestCarModel;
}

//Class untuk tiap delivery type
export class DeliveryRequestNormalModel {
    requestedDeliveryTimeToBranch: Date;
    requestedDeliveryTimeToBranchView: string;
    validateDetail: boolean;
}

export class DeliveryRequestSelfPickModel {
    pickUpDate: Date;
    pickUpDateView: string;
    driverType: string;
    driverId: string;
    driverName: string;
    confirmationCode: string;
    validateDetail: boolean;
}

export class DeliveryRequestDirectDeliveryModel {
    estimasiPdcOut: Date;
    estimasiPdcOutView: string;
    customerName: string;
    customerAddress: string;
    customerCity: string;
    salesmanName: string;
    salesmanContactNo: string;
    validateDetail: boolean;
}

export class DeliveryRequestTransitToOthersModel {
    deliveryLocation: DeliveryRequestLocationModel;
    deliveryLocationName: DeliveryRequestLocationNameModel;
    locationAddress: string;
    locationCode: string;
    leadTimeDay: number;
    leadTimeHour: number;
    leadTimeMinute: number;
    pickUpDate: Date;
    pickUpDateView: string;
    deliveryRequestTransitType: string;
    validateDetail: boolean;
    validateDetailSelfPickToOthers: boolean;
    validateDetailTransitNormal: boolean;
    validateDetailSelfPickFromOthers: boolean;
    validateTransitDetail: boolean;
}

//Class untuk tiap delivery transit type
export class DeliveryRequestSelfPickToOthersModel {
    driverType: string;
    driverId: string;
    driverName: string;
    confirmationCode: string;
    returnPdcDate: Date;
    returnPdcDateView: string;
    validateTransitDetail: boolean;
}

export class DeliveryRequestSelfPickFromOtherModel {
    driverType: string;
    driverId: string;
    driverName: string;
    validateTransitDetail: boolean;
}

export class DeliveryRequestTransitToOthersNormalModel {
    deliveryTransitType: string;
    deliveryOtherPdcLocation: DeliveryRequestOtherPdcLocationModel;
    validateTransitDetail: boolean;
    validateDetailSelfPickFromOthers: boolean;
}

export class DeliveryRequestTransitToOthersNormalSelfPickModel {
    deliveryTransitType: string;
    deliverySelfPickFromOthers: DeliveryRequestSelfPickFromOtherModel;
}

export class DeliveryRequestTransitToOthersSelfPickToOthersModel {
    deliveryTransitToOthers: DeliveryRequestTransitToOthersModel;
    deliverySelfPickToOthers: DeliveryRequestSelfPickToOthersModel;
}

export class DeliveryRequestTransitToOthersNormalReturnToPdcModel {
    deliveryTransitToOthers: DeliveryRequestTransitToOthersModel;
    deliveryTransitToOthersNormalModel: DeliveryRequestTransitToOthersNormalModel;
}

export class DeliveryRequestTransitToOthersNormalSelfPickFromOthersModel {
    deliveryTransitToOthers: DeliveryRequestTransitToOthersModel;
    deliveryRequestTransitToOthersNormalModel: DeliveryRequestTransitToOthersNormalSelfPickModel;
}

//Class untuk create
export class DeliveryRequestNormalCreateModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestNormal: DeliveryRequestNormalModel;
}

export class DeliveryRequestSelfPickCreateModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestSelfPick: DeliveryRequestSelfPickModel;
}

export class DeliveryRequestDirectDeliveryCreateModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestDirectDelivery: DeliveryRequestDirectDeliveryModel; 
}

export class DeliveryRequestTransitToOthersSelfPickToOthersCreateModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestTransitToOthersSelfPickToOthers: DeliveryRequestTransitToOthersSelfPickToOthersModel;
}

export class DeliveryRequestTransitToOthersNormalReturnToPdcCreateModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestTransitToOthersNormalReturnToPdc: DeliveryRequestTransitToOthersNormalReturnToPdcModel;
}

export class DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel {
    deliveryRequest: DeliveryRequestModel;
    deliveryRequestTransitToOthersNormalSelfPickFromOthers: DeliveryRequestTransitToOthersNormalSelfPickFromOthersModel;
}