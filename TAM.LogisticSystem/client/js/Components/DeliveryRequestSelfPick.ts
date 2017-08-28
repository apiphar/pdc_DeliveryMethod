import * as deliveryRequestService from '../services';
import * as moment from 'moment';

class DeliveryRequestSelfPickController implements angular.IController {
    static $inject = ["$rootScope", "DeliveryRequestService"];

    root: angular.IRootScopeService;

    constructor(root: angular.IRootScopeService, deliveryRequestService: deliveryRequestService.DeliveryRequestService) {
        this.root = root;
        this.deliveryRequestService = deliveryRequestService;
    }
    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    deliveryRequestSelfPick: deliveryRequestService.DeliveryRequestSelfPickModel;
    confirmationCode: string;
    driverType: string[];
    validateDriverId: boolean = false;
    validateDriverIdDetail: boolean = true;
    validateDriverName: boolean = false;
    validateDriverNameDetail: boolean = true;
    validatePickUpDate: boolean = false;
    validatePickUpDateDetail: boolean = true;
    errorMessagePickUpDate: string = "";
    errorMessageDriverId: string = "";
    errorMessageDriverName: string = "";
    popup1: deliveryRequestService.PopUp;
    today: Date = moment().toDate();

    $onInit() {
        this.deliveryRequestSelfPick = new deliveryRequestService.DeliveryRequestSelfPickModel();
        this.today.setHours(0);
        this.today.setMinutes(0);
        this.today.setSeconds(0);
        this.today.setMilliseconds(0);
        this.driverType = new Array();
        this.driverType.push("KTP");
        this.driverType.push("SIM");
        this.generateConfirmationCode();
        this.popup1 = new deliveryRequestService.PopUp();
        //Reset Form
        this.root.$on('deliveryRequestSelfPickReset', (event, data) => {
            this.deliveryRequestSelfPick = data;
            this.deliveryRequestSelfPick = new deliveryRequestService.DeliveryRequestSelfPickModel();
            this.deliveryRequestSelfPick.confirmationCode = this.confirmationCode;
            this.deliveryRequestSelfPick.validateDetail = false;
            this.validateDriverId = false;
            this.validateDriverIdDetail = true;
            this.validateDriverName = false;
            this.validateDriverNameDetail = true;
            this.validatePickUpDate = false;
            this.validatePickUpDateDetail = true;
            this.root.$broadcast('deliveryRequestSelfPick', this.deliveryRequestSelfPick); 
        });
    }

    //Untuk membuka DatePicker Popup
    open1() {
        this.popup1.opened = true;
    }

    //Generate Confirmation Code
    generateConfirmationCode() {
        this.deliveryRequestService.getConfirmationCode().then(response => {
            this.deliveryRequestSelfPick.confirmationCode = response.data;
            this.confirmationCode = this.deliveryRequestSelfPick.confirmationCode;
        });
    }

    validationPickUpDate() {
        if (this.deliveryRequestSelfPick.pickUpDate === null) {
            this.validatePickUpDate = true;
            this.validatePickUpDateDetail = true;
            this.errorMessagePickUpDate = "Tanggal Pick Up harus diisi";
        } else if (this.deliveryRequestSelfPick.pickUpDate < this.today) {
            this.validatePickUpDate = true;
            this.validatePickUpDateDetail = true;
            this.errorMessagePickUpDate = "Tanggal Pick Up tidak boleh lebih kecil dari tanggal hari ini";
        } else {
            this.validatePickUpDate = false;
            this.validatePickUpDateDetail = false;
        }
    }

    validationIdentitasDriver() {
        if (this.deliveryRequestSelfPick.driverId === "" || this.deliveryRequestSelfPick.driverType === undefined) {
            this.validateDriverId = true;
            this.validateDriverIdDetail = true;
            this.errorMessageDriverId = "Identitas Driver harus dipilih atau diisi";
        } else if (!this.deliveryRequestSelfPick.driverId.match(/^[\d]+$/)) {
            this.validateDriverId = true;
            this.validateDriverIdDetail = true;
            this.errorMessageDriverId = "Identitas Driver harus berformat numeric";
        } else if (this.deliveryRequestSelfPick.driverId.length > 32) {
            this.validateDriverId = true;
            this.validateDriverIdDetail = true;
            this.errorMessageDriverId = "Identitas Driver tidak boleh > 32 karakter";
        } else {
            this.validateDriverId = false;
            this.validateDriverIdDetail = false;
        }
    }
    validationDriverName() {
        if (this.deliveryRequestSelfPick.driverName === "") {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMessageDriverName = "Nama Driver harus diisi";
        } else if (!this.deliveryRequestSelfPick.driverName.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMessageDriverName = "Nama Driver harus berformat alphanumeric";
        } else if (this.deliveryRequestSelfPick.driverName.length > 255) {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMessageDriverName = "Nama Driver tidak boleh > 255 karakter";
        } else {
            this.validateDriverName = false;
            this.validateDriverNameDetail = false;
        }
    }

    //Broadcast
    broadcast() {
        if (this.validateDriverIdDetail == false && this.validateDriverNameDetail == false && this.validatePickUpDateDetail == false) {
            this.deliveryRequestSelfPick.validateDetail = true;
        } else {
            this.deliveryRequestSelfPick.validateDetail = false;
        }
        this.root.$broadcast('deliveryRequestSelfPick', this.deliveryRequestSelfPick); 
    }
}

let DeliveryRequestSelfPickComponent = {
    controller: DeliveryRequestSelfPickController,
    controllerAs: "me",
    template: require("./DeliveryRequestSelfPick.html")
}

export { DeliveryRequestSelfPickComponent }