
import * as Services from '../Services';

export class EngineController implements angular.IController {
    static $inject = ['EngineService'];

    Data: string;
    ActiveUser: string;

    //ini asu
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

    KatashikiData: string;

    Katashiki: [{
        katashiki: string;
    }];
    KatashikiValue: string;

    KatashikiValidationId: string;

    CarModelData: JSON;  
    CarModel: [{
        carModelId: number;
        carModelName: string;
    }];
    CarModelValue: string;


    ModelName: string;
    
    CarModelId: string;
    FrameCode: string;
    EnginePrefix: string;

    Engine: Services.EngineService;

    constructor(Engine: Services.EngineService) {
        this.Engine = Engine;
        this.Katashiki = [{
            katashiki: 'katashiki'
        }];
        this.CarModel = [{
            carModelId: 0,
            carModelName: 'carModelName'
        }];
    }

    $onInit() {
        this.refreshData();
    }

    cascadeForModel() {
        this.KatashikiValue = this.Katashiki["katashiki"];
        console.log(this.KatashikiValue)

        this.Engine.GetCarModel(this.KatashikiValue).then(response => {
            this.CarModelData = response.data;
            
            this.CarModelValue = this.CarModelData[0]['carModelName'];
            this.CarModelId = this.CarModelData[0]["carModelId"];
            console.log(this.KatashikiValue, this.CarModelId, this.CarModelValue);
        });
    }

    checkInput() {
        console.log('Success', this.KatashikiValue, this.CarModelId, this.CarModelValue, this.EnginePrefix, this.FrameCode);
    }



    selectEdit(data) {
        console.log('Success', data);
        this.KatashikiValidationId = data.katashikiValidationId;
        this.Katashiki["katashiki"] = data.katashiki;
        this.cascadeForModel();
        this.CarModelValue = data.carModelName;
  
        //this.CarModel["carModelName"] = data.carModelName;
        //this.CarModel["carModelId"] = data.carModelId;
        this.EnginePrefix = data.enginePrefix;
        this.FrameCode = data.frameCode;
        /*this.ShowEdit = false;*/
        console.log(this.KatashikiValidationId);
    }

    selectData(data) {
        this.KatashikiValidationId = data.katashikiValidationId;    
    }


    refreshData() {
        this.Engine.GetAllData().then(response => {
            console.log(response);
            this.Data = response.data;
            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;


            this.totalItems = Math.ceil(response.data.length / this.pageSize);

            this.currentPage = 1;

            
            this.CarModelData = null;
            this.CarModelId = null;
            this.CarModelValue = null;
            this.KatashikiValidationId = null;
            this.CarModel = null;
        });

        this.Engine.GetAll().then(response => {
            this.KatashikiData = response.data;
            console.log(this.KatashikiData);
        });

    }

    createData() {
        console.log(this.KatashikiValidationId, this.KatashikiValue, this.FrameCode, this.EnginePrefix);
        this.Engine.PostData(this.KatashikiValue, this.FrameCode, this.EnginePrefix).then(response => {
            console.log(response.data);
            this.refreshData();
        });
    }

    updateData() {
        console.log(this.KatashikiValidationId, this.KatashikiValue, this.FrameCode, this.EnginePrefix);
        this.Engine.UpdateData(this.KatashikiValidationId, this.KatashikiValue, this.FrameCode, this.EnginePrefix).then(response => {
            this.refreshData();
        });
    }

    deleteData() {
        console.log(this.KatashikiValidationId);
        this.Engine.DeleteData(this.KatashikiValidationId).then(response => {
            this.refreshData();
        });
    }

}

export class EngineComponent implements angular.IComponentOptions {
    controller = EngineController;
    controllerAs = 'me';

    template = require('./Engine.html');
    bindings = {
        greet: '@'
    };
}