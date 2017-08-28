

export class FormARequestService {
    static $inject = ['$http'];

    http: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.http = $http;
    }

    getInvoicesList() {
        return this.http.get<FormARequestInvoiceViewModel[]>("/api/v1/FormARequestApi/InvoicesList");
    }

    updatePIB(updateInvoicesList: FormARequestInvoiceViewModel[]) {
        return this.http.post<FormARequestFormModel>("/api/v1/FormARequestApi/UpdatePIB", updateInvoicesList);
    }
}

export class FormARequestInvoiceViewModel {
    nomorAju: string;
    nomorPIB: string;
    tanggalPIB: Date;
    invoiceNumber: string;
    invoiceDate: Date;
    shipmentInvoiceDetailId: number;
    frameNumber: string;
    engineNumber: string;
    dtplod: Date;
    isAction: boolean;
}

export class FormARequestFormModel {
    guid: string;
    fileName: string;
}