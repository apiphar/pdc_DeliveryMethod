import * as Angular from 'angular';
import * as uib from 'angular-ui-bootstrap';
import * as Service from '../services';
import * as Lodash from 'lodash';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';

export class SalesAreaController implements Angular.IController {
    static $inject = ['SalesAreaService','$scope','$rootScope'];
    
    SalesArea: Service.SalesAreaService;

    dataSalesArea: any;
    searchFilter = {};

    //regex
    regexCode: RegExp = /^[a-zA-Z0-9]+$/;
    regexName: RegExp = /^[a-zA-Z0-9\s\-.,&\'\/]+$/;

    areaId: string = "";
    description: string = "";

    editCheck: boolean;

    deleteId: string;
    deleteDescription: string;

    $scope: angular.IScope;
	$rootScope: angular.IRootScopeService;
	pageState: boolean = true;

    updateState: boolean = false;
    loading: boolean;

    jsonSalesArea = {}

    constructor(salesArea: Service.SalesAreaService, _$scope: angular.IScope, _$rootScope: angular.IRootScopeService) {
        this.SalesArea = salesArea;
        this.$scope = _$scope;
		this.$rootScope = _$rootScope;
    }

    $onInit() {
        this.getDataSalesArea();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.getDataSalesArea();
        });
    }

    // get data from database
    getDataSalesArea() {
        this.dataSalesArea = null;
        this.loading = true;
        this.SalesArea.GetData().then(response => {
            this.dataSalesArea = response.data;
            this.totalItems = this.dataSalesArea.length;
        }).catch(response => {
            if (response.status == "500"){
                Alertify.error("Koneksi ke server bermasalah");
            }
        }).finally(() => {
            this.loading = false;
        });
    }

    // validasi untuk field code dan description
    validation() {
        let check = true;
        if (this.areaId == "") {
            Alertify.error("Kode Sales Area harus diisi");
            check = false;
        }
        if (this.description == "") {
            Alertify.error("Sales Area harus diisi");
            check = false;
        }
        if (!this.regexCode.test(this.areaId)) {
            Alertify.error("Kode Sales Area harus berformat alphanumeric");
            check = false;
        }
        if (this.areaId.length > 16) {
            Alertify.error("Kode Sales Area tidak boleh > 16");
            check = false;
        }
        if (!this.regexName.test(this.description)) {
            Alertify.error("Sales Area harus berformat alphanumeric");
            check = false;
        }
        if (this.areaId.length > 255) {
            Alertify.error("Sales Area tidak boleh > 255");
            check = false;
        }
        return check;
    }

    // insert to database
    addData(Form: Angular.IFormController) {
        if (this.validation()) {
            this.jsonSalesArea["Kode Sales Area"] = this.areaId;
            this.jsonSalesArea["Sales Area"] = this.description;

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonSalesArea)),
                () => {
                    this.SalesArea.PostData(this.areaId, this.description).then(response => {
                        Alertify.success("Data berhasil disimpan");
                        this.reset(Form);
                    }).catch(response => {
                        if (response.status == "500") {
                            Alertify.error("Koneksi ke server bermasalah");
                        } else if (response.status == "400") {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.getDataSalesArea();
                        this.setPage(this.currentPage);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }
    }

    // update to database
    updateData(Form: Angular.IFormController) {
        if (this.validation()) {
            this.jsonSalesArea["Kode Sales Area"] = this.areaId;
            this.jsonSalesArea["Sales Area"] = this.description;

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonSalesArea)),
                () => {
                    this.SalesArea.UpdateData(this.areaId, this.description).then(response => {
                        Alertify.success("Data berhasil disimpan");
                        this.reset(Form);
                    }).catch(response => {
                        if (response.status == "500") {
                            Alertify.error("Koneksi ke server bermasalah");
                        } else if (response.status == "400") {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.getDataSalesArea();
                        this.setPage(this.currentPage);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }
    }

    // delete to database
    deleteData(data, Form: angular.IFormController) {
        this.deleteId = data.salesAreaCode;
        this.deleteDescription = data.description;

        this.jsonSalesArea["Kode Sales Area"] = this.deleteId;
        this.jsonSalesArea["Sales Area"] = this.deleteDescription;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonSalesArea)),
            () => {
                this.SalesArea.DeleteData(this.deleteId).then(response => {
                    Alertify.success("Data berhasil dihapus");
                    this.reset(Form);
                }).catch(response => {
                    if (response.status == "500"){
                        Alertify.error("Koneksi ke server bermasalah");
                    } else if (response.status == "400"){
                        Alertify.error(response.data);
                    }
                }).finally(() => {
                    this.getDataSalesArea();
                    this.setPage(this.currentPage);
                });
            },
            () => { }
        ).set('labels', {ok:'Ya', cancel:'Tidak'});
    }

    // Selecting Data
    selectEdit(data) {
        this.areaId = data.salesAreaCode;
        this.description = data.description;
        this.editCheck = true;
    }

	download(result: any) {
		let tempData = [];
		Angular.forEach(result, (data) => {
			tempData.push(data.salesAreaCode);
		});
		this.pageState = false;
		let info: any = {};
		info.master = 'SalesArea';
		info.tipe = '3';
		this.$rootScope.$emit('UploadDownload', tempData, info);
	}
	
	upload() {
		let info: any = {};
		info.master = 'SalesArea';
		info.tipe = '2';
		this.pageState = false;
		this.$rootScope.$emit('UploadDownload', null, info);
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
    };

    /**
     * untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
     * @param action insert,update,delete (salah satu) *case insensitive
     * @param json json data -> { label : value , label2 : value2 }
     */
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

    reset(Form: Angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();

        this.areaId = null;
        this.description = null;
        this.deleteId = null;
        this.deleteDescription = null;
        this.editCheck = false;
        this.updateState = false;
        this.searchFilter = {};
    }
}

export class SalesAreaComponent implements Angular.IComponentOptions {
    controller = SalesAreaController;
    controllerAs = 'me';

    template = require('./SalesArea.html');
}
