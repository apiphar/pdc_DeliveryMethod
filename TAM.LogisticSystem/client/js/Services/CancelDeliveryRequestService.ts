export class CancelDeliveryRequestService {
    static $inject = ["$http"];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    } 

    httpService: angular.IHttpService;

    getAllLocation() {
        return this.httpService.get<CancelDeliveryRequestLocationModel[]>("api/v1/CancelDeliveryRequestAPI/GetAllCancelDeliveryRequestLocation");
    }

    getAllData() {
        return this.httpService.get<CancelDeliveryRequestPageViewModel>("api/v1/CancelDeliveryRequestAPI/");
    }


    cancelDeliveryRequest(model: CancelDeliveryRequestViewModel) {
        return this.httpService.post<number>("api/v1/CancelDeliveryRequestAPI/CancelDeliveryRequest", model);
    }
}

//Class untuk semua data yang perlu di GET
export class CancelDeliveryRequestPageViewModel {
    cancelDeliveryRequest: CancelDeliveryRequestViewModel[];
    cancelDeliveryRequestLocation: CancelDeliveryRequestLocationModel[];
}

//Class untuk mendapatkan Location Name, Address, & Type
export class CancelDeliveryRequestLocationModel {
    locationCode: string;
    locationName: string;
    locationTypeName: string;
    locationAddress: string;
    locationTypeCode: string;
}

//Class View Model
export class CancelDeliveryRequestViewModel {
    deliveryRequestNumber: string;
    cancelledAt: string;
    createdAt: Date;
    vehicleId: number;
    deliveryRequestTypeId: number;
    deliveryRequestTransitTypeId: number;
    frameNumber: string;
    katashiki: string;
    suffix: string;
    warna: string;
    branch: string;
    requestedPdd: Date;
    estimatedPdcIn: Date;
    customerAssign: boolean;
    posisiTerakhir: string;
    lokasiTerakhir: string;
    requestedDeliveryTimeToBranch: Date;
    pickUpDate: Date;
    pickUpIdentityIsKtp: boolean;
    driverType: string;
    driverId: string;
    driverName: string;
    confirmationCode: string;
    estimasiPDCOut: Date;
    customerName: string;
    customerAddress: string;
    customerCity: string;
    salesmanName: string;
    salesmanContactNo: string;
    locationType: string;
    locationTypeCode: string;
    locationName: string;
    locationAddress: string;
    locationCode: string;
    leadTimeDay: number;
    leadTimeHour: number;
    leadTimeMinute: number;
    transitReturnDate: Date;
    otherPdcLocation: string;
    otherPdcLocationCode: string;
}