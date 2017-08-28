import * as angular from 'angular';﻿
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as _ from 'lodash';
class MasterWarnaVehicleController implements angular.IController {
    static $inject = ['MasterWarnaVehicleService', '$rootScope'];

    isEdit: boolean;
    $rootScope: angular.IRootScopeService;

    vehicleColor: Service.MasterWarnaVehicleService;
    vehicleColorViewModel: Service.MasterWarnaVehicleViewModel;
    vehicleColorValidateModel: Service.MasterWarnaVehicleViewModel;
    vehicleColorViewModelList: Service.MasterWarnaVehicleViewModel[];
    vehicleColorCreateModel: Service.MasterWarnaVehicleCreateModel;
    vehicleColorModel: Service.MasterWarnaVehicleColorModel;
    kodeBrandList: Service.MasterWarnaVehicleBrandModel[];
    defaultValueBrand: Service.MasterWarnaVehicleBrandModel;
    kodeModel: Service.MasterWarnaVehicleModelModel[];
    kodeModelList: Service.MasterWarnaVehicleModelModel[];
    kodeWarna: Service.MasterWarnaVehicleColorModel;
    kodeWarnaList: Service.MasterWarnaVehicleColorModel[];
    jsonVehicleColor: {};
    loading: boolean = false;
    validateKodeWarnaVehicle: boolean = true;
    validateKodeWarnaVehicleDetail: boolean = true;
    validateKodeBrand: boolean = true;
    validateKodeModel: boolean = true;
    validateKodeWarna: boolean = true;
    validate: boolean = true;
    errorMsgKodeWarnaVehicle: string = "";
    pageState: boolean = true;

    constructor(vehicleColor: Service.MasterWarnaVehicleService, root: angular.IRootScopeService) {
        this.vehicleColor = vehicleColor;
        this.$rootScope = root;
        this.isEdit = false;
    }
    $onInit() {
        this.jsonVehicleColor = {};
        this.vehicleColorCreateModel = new Service.MasterWarnaVehicleCreateModel();
        this.defaultValueBrand = new Service.MasterWarnaVehicleBrandModel();
        this.refreshData();
        this.getAllKodeBrand();
        this.getAllKodeModel();
        this.getAllKodeWarna();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
    }
    //Get All Vehicle Color Data
    refreshData() {
        this.vehicleColorViewModel = new Service.MasterWarnaVehicleViewModel();
        this.vehicleColor.getAllVehicleColor().then(response => {
            this.vehicleColorViewModelList = response.data;
            this.totalItems = this.vehicleColorViewModelList.length;
        });
    }

    //Get Model by BrandCode
    getKodeModel() {
        this.kodeModel = _.filter(this.kodeModelList, {
            'kodeBrand': this.vehicleColorCreateModel.brand.kodeBrand
        });

        if (this.vehicleColorCreateModel.model !== undefined) {
            this.vehicleColorCreateModel.model.kodeModel = undefined;
            this.validationKodeModel();
        }
        
    }

    //Get Color Description by ColorCode
    getWarna() {
        this.kodeWarna = _.find(this.kodeWarnaList, {
            'kodeWarna': this.vehicleColorCreateModel.warna.kodeWarna
        });
    }

    //Get All Brand
    getAllKodeBrand() {
        this.vehicleColor.getAllKodeBrand().then(response => {
            this.kodeBrandList = response.data;
        });
    }

    //Get All Model
    getAllKodeModel() {
        this.vehicleColor.getAllKodeModel().then(response => {
            this.kodeModelList = response.data;
        });
    }

    //Get All Color
    getAllKodeWarna() {
        this.vehicleColor.getAllKodeWarna().then(response => {
            this.kodeWarnaList = response.data;
        });
    }

    //Create Vehicle Color
    createVehicleColor(form: angular.IFormController) {
        this.jsonVehicleColor["Kode Warna Vehicle"] = this.vehicleColorCreateModel.kodeWarnaVehicle;
        this.jsonVehicleColor["Kode Brand"] = this.vehicleColorCreateModel.brand.kodeBrand;
        this.jsonVehicleColor["Kode Model"] = this.vehicleColorCreateModel.model.kodeModel;
        this.jsonVehicleColor["Kode Warna"] = this.vehicleColorCreateModel.warna.kodeWarna;
        this.jsonVehicleColor["Deskripsi Warna (Ind)"] = this.vehicleColorCreateModel.warna.deskripsiWarnaInd;
        this.jsonVehicleColor["Deskripsi Warna (Eng)"] = this.vehicleColorCreateModel.warna.deskripsiWarnaEng;
        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonVehicleColor)),
            () => {
                this.loading = true;
                if (this.vehicleColorCreateModel.brand.kodeBrand === undefined) {
                    alertify.error('Kode Brand harus dipilih');
                } else if (this.vehicleColorCreateModel.model.kodeModel === undefined) {
                    alertify.error('Kode Model harus dipilih');
                } else if (this.vehicleColorCreateModel.warna.kodeWarna === undefined) {
                    alertify.error('Kode Warna harus dipilih');
                } else {
                    this.vehicleColor.postVehicleColor(this.vehicleColorCreateModel).then(response => {
                        if (response.data === 'FALSE') {
                            alertify.error('Data gagal disimpan');
                        }
                        else {
                            alertify.success('Data berhasil disimpan');
                            this.refreshData();
                            this.reset(form);
                        }
                    });
                }
                this.loading = false;

            },
            () => {

            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    //Change Panel Insert to selected data
    updateSelected(vehicleColorModel: Service.MasterWarnaVehicleViewModel) {
        this.vehicleColorCreateModel = new Service.MasterWarnaVehicleCreateModel();
        this.vehicleColorCreateModel.brand = new Service.MasterWarnaVehicleBrandModel();
        this.vehicleColorCreateModel.model = new Service.MasterWarnaVehicleModelModel();
        this.vehicleColorCreateModel.warna = new Service.MasterWarnaVehicleColorModel();
        this.vehicleColorCreateModel.kodeWarnaVehicle = vehicleColorModel.kodeWarnaVehicle;
        this.vehicleColorCreateModel.brand.kodeBrand = vehicleColorModel.kodeBrand;
        this.getKodeModel();
        this.vehicleColorCreateModel.model.kodeModel = vehicleColorModel.kodeModel;
        this.vehicleColorCreateModel.warna.kodeWarna = vehicleColorModel.kodeWarna;
        this.vehicleColorCreateModel.warna.deskripsiWarnaInd = vehicleColorModel.deskripsiWarnaInd;
        this.vehicleColorCreateModel.warna.deskripsiWarnaEng = vehicleColorModel.deskripsiWarnaEng;
        this.validateKodeWarnaVehicle = false;
        this.validateKodeBrand = false;
        this.validateKodeModel = false;
        this.validateKodeWarna = false;
        this.validation();
        this.isEdit = true;
    }

    //Update Vehicle Color by KodeWarnaVehicle
    updateVehicleColor(form: angular.IFormController) {
        this.jsonVehicleColor["Kode Warna Vehicle"] = this.vehicleColorCreateModel.kodeWarnaVehicle;
        this.jsonVehicleColor["Kode Brand"] = this.vehicleColorCreateModel.brand.kodeBrand;
        this.jsonVehicleColor["Kode Model"] = this.vehicleColorCreateModel.model.kodeModel;
        this.jsonVehicleColor["Kode Warna"] = this.vehicleColorCreateModel.warna.kodeWarna;
        this.jsonVehicleColor["Deskripsi Warna (Ind)"] = this.vehicleColorCreateModel.warna.deskripsiWarnaInd;
        this.jsonVehicleColor["Deskripsi Warna (Eng)"] = this.vehicleColorCreateModel.warna.deskripsiWarnaEng;
        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonVehicleColor)),
            () => {
                this.loading = true;
                this.vehicleColor.updateVehicleColor(this.vehicleColorCreateModel).then(response => {
                    if (response.data === 'FALSE') {
                        alertify.error('Data gagal tersimpan');
                    }
                    else {
                        alertify.success('Data berhasil disimpan');
                        this.refreshData();
                        this.reset(form);
                        this.isEdit = false;
                    }
                    this.loading = false;
                });
            },
            () => {

            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    //Delete Vehicle Color by KodeWarnaVehicle
    deleteVehicleColor(vehicleColor: Service.MasterWarnaVehicleViewModel, form: angular.IFormController) {
        this.jsonVehicleColor["Kode Warna Vehicle"] = vehicleColor.kodeWarnaVehicle;
        this.jsonVehicleColor["Kode Brand"] = vehicleColor.kodeBrand;
        this.jsonVehicleColor["Kode Model"] = vehicleColor.kodeModel;
        this.jsonVehicleColor["Kode Warna"] = vehicleColor.kodeWarna;
        this.jsonVehicleColor["Deskripsi Warna (Ind)"] = vehicleColor.deskripsiWarnaInd;
        this.jsonVehicleColor["Deskripsi Warna (Eng)"] = vehicleColor.deskripsiWarnaEng;
        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonVehicleColor)),
            () => {
                this.loading = true;
                this.vehicleColor.deleteVehicleColor(vehicleColor.kodeWarnaVehicle).then(response => {
                    this.refreshData();
                    alertify.success('Data berhasil dihapus');
                    this.loading = false;
                    this.reset(form);
                }).catch(() => {
                    alertify.error('Data gagal dihapus');
                    this.loading = false;
                });

            },
            () => {

            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });


    }

    validation() {
        console.log(this.validateKodeWarnaVehicle, this.validateKodeBrand, this.validateKodeModel, this.validateKodeWarna);
        if (this.validateKodeWarnaVehicle === false && this.validateKodeBrand === false && this.validateKodeModel === false
            && this.validateKodeWarna === false) {
            this.validate = false;
        } else {
            this.validate = true;
        }
    }

    validationKodeWarnaVehicle() {

        this.vehicleColorValidateModel = _.find(this.vehicleColorViewModelList, {
            'kodeWarnaVehicle': this.vehicleColorCreateModel.kodeWarnaVehicle
        });

        if (this.vehicleColorCreateModel.kodeWarnaVehicle === undefined || !this.vehicleColorCreateModel.kodeWarnaVehicle.match(/^[\w]+$/)) {
            this.validateKodeWarnaVehicle = true;
            this.validateKodeWarnaVehicleDetail = false;
        }else if (this.vehicleColorValidateModel !== undefined) {
            this.validateKodeWarnaVehicle = true;
            this.validateKodeWarnaVehicleDetail = true;
            this.errorMsgKodeWarnaVehicle = "Kode Warna Vehicle sudah pernah disimpan";
        } else {
            this.validateKodeWarnaVehicle = false;
            this.validateKodeWarnaVehicleDetail = false;
        }

        this.validation();
    }

    validationKodeBrand() {
        
        if (this.vehicleColorCreateModel.brand.kodeBrand === undefined) {
            this.validateKodeBrand = true;
        } else {
            this.validateKodeBrand = false;
        }

        this.validation();
    }

    validationKodeModel() {
        if (this.vehicleColorCreateModel.model.kodeModel === undefined) {
            this.validateKodeModel = true;
        } else {
            this.validateKodeModel = false;
        }

        this.validation();
    }

    validationKodeWarna() {
        if (this.vehicleColorCreateModel.warna.kodeWarna === undefined) {
            this.validateKodeWarna = true;
        } else {
            this.validateKodeWarna = false;
        }

        this.validation();
    }

    //Reet Form
    reset(form: angular.IFormController) {
        form.$setUntouched();
        form.$setPristine();
        this.vehicleColorCreateModel = new Service.MasterWarnaVehicleCreateModel();
        this.kodeModel = null;
        this.refreshData();
        this.isEdit = false;
        this.validate = true;
        this.validateKodeWarnaVehicle = true;
        this.validateKodeWarnaVehicleDetail = false;
        this.validateKodeBrand = true;
        this.validateKodeModel = true;
        this.validateKodeBrand = true;
        this.validateKodeWarna = true;
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

    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin data ingin disimpan?";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin data ingin disimpan?";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin data ingin dihapus?";
        convertResult["grid"] = tempJson;
        return convertResult;
    }

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.kodeWarnaVehicle);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "CarTypeColor";
        info.title = "Warna Vehicle";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "CarTypeColor";
        info.title = "Warna Vehicle";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

}

let MasterVehicleColor = {
    controller: MasterWarnaVehicleController,
    controllerAs: 'me',

    template: require('./MasterWarnaVehicle.html'),
    bindings: {
        greet: '@'
    }
}
export { MasterVehicleColor }
