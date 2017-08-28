import * as Services from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as angular from 'angular';

export class CityLegController implements angular.IController {
    static $inject = ['CityLegService', '$rootScope'];

    constructor(cityLegService: Services.CityLegService, root: angular.IRootScopeService) {
        this.cityLegService = cityLegService;
        this.root = root;
    }
    cityLegService: Services.CityLegService;
    root: angular.IRootScopeService

    // Model property
    cityLegViewModels: Services.CityLegViewModel[];
    cityLegLocation: Services.CityLegLocation[];
    cityLegSendViewModel: Services.CityLegSendViewModel;
    search = {};

    // Pattern property
    codePattern: RegExp = new RegExp('^[A-Za-z0-9]+$');
    namePattern: RegExp = new RegExp('^[a-zA-Z\\s\-.&,\'/]+$');

    // Boolean property
    updateClick: boolean = false;
    pageState: boolean = true;
    onSameLocation: boolean = false;

    // Pagination
    /* START PAGINATION */
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    setPage() {
        this.currentPage = 1;
    };
    /* END PAGINATION */

    // On page load
    $onInit() {
        this.refreshData();
        this.cityLegSendViewModel = new Services.CityLegSendViewModel();
        this.cityLegSendViewModel.calculatingSwappingCost = true;
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.refreshData();
        });

    }

    // Refresh data
    refreshData() {
        return this.cityLegService.getAll().then(response => {
            this.cityLegViewModels = response.data.viewModels as Services.CityLegViewModel[];
            this.cityLegLocation = response.data.cityList as Services.CityLegLocation[];
            angular.forEach(this.cityLegViewModels, (tempModel) => {
                tempModel.cityFromGrid = this.locationDetail(tempModel.cityFrom);
                tempModel.cityToGrid = this.locationDetail(tempModel.cityTo);
            });
        });
    }

    locationDetail(cityForLegCode: string) {
        let tempModel = _.find(this.cityLegLocation, ['cityForLegCode', cityForLegCode]);
        return tempModel.cityForLegCode + ' - ' + tempModel.cityName;
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.cityLegCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "CityLeg";
        info.title = "City Leg";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "CityLeg";
        info.title = "City Leg";
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }

    // Fill model on update/delete button click
    fillModel(item: Services.CityLegViewModel) {
        let tempViewModel = new Services.CityLegSendViewModel();
        tempViewModel.cityLegCode = item.cityLegCode;
        tempViewModel.cityLegName = item.cityLegName;
        tempViewModel.cityFrom = item.cityFrom;
        tempViewModel.cityFromGrid = item.cityFromGrid;
        tempViewModel.cityTo = item.cityTo;
        tempViewModel.cityToGrid = item.cityToGrid;
        if (item.calculatingSwappingCost === "Ya") {
            tempViewModel.calculatingSwappingCost = true;
        } else {
            tempViewModel.calculatingSwappingCost = false;
        }
        return tempViewModel;
    }

    // Fill form on update/delete button click
    fillForm(item: Services.CityLegViewModel) {
        this.cityLegSendViewModel = this.fillModel(item);
        this.updateClick = true;
    }

    hasDuplicate() {
        let cityLegCode: string = this.cityLegSendViewModel.cityLegCode != null ? this.cityLegSendViewModel.cityLegCode.toUpperCase() : '';
        let tempModel = _.find(this.cityLegViewModels, ['cityLegCode', cityLegCode]);
        if (tempModel == null) {
            return false;
        } else {
            return true;
        }
    }

    checkLocation() {
        let tempModel = this.cityLegSendViewModel;
        if (tempModel.cityFrom == null || tempModel.cityTo == null) {
            this.onSameLocation = false;
            return;
        }
        if (tempModel.cityFrom == tempModel.cityTo) {
            this.onSameLocation = true;
            return;
        }
        this.onSameLocation = false;
    }

    // Disable button if form not valid
    disableButton(formInvalidity: boolean) {
        let tempModel = this.cityLegSendViewModel;
        if (formInvalidity == true || tempModel.cityFrom == null || tempModel.cityTo == null || this.onSameLocation == true) {
            return true;
        }
        return false;
    }

    // Send data
    sendData(form: angular.IFormController) {
        this.cityLegSendViewModel.cityLegCode.trim();
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.createJsonData(this.cityLegSendViewModel))),
            () => {
                if (this.hasDuplicate() == true) {
                    alertify.error('Kode City Leg telah terdaftar');
                } else {
                    this.cityLegService.sendData(this.cityLegSendViewModel).then(response => {
                        alertify.success('Data tersimpan');
                        this.setPristine(form);
                        this.refreshData();
                    }).catch(response => {
                        if (response.data == "DUPLICATE") {
                            alertify.error('Kode City Leg telah terdaftar');
                            return;
                        } else {
                            alertify.error('Data gagal disimpan');
                        }
                    });
                }
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Update data
    updateData(form: angular.IFormController) {
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('update', this.createJsonData(this.cityLegSendViewModel))),
            () => {
                this.cityLegService.updateData(this.cityLegSendViewModel).then(response => {
                    alertify.success('Data tersimpan');
                    this.setPristine(form);
                    this.refreshData();
                }).catch(() => {
                    alertify.error('Data gagal disimpan');
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Delete data
    deleteData(item: Services.CityLegViewModel, form: angular.IFormController) {
        let tempModel = this.fillModel(item);
        alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', this.createJsonData(tempModel))),
            () => {
                this.cityLegService.deleteData(tempModel.cityLegCode).then(response => {
                    alertify.success('Data terhapus');
                    this.setPristine(form);
                    this.refreshData();
                }).catch(() => {
                    alertify.error('Data gagal dihapus');
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Generate JSON data for alertify
    createJsonData(item: Services.CityLegSendViewModel) {
        let jsonData = {};
        jsonData["Kode City Leg"] = item.cityLegCode;
        jsonData["City Leg"] = item.cityLegName;
        jsonData["Kode City Leg From"] = this.locationDetail(item.cityFrom);
        jsonData["Kode City Leg To"] = this.locationDetail(item.cityTo);
        jsonData["Menghitung Biaya Swapping"] = item.calculatingSwappingCost == true ? "Ya" : "Tidak";
        return jsonData;
    }

    // Convert JSON data mustache
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

    // Set Pristine
    setPristine(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.cityLegSendViewModel = new Services.CityLegSendViewModel();
        this.cityLegSendViewModel.cityLegCode = null;
        this.cityLegSendViewModel.cityLegName = null;
        this.cityLegSendViewModel.calculatingSwappingCost = true;
        this.updateClick = false;
        this.onSameLocation = false;
        this.search = {};
    }
}

export class CityLegComponent implements angular.IComponentOptions {
    controller = CityLegController;
    controllerAs = 'me';

    template = require('./CityLeg.html');
    bindings = {
        greet: '@'
    };
}