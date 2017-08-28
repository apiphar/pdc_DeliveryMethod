
import * as Service from '../Services';

export class InspectionPartController implements angular.IController {
    static $inject = ['InspectionPartService', '$timeout'];

    InspectionPart: Service.InspectionPartService;

    Data: string;
	DataSide: string;
	DataCategory: string;

    InspectionSide: [{
        inspectionSideId: number,
        name: string
	}];

	InspectionCategory: [{
		inspectionCategoryId: number,
		name: string
	}];

    inspectionPartId: number;
    Name: string;

    InspectionSideId: number;
    SideName: string;

	InspectionCategoryId: number;
    Category: string;

	SelectedSide: string;
	SelectedCategory: string;

	ShowHideButton: boolean;

	createdAt: Date;
	createdBy: string;
	updateAt: Date;
	updateBy: string;

    viewby = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    pageSizes = [5, 10, 15, 20];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;

    Success: boolean;
    Delete: boolean;
    MessageSuccess: string;
    MessageDelete: string;

    constructor(inspectionPart: Service.InspectionPartService, private $timeout) {
        this.InspectionPart = inspectionPart;
        this.Success = false;
        this.Delete = false;
		this.ShowHideButton = false;
		this.InspectionCategory = [{
			inspectionCategoryId: 0,
			name: '(Please Choose One)'
		}];
        this.InspectionSide = [{
            inspectionSideId: 0,
            name: '(Please Choose One)'
        }];
	}

	reset()
	{
        this.ShowHideButton = false;
        this.inspectionPartId = null;
		this.Name = null;

		this.InspectionCategoryId = 0;
		this.InspectionSideId = 0;

        this.InspectionCategory["inspectionCategoryId"] = 0;
		this.InspectionSide["inspectionSideId"] = 0;

        this.InspectionCategory["name"] = "(Please Choose One)";
		this.InspectionSide["name"] = "(Please Choose One)";
    }

    refreshData() {
        this.InspectionPart.GetAll().then(response =>
		{
			this.Data = response.data;

            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;
		});

		this.InspectionPart.GetDropdownCategory().then(response => {
			this.DataCategory = response.data;
		});

        this.InspectionPart.GetDropdownSide().then(response => {
            this.DataSide = response.data;
        });

        this.reset();
    }

    $onInit() {
        this.refreshData();
    }

    createData() {
		this.createdAt = new Date;
		this.createdBy = 'Functional_Test';
		this.updateAt = new Date;
		this.updateBy = 'Functional_Test';
        this.InspectionPart.PostData(this.Name, this.InspectionCategoryId, this.InspectionSideId, this.createdAt, this.createdBy, this.updateAt, this.updateBy).then(response => {
            this.refreshData();
            this.successMesaggeAlert();
        });
    }

	selectEdit(data)
	{
		this.ShowHideButton = true;

		this.inspectionPartId = data.inspectionPartId;
		this.Name = data.name;
		this.Category = data.category;
		this.SideName = data.sideName;

		this.InspectionCategory["inspectionCategoryId"] = data.inspectionCategoryId;
		this.InspectionSide["inspectionSideId"] = data.inspectionSideId;

		this.InspectionCategory["name"] = data.category;
		this.InspectionSide["name"] = data.sideName;

		this.InspectionCategoryId = data.inspectionCategoryId;
		this.InspectionSideId = data.inspectionSideId;

		this.createdAt = data.createdAt;
		this.createdBy = data.createdBy;

        //this.SideName = data.sideName;
        //this.InspectionSide["sideName"] = data.sideName;
	}

	updateData() {
		this.Category = this.InspectionCategory["name"];
		this.SideName = this.InspectionSide["name"];

		this.updateAt = new Date;
		this.updateBy = 'Functional_Test';
		this.InspectionCategoryId = this.InspectionCategory["inspectionCategoryId"];
		this.InspectionSideId = this.InspectionSide["inspectionSideId"];
		
		this.InspectionPart.UpdateData(this.inspectionPartId, this.Name, this.InspectionCategoryId, this.InspectionSideId, this.createdAt, this.createdBy, this.updateAt, this.updateBy).then(response => {
		    this.refreshData();
		    this.successMesaggeAlert();
		});
	}

	SelectCategoryOnChange() {
		this.Category = this.InspectionCategory["name"];
		this.InspectionCategoryId = this.InspectionCategory["inspectionCategoryId"];
	}

	SelectSideOnChange() {
		this.SideName = this.InspectionSide["name"];
		this.InspectionSideId = this.InspectionSide["inspectionSideId"];
	}

    selectDelete(data) {
        this.inspectionPartId = data.inspectionPartId;
        this.Name = data.name;
    }   

    deleteData() {
        this.InspectionPart.DeleteData(this.inspectionPartId).then(response => {
            this.refreshData();
            this.deleteMesaggeAlert();
        });
    }

    successMesaggeAlert() {
        this.Success = true;
        this.MessageSuccess = "Success! Data has benn saved.";
        this.$timeout(() => {
            this.Success = false;
        }, 3000);
    }

    deleteMesaggeAlert() {
        this.Delete = true;
        this.MessageDelete = "Success! Data has been deleted.";
        this.$timeout(() => {
            this.Delete = false; 
        }, 3000);
    }
}

export class InspectionPartComponent implements angular.IComponentOptions {
    controller = InspectionPartController;
    controllerAs = 'me';

    template = require('./InspectionPart.html');
    bindings = {
        greet: '@',
    };
} 