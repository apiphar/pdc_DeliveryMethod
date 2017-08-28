import * as Service from '../services';
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as angular from 'angular';
import * as mustache from 'mustache';

export class GesekNoRangkaController implements angular.IController {
    $inject = ['GesekNoRangkaService'];


    //property
    GesekNoRangkaService: Service.GesekNoRangkaService;
    frameNo: string;
    data: Service.GesekNoRangkaData;
    dataInput: Service.GesekNoRangkaInputModel;
    dataConfirmation: Service.GesekNoRangkaConfirmation;
    dataLocation: Service.GesekNoRangkaLocationViewModel;
    location: string;
    tanggal: Date;
    regexCode: RegExp = /^[A-Za-z0-9]+$/;
    btnDisableFlag: boolean;
    formDisableFlag: boolean;
    dataAlertify = {};
    errorMessage: string;

    constructor(GesekNoRangkaService: Service.GesekNoRangkaService) {
        this.GesekNoRangkaService = GesekNoRangkaService;
    }



    //function
    $onInit() {
        this.btnDisableFlag = true;
        this.formDisableFlag = false;
        this.getLocation();
    }

    //get Location Data
    getLocation() {
        this.GesekNoRangkaService.getLocation().then(result => {
            this.dataLocation = result.data;
            this.location = this.dataLocation.locationCode + " - " + this.dataLocation.name;
        }).catch(response => {
            if (response.status == "400") {
                alertify.error(response.data);
                return;
            }
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        })
    }

    // template alertify confirmation
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


    //reset all field
    resetFields() {
        this.frameNo = null;
        this.data = new Service.GesekNoRangkaData();
        this.dataInput = new Service.GesekNoRangkaInputModel;
        this.dataConfirmation = new Service.GesekNoRangkaConfirmation;
        this.btnDisableFlag = true;
        this.tanggal = null;
        this.formDisableFlag = false;
    }

    //reset form
    reset(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.resetFields();
    }


    //mendapatkan informasi tanggal hari ini
    getCurrentDate() {
        this.tanggal = new Date();
    }


    //simpan ke db
    saveGesekan(form: angular.IFormController) {
        this.dataAlertify["Frame Number"] = this.dataConfirmation.frameNumber;
        this.dataAlertify["Jumlah Gesek"] = this.dataConfirmation.jumlahGesek;
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.dataAlertify)),
            () => {
                this.btnDisableFlag = true;
                this.GesekNoRangkaService.saveGesekan(this.dataInput).then(result => {
                    alertify.success("Data berhasil disimpan");
                    this.reset(form);
                }).catch(response => {
                    if (response.status == "400") {
                        alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    }
                })
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    //reset custom error
    resetCustomErrorMessage() {
        this.errorMessage = null;
    }

    //check apakah frame number ada dan apakah sudah di terdaftar atau belum
    checkDataByFrameNo(form: angular.IFormController) {    
        this.GesekNoRangkaService.checkDataByFrameNo(this.frameNo).then(result => {
            this.data = result.data;
            this.dataInput = new Service.GesekNoRangkaInputModel;
            this.dataInput.locationCode = this.dataLocation.locationCode;
            this.dataInput.vehicleId = this.data.vehicleId;
            this.getCurrentDate();
            this.dataConfirmation = new Service.GesekNoRangkaConfirmation;
            this.dataConfirmation.frameNumber = this.frameNo;
            this.dataConfirmation.jumlahGesek = this.data.jumlahGesek;
            this.btnDisableFlag = false;
            this.formDisableFlag = true;
        }).catch(response => {
            if (response.status == "400") {
                if (form.$valid == true) {
                    this.errorMessage = response.data;
                }
                return;
            }
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        })
    }
}


export class GesekNoRangka implements angular.IComponentOptions {
    controller = GesekNoRangkaController;
    controllerAs = 'me';
    template = require('./GesekNoRangka.html');
}