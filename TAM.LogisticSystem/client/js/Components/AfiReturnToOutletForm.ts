import * as Service from '../services';
import * as Component from '../components';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as mustache from 'mustache';
import * as Angular from 'angular';
export class AfiReturnToOutletFormController implements angular.IController {
    $inject = ['AfiReturnToOutletFormService','$rootScope'];
    
    AfiReturnToOutletService: Service.AfiReturnToOutletService;
    $rootScope: angular.IRootScopeService;
    regionData: any; 
    Color: any;
    Jenis: any;
    Model: any;
    provinsi: any;
    kabupaten: any;
    regionAfiData: any;
    dataInsert: Component.DataInsertROModelForm;
    url: string;
    disableState: boolean = false;
    pageState: boolean = false;
    tempData: any;
    disableFrame: boolean = true;
    popup: boolean = false;
    altInputFormats: any = ['M!/d!/yyyy'];
    dateOptions: any = {
        formatYear: 'yy',
        minDate: new Date(),
        startingDay: 1
    };
    errorMessage: string;
    constructor(AfiReturnToOutletService: Service.AfiReturnToOutletService, $rootScope: angular.IRootScopeService) {
        this.AfiReturnToOutletService = AfiReturnToOutletService;
        this.$rootScope = $rootScope;
    }

    $onInit() {
        this.$rootScope.$on("ReturnToOutlet", (event, result) => {
            this.AfiReturnToOutletService.getRegionAndRegionAFI().then((response: any) => {
                this.regionData = response.data.regionList;
                this.regionAfiData = response.data.regionAFIList;
                this.provinsi = _.filter(this.regionData, ['type', 'PRVN']);

                //retrieve data to form
                this.pageState = true;
                this.tempData = result;
                this.initialForm(result);
            }).catch(
                response => {
                    this.kembaliNoForm();
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                        return;
                    }
                });;
        });
        
    }
    /**
     * Initial Form to retrieve all  data to field
     */
    initialForm(result: any) {
        this.dataInsert = new DataInsertROModelForm();
        this.dataInsert.frameNumber = result.frameNumber;
        this.dataInsert.color = result.color;
        this.dataInsert.branch = result.branch;
        this.dataInsert.doDate = result.doDate;
        this.dataInsert.jenis = result.jenis;
        this.dataInsert.model = result.model;
        this.dataInsert.carModelCode = result.modelCode;
        this.dataInsert.carModelName = result.modelName;
        this.dataInsert.chassis = result.chassis;
        this.dataInsert.name = result.customerName;
        this.dataInsert.ktp = result.ktp;
        this.dataInsert.address1 = result.address1;
        this.dataInsert.address2 = result.address2;
        this.dataInsert.address3 = result.address3;
        this.dataInsert.postalCode = result.postalCode;
        this.dataInsert.province = _.find(this.provinsi, (data: any) => { return data.name.toLowerCase() == result.province.toLowerCase() });
        this.dataInsert.city = _.find(this.regionData, (data: any) => { return data.name.toLowerCase() == result.city.toLowerCase() });
        this.dataInsert.regionAFI = _.find(this.regionAfiData, ['afiRegionCode', result.region.split(' -')[0]]);
        this.dataInsert.tanggalEfektif = new Date(result.tanggalEfektif);
        this.dataInsert.vehicleId = result.vehicleId;
    }
    open1() {
        this.popup = true;
    }
    resetCustomError() {
        this.errorMessage = null;
    }
    checkData(event, myForm: angular.IFormController) {
        if (this.dataInsert.frameNumber == null || this.dataInsert.frameNumber == "") {
            return;
        }
        if (this.dataInsert.frameNumber.length > 30) {
            return;
        }
        this.AfiReturnToOutletService.checkDataByFrame(this.dataInsert.frameNumber).then((response:any) => {
            this.dataInsert = new DataInsertROModelForm();
            this.dataInsert.branch = response.data.branch;
            this.dataInsert.doDate = response.data.doDate;
            this.dataInsert.carModelCode = response.data.carModelCode;
            this.dataInsert.carModelName = response.data.carModelName;
            this.dataInsert.color = response.data.color;
            this.dataInsert.vehicleId = response.data.vehicleId;
            this.dataInsert.color = response.data.color;
            this.dataInsert.jenis = response.data.name;
            this.dataInsert.model = response.data.model;
            this.dataInsert.frameNumber = response.data.frameNumber;
            this.dataInsert.vehicleId = response.data.vehicleId;
            this.disableFrame = true;
            this.disableState = false;
        }).catch(
            response => {
                this.clearField(myForm);
                if (response.status == "400") {
                    this.errorMessage = response.data;
                    return;
                }
                alertify.error("Koneksi ke server bermasalah");
            }
        );
    }
    clearField(myForm: angular.IFormController) {
        this.dataInsert = null;
        this.disableState = true;
        this.disableFrame = false;
        myForm.$setPristine();
        myForm.$setUntouched();
    }
    convertToDatePicker(tanggal:Date) {
        return new Date(Date.UTC(tanggal.getFullYear(), tanggal.getMonth(), tanggal.getDate()));
    }
    kembaliNoForm() {
        this.dataInsert = null;
        this.tempData = null;
        this.pageState = false;
        this.$rootScope.$emit("ReturnToOutletKembali");
    }
    kembali(myForm:angular.IFormController) {
        this.dataInsert = null;
        this.tempData = null;
        this.pageState = false;
        myForm.$setPristine();
        myForm.$setUntouched();
        this.$rootScope.$emit("ReturnToOutletKembali");
    }
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
    updateDataDialog(myForm: angular.IFormController) {
        let json = {};
        json["Frame Number"] = this.dataInsert.frameNumber;
        json["Nama Customer"] = this.dataInsert.name;
        json["No Identitas"] = this.dataInsert.ktp;
        json["Alamat1"] = this.dataInsert.address1;
        json["Alamat2"] = this.dataInsert.address2;
        json["Alamat3"] = this.dataInsert.address3;
        json["Provinsi"] = (<any>this.dataInsert).province.name;
        json["Kota/Kabupaten"] = (<any>this.dataInsert).city.name;
        json["Kode Pos"] = this.dataInsert.postalCode;
        json["Tanggal Efektif Faktur"] = moment(this.dataInsert.tanggalEfektif).format("DD-MMM-YYYY");
        json["Kode Region"] = this.dataInsert.regionAFI.afiRegionCode + " - " + this.dataInsert.regionAFI.name;
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", json)),
            () => {
                this.updateData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
    updateData(myForm:angular.IFormController) {
        this.dataInsert.afiApplicationId = this.tempData.afiApplicationId;
        this.dataInsert.tanggalEfektif = this.convertToDatePicker(this.dataInsert.tanggalEfektif);
        this.AfiReturnToOutletService.UpdateAFINormal(this.dataInsert).then(response => {
            this.kembali(myForm);
            alertify.success(response.data);
        }).catch(
            response => {
                this.kembali(myForm);
                if (response.status == "400") {
                    alertify.error(response.data);
                    return;
                }
                alertify.error("Koneksi ke server bermasalah");
            }
         );
    }
    

    getKabupaten() {
        this.dataInsert.city = null;
        this.kabupaten = _.filter(this.regionData, { 'type': 'KOTA', 'parentRegionCode': (<any>this.dataInsert.province).regionCode });
    }
}

export class AfiReturnToOutletForm implements angular.IComponentOptions {
    controller = AfiReturnToOutletFormController;
    controllerAs = 'me';

    template =  require('./AfiReturnToOutletForm.html');
}


export class DataInsertROModelForm {
    revisi?: number;
    afiApplicationId: string;
    customerName: string;
    customerId?: number;
    ktp: string;
    address1: string;
    address2: string;
    address3: string;
    city: string;
    province: string;
    postalCode: string;
    regionCodeAFI: string;
    regionNameAFI: string;
    tanggalEfektif?: Date;
    regionAFI: any;

    vehicleId: number;
    doDate?: Date;
    branch: string;
    carModelCode: string;
    carModelName: string;
    color: string;
    model: string;
    name: string;
    chassis: string;
    afiApplicationProcessId: number;
    frameNumber: string;
    jenis: string;
}