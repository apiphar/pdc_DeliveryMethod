import * as Service from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';

export class ExchangeRateController implements angular.IController {
    static $inject = ['ExchangeRateService'];

    //declare IFormController
    MyForm: angular.IFormController;

    data: string;
    dataCurrency: string;
    validFrom: Date;
    validUntil: Date;
    toRupiah: number;
    exchangeRateId: number;
    currencySymbol: string
    name: string;
    createdAt: Date;
    updatedAt: Date;

    viewby = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    pageSizes = [2, 5, 10, 20];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;

    created: boolean;
    edited: boolean;

    ExchangeRate: Service.ExchangeRateService;

    //Dropdown Currencys
    currencysName: [{
        text: String;
        value: String;
    }];
    //For Modal
    currencyNameText: String;
    currencyNameValue: String;

    constructor(exchangeRate: Service.ExchangeRateService) {
        this.ExchangeRate = exchangeRate;
        this.currencysName = [{
            text : '',
            value : ''
        }];
    }

    refreshData() {
        this.ExchangeRate.getExchangeRates().then(response => {
            this.data = response.data;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;
        });

        this.ExchangeRate.getCurrencys().then(response => {
            this.dataCurrency = response.data;
        });
        this.validFrom = new Date;
        this.validUntil = new Date;
        this.created = true;
        this.edited = false;
    }

    clearForm() {
        this.exchangeRateId = null;
        this.currencysName["text"] = "(Please Choose One)";
        this.currencysName["value"] = '0'
        this.toRupiah = 0;
        this.validFrom = new Date;
        this.validUntil = new Date;
        this.created = true;
        this.edited = false;
    }
    
    $onInit() {
        this.refreshData();
    }

    saveData(MyForm: angular.IFormController) {
        this.confirmationDialog('Are you sure want to save this Exchange Rate ?', 'Simpan', MyForm);
    }

    createData() {
        this.createdAt = new Date;
        this.updatedAt = new Date;
        this.ExchangeRate.postData(this.currencysName["value"], this.validFrom, this.validUntil, this.toRupiah, this.createdAt, this.updatedAt).then(response => {
            this.refreshData();
            this.clearForm();
        });
    }


    updateData() {
        this.createdAt = new Date;
        this.updatedAt = new Date;
        this.ExchangeRate.updateData(this.exchangeRateId, this.currencysName["value"], this.validFrom, this.validUntil, this.toRupiah, this.createdAt, this.updatedAt).then(response => {
            this.refreshData();
            this.clearForm();
        });
    }

    deleteData() {
        this.ExchangeRate.deleteData(this.exchangeRateId).then(response => {
            this.refreshData();
            this.clearForm();
        });
    }

    selectEdit(data) {
        this.getAllSelectedData(data);
        this.created = false;
        this.edited = true;
    }

    selectDelete(data, MyForm: angular.IFormController) {
        this.getAllSelectedData(data);
        this.confirmationDialog('Are you sure want to delete this Exchange Rate ?', 'Delete', MyForm)
    }

    getAllSelectedData(data) {
        this.exchangeRateId = data["exchangeRateId"]
        this.toRupiah = data["toRupiah"]
        this.currencysName["value"] = data["currencySymbol"];
        this.validFrom = new Date(data["validFrom"]);
        this.validUntil = new Date(data["validUntil"]);
    }

    alertifyData() {
        var data = {
            exchangeRateId: this.exchangeRateId,
            currencyText: this.currencysName["text"],
            currencyValue: this.currencysName["value"],
            validFrom: moment(this.validFrom).format('YYYY/MM/D'),
            validUntil: moment(this.validUntil).format('YYYY/MM/D'),
            toRupiah: this.toRupiah
        };
        return data;
    }

    confirmationDialog(Message: string, Params: string, Form: angular.IFormController) {
        let self = this;

        alertify.confirm(Message,
            mustache.render(require("./alertify/ExchangeRateAlertify.html"), this.alertifyData()),
            function () {

                if (Params == 'Simpan') {
                    self.createData();
                    Form.$setPristine();
                    alertify.success('Data Succesfully Saved');
                }
                else if (Params == 'Update') {
                    self.updateData();
                    Form.$setPristine();
                    alertify.success('Data Succesfully Updated');
                }
                else if (Params == 'Delete') {
                    self.deleteData();
                    Form.$setPristine();
                    alertify.success('Data Succesfully Deleted');
                }
                else {
                    Form.$setPristine();
                }

            },
            function () {
                alertify.error('Cancel');
            }
        );
    }
}

export class ExchangeRateComponent implements angular.IComponentOptions {
    controller = ExchangeRateController;
    controllerAs = 'me';

    template = require('./ExchangeRate.html');
    bindings = {
        greet: '@'
    };
}