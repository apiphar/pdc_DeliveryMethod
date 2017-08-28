import * as angular from 'angular';
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as moment from 'moment';
import * as mustache from 'mustache';
export class PenyesuaianTanggalProduksiController implements angular.IController {
    //injection
    static $inject = ["PenyesuaianTanggalProduksiService"];
    //constructor - DI
    constructor(PenyesuaianTanggalProduksiService: Service.PenyesuaianTanggalProduksiService) {
        this.penyesuaianTanggalProduksiService = PenyesuaianTanggalProduksiService;
    }


    //declaration
    listTotalData: Service.TotalModel;
    listProkduksiModel: Service.PenyesuaianTanggalProduksiModel[];
    listPlant: Service.ListPlant[];
    singleProkduksiModel: Service.PenyesuaianTanggalProduksiModel;
    postProkduksiModel: Service.PenyesuaianTanggalProduksiModel;
    penyesuaianTanggalProduksiService: Service.PenyesuaianTanggalProduksiService;
    singleProd = {};
    editTrue: boolean = false;



    //on init, configuration opening-closing div and receiving new data of list Model
    $onInit() {
        this.getAllData();
        this.singleProkduksiModel = new Service.PenyesuaianTanggalProduksiModel;
        this.singleProkduksiModel.akhirBulan = false;
        this.singleProkduksiModel.bukanAkhirBulan = false;
    }

    //Pagination ordering and searching
    pageSizes: number[] = [5, 10, 15, 20];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    //ordering pagination
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    validation() {
        let check = true;
        if (this.singleProkduksiModel.plant == null) {
            alertify.error('Plant harus di pilih');
            check = false;
        }
        if (this.singleProkduksiModel.dateEnd == null) {
            alertify.error('Jam akhir harus di isi');
            check = false;
        }
        if (this.singleProkduksiModel.dateStart == null) {
            alertify.error('Jam mulai harus di isi');
            check = false;
        }
        return check;
    }

    //searching in pagination
    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }
    //setting page pagination
    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    //get All Tanggal Produksi & Plant Service
    getAllData() {
        this.penyesuaianTanggalProduksiService.getAll().then(response => {
            this.listTotalData = response.data as Service.TotalModel;
            this.listProkduksiModel = this.listTotalData.allViewModel;
            angular.forEach(this.listProkduksiModel, Q => {
                Q.penampungDateEnd = this.DateToStringPenampung(Q.dateEnd);
                Q.penampungDateStart = this.DateToStringPenampung(Q.dateStart);
            });
            this.listPlant = this.listTotalData.allPlantViewModel;
            this.totalItems = this.listProkduksiModel.length;
        })
    }

    //get single model after click edit
    getSingleModel(item: Service.PenyesuaianTanggalProduksiModel) {
        this.singleProkduksiModel = new Service.PenyesuaianTanggalProduksiModel;
        this.singleProkduksiModel.id = item.id;
        this.singleProkduksiModel.plant = item.plant;
        this.singleProkduksiModel.nama = item.nama;
        this.singleProkduksiModel.akhirBulan = item.akhirBulan;
        this.singleProkduksiModel.bukanAkhirBulan = item.bukanAkhirBulan;
        this.singleProkduksiModel.dateEnd = item.dateEnd;
        this.singleProkduksiModel.dateStart = item.dateStart;
        this.editTrue = true;
    }

    //Create new Data
    postData() {
        if (this.validation() == false) {
            return 0;
        }
        let dateEndDate = new Date(this.singleProkduksiModel.dateEnd);
        let dateStartDate = new Date(this.singleProkduksiModel.dateStart);
        this.singleProkduksiModel.penampungDateEnd = this.DateToStringPenampung(dateEndDate);
        this.singleProkduksiModel.penampungDateStart = this.DateToStringPenampung(dateStartDate);
        if (this.singleProkduksiModel.bukanAkhirBulan == false || this.singleProkduksiModel.bukanAkhirBulan.toString() == 'false') {
            this.singleProkduksiModel.penampungBab = "Tanggal Sistem";
        } else {
            this.singleProkduksiModel.penampungBab = "Tanggal Produksi";
        }
        if (this.singleProkduksiModel.akhirBulan == false || this.singleProkduksiModel.akhirBulan.toString() == 'false') {
            this.singleProkduksiModel.penampungAkhirBulan = "Tanggal Sistem";
        } else {
            this.singleProkduksiModel.penampungAkhirBulan = "Tanggal Produksi";
        }

        this.singleProd["Plant"] = this.singleProkduksiModel.plant;
        this.singleProd["Akhir Bulan"] = this.singleProkduksiModel.penampungAkhirBulan;
        this.singleProd["Bukan Akhir Bulan"] = this.singleProkduksiModel.penampungBab;
        this.singleProd["Jam Mulai"] = this.singleProkduksiModel.penampungDateStart;
        this.singleProd["Jam Selesai"] = this.singleProkduksiModel.penampungDateEnd;


        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("insert", this.singleProd)), () => {
            this.penyesuaianTanggalProduksiService.postData(this.singleProkduksiModel).then(response => {
                this.listTotalData = response.data as Service.TotalModel;
                this.listProkduksiModel = this.listTotalData.allViewModel;
                this.listPlant = this.listTotalData.allPlantViewModel;
                angular.forEach(this.listProkduksiModel, Q => {
                    Q.penampungDateEnd = this.DateToStringPenampung(Q.dateEnd);
                    Q.penampungDateStart = this.DateToStringPenampung(Q.dateStart);
                });
                alertify.success("Sukses menambahkan data");
                this.singleProkduksiModel = new Service.PenyesuaianTanggalProduksiModel;
                this.totalItems = this.listProkduksiModel.length;
            }).catch(response => {
                alertify.error("Gagal menambahkan data, pastikan tidak ada data yang terduplikat dan semua field terisi");
            })
        },
            () => {
                alertify.error('Batal');
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });;

        // TIE: START
        return 0;
        // TIE: END
    }

    //update Existing Data
    updateData() {
        if (this.validation() == false) {
            return 0;
        }

        let dateEndDate = new Date(this.singleProkduksiModel.dateEnd);
        let dateStartDate = new Date(this.singleProkduksiModel.dateStart);
        this.singleProkduksiModel.penampungDateStart = this.DateToStringPenampung(dateStartDate);
        this.singleProkduksiModel.penampungDateEnd = this.DateToStringPenampung(dateEndDate);
        if (this.singleProkduksiModel.bukanAkhirBulan == false || this.singleProkduksiModel.bukanAkhirBulan.toString() == 'false') {
            this.singleProkduksiModel.penampungBab = "Tanggal Sistem";
        } else {
            this.singleProkduksiModel.penampungBab = "Tanggal Produksi";
        }
        if (this.singleProkduksiModel.akhirBulan == false || this.singleProkduksiModel.akhirBulan.toString() == 'false') {
            this.singleProkduksiModel.penampungAkhirBulan = "Tanggal Sistem";
        } else {
            this.singleProkduksiModel.penampungAkhirBulan = "Tanggal Produksi";
        }
        this.singleProd["Plant"] = this.singleProkduksiModel.plant;
        this.singleProd["Akhir Bulan"] = this.singleProkduksiModel.penampungAkhirBulan;
        this.singleProd["Bukan Akhir Bulan"] = this.singleProkduksiModel.penampungBab;
        this.singleProd["Jam Mulai"] = this.singleProkduksiModel.penampungDateStart;
        this.singleProd["Jam Selesai"] = this.singleProkduksiModel.penampungDateEnd;
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.singleProd)), () => {
            this.penyesuaianTanggalProduksiService.updateData(this.singleProkduksiModel).then(response => {
                this.listTotalData = response.data as Service.TotalModel;
                this.listProkduksiModel = this.listTotalData.allViewModel;
                this.listPlant = this.listTotalData.allPlantViewModel;
                alertify.success("Success Insert Data");
                angular.forEach(this.listProkduksiModel, Q => {
                    Q.penampungDateEnd = this.DateToStringPenampung(Q.dateEnd);
                    Q.penampungDateStart = this.DateToStringPenampung(Q.dateStart);
                });
            }).catch(response => {
                alertify.error("Gagal mengubah data");
            })
        },
            () => {
                alertify.error('Batal');
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });;
        // TIE: START
        return 0;
        // TIE: END
    }

    //clearForm
    cancel(Form: angular.IFormController) {
        this.singleProkduksiModel = new Service.PenyesuaianTanggalProduksiModel;
        Form.$setPristine();
        Form.$setUntouched();
        this.editTrue = false;
        this.singleProkduksiModel.akhirBulan = false;
        this.singleProkduksiModel.bukanAkhirBulan = false;
    }

    //Delete Existing data
    deleteData(data: Service.PenyesuaianTanggalProduksiModel) {
        if (this.validation() == false) {
            return 0;
        }
        let dateEndDate = new Date(data.dateEnd);
        let dateStartDate = new Date(data.dateStart);
        this.singleProkduksiModel.penampungDateStart = this.DateToStringPenampung(dateStartDate);
        this.singleProkduksiModel.penampungDateEnd = this.DateToStringPenampung(dateEndDate);
        if (data.bukanAkhirBulan == false || data.bukanAkhirBulan.toString() == 'false') {
            data.penampungBab = "Tanggal Sistem";
        } else {
            data.penampungBab = "Tanggal Produksi";
        }
        if (data.akhirBulan == false || data.akhirBulan.toString() == 'false') {
            data.penampungAkhirBulan = "Tanggal Sistem";
        } else {
            data.penampungAkhirBulan = "Tanggal Produksi";
        }
        this.singleProd["Plant"] = this.singleProkduksiModel.plant;
        this.singleProd["Akhir Bulan"] = this.singleProkduksiModel.penampungAkhirBulan;
        this.singleProd["Bukan Akhir Bulan"] = this.singleProkduksiModel.penampungBab;
        this.singleProd["Jam Mulai"] = this.singleProkduksiModel.penampungDateStart;
        this.singleProd["Jam Selesai"] = this.singleProkduksiModel.penampungDateEnd;
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("delete", this.singleProd)),
            () => {
                this.penyesuaianTanggalProduksiService.deleteData(data.plant).then(response => {
                    this.listTotalData = response.data as Service.TotalModel;
                    this.listProkduksiModel = this.listTotalData.allViewModel;
                    angular.forEach(this.listProkduksiModel, Q => {
                        Q.penampungDateEnd = this.DateToStringPenampung(Q.dateEnd);
                        Q.penampungDateStart = this.DateToStringPenampung(Q.dateStart);
                    });
                    this.listPlant = this.listTotalData.allPlantViewModel;
                    this.totalItems = this.listProkduksiModel.length;
                    alertify.success("Sukses menghapus data");
                }).catch(response => {
                    alertify.error("Gagal menghapus data");
                })
            },
            () => {
                alertify.error('Batal');
            }).set('labels', { ok: 'Ya', cancel: 'Tidak' });;
        this.editTrue = false;
        // TIE: START
        return 0;
        // TIE: END
    }
    //date to string
    DateToStringPenampung(date: Date) {
        let dates = new Date(date);
        let penampungDate: string;
        if (dates.getMinutes() < 10 && dates.getHours() < 10) {
            penampungDate = "0" + dates.getHours().toString() + ":0" + dates.getMinutes().toString();
        }
        else if (dates.getHours() < 10) {
            penampungDate = "0" + dates.getHours().toString() + ":" + dates.getMinutes().toString();
        }
        else if (dates.getMinutes() < 10) {
            penampungDate = dates.getHours().toString() + ":0" + dates.getMinutes().toString();
        }
        else {
            penampungDate = dates.getHours().toString() + ":" + dates.getMinutes().toString();
        }
        return penampungDate;
    }
    /**
 * fungsi untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
 * @param action insert,update,delete (salah satu) *case insensitive
 * @param json json data -> { label : value , label2 : value2 }
 */
    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin untuk menambahkan data :";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data :";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data :";
        convertResult["grid"] = tempJson;
        return convertResult;
    }
}


let penyesuaianTanggalProduksi = {
    controller: PenyesuaianTanggalProduksiController,
    controllerAs: 'me',
    template: require("./PenyesuaianTanggalProduksi.html")
}

export { penyesuaianTanggalProduksi };