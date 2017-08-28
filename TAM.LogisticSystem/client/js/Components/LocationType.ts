import * as angular from 'angular';
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as _ from 'lodash';
class LocationTypeController implements angular.IController {
    static $inject = ['LocationTypeService', '$rootScope'];

    isEdit: boolean;
    //Alertify JSON
    jsonLocationType = {};

    //validasi regex
    regexStringCode: RegExp = /^[A-Za-z0-9]+$/;
    regexStringName: RegExp = /^[a-zA-Z0-9\s\-.&,\'/]+$/;

    locationType: Service.LocationTypeService;
    locationTypeViewModelList: Service.LocationTypeModel[];
    locationTypeViewModel: Service.LocationTypeModel;
    locationTypeModelConfirmation: Service.LocationTypeModelConfirmation;
    constructor(locationType: Service.LocationTypeService, root: angular.IRootScopeService) {
        this.locationType = locationType;
        this.isEdit = false;
        this.$rootScope = root;
    }
    $onInit() {
        this.refreshData();
        this.$rootScope.$on('Kembali', (event) => {
            this.pageState = true;
            this.refreshData();
        });
    }
    //method to get all location type data
    refreshData() {
        this.locationTypeViewModel = new Service.LocationTypeModel();
        this.locationType.getAllLocationType().then(response => {
            this.locationTypeViewModelList = response.data as Service.LocationTypeModel[];
            this.totalItems = this.locationTypeViewModelList.length;
            angular.forEach(this.locationTypeViewModelList, item => {
                item.tanggungJawab = (item.hasResponsibility.toString() === 'true') ? 'Ya' : 'Tidak';
                item.sjkb = (item.needSjkbTarikan.toString() === 'true') ? 'Ya' : 'Tidak';
            });

        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }
    //method create location type
    createLocationType(form) {
        this.confirmationData(this.locationTypeViewModel);
        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonLocationType)),
            () => {
                if (_.find(this.locationTypeViewModelList, ['locationTypeCode', this.locationTypeViewModel.locationTypeCode]) === undefined) {
                    this.locationType.postLocationType(this.locationTypeViewModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.reset(form);
                        this.refreshData();

                    }).catch(response => {
                        if (response.data === 'EXIST') {
                            alertify.error('Kode Tipe Lokasi telah terdaftar');
                        }
                        if (response.data === 'INVALID') {
                            alertify.error('Data tidak valid');
                        }
                        if (response.status == "500") {
                            alertify.error("Koneksi ke server bermasalah");
                        }
                        else {
                            alertify.error('Data gagal disimpan');
                        }

                    });
                }
                else {
                    alertify.error('Kode Tipe Lokasi telah terdaftar');
                }
            },
            () => {
                //alertify.error('Batal');
            }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }
    //masukin data ke model di form
    updateSelected(locationTypeModel: Service.LocationTypeModel) {
        this.locationTypeViewModel = new Service.LocationTypeModel();
        this.locationTypeViewModel.locationTypeCode = locationTypeModel.locationTypeCode;
        this.locationTypeViewModel.name = locationTypeModel.name;
        this.locationTypeViewModel.hasResponsibility = locationTypeModel.hasResponsibility;
        this.locationTypeViewModel.needSjkbTarikan = locationTypeModel.needSjkbTarikan;
        this.isEdit = true;
    }
    //method update location type
    updateLocationType(form) {
        this.confirmationData(this.locationTypeViewModel);

        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonLocationType)),
            () => {
                this.locationType.updateLocationType(this.locationTypeViewModel).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.refreshData();
                    this.reset(form);
                    this.isEdit = false;

                }).catch(response => {
                    if (response.status == "400") {
                        alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    }
                    else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => {
                //alertify.error('Batal');
            }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }
    //method delete location type
    deleteLocationType(locationTypeModel: Service.LocationTypeModel, form) {
        this.confirmationData(locationTypeModel);
        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonLocationType)),
            () => {
                this.locationType.deleteLocation(locationTypeModel.locationTypeCode).then(response => {
                    this.refreshData();
                    this.isEdit = false;
                    this.reset(form);
                    alertify.success('Data berhasil dihapus');
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    }
                    else {
                        alertify.error('Data gagal dihapus');
                    }
                });
            },
            () => {
                //alertify.error('Batal');
            }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    /**
     * fungsi untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
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
    //data yg dilempar ke alertify
    confirmationData(locationTypeModel: Service.LocationTypeModel) {
        this.locationTypeModelConfirmation = new Service.LocationTypeModelConfirmation();
        this.locationTypeModelConfirmation.kodeLokasi = locationTypeModel.locationTypeCode;
        this.locationTypeModelConfirmation.tipeLokasi = locationTypeModel.name;
        this.locationTypeModelConfirmation.tanggungJawab = (locationTypeModel.hasResponsibility.toString() === 'true') ? 'Ya' : 'Tidak';
        this.locationTypeModelConfirmation.sjkb = (locationTypeModel.needSjkbTarikan.toString() === 'true') ? 'Ya' : 'Tidak';

        this.jsonLocationType["Kode Tipe Lokasi"] = this.locationTypeModelConfirmation.kodeLokasi;
        this.jsonLocationType["Tipe Lokasi"] = this.locationTypeModelConfirmation.tipeLokasi;
        this.jsonLocationType["Memiliki Tanggung Jawab"] = this.locationTypeModelConfirmation.tanggungJawab;
        this.jsonLocationType["SJKB Tarikan"] = this.locationTypeModelConfirmation.sjkb;
    }
    //method untuk membersihkan form
    reset(form: angular.IFormController) {
        form.$setUntouched();
        form.$setPristine();
        this.locationTypeViewModel = new Service.LocationTypeModel();
        this.locationTypeViewModel.locationTypeCode = null;
        this.locationTypeViewModel.name = null;
        this.isEdit = false;
        this.searchFilter = {};
    }
    //download upload
    //upload download Romy
    pageState: boolean = true;
    $rootScope: angular.IRootScopeService;
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.locationTypeCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "LocationType";
        info.title = "Master Tipe Lokasi";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }
    upload() {
        let info: any = {};
        info.master = "LocationType";
        info.title = "Master Tipe Lokasi";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchFilter = {};
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

let LocationType = {
    controller: LocationTypeController,
    controllerAs: 'me',

    template: require('./LocationType.html'),

}
export { LocationType }