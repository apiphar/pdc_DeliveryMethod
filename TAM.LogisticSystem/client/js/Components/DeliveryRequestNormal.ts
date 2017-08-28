import * as deliveryRequestService from '../services';
import * as moment from 'moment';

class DeliveryRequestNormalController implements angular.IController {

    static $inject = ["$rootScope", "DeliveryRequestService"];

    root: angular.IRootScopeService;

    constructor(root: angular.IRootScopeService, deliveryRequestService: deliveryRequestService.DeliveryRequestService) {
        this.root = root;
        this.deliveryRequestService = deliveryRequestService;
    }

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    popup1: deliveryRequestService.PopUp;
    validate: boolean = false;
    validateDetail: boolean = false;
    errorMessage: string = "";
    today: Date = moment().toDate();
   

    $onInit() {
        this.deliveryRequestNormal = new deliveryRequestService.DeliveryRequestNormalModel();
        this.popup1 = new deliveryRequestService.PopUp();
        this.today.setHours(0);
        this.today.setMinutes(0);
        this.today.setSeconds(0);
        this.today.setMilliseconds(0);
        //Reset Form
        this.root.$on('deliveryRequestNormalReset', (event, data) => {
            this.deliveryRequestNormal = data;
            this.deliveryRequestNormal.validateDetail = false;
            this.validate = false;
            this.root.$broadcast('deliveryRequestNormal', this.deliveryRequestNormal);
        });
    }

    //Untuk membuka DatePicker Popup
    open1() {
        this.popup1.opened = true;
    }

    validation() {
        if (this.deliveryRequestNormal.requestedDeliveryTimeToBranch === null) {
            this.validate = true;
            this.validateDetail = true;
            this.errorMessage = "Perkiraan Sampai di Cabang harus diisi";
        } else if (this.deliveryRequestNormal.requestedDeliveryTimeToBranch < this.today) {
            this.validate = true;
            this.validateDetail = true;
            this.errorMessage = "Perkiraan Sampai di Cabang tidak boleh lebih kecil dari tanggal hari ini";
        }else {
            this.validate = false;
            this.validateDetail = false;
        }
    }

    //Broadcast
    broadcast() {
        if (this.validate === false) {
            this.deliveryRequestNormal.validateDetail = true;
        } else {
            this.deliveryRequestNormal.validateDetail = false;
        }

        this.root.$broadcast('deliveryRequestNormal', this.deliveryRequestNormal);
    }

    deliveryRequestNormal: deliveryRequestService.DeliveryRequestNormalModel;
}

let DeliveryRequestNormalComponent = {
    controller: DeliveryRequestNormalController,
    controllerAs: "me",
    template: require("./DeliveryRequestNormal.html")
}

export { DeliveryRequestNormalComponent }