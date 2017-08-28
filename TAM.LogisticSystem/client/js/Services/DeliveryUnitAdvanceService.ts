

export class DeliveryUnitAdvanceService {
    static $inject = ["$http"];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    //private readonly
    httpService: angular.IHttpService;

    //mendapatkan semua data untuk form
    getUnitData() {
        return this.httpService.get<DeliveryUnitAdvanceModel[]>("api/v1/DeliveryUnitAdvanceApi/");
    }

    //mengirim form data ke controller
    submitData(deliveryUnitAdvanceData: DeliveryUnitAdvanceModel) {
        return this.httpService.post<DeliveryUnitAdvanceModel>("api/v1/DeliveryUnitAdvanceApi", deliveryUnitAdvanceData);
    }
}

export class DeliveryUnitAdvanceModel {
    frameNumber: string;
    katashiki: string;
    suffix: string;
    tipe: string;
    branch: string;
    requestedPDD: Date;
    model: string;
    warna: string;
    customerAssign: boolean;
}