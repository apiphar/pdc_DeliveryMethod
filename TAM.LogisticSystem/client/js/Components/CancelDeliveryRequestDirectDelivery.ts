import * as deliveryRequestService from '../services';
import * as _ from 'lodash';

class CancelDeliveryRequestDirectDeliveryController implements angular.IController {
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

        this.root.$on('cancelDeliveryRequestDirectDelivery', (event, data) => {
            this.cancelDeliveryRequestViewModel = data;
        });

    }
}

let CancelDeliveryRequestDirectDeliveryComponent = {
    controller: CancelDeliveryRequestDirectDeliveryController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequestDirectDelivery.html")
}

export { CancelDeliveryRequestDirectDeliveryComponent}