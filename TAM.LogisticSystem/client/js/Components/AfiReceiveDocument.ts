import * as Angular from 'angular';
import * as Service from '../services';
import * as _ from 'lodash';
import * as Alertify from 'alertifyjs';
import * as mustache from 'mustache'
export class AfiReceiveDocumentController implements Angular.IController {
    $inject = ['AfiReceiveDocumentService'];

    afiReceiveDocumentService: Service.AfiReceiveDocumentService;
    currentDate: Date;
    rbStatus: any;
    dataReceiveDocument: any;
    dataReceiveDocumentAll: any = [];
    cbSelectAll: boolean;
    frameNumber: string;
    loader: boolean = true;
    showDataWhenLoading: boolean = true;
    //Nicky-New
    saveButtonDisabled: boolean = true;


    //pagination
    orderState: boolean = false;
    orderString: string;
    pageSizes: number[] = [5, 10, 15, 20,25];
    pageSize: number = 5;
    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;
    pageState: boolean = true;

    constructor(AfiReceiveDocumentService: Service.AfiReceiveDocumentService) {
        this.afiReceiveDocumentService = AfiReceiveDocumentService;
    }

    $onInit() {
        this.currentDate = this.getCurrentDate();
        this.rbStatus = 'Revisi';
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

    setPage(pageNo) {
        this.currentPage = pageNo;
    };
        
    getCurrentDate() :Date{
        var currentdate = new Date();
        return currentdate;
    }

    //Post to the server
    receiveDocument() {
        Alertify.confirm("Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update",null)),
            Q => {
            var RCU: ReceiveDocumentUpdate = new ReceiveDocumentUpdate();
            RCU.AfiApplicationList = this.dataReceiveDocumentAll;
            RCU.rbStatus = this.rbStatus;
                
            this.afiReceiveDocumentService.receiveDocument(RCU).then(
                response => {
                    Alertify.success(response.data);
                    this.dataReceiveDocumentAll = null;
                }
            ).catch(
                response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                    }
                }
                );   
        }, () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //Delete The FrameNumber from the temp list table
    deleteRow(data) {
        _.pull(this.dataReceiveDocumentAll, data);
        this.totalItems = this.dataReceiveDocumentAll.length;
        if (this.totalItems == 0) {
            this.saveButtonDisabled = true;
        }
    }

    //Add The FrameNumber to the temp list table, server side now....
    addFrameNumber() {

        this.showDataWhenLoading = false;
        this.loader = false;
        this.afiReceiveDocumentService.getAllDocument(this.frameNumber).then(
            response => {
                //if frame number found
                this.dataReceiveDocument = response.data;

                //Not allowed duplicate data in table temp
                let temp: any = _.find(this.dataReceiveDocumentAll, ['frameNumber', this.frameNumber]);
                if (temp) {
                    Alertify.error("Frame Number sudah diinput");
                    return;
                } else {
                    this.dataReceiveDocumentAll.push(this.dataReceiveDocument);
                    this.totalItems = this.dataReceiveDocumentAll.length;
                    Alertify.success("Berhasil menambah frame number");
                    if (this.totalItems > 0) {
                        this.saveButtonDisabled = false;
                    }
                } 
            }).catch(response => {
                if (response.status == "500") {
                    Alertify.error("koneksi ke server bermasalah");
                } else if (response.status == "400") {
                    Alertify.error(response.data);
                }
            }).finally(() => {
                this.showDataWhenLoading = true;
                this.loader = true;
                this.frameNumber = null;
            });
    }

    /**
     * untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
     * @param action insert,update,delete (salah satu) *case insensitive
     * @param json json data -> { label : value , label2 : value2 }
     */
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
    
}

export class ReceiveDocumentUpdate{
    rbStatus: string;
    AfiApplicationList: any;
}

export class AfiReceiveDocument implements Angular.IComponentOptions {
    controller = AfiReceiveDocumentController;
    controllerAs = 'me';
    template = require('./AfiReceiveDocument.html');
}