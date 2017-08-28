import * as deliveryRequestService from '../services';

class DeliveryRequestTransitToOthersSelfPickFromOthersController implements angular.IController {
    static $inject = ["$rootScope", "DeliveryRequestService"];

    root: angular.IRootScopeService;

    constructor(root: angular.IRootScopeService, deliveryRequestService: deliveryRequestService.DeliveryRequestService) {
        this.root = root;
        this.deliveryRequestService = deliveryRequestService;
    }

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    deliveryRequestSelfPickFromOtherModel: deliveryRequestService.DeliveryRequestSelfPickFromOtherModel;
    driverType: string[];
    validateIdentitasDriver: boolean = false;
    validateIdentitasDriverDetail: boolean = true;
    validateDriverName: boolean = false;
    validateDriverNameDetail: boolean = true;
    errorMsgIdentitasDriver: string = "";
    errorMsgDriverName: string = "";

    $onInit() {
        this.deliveryRequestSelfPickFromOtherModel = new deliveryRequestService.DeliveryRequestSelfPickFromOtherModel();
        this.driverType = new Array();
        this.driverType.push("KTP");
        this.driverType.push("SIM");
        //Reset Form
        this.root.$on('deliveryRequestSelfPickFromOthersReset', (event, data) => {
            this.deliveryRequestSelfPickFromOtherModel = data;
            this.validateIdentitasDriver = false;
            this.validateIdentitasDriverDetail = true;
            this.validateDriverName = false;
            this.validateDriverNameDetail = true;
            this.root.$broadcast('deliveryRequestTransitToOthersSelfPickFromOthers', this.deliveryRequestSelfPickFromOtherModel);

        });
    }

    validationIdentitasDriver() {
        if (this.deliveryRequestSelfPickFromOtherModel.driverId === "" || this.deliveryRequestSelfPickFromOtherModel.driverType === undefined) {
            this.validateIdentitasDriver = true;
            this.validateIdentitasDriverDetail = true;
            this.errorMsgIdentitasDriver = "Identitas Driver harus dipilih atau diisi";
        } else if (!this.deliveryRequestSelfPickFromOtherModel.driverId.match(/^[\d]+$/)) {
            this.validateIdentitasDriver = true;
            this.validateIdentitasDriverDetail = true;
            this.errorMsgIdentitasDriver = "Identitas Driver harus berformat numeric";
        } else if (this.deliveryRequestSelfPickFromOtherModel.driverId.length > 32) {
            this.validateIdentitasDriver = true;
            this.validateIdentitasDriverDetail = true;
            this.errorMsgIdentitasDriver = "Identitas Driver tidak boleh > 32 karakter";
        } else {
            this.validateIdentitasDriver = false;
            this.validateIdentitasDriverDetail = false;
        }
    }

    validationDriverName() {
        if (this.deliveryRequestSelfPickFromOtherModel.driverName === "") {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMsgDriverName = "Nama Driver harus diisi";
        } else if (!this.deliveryRequestSelfPickFromOtherModel.driverName.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateDriverName = true;
            this.validateDriverNameDetail = true;
            this.errorMsgDriverName = "Nama Driver harus berformat alphanumeric";
        } else if (this.deliveryRequestSelfPickFromOtherModel.driverName.length > 255) {
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

        if (this.validateDriverNameDetail === false && this.validateIdentitasDriverDetail === false) {
            this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail = true;
        } else {
            this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail = false;
        }

        this.root.$broadcast('deliveryRequestTransitToOthersSelfPickFromOthers', this.deliveryRequestSelfPickFromOtherModel);
    }

    
}

let DeliveryRequestTransitToOthersSelfPickFromOthersComponent = {
    controller: DeliveryRequestTransitToOthersSelfPickFromOthersController,
    controllerAs: "me",
    template: require("./DeliveryRequestTransitToOthersSelfPickFromOthers.html")
}

export { DeliveryRequestTransitToOthersSelfPickFromOthersComponent }