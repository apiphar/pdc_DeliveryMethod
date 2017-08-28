

export class CBUFinalizePIBService {
    static $inject = ['$http'];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    //private readonly
    httpService: angular.IHttpService;

    getAllData() {
        return this.httpService.get<CBUFinalizePIBModel[]>('api/v1/CBUFinalizePIBApi/GetAllImportData');
    }
    getAllPreFinalizeData(noAju: string) {
        return this.httpService.get<FinalizeTableModel[]>('api/v1/CBUFinalizePIBApi/GetAllPreFinalizeData/'+noAju);
    }
    getCurrencyData() {
        return this.httpService.get<CurrencyModel[]>('api/v1/CBUFinalizePIBApi/GetCurrencyData');
    }
    getPercentageData() {
        return this.httpService.get<PercentageModel[]>('api/v1/CBUFinalizePIBApi/GetPercentageData');
    }
    finalizePIB(finalizePIBModel: FinalizePIBModel) {
        return this.httpService.post('api/v1/CBUFinalizePIBApi/Finalize', finalizePIBModel);
    }
    uploadFile(file: File) {
        let excelFile = this.transformRequestToFormData(file);
        return this.httpService.post('api/v1/CBUFinalizePIBApi/ImportFromExcel', excelFile, {
            headers: { "Content-Type": undefined }
        });
    }
    transformRequestToFormData(excelFile: File) {
        let formData: FormData = new FormData();
        formData.append("file", excelFile);
        return formData;
    }
}

export class CBUFinalizePIBModel {
    noAju: string;
    ajuDate: Date;
    totalQty: number;
    currencySymbol: string;
    schema: string;
    currencyRate: number;
}

export class CurrencyModel {
    currency: string;
    ndpbm: number;
}

export class FinalizeTableModel {
    invoiceNumber: string;
    frameNumber: string;
    engineNumber: string;
    katashiki: string;
    globalsuffix: string;
    localsuffix: string;
    hsCode: string;
    edNumber: string;
    price: number;
    priceRupiah: number;
    bm: number;
    importValue: number;
    pph: number;
    ppn: number;
    ppnbm: number;
}

export class PercentageModel {
    beaMasukPercentage: number;
    hsCode: string;
    schema: string;
    pphPercentage: number;
    ppnPercentage: number;
    ppnBmPercentage: number;
}

export class FinalizeInfo {
    nomorAju: string;
    schemaFinal: string;
    tanggalAjuApproved: Date;
    currencyRateFinal: number;
}

export class FinalizePIBModel{
    finalizeTable: FinalizeTableModel[];
    finalizeInfo: FinalizeInfo;
}