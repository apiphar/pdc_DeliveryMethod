
import * as Services from '../Services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';
import * as _ from 'lodash';

export class CompanyMasterController implements angular.IController {
    static $inject = ['CompanyMasterService'];

    editBtn: boolean;
    showEdit: boolean;
    Created: boolean;
    Edited: boolean;

    Data: string;
    ActiveUser: string;

    viewby = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    //pageSize = 5; 
    pageSizes = [5, 10, 15, 20, 25];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;

    CompanyMaster: Services.CompanyMasterService;

    dealer: any;
    dealerCodeData: any;

    dealerCode: any;
    companyCode: string;
    name: string;
    npwpAddress: string;
    sapCode: string;
    phone: string;
    fax: string;
    email: string;
    npwp: string;
    tradeName: string;
    isDealerFinancing: boolean = false;
    orderString = 'dealerCode';
    orderState = false;

    constructor(CompanyMaster: Services.CompanyMasterService) {
        this.CompanyMaster = CompanyMaster;
        this.dealer = [{
            dealerCode: ''
        }];

        this.editBtn = false;
        this.showEdit = true;
    }

    order(name) {
        this.orderString = name;
        this.orderState = !this.orderState;
    }

    $onInit() {
        this.refreshData();
        this.Created = true;
        this.Edited = false;
    }

    selectEdit(data) {
     
        this.dealer["dealerCode"] = data.dealerCode;
        //this.dealerCodeData = { 'dealerCode':data.dealerCode};
        this.companyCode = data.companyCode;
        this.name = data.name;
        this.npwpAddress = data.npwpAddress;
        this.fax = data.fax;
        this.phone = data.phone;
        this.tradeName = data.tradeName;
        this.npwp = data.npwp;
        this.email = data.email;
        this.sapCode = data.sapCode;
        this.isDealerFinancing = data.isDealerFinancing;
        console.log(this.companyCode);
        this.editBtn = true;
        this.showEdit = false;
        this.Created = false;
        this.Edited = true;
    }

    refreshData() {

        this.CompanyMaster.GetData().then(response => {
            console.log(response);
            this.Data = response.data;

            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;
            this.editBtn = false;

            this.dealer["dealerCode"] = null;
            this.npwpAddress = null;
            this.fax = null;
            this.npwp = null;
            this.sapCode = null;
            this.tradeName = null;
            this.email = null;
            this.companyCode = null;
            this.name = null;
            this.phone = null;
            this.isDealerFinancing = null;
            this.Created = true;
            this.Edited = false;
        });
        this.CompanyMaster.GetAll().then(response => {
            this.dealerCodeData = response.data;
            console.log(this.dealerCodeData);
        });
    }

    createData() {
        let isDealerFinancing = this.isDealerFinancing;
        //this.dealerCode = this.dealer["dealerCode"];
        if (!this.isDealerFinancing) {
            isDealerFinancing = false;
        }
        console.log(this.dealerCode, this.name, this.companyCode, this.isDealerFinancing);
        this.CompanyMaster.PostData(this.dealer["dealerCode"], this.companyCode, this.name, this.npwpAddress, this.sapCode, this.phone, this.fax, this.email, this.tradeName, this.npwp, isDealerFinancing).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    updateData() {
        //this.dealerCode = this.dealer["dealerCode"];
        console.log(this.dealerCode, this.companyCode, this.name, this.isDealerFinancing);
        this.CompanyMaster.UpdateData(this.companyCode, this.dealer["dealerCode"], this.name, this.npwpAddress, this.sapCode, this.phone, this.fax, this.email, this.tradeName, this.npwp, this.isDealerFinancing).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    deleteData() {

        console.log(this.companyCode);
        this.CompanyMaster.DeleteData(this.companyCode).then(response => {
            this.refreshData();
        });
    }
    selectDelete(data, MyForm: angular.IFormController) {
        this.selectEdit(data);
        this.confirmationDialog('Are you sure want to delete this Exchange Rate ?', 'Delete', MyForm)
    }


    alertifyData() {
        let dealerFinancing = 'Ya';
        if (!this.isDealerFinancing) dealerFinancing = 'Tidak';

        var data = {
            dealerCode: this.dealer["dealerCode"],
            companyCode: this.companyCode,
            name: this.name,
            address: this.npwpAddress,
            sapCode: this.sapCode,
            phone: this.phone,
            fax: this.fax,
            email: this.email,
            tradeName: this.tradeName,
            npwp: this.npwp,
            isDealerFinancing: dealerFinancing,
            //groupDealerCode: (!this.groupDealerCodes) ? '' : this.groupDealerCodes["groupDealerCode"],

        };

        return data;
    }

    confirmationDialog(Message: string, Params: string, MyForm: angular.IFormController) {
        let self = this
        console.log(self.npwpAddress, self.companyCode, self.npwp, self.email, self.phone, self.tradeName, self.fax, self.sapCode, self.name, self.dealer["dealerCode"])
        if (!self.companyCode || !self.dealer["dealerCode"] || !self.npwpAddress || !self.name || !self.npwp || !self.phone || !self.tradeName || !self.fax || !self.sapCode || !self.email) {
            alertify.error('Data Harus Di Isi');
        }
        else {
            alertify.confirm(Message,
                mustache.render(require("./alertify/CompanyMasterAlertify.html"), this.alertifyData()),
                function () {

                    if (Params == 'Create') {
                        self.createData();
                        MyForm.$setPristine();
                        alertify.success('Data Succesfully Saved');
                    }
                    else if (Params == 'Update') {
                        self.updateData();
                        MyForm.$setPristine();
                        alertify.success('Data Succesfully Updated');
                    }
                    else if (Params == 'Delete') {
                        self.deleteData();
                        MyForm.$setPristine();
                        alertify.success('Data Succesfully Deleted');
                    }
                    else {
                        MyForm.$setPristine();
                    }

                },
                function () {
                    alertify.error('Cancel');
                }
            );
        }
    }

}

export class CompanyMasterComponent implements angular.IComponentOptions {
    controller = CompanyMasterController;
    controllerAs = 'me';

    template = require('./CompanyMaster.html');
    bindings = {
        greet: '@'
    };
}