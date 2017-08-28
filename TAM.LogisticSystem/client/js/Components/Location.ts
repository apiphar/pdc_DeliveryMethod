import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as angular from 'angular';


export class LocationController implements angular.IController {
    static $inject = ['LocationService', '$rootScope'];

    location: Service.LocationService;
    isUpdate: number;
    isError: number;

    validationFlag: boolean = true;
    searchFilter: {};

    data: Service.LocationViewModel[];
    locTypeData: string;
    cityForLegData: string;
    cityForShipmentData: string;

    locationType: [{
        locationTypeCode: any,
        name: any
    }];

    cityForLeg: [{
        cityForLegCode: any,
        name: any
    }];

    cityForShipment: [{
        cityForShipmentCode: any,
        name: any
    }];

    locationCode: any;
    tempLocation: Service.LocationViewModel;
    locationName: any;
    alamat: any;
    cetakSJKB: boolean;
    tempLocId: any;

    tempLocationType: [{
        locationTypeCode: any,
        name: any
    }];
    tempCityForLeg: [{
        cityForLegCode: any,
        name: any
    }];

    tempCityForShipment: [{
        cityForShipmentCode: any,
        name: any
    }];
    tempLocationCode: any;
    tempLocationName: any;
    tempAlamat: any;
    tempCetakSJKB: boolean;
    constructor(LocationService: Service.LocationService, rootScope: angular.IRootScopeService) {

        this.$rootScope = rootScope;
        this.location = LocationService;
        this.locationType = null;
        this.cityForLeg = null;
        this.cityForShipment = null;
    }

    //refresh data location
    refreshData(Form?: angular.IFormController) {
        this.location.getAllData().then(response => {
            this.data = response.data.location;
            this.locTypeData = response.data.locationType;
            this.cityForLegData = response.data.cityForLeg;
            this.cityForShipmentData = response.data.cityForShipment;

            angular.forEach(this.data, item => {
                item.cityForLegView = item.cityForLegCode + " - " + item.cityForLegName;
                item.cityForShipmentView = item.cityForShipmentCode + " - " + item.cityForShipmentName;
                item.locationTypeView = item.locationTypeCode + " - " + item.locationTypeName;
            });

            this.totalItems = this.data.length;
        }).catch(response => {
            if (response.status == "500") {
                Alertify.error('Koneksi ke server bermasalah');
            }
        });

        if (Form) {
            Form.$setPristine();
            Form.$setUntouched();
        }
        this.reset();
    }

    //validasi duplicate data
    validationKodeLokasi() {

        if (this.locationCode === null || this.locationCode === undefined) {
            this.validationFlag = true;
        } else if (!this.locationCode.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validationFlag = true;
        } else {
            this.validationFlag = true;
        }
    }

    reset() {
        this.isUpdate = 0;
        this.searchFilter = {};
        this.locationCode = null;
        this.locationName = null;
        this.alamat = null;
        this.locationType = null;
        this.cityForLeg = null;
        this.cityForShipment = null;
        this.cetakSJKB = false;
    }

    $onInit() {
        this.tempLocation = new Service.LocationViewModel();
        this.refreshData();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
    }

    //proses setelah memilih tombol edit
    selectEdit(data) {
        this.isUpdate = 1;
        this.getAllSelectedData(data);
    }

    //proses setelah memilih tombol delete
    selectDelete(data, Form: angular.IFormController) {
        this.getAllSelectedDeleteData(data);
        this.deleteConfirmation('delete', Form);
    }

    //get data yang dipilih (edit)
    getAllSelectedData(data) {
        this.locationType = [{
            locationTypeCode: "",
            name: ""
        }];

        this.cityForLeg = [{
            cityForLegCode: "",
            name: ""
        }];

        this.cityForShipment = [{
            cityForShipmentCode: "",
            name: ""
        }];

        this.locationCode = data.locationCode;
        this.locationName = data.locationName;
        this.alamat = data.alamat;
        this.locationType["locationTypeCode"] = data.locationTypeCode;
        this.locationType["name"] = data.locationTypeName;
        this.cityForLeg["cityForLegCode"] = data.cityForLegCode;
        this.cityForLeg["name"] = data.cityForLegName;
        this.cityForShipment["cityForShipmentCode"] = data.cityForShipmentCode;
        this.cityForShipment["name"] = data.cityForShipmentName;
        this.cetakSJKB = data.cetakSJKB;

    }
    //get data yang akan ditampilkan ke alertify
    confirmationData(status: string) {
        let data = {};
        data["Kode Lokasi"] = this.locationCode;
        data["Lokasi"] = this.locationName;
        data["Alamat"] = this.alamat;
        data["Kode Tipe Lokasi"] = this.locationType["locationTypeCode"] + " - " + this.locationType["name"];
        data["Kode City"] = this.cityForLeg["cityForLegCode"] + " - " + this.cityForLeg["name"];
        data["Kode City Location"] = this.cityForShipment["cityForShipmentCode"] + " - " + this.cityForShipment["name"];

        if (this.cetakSJKB === true) {
            data["Cetak SJKB"] = "Ya";
        } else {
            data["Cetak SJKB"] = "Tidak";
        }

        if (status == "delete") {
            this.tempLocId = this.locationCode;
            this.reset();
        }
        return data;
    }

    //get data yang mau di delete
    getAllSelectedDeleteData(data) {
        this.tempLocationType = [{
            locationTypeCode: "",
            name: ""
        }];

        this.tempCityForLeg = [{
            cityForLegCode: "",
            name: ""
        }];

        this.tempCityForShipment = [{
            cityForShipmentCode: "",
            name: ""
        }];

        this.tempLocationCode = data.locationCode;
        this.tempLocationName = data.locationName;
        this.tempAlamat = data.alamat;
        this.tempLocationType["locationTypeCode"] = data.locationTypeCode;
        this.tempLocationType["name"] = data.locationTypeName;
        this.tempCityForLeg["cityForLegCode"] = data.cityForLegCode;
        this.tempCityForLeg["name"] = data.cityForLegName;
        this.tempCityForShipment["cityForShipmentCode"] = data.cityForShipmentCode;
        this.tempCityForShipment["name"] = data.cityForShipmentName;
    }

    //get data yang mau di delete (ditampilkan di alertify)
    confirmationDeleteData(status: string) {
        let data = {};
        data["Kode Lokasi"] = this.tempLocationCode;
        data["Lokasi"] = this.tempLocationName;
        data["Alamat"] = this.tempAlamat;
        data["Kode Tipe Lokasi"] = this.tempLocationType["locationTypeCode"] + " - " + this.tempLocationType["name"];
        data["Kode City"] = this.tempCityForLeg["cityForLegCode"] + " - " + this.tempCityForLeg["name"];
        data["Kode City Location"] = this.tempCityForShipment["cityForShipmentCode"] + " - " + this.tempCityForShipment["name"];

        if (this.cetakSJKB === true) {
            data["Cetak SJKB"] = "Ya";
        } else {
            data["Cetak SJKB"] = "Tidak";
        }

        if (status == "delete") {
            this.tempLocId = this.tempLocationCode;
        }
        return data;
    }

    //Alertify untuk insert, update, delete
    confirmation(type: string, Form: angular.IFormController) {
        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON(type, this.confirmationData(type))),
            () => {
                if (type === 'insert') {
                    this.createData(Form);
                }
                else if (type === 'update') {
                    this.updateData(Form);
                }
                else if (type === 'delete') {
                    this.deleteData(Form);
                }
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //Alertify untuk delete
    deleteConfirmation(type: string, Form: angular.IFormController) {
        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON(type, this.confirmationDeleteData(type))),
            () => {
                this.deleteData(Form);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //Insert
    createData(Form: angular.IFormController) {
        this.location.postData(this.locationCode, this.locationName, this.alamat, this.locationType["locationTypeCode"], this.cityForLeg["cityForLegCode"], this.cityForShipment["cityForShipmentCode"], this.cetakSJKB).then(response => {
            this.refreshData(Form);
            Alertify.success('Data berhasil disimpan');
        }).catch(response => {
            this.isError = response.data;
            if (this.isError == 1) {
                Alertify.error('Data tidak valid');
            } else if (this.isError == 2) {
                Alertify.error('Kode Lokasi telah terdaftar');
            } else if (response.status == "500") {
                Alertify.error('Koneksi ke server bermasalah');
            } else {
                Alertify.error('Data gagal disimpan');
            }
        });
    }

    //Update
    updateData(Form: angular.IFormController) {

        this.location.updateData(this.locationCode, this.locationName, this.alamat, this.locationType["locationTypeCode"], this.cityForLeg["cityForLegCode"], this.cityForShipment["cityForShipmentCode"], this.cetakSJKB).then(response => {
            this.refreshData(Form);
            Alertify.success('Data berhasil disimpan');
        }).catch(response => {
            this.isError = response.data;
            if (this.isError == 1) {
                Alertify.error('Data tidak valid');
            } else if (response.status == "500") {
                Alertify.error('Koneksi ke server bermasalah');
            } else {
                Alertify.error('Data gagal disimpan');
            }

        });

    }

    //Delete
    deleteData(Form: angular.IFormController) {
        this.location.deleteData(this.tempLocId).then(response => {
            if (this.isError === 1) {
                Alertify.error('Data gagal dihapus');
            } else {
                this.refreshData(Form);
                Alertify.success('Data berhasil dihapus');
            }
        }).catch(response => {
            if (response.status == "500") {
                Alertify.error('Koneksi ke server bermasalah');
            } else {
                Alertify.error('Data gagal dihapus');
            }
        });
    }


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

    Download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.locationCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Location";
        info.title = "Lokasi";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }
    Upload() {

        let info: any = {};
        info.master = "Location";
        info.title = "Lokasi";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }




    // Paging Sorting Searching upload downlod
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;
    $rootScope: angular.IRootScopeService;
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
export class LocationComponent implements angular.IComponentOptions {
    controller = LocationController;
    controllerAs = 'me';

    template = require('./Location.html');
}