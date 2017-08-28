import * as angular from 'angular';
import * as services from '../Services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as _ from 'lodash';

export class LegPriceMasterController implements angular.IController {
    static $inject = ['LegPriceMasterService', '$rootScope'];

    data: string;
    activeUser: string;

    showEdit: boolean;
    created: boolean;
    edited: boolean;

    viewBy = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewBy;
    maxSize = 5;
    //pageSize = 5; 
    pageSizes = [5, 10, 15, 20];
    pageSize = this.pageSizes[0]
    totalItems: number; 0
    pageNumber: number;

    legPriceMaster: services.LegPriceMasterService;
    legPriceMasterViewModelList: services.LegPriceMasterViewModel[];
    deliveryVendorModelList: services.LegPriceMasterDeliveryVendorModel[];
    cityLegModelList: services.LegPriceMasterCityLegModel[];
    deliveryMethodModelList: services.LegPriceMasterDeliveryMethodModel[];
    carSeriesModelList: services.LegPriceMasterCarSeriesModel[];
    currencyModelList: services.LegPriceMasterCurrencyModel[];
    legPriceMasterCreateModel: services.LegPriceMasterCreateModel;

    opened: boolean;
    isError: number;

    validate: boolean = false;
    validateCityLegCostCode: boolean = false;
    validateDeliveryVendorCode: boolean = false;
    validateDeliveryVendorCodeDetail: boolean = true;
    validateCarSeriesCode: boolean = false;
    validateCarSeriesCodeDetail: boolean = true;
    validateCityLegCode: boolean = false;
    validateCityLegCodeDetail: boolean = true;
    validateDeliveryMethodCode: boolean = false;
    validateDeliveryMethodCodeDetail: boolean = true;
    validateValidDate: boolean = false;
    validateCurrencySymbol: boolean = false;
    validateCurrencySymbolDetail: boolean = true;
    validateNominal: boolean = false;
    validateSubstitusi: boolean = false;

    errorMsgDeliveryVendorCode: string = "";
    errorMsgCarSeriesCode: string = "";
    errorMsgCityLegCode: string = "";
    errorMsgDeliveryMethodCode: string = "";
    errorMsgCurrencySymbol: string = "";

    show: boolean = false;
    $rootScope: angular.IRootScopeService;
    pageState: boolean = true;
    jsonLegPrice: {};

    constructor(LegPriceMaster: services.LegPriceMasterService, rootScope: angular.IRootScopeService) {
        this.legPriceMaster = LegPriceMaster;
        this.$rootScope = rootScope;
        this.jsonLegPrice = {};
    }

    $onInit() {
        this.refreshData();
        this.created = true;
        this.edited = false;
        this.opened = false;
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });

    }

    validation() {
        if (this.validateCityLegCostCode === false || this.validateCarSeriesCode === false || this.validateCityLegCode === false
            || this.validateCurrencySymbol === false || this.validateDeliveryMethodCode === false || this.validateDeliveryVendorCode === false
            || this.validateNominal === false || this.validateValidDate === false) {
            this.validate = false;
        } else {
            this.validate = true;
        }
    }

    validationCityLegCostCode() {
        if (this.legPriceMasterCreateModel.cityLegCostCode === undefined) {
            this.validateCityLegCostCode = false;
        } else {
            this.validateCityLegCostCode = true;
        }
        this.validation();
    }

    validationDeliveryVendorCode() {
        if (this.legPriceMasterCreateModel.deliveryVendor === undefined) {
            this.validateDeliveryVendorCode = false;
            this.validateDeliveryVendorCodeDetail = false;
            this.errorMsgDeliveryVendorCode = "Kode Vendor harus dipilih";
        } else {
            this.validateDeliveryVendorCode = true;
            this.validateDeliveryVendorCodeDetail = true;
        }
        this.validation();
    }

    validationCarSeriesCode() {
        if (this.legPriceMasterCreateModel.carSeries === undefined) {
            this.validateCarSeriesCode = false;
            this.validateCarSeriesCodeDetail = false;
            this.errorMsgCarSeriesCode = "Kode Model Series harus dipilih";
        } else {
            this.validateCarSeriesCode = true;
            this.validateCarSeriesCodeDetail = true;
        }
        this.validation();
    }

    validationCityLegCode() {
        if (this.legPriceMasterCreateModel.cityLeg === undefined) {
            this.validateCityLegCode = false;
            this.validateCityLegCodeDetail = false;
            this.errorMsgCityLegCode = "Kode City Leg harus dipilih";
        } else {
            this.validateCityLegCode = true;
            this.validateCityLegCodeDetail = true;
        }

        this.validation();
    }

    validationDeliveryMethodCode() {
        if (this.legPriceMasterCreateModel.deliveryMethod === undefined) {
            this.validateDeliveryMethodCode = false;
            this.validateDeliveryMethodCodeDetail = false;
            this.errorMsgDeliveryMethodCode = "Kode Moda harus dipilih";
        } else {
            this.validateDeliveryMethodCode = true;
            this.validateDeliveryMethodCodeDetail = true;
        }

        this.validation();
    }

    validationValidDate() {
        if (this.legPriceMasterCreateModel.validDate === undefined) {
            this.validateValidDate = false;
        } else {
            this.validateValidDate = true;
        }

        this.validation();
    }

    validationNominal() {
        if (this.legPriceMasterCreateModel.nominal === undefined) {
            this.validateNominal = false;
        } else {
            this.validateNominal = true;
        }

        this.validation();
    }

    validationCurrencySymbol() {
        if (this.legPriceMasterCreateModel.currency === undefined) {
            this.validateCurrencySymbol = false;
            this.validateCurrencySymbolDetail = false;
            this.errorMsgCurrencySymbol = "Currency harus dipilih";
        } else {
            this.validateCurrencySymbol = true;
            this.validateCurrencySymbolDetail = true;
        }

        this.validation();
    }

    orderState: boolean = false;
    orderString: string;


    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }


    open1() {
        this.opened = true;
    }

    showData() {
        this.show = true;
    }
    closeData(data) {
        this.show = false;
    }
    batalData() {
        this.show = false;
        this.legPriceMasterCreateModel.needAdditionalCityLegCostCode = null;
    }

    refreshData() {

        this.legPriceMasterCreateModel = new services.LegPriceMasterCreateModel();

        this.legPriceMaster.getAllCityLegCost().then(response => {
            this.legPriceMasterViewModelList = response.data;

            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;

            this.validateCarSeriesCode = false;
            this.validateCityLegCostCode = false;
            this.validateCityLegCode = false;
            this.validateCurrencySymbol = false;
            this.validateDeliveryMethodCode = false;
            this.validateDeliveryVendorCode = false;
            this.validateNominal = false;
            this.validateValidDate = false;
            this.validate = false;

            this.show = false;
            this.showEdit = false;
            this.created = true;
            this.edited = false;

        });
        this.legPriceMaster.getAllKodeVendor().then(response => {
            this.deliveryVendorModelList = response.data;
        });

        this.legPriceMaster.getAllCarSeries().then(response => {
            this.carSeriesModelList = response.data;
        });

        this.legPriceMaster.getDeliveryMethod().then(response => {
            this.deliveryMethodModelList = response.data;
        });
        this.legPriceMaster.getAllCityLeg().then(response => {
            this.cityLegModelList = response.data;
        });
        this.legPriceMaster.getAllCurrency().then(response => {
            this.currencyModelList = response.data;
        });
    }

    selectEdit(data) {
        this.legPriceMasterCreateModel.cityLegCostCode = data.cityLegCostCode;
        this.legPriceMasterCreateModel.deliveryVendor = new services.LegPriceMasterDeliveryVendorModel();
        this.legPriceMasterCreateModel.deliveryVendor.deliveryVendorCode = data.deliveryVendorCode;
        this.legPriceMasterCreateModel.carSeries = new services.LegPriceMasterCarSeriesModel();
        this.legPriceMasterCreateModel.carSeries.carSeriesCode = data.carSeriesCode;
        this.legPriceMasterCreateModel.deliveryMethod = new services.LegPriceMasterDeliveryMethodModel();
        this.legPriceMasterCreateModel.deliveryMethod.deliveryMethodCode = data.deliveryMethodCode;
        this.legPriceMasterCreateModel.validDate = new Date(data["validDate"]);
        this.legPriceMasterCreateModel.cityLeg = new services.LegPriceMasterCityLegModel();
        this.legPriceMasterCreateModel.cityLeg.cityLegCode = data.cityLegCode;
        this.legPriceMasterCreateModel.currency = new services.LegPriceMasterCurrencyModel();
        this.legPriceMasterCreateModel.currency.currencySymbol = data.currencySymbol;
        this.legPriceMasterCreateModel.nominal = data.nominal;
        this.legPriceMasterCreateModel.needAdditionalCityLegCostCode = data.needAdditionalCityLegCostCode;

        this.showEdit = true;
        this.created = false;
        this.edited = true;

        this.validateCarSeriesCode = true;
        this.validateCityLegCode = true;
        this.validateCityLegCostCode = true;
        this.validateCurrencySymbol = true;
        this.validateDeliveryMethodCode = true;
        this.validateDeliveryVendorCode = true;
        this.validateNominal = true;
        this.validateValidDate = true;
        this.validate = true;
    }
    selectCode(data) {
        this.legPriceMasterCreateModel.needAdditionalCityLegCostCode = data.cityLegCostCode;
    }



    createData() {
        this.legPriceMaster.postData(this.legPriceMasterCreateModel).then(response => {
            this.isError = response.data;
            if (this.isError === 1) {
                alertify.error("Data gagal disimpan");
            } else if (this.isError === 2) {
                alertify.error('Kode Price Leg telah terdaftar');
            } else {
                alertify.success("Data berhasil disimpan");
                this.refreshData();
            }
        });
    }

    updateData() {
        this.legPriceMaster.updateData(this.legPriceMasterCreateModel).then(response => {

                this.isError = response.data;

                if (this.isError === 1) {
                    alertify.error("Data gagal disimpan");
                } else {
                    alertify.success("Data berhasil disimpan");
                    this.refreshData();
                }
                
            });
    }


    deleteData() {
        this.legPriceMaster.deleteData(this.legPriceMasterCreateModel.cityLegCostCode).then(response => {
            this.refreshData();
            alertify.success("Data berhasil dihapus");
        }).catch(() => {
            alertify.error("Data gagal dihapus");
        });
    }
    selectDelete(data, MyForm: angular.IFormController) {
        this.selectEdit(data);
        this.jsonLegPrice["Kode Price Leg"] = this.legPriceMasterCreateModel.cityLegCostCode;
        this.jsonLegPrice["Kode Vendor"] = this.legPriceMasterCreateModel.deliveryVendor.deliveryVendorCode;
        this.jsonLegPrice["Kode Moda"] = this.legPriceMasterCreateModel.deliveryMethod.deliveryMethodCode;
        this.jsonLegPrice["Kode City Leg"] = this.legPriceMasterCreateModel.cityLeg.cityLegCode;
        this.jsonLegPrice["Kode Model Series"] = this.legPriceMasterCreateModel.carSeries.carSeriesCode;
        this.jsonLegPrice["Currency"] = this.legPriceMasterCreateModel.currency.currencySymbol;
        this.jsonLegPrice["Biaya"] = this.legPriceMasterCreateModel.nominal;
        this.jsonLegPrice["Tanggal Berlaku"] = moment(this.legPriceMasterCreateModel.validDate).format('DD-MMM-YYYY');      
        this.jsonLegPrice["Substitusi"] = this.legPriceMasterCreateModel.needAdditionalCityLegCostCode;

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonLegPrice)),
            () => {
                this.deleteData();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

        
    }

    insertData() {
        this.jsonLegPrice["Kode Price Leg"] = this.legPriceMasterCreateModel.cityLegCostCode;
        this.jsonLegPrice["Kode Vendor"] = this.legPriceMasterCreateModel.deliveryVendor.deliveryVendorCode;
        this.jsonLegPrice["Kode Moda"] = this.legPriceMasterCreateModel.deliveryMethod.deliveryMethodCode;
        this.jsonLegPrice["Kode City Leg"] = this.legPriceMasterCreateModel.cityLeg.cityLegCode;
        this.jsonLegPrice["Kode Model Series"] = this.legPriceMasterCreateModel.carSeries.carSeriesCode;
        this.jsonLegPrice["Currency"] = this.legPriceMasterCreateModel.currency.currencySymbol;
        this.jsonLegPrice["Biaya"] = this.legPriceMasterCreateModel.nominal;
        this.jsonLegPrice["Tanggal Berlaku"] = moment(this.legPriceMasterCreateModel.validDate).format('DD-MMM-YYYY');
        this.jsonLegPrice["Substitusi"] = this.legPriceMasterCreateModel.needAdditionalCityLegCostCode;

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonLegPrice)),
            () => {
                this.createData();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

    }

    editData() {
        this.jsonLegPrice["Kode Price Leg"] = this.legPriceMasterCreateModel.cityLegCostCode;
        this.jsonLegPrice["Kode Vendor"] = this.legPriceMasterCreateModel.deliveryVendor.deliveryVendorCode;
        this.jsonLegPrice["Kode Moda"] = this.legPriceMasterCreateModel.deliveryMethod.deliveryMethodCode;
        this.jsonLegPrice["Kode City Leg"] = this.legPriceMasterCreateModel.cityLeg.cityLegCode;
        this.jsonLegPrice["Kode Model Series"] = this.legPriceMasterCreateModel.carSeries.carSeriesCode;
        this.jsonLegPrice["Currency"] = this.legPriceMasterCreateModel.currency.currencySymbol;
        this.jsonLegPrice["Biaya"] = this.legPriceMasterCreateModel.nominal;
        this.jsonLegPrice["Tanggal Berlaku"] = moment(this.legPriceMasterCreateModel.validDate).format('DD-MMM-YYYY');
        this.jsonLegPrice["Substitusi"] = this.legPriceMasterCreateModel.needAdditionalCityLegCostCode;

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonLegPrice)),
            () => {
                this.updateData();
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });
        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin data ingin disimpan?";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin data ingin disimpan?";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin data ingin dihapus?";
        convertResult["grid"] = tempJson;
        return convertResult;
    }

    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.cityLegCostCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "CityLegCost";
        info.title = "Leg Price Master";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    upload() {
        let info: any = {};
        info.master = "CityLegCost";
        info.title = "Leg Price Master";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

}

export class LegPriceMasterComponent implements angular.IComponentOptions {
    controller = LegPriceMasterController;
    controllerAs = 'me';

    template = require('./LegPriceMaster.html');
    bindings = {
        greet: '@'
    };
}