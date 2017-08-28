import * as Services from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as moment from 'moment';


export class RoutingProductionLeadTimeController implements angular.IController {
    static $inject = ['RoutingProductionLeadTimeService'];



    MyForm: angular.IFormController;

    Data: string;
    ActiveUser: string;
    ActiveLocation: string;
    ActiveRoutingMaster: string;

    created: boolean;
    edited: boolean;

    orderState: boolean = false;
    orderString: string;

    Ordering: number;
  
    RoutingProductionLeadTime: Services.RoutingProductionLeadTimeService;

    RoutingDictionaryProductionId: number;

    RoutingMasterCode: string;
    SelectRoutingMaster: [{
        routingMasterCode: string;
        namaRute: string;
    }]
    RoutingMasterValue: string;


    LocationData: JSON;
    SelectedLocation: [{
        locationCode: string;
        namaLocation: string;
    }];
    LocationCodeValue: string;



    LeadMinutes: number;

    NamaRuteData: JSON;
    NamaRute: string;


    KatashikiData: string;
    Katashiki: [{
        katashiki: string;
    }];
    KatashikiValue: string;

    SuffixData: string;
    Suffix: [{
        suffix: string;
    }];
    SuffixValue: string;


    CarModelData: JSON;
    CarModel: [{
        carModelCode: number;
        namaType: string;
    }];
    CarModelValue: string;
    CarModelCode: number;

    SearchKatashiki: string;

    viewby = 1;
    itemsCount = 2;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 2
    pageSizes = [5, 10, 15, 20, 25];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;

    constructor(routingProductionLeadTime: Services.RoutingProductionLeadTimeService) {
        this.RoutingProductionLeadTime = routingProductionLeadTime;

        this.SelectedLocation = [{
            locationCode: "locationCode",
            namaLocation: "namaLocation"
        }];

        this.Katashiki = [{
            katashiki: 'katashiki'
        }];
        this.Suffix = [{
            suffix: 'suffix'
        }];
        this.CarModel = [{
            carModelCode: 0,
            namaType: 'namaType'
        }];
        this.SelectRoutingMaster = [{
            routingMasterCode: "routingMasterCode",
            namaRute: 'namaRute'
        }];




    }

    clearForm() {

        this.SelectRoutingMaster["routingMasterCode"] = "";
        this.NamaRute = null;

        this.Katashiki["katashiki"] = "";
        this.Suffix["suffix"] = "";
        this.CarModelData = null;
        this.CarModelCode = null;
        this.CarModelValue = null;
        this.Ordering = null;
        this.SelectedLocation["locationCode"] = "";
        this.SelectedLocation["namaLocation"] = "";
        this.LeadMinutes = null;

        this.edited = false;
        this.created = true;

    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    refreshData() {
        this.RoutingProductionLeadTime.GetData().then(response => {
            this.Data = response.data;

            this.itemsPerPage = 20;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;

            this.SelectRoutingMaster["routingMasterCode"] = null;
            this.NamaRute = null;

            this.Katashiki["katashiki"] = null;
            this.Suffix["suffix"] = null;
            this.CarModelData = null;
            this.CarModelCode = null;
            this.CarModelValue = null;
            this.Ordering = null;
            this.SelectedLocation["locationCode"] = null;
            this.SelectedLocation["namaLocation"] = null;
            this.LeadMinutes = null;

        });

        this.RoutingProductionLeadTime.GetRoutingMasterCode().then(response => {
            this.ActiveRoutingMaster = response.data;
        });

        this.RoutingProductionLeadTime.GetDropDownLocationCode().then(response => {
            this.ActiveLocation = response.data;
        });

        this.RoutingProductionLeadTime.GetKatashiki().then(response => {
            this.KatashikiData = response.data;
        });



    }

    cascadeForSuffix(data) {

        this.RoutingProductionLeadTime.GetSuffix(data.katashiki).then(response => {
            this.SuffixData = response.data;
        });
    }

    cascadeForModel(data) {

        this.RoutingProductionLeadTime.GetCarModel(data.katashiki, data.suffix).then(response => {
            this.CarModelData = response.data;
        });

    }

    cascadeForRute(data) {

        console.log(data);
        this.RoutingProductionLeadTime.GetKodeRute(data.routingMasterCode).then(response => {
            this.NamaRute = response.data['namaRute'];

        });


    }


    $onInit() {
        this.refreshData();
        this.edited = false;
        this.created = true;
    }


    cascade() {
        if (this.SearchKatashiki.length > 3) {

            this.RoutingProductionLeadTime.GetKatashiki().then(response => {
                this.Data = response.data;
            });

        }
    }

    createData() {
        this.LocationCodeValue = this.SelectedLocation["locationCode"];
        this.RoutingMasterValue = this.SelectRoutingMaster["routingMasterCode"];
        this.RoutingProductionLeadTime.PostData(this.LocationCodeValue, this.Katashiki["katashiki"], this.Suffix["suffix"], this.RoutingMasterValue, this.Ordering, this.LeadMinutes).then(response => {
            this.refreshData();
            this.clearForm();
        });
    }

    getAllSelectedData(data) {
        this.RoutingDictionaryProductionId = data.routingDictionaryProductionId;
        this.KatashikiValue = data.katashiki;
        this.Katashiki["katashiki"] = data.katashiki;
        this.cascadeForSuffix(data);
        this.SuffixValue = data.suffix;
        this.Suffix["suffix"] = data.suffix;
        this.cascadeForModel(data);
        this.CarModelValue = data.carModel;
        this.CarModel["namaType"] = data.namaType;
        this.CarModel["carModelCode"] = data.carModelCode;
        this.NamaRute = data.namaRute;
        this.LocationCodeValue = data.locationCode;
        this.SelectedLocation["locationCode"] = data.locationCode;
        this.SelectedLocation["namaLocation"] = data.namaLocation;
        this.RoutingMasterValue = data.routingMasterCode;
        this.SelectRoutingMaster["routingMasterCode"] = data.routingMasterCode;
        this.LeadMinutes = data.leadMinutes;
        this.Ordering = data.ordering;
    }

    selectEdit(data) {
        this.getAllSelectedData(data);
        this.edited = true;
        this.created = false;
    }

    selectDelete(data) {
        this.getAllSelectedData(data);
        this.edited = true;
        this.created = false;

    }

    selectOnChageLocation() {
        this.LocationCodeValue = this.SelectedLocation["locationCode"];
    }

    selectOnChageRoutingMaster() {
        this.RoutingMasterValue = this.SelectRoutingMaster["routingMasterCode"];
    }


    updateData() {
        this.RoutingProductionLeadTime.UpdateData(this.RoutingDictionaryProductionId, this.LocationCodeValue, this.Katashiki["katashiki"], this.Suffix["suffix"], this.RoutingMasterValue, this.Ordering, this.LeadMinutes).then(response => {
            this.refreshData();
            this.clearForm();
        });
    }


    deleteData(data) {
        this.getAllSelectedData(data);
        this.RoutingProductionLeadTime.DeleteData(this.RoutingDictionaryProductionId).then(response => {
            this.refreshData();
            this.clearForm();
        });
    }

    alertifyData() {
        var data = {
            namaLocation: this.SelectedLocation["namaLocation"],
            ordering: this.Ordering,
            katashiki: this.Katashiki["katashiki"],
            suffix: this.Suffix["suffix"],
            namaRute: this.NamaRute,
            routingMasterCode: this.SelectRoutingMaster["routingMasterCode"],
            leadMinutes: this.LeadMinutes

        };
        return data;
    }

    confirmationDialog(Message: string, Params: string, MyForm: angular.IFormController) {
        let self = this;
        if (!self.SelectedLocation || !self.Ordering || !self.Katashiki["katashiki"] || !self.Suffix["suffix"] || !self.NamaRute || !self.SelectRoutingMaster) {
            alertify.error('Data Harus Di Isi');
        }
        else {
            alertify.confirm(Message,
                mustache.render(require("./alertify/RoutingProductionLeadTimeAlertify.html"), this.alertifyData()),
                function () {

                    if (Params == 'Create') {
                        self.createData();
                        alertify.success('Data Succesfully Saved');
                    }
                    else if (Params == 'Update') {
                        self.updateData();
                        alertify.success('Data Succesfully Updated');
                    }

                },
                function () {
                    alertify.error('Cancel');
                }
            );
        }
    }

    alertifyDelete(dt) {
        var data = {
            ordering: dt.ordering,
            katashiki: dt.katashiki,
            suffix: dt.suffix,
            namaRute: dt.namaRute,
            namaLocation: dt.namaLocation,
            routingMasterCode: dt.routingMasterCode,
            leadMinutes: dt.leadMinutes
        };
        return data;
    }

    confirmationDelete(type: string, data) {
        var self = this;

        alertify.confirm(
            'Are you sure want to ' + type + ' this details ?',
            mustache.render(require('./alertify/RoutingProductionLeadTimeAlertify.html'), self.alertifyDelete(data)), function () {
                if (type === 'Delete') {
                    self.deleteData(data);
                    alertify.success('Ok');
                }
            },
            function () {
                alertify.error('Cancel')
            }
        );

    }
}



export class RoutingProductionLeadTimeComponent implements angular.IComponentOptions {
    controller = RoutingProductionLeadTimeController;
    controllerAs = 'me';

    template = require('./RoutingProductionLeadTime.html');
    bindings = {
        greet: '@'
    };
}