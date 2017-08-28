import * as deliveryRequestService from '../services';
import * as _ from 'lodash';


class CancelDeliveryRequestTransitToOthersNormalController implements angular.IController {
    static $inject = ["CancelDeliveryRequestService", "$rootScope"];

    constructor(cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService, root: angular.IRootScopeService) {
        this.cancelDeliveryRequestService = cancelDeliveryRequestService;
        this.root = root;
    }

    root: angular.IRootScopeService;

    cancelDeliveryRequestService: deliveryRequestService.CancelDeliveryRequestService;
    cancelDeliveryRequestViewModel: deliveryRequestService.CancelDeliveryRequestViewModel;
    cancelDeliveryRequestLocationList: deliveryRequestService.CancelDeliveryRequestLocationModel[];
    cancelDeliveryRequestLocationModel: deliveryRequestService.CancelDeliveryRequestLocationModel;

    $onInit() {
        this.cancelDeliveryRequestViewModel = new deliveryRequestService.CancelDeliveryRequestViewModel();
        this.root.$on('cancelDeliveryRequestTransitToOthersNormal', (event, data) => {
            this.cancelDeliveryRequestViewModel = data;

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTransitTypeId == 3) {
                this.root.$broadcast('cancelDeliveryRequestSelfPickFromOthers', this.cancelDeliveryRequestViewModel);
            }
        });
    }
}

let CancelDeliveryRequestTransitToOthersNormalComponent = {
    controller: CancelDeliveryRequestTransitToOthersNormalController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequestTransitToOthersNormal.html")
}

export { CancelDeliveryRequestTransitToOthersNormalComponent }