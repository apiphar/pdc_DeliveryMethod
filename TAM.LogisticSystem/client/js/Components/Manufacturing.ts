import * as angular from 'angular';
import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as mustache from 'mustache';

class ManufacturingController implements angular.IController {
    static $inject = ["ManufacturingService", "$rootScope"];



    //private readonly style
    manufacturingService: service.ManufacturingService;
    rootScope: angular.IRootScopeService;

    //Data Types
    manufacturingViewModel: service.ManufacturingViewModel[];
    postManufacturingModel: service.ManufacturingViewModel;
    saveMode: string;
    jsonModel = {};//create object
    postNameCheck: boolean;
    postPlantCodeCheck: boolean;
    postCountryCheck: boolean;
    pageState: boolean = true;
    isLoading: boolean = true;
    searchString = {};

    constructor(ManufacturingService: service.ManufacturingService, rs: angular.IRootScopeService) {
        this.manufacturingService = ManufacturingService;
        this.rootScope = rs;
    }

    $onInit() {
        this.manufacturingService.getAll().then(response => {
            this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
            this.totalItems = this.manufacturingViewModel.length;
            
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            } 
            }).finally(() => {
                this.isLoading = false;
            });

        this.rootScope.$on('Kembali', (event) => {
            this.pageState = true;
            this.manufacturingService.getAll().then(response => {
                this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                this.totalItems = this.manufacturingViewModel.length;
            });
        });
        this.saveMode = 'Simpan';
    }

    //make bind the selected row to the update form
    bindUpdate(data) {
        this.postManufacturingModel = new service.ManufacturingViewModel();
        this.postManufacturingModel.plantCode = data.plantCode;
        this.postManufacturingModel.country = data.country;
        this.postManufacturingModel.name = data.name;
        this.saveMode = 'Update';
    }

    //update the data
    update(form: angular.IFormController) {
        this.jsonModel["Plant Code"] = this.postManufacturingModel.plantCode;
        this.jsonModel["Manufacturing"] = this.postManufacturingModel.name;
        this.jsonModel["Country"] = this.postManufacturingModel.country;
        
        alertify.confirm("Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonModel)),
            Q => {
                this.manufacturingViewModel = null;
                this.isLoading = true;
                this.manufacturingService.updateData(this.postManufacturingModel).then(response => {
                    this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                    this.totalItems = this.manufacturingViewModel.length;
                    this.reset(form);
                    this.saveMode = 'Simpan';
                    alertify.success("Data berhasil disimpan");
                    this.searchString = {};
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if (response.status == "400") {
                        alertify.error(response.data);
                    }
                    this.manufacturingService.getAll().then(response => {
                        this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                        this.totalItems = this.manufacturingViewModel.length;
                        this.isLoading = false;
                    });
                });
            },
            function () {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //cancel the state, clear the form
    cancel(form: angular.IFormController) {
        if (this.saveMode === 'Simpan') {
            //reset form
            this.postManufacturingModel = new service.ManufacturingViewModel();
            form.$setPristine();
            form.$setUntouched();
        }
        if (this.saveMode === 'Update') {
            this.postManufacturingModel = new service.ManufacturingViewModel();
            this.saveMode = "Simpan";
            form.$setPristine();
            form.$setUntouched();
        }
        this.searchString = {};
        this.postManufacturingModel.country = null;
        this.postManufacturingModel.name = null;
        this.postManufacturingModel.plantCode = null;
    }

    //insert the data
    save(form: angular.IFormController) {   
        this.jsonModel["Plant Code"] = this.postManufacturingModel.plantCode;
        this.jsonModel["Manufacturing"] = this.postManufacturingModel.name;
        this.jsonModel["Country"] = this.postManufacturingModel.country;

        alertify.confirm("Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonModel)),
            Q => {
                this.manufacturingViewModel = null;
                this.isLoading = true;
                this.manufacturingService.save(this.postManufacturingModel).then(response => {
                    alertify.success("Data berhasil disimpan");
                    this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                    this.totalItems = this.manufacturingViewModel.length;
                    this.reset(form);
                    this.searchString = {};
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if (response.status == "400") {
                        alertify.error(response.data);
                    }
                    this.manufacturingService.getAll().then(response => {
                        this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                        this.totalItems = this.manufacturingViewModel.length;
                        this.isLoading = false;
                    });
                });
            },
            function () {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //delete data
    deleteData(data, form: angular.IFormController) {
        this.jsonModel["Plant Code"] = data.plantCode;
        this.jsonModel["Manufacturing"] = data.name;
        this.jsonModel["Country"] = data.country;

        alertify.confirm("Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonModel)),
            Q => {
                this.manufacturingViewModel = null;
                this.isLoading = true;
                this.manufacturingService.delete(data).then(response => {
                    alertify.success('Data berhasil dihapus');
                    this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                    this.totalItems = this.manufacturingViewModel.length;
                    this.isLoading = false;
                    this.reset(form);
                    this.saveMode = "Simpan";
                    this.searchString = {};
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if (response.status == "400") {
                        alertify.error(response.data);
                    }
                    this.manufacturingService.getAll().then(response => {
                        this.manufacturingViewModel = response.data as service.ManufacturingViewModel[];
                        this.totalItems = this.manufacturingViewModel.length;
                        this.isLoading = false;
                    });
                });
            },
            function () {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    reset(Form: angular.IFormController) {
        this.isLoading = false;
        Form.$setPristine();
        Form.$setUntouched();
        this.postManufacturingModel = new service.ManufacturingViewModel();
        this.postManufacturingModel.plantCode = null;
        this.postManufacturingModel.country = null;
        this.postManufacturingModel.name = null;
    }
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.plantCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Plant";
        info.title = "Manufacturing";
        info.tipe = "3";
        this.rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "Plant";
        info.title = "Manufacturing";
        info.tipe = "2";
        this.pageState = false;
        this.rootScope.$emit("UploadDownload", null, info);
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

    //PAGING
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
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

}


export class Manufacturing implements angular.IComponentOptions {
    controller = ManufacturingController;
    controllerAs = 'me';
    template = require('./Manufacturing.html');
}