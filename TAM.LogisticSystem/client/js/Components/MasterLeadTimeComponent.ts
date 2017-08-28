
import * as Services from '../Services';

export class MasterLeadTimeController implements angular.IController {
    static $inject = ['MasterLeadTimeService'];

    ShowEdit: boolean;

    Data: string;
    ActiveUser: string;

    RoutingMasterValue: string;

    MasterLeadTime: Services.MasterLeadTimeService;
    checkDuplicate: boolean;
   
    RoutingMasterCode: string;
    ModelName: string;
    LeadMinutes: number;

    NamaRuteData: JSON;
    NamaRouting: [{
       namaRute: string;
    }]

    LocationData: string;
    SelectedLocation: [{
        locationCode: string;
    }];
    LocationCodeValue: string;
   
    LocationCode: string; 


    SearchLocation: string;

    viewby = 1;
    itemsCount = 2;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 2;

    pageSizes = [5];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;

    constructor(masterLeadTime: Services.MasterLeadTimeService) {
        this.MasterLeadTime = masterLeadTime;
        this.checkDuplicate = false;
        this.SelectedLocation = [{
            locationCode: "locationCode"
        }];
        this.ShowEdit = true;
    }

    refreshData() {
        this.checkDuplicate = false;
        this.MasterLeadTime.GetData().then(response => {
            this.Data = response.data;

            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;

            this.RoutingMasterCode = null;
            this.ModelName = null;

            this.SelectedLocation["locationCode"] = null;
            this.LeadMinutes = null;

        });

  
        this.MasterLeadTime.GetDropDownLocation().then(response => {
            this.ActiveUser = response.data;
        });



        this.ShowEdit = true;
    }



    $onInit() {
        this.refreshData();

    }

   
    createData(form) {
        this.LocationCode = this.SelectedLocation["locationCode"];
        this.MasterLeadTime.PostData(this.LocationCode, this.RoutingMasterCode, this.LeadMinutes).then(response => {
            this.refreshData();
        });
        form.$setPristine();
    }

    selectItemOnChangeLocation() {
        //this.LocationCodeValue = this.LocationCodeValue["locationCode"];
        this.checkLocationCodeAndRoutingCode(this.LocationCodeTemp, this.RoutingMasterValue);
    }


    cascadeForRute(routingMasterCode) {
        this.RoutingMasterValue = routingMasterCode;
        this.checkLocationCodeAndRoutingCode(this.SelectedLocation['locationCode'], this.RoutingMasterValue);
        this.MasterLeadTime.GetKodeRute(this.RoutingMasterValue).then(response => {
            this.NamaRuteData = response.data;
            this.ModelName = response.data['namaRute'];
        });
    }

    checkLocationCodeAndRoutingCode(LocationCodeValue: string, RoutingMasterValue: string) {
        this.checkDuplicate = false;
        if (LocationCodeValue && RoutingMasterValue) {
            this.MasterLeadTime.checkLocationCodeAndRoutingCode(LocationCodeValue, RoutingMasterValue).then(response => {
                if (response.data[0]) {
                    this.checkDuplicate = true;
                }
            });
        }
    }

   

    selectEdit(data) {
        this.ModelName = data.namaRute;
    
        this.LocationCodeValue = data.locationCode;
        this.SelectedLocation["locationCode"] = data.locationCode;
        this.RoutingMasterCode = data.routingMasterCode;
        this.LeadMinutes = data.leadMinutes;
        this.ShowEdit = false;
     
    }

    updateData(form) {
        this.LocationCode = this.SelectedLocation["locationCode"];

        this.MasterLeadTime.UpdateData(this.LocationCode, this.RoutingMasterCode, this.LeadMinutes).then(response => {
     
            this.refreshData();
        });
        form.$setPristine();
    }

    deleteData() {
        //this.LocationCode = this.SelectedLocation["locationCode"];
        console.log(this.LocationCodeTemp);
        this.MasterLeadTime.DeleteData(this.LocationCodeTemp).then(response => {
            this.refreshData();
        });
    }

    LocationCodeTemp: string;

    selectDelete(data) {
        this.LocationCodeTemp = data;
        console.log(this.LocationCodeTemp);
    }
}




export class MasterLeadTimeComponent implements angular.IComponentOptions {
    controller = MasterLeadTimeController;
    controllerAs = 'me';

    template = require('./MasterLeadTime.html');
    bindings = {
        greet: '@'
    };
}