import * as deliveryRequestService from '../services';
import * as _ from 'lodash';

class CancelDeliveryRequestSelfPickController implements angular.IController {
    static $inject = ["CancelDeliveryRequestService", "$rootScope"];

    constructor(cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService, root: angular.IRootScopeService) {
        this.cancelDeliveryRequestService = cancelDeliveryRequestService;
        this.root = root;
    }



    root: angular.IRootScopeService;

    cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService;
    cancelDeliveryRequestViewModel: deliveryRequestService.CancelDeliveryRequestViewModel;

    $onInit() {
        this.cancelDeliveryRequestViewModel = new deliveryRequestService.CancelDeliveryRequestViewModel();

        this.root.$on('cancelDeliveryRequestSelfPick', (event, data) => {
            this.cancelDeliveryRequestViewModel = data;
            console.log(this.cancelDeliveryRequestViewModel);
            console.log(this.cancelDeliveryRequestViewModel.pickUpIdentityIsKtp);
            if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 2 && this.cancelDeliveryRequestViewModel.pickUpIdentityIsKtp == true) {
                this.cancelDeliveryRequestViewModel.driverType = "KTP";
            } else if (this.cancelDeliveryRequestViewModel.deliveryRequestTypeId == 2 && this.cancelDeliveryRequestViewModel.pickUpIdentityIsKtp == false) {
                this.cancelDeliveryRequestViewModel.driverType = "SIM";
            }

        });

    }
}

let CancelDeliveryRequestSelfPickComponent = {
    controller: CancelDeliveryRequestSelfPickController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequestSelfPick.html")
}

export { CancelDeliveryRequestSelfPickComponent }