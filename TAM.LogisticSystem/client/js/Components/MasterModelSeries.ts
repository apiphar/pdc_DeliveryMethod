import * as Angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';

export class MasterModelSerieslController implements angular.IController {
    static $inject = ['MasterModelSeriesService', '$rootScope'];

    masterModelSeries: Service.MasterModelSeriesService;
    $rootScope: angular.IRootScopeService;

    viewby = 1;
    itemsCount = 5;
    currentPage = 1;
    itemsPerPage = this.viewby;
    maxSize = 5;
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    totalItems: number;
    pageNumber: number;
    searchString = {};
    orderState: boolean = false;
    orderString: string;
    count: number;

    data: any;
    dataModel: any;

    masterModel: any;

    masterModelSeriesCode: string;
    masterModelName: string;

    masterModelCode: string;
    masterName: string;

    selectedMasterModel: string;

    editMe: boolean;

    pageState: boolean = true;
    loader: boolean = true;
    isCodeExists: boolean = false;


    jsonMasterModelSeries = {
        "Kode Model": null,
        "Kode Model Series": null,
        "Model Series": null
    }

    isFormValid(form: angular.IFormController) {
        return Angular.equals(form.$error, {});
    }



    constructor(masterModelSeries: Service.MasterModelSeriesService, rootScope: angular.IRootScopeService) {
        this.count = 0;
        this.totalItems = 0;
        this.masterModelSeries = masterModelSeries;
        this.$rootScope = rootScope;
        this.editMe = false;

    }

    refreshData(Form?: angular.IFormController) {
        this.masterModelSeries.GetData().then(response => {
            this.data = response.data;
            this.totalItems = response.data.length;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.editMe = false;
            this.loader = false;
        });
        this.masterModel = null;
        this.masterModelSeriesCode = '';
        this.masterModelName = '';

        this.masterModelSeries.GetDropdownCarModel().then(response => {
            this.dataModel = response.data;
        });


    }

    $onInit() {
        this.refreshData();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
        });
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        Angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["insert"] = 1;
        else if (action.toLowerCase() == "update") convertResult["update"] = 1;
        else if (action.toLowerCase() == "delete") convertResult["delete"] = 1;

        convertResult["grid"] = tempJson;
        return convertResult;
    }





    selectEdit(data) {
        console.log(data);
        this.masterModelSeriesCode = data.carSeriesCode;
        this.masterModelName = data.carSeriesName;
        this.masterModel = _.find(this.dataModel, ['carModelCode', data.carModelCode]);
        this.masterModel = _.find(this.dataModel, ['name', data.carModelName]);
        this.editMe = true;

    }

    selectDelete(data) {
        this.masterModelSeriesCode = data.carSeriesCode;
        this.masterName = data.name;
    }

    SelectModelOnChange() {
        this.masterName = this.masterModel["name"];
        this.masterModelCode = this.masterModel["carModelCode"];
    }



    reset(Form: angular.IFormController) {
        this.editMe = false;
        this.masterModel = "";
        this.masterModelName = "";
        this.masterModelSeriesCode = "";
        this.searchString = {};
        if (Form) {
            Form.$setPristine();
        }
    }



    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            // angular's default (non-strict) internal comparator
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };
    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);

    }

    Download(result: any) {
        let tempData = [];
        Angular.forEach(result, (data) => {
            tempData.push(data.carSeriesCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "carSeries";
        info.title = "Model Series";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    Upload() {
        let info: any = {};
        info.master = "carSeries";
        info.title = "Model Series";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }


    addData(Form: angular.IFormController) {
        this.jsonMasterModelSeries["Kode Model"] = this.masterModel['carModelCode'] + ' - ' + this.masterModel['name'];
        this.jsonMasterModelSeries["Kode Model Series"] = this.masterModelSeriesCode;
        this.jsonMasterModelSeries["Model Series"] = this.masterModelName;


        if (this.masterModel == null) {
            Alertify.error('Kode Model harus dipilih');
        } else if (this.masterModelName == '') {
            Alertify.error('Model Series harus diisi');
        } else {
            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonMasterModelSeries)),
                () => {
                    this.masterModelSeries.PostData(this.masterModelSeriesCode, this.masterModelName, this.masterModel['carModelCode']).then(response => {
                        if (response.data == 1) {
                            Alertify.success("Data berhasil disimpan");
                        } else if (response.data == 0) {
                            Alertify.error('Kode Model Series telah terdaftar');
                        }

                    }).catch(response => {
                        if (response.status == "500") {
                            Alertify.error("Data gagal disimpan");
                        } else {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.refreshData();
                        this.setPage(this.currentPage);
                        this.reset(Form);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

    }

    updatedData(Form: angular.IFormController) {
        this.jsonMasterModelSeries["Kode Model"] = this.masterModel['carModelCode'] + ' - ' + this.masterModel['name'];
        this.jsonMasterModelSeries["Kode Model Series"] = this.masterModelSeriesCode;
        this.jsonMasterModelSeries["Model Series"] = this.masterModelName;

        if (this.masterModel == null) {
            Alertify.error('Kode Model harus dipilih');
        } else if (this.masterModelName == '') {
            Alertify.error('Model Series harus diisi');
        } else {
            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonMasterModelSeries)),
                () => {
                    this.masterModelSeries.UpdateData(this.masterModelSeriesCode, this.masterModelName, this.masterModel['carModelCode']).then(response => {
                        Alertify.success("Data berhasil disimpan");
                    }).catch(response => {
                        if (response.status == "500") {
                            Alertify.error("Data gagal disimpan");
                        } else {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.refreshData();
                        this.setPage(this.currentPage);
                        this.reset(Form);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

    }

    deleteData(data,myForm:angular.IFormController) {
        this.jsonMasterModelSeries["Kode Model"] = data.carModelCode + ' - ' + data.carModelName;
        this.jsonMasterModelSeries["Kode Model Series"] = data.carSeriesCode;
        this.jsonMasterModelSeries["Model Series"] = data.carSeriesName;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonMasterModelSeries)),
            () => {
                this.masterModelSeries.DeleteData(data.carSeriesCode).then(response => {
                    Alertify.success("Data berhasil dihapus");
                }).catch(response => {
                    if (response.status == "500") {
                        Alertify.error("Data gagal dihapus");
                    } else {
                        Alertify.error(response.data);
                    }
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(myForm);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    setPage(pageNo) {
        this.currentPage = pageNo;
    };

    cekPola() {

        let check = _.find(this.data, { 'carSeriesCode': this.masterModelSeriesCode });
        if (check != null) {
            this.isCodeExists = true;
            return 0;
        }
        this.isCodeExists = false;
        // TIE: START
        return 0;
        // TIE: END
    }

}



export class MasterModelSeriesComponent implements angular.IComponentOptions {
    controller = MasterModelSerieslController;
    controllerAs = 'mastermodelseries';

    template = require('./MasterModelSeries.html');
    bindings = {
        great: '@',
    };
}