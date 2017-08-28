import * as Angular from 'angular';
import * as uib from 'angular-ui-bootstrap';
import * as Service from '../services';
import * as Lodash from 'lodash';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';

export class PolaRangkaianTahapAkhirHeader {
    routingDictionaryTailCode: string;
    description: string;
}

export class PolaRangkaianTahapAkhirDetail {
    routingDictionaryTailCode: string;
    processMasterCode: string;
    deliveryMethodCode: string;
    locationCode: string;
    ordering: number;
    locationName: string;
    routingMasterName: string;
    deliveryMethodName: string;
}

export class PolaRangkaianTahapAkhirController implements angular.IController {
    static $inject = ['PolaRangkaianTahapAkhirService', '$scope'];

    $scope: ng.IScope;
    polaRangkaianTahapAkhir: Service.PolaRangkaianTahapAkhirService;
    searchFilter = {};

    dataRangkaianTahapAkhir: any;
    regexCode: RegExp = /^[a-zA-Z0-9]+$/;
    regexName: RegExp = /^[a-zA-Z0-9\s\-.,&\'\/]+$/;
    regexNumeric: RegExp = /^[0-9]+$/;

    loading: boolean;

    //header
    routingDictionaryTailCode: string = "";
    description: string = "";
    lock: boolean = false;
    addHeaderState: boolean = true;

    //detail
    detailData: Array<PolaRangkaianTahapAkhirDetail> = [];
    addState: boolean = true;
    detailState: boolean = false;
    
    location: any;
    deliveryMethod: any;
    routingMaster: any;
    ordering: string;

    orderingBefore: number;

    //delete
    deleteRoutingDictionaryTailCode: string;
    deleteRoutingMasterCode: string;

    constructor(polaRangkaianTahapAkhirService: Service.PolaRangkaianTahapAkhirService, protected _$scope: ng.IScope) {
        this.polaRangkaianTahapAkhir = polaRangkaianTahapAkhirService;
        this.$scope = _$scope;
    }

    $onInit() {
        this.getDataRangkaianTahapAkhir();
    }

    // get all data from server
    getDataRangkaianTahapAkhir() {
        this.loading = true;
        this.dataRangkaianTahapAkhir = null;

        this.polaRangkaianTahapAkhir.getData().then(response => {
            this.dataRangkaianTahapAkhir = response.data;
            this.detailData = [];

            Lodash.forEach(this.dataRangkaianTahapAkhir.routingDictionaryTailDetail, (detail) => {
                this.detailData.push(detail);
            });

            this.totalItems = this.dataRangkaianTahapAkhir.routingDictionaryTail.length;
            this.setPage(this.currentPage);
        }).catch(response => {
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        }).finally(() => {
            this.loading = false;
        });
    }
    
    //reset form
    reset(Form: angular.IFormController, detailForm: angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();
        if (detailForm) {
            detailForm.$setPristine();
            detailForm.$setUntouched();
        }

        this.routingDictionaryTailCode = null;
        this.description = null;
        this.location = null;
        this.ordering = null;
        this.deliveryMethod = null;
        this.routingMaster = null;
        this.orderingBefore = null;

        this.addState = true;
        this.detailState = false;
        this.lock = false;
        this.addHeaderState = true;

        this.searchFilter = {};
    }

    // reset detail only
    resetDetail(detailForm: angular.IFormController) {
        detailForm.$setPristine();
        detailForm.$setUntouched();

        this.location = null;
        this.ordering = null;
        this.deliveryMethod = null;
        this.routingMaster = null;
        this.orderingBefore = null;

        this.addState = true;
    }

    //check header exist
    checkHeader() {
        let data: any = Lodash.find(this.dataRangkaianTahapAkhir.routingDictionaryTail, ['processTailTemplateCode', this.routingDictionaryTailCode.toUpperCase()]);
        this.description = (data) ? data.description : "";
        this.addHeaderState = (data) ? false : true;
    }

    //select header that user want to edit
    selectEditHeader(data) {
        this.routingDictionaryTailCode = data.processTailTemplateCode;
        this.description = data.description;
        this.addHeaderState = false;
    }

    //delete header from database
    selectDeleteHeader(data, Form: angular.IFormController, detailForm: angular.IFormController) {
        let headerJson = {
            'Kode Pola': data.processTailTemplateCode,
            'Keterangan': data.description
        };

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON('delete', headerJson)),
            () => {
                this.polaRangkaianTahapAkhir.DeleteDataHeader(data.processTailTemplateCode).then(response => {
                    Alertify.success("Data berhasil dihapus");
                }).catch(response => {
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                    } else {
                        Alertify.error(response.data);
                    }
                }).finally(() => {
                    this.getDataRangkaianTahapAkhir();
                    this.reset(Form, detailForm);
                });
            },
            () => { }
        );
    }

    //select detail that user choose want to edit
    selectEditDetail(data) {
        this.routingDictionaryTailCode = data.routingDictionaryTailCode;
        this.location = Lodash.find(this.dataRangkaianTahapAkhir.location, ['locationCode', data.locationCode]);
        this.deliveryMethod = Lodash.find(this.dataRangkaianTahapAkhir.deliveryMethod, ['deliveryMethodCode', data.deliveryMethodCode]);
        this.routingMaster = Lodash.find(this.dataRangkaianTahapAkhir.routingMaster, ['processMasterCode', data.processMasterCode]);

        this.ordering = data.ordering;

        this.orderingBefore = parseInt(data.ordering);
        this.addState = false;
    }

    //delete detail from array
    selectDeleteDetail(data) {
        let detailJson = {
            'Lokasi': data.locationName,
            'No. Urut': data.ordering,
            'Kode Rute': data.processMasterCode,
            'Nama Rute': data.routingMasterName,
            'Moda': data.deliveryMethodName
        };

        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON('delete', detailJson)),
            () => {
                let deleteDetail = Lodash.findIndex(this.detailData, {
                    'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                    'ordering': data.ordering
                });

                //temporary solution
                this.$scope.$apply(() => {
                    let check = Lodash.pullAt(this.detailData, deleteDetail);
                    if (check) {
                        let found = Lodash.filter(this.detailData, {
                            'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                        });

                        let max = found.length + 1;
                        let addIndex;
                        for (let i = data.ordering + 1; i < max + 1; i++) {
                            addIndex = Lodash.findIndex(this.detailData, {
                                'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                                'ordering': i
                            });
                            this.detailData[addIndex].ordering = i - 1;
                        }

                        this.location = null;
                        this.ordering = null;
                        this.deliveryMethod = null;
                        this.routingMaster = null;
                        this.addState = true;
                        Alertify.success("Data berhasil dihapus");
                    } else {
                        Alertify.error("Data gagal dihapus");
                    }
                });
            },
            () => { }
        );
    }

    //insert data header and detail to database
    addData(Form: angular.IFormController, detailForm: angular.IFormController) {
        if (this.headerValidation()) {
            let header: PolaRangkaianTahapAkhirHeader = new PolaRangkaianTahapAkhirHeader();
            header.routingDictionaryTailCode = this.routingDictionaryTailCode.toUpperCase();
            header.description = this.description;

            let jsonTahapAkhir = {}
            jsonTahapAkhir["header"] = header;

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/PolaRangkaianAlertify.html"), jsonTahapAkhir),
                () => {
                    let detail = Lodash.remove(this.detailData, ['routingDictionaryTailCode', this.routingDictionaryTailCode.toUpperCase()]);
                    this.polaRangkaianTahapAkhir.postData(header, detail).then(response => {
                        Alertify.success("Data berhasil disimpan");
                    }).catch(response => {
                        if (response.status == "500") {
                            Alertify.error("Koneksi ke server bermasalah");
                        } else {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.getDataRangkaianTahapAkhir();
                        this.reset(Form, detailForm);
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }
    }

    //add detail to array
    addDetail(detailForm: angular.IFormController) {
        if (this.detailValidation()) {
            let detail: PolaRangkaianTahapAkhirDetail = new PolaRangkaianTahapAkhirDetail();
            detail.routingDictionaryTailCode = this.routingDictionaryTailCode.toUpperCase();
            detail.processMasterCode = this.routingMaster.processMasterCode;
            detail.locationCode = this.location.locationCode;
            detail.ordering = parseInt(this.ordering);
            detail.locationName = this.location.name;
            detail.routingMasterName = this.routingMaster.name;

            detail.deliveryMethodCode = (this.deliveryMethod) ? this.deliveryMethod.deliveryMethodCode : null;
            detail.deliveryMethodName = (this.deliveryMethod) ? this.deliveryMethod.name : null;

            let found = Lodash.filter(this.detailData, {
                'routingDictionaryTailCode': detail.routingDictionaryTailCode
            });

            let max = found.length + 1;
            let addIndex;

            if (detail.ordering < 1) {
                Alertify.error('No. Urut tidak boleh lebih kecil daripada 1');
            } else if (detail.ordering > max) {
                Alertify.error('No. Urut tidak boleh lebih besar daripada ' + max);
            } else if (max >= detail.ordering) {
                if (max != detail.ordering) {
                    for (let i = max - 1; i >= detail.ordering; i--) {
                        addIndex = Lodash.findIndex(this.detailData, {
                            'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                            'ordering': i
                        });
                        this.detailData[addIndex].ordering = i + 1;
                    }
                }
                this.detailData.push(detail);
                Alertify.success("Data berhasil ditambah");
                this.resetDetail(detailForm);
            }
        }
    }

    /**
     * fungsi untuk mengubah data detail
     */
    updateDetail() {
        let check = this.detailValidation();
        //============================================= Validation
        if (parseInt(this.ordering) < 1 || parseInt(this.ordering) > Lodash.filter(this.detailData, ['routingDictionaryTailCode', this.routingDictionaryTailCode.toUpperCase()]).length) {
            Alertify.error("No. Urut harus di antara 1 dan " + Lodash.filter(this.detailData, ['routingDictionaryTailCode', this.routingDictionaryTailCode.toUpperCase()]).length);
            check = false;
        }
        //=============================================
        if (check) {
            let found = Lodash.findIndex(this.detailData, {
                'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                'ordering': this.orderingBefore
            });
            if (found < 0) {
                Alertify.error("Detail tidak ditemukan");
            } else {
                let i;
                if (this.orderingBefore != parseInt(this.ordering)) {
                    if (this.orderingBefore > parseInt(this.ordering)) {
                        for (i = (this.orderingBefore - 1); i >= parseInt(this.ordering); i--) {
                            let index = Lodash.findIndex(this.detailData, {
                                'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                                'ordering': i
                            });
                            this.detailData[index].ordering += 1;
                        }
                    } else {
                        for (i = this.orderingBefore + 1; i <= parseInt(this.ordering); i++) {
                            let index = Lodash.findIndex(this.detailData, {
                                'routingDictionaryTailCode': this.routingDictionaryTailCode.toUpperCase(),
                                'ordering': i
                            });
                            this.detailData[index].ordering -= 1;
                        }
                    }
                }

                this.detailData[found].ordering = parseInt(this.ordering);
                this.detailData[found].routingDictionaryTailCode = this.routingDictionaryTailCode.toUpperCase();
                this.detailData[found].processMasterCode = this.routingMaster.processMasterCode;
                this.detailData[found].locationCode = this.location.locationCode;
                this.detailData[found].locationName = this.location.name;
                this.detailData[found].routingMasterName = this.routingMaster.name;

                this.detailData[found].deliveryMethodCode = (this.deliveryMethod) ? this.deliveryMethod.deliveryMethodCode : null;
                this.detailData[found].deliveryMethodName = (this.deliveryMethod) ? this.deliveryMethod.name : null;

                this.orderingBefore = parseInt(this.ordering);
                Alertify.success("Data berhasil disimpan");
            }
        }
    }

    // untuk validasi header yg berisi code dan keterangan
    headerValidation() {
        //================================================= Kode Pola Validation
        if (!this.routingDictionaryTailCode) {
            return false;
        }
        if (this.routingDictionaryTailCode.length > 8) {
            return false;
        }
        if (!this.regexCode.test(this.routingDictionaryTailCode)) {
            return false;
        }
        //================================================= Keterangan Validation
        if (this.description.length > 255) {
            return false;
        }
        if (!this.regexName.test(this.description)) {
            return false;
        }
        if (!this.description) {
            return false;
        }
        //=================================================
        return true;
    }

    // untuk validasi detail yang berisi 3 field mandatory dan 1 field optional
    detailValidation() {
        //============================================= Validation
        if (this.location == null) {
            return false;
        }
        if (this.ordering == null) {
            return false;
        }
        if (!this.regexNumeric.test(this.ordering)) {
            return false;
        }
        /*if (this.deliveryMethod == null) {
            Alertify.error("Moda harus dipilih");
            check = false;
        }*/
        if (this.routingMaster == null) {
            return false;
        }
        //=============================================
        return true;
    }

    // untuk button lihat detail
    changeDetailState() {
        if (this.headerValidation()) {
            this.detailState = true;
            this.lock = true;
        }
    }

    //go to next page
    applyTo() {
        window.location.href = "/PolaRangkaianTahapAkhirPenerapan";
    }

    //paging
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    // sorting data by column name
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    // search data
    search(data) {
        this.totalItems = data.length;
        this.setPage(1);
    }

    // set page in html
    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

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


export class PolaRangkaianTahapAkhirComponent implements angular.IComponentOptions {
    controller = PolaRangkaianTahapAkhirController;
    controllerAs = 'me';

    template = require('./PolaRangkaianTahapAkhir.html');
}