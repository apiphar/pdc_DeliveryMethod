import * as moment from 'moment';
import * as angular from 'angular';
import * as Service from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as mustache from 'mustache';


class Region {
    postCode: string;
    afiRegionCode: string;
}

export class MasterRegionAfiController implements angular.IController {
    static $inject = ['MasterRegionAfiService', '$rootScope', '$scope'];

    constructor(regionAfiService: Service.MasterRegionAfiService, root: angular.IRootScopeService, $scope: angular.IScope) {
        this.regionAfiService = regionAfiService;
        this.$scope = $scope;
        this.root = root;
       

    }

    regionAfiService: Service.MasterRegionAfiService;
    $scope: angular.IScope;

    regexStringCode: RegExp = /^[A-Za-z0-9]+$/;
    regexStringName: RegExp = /^[a-zA-Z0-9\s\-.&,\'/]+$/;


    jsonInfo = {};
    pageState: boolean = true;
    root: angular.IRootScopeService;
    searchAfi = {};
    postCodeData: Service.PostCode[];
    regionAfiData: Service.RegionAfi[];
    regionData: any;
    singleRegionAfiData: Service.RegionAfi;
    singleRegionData: Region;
    
    afiRegionCode: any;
    postCode: any;

    editChecked: boolean = false;

    $onInit() {
        this.getAllRegionData();
        this.getAllRegionAfiData();
        this.getPostCode();
        
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.getAllRegionAfiData();

            //this.getPostCode();
            //console.log(this.getPostCode);
        });
        this.editChecked = false;
        this.singleRegionData = new Service.Region();
        

    }

    //untuk pagination
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;


    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {

        this.totalItems = data.length;
        this.setPage(1);

    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

    //pagination end

    //untuk ambil semua data regionafi dari DB
    getAllRegionAfiData() {
        this.regionAfiService.getAllRegionAfiData().then(result => {
            this.regionAfiData = result.data as Service.RegionAfi[];
           
        })
    }
    getPostCode() {
        this.regionAfiService.getPostCode().then(result => {
            this.postCodeData = result.data as Service.PostCode[];
        })
    }

    getAllRegionData() {
        this.regionAfiService.getAllRegionData().then(result => {
            this.regionData = result.data;
            this.totalItems = this.regionData.length;

        });
    }

    /*/tambah region afi
    /addRegionAfiData(form) {
        this.singleRegionData.postCode = this.postCode;
        this.singleRegionData.afiRegionCode = this.afiRegionCode;

        this.jsonInfo["Post Code"] = this.postCode["postCode"];
        this.jsonInfo["AFI Region Code"] = this.afiRegionCode["afiRegionCode"] + " - " + this.afiRegionCode["name"];

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonInfo)),
            () => {
                this.regionAfiService.addRegionAfiData(this.singleRegionData).then(result => {
                    
                    
                        this.resetAll(form);
                        this.getAllRegionAfiData();
                        alertify.success("Data berhasil disimpan");
                    
                   
                }).catch(result => {
                    if (result.data === "Data sudah terdaftar") {
                        alertify.error("Data sudah terdaftar");
                        
                    }
                })
                  
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

        
    }
    */
    //delete region afi data
    deleteRegionAfiData(data,form) {

        this.jsonInfo["Post Code"] = data["postCode"];
        let temp = _.find(this.regionAfiData, ["afiRegionCode", data["afiRegionCode"]]);
        this.jsonInfo["AFI Region Code"] = temp["afiRegionCode"] + " - " + temp["name"];
        console.log("delete", data);
        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonInfo)),
            () => {

                //this.singleRegionData.afiRegionCode = this.afiRegionCode["afiRegionCode"];
                //this.singleRegionData.postCode = this.postCode["postCode"];
                this.regionAfiService.deleteRegionAfiData(data['regionCode']).then(result => {
                    alertify.success("Data berhasil dihapus");
                    this.getAllRegionData();
                    this.resetAll(form);
                    
                });

            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });

        //alertify.confirm(
        //    "Konfirmasi",
        //    mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonInfo)),
        //    () => {
        //        this.regionAfiService.deleteRegionAfiData(data["region"]).then(result => {
        //            alertify.success("Data berhasil dihapus");
        //            this.getAllRegionData();
        //            this.resetAll(form);
        //            console.log(this.jsonInfo);
        //        });

        //    },
        //    () => { }
        //).set('labels', { ok: 'Ya', cancel: 'Tidak' }); 
    }

    //button edit
    editClicked(data) {
        console.log(this.editChecked);
        this.editChecked = true;
        this.afiRegionCode = _.find(this.regionAfiData, ['afiRegionCode', data.afiRegionCode]);
        this.postCode = _.find(this.postCodeData, ['postCode', data.postCode]);
        console.log(this.editChecked);

       
    }

    id: string
    //update data region afi
    updateRegionAfiData(form) {

        /*this.singleRegionData.postCode = this.postCode*/

        this.id = this.postCode
        //this.singleRegionData.afiRegionCode = this.afiRegionCode;
        //this.singleRegionData.postCode = this.postCode;   
        this.jsonInfo["Post Code"] = this.postCode["postCode"];
        this.jsonInfo["AFI Region Code"] = this.afiRegionCode["afiRegionCode"] + " - " + this.afiRegionCode["name"];

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonInfo)),
            () => {

                this.singleRegionData.afiRegionCode = this.afiRegionCode["afiRegionCode"];
                this.singleRegionData.postCode = this.postCode["postCode"];
                this.regionAfiService.updateRegionAfiData(this.singleRegionData).then(result => {
                    this.getAllRegionData();                
                    this.resetAll(form);
                    alertify.success("Data berhasil disimpan");
                });

            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }


    //convert data ke json
    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin untuk menambahkan data ?";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data ?";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data ?";
        convertResult["grid"] = tempJson;
        return convertResult;
    }



    // Download button
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.regionAFICode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "RegionAfi";
        info.title = "Region Afi";
        info.tipe = "3";
        this.root.$emit("UploadDownload", tempData, info);
    }

    // Upload button
    upload() {
        let info: any = {};
        info.master = "RegionAfi";
        info.title = "Region Afi"
        info.tipe = "2";
        this.pageState = false;
        this.root.$emit("UploadDownload", null, info);
    }



    //reset form
    resetAll(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.postCode = null;
        this.afiRegionCode = null;
        this.singleRegionAfiData = new Service.RegionAfi;
        this.editChecked = false;
        this.searchAfi = {};
    }


}


let MasterRegionAfi = {
    controller: MasterRegionAfiController,
    controllerAs: 'me',

    template: require('./MasterRegionAfi.html')
}

export { MasterRegionAfi }
