
import * as Service from '../services';

export class MasterModelSerieslController implements angular.IController {
    static $inject = ['Value', 'MasterModelSeriesService'];

    MasterModelSeries: Service.MasterModelSeriesService;

    viewby = 1;
    itemsCount = 5;
    currentPage = 1;
    itemsPerPage = this.viewby;
    maxSize = 5;
    pageSize= 5;
    totalItems: number;
    pageNumer: number;
    

    Value: number;
    Count: number;

    Data: string;
    DataModel: string;
    
    MasterModel: [{
        carModelCode: string,
        name: string
    }];

    MasterModelSeriesCode: string;
    MasterModelName: string;

    MasterModelCode: string;
    MasterName: string;
    
    SelectedMasterModel: string;
    
    EditMe: boolean;
    succesNotification: boolean;
    errorNotication: boolean;
    messageSuccess: string;
    messageError: string;

    constructor(value: number, masterModelSeries: Service.MasterModelSeriesService) {
        this.Count = 0;
        this.totalItems = 0;
        this.MasterModelSeries = masterModelSeries;
        this.MasterModelCode = '';
        this.MasterModel = [{
            carModelCode: '',
            name: '-- Please Choose Model --'
        }];

        this.MasterModelSeriesCode = null;
        this.EditMe = false;
        this.succesNotification = false;
        this.errorNotication = false;
    }

    refreshData() {
        this.MasterModelSeries.GetData().then(response => {
            this.Data = response.data;
            this.totalItems = response.data.length;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.EditMe = false;
            this.succesNotification = false;
            this.errorNotication = false;
            this.MasterModelName = null;
            this.MasterModelSeriesCode = null;
            this.SelectedMasterModel = "'-- Please Choose Model --'";
            //this.sideName = "(Please Choose One)";
            this.MasterModelCode = '';
            this.MasterModel["carModelCode"] = '';
            this.MasterModel["name"] = "'-- Please Choose Model --'";
            
        });

        this.MasterModelSeries.GetDropdownCarModel().then(response => {
            this.DataModel = response.data;
          
            
            console.log(this.totalItems);
        });

       
    }

    $onInit() {
        this.refreshData();
    }



    checkInput() {
        if (this.MasterModelSeriesCode == null || this.MasterModelName == null || this.MasterModelCode == '') {
            alert('Please Entry');
            $('#SeriesCode').focus();
            this.refreshData();
        } else {
            console.log('Success', this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode);
        }
    }

    checkUpdate(data) {
        console.log('Success', this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode);
    }

    checkDelete(data) {
        console.log('Success', this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode);
    }
    createData() {
        console.log(this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode);
        this.MasterModelSeries.PostData(this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode).then(response => {
            if (response.data == 1) {
               
                this.messageSuccess = "Data has been Saved";
                this.succesNotification = true;
              // this.refreshData();

            } else {

                this.messageError = "Data hasn't been saved, Please Check Your Entry!";
                this.errorNotication = true;
                //this.refreshData();
            }
        });
        
    }

    selectEdit(data) {
        console.log('Success', data);
        console.log('Test...', data.masterName);
        this.MasterModelSeriesCode = data.carSeriesCode;
        this.MasterModelName = data.carSeriesName;
        //this.SelectedBrand = data.brand;
        //this.BrandCode = data.brandCode;
        //this.BrandName = data.brandName;
        this.MasterModel["name"] = data.carModelName;
        this.MasterModel["carModelCode"] = data.carModelCode;
        this.EditMe = true;
       // $('#SeriesCode').prop('readonly', true);
        
        //this.CascadeCategory();
    }

    selectDelete(data) {
        console.log('Test...', data);
        this.MasterModelSeriesCode = data.carSeriesCode;
        this.MasterName = data.name;
    }

    SelectModelOnChange() {
        console.log(this.MasterModel);
        this.MasterName = this.MasterModel["name"];
        this.MasterModelCode = this.MasterModel["carModelCode"];
        console.log('selected', this.MasterName, this.MasterModelCode);
    }

    updateData() {
        console.log(this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode);
        this.MasterModelSeries.UpdateData(this.MasterModelSeriesCode, this.MasterModelName, this.MasterModelCode).then(response => {
            if (response.data == 1) {

                this.messageSuccess = "Data has been Changed";
                this.succesNotification = true;
                // this.refreshData();

            } else {

                this.messageError = "Data hasn't been Changed, Please Check Your Entry!";
                this.errorNotication = true;
                //this.refreshData();
            }
        });
    }

    deleteData() {
        this.MasterModelSeries.DeleteData(this.MasterModelSeriesCode).then(response => {
            if (response.data == 1) {

                this.messageSuccess = "Data has been Deleted";
                this.succesNotification = true;
                // this.refreshData();

            } else {

                this.messageError = "Data hasn't been Deleted, Please Check Your Data!";
                this.errorNotication = true;
                //this.refreshData();
            }
        });
    }
}
export class MasterModelSeriesComponent implements angular.IComponentOptions {
    controller = MasterModelSerieslController;
    controllerAs = 'mastermodelseries';

    template = require('./MasterModelSeries.html');
    bindings = {
        great: '@',
    };
}