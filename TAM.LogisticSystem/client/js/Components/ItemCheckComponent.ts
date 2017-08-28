import * as Angular from "angular";
import * as Service from "../services";

export class ItemCheckController implements angular.IController {
    static $inject = ["InspectionItemService"];

    ShowEdit: boolean;

    Data: string;
    DataItem: string;

    InspectionPart: Service.ItemCheckService;

    InspectionPartId: number;
    partName: string;

    InspectionItem: [{
        inspectionItemId: number,
        itemName: String
    }];

    InspectionItemId: number;
    itemName: string;

    viewby = 1;
    itemsCount = 2;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    // pageSize = 5; 
    pageSizes = [5];
    pageSize = this.pageSizes[0];
    totalItems: number;
    pageNumber: number;

    constructor(inspectionPart: Service.ItemCheckService)
    {
        this.InspectionPart = inspectionPart;
        this.InspectionItem = [{
            inspectionItemId: 0,
            itemName: "Choose one",
        }];
        this.ShowEdit = true;
    }

    refreshData() {

        this.InspectionPart.GetData().then(response => {
            this.Data = response.data;
            this.itemsPerPage = 2;
            this.itemsCount = this.totalItems;


            this.totalItems = Math.ceil(response.data.length / this.pageSize);

            this.currentPage = 1;

            this.partName = "";
            this.InspectionItem["itemName"] = "Choose one";
            this.InspectionItem["inspectionItemId"] = 0;
        });

        this.InspectionPart.GetDropdownItem().then(response => {
            this.DataItem = response.data;
        });
        this.ShowEdit = true;
    }

    $onInit() {
        this.refreshData();

    }

    checkInput() {
        console.log("Success", this.partName, this.InspectionItemId);
    }

    checkUpdate(data) {
        console.log("Success", this.partName, this.InspectionItemId);
    }

    checkDelete(data) {
        console.log("Success", this.partName, this.InspectionItemId);
    }


    createData () {
        console.log(this.partName, this.InspectionItemId);
        this.InspectionPart.PostData(this.partName, this.InspectionItemId,1).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    selectData(data) {
        console.log("Success", data);
        this.InspectionPartId = data.inspectionPartId;
        this.partName = data.partName;
        this.InspectionItemId = data.inspectionItemId;
        this.itemName = data.itemName;
        this.InspectionItem["itemName"] = data.itemName;
        this.InspectionItem["inspectionItemId"] = data.inspectionItemId;
        console.log(this.InspectionItem);
        this.ShowEdit = false;
    }

    SelectItemOnChange() {
        console.log(this.InspectionItem);
        this.itemName = this.InspectionItem["name"];
        this.InspectionItemId = this.InspectionItem["inspectionItemId"];
        console.log(this.itemName, this.InspectionItemId);
    }

    updateData() {
        console.log(this.InspectionPartId, this.partName, this.InspectionItemId);
        this.InspectionPart.UpdateData(this.InspectionPartId, this.partName, this.InspectionItemId, 1).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    deleteData() {
        console.log(this.InspectionPartId);
        this.InspectionPart.DeleteData(this.InspectionPartId).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }
}

export class ItemCheckComponent implements angular.IComponentOptions {
    controller = ItemCheckController;
    controllerAs = "me";

    template = require("./ItemCheck.html");
    bindings = {
        greet: "@",
        // ftoken: "@"
    };
}