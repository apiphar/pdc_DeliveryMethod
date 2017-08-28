import * as angular from 'angular';
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';

export class EditDccpVolumeController implements angular.IController {
    //injection
    static $inject = ["DccpReadinessVolumeService", "$rootScope"];

    //constructor, DI and receiving broadcast
    constructor(DccpService: Service.DccpReadinessVolumeService, rootScope: angular.IRootScopeService) {
        this.dccpService = DccpService;
        this.root = rootScope;

    }

    //declaration
    root: angular.IRootScopeService;
    dccpService: Service.DccpReadinessVolumeService;
    dccpListModel: Service.DccpReadinessVolumeModel[];
    editDccpModel: Service.DccpReadinessVolumeModel;
    divOpen: boolean;
    jsonAlertify = {};

    //on initialize receiving the broadcast
    $onInit() {
        this.root.$on("ModelView", (event, data) => {

            this.editDccpModel = data;
        });
        this.root.$on("OpenDiv", (event, data) => {
            this.divOpen = data;
        });
    }
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
    batal(Form: angular.IFormController) {
        this.editDccpModel = new Service.DccpReadinessVolumeModel;
        Form.$setPristine(); Form.$setUntouched();
    }

    //updateData of Dccp and alertify if failed
    updateData() {
        this.jsonAlertify["Tanggal"] = this.editDccpModel.tanggal
        this.jsonAlertify["Lokasi Asal"] = this.editDccpModel.asal;
        this.jsonAlertify["Lokasi Tujuan"] = this.editDccpModel.tujuan;
        this.jsonAlertify["Trip"] = this.editDccpModel.trip;
        this.jsonAlertify["Load"] = this.editDccpModel.load;
        this.jsonAlertify["Shift"] = this.editDccpModel.shift;
        this.jsonAlertify["Quantity"] = this.editDccpModel.qty;
        this.jsonAlertify["Adjust"] = this.editDccpModel.adjust;
        this.jsonAlertify["Unit Estimasi"] = this.editDccpModel.estimasi;
        alertify.confirm('Konfirmasi DCCP Readiness Volume', mustache.render(require('../components/alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.jsonAlertify)),
            () => {
            this.dccpService.updateData(this.editDccpModel).then(response => {
                this.dccpListModel = response.data as Service.DccpReadinessVolumeModel[];
                this.root.$broadcast("ListModel", this.dccpListModel);
                alertify.success('Success Edit Data');
                this.divOpen = false;
                this.root.$broadcast("OpenDiv", this.divOpen);
            }).catch(response => {
                alertify.error("Gagal Edit Data");
            });
        },
            () => {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
}
let editDccpVolume = {
    controller: EditDccpVolumeController,
    controllerAs: 'me',
    template: require("./EditDccpVolume.html")
}

export { editDccpVolume };