﻿import * as Service from '../services';
import * as Component from '../components';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as mustache from 'mustache';
import * as Angular from 'angular';
export class AfiRequestExCancelFormController implements angular.IController {
    $inject = ['AfiRequestRevisiAndExCancelFormService','$rootScope'];
    
    AfiRequestRevisiAndExCancelFormService: Service.AfiRequestRevisiAndExCancelFormService;
    $rootScope: angular.IRootScopeService;
    regionData: any; 
    Color: any;
    Jenis: any;
    Model: any;
    provinsi: any;
    kabupaten: any;
    regionAfiData: any;
    dataInsert: Component.DataInsertCanceModelForm;
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
    constructor(AfiRequestRevisiAndExCancelFormService: Service.AfiRequestRevisiAndExCancelFormService, $rootScope: angular.IRootScopeService) {
        this.AfiRequestRevisiAndExCancelFormService = AfiRequestRevisiAndExCancelFormService;
        this.$rootScope = $rootScope;
    }

    $onInit() {
        this.$rootScope.$on("RequestExCancel", (event, result) => {
            this.AfiRequestRevisiAndExCancelFormService.getRegionAndRegionAFI().then((response: any) => {
                this.regionData = response.data.regionList;
                this.regionAfiData = response.data.regionAFIList;
                this.provinsi = _.filter(this.regionData, ['type', 'PRVN']);

                //retrieve data to form
                this.pageState = true;
                this.tempData = result;
                this.initialForm(result);
            }).catch(
                response => {
                    if (response.status == "500") {
                        this.kembaliNoForm();
                        alertify.error("Koneksi ke server bermasalah");
                        return;
                    }
                });
            
        });
        
    }
    /**
     * Initial Form to retrieve all  data to field
     */
    initialForm(result: any) {
        this.dataInsert = new DataInsertCanceModelForm();
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
    clearField(myForm: angular.IFormController) {
        this.dataInsert = null;
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
    updateData(myForm:angular.IFormController) {
        this.dataInsert.afiApplicationId = this.tempData.afiApplicationId;
        this.dataInsert.tanggalEfektif = this.convertToDatePicker(this.dataInsert.tanggalEfektif);
        this.AfiRequestRevisiAndExCancelFormService.UpdateAFIExCancel(this.dataInsert).then(response => {
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
}

export class AfiRequestExCancelForm implements angular.IComponentOptions {
    controller = AfiRequestExCancelFormController;
    controllerAs = 'me';

    template =  require('./AfiRequestExCancelForm.html');
}


export class DataInsertCanceModelForm {
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