import * as angular from 'angular';
import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as mustache from 'mustache';
import * as moment from 'moment';

class DeliveryUnitAdvanceController implements angular.IController {
    static $inject = ['DeliveryUnitAdvanceService'];

    constructor(deliveryUnitAdvanceService: service.DeliveryUnitAdvanceService) {
        this.deliveryUnitAdvanceService = deliveryUnitAdvanceService
    }

    //private readonly
    deliveryUnitAdvanceService: service.DeliveryUnitAdvanceService;

    //model
    deliveryUnitAdvanceModel: service.DeliveryUnitAdvanceModel;
    deliveryUnitAdvanceModels: service.DeliveryUnitAdvanceModel[];
    jsonMasterGroupDealer = {};
    frameNumberSearch: string;

    //variable
    frameNumberRegex = "^[a-zA-Z0-9]+$";
    isFrameNumberExists: boolean = false;

    $onInit() {
        this.getUnitData();
        this.deliveryUnitAdvanceModel = null;
    }

    //mengirim form data dan validasi
    submitData(form: angular.IFormController) {
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.confirmationData(this.deliveryUnitAdvanceModel))),
            () => {
                this.deliveryUnitAdvanceService.submitData(this.deliveryUnitAdvanceModel).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.resetField(form);
                }).catch(response => {
                    if (response.data == "Data tidak valid") {
                        alertify.error(response.data);
                    }
                    else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => {
            }
        );
    }

    confirmationData(deliveryUnitAdvanceModel: service.DeliveryUnitAdvanceModel) {
        this.jsonMasterGroupDealer['Frame Number'] = deliveryUnitAdvanceModel.frameNumber;
        this.jsonMasterGroupDealer['Katashiki'] = deliveryUnitAdvanceModel.katashiki;
        this.jsonMasterGroupDealer['Suffix'] = deliveryUnitAdvanceModel.suffix;
        this.jsonMasterGroupDealer['Tipe'] = deliveryUnitAdvanceModel.tipe;
        this.jsonMasterGroupDealer['Branch'] = deliveryUnitAdvanceModel.branch;
        this.jsonMasterGroupDealer['Requested PDD'] = moment(deliveryUnitAdvanceModel.requestedPDD).format("DD/MM/YYYY");
        this.jsonMasterGroupDealer['Model'] = deliveryUnitAdvanceModel.model;
        this.jsonMasterGroupDealer['Warna'] = deliveryUnitAdvanceModel.warna;
        this.jsonMasterGroupDealer['Customer Assign'] = deliveryUnitAdvanceModel.customerAssign == true ? "Ya" : "Tidak";
        return this.jsonMasterGroupDealer;
    }

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


    //mendapatkan semua data
    getUnitData() {
        this.deliveryUnitAdvanceService.getUnitData().then(response => {
            this.deliveryUnitAdvanceModels = response.data as service.DeliveryUnitAdvanceModel[];
        })
    }

    //search data menggunakan lodash
    searchData() {
        this.deliveryUnitAdvanceModel = _.find(this.deliveryUnitAdvanceModels, ['frameNumber', this.frameNumberSearch]);        
        if (this.frameNumberSearch != undefined) {
            if (this.deliveryUnitAdvanceModel == null && this.frameNumberSearch.length == 17) {
                this.isFrameNumberExists = true;
            }
        }
        else {
            this.isFrameNumberExists = false;
        }
    }

    //reset field form
    resetField(form: angular.IFormController) {
        this.frameNumberSearch = null;
        this.deliveryUnitAdvanceModel = null;
        form.$setPristine();
        form.$setUntouched();
        this.getUnitData();
    }
}

let deliveryUnitAdvanceComponent = {
    controller: DeliveryUnitAdvanceController,
    controllerAs: 'me',
    template: require("./DeliveryUnitAdvanceComponent.html")
}

export { deliveryUnitAdvanceComponent }