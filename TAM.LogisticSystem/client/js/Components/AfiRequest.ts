import * as Angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as Mustache from 'mustache';
import * as Moment from 'moment';
export class AfiRequestController implements Angular.IController {
    $inject = ['AfiRequestService'];

    afiRequestService: Service.AfiRequestService;
    data: any;
    vehicle: Vehicle;
    dataInsert: DataInsertModel;
    regionData: any;
    regionAfiData: any;
    provinsi: any;
    kabupaten: any;
    kecamatan: any;
    buttonState: false;
    myForm: any;
    popup: boolean = false;
    altInputFormats: any = ['M!/d!/yyyy'];
    dateOptions : any = {
        formatYear: 'yy',
        minDate: new Date(),
        startingDay: 1
    };
    errorMessage: string;
    constructor(AfiRequestService: Service.AfiRequestService) {
        this.afiRequestService = AfiRequestService;
    }

    $onInit() {
        this.vehicle = new Vehicle();
        this.afiRequestService.getRegionAndRegionAFI().then((response:any) => {
            this.regionData = response.data.regionList;
            this.regionAfiData = response.data.regionAFIList;
            this.provinsi = _.filter(this.regionData, ['type', 'PROV']);
        });
        
    }
    open1() {
        this.popup = true;
    }
    //contoh penggunaan untuk delete
    jsonSalesArea = {};
    
    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        Angular.forEach(json, (value, key) => {
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
    postDataDialog(myForm: Angular.IFormController) {
        let json = {};
        json["Frame Number"] = this.vehicle.FrameNumber;
        json["Nama Customer"] = this.dataInsert.name;
        json["No Identitas"] = this.dataInsert.ktp;
        json["Alamat1"] = this.dataInsert.address1;
        json["Alamat2"] = this.dataInsert.address2;
        json["Alamat3"] = this.dataInsert.address3;
        json["Provinsi"] = (<any>this.dataInsert).province.name;
        json["Kota/Kabupaten"] = (<any>this.dataInsert).city.name;
        json["Kode Pos"] = this.dataInsert.postalCode;
        json["Tanggal Efektif Faktur"] = Moment(this.dataInsert.tanggalEfektif).format("DD-MMM-YYYY");
        json["Kode Region"] = this.dataInsert.regionAFI.afiRegionCode + " - " + this.dataInsert.regionAFI.name;
        alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                this.postData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }
    postData(myForm: Angular.IFormController) {
        this.dataInsert.vehicleId = this.vehicle.vehicleId;
        this.dataInsert.branch = this.vehicle.branch;
        this.dataInsert.chassis = (this.vehicle.chassis ? this.vehicle.chassis : null);
        let day = this.dataInsert.tanggalEfektif.getDate();
        let month = this.dataInsert.tanggalEfektif.getMonth();
        let year = this.dataInsert.tanggalEfektif.getFullYear();
        this.dataInsert.tanggalEfektif = new Date(Date.UTC(year, month, day));
        this.afiRequestService.insertAfi(this.dataInsert).then(response => {
            this.clearField(myForm);
            alertify.success(response.data);
        }).catch(
            response =>
            {
                if (response.status == "400") {
                    alertify.error(response.data);
                    return;
                }
                alertify.error("Koneksi ke server bermasalah");
            }
        );
    }
    getKabupaten() {
        this.kabupaten = _.filter(this.regionData, { 'type': 'KOTA', 'parentRegionCode': (<any>this.dataInsert.province).regionCode});
    }
    resetCustomError() {
        this.errorMessage = null;
    }
    checkData(event, myForm: Angular.IFormController) {
        if (this.vehicle.FrameNumber == null || this.vehicle.FrameNumber == "") {
            return;
        }
        if (this.vehicle.FrameNumber.length > 30) {
            return;
        }
        this.afiRequestService.checkDataByFrame(this.vehicle.FrameNumber).then(response => {
            this.data = response.data;
            if (this.data) {
                this.dataInsert = new DataInsertModel();
                this.vehicle.branch = this.data.branch;
                this.vehicle.doDate = this.data.doDate;
                this.vehicle.carModelCode = this.data.carModelCode;
                this.vehicle.carModelName = this.data.carModelName;
                this.vehicle.color = this.data.color;
                this.vehicle.vehicleId = this.data.vehicleId;
                //this.dataInsert.regionCode = this.data.currentRegion;
                this.dataInsert.color = this.data.color;
                this.dataInsert.jenis = this.data.name;
                this.dataInsert.model = this.data.model;
            } else {
                this.clearField(myForm);
            }
        }).catch(
            response =>
            {
                this.clearField(myForm);
                if (response.status == "400") {
                    this.errorMessage = response.data;
                    return;
                }
                alertify.error("Koneksi ke server bermasalah");
            }
        );
    }

    clearField(myForm: Angular.IFormController) {
        this.vehicle = null;
        $('#txtChassis').prop("disabled", true);
        this.dataInsert = null;
        myForm.$setPristine();
        myForm.$setUntouched();
    }
    //reset custom error
    resetCustomErrorMessage() {
        this.errorMessage = null;
    }
}

export class DataInsertModel {
    name: string;
    customerId?: number;
    ktp: string;
    address1: string;
    address2: string;
    address3: string;
    city: string;
    province: string;
    postalCode: string;
    regionCode: string;
    tanggalEfektif?: Date;
    vehicleId: number;
    branch: string;
    chassis: string;
    color: string;
    jenis: string;
    model: string;
    regionAFI: any;
}

export class Vehicle {
    FrameNumber: string;
    doDate?: Date;
    branch: string;
    carModelCode: string;
    carModelName: string;
    color: string;
    vehicleId: number;
    chassis: string;
}

export class AfiRequest implements Angular.IComponentOptions {
    controller = AfiRequestController;
    controllerAs = 'me';

    template =  require('./AfiRequest.html');
}