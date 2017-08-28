import * as angular from 'angular';
import * as Service from '../Services';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as mustache from 'mustache';

export class DeliveryLegController implements angular.IController {
    static $inject = ['DeliveryLegService', '$rootScope'];

    constructor(deliveryLegService: Service.DeliveryLegService, root: angular.IRootScopeService) {
        this.deliveryLegService = deliveryLegService;
        this.root = root;
    }
    deliveryLegService: Service.DeliveryLegService;
    root: angular.IRootScopeService

    // Property model and array
    deliveryLegViewModels: Service.DeliveryLegViewModel[];
    deliveryLegViewModel: Service.DeliveryLegLeadTimeViewModel;
    deliveryLegPostPutViewModel: Service.DeliveryLegPostPutViewModel;
    deliveryLegLocationViewModels: Service.DeliveryLegLocationViewModel[];
    cityLegList: Service.DeliveryLegCityListViewModel[];
    search = {};

    // Property bufferTime
    bufferHari: number = 0;
    bufferJam: number = 0;
    bufferMenit: number = 0;

    // Property boolean untuk show, required, dan disabled
    editCheck: boolean;
    checkBoxChecked: boolean = false;
    disabledCodeInput: boolean
    pageState: boolean = true;

    // Property pattern
    codePattern: RegExp = new RegExp('^[a-zA-Z0-9]+$');
    namePattern: RegExp = new RegExp('^[a-zA-Z0-9\\s\-.&,\'/]+$');

    $onInit() {
        this.getAll();
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.getAll();
        });
        this.deliveryLegPostPutViewModel = new Service.DeliveryLegPostPutViewModel();
    }

    getAll() {
        return this.deliveryLegService.getAll().then(response => {
            this.deliveryLegViewModels = response.data.viewModels as Service.DeliveryLegViewModel[];
            this.deliveryLegLocationViewModels = response.data.deliveryLegLocations as Service.DeliveryLegLocationViewModel[];
            this.cityLegList = response.data.cityLegCodes as Service.DeliveryLegCityListViewModel[];

            angular.forEach(this.deliveryLegViewModels, tempViewModel => {
                tempViewModel.locationFromString = this.showLocationDetail(tempViewModel.locationFrom);
                tempViewModel.locationToString = this.showLocationDetail(tempViewModel.locationTo);
                tempViewModel.cityLegCodeString = this.showCityCodeDetail(tempViewModel.cityLegCode);
            });
        }).catch(response => {
            if (response.status == '500') {
                Alertify.error('Koneksi ke server bermasalah');
            }
        });
    }

    // Show location name
    showLocationDetail(locationCode: string) {
        let tempLocationModel = _.find(this.deliveryLegLocationViewModels, ['locationCode', locationCode]);
        return tempLocationModel.locationCode + ' - ' + tempLocationModel.name;
    }

    showCityCodeDetail(cityLegCode: string) {
        let cityLegViewModel = _.find(this.cityLegList, ['cityLegCode', cityLegCode]);
        return cityLegViewModel.cityLegCode + ' - ' + cityLegViewModel.cityLegName;
    }

    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.deliveryLegCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "DeliveryLeg";
        info.title = "Delivery Leg";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "DeliveryLeg";
        info.title = "Delivery Leg";
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }

    // Convert bufferMinute to day
    convertToDay(bufferTime: number) {
        let menitString = bufferTime % 60 + " Menit";
        bufferTime = (bufferTime - bufferTime % 60) / 60;
        let jamString = bufferTime % 24 + " Jam ";
        bufferTime = (bufferTime - bufferTime % 24) / 24;
        let hariString = bufferTime + " Hari ";
        let bufferDetail = hariString + jamString + menitString;
        return bufferDetail;
    }

    checkBoxClicked() {
        this.checkBoxChecked = true;
    }

    fillData(item: Service.DeliveryLegViewModel) {
        let tempModel = new Service.DeliveryLegPostPutViewModel();
        tempModel.deliveryLegCode = item.deliveryLegCode;
        tempModel.name = item.name;
        tempModel.locationFrom = item.locationFrom;
        tempModel.locationTo = item.locationTo;
        tempModel.cityLegCode = item.cityLegCode;
        if (item.needSJKB == "Ya") {
            tempModel.needSJKB = true;
        } else {
            tempModel.needSJKB = false;
        }
        return tempModel;
    }

    // Fill form with selected data
    fillForm(item: Service.DeliveryLegViewModel) {
        this.deliveryLegPostPutViewModel = this.fillData(item);

        let bufferMinutes = item.bufferMinutes;
        this.bufferMenit = bufferMinutes % 60;
        bufferMinutes = (bufferMinutes - this.bufferMenit) / 60;
        this.bufferJam = bufferMinutes % 24;
        bufferMinutes = (bufferMinutes - this.bufferJam) / 24;
        this.bufferHari = bufferMinutes;

        this.editCheck = true;
        this.disabledCodeInput = true;
        this.checkBoxChecked = true;
    }

    checkSameLocation() {
        let tempModel = this.deliveryLegPostPutViewModel;
        if (tempModel.locationFrom != null && (tempModel.locationFrom == tempModel.locationTo)) {
            return true;
        }
        return false;
    }

    // Get total minute
    countMinutes() {
        let totalMenit: number = this.bufferHari * 24 * 60 + this.bufferJam * 60 + this.bufferMenit * 1;
        return totalMenit;
    }

    hasDuplicate() {
        let deliveryLegCode = this.deliveryLegPostPutViewModel.deliveryLegCode != null ? this.deliveryLegPostPutViewModel.deliveryLegCode.toUpperCase() : '';
        let tempModel = _.find(this.deliveryLegViewModels, ['deliveryLegCode', deliveryLegCode]);
        if (tempModel == null) {
            return false;
        } else {
            return true;
        }
    }

    // Confirmation on alertify
    confirmationData(item: Service.DeliveryLegPostPutViewModel) {
        let jsonData = {};
        jsonData['Delivery Leg Code'] = item.deliveryLegCode;
        jsonData['Delivery Leg'] = item.name;
        jsonData['Lokasi Asal'] = this.showLocationDetail(item.locationFrom);
        jsonData['Lokasi Tujuan'] = this.showLocationDetail(item.locationTo);
        jsonData['Kode City Leg'] = this.showCityCodeDetail(item.cityLegCode);
        jsonData['Buffer Time'] = this.bufferHari + ' Hari ' + this.bufferJam + ' Jam ' + this.bufferMenit + ' Menit';
        jsonData['Menggunakan SJKB'] = item.needSJKB == true ? "Ya" : "Tidak";
        return jsonData;
    }

    disableButton() {
        let temp = this.deliveryLegPostPutViewModel;
        if (temp.deliveryLegCode == undefined || temp.name == undefined || temp.locationFrom == undefined || temp.locationTo == undefined || temp.cityLegCode == undefined || this.checkBoxChecked == false) {
            return true;
        }
        if (temp.locationFrom == temp.locationTo) {
            return true;
        }
        if (this.bufferHari == undefined || this.bufferJam == undefined || this.bufferMenit == undefined) {
            return true;
        }
        return false;
    }

    // Send data to database
    submitData(form: angular.IFormController) {
        this.deliveryLegPostPutViewModel.bufferMinutes = this.countMinutes();
        this.deliveryLegPostPutViewModel.deliveryLegCode.trim();
        Alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('insert', this.confirmationData(this.deliveryLegPostPutViewModel))),
            () => {
                if (this.hasDuplicate() == true) {
                    Alertify.error('Kode Delivery Leg telah terdaftar')
                } else {
                    this.deliveryLegService.sendData(this.deliveryLegPostPutViewModel).then(response => {
                        Alertify.success('Data berhasil disimpan');
                        this.getAll();
                        this.setPristine(form);
                    }).catch(response => {
                        if (response.status == '500') {
                            Alertify.error('Koneksi ke server bermasalah');
                        } else if (response.data == 'DUPLICATE') {
                            Alertify.error('Kode Delivery Leg telah terdaftar');
                        } else {
                            Alertify.error('Data gagal disimpan');
                        }
                    });
                }
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Update data to database
    updateData(form: angular.IFormController) {
        this.deliveryLegPostPutViewModel.bufferMinutes = this.countMinutes();
        Alertify.confirm(
            'Konfirmasi',
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('update', this.confirmationData(this.deliveryLegPostPutViewModel))),
            () => {
                this.deliveryLegService.updateData(this.deliveryLegPostPutViewModel).then(() => {
                    Alertify.success('Data berhasil disimpan');
                    this.getAll();
                    this.setPristine(form);
                }).catch(response => {
                    if (response.status == '500') {
                        Alertify.error('Koneksi ke server bermasalah');
                    } else {
                        Alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Delete data from database
    deleteData(form: angular.IFormController, item: Service.DeliveryLegViewModel) {
        let tempModel = this.fillData(item);
        Alertify.confirm(
            "Konfirmasi",
            mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', this.confirmationData(tempModel))),
            () => {
                return this.deliveryLegService.deleteData(tempModel.deliveryLegCode).then(response => {
                    Alertify.success('Data berhasil di hapus');
                    this.getAll();
                    this.setPristine(form);
                }).catch(response => {
                    if (response.status == '500') {
                        Alertify.error('Koneksi ke server bermasalah');
                    } else {
                        Alertify.error('Data gagal disimpan');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

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

    // Set pristine
    setPristine(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.deliveryLegPostPutViewModel = new Service.DeliveryLegPostPutViewModel();
        this.deliveryLegPostPutViewModel.deliveryLegCode = null;
        this.deliveryLegPostPutViewModel.name = null;
        this.deliveryLegPostPutViewModel.needSJKB = undefined;
        this.bufferHari = 0;
        this.bufferJam = 0;
        this.bufferMenit = 0;
        this.editCheck = false;
        this.disabledCodeInput = false;
        this.checkBoxChecked = false;
        this.search = {};
    }

    // Pagination
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    setPage(page: number) {
        this.currentPage = page;
    };
}

let DeliveryLegComponent = {
    controller: DeliveryLegController,
    controllerAs: 'me',
    template: require('./deliveryLeg.html')
}

export { DeliveryLegComponent }