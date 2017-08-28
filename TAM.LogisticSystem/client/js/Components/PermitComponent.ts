
import * as Service from '../Services';

export class PermitController implements angular.IController {
    static $inject = ['PermitService'];

    editBtn: boolean;

    Data: string;
    DataSide: string;

    CarModelData: JSON;
    CarModel: [{
        carModelCode: string;
        name: string;
    }];

    //CarModelValue: string;
    CarModelCode :string;
    CarModelName: string;

    KatashikiValue: string;
    SuffixValue: string;

    Permit: Service.PermitService;

    katashiki: string;
    suffix: string;
    modelName: string;
    quota: number;
    effectiveFrom: string;
    effectiveUntil: string;
    permitId: string;
	SearchKatashiki: string;

	createdAt: Date;
	createdBy: string;
	updateAt: Date;
	updateBy: string;

    constructor(permit: Service.PermitService) {
        this.Permit = permit;
        this.editBtn = false;
        //this.CarModel = [{
        //    carModelCode = string;
        //    name = string;
        //}];
    }

    $onInit() {
        this.refreshData();
    }

	checkInput()
	{
        //console.log('Success', this.katashiki, this.suffix, this.Permit, this.modelName, this.quota, this.effectiveFrom, this.effectiveUntil);
        console.log(this.CarModelData)
	}

	checkUpdate(data)
	{
         console.log(data);
         console.log(this.katashiki, this.suffix, this.Permit, this.quota, this.effectiveFrom, this.effectiveUntil, this.permitId, this.modelName);
    }
    

    createData() {
        //console.log(this.katashiki, this.suffix, this.permitId, this.quota, this.CarModelCode, this.modelName, this.effectiveFrom, this.effectiveUntil);
		this.createdAt = new Date;
		this.createdBy = 'Functional_Test';
		this.updateAt = new Date;
		this.updateBy = 'Functional_Test';
		this.Permit.PostData(this.katashiki, this.suffix, this.effectiveFrom, this.effectiveUntil, this.quota, this.permitId, this.modelName, this.CarModelCode, this.createdAt, this.createdBy, this.updateAt, this.updateBy).then(response => {
            //console.log(response.data);
            this.refreshData();
        });
    }

    selectData(data) {
		console.log('Success', data);
		this.CarModelCode = data.carModelCode;
        this.katashiki = data.katashiki;
        this.suffix = data.suffix;
        this.quota = data.quota;
        this.permitId = data.permitId;
        this.modelName = data.name;
        this.effectiveFrom = data.effectiveFrom;
        this.effectiveUntil = data.effectiveUntil;
        this.editBtn = true;
    }

    updateData() {
		//console.log(this.katashiki, this.suffix, this.permitId, this.quota, this.modelName, this.effectiveFrom, this.effectiveUntil);
		this.updateAt = new Date;
		this.updateBy = 'Functional_Test';
        this.Permit.UpdateData(this.katashiki, this.suffix, this.effectiveFrom, this.effectiveUntil, this.quota, this.permitId, this.modelName, this.CarModelCode, this.updateAt, this.updateBy).then(response => {
            this.refreshData();
        });
	}

	deleteData() {
		console.log(this.permitId);
		this.Permit.DeleteData(this.permitId).then(response => {
			console.log(response.data);
			this.refreshData();
		});
	}

    cascadeForSuffix() {
        this.KatashikiValue = this.katashiki;

        this.Permit.GetSuffix(this.KatashikiValue).then(response => {
            this.suffix = response.data;
            console.log(this.KatashikiValue, this.suffix);
        });
    }

    cascadeForModel(katashiki, suffix) {
       
        this.KatashikiValue = katashiki;
        this.SuffixValue = suffix;

     
        this.Permit.GetCarModel(this.KatashikiValue, this.SuffixValue).then(response => {
            this.CarModelData = response.data;
            console.log(this.CarModelData);
            
            this.modelName = response.data['name'];
            this.CarModelCode = response.data["carModelCode"];
            //this.CarModelValue = this.CarModelData[0]["name"];
        
            console.log(this.CarModelData, this.modelName, this.CarModelCode);
        });

    }



    //cascade() {
    //    if (this.katashiki.length > 3) {
    //        console.log(this.SearchKatashiki);

    //        this.Permit.GetData().then(response => {
    //            this.Data = response.data;
    //        });

    //    }
    //}


    refreshData() {

        this.katashiki = "";
        this.suffix = "";
        this.permitId = "";
        this.modelName = "";
        this.quota = 0;
        this.effectiveFrom = "";
        this.effectiveUntil = "";
        this.editBtn = false;

        this.Permit.GetAllData().then(response => {
            this.Data = response.data;
           
        });
    }

}

export class PermitComponent implements angular.IComponentOptions {
    controller = PermitController;
    controllerAs = 'me';

    template = require('./Permit.html');
    bindings = {
        greet: '@',
        //ftoken: '@'
    };
}