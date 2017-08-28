import * as Services from '../Services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as moment from 'moment';
import * as angular from "angular";
import * as lodash from 'lodash'

export class MaintenanceShiftKerjaController implements angular.IController {
    static $inject = ['MaintenanceShiftKerjaService'];
    constructor(MaintenanceKonfigurasi: Services.MaintenanceShiftKerjaService) {
        this.maintenanceShift = MaintenanceKonfigurasi;
    };
    maintenanceShift: Services.MaintenanceShiftKerjaService;
    listAllData: Services.MaintenanceShiftKerjaFullModel;
    fullViewModel: Services.MaintenanceShiftKerjaPostViewModel[];
    singleViewModel: Services.MaintenanceShiftKerjaPostViewModel;
    hourStep: number = 1;
    minuteStep: number = 1;
    editValue: boolean = false;
    isLoading: boolean = true;
    singleProd = {};
    searchString = {};

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    dateChange(Form: angular.IFormController) {
        this.dateEndoption.minDate = moment(this.singleViewModel.start).add(1, 'minutes').toDate();
        this.singleViewModel.finish = null;
        Form.$setUntouched;
    }
    dateEndoption = {
        minDate: new Date()
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    $onInit() {
        this.getData();
    }

    getData() {
        this.maintenanceShift.getData().then(response => {
            this.listAllData = response.data as Services.MaintenanceShiftKerjaFullModel;
            this.totalItems = this.listAllData.maintenanceShiftKerjaFullModel.length;
            this.listAllData.maintenanceShiftKerjaFullModel.forEach(Q => {
                Q.penampungDateFrom = moment(Q.start).format("D-MMM-YYYY HH:mm").toString();
                Q.penampungDateTo = moment(Q.finish).format("D-MMM-YYYY HH:mm").toString();
            });
            
        }).catch(response => {
                if (response.status == "500") { 
                    alertify.error("Koneksi ke server bermasalah");
                }
                
            }).finally(()=>{
                this.isLoading = false;
            })
    }
    selectSingleData(item: Services.MaintenanceShiftKerjaPostViewModel) {
        this.editValue = true;
        this.singleViewModel = new Services.MaintenanceShiftKerjaPostViewModel();
        this.singleViewModel.locationWorkHourId = item.locationWorkHourId;
        this.singleViewModel.shiftCode = item.shiftCode;
        this.singleViewModel.locationCode = item.locationCode;
        this.singleViewModel.start = new Date(item.start);
        this.singleViewModel.finish = new Date(item.finish);
    }
    submitData(Form: angular.IFormController) {
        this.singleProd["Kode Lokasi"] = this.singleViewModel.locationCode + ' - ' + lodash.find(this.listAllData.lokasiModel, { 'locationCode': this.singleViewModel.locationCode }).nama.toString();
        this.singleProd["Kode Shift"] = this.singleViewModel.shiftCode + ' - ' + lodash.find(this.listAllData.shiftModel, { 'shiftCode': this.singleViewModel.shiftCode }).description.toString();
        this.singleProd["Jam Mulai"] = moment(this.singleViewModel.start).format("D-MMM-YYYY HH:mm");
        this.singleProd["Jam Selesai"] = moment(this.singleViewModel.finish).format("D-MMM-YYYY HH:mm");
        alertify.confirm("Konfirmasi Maintenance Shift Kerja", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("insert", this.singleProd)), () => {
            this.maintenanceShift.postData(this.singleViewModel).then(response => {
                this.listAllData = response.data as Services.MaintenanceShiftKerjaFullModel;
                this.totalItems = this.listAllData.maintenanceShiftKerjaFullModel.length;
                this.singleViewModel = new Services.MaintenanceShiftKerjaPostViewModel;
                this.listAllData.maintenanceShiftKerjaFullModel.forEach(Q => {
                    Q.penampungDateFrom = moment(Q.start).format("D-MMM-YYYY HH:mm").toString();
                    Q.penampungDateTo = moment(Q.finish).format("D-MMM-YYYY HH:mm").toString();
                });
                this.searchString = {};
                Form.$setPristine();
                Form.$setUntouched();
                alertify.success("Data berhasil disimpan");
            }).catch(response => {
                if (response.status == "400") {
                    alertify.error("Data gagal disimpan");
                    return;
                }
                if (response.status == "500") {
                    alertify.error("Koneksi ke server bermasalah");
                }

            })
        },
            () => {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });


    }
    editData(Form: angular.IFormController) {
        this.singleProd["Kode Lokasi"] = this.singleViewModel.locationCode + ' - ' + lodash.find(this.listAllData.lokasiModel, { 'locationCode': this.singleViewModel.locationCode }).nama.toString();
        this.singleProd["Kode Shift"] = this.singleViewModel.shiftCode + ' - ' + lodash.find(this.listAllData.shiftModel, { 'shiftCode': this.singleViewModel.shiftCode }).description.toString();
        this.singleProd["Jam Mulai"] = moment(this.singleViewModel.start).format("D-MMM-YYYY HH:mm");
        this.singleProd["Jam Selesai"] = moment(this.singleViewModel.finish).format("D-MMM-YYYY HH:mm");
        alertify.confirm("Konfirmasi Maintenance Shift Kerja", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.singleProd)), () => {
            this.maintenanceShift.updateData(this.singleViewModel).then(response => {
                this.listAllData = response.data as Services.MaintenanceShiftKerjaFullModel;
                this.totalItems = this.listAllData.maintenanceShiftKerjaFullModel.length;
                this.editValue = false;
                this.searchString = {};
                this.listAllData.maintenanceShiftKerjaFullModel.forEach(Q => {
                    Q.penampungDateFrom = moment(Q.start).format("D-MMM-YYYY HH:mm").toString();
                    Q.penampungDateTo = moment(Q.finish).format("D-MMM-YYYY HH:mm").toString();
                });
                this.singleViewModel = new Services.MaintenanceShiftKerjaPostViewModel;
                Form.$setPristine();
                Form.$setUntouched();
                alertify.success("Data berhasil disimpan");
            }).catch(response => {
                if (response.status == "400") {
                    alertify.error("Data gagal disimpan");
                    return;
                }
                if (response.status == "500") {
                    alertify.error("Koneksi ke server bermasalah");
                }
            })
        },
            () => {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
    deleteData(data: Services.MaintenanceShiftKerjaPostViewModel, Form: angular.IFormController) {
        this.singleProd["Kode Lokasi"] = data.locationCode + ' - ' + lodash.find(this.listAllData.lokasiModel, { 'locationCode': data.locationCode }).nama.toString();
        this.singleProd["Kode Shift"] = data.shiftCode + ' - ' + lodash.find(this.listAllData.shiftModel, { 'shiftCode': data.shiftCode }).description.toString();
        this.singleProd["Jam Mulai"] = moment(data.start).format("D-MMM-YYYY HH:mm");
        this.singleProd["Jam Selesai"] = moment(data.finish).format("D-MMM-YYYY HH:mm");
        alertify.confirm("Konfirmasi Maintenance Shift Kerja", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("delete", this.singleProd)), () => {
            this.maintenanceShift.deleteData(data.locationWorkHourId).then(response => {
                this.listAllData = response.data as Services.MaintenanceShiftKerjaFullModel;
                this.singleViewModel = new Services.MaintenanceShiftKerjaPostViewModel;
                this.totalItems = this.listAllData.maintenanceShiftKerjaFullModel.length;
                this.listAllData.maintenanceShiftKerjaFullModel.forEach(Q => {
                    Q.penampungDateFrom = moment(Q.start).format("D-MMM-YYYY HH:mm").toString();
                    Q.penampungDateTo = moment(Q.finish).format("D-MMM-YYYY HH:mm").toString();
                });
                Form.$setPristine();
                Form.$setUntouched();
                this.searchString = {};
                alertify.success("Data berhasil dihapus");
            }).catch(response => {
                if (response.status == "400") {
                    alertify.error("Data gagal dihapus");
                    return;
                }
                if (response.status == "500") { 
                    alertify.error("Koneksi ke server bermasalah");
                }
                
            })
        },
            () => {
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });
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
    resetForm(Form: angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();
        this.singleViewModel = new Services.MaintenanceShiftKerjaPostViewModel;
        this.editValue = false;
    }
}

export class MaintenanceShiftKerjaComponent implements angular.IComponentOptions {
    controller = MaintenanceShiftKerjaController;
    controllerAs = 'me';
    template = require('./MaintenanceShiftKerja.html');
}