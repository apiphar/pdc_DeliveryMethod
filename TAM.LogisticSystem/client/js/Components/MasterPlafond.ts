import * as angular from 'angular';
import * as Lodash from 'lodash';
import * as Alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as service from '../services';

class MasterPlafondController implements angular.IController {
    static $inject = ["MasterPlafondService", '$rootScope'];

    root: angular.IRootScopeService;

    masterPlafondService: service.MasterPlafondService;

    masterPlafondViewModel: service.MasterPlafondViewModel[];

    kodeCompanyViewModel: service.CodeCompanyViewModel[];
    kodeCompany: string;
    plafond: number;
    plafondMasterId: number;
    pageState: boolean = true;
    jsonMasterPlafond = {};

    regexPlafond: string = '[0-9]+';

    deleteId: number;

    editCheck: boolean;

    constructor(masterPlafondService: service.MasterPlafondService, root: angular.IRootScopeService) {
        this.masterPlafondService = masterPlafondService;
        this.root = root;
    }

    $onInit() {
        this.refreshData();
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });
    }

    //Get all plafond data
    refreshData() {
        this.masterPlafondService.getAllPlafondData().then(response => {
            this.masterPlafondViewModel = response.data;
            this.totalItems = this.masterPlafondViewModel.length;
        });
        //Get Company code yang belum diinsert ke table PlafondMaster
        this.masterPlafondService.getCompanyCode().then(response => {
            this.kodeCompanyViewModel = response.data;
        });
    }

    //insert plafond data
    addData(MasterPlafondForm) {
        this.jsonMasterPlafond["Kode Company"] = this.kodeCompany;
        this.jsonMasterPlafond["Plafond"] = this.plafond;
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonMasterPlafond)),
            () => {
                this.masterPlafondService.postPlafondData(this.kodeCompany, this.plafond).then(response => {
                    Alertify.success("Data berhasil disimpan");
                }).catch(response => {
                    Alertify.error(response.data);
                    this.reset(MasterPlafondForm);
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //update selecting plafond data
    updateData(MasterPlafondForm) {
        this.jsonMasterPlafond["Kode Company"] = this.kodeCompany;
        this.jsonMasterPlafond["Plafond"] = this.plafond;
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonMasterPlafond)),
            () => {
                this.masterPlafondService.updatePlafondData(this.plafondMasterId, this.plafond).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.reset(MasterPlafondForm);
                }).catch(response => {
                    Alertify.error(response.data);
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //delete selecting plafond data
    deleteData(data, MasterPlafondForm) {
        this.jsonMasterPlafond["Kode Company"] = data.kodeCompany;
        this.jsonMasterPlafond["Plafond"] = data.plafond;
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonMasterPlafond)),
            () => {
                this.masterPlafondService.deletePlafondData(data.plafondMasterId).then(response => {
                    Alertify.success("Data berhasil dihapus");
                    this.reset(MasterPlafondForm);
                }).catch(response => {
                    Alertify.error("Data gagal dihapus");
                }).finally(() => {
                    this.refreshData();
                    this.setPage(this.currentPage);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Selecting Data
    selectEdit(data) {
        this.plafondMasterId = data.plafondMasterId;
        this.kodeCompany = data.kodeCompany;
        this.plafond = data.plafond;
        this.editCheck = true;
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
        this.kodeCompany = null;
        this.plafond = undefined;
        this.deleteId = undefined;
        this.editCheck = undefined;
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.plafondMasterId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "PlafondMaster";
        info.title = "Master Plafond";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "PlafondMaster";
        info.title = "Master Plafond"
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }
    ////to alertify
    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin untuk menambahkan data ?";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data ?";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data ?";
        convertResult["grid"] = tempJson;
        return convertResult;
    }

    showKodeCompany(kodeCompany: string) {
        let tempKodeModel = Lodash.find(this.kodeCompanyViewModel, ['kodeCompany', kodeCompany]);
        if (tempKodeModel == undefined) {
            // TIE: START
            // return;
            return 0;
            // TIE: END
        }
        return tempKodeModel.kodeCompany + ' - ' + tempKodeModel.name;
    }

    //Disable button Simpan / Update
    disableButton() {
        if (this.kodeCompany == undefined || this.plafond == undefined || this.plafond < 0) {
            return true;
        }
        return false;
    }
}



let MasterPlafondComponent = {
    controller: MasterPlafondController,
    controllerAs: 'me',
    template: require("./MasterPlafond.html")
}

export { MasterPlafondComponent };