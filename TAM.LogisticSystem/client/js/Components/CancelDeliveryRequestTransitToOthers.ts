import * as deliveryRequestService from '../services';
import * as _ from 'lodash';
import * as moment from 'moment';

class CancelDeliveryRequestTransitToOthersController implements angular.IController {
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
    leadTimeRemaining: number;
    timeDiff: number;
    diffDays: number;
    diffHours: number;
    diffMinutes: number;
    transitReturnDateTemp: Date;
    deliveryRequestDate: moment.Moment;
    transitReturnDate: moment.Moment;
    deliveryTransitTypeId: number;

    $onInit() {
        this.cancelDeliveryRequestViewModel = new deliveryRequestService.CancelDeliveryRequestViewModel();
        this.root.$on('cancelDeliveryRequestTransitToOthers', (event, data) => {
            this.cancelDeliveryRequestViewModel = data;
            this.deliveryTransitTypeId = this.cancelDeliveryRequestViewModel.deliveryRequestTransitTypeId;

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTransitTypeId < 4) {
                this.root.$broadcast('cancelDeliveryRequestTransitToOthersNormal', this.cancelDeliveryRequestViewModel);
            }

            if (this.cancelDeliveryRequestViewModel.deliveryRequestTransitTypeId == 4) {
                this.getLeadTime();
                this.root.$broadcast('cancelDeliveryRequestSelfPickToOthers', this.cancelDeliveryRequestViewModel);
            }       
        });
    }

    getLeadTime() {
        this.transitReturnDateTemp = new Date(this.cancelDeliveryRequestViewModel.transitReturnDate);
        this.deliveryRequestDate = moment(this.cancelDeliveryRequestViewModel.createdAt);
        this.transitReturnDate = moment(this.transitReturnDateTemp);
        this.diffDays = this.transitReturnDate.diff(this.deliveryRequestDate, 'days');
        this.transitReturnDateTemp.setDate(this.transitReturnDateTemp.getDate() - this.diffDays);
        this.transitReturnDate = moment(this.transitReturnDateTemp);
        this.diffHours = this.transitReturnDate.diff(this.deliveryRequestDate, 'hours');
        this.transitReturnDateTemp.setHours(this.transitReturnDateTemp.getHours() - this.diffHours);
        this.transitReturnDate = moment(this.transitReturnDateTemp);
        this.diffMinutes = this.transitReturnDate.diff(this.deliveryRequestDate, 'minutes');
        this.cancelDeliveryRequestViewModel.leadTimeDay = this.diffDays;
        this.cancelDeliveryRequestViewModel.leadTimeHour = this.diffHours;
        this.cancelDeliveryRequestViewModel.leadTimeMinute = this.diffMinutes;
    }
}

let CancelDeliveryRequestTransitToOthersComponent = {
    controller: CancelDeliveryRequestTransitToOthersController,
    controllerAs: "me",
    template: require("./CancelDeliveryRequestTransitToOthers.html")
}

export { CancelDeliveryRequestTransitToOthersComponent }