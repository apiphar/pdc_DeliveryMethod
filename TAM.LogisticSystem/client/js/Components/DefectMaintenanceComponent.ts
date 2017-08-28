
import * as Service from '../Services';

export class DefectMaintenanceController implements angular.IController {
    static $inject = ['DefectMaintenanceService'];

    Data: string;
    DataSide: string;

    

    //ini paging
    viewby = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    //pageSize = 5; 
    pageSizes = [2, 5, 10, 20];
    pageSize = this.pageSizes[0]
    totalItems: number;
    pageNumber: number;

    DefectMaintenance: Service.DefectMaintenanceService;

    name: string;
    defectId: number;
    createdAt: Date;
    createdBy: string;
    updateAt: Date;
    updateBy: string;

    constructor(defectMaintenance: Service.DefectMaintenanceService) {
        this.DefectMaintenance = defectMaintenance;
    }

    $onInit() {
        this.DefectMaintenance.GetData().then(response => {
            this.Data = response.data;
        });
    }

    checkInput() {
        console.log('Success', this.name);
    }

    checkUpdate(data) {
        console.log('Success', this.defectId, this.name);
    }

    checkDelete(data) {
        console.log('Success', this.name);
    }
    
    createData() {
        console.log(this.name);
        this.createdAt = new Date;
        this.updateAt = new Date;
        this.DefectMaintenance.PostData(this.name,this.createdAt,this.createdBy,this.updateAt,this.updateBy).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    selectData(data) {
        console.log('Success', data);
        this.name = data.name;
        this.defectId = data.defectId;
    }

    updateData() {
        console.log(this.defectId, this.name);
        this.DefectMaintenance.UpdateData(this.defectId, this.name).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    } 

    deleteData() {
        console.log(this.defectId);
        this.DefectMaintenance.DeleteData(this.defectId).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    refreshData() {
       
        this.name = "";

        this.DefectMaintenance.GetData().then(response => {
            this.Data = response.data;
            this.Data = response.data;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;


            this.totalItems = Math.ceil(response.data.length / this.pageSize);

            this.currentPage = 1;
        });
    }    
}

export class DefectMaintenanceComponent implements angular.IComponentOptions {
    controller = DefectMaintenanceController;
    controllerAs = 'me';

    template = require('./DefectMaintenance.html');
    bindings = {
        greet: '@',
        //ftoken: '@'
    };
}