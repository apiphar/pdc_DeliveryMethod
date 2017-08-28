
import * as Service from '../services';

export class masterRangkaianRuteController implements angular.IController {
    static $inject = ['masterRangkaianRuteService'];

    ShowEdit: boolean;


    MasterRangkaianRute: Service.masterRangkaianRuteService;

    //header
    DataHeader: string;
    DataRoutingDictionary: string;
    DataVehicle: string;
    DataBranch: string;
    DataDealer: string;


    //detail
    DataDetail: string;
    DataRoutingDictionaryDetail: string;
    DataLocation: string;
    DataDeliveryMethod: string;
    DataRoutingMaster: string;


    RoutingDictionaryId: number;
    RoutingDictionaryId1: number;

    RoutingDictionaryIdForDetail: number;

    Katashiki: string;
    Suffix: string;
    CarModelCode: string;
    CarModelName: string;

    Vehicle: [{
        katashiki: string;
        suffix: string;
        carModelCode: string;
        carModelName: string;
    }]

    BranchCode: string;
    BranchName: string;

    Branch: [{
        branchCode: string;
        branchName: string;
    }]

    DealerCode: string;
    DealerName: string;

    Dealer: [{
        dealerCode: string;
        dealerName: string;
    }]

     
    //detail

    RoutingDictionaryDetailId: number;
    Ordering: number;

    LocationCode: string;
    LocationName: string;

    Location: [{
        locationCode: string;
        locationName: string;
    }]

    DeliveryMethodCode: string;
    DeliveryMethodName: string;

    DeliveryMethod: [{
        deliveryMethodCode: string;
        deliveryMethodName: string;
    }]

    RoutingMasterCode: string;
    RoutingMasterName: string;

    RoutingMaster: [{
        routingMasterCode: string;
        routingMasterName: string;
    }]
    
    viewbyHeader = 1;
    itemsCountHeader = 2;
    currentPageHeader = 0;
    itemsPerPageHeader = this.viewbyHeader;
    maxSizeHeader = 2;
    pageSizesHeader = [5];
    pageSizeHeader = this.pageSizesHeader[0]
    totalItemsHeader: number;
    pageNumberHeader: number;

    
    viewbyDetail = 1;
    itemsCountDetail = 2;
    currentPageDetail = 0;
    itemsPerPageDetail = this.viewbyDetail;
    maxSizeDetail = 2;
    pageSizesDetail = [5];
    pageSizeDetail = this.pageSizesDetail[0]
    totalItemsDetail: number;
    pageNumberDetail: number;


    constructor(masterRangkaianRute: Service.masterRangkaianRuteService) {
        this.MasterRangkaianRute = masterRangkaianRute;
        console.log(this.MasterRangkaianRute)

        this.Vehicle = [{
            katashiki: 'Choose One',
            suffix: 'Choose One',
            carModelCode: "",
            carModelName: 'Choose One'
        }]

        this.Branch = [{
            branchCode: "",
            branchName: 'Choose One'
        }]

        this.Dealer = [{
            dealerCode: "",
            dealerName: 'Choose One'
        }]

        this.Location = [{
            locationCode: "",
            locationName: 'Choose One'
        }]

        this.DeliveryMethod = [{
            deliveryMethodCode: "",
            deliveryMethodName: 'Choose One'
        }]

        this.RoutingMaster = [{
            routingMasterCode: "",
            routingMasterName: 'Choose One'
        }]

        this.ShowEdit = true;
    }

    refreshDataHeader() {
        this.MasterRangkaianRute.GetRoutingDictionary().then(response => {
            this.DataHeader = response.data;

            this.itemsPerPageHeader = 5;
            this.itemsCountHeader = this.totalItemsHeader;
            this.totalItemsHeader = Math.ceil(response.data.length / this.pageSizeHeader);
            this.currentPageHeader = 1;

            this.RoutingDictionaryId =0;
            this.RoutingDictionaryId1 = response.data.length + 1;
            this.Vehicle["katashiki"] = "";
            this.Vehicle["suffix"] = "";
            this.Vehicle["carModelCode"] = "";
            this.Vehicle["carModelName"] = "";
            this.Branch["branchCode"] = "";
            this.Branch["branchName"] = "";
            this.Dealer["dealerCode"] = "";
            this.Dealer["dealerName"] = "";

            this.Katashiki = "";
            this.Suffix = "";
            this.CarModelCode = "";
            this.CarModelName = "";
            this.BranchCode = "";
            this.BranchName = "";
            this.DealerCode = "";
            this.DealerName = "";
        });

        this.MasterRangkaianRute.GetVehicle().then(respose => {
            this.DataVehicle = respose.data;
        });
        this.MasterRangkaianRute.GetBranch().then(respose => {
            this.DataBranch = respose.data;
        });
        this.MasterRangkaianRute.GetDealer().then(respose => {
            this.DataDealer = respose.data;
        });
        //this.ShowEdit = true;

        
    }

    refreshDataDetail(RoutingDictionaryId) {
        if (RoutingDictionaryId == undefined) {
            RoutingDictionaryId = this.RoutingDictionaryIdForDetail;
        }
        else
        {
            this.RoutingDictionaryIdForDetail = RoutingDictionaryId;
        }

        this.MasterRangkaianRute.GetRoutingDictionaryDetail(RoutingDictionaryId).then(response => {
            this.DataDetail = response.data;
            this.itemsPerPageDetail = 5;
            this.itemsCountDetail = this.totalItemsDetail;
            this.totalItemsDetail = Math.ceil(response.data.length / this.pageSizeDetail);
            this.currentPageDetail = 1;

            this.RoutingDictionaryDetailId = 0;
            this.Location["locationCode"] = "";
            this.Location["locationName"] = "";
            this.DeliveryMethod["deliveryMethodCode"] = "";
            this.DeliveryMethod["deliveryMethodName"] = "";
            this.RoutingMaster["routingMasterCode"] = "";
            this.RoutingMaster["routingMasterName"] = "";

            this.Ordering = undefined;
            this.LocationCode = "";
            this.LocationName = "";
            this.DeliveryMethodCode = "";
            this.DeliveryMethodName = "";
            this.RoutingMasterCode = "";
            this.RoutingMasterName = "";
        });

        this.MasterRangkaianRute.GetLocation().then(respose => {
            this.DataLocation = respose.data;
        });
        this.MasterRangkaianRute.GetDeliveryMethod().then(respose => {
            this.DataDeliveryMethod = respose.data;
        });
        this.MasterRangkaianRute.GetRoutingMaster().then(respose => {
            this.DataRoutingMaster = respose.data;
        });
        this.ShowEdit = true;

    }

    $onInit() {
        this.refreshDataHeader();
    }

    checkInputHeader() {
        this.Katashiki = this.Vehicle["katashiki"];
        this.Suffix = this.Vehicle["suffix"];
        this.CarModelCode = this.Vehicle["carModelCode"];
        this.CarModelName = this.Vehicle["carModelName"];
        this.BranchCode = this.Branch["branchCode"];
        this.BranchName = this.Branch["branchName"];
        this.DealerCode = this.Dealer["dealerCode"];
        this.DealerName = this.Dealer["dealerName"];

    }

    checkUpdateHeader(data) {
        this.Katashiki = this.Vehicle["katashiki"];
        this.Suffix = this.Vehicle["suffix"];
        this.CarModelCode = this.Vehicle["carModelCode"];
        this.CarModelName = this.Vehicle["carModelName"];
        this.BranchCode = this.Branch["branchCode"];
        this.BranchName = this.Branch["branchName"];
        this.DealerCode = this.Dealer["dealerCode"];
        this.DealerName = this.Dealer["dealerName"];

    }

    checkDeleteHeader(data) {
            }

    checkInputDetail() {
        this.RoutingDictionaryId = this.RoutingDictionaryId;
        this.LocationCode = this.Location["locationCode"];
        this.LocationName = this.Location["locationName"];
        this.DeliveryMethodCode = this.DeliveryMethod["deliveryMethodCode"];
        this.DeliveryMethodName = this.DeliveryMethod["deliveryMethodName"];
        this.RoutingMasterCode = this.RoutingMaster["routingMasterCode"];
        this.RoutingMasterName = this.RoutingMaster["routingMasterName"];
       
    }

    checkUpdateDetail(data) {
        this.RoutingDictionaryId = this.RoutingDictionaryId;
        this.LocationCode = this.Location["locationCode"];
        this.LocationName = this.Location["locationName"];
        this.DeliveryMethodCode = this.DeliveryMethod["deliveryMethodCode"];
        this.DeliveryMethodName = this.DeliveryMethod["deliveryMethodName"];
        this.RoutingMasterCode = this.RoutingMaster["routingMasterCode"];
        this.RoutingMasterName = this.RoutingMaster["routingMasterName"];
      
    }

    checkDeleteDetail(data) {
       
    }

    createDataHeader() {

        this.MasterRangkaianRute.PostDataHeader(this.BranchCode, this.Katashiki, this.Suffix).then(response => {
            this.refreshDataHeader();
        });
    }

    createDataDetail() {

        this.MasterRangkaianRute.PostDataDetail(this.RoutingDictionaryId, this.LocationCode, this.Ordering, this.DeliveryMethodCode, this.RoutingMasterCode).then(response => {
            this.refreshDataDetail(this.RoutingDictionaryId);
        });
    }

    selectDataHeader(data) {
        //this.PIOLineMasterData = data.pioLineMaster;
        this.RoutingDictionaryId = data.routingDictionaryId;

        this.Katashiki =  data.katashiki;
        this.Suffix = data.suffix;
        this.CarModelCode = data.carModelCode;
        this.CarModelName = data.carModelName;
        this.Vehicle["katashiki"] = data.katashiki;
        this.Vehicle["suffix"] = data.suffix;
        this.Vehicle["carModelCode"] = data.carModelCode;
        this.Vehicle["carModelName"] = data.carModelName;
        //this.Katashiki = this.Vehicle["katashiki"] = data.katashiki;
        //this.Suffix = this.Vehicle["suffix"] = data.suffix;
        //this.CarModelCode = this.Vehicle["carModelCode"] = data.carModelCode;
        //this.CarModelName = this.Vehicle["carModelName"] = data.carModelName;

        this.BranchCode = data.branchCode;
        this.BranchName = data.branchName;
        this.Branch["branchCode"] = data.branchCode;
        this.Branch["branchName"] = data.branchName;
        //this.BranchCode = this.Branch["branchCode"] = data.branchCode;
        //this.BranchName = this.Branch["branchName"] = data.branchName;

        this.DealerCode = data.dealerCode;
        this.DealerName = data.dealerName;
        this.Dealer["dealerCode"] = data.dealerCode;
        this.Dealer["dealerName"] = data.dealerName;
        //this.DealerCode = this.Dealer["dealerCode"] = data.dealerCode;
        //this.DealerName = this.Dealer["dealerName"] = data.dealerName;

        this.ShowEdit = false;
    }

    selectDataDetail(data) {
        //this.PIOLineMasterData = data.pioLineMaster;
        this.RoutingDictionaryDetailId = data.routingDictionaryDetailId
        this.RoutingDictionaryId = data.routingDictionaryId;
        this.BranchName = data.branchName;
        this.DealerName = data.dealerName;
        this.Ordering = data.ordering;

        this.LocationCode = data.locationCode;
        this.LocationName = data.locationName;
        this.Location["locationCode"] = data.locationCode;
        this.Location["locationName"] = data.locationName;
        //this.LocationCode = this.Location["locationCode"] = data.locationCode;
        //this.LocationName = this.Location["locationName"] = data.locationName;

        this.DeliveryMethodCode = data.deliveryMethodCode;
        this.DeliveryMethodName = data.deliveryMethodName;
        this.DeliveryMethod["deliveryMethodCode"] = data.deliveryMethodCode;
        this.DeliveryMethod["deliveryMethodName"] = data.deliveryMethodName;
        //this.DeliveryMethodCode = this.DeliveryMethod["deliveryMethodCode"] = data.deliveryMethodCode;
        //this.DeliveryMethodName = this.DeliveryMethod["deliveryMethodName"] = data.deliveryMethodName;

        this.RoutingMasterCode = data.routingMasterCode;
        this.RoutingMasterName = data.routingMasterName;
        this.RoutingMaster["routingMasterCode"] = data.routingMasterCode;
        this.RoutingMaster["routingMasterName"] = data.routingMasterName;
        //this.RoutingMasterCode = this.RoutingMaster["routingMasterCode"] = data.routingMasterCode;
        //this.RoutingMasterName = this.RoutingMaster["routingMasterName"] = data.routingMasterName;

        this.ShowEdit = false;
    }

    SelectItemOnChangeVehicle() {
        this.Katashiki = this.Vehicle["katashiki"];
        this.Suffix = this.Vehicle["suffix"];
        this.CarModelCode = this.Vehicle["carModelCode"];
        this.CarModelName = this.Vehicle["carModelName"];
    }

    SelectItemOnChangeBranch() {
        this.BranchCode = this.Branch["branchCode"];
        this.BranchName = this.Branch["name"];
    }

    SelectItemOnChangeDealer() {
        this.DealerCode = this.Dealer["dealerCode"];
        this.DealerName = this.Dealer["name"];
    }

    SelectItemOnChangeLocation() {
        this.LocationCode = this.Location["locationCode"];
        this.LocationName = this.Location["name"];
    }

    SelectItemOnChangeDeliveryMethod() {
        this.DeliveryMethodCode = this.DeliveryMethod["deliveryMethodCode"];
        this.DeliveryMethodName = this.DeliveryMethod["name"];
    }

    SelectItemOnChangeRoutingMaster() {
        this.RoutingMasterCode = this.RoutingMaster["routingMasterCode"];
        this.RoutingMasterName = this.RoutingMaster["name"];
    }

    updateDataHeader() {
       
        this.MasterRangkaianRute.UpdateDataHeader(this.RoutingDictionaryId, this.BranchCode, this.Katashiki, this.Suffix).then(response => {
            this.refreshDataHeader();
        });
    }

    updateDataDetail() {
        
        this.MasterRangkaianRute.UpdateDataDetail(this.RoutingDictionaryDetailId, this.LocationCode, this.Ordering, this.DeliveryMethodCode, this.RoutingMasterCode).then(response => {
            this.refreshDataDetail(this.RoutingDictionaryId);
        });
    }

    deleteDataHeader() {
        this.MasterRangkaianRute.DeleteDataHeader(this.RoutingDictionaryId).then(response => {
            this.refreshDataHeader();
        });
    } 

    deleteDataDetail() {
        this.MasterRangkaianRute.DeleteDataDetail(this.RoutingDictionaryDetailId).then(response => {
            this.refreshDataDetail(this.RoutingDictionaryId);
        });
    }        
}

export class masterRangkaianRuteComponent implements angular.IComponentOptions {
    controller = masterRangkaianRuteController;
    controllerAs = 'me';

    template = require('./masterrangkaianrute.html');
    bindings = {
        greet: '@',
        //ftoken: '@'
    };
}