import * as deliveryRequestService from '../services';
import * as moment from 'moment';

class DeliveryRequestTransitToOthersSelfPickToOthersController implements angular.IController {
    static $inject = ["DeliveryRequestService", "$rootScope"];


    constructor(deliveryRequestService: deliveryRequestService.DeliveryRequestService, root: angular.IRootScopeService) {
        this.deliveryRequestService = deliveryRequestService;
        this.root = root;
    }

    root: angular.IRootScopeService;

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    deliveryRequestSelfPickToOthers: deliveryRequestService.DeliveryRequestSelfPickToOthersModel;
    deliveryRequestTransitToOthersModel: deliveryRequestService.DeliveryRequestTransitToOthersModel;
    deliveryRequestTransitToOthersSelfPickToOthersCreateModel: deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersCreateModel;
    deliveryRequest: deliveryRequestService.DeliveryRequestModel;
    returnPdcDateModel: deliveryRequestService.ReturnPdcDateModel;
    confirmationCode: string;
    leadTime: number;
    driverType: string[];
    validateIdentitasDriver: boolean = false;
    validateIdentitasDriverDetail: boolean = true;
    validateDriverName: boolean = false;
    validateDriverNameDetail: boolean = true;
    errorMsgIdentitasDriver: string = "";
    errorMsgDriverName: string = "";

    $onInit() {
        this.deliveryRequestSelfPickToOthers = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel();
        this.deliveryRequestSelfPickToOthers.validateTransitDetail = false;
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel = new deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersCreateModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequest = new deliveryRequestService.DeliveryRequestModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequest.createdAt = new Date();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers = new deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersModel();
        this.deliveryRequestTransitToOthersSelfPickToOthersCreateModel.deliveryRequestTransitToOthersSelfPickToOthers.deliveryTransitToOthers = new deliveryRequestService.DeliveryRequestTransitToOthersModel();

        this.driverType = new Array();
        this.driverType.push("KTP");
        this.driverType.push("SIM");
        this.deliveryRequestTransitToOthersModel = new deliveryRequestService.DeliveryRequestTransitToOthersModel();
        this.deliveryRequest = new deliveryRequestService.DeliveryRequestModel();
        this.returnPdcDateModel = new deliveryRequestService.ReturnPdcDateModel();
        this.root.$on('deliveryRequestTransitToOthers', (event, data) => {
            this.deliveryRequestTransitToOthersModel = data;
            this.generateReturnPdcDate();
        });

        this.root.$on('deliveryRequestDate', (event, data) => {
            this.deliveryRequest = data;
        });

        this.root.$on('deliveryRequestSelfPickToOthersReset', (event, data) => {
            this.deliveryRequestSelfPickToOthers = data;
            this.deliveryRequestSelfPickToOthers = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel();
            this.deliveryRequestSelfPickToOthers.confirmationCode = this.confirmationCode;
            this.deliveryRequestSelfPickToOthers.validateTransitDetail = false;
            this.validateIdentitasDriver = false;
            this.validateIdentitasDriverDetail = true;
            this.validateDriverName = false;
            this.validateDriverNameDetail = true;
            this.generateReturnPdcDate();
            this.root.$broadcast('deliveryRequestSelfPickToOthers', this.deliveryRequestSelfPickToOthers);
        });
        this.generateConfirmationCode();
    }

    //Generate Confirmation Code
    generateConfirmationCode() {
        this.deliveryRequestService.getConfirmationCode().then(response => {
            this.deliveryRequestSelfPickToOthers.confirmationCode = response.data;
            this.confirmationCode = this.deliveryRequestSelfPickToOthers.confirmationCode;
        });
        this.root.$broadcast('deliveryRequestSelfPickToOthers', this.deliveryRequestSelfPickToOthers);
    }

    //Generate Return Pdc Date
    generateReturnPdcDate() {
        this.returnPdcDateModel.date = this.deliveryRequest.createdAt
        this.returnPdcDateModel.leadTimeDay = this.deliveryRequestTransitToOthersModel.leadTimeDay;
        this.returnPdcDateModel.leadTimeHour = this.deliveryRequestTransitToOthersModel.leadTimeHour;
        this.returnPdcDateModel.leadTimeMinute = this.deliveryRequestTransitToOthersModel.leadTimeMinute;
        this.deliveryRequestService.getReturnPdcDate(this.returnPdcDateModel).then(response2 => {
            this.deliveryRequestSelfPickToOthers.returnPdcDate = response2.data;
            this.deliveryRequestSelfPickToOthers.returnPdcDateView = moment(this.deliveryRequestSelfPickToOthers.returnPdcDate).format('DD MMM YYYY - HH:mm');
        });

        this.root.$broadcast('deliveryRequestSelfPickToOthers', this.deliveryRequestSelfPickToOthers);
    }

    validationIdentitasDriver() {
        if (this.deliveryRequestSelfPickToOthers.driverId === "" || this.deliveryRequestSelfPickToOthers.driverType === undefined) {
            this.validateIdentitasDriver = true;
            this.validateIdentitasDriverDetail = true;
            this.errorMsgIdentitasDriver = "Identitas Driver harus dipilih atau diisi";
        } else if (!this.deliveryRequestSelfPickToOthers.driverId.match(/^[\d]+$/)) {
            this.validateIdentitasDriver = true;
            this.validateIdentitasDriverDetail = true;
            this.errorMsgIdentitasDriver = "Identitas Driver harus berformat numeric";
        } else if (this.deliveryRequestSelfPickToOthers.driverId.length > 32) {
            this.validateIdentitasDriver = true;
            this.validateIdentitasDriverDetail = true;
            this.errorMsgIdentitasDriver = "Identitas Driver tidak boleh > 32 karakter";
        } else {
            this.validateIdentitasDriver = false;
            this.validateIdentitasDriverDetail = false;
        }
    }

    validationDriverName() {
        if (this.deliveryRequestSelfPickToOthers.driverName === "") {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMsgDriverName = "Nama Driver harus diisi";
        } else if (!this.deliveryRequestSelfPickToOthers.driverName.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMsgDriverName = "Nama Driver harus berformat alphanumeric";
        } else if (this.deliveryRequestSelfPickToOthers.driverName.length > 255) {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMsgDriverName = "Nama Driver tidak boleh > 255 karakter";
        } else {
            this.validateDriverName = false;
            this.validateDriverNameDetail = false;
        }
    }

    //Broadcast
    broadcast() {
        if (this.validateIdentitasDriverDetail === false && this.validateDriverNameDetail === false) {
            this.deliveryRequestSelfPickToOthers.validateTransitDetail = true;
        } else {
            this.deliveryRequestSelfPickToOthers.validateTransitDetail = false;
        }
        this.root.$broadcast('deliveryRequestSelfPickToOthers', this.deliveryRequestSelfPickToOthers);
    }


}

let DeliveryRequestTransitToOthersSelfPickToOthersComponent = {
    controller: DeliveryRequestTransitToOthersSelfPickToOthersController,
    controllerAs: "me",
    template: require("./DeliveryRequestTransitToOthersSelfPickToOthers.html")
}

export { DeliveryRequestTransitToOthersSelfPickToOthersComponent }