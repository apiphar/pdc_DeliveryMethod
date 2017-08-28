import * as Service from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as angular from 'angular';

export class KodeShiftController implements angular.IController {
    static $inject = ['KodeShiftService', '$scope', '$rootScope'];

    //declare IFormController
    loading: boolean = true;

    Created: boolean;
    Edited: boolean;
    editBtn: boolean;

    dataKodeShift: any;

    $scope: angular.IScope;
    $rootScope: angular.IRootScopeService;

    Data: string;
    DataSide: string;

    //ini paging
    viewby = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    //pageSize = 5; 
    pageSizes = [5, 10, 15, 20, 25];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;
    orderString = 'shiftCode';
    orderState = false;

    shiftCodeMessage: string;

    KodeShift: Service.KodeShiftService;

    shiftCode: string;
    description: string;
    deleteId: string;
    deleteDescription: string;

    jsonKodeShift = {
        "Kode Shift": null,
        "Keterangan": null
    }

    //------ NEW
    searchTable = {};


    constructor(kodeShift: Service.KodeShiftService, _$scope: angular.IScope, _$rootScope: angular.IRootScopeService) {
        this.KodeShift = kodeShift;
        this.$scope = _$scope;
        this.$rootScope = _$rootScope;
        this.editBtn = false;
    }

    $onInit() {
        this.refreshData();
        this.Created = true;
    }
    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    selectData(data) {
        this.shiftCode = data.shiftCode;
        this.description = data.description;
        this.Edited = true;
        this.Created = false;
        this.editBtn = true;
    }

    order(name) {
        this.orderString = name;
        this.orderState = !this.orderState;
    }

    addData(frmdata: angular.IFormController) {
        this.jsonKodeShift["Kode Shift"] = this.shiftCode;
        this.jsonKodeShift["Keterangan"] = this.description;
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonKodeShift)),
            () => {
                this.Data = null;
                this.loading = true;
                this.KodeShift.PostData(this.shiftCode, this.description).then(response => {
                    alertify.success("Data berhasil disimpan");
                    this.refreshData(frmdata);
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if (response.status == "400") {
                        alertify.error(response.data);
                        this.refreshDataWithoutClear(frmdata);
                    }
                }).finally(() => {
                    this.setPage(this.currentPage);
                    if (frmdata) {
                        frmdata.$setPristine();
                    }
                    this.loading = false;
                    this.searchTable = {};
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    updateData(frmdata: angular.IFormController) {
        this.jsonKodeShift["Kode Shift"] = this.shiftCode;
        this.jsonKodeShift["Keterangan"] = this.description;

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonKodeShift)),
            () => {
                this.Data = null;
                this.KodeShift.UpdateData(this.shiftCode, this.description).then(response => {
                    this.searchTable = {};
                    alertify.success("Data berhasil disimpan");
                    this.Created = true;
                    this.editBtn = false;
                    this.Edited = false;
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if (response.status == "400") {
                        alertify.error(response.data);
                    }
                }).finally(() => {
                    this.setPage(this.currentPage);
                    this.refreshData(frmdata);
                    if (frmdata) {
                        frmdata.$setPristine();
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    deleteData(data, frmdata: angular.IFormController) {
        this.deleteId = data.shiftCode;
        this.deleteDescription = data.description;
        this.jsonKodeShift["Kode Shift"] = this.deleteId;
        this.jsonKodeShift["Keterangan"] = this.deleteDescription;

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonKodeShift)),
            () => {
                this.Data = null;
                this.KodeShift.DeleteData(this.deleteId).then(response => {
                    this.searchTable = {};
                    alertify.success("Data berhasil dihapus");
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if(response.status == "400") {
                        alertify.error(response.data);
                    }
                }).finally(() => {
                    this.setPage(this.currentPage);
                    this.refreshData(frmdata);
                    if (frmdata) {
                        frmdata.$setPristine();
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    refreshData(frmdata?: angular.IFormController) {

        this.loading = true;
        this.shiftCodeMessage = undefined;
        this.KodeShift.GetData().then(response => {
            this.loading = false;
            this.Data = response.data;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;

            this.shiftCode = null;
            this.description = null;
        }).catch(response => {
                if (response.status == "500") {
                    alertify.error("Koneksi ke server bermasalah");
                }
            }).finally(() => {
                this.loading = false;
                this.searchTable = {};
            });
    }

    refreshDataWithoutClear(frmdata?: angular.IFormController) {
        this.loading = true;
        this.KodeShift.GetData().then(response => {
            this.loading = false;
            this.Data = response.data;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            } 
            }).finally(() => {
                this.loading = false;
                this.searchTable = {};
            });
    }

    clearForm(frmdata: angular.IFormController) {
        this.shiftCode = '';
        this.description = '';
        this.Edited = false;
        this.Created = true;
        this.editBtn = false;
        if (frmdata) {
            frmdata.$setPristine();
        }
        this.searchTable = {};
    }

    alertifyData() {
        var data = {
            shiftCode: this.shiftCode,
            description: this.description
        };
        return data;
    }

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
        angular.forEach(json, (value, key) => {
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
}

export class KodeShiftComponent implements angular.IComponentOptions {
    controller = KodeShiftController;
    controllerAs = 'me';

    template = require('./KodeShift.html');
    bindings = {
        greet: '@',
        //ftoken: '@'
    };
}