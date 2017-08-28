import * as angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';

export class CarTypeController implements angular.IController {
    static $inject = ['CarTypeService', '$rootScope'];

    carType: Service.CarTypeService;
    $rootScope: angular.IRootScopeService;
    cartypeModel: CartypeModel;

    dataCarType: any;
    dataSeries: any;
    dataCategory: any;
    dataSteerPosition = [
        {
            "steerPosition": "R",
            //"steerPositionName": "Kanan"
        },
        {
            "steerPosition": "L",
            //"steerPositionName": "Kiri"
        }
    ];

    carSeries: any;
    carCategory: any;
    listSteerPosition: any;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;

    showHideButton: boolean = false;
    loader: boolean = true;

    Search: any = {};
    isFTZText: string;
    
    constructor(carType: Service.CarTypeService, rootScope: angular.IRootScopeService) {
        this.carType = carType;
        this.cartypeModel = new CartypeModel();
        this.$rootScope = rootScope;
        this.cartypeModel.isFTZ = null;
    }
    
    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    Download(result: any) {
        let tempData: any = {};
        let firstData = [];
        let secondData = [];
        angular.forEach(result, (data) => {
            firstData.push(data.katashiki);
            secondData.push(data.suffix);
        });
        tempData.firstProp = firstData;
        tempData.secondProp = secondData;
        this.pageState = false;
        let info: any = {};
        info.master = "CarType";
        info.title = "Tipe";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    Upload() {
        let info: any = {};
        info.master = "CarType";
        info.title = "Tipe";
        info.tipe = "2";
        this.pageState = false;
        this.$rootScope.$emit("UploadDownload", null, info);
    }

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };

    reset(myForm: angular.IFormController) {
        this.showHideButton = false;
        this.cartypeModel.katashiki = null;
        this.cartypeModel.suffix = null;

        this.carSeries = null;      
        this.carCategory = null;
        this.listSteerPosition = null;
        this.cartypeModel.name = null;
        //this.cartypeModel.hSCode = null;
        this.cartypeModel.engineDescription = null;
        this.cartypeModel.engineVolume = null;
        this.cartypeModel.wheelDiameter = null;
        this.cartypeModel.wheelSize = null;
        this.cartypeModel.assembly = null;
        this.cartypeModel.isFTZ = null;
        this.Search = {};
        if (myForm) {
            myForm.$setPristine();
        }
    }
     
    refresh(myForm?: angular.IFormController) {
        this.carType.getAllCarType().then(response => {
            this.dataCarType = response.data;            
            this.reset(myForm);
            this.loader = false;
            console.log(response.data);

            _.each(this.dataCarType, (item) => {
                item.isFTZText = (item.isFTZ ? 'Ya' : 'Tidak');
            });
        });

        this.carType.getAllCarSeries().then(response => {
            this.dataSeries = response.data;
        });


        this.carType.getAllAfiCarType().then(response => {
            this.dataCategory = response.data;
            console.log(this.dataCategory);
        });
    }

    $onInit() {
        this.refresh();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
            this.refresh();
        });
    }

    selectEdit(data) {
        console.log(data);
        this.showHideButton = true;
        this.cartypeModel.katashiki = data.katashiki;
        this.cartypeModel.suffix = data.suffix;
        this.carSeries = _.find(this.dataSeries, ['carSeriesCode', data.carSeriesCode]);
        //this.carSeries = _.find(this.dataSeries, ['carSeriesName', data.carSeriesName]);
        this.carCategory = _.find(this.dataCategory, ['afiCarTypeCode', data.afiCarTypeCode]);
        //this.carCategory = _.find(this.dataCategory, ['aficartypeName', data.carCategoryName]);
        this.listSteerPosition = _.find(this.dataSteerPosition, ['steerPosition', data.steerPosition]);
       
        this.cartypeModel.name = data.name; 
        //this.cartypeModel.hSCode = data.hSCode;
        this.cartypeModel.engineDescription = data.engineDescription;
        this.cartypeModel.engineVolume = data.engineVolume;
        this.cartypeModel.wheelDiameter = <any>data.wheelDiameter;
        this.cartypeModel.wheelSize = data.wheelSize;
        this.cartypeModel.assembly = data.assembly;1
        this.cartypeModel.isFTZ = data.isFTZ;
        //this.cartypeModel.carCategoryId = data.carCategoryId;
        
    }   

    postCartype(myForm: angular.IFormController) {
        console.log(this.carCategory);
        let json = {};
        json["Katashiki"] = this.cartypeModel.katashiki;
        json["Suffix"] = this.cartypeModel.suffix;
        json["Tipe"] = this.cartypeModel.name;
        json["Engine Desc."] = this.cartypeModel.engineDescription;
        json["CC"] = this.cartypeModel.engineVolume;
        json["Steer Position"] = this.listSteerPosition.steerPosition;
        json["Wheel Diameter"] = this.cartypeModel.wheelDiameter;
        json["Wheel Size"] = this.cartypeModel.wheelSize;
        json["Assembly"] = this.cartypeModel.assembly;
        json["Kode Model Series"] = this.carSeries.carSeriesName;
        json["Kode Jenis"] = this.carCategory.aficartypeName;  
        json["FTZ"] = this.cartypeModel.isFTZ ? "Ya" : "Tidak";

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                //if (_.find(this.dataCarType, ['katashiki', 'suffix', this.cartypeModel.katashiki, this.cartypeModel.suffix])) {
                if (_.find(this.dataCarType, (data: any) => {
                    return <any>data.katashiki.toLocaleLowerCase() == this.cartypeModel.katashiki.toLocaleLowerCase() && <any>data.suffix.toLocaleLowerCase() == this.cartypeModel.suffix.toLocaleLowerCase();
                })) {
                    Alertify.error("Tipe sudah terdaftar");
                    return;
                }
                this.createData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
     
    createData(myForm: angular.IFormController) {
        this.cartypeModel.carCategoryId = this.carCategory.carCategoryId;
        this.cartypeModel.carSeriesCode = this.carSeries.carSeriesCode;
        this.cartypeModel.steerPosition = this.listSteerPosition.steerPosition;
        //this.cartypeModel.hSCode = "91OB1L";
        //this.dataCarType.push(this.cartypeModel);
        this.carType.postData(this.cartypeModel).then(
            response => {
                if (response.status != 200) {
                     Alertify.error("Data gagal disimpan");  
                }
                else {   
                    this.refresh(myForm);
                    Alertify.success("Data berhasil disimpan");
                }
            }
        )
    } 

    updateCartype(myForm: angular.IFormController) {
        let json = {};
        json["Katashiki"] = this.cartypeModel.katashiki; 
        json["Suffix"] = this.cartypeModel.suffix;
        json["Tipe"] = this.cartypeModel.name; 
        json["Engine Desc."] = this.cartypeModel.engineDescription;
        json["CC"] = this.cartypeModel.engineVolume;
        json["Steer Position"] = this.listSteerPosition["steerPosition"],
        json["Wheel Diameter"] = this.cartypeModel.wheelDiameter;
        json["Wheel Size"] = this.cartypeModel.wheelSize; 
        json["Assembly"] = this.cartypeModel.assembly;
        json["Kode Model Series"] = this.carSeries["carSeriesName"];
        json["Kode Jenis"] = this.carCategory["afiCarTypeCode"];
        json["FTZ"] = this.cartypeModel.isFTZ ? "Ya" : "Tidak";

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", json)),
            () => {
                this.updateData(myForm);
            },
            () => { }  
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(myForm: angular.IFormController) {
        this.cartypeModel.afiCarTypeCode = this.carCategory.afiCarTypeCode;
        this.cartypeModel.carSeriesCode = this.carSeries.carSeriesCode;
        this.cartypeModel.steerPosition = this.listSteerPosition.steerPosition;
        //this.cartypeModel.hSCode = "91OB1L";
        //this.dataCarType.push(this.cartypeModel);
        console.log(this.cartypeModel);
        this.carType.updateData(this.cartypeModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal diubah");
                }
                else {
                    this.refresh(myForm);
                    Alertify.success("Data berhasil disimpan");
                }
            }
        )
    }

    deleteCartype(data, myForm: angular.IFormController) {
        let json = {};
        json["Katashiki"] = data.katashiki;
        json["Suffix"] = data.suffix;
        json["Tipe"] = data.name;
        json["Engine Desc."] = data.engineDescription;
        json["CC"] = data.engineVolume;
        json["Steer Position"] = data.steerPosition;
        json["Wheel Diameter"] = data.wheelDiameter;
        json["Wheel Size"] = data.wheelSize;
        json["Assembly"] = data.assembly;
        json["Kode Model Series"] = data.carSeriesCode;
        json["Kode Jenis"] = data.carCategoryName;
        json["FTZ"] = data.isFTZ ? "Ya" : "Tidak";

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", json)),
            () => {
                this.deleteData(data.katashiki, data.suffix, myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(katashiki, suffix, myForm: angular.IFormController) {
        this.cartypeModel.katashiki = katashiki;
        this.cartypeModel.suffix = suffix;
        let key = _.findIndex(this.dataCarType, { katashiki, suffix });
        this.dataCarType.splice(key, 1);
        console.log(this.dataCarType);
        this.carType.deleteData(this.cartypeModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal dihapus");
                }
                else {
                    this.refresh(myForm);
                    Alertify.success("Data berhasil dihapus");
                }
            }
        )
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
    
}

export class CartypeModel {
    katashiki: string;
    suffix: string;
    name: string;
    //hSCode: string;
    engineDescription: string;
    engineVolume: string;
    steerPosition: string;
    wheelDiameter: number;
    wheelSize: string;
    assembly: string;
    carSeriesCode: string;
    carCategoryId: number;
    isFTZ: boolean; 
    afiCarTypeCode: string;

    carSeriesName: string;
    carCategoryName: string;
}

export class CarType implements angular.IComponentOptions {
    controller = CarTypeController;
    controllerAs = 'me';
    template = require('./CarType.html');
} 