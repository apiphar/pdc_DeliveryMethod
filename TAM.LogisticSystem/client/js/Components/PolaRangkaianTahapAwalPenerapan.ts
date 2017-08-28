import * as Angular from 'angular';
import * as uib from 'angular-ui-bootstrap';
import * as Lodash from 'lodash';
import * as Service from '../services';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';

export class PolaRangkaianTahapAwalPenerapanController implements Angular.IController {
    static $inject = ['PolaRangkaianTahapAwalPenerapanService'];

    polaRangkaianTahapAwalPenerapan: Service.PolaRangkaianTahapAwalPenerapanService;

    dataRangkaianTahapAwalPenerapan: any;
    searchFilter = {};
    loading: boolean;

    routingDictionaryHeadCode: any;
    description: any;
	allModel: any = false;

    constructor(polaRangkaianTahapAwalPenerapan: Service.PolaRangkaianTahapAwalPenerapanService) {
        this.polaRangkaianTahapAwalPenerapan = polaRangkaianTahapAwalPenerapan;
    }

    $onInit() {
        this.getData();
    }

    getData() {
        this.dataRangkaianTahapAwalPenerapan = null;
        this.loading = true;
        this.allModel = false;

        this.polaRangkaianTahapAwalPenerapan.getData().then(response => {
            this.dataRangkaianTahapAwalPenerapan = response.data;
            console.log(response.data);
            this.totalItems = this.dataRangkaianTahapAwalPenerapan.routingDictionaryHead.length;

            Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.routingDictionaryHead, (routing) => {
                routing.selected = false;
            });

            Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carModel, (model) => {
                model.selected = false;
            });
            Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carSeries, (series) => {
                series.selected = false;
            });
            Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carType, (suffix) => {
                suffix.selected = false;
            });
        }).catch(response => {
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        }).finally(() => {
            this.loading = false;
        });
    }

    //select routing dictionary head
    selectRouting(routingDictionaryHead) {
        this.routingDictionaryHeadCode = routingDictionaryHead.processHeadTemplateCode;
        this.description = routingDictionaryHead.description;

        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carType, (suffix) => {
            let found = Lodash.find(this.dataRangkaianTahapAwalPenerapan.routingDictionaryHeadVehicleMapping, {
                'processHeadTemplateCode': routingDictionaryHead.processHeadTemplateCode,
                'katashiki': suffix.katashiki,
                'suffix': suffix.suffix
            });

            if (found) {
                suffix.selected = true;
            } else {
                suffix.selected = false;
            }
            let carSeries = Lodash.find(this.dataRangkaianTahapAwalPenerapan.carSeries, {
                'carSeriesCode': suffix.carSeriesCode
            });
            this.stateSeries(carSeries);
        });
    }

    validation() {
        let data = Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carType, ['selected', true]);
        if (this.routingDictionaryHeadCode == null) {
            return false;
        }
        if (data.length < 1) {
            return false;
        }
        return true;
    }

    //get katsu
    addData(Form: angular.IFormController) {
        if (this.validation()) {
            let data = Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carType, ['selected', true]);

            let jsonTahapAwalPenerapan = {}
            jsonTahapAwalPenerapan["code"] = this.routingDictionaryHeadCode;
            jsonTahapAwalPenerapan["description"] = this.description;

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/PolaRangkaianPenerapanAlertify.html"), jsonTahapAwalPenerapan),
                () => {
                    this.polaRangkaianTahapAwalPenerapan.postData(this.routingDictionaryHeadCode, data).then(response => {
                        Alertify.success("Data berhasil disimpan");
                    }).catch(response => {
                        if (response.status == "500"){
                            Alertify.error("Koneksi ke server bermasalah");
                        } else {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        Form.$setPristine();
                        Form.$setUntouched();
                        this.getData();
                        this.searchFilter = {};
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }
    }

    // select all
    selectAllModel(status) {
        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carModel, function (model) {
            model.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carSeries, function (series) {
            series.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.katashiki, function (katashiki) {
            katashiki.selected = status;
        });
        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carType, function (suffix) {
            suffix.selected = status;
        });
    }

    // check model
    selectModel($event, status, carModelCode, parent) {
        $event.stopPropagation();
        let katsu = this.dataRangkaianTahapAwalPenerapan.carType;

        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carSeries, function (series) {
            if (series.carModelCode == carModelCode) {
                series.selected = status;

                Lodash.forEach(katsu, function (suffix) {
                    if (suffix.carSeriesCode == series.carSeriesCode) {
                        suffix.selected = status;
                    }
                });
            }
        });
        
        this.dataRangkaianTahapAwalPenerapan.carType = katsu;
    }

    // check car series
    selectSeries($event, status, carSeriesCode) {
        $event.stopPropagation();
        let katsu = this.dataRangkaianTahapAwalPenerapan.carType;

        Lodash.forEach(katsu, function (suffix) {
            if (suffix.carSeriesCode == carSeriesCode) {
                suffix.selected = status;
            }
        });

        this.dataRangkaianTahapAwalPenerapan.carType = katsu;
    }

    // check katashiki
    selectKatashiki($event, status, carSeriesCode, katashiki) {
        $event.stopPropagation();
        Lodash.forEach(this.dataRangkaianTahapAwalPenerapan.carType, function (suffix) {
            if (suffix.carSeriesCode == carSeriesCode && suffix.katashiki == katashiki) {
                suffix.selected = status;
            }
        });
    }

    stateSeries(carSeries) {
        carSeries.selected = Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carType, {
            'selected': true,
            'carSeriesCode': carSeries.carSeriesCode
        }).length != 0;
        this.stateModel(Lodash.find(this.dataRangkaianTahapAwalPenerapan.carModel, ['carModelCode', carSeries.carModelCode]));
    }

    stateModel(carModel) {
        carModel.selected = Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carSeries, {
            'selected': true,
            'carModelCode': carModel.carModelCode
        }).length != 0;
        this.stateAll();
    }

    stateAll() {
        this.allModel = Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carModel, ['selected', true]).length != 0;
    }
    
    //filter car series
    filterSeries(carModelCode) {
        return Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carSeries, ['carModelCode', carModelCode]);
    }

    //filter suffix
    filterSuffix(carSeriesCode) {
        return Lodash.filter(this.dataRangkaianTahapAwalPenerapan.carType, {
            'carSeriesCode': carSeriesCode
        });
    }

    //reset form
    reset(Form: angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();
		this.allModel = false;
        this.getData();
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
        window.location.href = "/PolaRangkaianTahapAwal";
    }
}

export class PolaRangkaianTahapAwalPenerapanComponent implements Angular.IComponentOptions {
    controller = PolaRangkaianTahapAwalPenerapanController;
    controllerAs = 'me';

    template = require('./PolaRangkaianTahapAwalPenerapan.html');
}