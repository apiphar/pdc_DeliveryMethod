
import * as service from '../services';
import * as _ from 'lodash';

class CBUFinalizePIBController implements angular.IController {
    static $inject = ['CBUFinalizePIBService', '$rootScope'];

    constructor(cbuFinalizePIBService: service.CBUFinalizePIBService, root: angular.IRootScopeService) {
        this.cbuFinalizePIBService = cbuFinalizePIBService;
        this.root = root;
    }

    $onInit()
    {
        this.getAllData();
        this.startDate = new Date(1, 1, 1);
        this.endDate = new Date();
        this.isShow = false
        this.root.$on('isShow', (event, data) => {
            this.getAllData();
            this.isShow = data;
        });
    }
    //private readonly
    cbuFinalizePIBService: service.CBUFinalizePIBService;
    allCBUFinalizePIBTableModel: service.CBUFinalizePIBModel[];
    cbuFinalizePIBTableModel: service.CBUFinalizePIBModel[];
    root: angular.IRootScopeService;

    //variable
    popUpStartDate: boolean;
    popUpEndDate: boolean;
    noAju: string;
    startDate: Date;
    endDate: Date;
    isShow: boolean;
    currencyRate: number;
    finalizeTableModels: service.FinalizeTableModel[];
    
    //date picker function
    openStartDate() {
        this.popUpStartDate = true;
    }
    openEndDate() {
        this.popUpEndDate = true;
    }

    //getAllData
    getAllData() {
        this.cbuFinalizePIBService.getAllData().then(response => {
            this.allCBUFinalizePIBTableModel = response.data as service.CBUFinalizePIBModel[];
            this.cbuFinalizePIBTableModel = response.data as service.CBUFinalizePIBModel[];           
        });
    }

    //filter data
    filter() {
        let self = this;
        this.cbuFinalizePIBTableModel = _.filter(this.allCBUFinalizePIBTableModel, function (o) {
            if (self.noAju == null) {
                return new Date(o.ajuDate) >= self.startDate && new Date(o.ajuDate) <= self.endDate;
            }            
            return new Date(o.ajuDate) >= self.startDate && new Date(o.ajuDate) <= self.endDate && _.includes(o.noAju, self.noAju);            
        });
    }

    selectedNomorAju(finalizeInfoModel: service.CBUFinalizePIBModel) {
        this.isShow = true;
        this.getAllPreFinalizeData(finalizeInfoModel.noAju);
        this.root.$broadcast('finalizeInfoModel', finalizeInfoModel);
    }

    getAllPreFinalizeData(noAju: string) {
        this.cbuFinalizePIBService.getAllPreFinalizeData(noAju).then(response => {
            this.finalizeTableModels = response.data as service.FinalizeTableModel[];
            this.root.$broadcast('finalizeTableModels', this.finalizeTableModels);
        });
    }
}

let cbuImportFinalizePIB = {
    controller: CBUFinalizePIBController,
    controllerAs: 'me',
    template: require('./CBUImportFinalizePIB.html')
}

export { cbuImportFinalizePIB }