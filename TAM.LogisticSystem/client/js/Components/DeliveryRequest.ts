import * as deliveryRequestService from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as mustache from 'mustache';
import * as moment from 'moment';
import * as angular from 'angular';

class DeliveryRequestController implements angular.IController {
    static $inject = ["DeliveryRequestService", "$rootScope"];


    constructor(deliveryRequestService: deliveryRequestService.DeliveryRequestService, root: angular.IRootScopeService) {
        this.deliveryRequestService = deliveryRequestService;
        this.root = root;
    }

    root: angular.IRootScopeService;

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    deliveryRequestViewModel: deliveryRequestService.DeliveryRequestViewModel;
    deliveryRequestViewCarModelList: deliveryRequestService.DeliveryRequestCarModel[];
    deliveryRequestTransitToOthersModel: deliveryRequestService.DeliveryRequestTransitToOthersModel;
    deliveryRequestSelfPickToOthersModel: deliveryRequestService.DeliveryRequestSelfPickToOthersModel;
    deliveryRequestTransitToOthersNormalModel: deliveryRequestService.DeliveryRequestTransitToOthersNormalModel;
    deliveryRequestSelfPickFromOtherModel: deliveryRequestService.DeliveryRequestSelfPickFromOtherModel;
    deliveryRequestNormalCreateModel: deliveryRequestService.DeliveryRequestNormalCreateModel;
    deliveryRequestSelfPickCreateModel: deliveryRequestService.DeliveryRequestSelfPickCreateModel;
    deliveryRequestDirectDeliveryCreateModel: deliveryRequestService.DeliveryRequestDirectDeliveryCreateModel;
    deliveryRequestTransitToOthersSelfPickToOthersCreateModel: deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersCreateModel;
    deliveryRequestTransitToOthersNormalReturnToPdcCreateModel: deliveryRequestService.DeliveryRequestTransitToOthersNormalReturnToPdcCreateModel;
    deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel: deliveryRequestService.DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel;
    frameNo: string;
    isError: number;
    //flag untuk menampilkan error message
    validate: boolean = false;
    //flag apakah validasi Delivery Request sudah terpenuhi atau blm
    validateRequest: boolean = false;
    //flag apakah validasi Radio Button sudah terpenuhi atau blm
    validateType: boolean = false;
    //flag apakah validasi Detail (Normal, Self Pick, Direct Delivery, Transit To Others) sudah terpenuhi atau blm
    validateDetail: boolean = false;
    //flag untuk validasi method Normal, dll. (flag ini yg akan dikirim antar component)
    validateDetailNormal: boolean = false;
    validateDetailSelfPick: boolean = false;
    validateDetailDirectDelivery: boolean = false;
    validateDetailTransitToOthers: boolean = false;
    validateDetailSelfPickToOthers: boolean = false;
    validateDetailSelfPickFromOthers: boolean = false;
    validateDetailTransitNormal: boolean = false;
    //flag uapakah validasi Transit Detail (Normal, Self Pick From Others, Return To PDC, Return To Others PDC, Self Pick To Others) sdh terpenuhi atau blm
    validateTransitDetail: boolean = true;
    //flag apakah validasi di component Main (DeliveryRequest.html) sudah terpenuhi atau blm (dilihat dr validateRequest == true && validateType == true => maka menjadi true)
    //agar button menjadi ENABLED, maka validateMain, validateDetail, & validateTransitDetail nya harus TRUE
    validateMain: boolean = false;
    deliveryRequestModelList: deliveryRequestService.DeliveryRequestModel[];
    deliveryRequestModelbyVehicleIdList: deliveryRequestService.DeliveryRequestModel[];
    deliveryRequestModel: deliveryRequestService.DeliveryRequestModel;
    deliveryRequestLocationType: deliveryRequestService.DeliveryRequestLocationModel[];
    deliveryRequestLocationName: deliveryRequestService.DeliveryRequestLocationNameModel[];
    deliveryRequestLocationAddress: deliveryRequestService.DeliveryRequestTransitToOthersModel[];
    deliveryRequestOtherPdcLocation: deliveryRequestService.DeliveryRequestOtherPdcLocationModel[];
    errorMessage: string = "";
    jsonDeliveryRequest: {};
    kodeBranch: string = "";
    tipeDR: string = "";
    tanggalDR: string = "";
    bulanDR: string = "";
    dateDR: string = "";
    sequentialNo: number = 0;

    $onInit() {
        this.jsonDeliveryRequest = {};
        this.deliveryRequestModelList = new Array();
        this.deliveryRequestViewModel = new deliveryRequestService.DeliveryRequestViewModel();
        this.deliveryRequestViewModel.deliveryRequest = new deliveryRequestService.DeliveryRequestModel();
        this.deliveryRequestViewModel.deliveryRequest.createdAt = new Date();
        this.deliveryRequestTransitToOthersModel = new deliveryRequestService.DeliveryRequestTransitToOthersModel();
        this.deliveryRequestSelfPickToOthersModel = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel();
        this.deliveryRequestTransitToOthersNormalModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalModel();
        this.deliveryRequestSelfPickFromOtherModel = new deliveryRequestService.DeliveryRequestSelfPickFromOtherModel();
        this.deliveryRequestNormalCreateModel = new deliveryRequestService.DeliveryRequestNormalCreateModel();
        this.deliveryRequestNormalCreateModel.deliveryRequest = new deliveryRequestService.DeliveryRequestModel();
        this.deliveryRequestNormalCreateModel.deliveryRequestNormal = new deliveryRequestService.DeliveryRequestNormalModel();
        this.deliveryRequestSelfPickCreateModel = new deliveryRequestService.DeliveryRequestSelfPickCreateModel();
        this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick = new deliveryRequestService.DeliveryRequestSelfPickModel();
        this.deliveryRequestDirectDeliveryCreateModel = new deliveryRequestService.DeliveryRequestDirectDeliveryCreateModel();
        this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery = new deliveryRequestService.DeliveryRequestDirectDeliveryModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel = new deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersCreateModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers = new deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers.returnPdcDate = new Date();
        this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalReturnToPdcCreateModel();
        this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc = new deliveryRequestService.DeliveryRequestTransitToOthersNormalReturnToPdcModel();
        this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthersNormalModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalModel();
        this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel();
        this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers = new deliveryRequestService.DeliveryRequestTransitToOthersNormalSelfPickFromOthersModel();
        this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryRequestTransitToOthersNormalModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalSelfPickModel();
        this.getAllDeliveryRequest();
        this.generateDeliveryRequestNumber();

        //get data di method Normal
        this.root.$on('deliveryRequestNormal', (event, data) => {
            this.deliveryRequestNormalCreateModel.deliveryRequestNormal = data;
            this.validateDetail = this.deliveryRequestNormalCreateModel.deliveryRequestNormal.validateDetail;
            this.validateDetailNormal = this.deliveryRequestNormalCreateModel.deliveryRequestNormal.validateDetail;
        });

        //get data di method Self Pick
        this.root.$on('deliveryRequestSelfPick', (event, data) => {
            this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick = data;
            this.validateDetail = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.validateDetail;
            this.validateDetailSelfPick = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.validateDetail;
        });

        //get data di method Direct Delivery
        this.root.$on('deliveryRequestDirectDelivery', (event, data) => {
            this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery = data;
            this.validateDetail = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.validateDetail;
            this.validateDetailDirectDelivery = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.validateDetail;
        });

        //get data di method Transit To Others
        this.root.$on('deliveryRequestTransitToOthers', (event, data) => {
            this.deliveryRequestTransitToOthersModel = data;
            this.validateDetail = this.deliveryRequestTransitToOthersModel.validateDetail;
            this.validateTransitDetail = this.deliveryRequestTransitToOthersModel.validateTransitDetail;
            this.validateDetailTransitToOthers = this.deliveryRequestTransitToOthersModel.validateDetail;
        });

        //get data di method Self Pick To Others
        this.root.$on('deliveryRequestSelfPickToOthers', (event, data) => {
            this.deliveryRequestSelfPickToOthersModel = data;
            this.validateDetail = this.validateDetailTransitToOthers;
            if (this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType === "SelfPickToOthers") {
                this.validateTransitDetail = this.deliveryRequestSelfPickToOthersModel.validateTransitDetail;
            }
            this.validateDetailSelfPickToOthers = this.deliveryRequestSelfPickToOthersModel.validateTransitDetail;

        });

        //get data di method Transit To Others - Normal
        this.root.$on('deliveryRequestTransitToOthersNormal', (event, data) => {
            this.deliveryRequestTransitToOthersNormalModel = data;
            this.validateDetail = this.validateDetailTransitToOthers;

            this.validateTransitDetail = this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail;

            this.validateDetailTransitNormal = this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail;
        });

        //get data di method Self Pick From Others
        this.root.$on('deliveryRequestTransitToOthersSelfPickFromOthers', (event, data) => {
            this.deliveryRequestSelfPickFromOtherModel = data;
            this.validateDetail = this.validateDetailTransitToOthers;
            this.validateTransitDetail = this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail;
            this.validateDetailSelfPickFromOthers = this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail;
        });

    }


    //Generate Delivery Request (Kode Branch/DeliveryMethod/TanggalDR/SequentialNumber)
    generateDeliveryRequestNumber() {
        this.sequentialNo = 0;
        if (this.deliveryRequestViewModel.deliveryRequestCar !== undefined && this.deliveryRequestViewModel.deliveryRequestCar.branchCode !== undefined) {
            this.kodeBranch = this.deliveryRequestViewModel.deliveryRequestCar.branchCode;
        } else {
            this.kodeBranch = "";
            this.sequentialNo = 0;
        }
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === 'Normal') {
            this.tipeDR = 'NR';
        }

        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === 'SelfPick') {
            this.tipeDR = 'SP';
        }

        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === 'DirectDelivery') {
            this.tipeDR = 'DD';
        }

        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === 'TransitToOthers') {
            this.tipeDR = 'TO';
        }

        if (this.deliveryRequestViewModel.deliveryRequest.createdAt.getMonth() < 10) {
            this.bulanDR = '0' + (this.deliveryRequestViewModel.deliveryRequest.createdAt.getMonth() + 1);
        } else {
            this.bulanDR = (this.deliveryRequestViewModel.deliveryRequest.createdAt.getMonth() + 1).toString();
        }

        if (this.deliveryRequestViewModel.deliveryRequest.createdAt.getDate() < 10) {
            this.dateDR = '0' + this.deliveryRequestViewModel.deliveryRequest.createdAt.getDate();
        } else {
            this.dateDR = this.deliveryRequestViewModel.deliveryRequest.createdAt.getDate().toString();
        }

        this.tanggalDR = this.deliveryRequestViewModel.deliveryRequest.createdAt.getFullYear() + this.bulanDR + this.dateDR;

        if (this.kodeBranch != "" && this.tanggalDR != "" && this.tipeDR != "") {
            this.deliveryRequestService.getSequentialNumber(this.kodeBranch, this.tipeDR).then(response => {
                this.sequentialNo = response.data;
                this.deliveryRequestViewModel.deliveryRequest.deliveryRequestNumber = this.kodeBranch + '/' + this.tipeDR + '/' + this.tanggalDR + '/' + this.sequentialNo;
                this.deliveryRequestViewModel.deliveryRequest.sequentialNumber = this.sequentialNo;

            });
        } else {
            this.deliveryRequestViewModel.deliveryRequest.deliveryRequestNumber = this.kodeBranch + '/' + this.tipeDR + '/' + this.tanggalDR + '/' + this.sequentialNo;

        }
        this.deliveryRequestViewModel.deliveryRequest.branchCode = this.kodeBranch;
    }

    //Create Delivery Request
    createDeliveryRequest(form: angular.IFormController) {

        //Delivery Request - Normal
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "Normal") {
            this.deliveryRequestNormalCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestNormalCreateModel.deliveryRequest.frameNumber = this.frameNo;

            //Format tanggal menjadi string
            if (this.deliveryRequestNormalCreateModel.deliveryRequestNormal.requestedDeliveryTimeToBranch !== undefined && this.deliveryRequestNormalCreateModel.deliveryRequestNormal.requestedDeliveryTimeToBranch !== null) {
                this.deliveryRequestNormalCreateModel.deliveryRequestNormal.requestedDeliveryTimeToBranchView = moment(this.deliveryRequestNormalCreateModel.deliveryRequestNormal.requestedDeliveryTimeToBranchView).format('DD-MMM-YYYY');
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestNormalCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Perkiraan Sampai di Cabang"] = this.deliveryRequestNormalCreateModel.deliveryRequestNormal.requestedDeliveryTimeToBranchView;

            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliveryNormalRequest(this.deliveryRequestNormalCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });



        }

        //Delivery Request - Self Pick
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "SelfPick") {
            this.deliveryRequestSelfPickCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestSelfPickCreateModel.deliveryRequest.frameNumber = this.frameNo;

            if (this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.pickUpDate !== undefined && this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.pickUpDate !== null) {
                this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.pickUpDateView = moment(this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.pickUpDate).format('DD-MMM-YYYY');
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestSelfPickCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Tanggal Pick Up"] = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.pickUpDateView;
            this.jsonDeliveryRequest["Identitas Driver (Tipe)"] = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.driverType;
            this.jsonDeliveryRequest["Identitas Driver (ID)"] = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.driverId;
            this.jsonDeliveryRequest["Nama Driver"] = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.driverName;
            this.jsonDeliveryRequest["Kode Konfirmasi"] = this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick.confirmationCode;
            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliverySelfPickRequest(this.deliveryRequestSelfPickCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

        //Delivery Request - Direct Delivery
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "DirectDelivery") {
            this.deliveryRequestDirectDeliveryCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestDirectDeliveryCreateModel.deliveryRequest.frameNumber = this.frameNo;

            if (this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.estimasiPdcOut !== undefined && this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.estimasiPdcOut !== null) {
                this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.estimasiPdcOutView = moment(this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.estimasiPdcOut).format('DD-MMM-YYYY');
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Estimasi PDC Out"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.estimasiPdcOutView;
            this.jsonDeliveryRequest["Nama Customer"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.customerName;
            this.jsonDeliveryRequest["Alamat Customer"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.customerAddress;
            this.jsonDeliveryRequest["Kota Customer"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.customerCity;
            this.jsonDeliveryRequest["Nama Salesman"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.salesmanName;
            this.jsonDeliveryRequest["Contact No. Salesman"] = this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery.salesmanContactNo;

            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliveryDirectDeliveryRequest(this.deliveryRequestDirectDeliveryCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

        //Delivery Request - Transit To Others - Self Pick To Others
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "TransitToOthers" && this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType == "SelfPickToOthers") {
            this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequest.frameNumber = this.frameNo;
            this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers = this.deliveryRequestTransitToOthersModel;
            this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers = this.deliveryRequestSelfPickToOthersModel;

            if (this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.pickUpDate !== undefined &&
                this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.pickUpDate !== null) {
                this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.pickUpDateView =
                    moment(this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.pickUpDate).format('DD-MMM-YYYY');
            }

            //Ubah Lead Time Hari, Jam, Menit menjadi 0 apabila tidak diinput (undefined)
            if (this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeDay === null) {
                this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeDay = 0;
            }
            if (this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeHour === null) {
                this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeHour = 0;
            }
            if (this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeMinute === null) {
                this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeMinute = 0;
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Tipe"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.deliveryLocation.locationType;
            this.jsonDeliveryRequest["Nama"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.deliveryLocationName.locationName;
            this.jsonDeliveryRequest["Alamat"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.locationAddress;
            this.jsonDeliveryRequest["Prakiraan Lead Time"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeDay
                + " Hari " + this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeHour
                + " Jam " + this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.leadTimeMinute
                + " Menit";
            this.jsonDeliveryRequest["Tanggal Pengambilan"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers.pickUpDateView;
            this.jsonDeliveryRequest["Identitas Driver (Tipe)"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers.driverType;
            this.jsonDeliveryRequest["Identitas Driver (ID)"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers.driverId;
            this.jsonDeliveryRequest["Nama Driver"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers.driverName;
            this.jsonDeliveryRequest["Kode Konfirmasi"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers.confirmationCode;
            this.jsonDeliveryRequest["Unit ini harus dikembalikan ke PDC pada tanggal"] = this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliverySelfPickToOthers.returnPdcDateView;
            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliveryTransitToOthersSelfPickToOthersRequest(this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

        //Delivery Request - Transit To Others - Normal - Return To PDC
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "TransitToOthers" && this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType == "Normal" && this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType == "ReturnToPDC") {
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequest.frameNumber = this.frameNo;
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers = this.deliveryRequestTransitToOthersModel;
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthersNormalModel.deliveryTransitType = this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType;

            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDate !== undefined &&
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDate !== null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDateView =
                    moment(this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDate).format('DD-MMM-YYYY');
            }

            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeDay === null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeDay = 0;
            }
            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeHour === null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeHour = 0;
            }
            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeMinute === null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeMinute = 0;
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Tipe"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.deliveryLocation.locationType;
            this.jsonDeliveryRequest["Nama"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.deliveryLocationName.locationName;
            this.jsonDeliveryRequest["Alamat"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.locationAddress;
            this.jsonDeliveryRequest["Prakiraan Lead Time"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeDay
                + " Hari " + this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeHour
                + " Jam " + this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeMinute
                + " Menit";
            this.jsonDeliveryRequest["Tanggal Pengambilan"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDateView;


            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliveryTransitToOthersNormalReturnToPdcRequest(this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

        //Delivery Request - Transit To Others - Normal - Return To Others Pdc
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "TransitToOthers" && this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType == "Normal" && this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType == "ReturnToOthersPDC") {
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequest.frameNumber = this.frameNo;
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers = this.deliveryRequestTransitToOthersModel;
            this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthersNormalModel = this.deliveryRequestTransitToOthersNormalModel;

            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDate !== undefined &&
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDate !== null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDateView =
                    moment(this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDate).format('DD-MMM-YYYY');
            }

            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeDay === null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeDay = 0;
            }
            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeHour === null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeHour = 0;
            }
            if (this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeMinute === null) {
                this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeMinute = 0;
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Tipe"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.deliveryLocation.locationType;
            this.jsonDeliveryRequest["Nama"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.deliveryLocationName.locationName;
            this.jsonDeliveryRequest["Alamat"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.locationAddress;
            this.jsonDeliveryRequest["Prakiraan Lead Time"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeDay
                + " Hari " + this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeHour
                + " Jam " + this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.leadTimeMinute
                + " Menit";
            this.jsonDeliveryRequest["Tanggal Pengambilan"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthers.pickUpDateView;
            this.jsonDeliveryRequest["Return To Others PDC"] = this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel.deliveryRequestTransitToOthersNormalReturnToPdc.deliveryTransitToOthersNormalModel.deliveryOtherPdcLocation.otherPdcLocation;

            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliveryTransitToOthersNormalReturnToPdcRequest(this.deliveryRequestTransitToOthersNormalReturnToPdcCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

        //Delivery Request - Transit To Others - Normal - Self Pick From Others
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType == "TransitToOthers" && this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType == "Normal" && this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType == "SelfPickFromOthers") {
            this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequest = this.deliveryRequestViewModel.deliveryRequest;
            this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequest.frameNumber = this.frameNo;
            this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers = this.deliveryRequestTransitToOthersModel;
            this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryRequestTransitToOthersNormalModel.deliveryTransitType = this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType;
            this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryRequestTransitToOthersNormalModel.deliverySelfPickFromOthers = this.deliveryRequestSelfPickFromOtherModel;

            if (this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.pickUpDate !== undefined &&
                this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.pickUpDate !== null) {
                this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.pickUpDateView =
                    moment(this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.pickUpDate).format('DD-MMM-YYYY');
            }

            if (this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeDay === null) {
                this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeDay = 0;
            }
            if (this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeHour === null) {
                this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeHour = 0;
            }
            if (this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeMinute === null) {
                this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeMinute = 0;
            }

            this.jsonDeliveryRequest = {};
            this.jsonDeliveryRequest["Frame Number"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequest.frameNumber;
            this.jsonDeliveryRequest["Tipe"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.deliveryLocation.locationType;
            this.jsonDeliveryRequest["Nama"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.deliveryLocationName.locationName;
            this.jsonDeliveryRequest["Alamat"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.locationAddress;
            this.jsonDeliveryRequest["Prakiraan Lead Time"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeDay
                + " Hari " + this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeHour
                + " Jam " + this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.leadTimeMinute
                + " Menit";
            this.jsonDeliveryRequest["Tanggal Pengambilan"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryTransitToOthers.pickUpDateView;
            this.jsonDeliveryRequest["Identitas Driver (Tipe)"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryRequestTransitToOthersNormalModel.deliverySelfPickFromOthers.driverType;
            this.jsonDeliveryRequest["Identitas Driver (ID)"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryRequestTransitToOthersNormalModel.deliverySelfPickFromOthers.driverId;
            this.jsonDeliveryRequest["Nama Driver"] = this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel.deliveryRequestTransitToOthersNormalSelfPickFromOthers.deliveryRequestTransitToOthersNormalModel.deliverySelfPickFromOthers.driverName;

            alertify.confirm("Konfirmasi", mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonDeliveryRequest)),
                () => {
                    this.deliveryRequestService.createDeliveryTransitToOthersNormalSelfPickFromOthersRequest(this.deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.resetForm(form);
                        this.getAllDeliveryRequest();
                    }).catch(response => {
                        this.isError = response.data;

                        if (response.status == "500") {
                            alertify.error('Koneksi ke server bermasalah');
                        }

                        if (this.isError == 1) {
                            alertify.error('Data tidak valid');
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => {

                }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }


    }

    //Get Car Data by Frame Number
    getDeliveryCar() {
        this.deliveryRequestViewModel.deliveryRequestCar = _.find(this.deliveryRequestViewCarModelList, {
            'frameNumber': this.frameNo
        });
        this.generateDeliveryRequestNumber();
        this.validation();
    }

    //validasi untuk disable button & menampilkan error message
    validation() {

        //cari mobil yang CancelledAt nya NULL
        if (this.deliveryRequestViewModel.deliveryRequestCar !== undefined) {
            this.deliveryRequestModelbyVehicleIdList = _.filter(this.deliveryRequestModelList, {
                'vehicleId': this.deliveryRequestViewModel.deliveryRequestCar.vehicleId
            });
            this.deliveryRequestModel = _.find(this.deliveryRequestModelbyVehicleIdList, {
                'cancelledAt': null
            });
        }

        //Validasi Frame Number
        if (this.deliveryRequestViewModel.deliveryRequestCar === undefined && this.frameNo !== undefined) {
            this.validate = true;
            this.validateRequest = false;
            this.errorMessage = "Frame No. tidak ditemukan";
        } else if (this.deliveryRequestViewModel.deliveryRequestCar === undefined || this.frameNo === null) {
            this.validate = false;
            this.validateRequest = false;
        } else if (this.deliveryRequestModel !== undefined) {
            this.validate = true;
            this.validateRequest = false;
            this.errorMessage = "Frame No. sudah digunakan";
        } else {
            this.validate = false;
            this.validateRequest = true;
        }

        //Validasi Radio Button Method
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType !== null) {
            this.validateType = true;
        } else {
            this.validateType = false;
        }

        //cek validateRequest & validateType untuk mendapatkan validateMain
        if (this.validateRequest === true && this.validateType === true) {
            this.validateMain = true;
        } else {
            this.validateMain = false;
        }
    }

    //Get All Data untuk Delivery Request
    getAllDeliveryRequest() {
        this.deliveryRequestService.getAllDeliveryRequestPageView().then(response => {
            //DeliveryRequest
            this.deliveryRequestModelList = response.data.deliveryRequest;
            //Mobil
            this.deliveryRequestViewCarModelList = response.data.deliveryRequestCar;
            //Tipe Lokasi
            this.deliveryRequestLocationType = response.data.deliveryRequestLocationType;
            //Nama Lokasi
            this.deliveryRequestLocationName = response.data.deliveryRequestLocationName;
            //Alamat Lokasi
            this.deliveryRequestLocationAddress = response.data.deliveryRequestLocationAddress;
            //Other PDC Location
            this.deliveryRequestOtherPdcLocation = response.data.deliveryRequestOtherPdcLocation;
            //Broadcast ke component yang membutuhkan
            this.root.$broadcast('deliveryRequestLocationType', this.deliveryRequestLocationType);
            this.root.$broadcast('deliveryRequestLocationName', this.deliveryRequestLocationName);
            this.root.$broadcast('deliveryRequestLocationAddress', this.deliveryRequestLocationAddress);
            this.root.$broadcast('deliveryRequestOtherPdcLocation', this.deliveryRequestOtherPdcLocation);

        }).catch(response => {
            if (response.status == "500") {
                alertify.error('Koneksi ke server bermasalah');
            }
        });
    }

    //Reset Form
    resetForm(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.frameNo = null;
        this.validateRequest = false;
        this.validateType = false;
        this.validateDetail = false;
        this.validateTransitDetail = false;
        this.validateMain = false;
        this.validate = false;
        this.kodeBranch = "";
        this.tipeDR = "";
        this.tanggalDR = "";
        this.bulanDR = "";
        this.dateDR = "";
        this.sequentialNo = 0;
        this.deliveryRequestViewModel.deliveryRequest = new deliveryRequestService.DeliveryRequestModel();
        this.deliveryRequestViewModel.deliveryRequest.createdAt = new Date();
        this.deliveryRequestViewModel.deliveryRequestCar = new deliveryRequestService.DeliveryRequestCarModel();
        this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType = null;
        this.deliveryRequestNormalCreateModel.deliveryRequestNormal = new deliveryRequestService.DeliveryRequestNormalModel();
        this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick = new deliveryRequestService.DeliveryRequestSelfPickModel();
        this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery = new deliveryRequestService.DeliveryRequestDirectDeliveryModel();
        this.deliveryRequestTransitToOthersModel = new deliveryRequestService.DeliveryRequestTransitToOthersModel();
        this.deliveryRequestTransitToOthersModel.leadTimeDay = null;
        this.deliveryRequestTransitToOthersModel.leadTimeHour = null;
        this.deliveryRequestTransitToOthersModel.leadTimeMinute = null;
        this.deliveryRequestTransitToOthersNormalModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalModel();
        this.deliveryRequestSelfPickToOthersModel = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel();
        this.deliveryRequestSelfPickFromOtherModel = new deliveryRequestService.DeliveryRequestSelfPickFromOtherModel();
        this.root.$broadcast('deliveryRequestNormalReset', this.deliveryRequestNormalCreateModel.deliveryRequestNormal);
        this.root.$broadcast('deliveryRequestSelfPickReset', this.deliveryRequestSelfPickCreateModel.deliveryRequestSelfPick);
        this.root.$broadcast('deliveryRequestDirectDeliveryReset', this.deliveryRequestDirectDeliveryCreateModel.deliveryRequestDirectDelivery);
        this.root.$broadcast('deliveryRequestTransitToOthersReset', this.deliveryRequestTransitToOthersModel);
        this.root.$broadcast('deliveryRequestTransitToOthersNormalReset', this.deliveryRequestTransitToOthersNormalModel);
        this.root.$broadcast('deliveryRequestSelfPickToOthersReset', this.deliveryRequestSelfPickToOthersModel);
        this.root.$broadcast('deliveryRequestSelfPickFromOthersReset', this.deliveryRequestSelfPickFromOtherModel);
        this.generateDeliveryRequestNumber();
    }

    //Broadcast untuk mendapatkan Transit Return Date
    broadcast() {
        this.root.$broadcast('deliveryRequestDate', this.deliveryRequestViewModel.deliveryRequest);
    }

    //Reset Validasi untuk component Detail (Normal, Self Pick, Direct Delivery, Transit To Others) sesuai isi component tersebut (apabila sudah terpenuhi atau belum validasinya)
    resetValidateDetail() {
        //untuk method yg bukan Transit To Others
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType !== "TransitToOthers") {
            if (this.validateDetailNormal === false && this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === "Normal") {
                this.validateDetail = false;
            } else if (this.validateDetailSelfPick === false && this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === "SelfPick") {
                this.validateDetail = false;
            } else if (this.validateDetailDirectDelivery === false && this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === "DirectDelivery") {
                this.validateDetail = false;
            } else if (this.validateDetailTransitToOthers === false && this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === "TransitToOthers") {
                this.validateDetail = false;
            } else {
                this.validateDetail = true;
                this.validateTransitDetail = true;
            }
        //Untuk method Transit Detail (di Transit To Others)
        } else if (this.validateDetailSelfPickToOthers === false && this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType === "SelfPickToOthers") {
            this.validateTransitDetail = false;
        } else if (this.validateDetailSelfPickFromOthers === false && this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "SelfPickFromOthers") {
            this.validateTransitDetail = false;
        } else if (this.validateDetailTransitToOthers === false && this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType === "TransitToOthers") {
            this.validateDetail = false;
        } else if (this.validateDetailTransitNormal === false && this.deliveryRequestTransitToOthersModel.deliveryRequestTransitType === "Normal"
            && this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType !== "SelfPickFromOthers") {
            this.validateTransitDetail = false;
        } else {
            this.validateDetail = true;
            this.validateTransitDetail = true;
        }
        //Apabila bukan Transit To Others, Transit Detail dijadikan true (karena tidak ada transit Detailnya)
        if (this.deliveryRequestViewModel.deliveryRequest.deliveryRequestType !== "TransitToOthers") {
            this.validateTransitDetail = true;
        }
    }
    //DIO Item belum ada yg buat
    modulInProgress() {
        alertify.error("Modul In Progress");
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
}

let DeliveryRequestComponent = {
    controller: DeliveryRequestController,
    controllerAs: "me",
    template: require("./DeliveryRequest.html")
}

export { DeliveryRequestComponent }