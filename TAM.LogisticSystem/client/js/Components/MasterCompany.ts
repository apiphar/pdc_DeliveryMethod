import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as _ from 'lodash';
import * as service from '../services';

export class MasterCompanyController implements angular.IController {
    static $inject = ['MasterCompanyService', '$rootScope'];

    masterCompanyService: service.MasterCompanyService;
    masterCompaniesData: service.MasterCompanyViewModel[];
    masterCompanyData: service.MasterCompanyViewModel;
    masterCompanyDealersData: service.MasterCompanyDealerComboBoxModel[];
    $rootscope: angular.IRootScopeService;
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    totalItems: number;
    orderState: boolean = false;
    orderString: string;
    pageState: boolean = true;
    isUpdate: boolean = false;
    searchTable = {};
    jsonConfirmData = {};
    codePattern = '[A-Za-z0-9]+';
    numericPattern = '[0-9]*';
    npwpPattern = '[0-9-.]*';
    namePattern = '[A-Za-z0-9\\s-.&,\'\/]+';

    constructor(masterCompanyService: service.MasterCompanyService, $rootscope: angular.IRootScopeService) {
        this.masterCompanyService = masterCompanyService;
        this.$rootscope = $rootscope;
    }

    $onInit() {
        this.getCompanies();
        this.getDealers();
        this.masterCompanyData = new service.MasterCompanyViewModel();
        this.masterCompanyData.termOfPaymentDay = 0;
        this.$rootscope.$on('Kembali', (event) => {
            this.pageState = true;
            this.getCompanies();
            this.getDealers();
        });
    }

    //for download
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.companyCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Company";
        info.title = "Master Company";
        info.tipe = "3";
        this.$rootscope.$emit("UploadDownload", tempData, info);
    }

    //clean the form data
    resetForm(form: angular.IFormController) {
        this.masterCompanyData = new service.MasterCompanyViewModel();
        this.isUpdate = false;
        this.searchTable = {};
        this.jsonConfirmData = {};
        this.masterCompanyData.termOfPaymentDay = 0;
        form.$setPristine();
        form.$setUntouched();
    }

    //get all companies data
    getCompanies() {
        this.masterCompanyService.getCompanies().then(response => {
            this.masterCompaniesData = response.data as service.MasterCompanyViewModel[];
            this.totalItems = this.masterCompaniesData.length;
            this.generateTermOfPaymentDayString();
            this.generateDealerString();
        }).catch(response => {
            if (response.status === 500) {
                alertify.error('Koneksi ke server bermasalah')
            }
        });
    }

    //get all dealers data
    getDealers() {
        this.masterCompanyService.getDealers().then(response => {
            this.masterCompanyDealersData = response.data as service.MasterCompanyDealerComboBoxModel[];
        }).catch(response => {
            if (response.status === 500) {
                alertify.error('Koneksi ke server bermasalah')
            }});
    }

    //generate term of payment day string
    generateTermOfPaymentDayString() {
        angular.forEach(this.masterCompaniesData, (data) => {
            if (data.termOfPaymentDay === null) {
                data.termOfPaymentDayString = '';
            }
            data.termOfPaymentDayString = data.termOfPaymentDay + ' hari';
        });
    }

    //generate dealer string
    generateDealerString() {
        angular.forEach(this.masterCompaniesData, (data) => {
            data.dealerString = data.dealerCode + ' - ' + data.dealerName;
        });
    }

    //bind the selected row to the form
    setUpdate(masterCompanyData: service.MasterCompanyViewModel) {
        this.masterCompanyData = new service.MasterCompanyViewModel();
        this.masterCompanyData.companyCode = masterCompanyData.companyCode;
        this.masterCompanyData.dealerCode = masterCompanyData.dealerCode;
        this.masterCompanyData.email = masterCompanyData.email;
        this.masterCompanyData.fax = masterCompanyData.fax;
        this.masterCompanyData.isDealerFinancing = masterCompanyData.isDealerFinancing;
        this.masterCompanyData.companyName = masterCompanyData.companyName;
        this.masterCompanyData.npwp = masterCompanyData.npwp;
        this.masterCompanyData.npwpAddress = masterCompanyData.npwpAddress;
        this.masterCompanyData.phone = masterCompanyData.phone;
        this.masterCompanyData.sapCode = masterCompanyData.sapCode;
        this.masterCompanyData.termOfPaymentDay = masterCompanyData.termOfPaymentDay;
        this.masterCompanyData.tradeName = masterCompanyData.tradeName;
        this.isUpdate = true;
    }

    //update the data in the form
    update(form: angular.IFormController) {
        this.jsonConfirmData['Kode Company Dealer'] = this.masterCompanyData.companyCode;
        this.jsonConfirmData['Kode Group Dealer'] = this.masterCompanyData.dealerCode;
        this.jsonConfirmData['Nama'] = this.masterCompanyData.companyName;
        this.jsonConfirmData['Alamat NPWP'] = this.masterCompanyData.npwpAddress;
        this.jsonConfirmData['Kode Company SAP'] = this.masterCompanyData.sapCode;
        this.jsonConfirmData['No Telp'] = this.masterCompanyData.phone;
        this.jsonConfirmData['No Faxs'] = this.masterCompanyData.fax;
        this.jsonConfirmData['Email'] = this.masterCompanyData.email;
        this.jsonConfirmData['Trade Name'] = this.masterCompanyData.tradeName;
        this.jsonConfirmData['NPWP'] = this.masterCompanyData.npwp;
        this.jsonConfirmData['Dealer Financing'] = this.masterCompanyData.isDealerFinancing === true ? 'Ya' : 'Tidak';
        this.jsonConfirmData['Term of Payment'] = this.masterCompanyData.termOfPaymentDay + ' hari';
        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.jsonConfirmData)),
            () => {
                this.masterCompanyService.updateData(this.masterCompanyData).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.resetForm(form);
                    this.masterCompaniesData = new Array<service.MasterCompanyViewModel>();
                    this.getCompanies();
                }).catch(response => {
                    if (response.status === 400) {
                        alertify.error(response.data);
                    }
                    if (response.status === 500) {
                        alertify.error('Koneksi ke server bermasalah')
                    }
                    else {
                        alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //set json data form confirmation modal
    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["insert"] = 1;
        else if (action.toLowerCase() == "update") convertResult["update"] = 1;
        else if (action.toLowerCase() == "delete") convertResult["delete"] = 1;

        convertResult["grid"] = tempJson;
        return convertResult;
    }

    //set ordering in table by clicking
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    //to handle the search
    search(data) {
        this.totalItems = data.length;
        this.setPage(1);
    }

    //set current page
    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };
}

let MasterCompany = {
    controller: MasterCompanyController,
    controllerAs: 'me',
    template: require('./MasterCompany.html')
}

export { MasterCompany }