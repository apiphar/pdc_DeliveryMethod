import * as angular from 'angular';
import * as services from '../services';
import * as Alertify from 'alertifyjs';
import * as mustache from 'mustache';

export class CityMasterController implements angular.IController {
    static $inject = ['CityMasterService', '$rootScope'];

    constructor(cityMasterService: services.CityMasterService, root: angular.IRootScopeService) {
        this.cityMasterService = cityMasterService;
        this.root = root;
    }

    cityMasterService: services.CityMasterService;
    root: angular.IRootScopeService;
    cityMasterViewModel: services.CityMasterViewModel[];
    singleCityMasterViewModel: services.CityMasterViewModel;
    cityCode: string;
    name: string;
    editCheck: boolean;
    pageState: boolean = true;
    jsonData = {};
    searchData = {};
    regexCode: string = '^[A-Za-z0-9]+$';
    regexName: string = '^[a-zA-Z0-9\\s-.&,\'/]+$';


    $onInit() {
        this.refreshData();
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
        });
    }

    //to get all data from database
    refreshData() {
        this.cityMasterService.getAllData().then(response => {
            this.cityMasterViewModel = response.data;
            this.totalItems = this.cityMasterViewModel.length;
        }).catch(response => {
            if (response.status == "400") {
                Alertify.error(response.data);
                return;
            }
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.cityCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "City";
        info.title = "City";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "City";
        info.title = "City";
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }

    //Insert data to database
    addData(CityMasterForm) {
        this.jsonData['Kode City'] = this.cityCode.toUpperCase();
        this.jsonData['City'] = this.name.toUpperCase();
        Alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.jsonData)),
            () => {
                this.cityMasterService.createData(this.cityCode, this.name).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.reset(CityMasterForm);
                    this.refreshData();
                    this.setPage(this.currentPage);
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //update selected data to database
    updateData(CityMasterForm) {
        this.jsonData['Kode City'] = this.cityCode.toUpperCase();
        this.jsonData['City'] = this.name.toUpperCase();
        Alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('update', this.jsonData)),
            () => {
                this.cityMasterService.updateData(this.cityCode, this.name).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.reset(CityMasterForm);
                    this.refreshData();
                    this.setPage(this.currentPage);
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //delete selected data from database
    deleteData(data, CityMasterForm) {
        this.jsonData['Kode City'] = data.cityCode.toUpperCase();
        this.jsonData['City'] = data.name.toUpperCase();
        Alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', this.jsonData)),
            () => {
                this.cityMasterService.deleteData(data.cityCode).then(response => {
                    Alertify.success("Data berhasil dihapus");
                    this.reset(CityMasterForm);
                    this.refreshData();
                    this.setPage(this.currentPage);
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
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

    // Other
    reset(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.cityCode = null;;
        this.name = null;
        this.searchData = {};
        this.editCheck = false;
    }

    selectEdit(data) {
        this.cityCode = data.cityCode;
        this.name = data.name;
        this.editCheck = true;
    }

    //convert to json (Untuk alertify)
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

let CityMasterComponent = {
    controller: CityMasterController,
    controllerAs: 'me',
    template: require('./CityMaster.html')
}

export { CityMasterComponent }