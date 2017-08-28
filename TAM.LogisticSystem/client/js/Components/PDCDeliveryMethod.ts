import * as angular from 'angular';
import * as uib from 'angular-ui-bootstrap';
import * as service from '../services';
import * as lodash from 'lodash';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';

export class PDCDeliveryMethodController implements angular.IController {
    static $inject = ['PDCDeliveryMethodService','$scope'];

    pdcDeliveryMethodService: service.PDCDeliveryMethodService;

    $rootScope: angular.IRootScopeService;
    $scope: ng.IScope;

    pdcDeliveryViewModel: service.PDCDeliveryViewModel[];
    pdcDeliveryCreateViewModel: Array<service.PDCDeliveryCreateViewModel> = [];
    pdcDeliveryTempViewModel: service.PDCDeliveryTempViewModel;
    pdcDeliveryDeleteViewModel: service.PDCDeliveryDeleteViewModel;
    pdcDeliveryDeleteDetailModel: Array<service.PDCDeliveryDeleteDetailModel> = [];
    pdcBranchViewModel: service.PDCBranchViewModel[];
    pdcLocationViewModel: service.PDCLocationViewModel[];
    pdcDeliveryMethodViewModel: service.PDCDeliveryMethodViewModel[];

    checkPDC: boolean = false;

    constructor(pdcDeliveryMethodService: service.PDCDeliveryMethodService,  protected _$scope: ng.IScope) {
        this.pdcDeliveryMethodService = pdcDeliveryMethodService;
        this.pdcDeliveryViewModel = this.pdcDeliveryViewModel;
        this.$scope = _$scope;
    }

    locationCode: string;
    branchCode: string;
    deliveryMethodCode: string;
    locations: any;
    branches: any;
    deliveryMethods: any;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;
    json = {};
    Search = {};

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            // angular's default (non-strict) internal comparator
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };
    //set current page
    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    //convert json confirm data into the modal
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

    $onInit() {
        this.refreshData();
        this.pdcDeliveryTempViewModel = new service.PDCDeliveryTempViewModel();
        this.locations = null;
        this.branches = null;
    }

    pdcSelect: any;

    selectItemOnChange() {
        this.pdcSelect = lodash.filter(this.pdcBranchViewModel, ['branchCode', this.pdcLocationViewModel['locationCode']]);
    }

    refreshData() {
        this.pdcDeliveryMethodService.getAll().then(response => {
            this.pdcDeliveryViewModel = response.data as service.PDCDeliveryViewModel[];
            this.totalItems = this.pdcDeliveryViewModel.length;
        });

        this.pdcDeliveryMethodService.getLocation().then(response => {
            this.pdcLocationViewModel = response.data;           
        });

        this.pdcDeliveryMethodService.getBranch().then(response => {
            this.pdcBranchViewModel = response.data;
        });

        this.pdcDeliveryMethodService.getDeliveryMethod().then(response => {
            this.pdcDeliveryMethodViewModel = response.data;
        });
    }

    reset(Form: angular.IFormController) {
        this.pdcDeliveryTempViewModel = new service.PDCDeliveryTempViewModel();
        this.Search = {};
        if (Form) {
            Form.$setPristine();
            Form.$setUntouched();
            this.locations = null;
            this.branches = null;
            this.deliveryMethods = null;
   
        }
    }

    validateAllComboBox() {
        if (this.pdcDeliveryTempViewModel.locationCode === null) {
            return false;
        }
        if (this.pdcDeliveryTempViewModel.branchCode === null) {
            return false;
        }
        if (this.pdcDeliveryTempViewModel.deliveryMethodCode === null) {
            return false;
        }
        return true;
    }

    addDetail(Form: angular.IFormController) {
        if (this.validateAllComboBox()) {
            let data = new service.PDCDeliveryTempViewModel();
            this.checkLocationAndBranch();
            if (this.message == true) {
                alertify.error("Kombinasi PDC dan Kode Branch telah terdaftar");
            } else {
                data.locationCode = this.locations.locationCode;
                data.locationName = this.locations.name;
                data.branchCode = this.branches.branchCode;
                data.branchName = this.branches.branchName;
                data.deliveryMethodCode = this.deliveryMethods.deliveryMethodCode;
                data.deliveryMethodName = this.deliveryMethods.name;
                this.pdcDeliveryCreateViewModel.push(data);
                alertify.success("Data berhasil ditambah");
                this.reset(Form);
               
            }
        }
    }

    clearDetail(Form: angular.IFormController) {
        this.pdcDeliveryCreateViewModel = null;
        this.pdcDeliveryCreateViewModel = new Array<service.PDCDeliveryCreateViewModel>();
        this.reset(Form);
    }
    s
    message: boolean

    checkLocation() {
        //this.branches["branchCode"] = null;
        this.checkLocationAndBranch();
    }

    checkLocationAndBranch() {
        if (this.branches !== null && this.locations !== null) {
            this.pdcDeliveryMethodService.getId(this.locations["locationCode"], this.branches["branchCode"]).then(response => {
                if (response.data["locationCode"] === undefined && response.data["branchCode"] === undefined) {
                    this.message = false;
                } else {
                    this.message = true;
                }
            });
        }    
        
    }

    //insert data detail to database
    createData(Form: angular.IFormController) {
        if (this.validateAllComboBox()) {
            let detail = new service.PDCDeliveryCreateViewModel();
            detail.locationCode = this.locationCode;
            detail.branchCode = this.branchCode;
            detail.deliveryMethodCode = this.deliveryMethodCode;

            let json = {}
            json["save"] = detail;
            alertify.confirm(
                "Konfirmasi",
                mustache.render(require("./alertify/PDCDeliveryMethodAlertify.html"), json),
                () => {
                    this.pdcDeliveryMethodService.postData(this.pdcDeliveryCreateViewModel).then(response => {
                        alertify.success("Data berhasil disimpan");
                        this.refreshData();            
                        this.clearDetail(Form);
                    }).catch(response => {
                        if (response.status == "500") {
                            alertify.error("Koneksi ke server bermasalah");
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });          
        }
    }

    //delete selected row
    delete(data, form: angular.IFormController) {
        this.pdcDeliveryDeleteViewModel = new service.PDCDeliveryDeleteViewModel();
        this.pdcDeliveryDeleteViewModel.locationCode = data.locationCode;
        this.pdcDeliveryDeleteViewModel.branchCode = data.branchCode;

        let json = {};
        json["PDC"] = data.locationData;
        json["Kode Branch"] = data.branchData;
        json["Kode Moda"] = data.deliveryMethodData;
        
        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', json)),
            () => {
                this.pdcDeliveryMethodService.deleteData(this.pdcDeliveryDeleteViewModel).then(response => {
                    this.pdcDeliveryViewModel;
                    this.totalItems = this.pdcDeliveryViewModel.length;
                    alertify.success('Data berhasil dihapus');
                    this.refreshData();
                    this.reset(form);
                }).catch(response => {
                    alertify.error('Data gagal dihapus');
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteDetail(item) {
        this.locationCode = item.locationCode

        let json = {};
        json["PDC"] = item.locationCode + ' - ' + item.locationName;
        json["Kode Branch"] = item.branchCode + ' - ' + item.branchName;
        json["Kode Moda"] = item.deliveryMethodCode + ' - ' + item.deliveryMethodName;

        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', json)),
            () => {
                let deleteDetail = lodash.findIndex(this.pdcDeliveryCreateViewModel, {
                    'locationCode': this.locationCode,
                });
                 //temporary solution
                this.$scope.$apply(() => {
                    let check = lodash.pullAt(this.pdcDeliveryCreateViewModel, deleteDetail);
                    if (check) {
                        let found = lodash.filter(this.pdcDeliveryCreateViewModel, {
                            'locationCode': this.locationCode,
                        });
                        alertify.success("Data berhasil dihapus");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
}

export class PDCDeliveryMethod implements angular.IComponentOptions {
    controller = PDCDeliveryMethodController;
    controllerAs = 'me';

    template = require('./PDCDeliveryMethod.html');
}