import * as deliveryRequestService from '../services';
import * as _ from 'lodash';

class CancelDeliveryRequestSelfPickFromOthersController implements angular.IController {
    static $inject = ["CancelDeliveryRequestService", "$rootScope"];

    constructor(cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService, root: angular.IRootScopeService) {
        this.cancelDeliveryRequestService = cancelDeliveryRequestService;
        this.root = root;
    }

    root: angular.IRootScopeService;

    cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService;
    cancelDeliveryRequestViewModel: deliveryRequestService.CancelDeliveryRequestViewModel;
    driverType: string;

    $onInit() {
        this.cancelDeliveryRequestViewModel = new deliveryRequestService.CancelDeliveryRequestViewModel();
        this.root.$on('cancelDeliveryRequestSelfPickFromOthers', (event, data) => {
            this.cancelDeliveryRequestViewModel = data;
            this.driverType = "KTP";
            if (this.cancelDeliveryRequestViewModel.pickUpIdentityIsKtp == false) {
                this.driverType = "SIM";
            }
        });
    }

}

let CancelDeliveryRequestTransitToOthersSelfPickFromOthersComponent = {
    controller: CancelDeliveryRequestSelfPickFromOthersController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequestTransitToOthersSelfPickFromOthers.html")
}

export { CancelDeliveryRequestTransitToOthersSelfPickFromOthersComponent }