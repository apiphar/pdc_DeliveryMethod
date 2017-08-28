import * as Angular from 'angular';
import * as uib from 'angular-ui-bootstrap';
import * as Lodash from 'lodash';
import * as Service from '../services';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';

export class PolaRangkaianTahapAkhirPenerapanController implements angular.IController {
    static $inject = ['PolaRangkaianTahapAkhirPenerapanService'];

    polaRangkaianTahapAkhirPenerapan: Service.PolaRangkaianTahapAkhirPenerapanService;

    dataRangkaianTahapAkhirPenerapan: any;
    searchFilter = {};
    loading: boolean;

    routingDictionaryTailCode: any;
    description: any;
    allModel: boolean = false;
    allDealer: boolean = false;

    status = {};

    constructor(polaRangkaianTahapAkhirPenerapan: Service.PolaRangkaianTahapAkhirPenerapanService) {
        this.polaRangkaianTahapAkhirPenerapan = polaRangkaianTahapAkhirPenerapan;
    }

    $onInit() {
        this.getData();
    }

    getData() {
        this.dataRangkaianTahapAkhirPenerapan = null;
        this.loading = true;
        this.allModel = false;
        this.allDealer = false;

        this.polaRangkaianTahapAkhirPenerapan.getData().then(response => {
            this.dataRangkaianTahapAkhirPenerapan = response.data;

            this.totalItems = this.dataRangkaianTahapAkhirPenerapan.routingDictionaryTail.length;

            //radio
            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.routingDictionaryTail, (routing) => {
                routing.selected = false;
            });

            //katsu
            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carModel, (model) => {
                model.selected = false;
            });
            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carSeries, (series) => {
                series.selected = false;
            });
            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carType, (suffix) => {
                suffix.selected = false;
            });

            //dealer
            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.dealer, (dealer) => {
                dealer.selected = false;
            });
            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.branch, (branch) => {
                branch.selected = false;
            });
        }).catch(response => {
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        }).finally(() => {
            this.loading = false;
        });
    }

    //select routing dictionary Tail
    selectRouting(routingDictionaryTail) {
        this.routingDictionaryTailCode = routingDictionaryTail.processTailTemplateCode;
        this.description = routingDictionaryTail.description;

        if (this.dataRangkaianTahapAkhirPenerapan.routingDictionaryTailVehicleMapping.length > 0) {
            let found = null;

            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carType, (suffix) => {
                found = Lodash.find(this.dataRangkaianTahapAkhirPenerapan.routingDictionaryTailVehicleMapping, {
                    'processTailTemplateCode': routingDictionaryTail.processTailTemplateCode,
                    'katashiki': suffix.katashiki,
                    'suffix': suffix.suffix
                });

                if (found) suffix.selected = true;
                else suffix.selected = false;

                let carSeries = Lodash.find(this.dataRangkaianTahapAkhirPenerapan.carSeries, {
                    'carSeriesCode': suffix.carSeriesCode
                });
                this.stateSeries(carSeries);
            });

            Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.branch, (branch) => {
                found = Lodash.find(this.dataRangkaianTahapAkhirPenerapan.routingDictionaryTailVehicleMapping, {
                    'processTailTemplateCode': routingDictionaryTail.processTailTemplateCode,
                    'branchCode': branch.branchCode
                });

                if (found) branch.selected = true;
                else branch.selected = false;
                
                let dealer = Lodash.find(this.dataRangkaianTahapAkhirPenerapan.dealer, {
                    'dealerCode': branch.dealerCode
                });
                this.stateDealer(dealer);
            });
        }
    }

    //validasi field untuk button
    validation() {
        let carType = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carType, ['selected', true]);
        let branch = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.branch, ['selected', true]);

        if (this.routingDictionaryTailCode == null) {
            return false;
        }
        if (carType.length < 1) {
            return false;
        }
        if (branch.length < 1) {
            return false;
        }
        return true;
    }

    //get katsu
    addData(Form: angular.IFormController) {
        if (this.validation()) {
            let carType = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carType, ['selected', true]);
            let branch = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.branch, ['selected', true]);

            let jsonTahapAkhirPenerapan = {}
            jsonTahapAkhirPenerapan["code"] = this.routingDictionaryTailCode;
            jsonTahapAkhirPenerapan["description"] = this.description;

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/PolaRangkaianPenerapanAlertify.html"), jsonTahapAkhirPenerapan),
                () => {
                    this.polaRangkaianTahapAkhirPenerapan.postData(this.routingDictionaryTailCode, carType, branch).then(response => {
                        Alertify.success("Data berhasil disimpan");
                    }).catch(response => {
                        if (response.status == "500") {
                            Alertify.error("Koneksi ke server bermasalah");
                        } else {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.getData();
                        this.reset(Form);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }
    }

    // select all
    selectAllDealer(status) {
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.dealer, function (dealer) {
            dealer.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.branch, function (branch) {
            branch.selected = status;
        });
    }

    // check dealer
    selectDealer($event, status, dealerCode) {
        $event.stopPropagation();
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.branch, function (branch) {
            if (branch.dealerCode == dealerCode ) {
                branch.selected = status;
            }
        });
    }

    // select all
    selectAllModel(status) {
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carModel, function (model) {
            model.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carSeries, function (series) {
            series.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.katashiki, function (katashiki) {
            katashiki.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carType, function (suffix) {
            suffix.selected = status;
        });
    }

    // check model
    selectModel($event, status, carModelCode) {
        $event.stopPropagation();
        var katsu = this.dataRangkaianTahapAkhirPenerapan.carType;

        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carSeries, function (series) {
            if (series.carModelCode == carModelCode) {
                series.selected = status;

                Lodash.forEach(katsu, function (suffix) {
                    if (suffix.carSeriesCode == series.carSeriesCode) {
                        suffix.selected = status;
                    }
                });
            }
        });
        
        this.dataRangkaianTahapAkhirPenerapan.carType = katsu;
    }

    // check car series
    selectSeries($event, status, carSeriesCode) {
        $event.stopPropagation();
        var katsu = this.dataRangkaianTahapAkhirPenerapan.carType;

        Lodash.forEach(katsu, function (suffix) {
            if (suffix.carSeriesCode == carSeriesCode) {
                suffix.selected = status;
            }
        });

        this.dataRangkaianTahapAkhirPenerapan.carType = katsu;
    }

    // check katashiki
    selectKatashiki($event, status, carSeriesCode, katashiki) {
        $event.stopPropagation();
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carType, function (suffix) {
            if (suffix.carSeriesCode == carSeriesCode && suffix.katashiki == katashiki) {
                suffix.selected = status;
            }
        });
    }

    stateSeries(carSeries) {
        carSeries.selected = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carType, {
            'selected': true,
            'carSeriesCode': carSeries.carSeriesCode
        }).length != 0;
        this.stateModel(Lodash.find(this.dataRangkaianTahapAkhirPenerapan.carModel, ['carModelCode', carSeries.carModelCode]));
    }

    stateModel(carModel) {
        carModel.selected = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carSeries, {
            'selected': true,
            'carModelCode': carModel.carModelCode
        }).length != 0;
        this.stateAllModel();
    }

    stateAllModel() {
        this.allModel = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carModel, ['selected', true]).length != 0;
    }

    stateAllDealer() {
        this.allDealer = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.dealer, ['selected', true]).length != 0;
    }

    stateDealer(dealer) {
        dealer.selected = Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.branch, {
            'selected': true,
            'dealerCode': dealer.dealerCode
        }).length != 0;
        this.stateAllDealer();
    }

    //filter branch
    filterBranch(dealerCode) {
        return Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.branch, ['dealerCode', dealerCode]);
    }

    //filter car series
    filterSeries(carModelCode) {
        return Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carSeries, ['carModelCode', carModelCode]);
    }

    //filter suffix
    filterSuffix(carSeriesCode, katashiki) {
        return Lodash.filter(this.dataRangkaianTahapAkhirPenerapan.carType, {
            'carSeriesCode': carSeriesCode
        });
    }

    reset(Form: angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();

        Angular.forEach(this.status, (accordion) => {
            Angular.forEach(accordion, (value, key) => {
                accordion[key] = false;
            });
        });

        //radio
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.routingDictionaryTail, (routing) => {
            routing.selected = false;
        });

        //katsu
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carModel, (model) => {
            model.selected = false;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carSeries, (series) => {
            series.selected = false;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.carType, (suffix) => {
            suffix.selected = false;
        });

        //dealer
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.dealer, (dealer) => {
            dealer.selected = false;
        });
        Lodash.forEach(this.dataRangkaianTahapAkhirPenerapan.branch, (branch) => {
            branch.selected = false;
        });

        this.searchFilter = {};

        this.allModel = false;
        this.allDealer = false;
    }

    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    // sorting data by column name
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    // search data
    search(data) {
        this.totalItems = data.length;
        this.setPage(1);
    }

    // set page in html
    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    //back to last page
    back() {
        window.location.href = "/PolaRangkaianTahapAkhir";
    }

    // go to next page
    generatePola() {
        window.location.href = "/GeneratePolaRangkaianRute";
    }
}

export class PolaRangkaianTahapAkhirPenerapanComponent implements angular.IComponentOptions {
    controller = PolaRangkaianTahapAkhirPenerapanController;
    controllerAs = 'me';

    template = require('./PolaRangkaianTahapAkhirPenerapan.html');
}