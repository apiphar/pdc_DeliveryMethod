
import * as Service from '../services';
import * as lodash from 'lodash';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';

export class MaintenanceKonfigurasiExportFileDccpController implements angular.IController {
    //injection
    static $inject = ["MaintenanceKonfigurasiExportFileDccpService"];
    //constructor - DI
    constructor(MaintenanceKonfigurasi: Service.MaintenanceKonfigurasiExportFileDccpService) {
        this.maintenanceKonfigurasi = MaintenanceKonfigurasi;
    };
    //declaration Type
    newPostModel: Service.MaintenanceKonfigurasiExportFileDccpPostModel;
    maintenanceKonfigurasi: Service.MaintenanceKonfigurasiExportFileDccpService;
    listDataConfig: Service.MaintenanceKonfigurasiExportFileDccpModel[];
    singleDataConfig: Service.MaintenanceKonfigurasiExportFileDccpModel;
    editValue: boolean = false;

    pageSizes: number[] = [5, 10, 15, 20];
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

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    //on init, configuration opening-closing div and receiving new data of list Model
    $onInit() {
        this.getAllData();
        this.singleDataConfig = new Service.MaintenanceKonfigurasiExportFileDccpModel;
    }

    //getAllConfigData
    getAllData() {
        this.maintenanceKonfigurasi.getAllData().then(response => {
            this.listDataConfig = response.data as Service.MaintenanceKonfigurasiExportFileDccpModel[];
            this.totalItems = this.listDataConfig.length;
        });
    }

    //Post New COnfig Data
    postData(postModel: Service.MaintenanceKonfigurasiExportFileDccpModel) {
        if (!this.validation()) {
            return 0;
        }
        alertify.confirm(
            'Konfirmasi Maintenance Konfigurasi Export Excel File Dccp (Upload)',
            mustache.render(require('./alertify/KonfirmasiMaintenanceKonfigurasiExportExcelFileDccpAlertify.html'), this.singleDataConfig),
            () => {
                this.newPostModel = new Service.MaintenanceKonfigurasiExportFileDccpPostModel();
                this.newPostModel.description = postModel.description;
                this.newPostModel.rangeEnd = postModel.rangeEnd;
                this.newPostModel.rangeStart = postModel.rangeStart;
                this.maintenanceKonfigurasi.postData(this.newPostModel).then(response => {
                    this.listDataConfig = response.data as Service.MaintenanceKonfigurasiExportFileDccpModel[];
                    alertify.success("Success Added New Config");
                    this.singleDataConfig = new Service.MaintenanceKonfigurasiExportFileDccpModel();
                }).catch(response => {
                    alertify.error("Failed Add New Config");
                });
            },
            () => {
                alertify.error('Membatalkan Simpan');
            }
        );
        // TIE: START
        return 0;
        // TIE: END
    }

    //get selected item for edit button
    getSingleModel(item: Service.MaintenanceKonfigurasiExportFileDccpModel) {
        this.editValue = true;
        this.singleDataConfig = new Service.MaintenanceKonfigurasiExportFileDccpModel();
        this.singleDataConfig.description = item.description;
        this.singleDataConfig.rangeEnd = item.rangeEnd;
        this.singleDataConfig.rangeStart = item.rangeStart;
        this.singleDataConfig.id = item.id;
    }

    //delete selected Config Data
    deleteSelectedData(item: Service.MaintenanceKonfigurasiExportFileDccpModel) {
        alertify.confirm("Sure you want to Delete?.",
            () => {
                this.maintenanceKonfigurasi.deleteSelectedData(item.id).then(response => {
                    this.listDataConfig = response.data as Service.MaintenanceKonfigurasiExportFileDccpModel[];
                    this.getAllData();
                    alertify.success("Delete Success");
                }).catch(response => {
                    alertify.error("Error Delete Dccp Config.");
                });

            },
            () => {
                alertify.error('Cancel');
            });

    }

    //clear Form (cancel button)
    cancel(Form: angular.IFormController) {
        this.editValue = false;
        Form.$setPristine();
        Form.$setUntouched();
        this.singleDataConfig = new Service.MaintenanceKonfigurasiExportFileDccpModel();
    }

    //Update selected data
    updateData(updateData: Service.MaintenanceKonfigurasiExportFileDccpModel) {
        if (!this.validation()) {
            return 0;
        }
        alertify.confirm(
            'Konfirmasi Maintenance Konfigurasi Export Excel File Dccp (Upload)',
            mustache.render(require('./alertify/KonfirmasiMaintenanceKonfigurasiExportExcelFileDccpAlertify.html'), this.singleDataConfig),
            () => {
                this.maintenanceKonfigurasi.updateData(updateData).then(response => {
                    this.listDataConfig = response.data as Service.MaintenanceKonfigurasiExportFileDccpModel[];
                    this.editValue = false;
                    this.singleDataConfig = new Service.MaintenanceKonfigurasiExportFileDccpModel();
                    this.getAllData();
                    alertify.success("Update Success");
                }).catch(response => {
                    alertify.error("Update Failed");
                })
            },
            () => {
                alertify.error('Membatalkan Ubah');
            }
        );
        // TIE: START
        return 0;
        // TIE: END
    }

    validation() {
        if (this.singleDataConfig.description == null || this.singleDataConfig.rangeEnd == null || this.singleDataConfig.rangeStart == null) {
            alertify.error('Semua field harus terisi');
            return false;
        }
        else if (!this.singleDataConfig.description.match(/^[\w., -]+$/)) {
            alertify.error('Keterangan tidak boleh symbol');
            return false;
        }
        else if (!this.singleDataConfig.rangeStart.match(/^[\w]+$/)) {
            alertify.error('Range alamat cell awal tidak boleh symbol');
            return false;
        }
        else if (!this.singleDataConfig.rangeEnd.match(/^[\w]+$/)) {
            alertify.error('Range alamat cell awal tidak boleh symbol');
            return false;
        }
        return true;
    }

}
let maintenanceKonfigurasiExportFileDccp = {
    controller: MaintenanceKonfigurasiExportFileDccpController,
    controllerAs: 'me',
    template: require("./MaintenanceKonfigurasiExportFileDccp.html")
}

export { maintenanceKonfigurasiExportFileDccp };