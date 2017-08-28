import * as Angular from 'angular';
import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as _ from 'lodash';

export class ClusterController implements angular.IController {
    static $inject = ['ClusterService', '$rootScope'];

    cluster: Service.ClusterService;
    $rootScope: angular.IRootScopeService;

    clusterModel: ClusterModel;
    dataCluster: any;
    jsonCluster = {};
     
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = 5;
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;
    pageState: boolean = true;

    showHideButton: boolean = false;
    loader: boolean = true;
    Search: any = {};

    constructor(cluster: Service.ClusterService, rootScope: angular.IRootScopeService) {
        this.cluster = cluster;
        this.clusterModel = new ClusterModel();
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
            tempData.push(data.aS400ClusterCode);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "Cluster";
        info.title = "Cluster";
        info.tipe = "3";
        this.$rootScope.$emit("UploadDownload", tempData, info);
    }

    Upload() {
        let info: any = {};
        info.master = "Cluster";
        info.title = "Cluster";
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
        this.clusterModel.aS400ClusterCode = null;
        this.clusterModel.name = null;
        this.Search = {};

        if (myForm) {
            myForm.$setPristine();
        }
    }

    refresh(myForm?: angular.IFormController) {
        this.cluster.getDataCluster().then(response => {
            this.dataCluster = response.data;
            this.reset(myForm);
            this.loader = false;
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
        this.showHideButton = true;
        this.clusterModel.aS400ClusterCode = data.aS400ClusterCode;
        this.clusterModel.name = data.name;
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

    postCluster(myForm: angular.IFormController) {
        this.jsonCluster["Kode Cluster"] = this.clusterModel.aS400ClusterCode;
        this.jsonCluster["Cluster"] = this.clusterModel.name;

        Alertify.confirm(  
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.jsonCluster)),
            () => {
                if (_.find(this.dataCluster, (data: any) => {
                    return <any>data.aS400ClusterCode.toLocaleLowerCase() == this.clusterModel.aS400ClusterCode.toLocaleLowerCase();
                }))
                {
                    Alertify.error("Kode Cluster sudah terdaftar");
                    return;
                }
                this.createData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    createData(myForm: angular.IFormController) {
        this.cluster.postData(this.clusterModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal disimpan");
                }
                else {
                    this.refresh(myForm);
                    Alertify.success("Data berhasil disimpan");
                }
            }
        ).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
    }

    updateCluster(myForm: angular.IFormController) {
        this.jsonCluster["Kode Cluster"] = this.clusterModel.aS400ClusterCode;
        this.jsonCluster["Cluster"] = this.clusterModel.name;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", this.jsonCluster)),
            () => {
                this.updateData(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    updateData(myForm: angular.IFormController) {
        this.cluster.updateData(this.clusterModel).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal disimpan");
                }
                else {
                    this.refresh(myForm);
                    Alertify.success("Data berhasil disimpan");   
                }
            }
        ).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Koneksi ke server bermasalah");
                }
            }
        )
    }

    deleteCluster(data, myForm: angular.IFormController) {
        this.jsonCluster["Kode Cluster"] = data.aS400ClusterCode;
        this.jsonCluster["Cluster"] = data.name;

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", this.jsonCluster)),
            () => {
                this.deleteData(data.aS400ClusterCode, myForm);
                this.refresh(myForm);
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(id, myForm: angular.IFormController) {
        this.cluster.deleteData(id).then(
            response => {
                if (response.status != 200) {
                    Alertify.error("Data gagal dihapus");
                }
                else {
                    this.refresh(myForm);
                    Alertify.success("Data berhasil dihapus");
                }
            }
        ).catch(
            response => {
                if (response.status == "500") {
                    Alertify.error("Data gagal dihapus");
                }
            } 
        )
    }  
}

export class ClusterModel {
    aS400ClusterCode: string;
    name: string;
}

export class Cluster implements angular.IComponentOptions {
    controller = ClusterController;
    controllerAs = 'me';

    template = require('./Cluster.html');  
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               