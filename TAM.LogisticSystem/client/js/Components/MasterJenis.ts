import * as angular from 'angular';
import * as services from '../services';
import * as mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as moment from 'moment';

export class MasterJenisController implements angular.IController {
    static $inject = ['MasterJenisService', '$rootScope'];

    root: angular.IRootScopeService
    afiCarTypeCode: string;
    jenis: string;
    model: string;
    editCheck: boolean;
    pageState: boolean = true;
    disableInput: boolean = false;
    searchData = {};
    regexCode: string = '[A-Za-z0-9]+$';
    regexName: string = '[a-zA-Z0-9\\s-.&,\'/]+$';

    jsonJenisData = {};

    masterJenisService: services.MasterJenisService;
    masterJenisViewModel: services.MasterJenisViewModel[];

    constructor(masterJenisService: services.MasterJenisService, root: angular.IRootScopeService) {
        this.masterJenisService = masterJenisService;
        this.root = root;
    }

    //GET ALL DATA
    refreshData() {
        this.masterJenisService.getAllJenisData().then(response => {
            this.masterJenisViewModel = response.data;
            this.totalItems = this.masterJenisViewModel.length;
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

    $onInit() {
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
        this.refreshData();
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.afiCarTypeCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "AFICarType";
        info.title = "Master Jenis";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "AFICarType";
        info.title = "Master Jenis";
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }

    //insert data
    addData(MasterJenisForm) {
        this.jsonJenisData["Kode Jenis"] = this.afiCarTypeCode;
        this.jsonJenisData["Jenis"] = this.jenis;
        this.jsonJenisData["Model"] = this.model;
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonJenisData)),
            () => {
                this.masterJenisService.createJenisData(this.afiCarTypeCode, this.jenis, this.model).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(MasterJenisForm);
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

    //update data
    updateData(MasterJenisForm) {
        this.jsonJenisData["Kode Jenis"] = this.afiCarTypeCode;
        this.jsonJenisData["Jenis"] = this.jenis;
        this.jsonJenisData["Model"] = this.model;
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonJenisData)),
            () => {
                this.masterJenisService.updateJenisData(this.afiCarTypeCode, this.jenis, this.model).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(MasterJenisForm);
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

    //delete selected data
    deleteData(data, MasterJenisForm) {
        this.jsonJenisData["Kode Jenis"] = data.afiCarTypeCode;
        this.jsonJenisData["Jenis"] = data.jenis;
        this.jsonJenisData["Model"] = data.model;
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonJenisData)),
            () => {
                this.masterJenisService.deleteJenisData(data.afiCarTypeCode).then(response => {
                    Alertify.success("Data berhasil dihapus");
                    this.refreshData();
                    this.setPage(this.currentPage);
                    this.reset(MasterJenisForm);
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

    // Selecting Data
    selectEdit(data) {
        this.afiCarTypeCode = data.afiCarTypeCode;
        this.model = data.model;
        this.jenis = data.jenis;
        this.editCheck = true;
        this.disableInput = true;
    }

    // Other
    reset(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.afiCarTypeCode = null;
        this.jenis = null;
        this.model = null;
        this.searchData = {};
        this.editCheck = false;
        this.disableInput = false;
    }

    //Convert to mustache JSON
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


let MasterJenisComponent = {
    controller: MasterJenisController,
    controllerAs: 'me',
    template: require("./MasterJenis.html")
}

export { MasterJenisComponent };