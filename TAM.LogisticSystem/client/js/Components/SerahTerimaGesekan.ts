import * as angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';
import * as mustache from 'mustache';
export class SerahTerimaGesekanController implements angular.IController {
    $inject = ['SerahTerimaGesekanService'];

    //property
    regexCode: RegExp = /^[A-Za-z0-9]+$/;
    SerahTerimaGesekanService: Service.SerahTerimaGesekanService;
    tanggal: Date;
    noSurat: string;
    serahTerimaGesekanViewModels: Service.SerahTerimaGesekanViewModel[];
    serahTerimaGesekanInput: Service.SerahTerimaGesekanInputViewModel = new Service.SerahTerimaGesekanInputViewModel;
    serahTerimaGesekanExcelModel: Service.SerahTerimaGesekanExcelViewModel[]=[];
    jumlahUnit: number = 0;
    jumlahGesekan: number = 0;
    errorMessage: string;
    btnDisabled: boolean;
    loader: boolean = true;

    altInputFormats: any = ['M!/d!/yyyy'];
    cbMain: boolean;

    //pagination
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    //loader

    constructor(SerahTerimaGesekanService: Service.SerahTerimaGesekanService) {
        this.SerahTerimaGesekanService = SerahTerimaGesekanService;
    }


    //function
    $onInit() {

        this.refreshData();
    }

    setPage(pageNo) {
        this.currentPage = pageNo;
    };

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);

    }
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }


    //get alll serah terima gesekan view model
    getAllSerahTerimaGesekanData() {
        this.SerahTerimaGesekanService.serahTerimaGesekan().then(response => {
            this.serahTerimaGesekanViewModels = response.data as Service.SerahTerimaGesekanViewModel[];
            this.jumlahGesekan = 0;
            angular.forEach(this.serahTerimaGesekanViewModels, (data) => {               
                this.jumlahGesekan += data.jumlahGesek;
            });
            this.loader = false;
            this.totalItems = this.jumlahUnit = this.serahTerimaGesekanViewModels.length;
        }).catch(response => {
            if (response.status == "400") {
                Alertify.error(response.data);
                return;
            }
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    //reset all form 
    resetAll(form: angular.IFormController) {
        this.refreshData();
        form.$setPristine();
        form.$setUntouched();
    }

    //refresh data include clearing field
    refreshData() {
        this.tanggal = new Date();
        this.getAllSerahTerimaGesekanData();
        this.btnDisabled = true;
        this.noSurat = null;
        this.errorMessage = null;
        this.cbMain = false;
        angular.forEach(this.serahTerimaGesekanViewModels, data => {
            data.select = false;
        });
    }
    //select all data with checkbox all
    selectAll() {
        angular.forEach(this.serahTerimaGesekanViewModels, data => {
            data.select = this.cbMain;
        });
        this.checkDataSelected();
    }
    //checking if data is checked is true
    checkDataSelected() {
        let isAnySelect = _.filter(this.serahTerimaGesekanViewModels, ['select', true]);
        if (isAnySelect.length > 0 && this.noSurat != null && this.noSurat != "") {
            this.btnDisabled = false;
        } else {
            this.btnDisabled = true;
        }
    }
    //get all data when checked is true
    getTrueData() {
        this.serahTerimaGesekanInput.vehicleId = [];
        this.serahTerimaGesekanExcelModel = [];
        let dataTemp = new Service.SerahTerimaGesekanExcelViewModel();
        let dataSelected = _.filter(this.serahTerimaGesekanViewModels, ['select', true]);
        angular.forEach(dataSelected, data => {
            this.serahTerimaGesekanInput.vehicleId.push(data.vehicleId);
            dataTemp.branch = data.branch;
            dataTemp.color = data.color;
            dataTemp.customerAssign = data.customerAssign;
            dataTemp.frameNumber = data.frameNumber;
            dataTemp.jumlahGesek = data.jumlahGesek;
            dataTemp.katashiki = data.katashiki;
            dataTemp.lokasi = data.lokasi;
            dataTemp.modelName = data.modelName;
            dataTemp.requestedPDD = data.requestedPDD;
            dataTemp.suffix = data.suffix;
            dataTemp.tanggalGesek = data.tanggalGesek;
            this.serahTerimaGesekanExcelModel.push(dataTemp);
        })
    }
    //save and update with generate excel file
    generate(form: angular.IFormController) {
        this.getTrueData();
        this.serahTerimaGesekanInput.excelModel = this.serahTerimaGesekanExcelModel;        
        this.serahTerimaGesekanInput.noSurat = this.noSurat;
        this.serahTerimaGesekanInput.tanggal = this.tanggal;
        Alertify.confirm(
            "Konfirmasi",
            "Apakah anda yakin data ingin digenerate?",
            () => {
                this.btnDisabled = true;
                this.SerahTerimaGesekanService.generate(this.serahTerimaGesekanInput).then(response => {
                    Alertify.success("Data berhasil digenerate");
                    window.open(window.location.origin + '/api/v1/SerahTerimaGesekanApi/Download/' + response.data);
                    this.resetAll(form);                   
                }).catch(response => {
                    this.btnDisabled = false;
                    if (response.status == "400") {
                        if (response.data == "Data tidak valid") {
                            Alertify.error(response.data);
                        } else {
                            this.errorMessage = response.data;
                        }
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
}
export class SerahTerimaGesekan implements angular.IComponentOptions {
    controller = SerahTerimaGesekanController;
    controllerAs = 'me';

    template = require('./SerahTerimaGesekan.html');
}