
import * as service from '../services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';

class CBUFinalizePIBController implements angular.IController {
    static $inject = ['CBUFinalizePIBService', '$rootScope'];

    constructor(cbuFinalizePIBService: service.CBUFinalizePIBService, root: angular.IRootScopeService) {
        this.cbuFinalizePIBService = cbuFinalizePIBService;
        this.root = root;
    }

    $onInit() {
        this.getCurrencyData();
        this.getPercentageData();
        this.finalizeDisable = true;
        this.finalizeEnable = false;
        this.getBroadcastData();
    }
    //private readonly
    cbuFinalizePIBService: service.CBUFinalizePIBService;
    root: angular.IRootScopeService;

    //model
    currencyModels: service.CurrencyModel[];
    currencyModel: service.CurrencyModel;
    finalizeTableModels: service.FinalizeTableModel[];
    percentageModel: service.PercentageModel;
    percentageModels: service.PercentageModel[];
    finalizeInfoModel: service.CBUFinalizePIBModel;
    tempFinalizeInfoModel: service.CBUFinalizePIBModel;
    finalizePIBModel: service.FinalizePIBModel;

    //variable
    noAju: string;
    schema: string;
    textFile: string;
    file: File;
    excelModels: any;
    excelModel: any;
    finalizeDisable: boolean;
    finalizeEnable: boolean;

    //mengambil data dari broadcast
    getBroadcastData() {
        this.root.$on('finalizeInfoModel', (event, data) => {
            this.finalizeInfoModel = data;
            this.tempFinalizeInfoModel = data;
            this.noAju = this.finalizeInfoModel.noAju;
            this.initCurrencyDropDown(this.finalizeInfoModel.currencySymbol);
            this.schema = this.finalizeInfoModel.schema;
            this.getPercentageforCalculate();
        });
        this.root.$on('finalizeTableModels', (event, data) => {
            this.finalizeTableModels = data;
        });
    }

    //mengambil seluruh currency data dari currencyDB
    getCurrencyData() {
        this.cbuFinalizePIBService.getCurrencyData().then(response => {
            this.currencyModels = response.data as service.CurrencyModel[];
        });
    }

    //mengambil seluruh persentase data dari harmonizeDB
    getPercentageData() {
        this.cbuFinalizePIBService.getPercentageData().then(response => {
            this.percentageModels = response.data as service.PercentageModel[];
        });
    }

    //mengambil data yang digunakan untuk kalkulasi di validasi
    getPercentageforCalculate() {
        this.percentageModel = _.find(this.percentageModels, ['schema', this.schema]);
    }

    //untuk upload excel file
    uploadFile() {
        if (this.file != null) {
            this.cbuFinalizePIBService.uploadFile(this.file).then(response => {
                this.excelModels = response.data;
                this.textFile = this.file.name;
                this.excelModel = _.find(this.excelModels, ['noAju', this.noAju]);
            }).catch(response => {
                this.file = null;
                alertify.error("File input harus excel");
            });
        }
    }

    //untuk inisiasi dropdown dari currency
    initCurrencyDropDown(currency: string) {
        this.currencyModel = new service.CurrencyModel();
        this.currencyModel = _.find(this.currencyModels, ['currency', currency]);
    }

    //kalkulasi pembulatan 1000
    calculateTotal(total) {
        let substractOver: number;
        if (total % 1000 != 0) {
            substractOver = 0;
            substractOver = 1000 - (total % 1000);
            return substractOver;
        }
        return 0;
    }

    //validasi schema
    changeSchema() {
        let self = this;
        let checkSchema: service.PercentageModel;
        _.forEach(this.finalizeTableModels, function (o) {
            checkSchema = _.find(self.percentageModels, { 'hsCode': o.hsCode, 'schema': self.schema });
            if (checkSchema == null) {
                alertify.error('Schema tidak cocok');
                return false;
            }
            // TIE: START
            return 0;
            // TIE: END
        });
        if (checkSchema == null) {
            return 0;
        }
        else {
            this.percentageModel = checkSchema;
        }

        // TIE: START
        return 0;
        // TIE: END
    }

    //validasi apakah schema cocok? file ada? ndpbm berubah atau tidak? penghitungan data
    validate() {
        if (this.file == undefined) {
            alertify.error("Harap pilih file");
            return 0;
        }
        if (this.changeSchema() == 0) {
            // TIE: START
            return 0;
            // TIE: END
        }
        if (this.finalizeInfoModel.currencyRate != this.currencyModel.ndpbm) {
            let self = this;
            let totalBm: number = 0;
            let totalPph: number = 0;
            let totalPpn: number = 0;
            let totalPpnBm: number = 0;
            _.forEach(this.finalizeTableModels, function (o) {
                o.priceRupiah = o.price * self.currencyModel.ndpbm;
                o.bm = o.priceRupiah * self.percentageModel.beaMasukPercentage / 100;
                totalBm += o.bm;
            });
            console.log(this.calculateTotal(totalBm));
            this.finalizeTableModels[this.finalizeTableModels.length - 1].bm += this.calculateTotal(totalBm);
            console.log(this.finalizeTableModels[this.finalizeTableModels.length - 1].bm);
            _.forEach(this.finalizeTableModels, function (o) {
                o.importValue = o.bm + o.priceRupiah;
                o.pph = o.importValue * self.percentageModel.pphPercentage / 100;
                o.ppn = o.importValue * self.percentageModel.ppnPercentage / 100;
                o.ppnbm = o.importValue * self.percentageModel.ppnBmPercentage / 100;
                totalPph += o.pph;
                totalPpn += o.ppn;
                totalPpnBm += o.ppnbm;
            });
            this.finalizeTableModels[this.finalizeTableModels.length - 1].pph += this.calculateTotal(totalPph);
            this.finalizeTableModels[this.finalizeTableModels.length - 1].ppn += this.calculateTotal(totalPpn);
            this.finalizeTableModels[this.finalizeTableModels.length - 1].ppnbm += this.calculateTotal(totalPpnBm);
        }
        this.finalizeDisable = false;
        this.finalizeEnable = true;
        // TIE: START
        return 0;
        // TIE: END
    }

    finalizePIB() {
        this.finalizePIBModel = new service.FinalizePIBModel();
        this.finalizePIBModel.finalizeInfo = new service.FinalizeInfo();
        this.finalizePIBModel.finalizeTable = this.finalizeTableModels;
        this.finalizePIBModel.finalizeInfo.currencyRateFinal = this.currencyModel.ndpbm;
        this.finalizePIBModel.finalizeInfo.tanggalAjuApproved = this.excelModel.approvedDate;
        this.finalizePIBModel.finalizeInfo.schemaFinal = this.schema;
        this.finalizePIBModel.finalizeInfo.nomorAju = this.noAju;

        this.cbuFinalizePIBService.finalizePIB(this.finalizePIBModel).then(response => {
            alertify.success('Sukses finalize PIB');
        }).catch(response => {
            alertify.error('Gagal finalize PIB');
        });
        this.cancel();
        this.root.$broadcast('isShow', false);
    }

    cancel() {
        this.getCurrencyData();
        this.finalizeDisable = true;
        this.initCurrencyDropDown(this.tempFinalizeInfoModel.currencySymbol);
        this.schema = this.tempFinalizeInfoModel.schema;
        this.cbuFinalizePIBService.getAllPreFinalizeData(this.noAju).then(response => {
            this.finalizeTableModels = response.data as service.FinalizeTableModel[];
        });
        this.excelModel = null;
        this.textFile = null;
        this.finalizeDisable = true;
        this.finalizeEnable = false;
    }
}

let cbuFinalizePIB = {
    controller: CBUFinalizePIBController,
    controllerAs: 'me',
    template: require('./CBUFinalizePIB.html')
}

export { cbuFinalizePIB }