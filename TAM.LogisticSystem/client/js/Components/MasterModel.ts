
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as angular from 'angular';

export class MasterModelController implements angular.IController {
    static $inject = ['MasterModelService', '$rootScope'];

    $rootScope: angular.IRootScopeService;

    masterModel: Service.MasterModelService;
    myForm: angular.IFormController;

    count: number;
    
    viewby = 1;
    itemsCount = 5;
    currentPage = 1;
    itemsPerPage = this.viewby;
    maxSize = 5;
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    totalItems: number;
    pageNumber: number;

    orderState: boolean = false;
    orderString: string;
    searchString = {};
    data: any;
    dataBrand: any;
    dataManufacturing: any;
 
    brand: any;
    manufacturing: any;
    masterModelId: string;
    masterName: string;

    brandCode: string;
    brandName: string;
    brandModel: any;

    manufacturingCode: any;
    selectedBrand: string;

    editMe: boolean;
    succesNotification: boolean;
    errorNotication: boolean;
    messageSuccess: string;
    messageError: string;
    pageState: boolean = true;
    model: ModelInsert;

    errorMessage: any;
    errorState: boolean = false;
    isCodeExists: boolean = false;

    loader: boolean = true;
    jsonMasterModel = {
        "Kode Brand": null,
        "Kode Model": null,
        "Model": null,
        "Kode Manufacturing": null
    }


    constructor(masterModel: Service.MasterModelService, rootScope: angular.IRootScopeService) {
        this.count = 0;       
        this.pageSize = 5;
        this.masterModel = masterModel;
        this.model = new ModelInsert();
        this.$rootScope = rootScope;
            
        this.editMe = false;
       
        
    }

    isFormValid(form: angular.IFormController) {
        return angular.equals(form.$error, {});
    }

    

    refreshData(Form?: angular.IFormController) {
        this.masterModelId = '';
        this.brandModel = null;
        this.manufacturingCode = null;
        this.masterName = '';
        this.searchString = {};
        this.masterModel.GetData().then(response => {
            this.data = response.data;
            this.totalItems = response.data.length;
            this.itemsPerPage = this.viewby;
            this.itemsCount = this.totalItems;
            this.editMe = false;
            this.loader = false;
        });

        this.masterModel.GetDropdownBrand().then(response => {
            this.dataBrand = response.data;
        });

        this.masterModel.GetDropdownManufacturing().then(response => {
            this.dataManufacturing = response.data
        });
        
    }

    reset(Form: angular.IFormController) {
        this.editMe = false;
        this.brandModel = "";
        this.masterModelId = "";
        this.masterName = "";
        this.manufacturingCode = "";
        this.searchString = {};
        if (Form) {
            Form.$setPristine();
        }
    }

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            // angular's default (non-strict) internal comparator
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };
    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);

    }

    $onInit() {
        this.refreshData();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
        });
    }

    setPage(pageNo) {
        this.currentPage = pageNo;
    };

    setItemsPerPage(num) {
        this.itemsPerPage = num;
        this.currentPage = 1; //reset to first page
    }
    
   

    selectEdit(data) {
        console.log(data);
        this.editMe = true;       
        this.masterModelId = data.carModelCode;
        this.masterName = data.name;
        this.brandModel = _.find(this.dataBrand, ['brandCode', data.brandCode]);
        this.brandModel = _.find(this.dataBrand, ['name', data.brandName]);
        this.manufacturingCode = _.find(this.dataManufacturing, ['plantCode', data.plantCode]);
        this.manufacturingCode = _.find(this.dataManufacturing, ['name', data.plantName]);
        console.log(this.brandModel);    
    }

    selectDelete(data) {       
        this.masterModelId = data.carModelId;
        this.masterName = data.name;
    }

    SelectBrandOnChange() {
        this.brandName = this.brand["name"];
        this.brandCode = this.brand["brandCode"];
    }

    

   

   

    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["insert"] = 1;
        else if (action.toLowerCase() == "update") convertResult["update"] = 1;
        else if (action.toLowerCase() == "delete") convertResult["delete"] = 1;

        convertResult["grid"] = tempJson;
        return convertResult;
    }

  

   updatedData(Form: angular.IFormController) {
       this.jsonMasterModel["Kode Brand"] = this.brandModel['brandCode'] + ' - ' + this.brandModel['name'];
       this.jsonMasterModel["Kode Model"] = this.masterModelId.toUpperCase();
       this.jsonMasterModel["Model"] = this.masterName.toUpperCase();
       this.jsonMasterModel["Kode Manufacturing"] = this.manufacturingCode['plantCode'] + ' - ' + this.manufacturingCode['name'];

        if (this.brandModel == null) {
                Alertify.error('Kode Brand harus dipilih');
            } else if (this.manufacturingCode == null) {
                Alertify.error('Kode Manufacturing harus dipilih');
            } else {
                Alertify.confirm(
                    "Konfirmasi",
                    Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonMasterModel)),
                    () => {
                        this.masterModel.UpdateData(this.masterModelId, this.masterName, this.brandModel['brandCode'], this.manufacturingCode['plantCode']).then(response => {
                            Alertify.success("Data berhasil disimpan");
                            this.refreshData();
                            this.setPage(this.currentPage);
                            this.reset(Form);
                        }).catch(response => {
                            if (response.status == "500") {
                                Alertify.error("Koneksi ke server bermasalah");
                            } else {
                                Alertify.error(response.data);
                            }
                        });
                    },
                    () => { }
                ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
            }
        
    }

  
   

  

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    Download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.carModelCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "carModel";
        info.title = "Model";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

   
}

export class ModelInsert {
    modelCode: string;
    brandCode: string;
    name: string;
    manufacturingCode: string;
    
}

export class MasterModelComponent implements angular.IComponentOptions {
    controller = MasterModelController;
    controllerAs = 'mastermodel';

    template = require('./MasterModel.html');
    bindings = {
        greet: '@',
    };
}