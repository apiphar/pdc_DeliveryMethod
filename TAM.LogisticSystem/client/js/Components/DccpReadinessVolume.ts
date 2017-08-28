import * as angular from 'angular';
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as mustache from 'mustache';

export class DccpReadinessController implements angular.IController {
    //injection
    static $inject = ["DccpReadinessVolumeService", "$rootScope"];
    //constructor - DI
    constructor(DccpService: Service.DccpReadinessVolumeService, RootScope: angular.IRootScopeService) {
        this.dccpService = DccpService;
        this.root = RootScope;
    }


    //declaration
    root: angular.IRootScopeService;
    dccpService: Service.DccpReadinessVolumeService;
    dccpListModel: Service.DccpReadinessVolumeModel[];
    dccpSingleModel: Service.DccpReadinessVolumeModel;
    newEditModel: Service.DccpReadinessVolumeModel;
    divOpen: boolean;
    jsonAlertify = {};

    //on init, configuration opening-closing div and receiving new data of list Model
    $onInit() {
        this.divOpen = false;
        this.getData();
        this.root.$on("ListModel", (event, data) => {
            this.dccpListModel = data;
        });
        this.root.$on("OpenDiv", (event, data) => {
            this.divOpen = data;
        });
    }

    //Pagination ordering and searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    isLoading: boolean = true;

    //ordering pagination
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    //searching in pagination
    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }
    //setting page pagination
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

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin untuk menambahkan data :";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data :";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data :";
        convertResult["grid"] = tempJson;
        return convertResult;
    }
    //delete Dccp
    deleteData(item: Service.DccpReadinessVolumeModel) {
        this.jsonAlertify["Tanggal"] = item.tanggal
        this.jsonAlertify["Lokasi Asal"] = item.asal;
        this.jsonAlertify["Lokasi Tujuan"] = item.tujuan;
        this.jsonAlertify["Trip"] = item.trip;
        this.jsonAlertify["Load"] = item.load;
        this.jsonAlertify["Shift"] = item.shift;
        this.jsonAlertify["Quantity"] = item.qty;
        this.jsonAlertify["Adjust"] = item.adjust;
        this.jsonAlertify["Unit Estimasi"] = item.estimasi;
        alertify.confirm("Konfirmasi DCCP Readiness Volume", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonAlertify)),
            () => {
                this.dccpService.deleteData(item.id).then(response => {
                    this.dccpListModel = response.data as Service.DccpReadinessVolumeModel[];
                    this.getData();
                    alertify.success("Sukses Delete Data");
                    this.newEditModel = new Service.DccpReadinessVolumeModel;
                    this.root.$broadcast("ModelView", this.newEditModel);
                }).catch(response => {
                    alertify.error("Gagal delete data");
                });
            },
            () => {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
    //get All DCCP model
    getData() {
        this.dccpService.getData().then(response => {
            this.dccpListModel = response.data as Service.DccpReadinessVolumeModel[];
            this.totalItems = this.dccpListModel.length;
            this.isLoading = false;
        })

    }

    //select single model to edit
    getSingleModel(editDccpModel: Service.DccpReadinessVolumeModel) {
        this.newEditModel = new Service.DccpReadinessVolumeModel;
        this.newEditModel.id = editDccpModel.id;
        this.newEditModel.adjust = editDccpModel.adjust;
        this.newEditModel.asal = editDccpModel.asal;
        this.newEditModel.estimasi = editDccpModel.estimasi;
        this.newEditModel.load = editDccpModel.load;
        this.newEditModel.qty = editDccpModel.qty;
        this.newEditModel.shift = editDccpModel.shift;
        this.newEditModel.tanggal = editDccpModel.tanggal;
        this.newEditModel.trip = editDccpModel.trip;
        this.newEditModel.tujuan = editDccpModel.tujuan;
        this.root.$broadcast("ModelView", this.newEditModel);
        this.divOpen = true;
        this.root.$broadcast("OpenDiv", this.divOpen);
    }

}
let dccpReadinessVolume = {
    controller: DccpReadinessController,
    controllerAs: 'me',
    template: require("./DccpReadinessVolume.html")
}

export { dccpReadinessVolume };