import * as deliveryRequestService from '../services';
import * as _ from 'lodash';
import * as moment from 'moment';

class DeliveryRequestTransitToOthersController implements angular.IController {
    static $inject = ["$rootScope", "DeliveryRequestService"];

    root: angular.IRootScopeService;
    ScopedCredential: angular.IScope;



    constructor(root: angular.IRootScopeService, deliveryRequestService: deliveryRequestService.DeliveryRequestService) {
        this.root = root;
        this.deliveryRequestService = deliveryRequestService;

    }

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    deliveryRequestTransitToOthers: deliveryRequestService.DeliveryRequestTransitToOthersModel;
    deliveryRequestTransitToOthersSelfPickToOthersCreateModel: deliveryRequestService.DeliveryRequestTransitToOthersSelfPickToOthersCreateModel;
    deliveryLocationType: deliveryRequestService.DeliveryRequestLocationModel[];
    deliveryLocationName: deliveryRequestService.DeliveryRequestLocationNameModel[];
    deliveryLocationNameList: deliveryRequestService.DeliveryRequestLocationNameModel[];
    deliveryLocationAddressList: deliveryRequestService.DeliveryRequestTransitToOthersModel[];
    deliveryRequest: deliveryRequestService.DeliveryRequestModel;
    deliveryRequestSelfPickToOthersModel: deliveryRequestService.DeliveryRequestSelfPickToOthersModel;
    deliveryRequestTransitToOthersNormalModel: deliveryRequestService.DeliveryRequestTransitToOthersNormalModel;
    deliveryRequestSelfPickFromOtherModel: deliveryRequestService.DeliveryRequestSelfPickFromOtherModel;
    popup1: deliveryRequestService.PopUp;
    validateTipe: boolean = false;
    validateTipeDetail: boolean = true;
    validateNama: boolean = false;
    validateNamaDetail: boolean = true;
    validateTransitDetail: boolean = false;
    validateDetailSelfPickToOthers: boolean = false;
    validateDetailTransitNormal: boolean = false;
    validateLeadTime: boolean = false;
    validateLeadTimeDetail: boolean = true;
    validatePickUpDate: boolean = false;
    validatePickUpDateDetail: boolean = true;
    errorMsgTipe: string = "";
    errorMsgNama: string = "";
    errorMsgAlamat: string = "";
    errorMsgLeadTime: string = "";
    errorMsgPickUpDate: string = "";
    leadTimeDay: string = "";
    leadTimeHour: string = "";
    leadTimeMinute: string = "";
    today: Date = moment().toDate();

    $onInit() {
        this.deliveryRequestTransitToOthers = new deliveryRequestService.DeliveryRequestTransitToOthersModel();
        this.deliveryRequestTransitToOthers.leadTimeDay = null;
        this.deliveryRequestTransitToOthers.leadTimeHour = null;
        this.deliveryRequestTransitToOthers.leadTimeMinute = null;
        this.deliveryRequestSelfPickToOthersModel = new deliveryRequestService.DeliveryRequestSelfPickToOthersModel;
        this.deliveryRequestTransitToOthersNormalModel = new deliveryRequestService.DeliveryRequestTransitToOthersNormalModel;
        this.deliveryRequestSelfPickFromOtherModel = new deliveryRequestService.DeliveryRequestSelfPickFromOtherModel;
        this.deliveryRequest = new deliveryRequestService.DeliveryRequestModel();
        this.popup1 = new deliveryRequestService.PopUp();
        this.today.setHours(0);
        this.today.setMinutes(0);
        this.today.setSeconds(0);
        this.today.setMilliseconds(0);

        //get hasil validasi dr component Transit Detail 
        this.root.$on('deliveryRequestSelfPickToOthers', (event, data) => {
            this.deliveryRequestSelfPickToOthersModel = data;
            if (this.deliveryRequestTransitToOthers.deliveryRequestTransitType === "SelfPickToOthers") {
                this.deliveryRequestTransitToOthers.validateTransitDetail = this.deliveryRequestSelfPickToOthersModel.validateTransitDetail;
            }
            this.deliveryRequestTransitToOthers.validateDetailSelfPickToOthers = this.deliveryRequestSelfPickToOthersModel.validateTransitDetail;
        });

        this.root.$on('deliveryRequestTransitToOthersNormal', (event, data) => {
            this.deliveryRequestTransitToOthersNormalModel = data;
            if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType !== "SelfPickFromOthers") {
                this.deliveryRequestTransitToOthers.validateTransitDetail = this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail;
            }
            this.deliveryRequestTransitToOthers.validateDetailTransitNormal = this.deliveryRequestTransitToOthersNormalModel.validateTransitDetail;
        });

        this.root.$on('deliveryRequestTransitToOthersSelfPickFromOthers', (event, data) => {
            this.deliveryRequestSelfPickFromOtherModel = data;
            if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType !== undefined){
                this.deliveryRequestTransitToOthers.validateTransitDetail = this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail;
            }
            this.deliveryRequestTransitToOthers.validateDetailSelfPickFromOthers = this.deliveryRequestSelfPickFromOtherModel.validateTransitDetail;

        });

        //get data untuk combo box
        this.root.$on('deliveryRequestLocationType', (event, data) => {
            this.deliveryLocationType = data;
        });

        this.root.$on('deliveryRequestLocationName', (event, data) => {
            this.deliveryLocationNameList = data;
        });

        this.root.$on('deliveryRequestLocationAddress', (event, data) => {
            this.deliveryLocationAddressList = data;
        });

        //Reset Form
        this.root.$on('deliveryRequestTransitToOthersReset', (event, data) => {
            this.deliveryRequestTransitToOthers = data;
            this.deliveryRequestTransitToOthers.validateDetail = false;
            this.deliveryRequestTransitToOthers.validateTransitDetail = false;
            this.validateTipe = false;
            this.validateTipeDetail = true;
            this.validateNama = false;
            this.validateNamaDetail = true;
            this.validateTransitDetail = false;
            this.validateDetailSelfPickToOthers = false;
            this.validateDetailTransitNormal = false;
            this.validateLeadTime = false;
            this.validateLeadTimeDetail = true;
            this.validatePickUpDate = false;
            this.validatePickUpDateDetail = true;
            this.root.$broadcast('deliveryRequestTransitToOthers', this.deliveryRequestTransitToOthers);
        });
    }

    //Untuk membuka DatePicker Popup
    open1() {
        this.popup1.opened = true;
    }

    //Get Location Name by Location Type
    getLocationName() {
        this.deliveryLocationName = _.filter(this.deliveryLocationNameList, {
            'locationTypeCode': this.deliveryRequestTransitToOthers.deliveryLocation.locationTypeCode
        });
        this.deliveryRequestTransitToOthers.deliveryLocationName = new deliveryRequestService.DeliveryRequestLocationNameModel();
        this.deliveryRequestTransitToOthers.locationAddress = "";

    }

    //Get Location Address & Location Code by Location Name
    getLocationAddress() {
        this.deliveryRequestTransitToOthers.locationCode = _.find(this.deliveryLocationNameList, {
            'locationName': this.deliveryRequestTransitToOthers.deliveryLocationName.locationName
        }).locationCode;
        this.deliveryRequestTransitToOthers.locationAddress = _.find(this.deliveryLocationAddressList, {
            'locationCode': this.deliveryRequestTransitToOthers.locationCode
        }).locationAddress;

    }

    validationTipe() {
        if (this.deliveryRequestTransitToOthers.deliveryLocation.locationType === undefined) {
            this.validateTipe = true;
            this.validateTipeDetail = true;
            this.errorMsgTipe = "Tipe harus dipilih";
        } else {
            this.validateTipe = false;
            this.validateTipeDetail = false;
        }
    }

    validationNama() {
        if (this.deliveryRequestTransitToOthers.deliveryLocationName.locationName === undefined) {
            this.validateNama = true;
            this.validateNamaDetail = true;
            this.errorMsgNama = "Nama harus dipilih";
        } else {
            this.validateNama = false;
            this.validateNamaDetail = false;
        }
    }

    validationLeadTime() {
        if (this.deliveryRequestTransitToOthers.leadTimeDay === undefined || this.deliveryRequestTransitToOthers.leadTimeDay === null) {
            this.leadTimeDay = "0";
        } else {
            this.leadTimeDay = this.deliveryRequestTransitToOthers.leadTimeDay.toString();
        }

        if (this.deliveryRequestTransitToOthers.leadTimeHour === undefined || this.deliveryRequestTransitToOthers.leadTimeHour === null) {
            this.leadTimeHour = "0";

        } else {
            this.leadTimeHour = this.deliveryRequestTransitToOthers.leadTimeHour.toString();
        }

        if (this.deliveryRequestTransitToOthers.leadTimeMinute === undefined || this.deliveryRequestTransitToOthers.leadTimeMinute === null) {
            this.leadTimeMinute = "0";
        } else {
            this.leadTimeMinute = this.deliveryRequestTransitToOthers.leadTimeMinute.toString();
        }

        if (this.deliveryRequestTransitToOthers.leadTimeDay === null
            && this.deliveryRequestTransitToOthers.leadTimeHour === null
            && this.deliveryRequestTransitToOthers.leadTimeMinute === null) {
            this.validateLeadTime = true;
            this.validateLeadTimeDetail = true;
            this.errorMsgLeadTime = "Prakiraan Lead Time harus dipilih";
        }
        //undefined karena default inputan yang bkn numeric dr input type="number" menjadi undefined
        else if (this.deliveryRequestTransitToOthers.leadTimeDay === undefined || this.deliveryRequestTransitToOthers.leadTimeHour === undefined
            || this.deliveryRequestTransitToOthers.leadTimeMinute === undefined) {
            this.validateLeadTime = true;
            this.validateLeadTimeDetail = true;
            this.errorMsgLeadTime = "Prakiraan Lead Time harus berformat numeric";
        } else if (this.deliveryRequestTransitToOthers.leadTimeDay < 0) {
            this.validateLeadTime = true;
            this.validateLeadTimeDetail = true;
            this.errorMsgLeadTime = "Prakiraan Lead Time Hari tidak boleh < 0";
        } else if (this.deliveryRequestTransitToOthers.leadTimeHour < 0 || this.deliveryRequestTransitToOthers.leadTimeHour > 23) {
            this.validateLeadTime = true;
            this.validateLeadTimeDetail = true;
            this.errorMsgLeadTime = "Prakiraan Lead Time Jam tidak boleh < 0 dan > 23";
        } else if (this.deliveryRequestTransitToOthers.leadTimeMinute < 0 || this.deliveryRequestTransitToOthers.leadTimeMinute > 59) {
            this.validateLeadTime = true;
            this.validateLeadTimeDetail = true;
            this.errorMsgLeadTime = "Prakiraan Lead Time Menit tidak boleh < 0 dan > 59";
        } else {
            this.validateLeadTime = false;
            this.validateLeadTimeDetail = false;
        }
    }

    validationPickUpDate() {
        if (this.deliveryRequestTransitToOthers.pickUpDate === null) {
            this.validatePickUpDate = true;
            this.validatePickUpDateDetail = true;
            this.errorMsgPickUpDate = "Tanggal Pengambilan harus diisi";
        } else if (this.deliveryRequestTransitToOthers.pickUpDate < this.today) {
            this.validatePickUpDate = true;
            this.validatePickUpDateDetail = true;
            this.errorMsgPickUpDate = "Tanggal Pengambilan tidak boleh lebih kecil dari tanggal hari ini";
        } else {
            this.validatePickUpDate = false;
            this.validatePickUpDateDetail = false;
        }
    }
    //sama seperti resetValidateDetail di DeliveryRequest.ts, bedanya di Transit Detail sekarang
    resetValidateDetail() {
        if (this.deliveryRequestTransitToOthersNormalModel.deliveryTransitType !== "SelfPickFromOthers") {
            if (this.deliveryRequestTransitToOthers.validateDetailSelfPickToOthers === false
                && this.deliveryRequestTransitToOthers.deliveryRequestTransitType === "SelfPickToOthers") {
                this.deliveryRequestTransitToOthers.validateTransitDetail = false;
            } else if (this.deliveryRequestTransitToOthers.validateDetailTransitNormal === false
                && this.deliveryRequestTransitToOthers.deliveryRequestTransitType === "Normal") {
                this.deliveryRequestTransitToOthers.validateTransitDetail = false;
            } else {
                this.deliveryRequestTransitToOthers.validateTransitDetail = true;
            }
        } else if (this.deliveryRequestTransitToOthers.validateDetailSelfPickFromOthers === false
            && this.deliveryRequestTransitToOthers.deliveryRequestTransitType === "Normal") {
            this.deliveryRequestTransitToOthers.validateTransitDetail = false
        } else {
            this.deliveryRequestTransitToOthers.validateTransitDetail = true;
        }

    }

    //Broadcast
    broadcast() {
        if (this.validateLeadTimeDetail === false && this.validateNamaDetail === false
            && this.validatePickUpDateDetail === false && this.validateTipeDetail === false) {
            this.deliveryRequestTransitToOthers.validateDetail = true;
        } else if (this.deliveryRequestTransitToOthers.deliveryRequestTransitType == "Normal" && this.deliveryRequestTransitToOthers.validateDetailSelfPickFromOthers == false) {
            this.deliveryRequestTransitToOthers.validateTransitDetail = false;
        } else {
            this.deliveryRequestTransitToOthers.validateDetail = false;
        }
        this.root.$broadcast('deliveryRequestTransitToOthers', this.deliveryRequestTransitToOthers);

    }

}

let DeliveryRequestTransitToOthersComponent = {
    controller: DeliveryRequestTransitToOthersController,
    controllerAs: "me",
    template: require("./DeliveryRequestTransitToOthers.html")
}

export { DeliveryRequestTransitToOthersComponent }