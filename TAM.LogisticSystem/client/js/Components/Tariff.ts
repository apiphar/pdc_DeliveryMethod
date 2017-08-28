import * as Services from '../services';
export class SchemeTable {
    scheme: number;
    bm: number; 
    constructor(_scheme: number, _bm: number) {
        this.scheme = _scheme;
        this.bm = _bm;
    }
}

export class TariffController implements angular.IController {

    static $inject = ['TariffService'];
    SchemeData: Array<SchemeTable> = [];
    UpdateScheme: Array<SchemeTable> = [];
    isUpdate: number;

    AllTariffData: string;
    sService: Services.TariffService;
    tariffId: any;
    HSCode: string;
    PPH: number;
    PPn: number;
    PPnBM: number;
    EffectiveFrom: Date;

    Scheme: any;
    BM: number;
    constructor(private _sService: Services.TariffService) {
        this.sService = _sService;
    }

    refreshData() {
        this.sService.GetData().then(response => {            
            this.AllTariffData = response.data;
            this.totalItems = this.AllTariffData.length;
        });
        this.reset();
    }

    $onInit() {
        this.refreshData();
    }

    AddData() {
        this.SchemeData.push(new SchemeTable(this.Scheme, this.BM));
        console.log(this.SchemeData);
        this.Scheme = 0;
        this.BM = 0;
    }

    DeleteData(item: SchemeTable) {
        var index = this.SchemeData.indexOf(item);
        this.SchemeData.splice(index, 1);
    }


    createData() {
        this.sService.Add(this.HSCode, this.PPH, this.PPn, this.PPnBM, this.EffectiveFrom, this.SchemeData).then(response => {
            this.refreshData();
        });
    }
    updateData() {
        this.sService.Update(this.tariffId, this.HSCode, this.PPH, this.PPn, this.PPnBM, this.EffectiveFrom, this.Scheme, this.BM).then(response => {
            this.refreshData();
        });
    }
    Remove(data) {
        this.sService.DeleteData(data.tariffId).then(response => {
            this.refreshData();
        });
    }
    SelectEdit(data) {
        this.tariffId = data.tariffId;
        this.HSCode = data.hsCode;
        this.PPH = data.pph;
        this.PPn = data.pPn;
        this.PPnBM = data.pPnBM;
        this.EffectiveFrom = data.effectiveFrom;
        this.SchemeData = [];
        this.Scheme = data.scheme;
        this.BM = data.bm;
        this.isUpdate = 1;
    }
    reset() {
        this.HSCode = null;
        this.PPH=0;
        this.PPn = 0;
        this.PPnBM = 0;
        this.EffectiveFrom = null;
        this.SchemeData = [];
        this.isUpdate = 0;
        this.Scheme = "";
        this.BM = 0;
        this.isUpdate = 0;

    }
    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    }
}
   

export class TariffComponent implements angular.IComponentOptions {
    controller= TariffController;
    controllerAs= 'me';
   
    template= require('./Tariff.html');
}
