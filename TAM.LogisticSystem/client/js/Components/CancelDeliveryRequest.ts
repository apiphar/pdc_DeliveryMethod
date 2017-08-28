import * as deliveryRequestService from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as angular from 'angular';

import * as mustache from 'mustache';

class CancelDeliveryRequestController implements angular.IController {
    static $inject = ["CancelDeliveryRequestService", "$rootScope"];

    constructor(cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService, root: angular.IRootScopeService) {
        this.cancelDeliveryRequestService = cancelDeliveryRequestService;
        this.root = root;
    }

    root: angular.IRootScopeService;

    cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService;
    cancelDeliveryRequestList: deliveryRequestService.CancelDeliveryRequestViewModel[];
    cancelDeliveryRequestViewModel: deliveryRequestService.CancelDeliveryRequestViewModel;
    cancelDeliveryRequestViewModelList: deliveryRequestService.CancelDeliveryRequestViewModel[];
    cancelDeliveryRequestLocationList: deliveryRequestService.CancelDeliveryRequestLocationModel[];
    cancelDeliveryRequestLocationModel: deliveryRequestService.CancelDeliveryRequestLocationModel;
    deliveryRequestNumber: string;
    frameNumber: string;
    tempFrameNumber: string;
    isError: number;
    loading: boolean = false;
    validate: boolean = false;
    validateFrameNumber: boolean = false;
    validateButton: boolean = true;
    errorMessage: string;
    errorMessageFrameNumber: string;
    jsonDeliveryRequest: {};

    $onInit() {
        this.getAllData();
        this.cancelDeliveryRequestViewModel = new deliveryRequestService.CancelDeliveryRequestViewModel();
        this.jsonDeliveryRequest = {};
    }

    //Get Delivery Request by Frame Number
    getCancelDeliveryRequestByDeliveryRequestNumber() {
        this.cancelDeliveryRequestViewModel = _.find(this.cancelDeliveryRequestList, {
            'deliveryRequestNumber': this.deliveryRequestNumber
        });
        if (this.cancelDeliveryRequestViewModel !== undefined) {
            this.frameNumber = this.cancelDeliveryRequestViewModel.frameNumber;
        }

        if (this.cancelDeliveryRequestViewModel === undefined) {
            this.frameNumber = undefined;
        }

        if (this.cancelDeliveryRequestViewModel === undefined && this.deliveryRequestNumber !== undefined) {
            this.errorMessage = "No. Delivery Request tidak ditemukan";
            this.validate = true;
            this.validateButton = true;
        } else if (this.deliveryRequestNumber === undefined) {
            this.validate = false;
            this.validateButton = true;
            this.frameNumber = undefined;
        } else if (this.cancelDeliveryRequestViewModel.cancelledAt !== null) {
            this.errorMessage = "No. Delivery Request sudah dibatalkan";
            this.validate = true;
            this.validateButton = true;
        } else {
            this.validate = false;
            this.validateButton = false;

        }
       
        if (this.cancelDeliveryRequestViewModel !== undefined) {

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 1) {
                this.root.$broadcast('cancelDeliveryRequestNormal', this.cancelDeliveryRequestViewModel);
            }

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 2) {
                this.root.$broadcast('cancelDeliveryRequestSelfPick', this.cancelDeliveryRequestViewModel);
            }

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 3) {
                this.root.$broadcast('cancelDeliveryRequestDirectDelivery', this.cancelDeliveryRequestViewModel);
            }
            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 4) {
                this.getLocation();
                this.root.$broadcast('cancelDeliveryRequestTransitToOthers', this.cancelDeliveryRequestViewModel);
            }
        }

    }

    getCancelDeliveryRequestByFrameNumber() {

        this.cancelDeliveryRequestViewModel = _.find(this.cancelDeliveryRequestList, {
            'frameNumber': this.frameNumber
        });

        if (this.cancelDeliveryRequestViewModel !== undefined) {
            this.deliveryRequestNumber = this.cancelDeliveryRequestViewModel.deliveryRequestNumber;
        }

        this.tempFrameNumber = this.frameNumber;

        if (this.frameNumber == undefined) {
            this.tempFrameNumber = "";
        }

        if (this.cancelDeliveryRequestViewModel === undefined && this.tempFrameNumber.length == 17) {
            this.deliveryRequestNumber = undefined;
            this.errorMessageFrameNumber = "Frame Number tidak ditemukan";
            this.validateFrameNumber = true;
            this.validateButton = true;
        } else {
            this.validateFrameNumber = false;
            this.validateButton = false;
        }

        if (this.cancelDeliveryRequestViewModel == undefined) {
            this.deliveryRequestNumber = undefined;
        }

        if (this.cancelDeliveryRequestViewModel === undefined && this.deliveryRequestNumber !== undefined) {
            this.errorMessage = "No. Delivery Request tidak ditemukan";
            this.validate = true;
            this.validateButton = true;
        } else if (this.deliveryRequestNumber === undefined) {
            this.validate = false;
            this.validateButton = true;
        } else if (this.cancelDeliveryRequestViewModel.cancelledAt !== null) {
            this.errorMessage = "No. Delivery Request sudah dibatalkan";
            this.validate = true;
            this.validateButton = true;
        } else {
            this.validate = false;
            this.validateButton = false;
        }

        if (this.cancelDeliveryRequestViewModel !== undefined) {
            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 1) {
                this.root.$broadcast('cancelDeliveryRequestNormal', this.cancelDeliveryRequestViewModel);
            }

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 2) {
                this.root.$broadcast('cancelDeliveryRequestSelfPick', this.cancelDeliveryRequestViewModel);
            }

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 3) {
                this.root.$broadcast('cancelDeliveryRequestDirectDelivery', this.cancelDeliveryRequestViewModel);
            }
            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 4) {
                this.getLocation();
                this.root.$broadcast('cancelDeliveryRequestTransitToOthers', this.cancelDeliveryRequestViewModel);
            }
        }


    }

    //Get Semua Location Name, Address, & Tipe (termasuk nama dari Transit ReturnToPDC)
    getLocation() {
        this.cancelDeliveryRequestLocationModel = _.find(this.cancelDeliveryRequestLocationList, {
            'locationCode': this.cancelDeliveryRequestViewModel.locationCode
        });

        this.cancelDeliveryRequestViewModel.locationName = this.cancelDeliveryRequestLocationModel.locationName;
        this.cancelDeliveryRequestViewModel.locationAddress = this.cancelDeliveryRequestLocationModel.locationAddress;
        this.cancelDeliveryRequestViewModel.locationType = this.cancelDeliveryRequestLocationModel.locationTypeName;

        if (this.cancelDeliveryRequestViewModel.deliveryRequestTransitTypeId == 2) {
            this.cancelDeliveryRequestLocationModel = _.find(this.cancelDeliveryRequestLocationList,
                ['locationCode', this.cancelDeliveryRequestViewModel.otherPdcLocationCode]);
            this.cancelDeliveryRequestViewModel.otherPdcLocation = this.cancelDeliveryRequestLocationModel.locationName;
        }
    }

    //Get All Delivery Request
    getAllData() {
        this.cancelDeliveryRequestService.getAllData().then(response => {
            this.cancelDeliveryRequestList = response.data.cancelDeliveryRequest;
            this.cancelDeliveryRequestLocationList = response.data.cancelDeliveryRequestLocation;
        });//.catch(response => {
        //    if (response.status == "500") {
        //        alertify.error('Koneksi ke server bermasalah');
        //    }
        //});

    }

    //Submit Cancel Delivery Request
    cancelDeliveryRequest(form: angular.IFormController) {
        this.jsonDeliveryRequest["No Delivery Request"] = this.deliveryRequestNumber;
        this.jsonDeliveryRequest["Frame Number"] = this.frameNumber;
        if (this.cancelDeliveryRequestViewModel === undefined) {
            this.cancelDeliveryRequestViewModel = new deliveryRequestService.CancelDeliveryRequestViewModel();
        }
        this.cancelDeliveryRequestViewModel.deliveryRequestNumber = this.deliveryRequestNumber;
        alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonDeliveryRequest)),
            () => {
                this.cancelDeliveryRequestService.cancelDeliveryRequest(this.cancelDeliveryRequestViewModel).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.getAllData();
                    this.resetForm(form);
                }).catch(response => {
                    this.isError = response.data;
                    if (this.isError == 1) {
                        alertify.error('Data tidak valid');
                    } else if (response.status == "500") {
                        alertify.error('Koneksi ke server bermasalah');
                    } else {
                        alertify.error('Data gagal disimpan');
                    }
                });


            },
            () => {

            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });

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

    //Reset Form
    resetForm(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.deliveryRequestNumber = undefined;
        this.frameNumber = "";
        this.cancelDeliveryRequestViewModel = null;
        this.validateButton = true;
        this.validate = false;
    }
}

let CancelDeliveryRequestComponent = {
    controller: CancelDeliveryRequestController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequest.html")
}

export { CancelDeliveryRequestComponent }