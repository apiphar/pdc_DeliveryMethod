

export class DeliveryUnitLoadingDetailService {
    static $inject = ['$http'];
    constructor(HttpService: angular.IHttpService) {
        this.HttpService = HttpService;
    }

    HttpService: angular.IHttpService;

    //get all unitloading modul detail data
    getAlltUnitLoadingDetail(voyageNumber: string) {
        return this.HttpService.get<DeliveryUnitLoadingDetailModel[]>('/api/v1/DeliveryUnitLoadingApi/GetUnitLoadingDetailModels/' + voyageNumber);
    }


    

}

export class DeliveryUnitLoadingDetailModel {
    frameNo: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    customerAssign: boolean;
    requestedDeliveryTime: Date;
    status: string;
    dateTemp: string;
    estimatedPDCIn: Date;
    dateTempEPDC: string;
}

export class SearchData {
    frameNo: string;
    katashiki: string;
    suffix: string;
    model: string;
    tipe: string;
    warna: string;
    branch: string;
    customerAssign: boolean;
    requestedDeliveryTime: Date;
    status: string;
    dateTemp: string;
    dateTempEPDC: string;
}
