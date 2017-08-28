import * as deliveryRequestService from '../services';
import * as moment from 'moment';

class DeliveryRequestDirectDeliveryController implements angular.IController {
    static $inject = ["$rootScope", "DeliveryRequestService"];

    root: angular.IRootScopeService;

    constructor(root: angular.IRootScopeService, deliveryRequestService: deliveryRequestService.DeliveryRequestService) {
        this.root = root;
        this.deliveryRequestService = deliveryRequestService;
    }

    deliveryRequestService: deliveryRequestService.DeliveryRequestService;
    popup1: deliveryRequestService.PopUp;
    //validate tanpa detail = flag masing2 field untuk menampilkan error message
    //validate dengan detail = flag masing2 field yang dibandingkan untuk mengetahui apakah validasi PASS atau belum
    validateEstimasiPdcOut: boolean = false;
    validateEstimasiPdcOutDetail: boolean = true;
    validateCustomerName: boolean = false;
    validateCustomerNameDetail: boolean = true;
    validateCustomerAddress: boolean = false;
    validateCustomerAddressDetail: boolean = true;
    validateCustomerCity: boolean = false;
    validateCustomerCityDetail: boolean = true;
    validateSalesmanName: boolean = false;
    validateSalesmanNameDetail: boolean = true;
    validateSalesmanContactNo: boolean = true;
    validateSalesmanContactNoDetail: boolean = true;
    errorMsgEstimasiPdcOut: string = "";
    errorMsgCustomerName: string = "";
    errorMsgCustomerAddress: string = "";
    errorMsgCustomerCity: string = "";
    errorMsgSalesmanName: string = "";
    errorMsgSalesmanContactNo: string = "";
    //untuk validasi tanggal hr ini
    today: Date = moment().toDate();

    $onInit() {
        this.deliveryRequestDirectDelivery = new deliveryRequestService.DeliveryRequestDirectDeliveryModel;
        this.popup1 = new deliveryRequestService.PopUp();
        //di set ke 0 karena yang perlu dibandingkan hanya hari
        this.today.setHours(0);
        this.today.setMinutes(0);
        this.today.setSeconds(0);
        this.today.setMilliseconds(0);
        //Reset Form
        this.root.$on('deliveryRequestDirectDeliveryReset', (event, data) => {
            this.deliveryRequestDirectDelivery = data;
            this.deliveryRequestDirectDelivery.validateDetail = false;
            this.validateEstimasiPdcOut= false;
            this.validateEstimasiPdcOutDetail = true;
            this.validateCustomerName = false;
            this.validateCustomerNameDetail = true;
            this.validateCustomerAddress = false;
            this.validateCustomerAddressDetail = true;
            this.validateCustomerCity = false;
            this.validateCustomerCityDetail = true;
            this.validateSalesmanName = false;
            this.validateSalesmanNameDetail = true;
            this.validateSalesmanContactNo = true;
            this.validateSalesmanContactNoDetail = true;

            this.root.$broadcast('deliveryRequestDirectDelivery', this.deliveryRequestDirectDelivery);
        });
    }

    //Untuk membuka DatePicker Popup
    open1() {
        this.popup1.opened = true;
    }

    //Validasi masing2 field
    validationEstimasiPdcOut() {
        if (this.deliveryRequestDirectDelivery.estimasiPdcOut === null) {
            this.validateEstimasiPdcOut = true;
            this.validateEstimasiPdcOutDetail = true;
            this.errorMsgEstimasiPdcOut = "Estimasi PDC Out harus diisi";
        } else if (this.deliveryRequestDirectDelivery.estimasiPdcOut < this.today) {
            this.validateEstimasiPdcOut = true;
            this.validateEstimasiPdcOutDetail = true;
            this.errorMsgEstimasiPdcOut = "Estimasi PDC Out tidak boleh lebih kecil dari tanggal hari ini";
        } else {
            this.validateEstimasiPdcOut = false;
            this.validateEstimasiPdcOutDetail = false;
        }
    }

    validationCustomerName() {
        if (this.deliveryRequestDirectDelivery.customerName === "") {
            this.validateCustomerName = true;
            this.validateCustomerNameDetail = true;
            this.errorMsgCustomerName = "Nama Customer harus diisi";
        } else if (!this.deliveryRequestDirectDelivery.customerName.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateCustomerName = true;
            this.validateCustomerNameDetail = true;
            this.errorMsgCustomerName = "Nama Customer harus berformat alphanumeric";
        } else if (this.deliveryRequestDirectDelivery.customerName.length > 255) {
            this.validateCustomerName = true;
            this.validateCustomerNameDetail = true;
            this.errorMsgCustomerName = "Nama Customer tidak boleh > 255 karakter";
        } else {
            this.validateCustomerName = false;
            this.validateCustomerNameDetail = false;
        }
    }

    validationCustomerAddress() {
        if (this.deliveryRequestDirectDelivery.customerAddress === "") {
            this.validateCustomerAddress = true;
            this.validateCustomerAddressDetail = true;
            this.errorMsgCustomerAddress = "Alamat Customer harus diisi";
        } else if (!this.deliveryRequestDirectDelivery.customerAddress.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateCustomerAddress = true;
            this.validateCustomerAddressDetail = true;
            this.errorMsgCustomerAddress = "Alamat Customer harus berformat alphanumeric";
        } else if (this.deliveryRequestDirectDelivery.customerAddress.length > 255) {
            this.validateCustomerAddress = true;
            this.validateCustomerAddressDetail = true;
            this.errorMsgCustomerAddress = "Alamat Customer tidak boleh > 255 karakter";
        } else {
            this.validateCustomerAddress = false;
            this.validateCustomerAddressDetail = false;
        }
    }

    validationCustomerCity() {
        if (this.deliveryRequestDirectDelivery.customerCity === "") {
            this.validateCustomerCity = true;
            this.validateCustomerCityDetail = true;
            this.errorMsgCustomerCity = "Kota Customer harus diisi";
        } else if (!this.deliveryRequestDirectDelivery.customerCity.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateCustomerCity = true;
            this.validateCustomerCityDetail = true;
            this.errorMsgCustomerCity = "Kota Customer harus berformat alphanumeric";
        } else if (this.deliveryRequestDirectDelivery.customerCity.length > 255) {
            this.validateCustomerCity = true;
            this.validateCustomerCityDetail = true;
            this.errorMsgCustomerCity = "Kota Customer tidak boleh > 255";
        } else {
            this.validateCustomerCity = false;
            this.validateCustomerCityDetail = false;
        }
    }

    validationSalesmanName() {
        if (this.deliveryRequestDirectDelivery.salesmanName === "") {
            this.validateSalesmanName = true;
            this.validateSalesmanNameDetail = true;
            this.errorMsgSalesmanName = "Nama Salesman harus diisi";
        } else if (!this.deliveryRequestDirectDelivery.salesmanName.match(/^[\w\.\,\-\&\/\s]+$/)) {
            this.validateSalesmanName = true;
            this.validateSalesmanNameDetail = true;
            this.errorMsgSalesmanName = "Nama Salesman harus berformat alphanumeric";
        } else if (this.deliveryRequestDirectDelivery.salesmanName.length > 255) {
            this.validateSalesmanName = true;
            this.validateSalesmanNameDetail = true;
            this.errorMsgSalesmanName = "Nama Salesman tidak boleh > 255 karakter";
        } else {
            this.validateSalesmanName = false;
            this.validateSalesmanNameDetail = false;
        }
    }

    validationSalesmanContactNo() {
        if (this.deliveryRequestDirectDelivery.salesmanContactNo === "") {
            this.validateSalesmanContactNo = true;
            this.validateSalesmanContactNoDetail = true;
            this.errorMsgSalesmanContactNo = "No. Contact Salesman harus diisi";
        } else if (!this.deliveryRequestDirectDelivery.salesmanContactNo.match(/^[\d]+$/)) {
            this.validateSalesmanContactNo = true;
            this.validateSalesmanContactNoDetail = true;
            this.errorMsgSalesmanContactNo = "Contact No. Salesman harus berformat numeric";
        } else if (this.deliveryRequestDirectDelivery.salesmanContactNo.length > 255) {
            this.validateSalesmanContactNo = true;
            this.validateSalesmanContactNoDetail = true;
            this.errorMsgSalesmanContactNo = "Contact No. Salesman tidak boleh > 255 karakter";
        } else {
            this.validateSalesmanContactNo = false;
            this.validateSalesmanContactNoDetail = false;
        }
    }

    //Broadcast & menentukan apakah semua validasi sudah pass atau blm
    broadcast() {
        if (this.validateCustomerAddressDetail === false && this.validateCustomerCityDetail === false
            && this.validateCustomerNameDetail === false && this.validateEstimasiPdcOutDetail === false
            && this.validateSalesmanContactNoDetail === false && this.validateSalesmanNameDetail === false) {
            this.deliveryRequestDirectDelivery.validateDetail = true;
        } else {
            this.deliveryRequestDirectDelivery.validateDetail = false;
        }

        this.root.$broadcast('deliveryRequestDirectDelivery', this.deliveryRequestDirectDelivery);
    }

    deliveryRequestDirectDelivery: deliveryRequestService.DeliveryRequestDirectDeliveryModel;
}

let DeliveryRequestDirectDeliveryComponent = {
    controller: DeliveryRequestDirectDeliveryController,
    controllerAs: "me",
    template: require("./DeliveryRequestDirectDelivery.html")
}

export { DeliveryRequestDirectDeliveryComponent }