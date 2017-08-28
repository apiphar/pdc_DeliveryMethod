import * as angular from 'angular';
import * as service from '../services/UnitAssignService';
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as moment from 'moment';

class UnitAssignController implements angular.IController {
    static $inject = ["UnitAssignService", "$rootScope"];

    constructor(assign: service.UnitAssignService, root: angular.IRootScopeService) {
        this.assignByUnitService = assign;
        this.Root = root;
        this.loadingTable = false;
    }

    //private readonly style
    assignByUnitService: service.UnitAssignService;
    Root: angular.IRootScopeService

    //Data Types
    voyage: string;
    showAssignByUnit: boolean;
    loadingTable: boolean;
    dataTidakDitemukan: boolean;
    stringError: string;
    stringErrorShow: boolean;
    simpanButton: boolean;
    searchTable = {};

    //Model
    unitAssignDataModel: service.UnitAssignDataModel;

    $onInit() {
        this.showAssignByUnit = true;
        this.Root.$on("ShowAssign", (event, data) => {
            this.showAssignByUnit = data;
        });
        this.dataTidakDitemukan = true;
        this.stringErrorShow = false;
        this.simpanButton = false;
    }

    //Retrieve All The Data
    retrieveData() {
        this.dataTidakDitemukan = false;
        this.loadingTable = true;
        this.assignByUnitService.getAllVoyage(this.voyage).then(response => {
            this.unitAssignDataModel = response.data as service.UnitAssignDataModel;

            lodash.forEach(this.unitAssignDataModel.allUnit, Q => {
                Q.customerAssignModel = (Q.customerAssign) ? "Ya" : "Tidak";
            });

            this.totalItems = this.unitAssignDataModel.allUnit.length;
            this.Root.$broadcast("Delivery_AssignByUnit-Voyage", this.voyage);
            this.dataTidakDitemukan = false;
            this.stringErrorShow = false;
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            } 
            this.stringError = "Voyage No tidak ditemukan";
            this.stringErrorShow = true;
            this.unitAssignDataModel = null;
            this.dataTidakDitemukan = true;
            this.totalItems = 0;
            this.simpanButton = true;
            }).finally(() => {
                this.loadingTable = false;
            });;
    }
    //Send Trigger to show the component of detail
    showDetail() {
        this.Root.$broadcast("Delivery_AssignByUnit-ShowDetail", true);
        this.Root.$broadcast("Delivery_AssignByUnit-Voyage", this.voyage);
        this.showAssignByUnit = false;
        this.searchTable = {};
    }

    //filter the voyage detail based on Voyage
    getDetailVoyage() {
        //VALIDATION
        if (this.voyage == "" || this.voyage == null) {
            this.unitAssignDataModel = null;
            this.stringErrorShow = false;
            this.totalItems = 0;
            this.dataTidakDitemukan = true;
        }
        if (/^[a-zA-Z0-9]+$/.test(this.voyage) == false) {
            this.unitAssignDataModel = null;
            this.totalItems = 0;
            this.dataTidakDitemukan = false;
        }
        else if (this.voyage != null && this.voyage != "")
            this.retrieveData();
    }

    //insert the data to the server
    saveData(form: angular.IFormController) {
        alertify.confirm('Apakah anda ingin menambah data?',
            Q => {
                //this.unitAssignDataModel.allUnit = null;
                //this.loadingTable = true;
                this.assignByUnitService.saveData(this.unitAssignDataModel).then(response => {
                    alertify.success("Data berhasil disimpan");
                    this.unitAssignDataModel = response.data as service.UnitAssignDataModel;
                    this.totalItems = this.unitAssignDataModel.allUnit.length;
                    this.Root.$broadcast("Delivery_AssignByUnit-Voyage", this.voyage);                  
                    //this.loadingTable = false;
                    form.$setPristine();
                    form.$setUntouched();
                    this.searchTable = {};
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else if(response.status == "400") {
                        alertify.error(response.data);
                    }
                    this.retrieveData();
                });
            },
            Q => { }).set('labels', { ok: 'Ya', cancel: 'Tidak' }).setHeader('Konfirmasi');
    }

    //reset the form and clear the model
    cancel(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.unitAssignDataModel = null;
        this.voyage = null;
        this.totalItems = 0;
        this.dataTidakDitemukan = true;
        this.stringErrorShow = false;
        this.searchTable = {};
    }

    //Paging Section
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
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
}

export class UnitAssign implements angular.IComponentOptions {
    controller = UnitAssignController;
    controllerAs = 'me';
    template = require("./UnitAssign.html");
}
