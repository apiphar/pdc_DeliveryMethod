
import * as Service from '../services';
import * as Lodash from 'lodash';

export class MCCPController implements angular.IController {
    static $inject = ['MCCPService'];

    mccpService: Service.MCCPService;

    data: any;
    locationData: any;
    branchData: any;
    dealerData: any;

    isUpdate = 0;

    locationFrom: [{
        locationCode: any,
        name: any
    }];
    locationTo: [{
        locationCode: any,
        name: any
    }];
    dealer: [{
        dealerCode: any,
        name: any
    }];
    branch: [{
        branchCode: any,
        name: any
    }];

    locationFromId: any;
    locationToId: any;
    dealerId: any;
    branchId: any;

    baris: any;
    keterangan: any;
    sql1: any;
    cond: any;
    sql2: any;
    sambung: any;
    sql: any;

    constructor(MCCPService: Service.MCCPService) {
        this.mccpService = MCCPService;
        this.locationData= [{
            locationId: 0,
            name: ""
        }];
        this.branchData= [{
            branchId: 0,
            name: ""
        }];
        this.dealerData= [{
            dealerId: 0,
            name: ""
        }];

        this.locationFrom = [{
            locationCode: 0,
            name: ""
        }];
        this.locationTo = [{
            locationCode: 0,
            name: ""
        }];
        this.dealer = [{
            dealerCode: 0,
            name: ""
        }];
        this.branch = [{
            branchCode: 0,
            name: ""
        }];
        this.reset();
    }
    reset() {
        this.cond = "";
        this.sambung = "";
        this.isUpdate = 0;
        this.baris=null;
        this.keterangan = null;
        this.sql1 = "";
        this.sql2 = "";
        this.sql = "";

        this.locationFrom= [{
            locationCode:0,
            name:""
        }];
        this.locationTo= [{
            locationCode:0,
            name: ""
        }];
        this.dealer= [{
            dealerCode: 0,
            name: ""
        }];
        this.branch= [{
            branchCode: 0,
            name: ""
        }];
    }
    $onInit() {
        this.getData();
    }
    addSql() {
        this.sql += this.sql1 + " " + this.cond + " " + this.sql2 + " " + this.sambung + " ";
    }
    getData() {
        this.mccpService.getData().then(response => {
            this.data = response.data;
        });
        this.mccpService.getLocationData().then(response => {
            this.locationData = response.data;
        });
        this.mccpService.getDealerData().then(response => {
            this.dealerData = response.data;
        });
        this.mccpService.getBranchData().then(response => {
            this.branchData = response.data;
        });
    }
    selectLocationFromOnChange() {
        this.locationFromId = this.locationFrom["locationCode"];        
    }
    selectLocationToOnChange() {
        this.locationToId = this.locationTo["locationCode"];
    }
    selectDealerChange() {
        this.dealerId = this.dealer["dealerCode"];
    }
    selectBranchOnChange() {
        this.branchId = this.branch["branchCode"];
    }
    createData() {
        this.mccpService.create(this.baris, this.locationFromId, this.locationToId, this.keterangan, this.sql, this.dealerId, this.branchId).then(response => {
            alert("sukses");
            this.getData();
        });
    }
    selectEdit(data) {
        this.isUpdate = 1;
        this.baris = data.baris;
        this.keterangan = data.keterangan;
        this.sql = data.sqlField;
        this.locationFrom["locationCode"] = data.lokAsal;
        this.locationFrom["name"] = data.lokAsalName;
        this.locationTo["locationCode"] = data.lokTujuan;
        this.locationTo["name"] = data.lokTujuanName;
        this.dealer["dealerCode"] = data.dealerCode;
        this.dealer["name"] = data.dealerName;
        this.branch["branchCode"] = data.branchCode;
        this.branch["name"] = data.branchName;
    }
    updateData() {
        this.mccpService.update(this.baris, this.locationFromId, this.locationToId, this.keterangan, this.sql, this.dealerId, this.branchId).then(response => {
            alert("sukses");
            this.getData();
        });
    }
    deleteData(data) {
        this.mccpService.delete(data.baris).then(response => {
            alert("sukses");
            this.getData();
        });
    }
}


export class MCCPComponent implements angular.IComponentOptions {
    controller = MCCPController;
    controllerAs = 'me';

    template = require('./MCCP.html');
}