
import * as _ from 'lodash';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as service from '../services';

class FormARequestController implements angular.IController {
    static $inject = ['FormARequestService'];

    formARequestService: service.FormARequestService;

    invoicesList: service.FormARequestInvoiceViewModel[];

    nomorPIB: string;
    invoiceNumber: string;
    invoice: service.FormARequestInvoiceViewModel;
    searchInvoicesList: service.FormARequestInvoiceViewModel[];
    updateInvoicesList: service.FormARequestInvoiceViewModel[];

    formModel: service.FormARequestFormModel;

    loading: boolean;

    constructor(formARequestService: service.FormARequestService) {
        this.formARequestService = formARequestService;
    }

    $onInit() {
        this.getInvoicesList();
    }

    getInvoicesList() {
        this.invoicesList = new Array<service.FormARequestInvoiceViewModel>();
        this.formARequestService.getInvoicesList().then(response => {
            this.invoicesList = response.data;
        });
    }

    searchInvoices() {
        this.searchInvoicesList = new Array<service.FormARequestInvoiceViewModel>();
        if (this.invoiceNumber != null && this.invoiceNumber != "" && this.nomorPIB != null && this.nomorPIB != "") {
            this.searchInvoicesList = _.filter(this.invoicesList, { "invoiceNumber": this.invoiceNumber, "nomorPIB": this.nomorPIB });
        }
        else if (this.nomorPIB != null && this.nomorPIB != "") {
            this.searchInvoicesList = _.filter(this.invoicesList, ["nomorPIB", this.nomorPIB]);
        }
        else if (this.invoiceNumber != null && this.invoiceNumber != "") {
            this.searchInvoicesList = _.filter(this.invoicesList, ["invoiceNumber", this.invoiceNumber]);
        }
        else {
            this.searchInvoicesList = this.invoicesList;
        }

        this.totalItems = this.invoicesList.length;
    }

    updatePIB() {
        let self = this;

        alertify.confirm('Form A Request', 'Yakin akan membuat Form A Request?', function () {
            self.loading = true;

            self.updateInvoicesList = new Array<service.FormARequestInvoiceViewModel>();
            self.searchInvoicesList.forEach((item) => {
                if (item.isAction == true) {
                    self.invoice = new service.FormARequestInvoiceViewModel();
                    self.invoice = item;
                    self.updateInvoicesList.push(self.invoice);
                }
            });
            self.confirmUpdatePIB(self.updateInvoicesList);
            alertify.success('Sukses');
        },
            function () {
                alertify.error('Batal');
            }
        );
    }

    confirmUpdatePIB(updateInvoicesList: service.FormARequestInvoiceViewModel[]) {
        this.formARequestService.updatePIB(this.updateInvoicesList).then(response => {
            this.formModel = new service.FormARequestFormModel();
            this.formModel = response.data;
            let link = document.createElement('a');
            link.href = "/api/v1/FormARequestApi/Download?guid=" + this.formModel.guid + "&filename=" + this.formModel.fileName;
            document.body.appendChild(link);
            link.click();

            this.loading = false;
        });
    }

    emptyInput() {
        this.invoiceNumber = null;
        this.nomorPIB = null;
        this.searchInvoicesList = null;
        this.totalItems = 0;
    }

    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    }
}

let FormARequest = {
    controller: FormARequestController,
    controllerAs: "me",
    template: require("./FormARequest.html")
}

export { FormARequest }

