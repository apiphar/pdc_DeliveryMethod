import * as deliveryRequestService from '../services';

class DeliveryRequestTransitToOthersNormalController implements angular.IController {
    static $inject = ["$rootScope", "DeliveryRequestService"];

    root: angular.IRootScopeService;

    constructor(root: angular.IRootScopeService, deliveryRequestService: deliveryRequestService.DeliveryRequestService) {
        this.root = root;
        this.deliveryRequestService = deliveryRequestService;
    }

    deliveryRequestSelfPickFromOtherModel: deliveryRequestService.DeliveryRequestSelfPickFromOtherModel;
    deliveryRequestSelfPickToOtherModel: deliveryRequestService.DeliveryRequestSelfPickToOthersModel;

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    deliveryRequestTransitToOthersNormalModel: deliveryRequestService.DeliveryRequestTransitToOthersNormalModel;
    deliveryOtherPdcLocation: deliveryRequestService.DeliveryRequestOtherPdcLocationModel[];
    validate: boolean = false;
    validateDetail: boolean = false;
    errorMsg: string = "";
    count: number = 1;

    $onInit() {
        this.deliveryRequestTransitToOthersNormalModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalModel();
        this.deliveryRequestSelfPickFromOtherModel = new deliveryRequestService.DeliveryRequestSelfPickFromOtherModel();
        this.deliveryRequestSelfPickToOtherModel = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel();
        this.broadcast();

        //get hasil validasi dr Self Pick From Others (karena child dr Transit To Others - Normal hanya Self Pick From Others saja)
        this.root.$on('deliveryRequestTransitToOthersSelfPickFromOthers', (event, data) => {
            this.deliveryRequestSelfPickFromOtherModel = data;
            if (this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail === undefined) {
                this.deliveryRequestTransitToOthersNormalModel.validateDetailSelfPickFromOthers = false;

            } else {
                this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail;
                this.deliveryRequestTransitToOthersNormalModel.validateDetailSelfPickFromOthers = this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail;
            }
        });

        //Reset Form
        this.root.$on('deliveryRequestTransitToOthersNormalReset', (event, data) => {
            this.deliveryRequestTransitToOthersNormalModel = data;
            this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = false;
            this.validate = false;
            this.validateDetail = false;
            this.root.$broadcast('deliveryRequestTransitToOthersNormal', this.deliveryRequestTransitToOthersNormalModel);
        });

        //get data untuk combo box
        this.root.$on('deliveryRequestOtherPdcLocation', (event, data) => {
            this.deliveryOtherPdcLocation = data;
        });
    }

    validation() {
        if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "ReturnToPDC") {
            this.validate = false;
            this.validateDetail = false;
        } else if (this.deliveryRequestTransitToOthersNormalModel.deliveryOtherPdcLocation === undefined) {
            this.deliveryRequestTransitToOthersNormalModel.deliveryOtherPdcLocation = new deliveryRequestService.DeliveryRequestOtherPdcLocationModel();
            this.validate = true;
            this.validateDetail = true;
            this.count++;
        } else if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "ReturnToOthersPDC"
            && this.deliveryRequestTransitToOthersNormalModel.deliveryOtherPdcLocation.otherPdcLocation === undefined
            && this.count > 1) {
            this.validate = true;
            this.validateDetail = true;
            this.errorMsg = "Return To Others PDC harus dipilih";
        } else {
            this.validate = false;
            this.validateDetail = false;
        }
    }

    //Broadcast
    broadcast() {
        if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "ReturnToPDC"
            && this.validateDetail === false) {
            this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = true;
        } else if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "ReturnToOthersPDC"
            && this.validateDetail === false) {
            this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = true;
        } else if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "SelfPickFromOthers"
            && this.deliveryRequestTransitToOthersNormalModel.validateDetailSelfPickFromOthers === false) {
            this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = false;
        } else if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType === "SelfPickFromOthers"
            && this.deliveryRequestTransitToOthersNormalModel.validateDetailSelfPickFromOthers === true){
            this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = true;
        } else {
            this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail = false;
        }
        this.root.$broadcast('deliveryRequestTransitToOthersNormal', this.deliveryRequestTransitToOthersNormalModel);
    }



}

let DeliveryRequestTransitToOthersNormalComponent = {
    controller: DeliveryRequestTransitToOthersNormalController,
    controllerAs: "me",
    template: require("./DeliveryRequestTransitToOthersNormal.html")
}

export { DeliveryRequestTransitToOthersNormalComponent }