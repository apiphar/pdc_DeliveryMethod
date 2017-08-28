import * as deliveryRequestService from '../services';
import * as _ from 'lodash';

class CancelDeliveryRequestTransitToOthersController implements angular.IController {
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
        this.root.$on('cancelDeliveryRequestSelfPickToOthers', (event, data) => {
            this.cancelDeliveryRequestViewModel = data;
            console.log(this.cancelDeliveryRequestViewModel);
            console.log(this.cancelDeliveryRequestViewModel.pickUpIdentityIsKtp);
            this.driverType = "KTP";
            if (this.cancelDeliveryRequestViewModel.pickUpIdentityIsKtp == false) {
                this.driverType = "SIM";
            }
        });

    }


}

let CancelDeliveryRequestTransitToOthersSelfPickToOthersComponent = {
    controller: CancelDeliveryRequestTransitToOthersController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequestTransitToOthersSelfPickToOthers.html")
}

export { CancelDeliveryRequestTransitToOthersSelfPickToOthersComponent }