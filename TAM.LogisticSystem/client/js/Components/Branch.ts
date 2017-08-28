import * as Angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';

export class BranchAPIController implements angular.IController {
    static $inject = ['BranchService', '$rootScope'];

    branch: Service.BranchService;
    $rootScope: angular.IRootScopeService;
    branchModel: BranchModel;

    dataBranch: any;
    dataSalesArea: any;
    dataDestination: any;
    dataRegion: any;
    dataCompany: any;
    dataLocation: any;
    //dataLocation2: any;
    dataCluster: any;

    salesArea: any;
    destination: any;
    region: any;
    company: any;
    location: any = {
        locationCode: '',
        locationName: ''
    };
    //location2: any;  
    cluster: any;
    Search: any = {};
    jsonBranch = {};

    regexPattern = "^[a-zA-Z0-9\ \,\.\;\\-\/\&\'\"\(\)]*$";

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;  
    totalItems: number;
    pageState: boolean = true;

    showHideForm: boolean = false;
    loader: boolean = true;

    constructor(branch: Service.BranchService, rootScope: angular.IRootScopeService) {
        this.branch = branch;
        this.branchModel = new BranchModel();
        this.$rootScope = rootScope;
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
        let tempData = [];
        Angular.forEach(result, (data) => {
            tempData.push(data.branchCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Branch";
        info.title = "Branch";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    //Upload() {
    //    let info: any = {};
    //    info.master = "Branch";
    //    info.title = "Branch";  
    //    info.tipe = "2";
    //    this.pageState = false;
    //    this.$rootScope.$emit("UploadDownload", null, info);
    //}

    allowNullValue(expected, actual) {
        if (actual === null) {
            return true;
        } else {
            var text = ('' + actual).toLowerCase();
            return ('' + expected).toLowerCase().indexOf(text) > -1;
        }
    };

    reset(myForm: angular.IFormController) {
        this.showHideForm = false;
        this.branchModel.aS400BranchCode = null;
        this.branchModel.branchCode = null;
        this.branchModel.afiBranchCode = null;
        this.branchModel.name = null;
        this.branchModel.phone = null;
        this.branchModel.fax = null;
        this.branchModel.kabupatenCode = null;
        this.salesArea = null;
        this.company = null;
        this.region = null;
        this.destination = null;
        this.location = null;
        this.cluster = null;
        this.Search = {};
        if (myForm) {
            myForm.$setPristine();
            myForm.$setUntouched();
        }
    }

    refreshData(myForm?: angular.IFormController) {
        this.branch.getDataBranch().then(response => {
            this.dataBranch = response.data;
            this.loader = false;
            this.reset(myForm);
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
        this.branch.getDataSalesArea().then(response => {
            this.dataSalesArea = response.data;
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
        this.branch.getDataDestination().then(response => {
            this.dataDestination = response.data;
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
        this.branch.getDataRegion().then(response => {
            this.dataRegion = response.data;
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
        this.branch.getDataCompany().then(response => {
            this.dataCompany = response.data;
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
        this.branch.getDataLocation().then(response => {
            this.dataLocation = response.data;
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
        this.branch.getDataCluster().then(response => {
            this.dataCluster = response.data;
        }).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
    }

    $onInit() {
        this.refreshData();
        this.$rootScope.$on("Kembali", (event) => {
            this.pageState = true;
        });
    }

    selectLocation(selectItem) {

        let a = JSON.parse(JSON.stringify(selectItem));
        this.branchModel.branchCode = a.locationCode;
        this.branchModel.name = a.locationName;
    };

    //getDataLocation2(data) {
    //    this.branch.getDataLocation2(data.locationCode).then(response => {
    //        this.dataLocation2 = response.data;
    //    });
    //}


    //locations: locationModel[];

    //getDataLocation2(data) {

    //    //this.location = _.find(this.dataLocation, ['locationCode', data.locationCode]);
    //    //this.location = _.find(this.dataLocation, ['locationName', data.locationName]);

    //    data.locationCode = this.location;
    //    data.locationName = this.location;

    //    this.dataLocation2.push(this.dataLocation);

    //    this.branch.getDataLocation().then(response => {
    //        this.dataLocation2 = response.data;
    //    });
    //}

    selectEdit(data, myForm: angular.IFormController) {
        //this.getDataLocation2(data);  

        this.showHideForm = true;

        this.branchModel.aS400BranchCode = data.aS400BranchCode;
        this.branchModel.branchCode = data.branchCode;
        this.branchModel.afiBranchCode = data.afiBranchCode;
        this.branchModel.name = data.name;
        this.branchModel.phone = data.phone;
        this.branchModel.fax = data.fax;
        this.branchModel.kabupatenCode = data.kabupatenCode;
        this.salesArea = _.find(this.dataSalesArea, ['carSeriesCode', data.carSeriesCode]);
        this.salesArea = _.find(this.dataSalesArea, ['salesAreaName', data.salesAreaName]);
        this.destination = _.find(this.dataDestination, ['destinationCode', data.destinationCode]);
        this.destination = _.find(this.dataDestination, ['destinationName', data.destinationName]);
        this.company = _.find(this.dataCompany, ['companyCode', data.companyCode]);
        this.company = _.find(this.dataCompany, ['companyName', data.companyName]);
        //this.region = {
        //    regionCode: data.regionCode ? data.regionCode: null,
        //    locationName: data.locationName ? data.locationName : null
        //}
        this.region = _.find(this.dataRegion, ['regionCode', data.regionCode]);
        this.region = _.find(this.dataRegion, ['regionName', data.regionName]);
        this.location = _.find(this.dataLocation, ['locationCode', data.locationCode]);
        this.location = _.find(this.dataLocation, ['locationName', data.locationName]);
        //this.location2 = _.find(this.dataLocation2, ['locationCode', data.locationCode]);
        //this.location2 = _.find(this.dataLocation2, ['locationName', data.locationName]);
        this.cluster = _.find(this.dataCluster, ['aS400ClusterCode', data.aS400ClusterCode]);
        this.cluster = _.find(this.dataCluster, ['clusterName', data.clusterName]);
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        Angular.forEach(json, (value, key) => {
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

    postBranch(myForm: angular.IFormController) {
        let json = {};
        json["Kode Sales Area"] = this.salesArea.salesAreaName;
        json["Kode Company"] = this.company.companyName;
        json["Kode Lokasi"] = this.location.locationName;
        json["Kode Branch"] = this.branchModel.branchCode;
        json["Branch"] = this.branchModel.name;
        json["No. Telp"] = this.branchModel.phone;
        json["No. Fax"] = this.branchModel.fax;
        json["Destination Code"] = this.destination.destinationName;
        json["Region Code"] = this.region.regionName;
        json["Kode AFI Branch"] = this.branchModel.afiBranchCode;
        json["Branch AS 400"] = this.branchModel.aS400BranchCode;
        json["Kode Cluster"] = this.cluster.clusterName;
        json["Kode Kabupaten"] = this.branchModel.kabupatenCode;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                if (_.find(this.dataBranch, (data: any) => {
                    return <any>data.branchCode.toLocaleLowerCase() == this.branchModel.branchCode.toLocaleLowerCase();
                })) {
                    Alertify.error("Kode branch sudah terdaftar");
                    return;
                }
                else if (_.find(this.dataBranch, (data: any) => {
                    return <any>data.locationCode == this.location.locationCode;
                })) {
                    Alertify.error("Kode lokasi sudah digunakan, silahkan pilih kode lokasi yang lain!");
                    return;
                }
                this.createData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    createData(myForm: angular.IFormController) {
        this.branchModel.salesAreaCode = this.salesArea.salesAreaCode;
        this.branchModel.aS400ClusterCode = this.cluster.aS400ClusterCode;
        this.branchModel.companyCode = this.company.companyCode;
        this.branchModel.destinationCode = this.destination.destinationCode;
        this.branchModel.locationCode = this.location.locationCode;
        this.branchModel.regionCode = this.region.regionCode;
        //this.dataBranch.push(this.branchModel);
        this.branch.postData(this.branchModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal disimpan");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data berhasil disimpan");
                }
            }
        )
    }

    updateBranch(myForm: angular.IFormController) {
        this.jsonBranch["Kode Sales Area"] = this.salesArea.salesAreaName;
        this.jsonBranch["Kode Company"] = this.company.companyName;
        this.jsonBranch["Kode Lokasi"] = this.location.locationName;
        this.jsonBranch["Kode Branch"] = this.branchModel.branchCode;
        this.jsonBranch["Branch"] = this.branchModel.name;
        this.jsonBranch["No. Telp"] = this.branchModel.phone;
        this.jsonBranch["No. Fax"] = this.branchModel.fax;
        this.jsonBranch["Destination Code"] = this.destination.destinationName;
        this.jsonBranch["Region Code"] = this.region.regionName;
        this.jsonBranch["Kode AFI Branch"] = this.branchModel.afiBranchCode;
        this.jsonBranch["Branch AS 400"] = this.branchModel.aS400BranchCode;
        this.jsonBranch["Kode Cluster"] = this.cluster.clusterName;
        this.jsonBranch["Kode Kabupaten"] = this.branchModel.kabupatenCode;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonBranch)),
            () => {
                if (_.find(this.dataBranch, (data: any) => {
                    return <any>data.locationCode == this.location.locationCode && <any>data.branchCode.toLocaleLowerCase() != this.branchModel.branchCode.toLocaleLowerCase();
                })) {
                    Alertify.error("Kode lokasi sudah digunakan, silahkan pilih kode lokasi yang lain!");
                    return;
                }
                this.updateData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(myForm: angular.IFormController) {
        this.branchModel.salesAreaCode = this.salesArea.salesAreaCode;
        this.branchModel.aS400ClusterCode = this.cluster.aS400ClusterCode;
        this.branchModel.companyCode = this.company.companyCode;
        this.branchModel.destinationCode = this.destination.destinationCode;
        this.branchModel.locationCode = this.location.locationCode;
        this.branchModel.regionCode = this.region.regionCode;
        //this.dataBranch.push(this.branchModel);
        this.branch.updateData(this.branchModel).then(
            response => {
                this.refreshData(myForm);
                this.reset(myForm);
                Alertify.success("Data berhasil disimpan");
            }
        ).catch(
            response => {
                if (response.status == "400") {
                    Alertify.error("Data gagal disimpan");
                    return;
                } if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
            )
    }

    deleteBranch(data, myForm: angular.IFormController) {
        let json = {};
        this.jsonBranch["Kode Sales Area"] = data.salesAreaName;
        this.jsonBranch["Kode Company"] = data.companyName;
        this.jsonBranch["Kode Lokasi"] = data.locationName;
        this.jsonBranch["Kode Branch"] = data.branchCode;
        this.jsonBranch["Branch"] = data.name;
        this.jsonBranch["No. Telp"] = data.phone;
        this.jsonBranch["No. Fax"] = data.fax;
        this.jsonBranch["Destination Code"] = data.destinationName;
        this.jsonBranch["Region Code"] = data.regionName;
        this.jsonBranch["Kode AFI Branch"] = data.afiBranchCode;
        this.jsonBranch["Branch AS 400"] = data.aS400BranchCode;
        this.jsonBranch["Kode Cluster"] = data.clusterName;
        this.jsonBranch["Kode Kabupaten"] = data.kabupatenCode;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonBranch)),
            () => {
                this.deleteData(data.branchCode, myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(branchCode, myForm: angular.IFormController) {
        //let key = _.findIndex(this.dataBranch, { branchCode });
        //this.dataBranch.splice(key, 1);
        this.branch.deleteData(branchCode).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal dihapus");
                }
                else {
                    this.refreshData(myForm);
                    Alertify.success("Data berhasil dihapus");
                }
            }
        )
    }
}

export class BranchModel {
    branchCode: string;
    salesAreaCode: string;
    destinationCode: string;
    regionCode: string;
    companyCode: string;
    locationCode: string;
    aS400ClusterCode: string;
    aS400BranchCode: string;
    afiBranchCode: string;
    name: string;
    phone: string;
    fax: string;
    kabupatenCode: string;

    salesAreaName: string;
    destinationName: string;
    regionName: string;
    companyName: string;
    locationName: string;
    clusterName: string;
}

export class locationModel {
    locationCode: string;
    locationName: string;
}

export class Branch implements angular.IComponentOptions {
    controller = BranchAPIController;
    controllerAs = 'me';
    template = require('./Branch.html');
}