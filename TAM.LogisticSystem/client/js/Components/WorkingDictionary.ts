import * as Service from '../services';
import * as Lodash from 'lodash';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';
import * as Moment from 'moment';
import * as angular from 'angular';

class WorkHour {
    WorkHourTemplate: WorkHourTemplate;
    WorkHourTemplatesDetail: Array<WorkHourTemplateDetail>;
}
class WorkHourTemplate {
    workHourTemplateCode: string;
    description: string;
}

class WorkHourTemplateDetail {
    workHourTemplateCode: string;
    workHourTemplateDetailId?: number;
    shiftCode: Shift;
    timeStart: string;
    timeFinish: string;
    isMonday: boolean;
    isTuesday: boolean;
    isWednesday: boolean;
    isThursday: boolean;
    isFriday: boolean;
    isSaturday: boolean;
    isSunday: boolean;
}
class Shift {
    shiftCode: string;
    description: string;
}

class Detail {
    ordering: number;
    shift: any;
    timeStart: string;
    timeFinish: string;
    isMonday: boolean;
    isTuesday: boolean;
    isWednesday: boolean;
    isThursday: boolean;
    isFriday: boolean;
    isSaturday: boolean;
    isSunday: boolean;
}


export class WorkingDictionaryController implements angular.IController {
    static $inject = ['WorkingDictionaryServices', '$scope'];

    $scope: ng.IScope;



    workHourTemplateService: Service.WorkingDictionaryServices;
    searchString = {};
    loading: boolean;
    //all databaseData
    headerData: any;
    detailData: any;
    shiftDropdown: any;

    //detail control
    detailState: boolean;
    lock: boolean;
    addState: boolean;
    addHeaderState: boolean;

    //form data
    tempDetail: Array<Detail>;

    //header
    workHourTemplateCode: any;
    description: any;

    //detail
    ordering: number;
    shift: string;
    timeStart: Date;
    timeFinish: Date;
    isMonday: boolean = false;
    isTuesday: boolean = false;
    isWednesday: boolean = false;
    isThursday: boolean = false;
    isFriday: boolean = false;
    isSaturday: boolean = false;
    isSunday: boolean = false;

    constructor(WorkingDictionaryServices: Service.WorkingDictionaryServices, protected _$scope: ng.IScope) {
        this.$scope = _$scope;
        this.workHourTemplateService = WorkingDictionaryServices;
    }

    reset(Form: angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();

        this.workHourTemplateCode = null;
        this.description = null;
        this.searchString = {};
        this.addState = true;
        this.addHeaderState = true;
        this.detailState = false;
        this.lock = false;
        if (this.tempDetail !== undefined) {
            this.resetDetail();
            this.timeFinish = undefined;
            this.timeStart = undefined;
        }

    }
    resetDetail() {
        this.ordering = this.tempDetail.length + 1;
        this.shift = null;
        let def = new Date();
        def.setHours(0, 0, 0);
        this.timeFinish = def;
        this.timeStart = def;
        this.isMonday = false;
        this.isTuesday = false;
        this.isWednesday = false;
        this.isThursday = false;
        this.isFriday = false;
        this.isSaturday = false;
        this.isSunday = false;
        this.addState = true;
    }
    refresh() {
        this.loading = true;
        this.workHourTemplateService.getAll().then(response => {
            this.loading = false;
            this.headerData = response.data["workHourTemplates"];
            this.detailData = response.data["workHourTemplatesDetail"];
            this.shiftDropdown = response.data["shiftList"];
            this.totalItems = this.headerData.length;
            this.setPage(this.currentPage);
            this.searchString = {};
        });
    }

    $onInit() {
        this.refresh();
        this.timeFinish = undefined;
        this.timeStart = undefined;
        this.addState = true;
        this.addHeaderState = true;
        this.detailState = false;
        this.lock = false;


    }
    checkDaySubmit() {
        let check: boolean = (this.isMonday || this.isTuesday || this.isWednesday || this.isThursday || this.isFriday || this.isSaturday || this.isSunday);
        if (check) {
            return 1;
        } else {
            return 0;
        }
    }
    //check header exist
    checkHeader() {
        let data: any = null;
        data = Lodash.find(this.headerData, ['workHourTemplateCode', this.workHourTemplateCode]);
        if (data !== undefined) {
            this.description = data.description;
            this.addHeaderState = false;
        } else {
            this.description = undefined;
            this.addHeaderState = true;
        }
    }

    //show form detail check
    changeDetailState() {
        let regexp = /^[a-zA-Z0-9]+$/;
        let regexp2 = /^[a-zA-Z0-9\s,.\/\'\-&]+$/;
        let check = true;
        //================================================= Kode Pola Validation
        if (!this.workHourTemplateCode) {
            Alertify.error("Kode Pola harus diisi");
            check = false;
        }
        if (this.workHourTemplateCode.length > 8) {
            Alertify.error("Kode Pola tidak boleh melebihi 8 karakter");
            check = false;
        }
        if (!regexp.test(this.workHourTemplateCode)) {
            Alertify.error("Kode Pola harus berformat alphanumeric");
            check = false;
        }
        //================================================= Keterangan Validation
        if (this.description.length > 128) {
            Alertify.error("Keterangan tidak boleh melebihi 128 karakter");
            check = false;
        }
        if (!regexp2.test(this.description)) {
            Alertify.error("Keterangan harus berformat alphanumeric");
            check = false;
        }
        if (!this.description) {
            Alertify.error("Keterangan harus diisi");
            check = false;
        }
        //=================================================
        if (check) {
            this.detailState = true;
            this.lock = true;
            this.getDetail();
        }
    }

    //filter detail data
    getDetail() {
        //init

        this.tempDetail = new Array<Detail>();
        if (this.timeStart !== undefined || this.timeFinish !== undefined) {
            this.resetDetail();
            this.timeFinish = undefined;
            this.timeStart = undefined;
        }
        let data: any = Lodash.filter(this.detailData, ['workHourTemplateCode', this.workHourTemplateCode]);
        if (data !== null) {
            let order: number = 1;
            Lodash.forEach(data, (detail) => {
                let temp: Detail = new Detail();
                temp.ordering = order;
                temp.timeStart = detail.timeStart;// hh:mm:ss format here
                temp.timeFinish = detail.timeFinish;
                temp.shift = detail.shiftCode;
                temp.isMonday = detail.isMonday;
                temp.isTuesday = detail.isTuesday;
                temp.isWednesday = detail.isWednesday;
                temp.isThursday = detail.isThursday;
                temp.isFriday = detail.isFriday;
                temp.isSaturday = detail.isSaturday;
                temp.isSunday = detail.isSunday;

                //push to temporary data
                this.tempDetail.push(temp);
                order++;
            });
            this.ordering = order;
        }
    }

    //select header that user want to edit
    selectEditHeader(data) {
        this.workHourTemplateCode = data.workHourTemplateCode;
        this.description = data.description;
        this.addHeaderState = false;
        this.lock = true;
        this.detailState = false;
    }

    //delete header from database
    selectDeleteHeader(data, Form: angular.IFormController) {
        let json = {};
        json["Kode Pola"] = data.workHourTemplateCode;
        json["Keterangan"] = data.description;
        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", json)),
            () => {
                this.workHourTemplateService.deleteData(data.workHourTemplateCode).then(response => {
                    Alertify.success("Data berhasil dihapus");
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                        return;
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //select detail that user choose want to edit
    selectEditDetail(data: Detail) {
        //change time to date format
        let tempDate: Date = new Date();
        tempDate.setHours(parseInt(data.timeStart.split(":")[0]));
        tempDate.setMinutes(parseInt(data.timeStart.split(":")[1]));
        tempDate.setSeconds(parseInt(data.timeStart.split(":")[2]));
        this.timeStart = tempDate;
        tempDate = new Date();
        tempDate.setHours(parseInt(data.timeFinish.split(":")[0]));
        tempDate.setMinutes(parseInt(data.timeFinish.split(":")[1]));
        tempDate.setSeconds(parseInt(data.timeFinish.split(":")[2]));
        this.timeFinish = tempDate;
        //
        this.ordering = data.ordering;
        this.shift = data.shift;
        this.isMonday = data.isMonday;
        this.isTuesday = data.isTuesday;
        this.isWednesday = data.isWednesday;
        this.isThursday = data.isThursday;
        this.isFriday = data.isFriday;
        this.isSaturday = data.isSaturday;
        this.isSunday = data.isSunday;
        this.addState = false;
    }

    //delete detail from array
    selectDeleteDetail(data: Detail) {
        Alertify.confirm(
            "Konfirmasi",
            "Apakah anda yakin data ke-" + data.ordering + " ingin dihapus?",
            () => {
                let deleteDetail = Lodash.findIndex(this.tempDetail, [
                    'ordering', data.ordering
                ]);

                //temporary solution
                this.$scope.$apply(() => {
                    let check = Lodash.pullAt(this.tempDetail, deleteDetail);
                    if (check) {
                        let max = this.tempDetail.length;
                        for (let i = data.ordering - 1; i < max; i++) {
                            this.tempDetail[i].ordering = i + 1;
                        }
                        this.resetDetail();
                        this.addState = true;
                        this.ordering = this.tempDetail.length + 1;
                        Alertify.success("Data berhasil dihapus");
                    } else {
                        Alertify.error("Gagal menghapus data detail");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //confirmation JSON for MasterAlertify.html
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

    //insert data header and detail to database
    addData(form: angular.IFormController) {
        //only if update header data do this
        if (!this.addHeaderState && !this.detailState) {
            this.getDetail();
        }
        //--------------------------------------------------------
        //init
        let dataPost: WorkHour = new WorkHour();
        dataPost.WorkHourTemplatesDetail = new Array<WorkHourTemplateDetail>();
        //handle header data
        dataPost.WorkHourTemplate = new WorkHourTemplate();
        dataPost.WorkHourTemplate.workHourTemplateCode = this.workHourTemplateCode;
        dataPost.WorkHourTemplate.description = this.description;
        //handle detail
        Lodash.forEach(this.tempDetail, (detail) => {
            let tempDetail: WorkHourTemplateDetail = new WorkHourTemplateDetail();
            tempDetail.workHourTemplateCode = this.workHourTemplateCode;
            tempDetail.shiftCode = new Shift();
            tempDetail.shiftCode.shiftCode = detail.shift.shiftCode;
            tempDetail.shiftCode.description = detail.shift.description;
            tempDetail.timeFinish = detail.timeFinish;
            tempDetail.timeStart = detail.timeStart;
            tempDetail.isMonday = detail.isMonday;
            tempDetail.isTuesday = detail.isTuesday;
            tempDetail.isWednesday = detail.isWednesday;
            tempDetail.isThursday = detail.isThursday;
            tempDetail.isFriday = detail.isFriday;
            tempDetail.isSaturday = detail.isSaturday;
            tempDetail.isSunday = detail.isSunday
            dataPost.WorkHourTemplatesDetail.push(tempDetail);
        });
        let json = {}
        json["Kode Pola"] = dataPost.WorkHourTemplate.workHourTemplateCode;
        json["Keterangan"] = dataPost.WorkHourTemplate.description;
        Alertify.confirm(
            "Konfirmasi",
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", json)),
            () => {
                this.workHourTemplateService.postDataWork(dataPost).then(response => {
                    Alertify.success("Data berhasil disimpan");
                    this.reset(form);
                    this.refresh();
                }).catch(response => {
                    if (response.status == "400") {
                        Alertify.error(response.data);
                        return;
                    }
                    if (response.status == "500") {
                        Alertify.error("Koneksi ke server bermasalah");
                        return;
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }
    cekSinggungan(detail: Detail) {
        if (Moment(this.timeStart).format('HH:mm') <= Moment(detail.timeStart).format('HH:mm')
            && Moment(this.timeFinish).format('HH:mm') > Moment(detail.timeStart).format('HH:mm')) {
            Alertify.error("Batas waktu diinput harus diluar dari batas waktu sebelumnya");
            return false;
        } else if (Moment(this.timeStart).format('HH:mm') >= Moment(detail.timeStart).format('HH:mm')
            && Moment(this.timeStart).format('HH:mm') < Moment(detail.timeFinish).format('HH:mm')) {
            Alertify.error("Batas waktu diinput harus diluar dari batas waktu sebelumnya");
            return false;
        }
        return true;
    }

    //add detail to array
    addDetail() {
        let check = true;
        //============================================= Validation
        if (Moment(this.timeStart).format('HH:mm') >= Moment(this.timeFinish).format('HH:mm')) {
            Alertify.error("Waktu mulai harus lebih kecil dari waktu selesai");
            check = false;
        }
        //cek waktu bersinggungan yg makin panjang
        Lodash.forEach(this.tempDetail, (detail) => {
            if (check) {
                if (this.isMonday && detail.isMonday) {
                    check = this.cekSinggungan(detail);
                }
                if (this.isTuesday && detail.isTuesday) {
                    check = this.cekSinggungan(detail);
                }
                if (this.isWednesday && detail.isWednesday) {
                    check = this.cekSinggungan(detail);
                }
                if (this.isThursday && detail.isThursday) {
                    check = this.cekSinggungan(detail);
                }
                if (this.isFriday && detail.isFriday) {
                    check = this.cekSinggungan(detail);
                }
                if (this.isSaturday && detail.isSaturday) {
                    check = this.cekSinggungan(detail);
                }
                if (this.isSunday && detail.isSunday) {
                    check = this.cekSinggungan(detail);
                }
            }

        });
        //=============================================
        if (check) {
            let detail: Detail = new Detail();
            detail.ordering = this.ordering;
            detail.shift = this.shift;
            detail.timeFinish = Moment(this.timeFinish).format("HH:mm:ss");
            detail.timeStart = Moment(this.timeStart).format("HH:mm:ss");
            detail.isMonday = this.isMonday;
            detail.isTuesday = this.isTuesday;
            detail.isWednesday = this.isWednesday;
            detail.isThursday = this.isThursday;
            detail.isFriday = this.isFriday;
            detail.isSaturday = this.isSaturday;
            detail.isSunday = this.isSunday;
            this.tempDetail.push(detail);
            this.ordering = this.tempDetail.length + 1;
            Alertify.success("Data berhasil ditambah");
            this.resetDetail();
            this.timeStart = null;
            this.timeFinish = null;
        }
    }

    /**
     * fungsi untuk mengubah data detail
     */
    updateDetail() {
        let check = true;
        //============================================= Validation
        if (Moment(this.timeStart).format('HH:mm') >= Moment(this.timeFinish).format('HH:mm')) {
            Alertify.error("Waktu mulai harus lebih kecil dari waktu selesai");
            check = false;
        }
        //cek waktu bersinggungan yg makin panjang
        Lodash.forEach(this.tempDetail, (detail) => {
            if (check) {
                if (detail.ordering !== this.ordering) {
                    if (this.isMonday && detail.isMonday) {
                        check = this.cekSinggungan(detail);
                    }
                    if (this.isTuesday && detail.isTuesday) {
                        check = this.cekSinggungan(detail);
                    }
                    if (this.isWednesday && detail.isWednesday) {
                        check = this.cekSinggungan(detail);
                    }
                    if (this.isThursday && detail.isThursday) {
                        check = this.cekSinggungan(detail);
                    }
                    if (this.isFriday && detail.isFriday) {
                        check = this.cekSinggungan(detail);
                    }
                    if (this.isSaturday && detail.isSaturday) {
                        check = this.cekSinggungan(detail);
                    }
                    if (this.isSunday && detail.isSunday) {
                        check = this.cekSinggungan(detail);
                    }
                }
            }

        });
        //=============================================
        if (check) {
            this.tempDetail[this.ordering - 1].ordering = this.ordering;
            this.tempDetail[this.ordering - 1].shift = this.shift;
            this.tempDetail[this.ordering - 1].timeFinish = Moment(this.timeFinish).format("HH:mm:ss");;
            this.tempDetail[this.ordering - 1].timeStart = Moment(this.timeStart).format("HH:mm:ss");;
            this.tempDetail[this.ordering - 1].isMonday = this.isMonday;
            this.tempDetail[this.ordering - 1].isTuesday = this.isTuesday;
            this.tempDetail[this.ordering - 1].isWednesday = this.isWednesday;
            this.tempDetail[this.ordering - 1].isThursday = this.isThursday;
            this.tempDetail[this.ordering - 1].isFriday = this.isFriday;
            this.tempDetail[this.ordering - 1].isSaturday = this.isSaturday;
            this.tempDetail[this.ordering - 1].isSunday = this.isSunday;
            this.addState = true;
            this.resetDetail();
            this.timeStart = null;
            this.timeFinish = null;
            Alertify.success("Data berhasil disimpan");
        }
    }

    //paging
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    pageSizeDetail: number = this.pageSizes[0];
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

    gotoGenerate() {
        window.location.href = '/generatejamwork';
    }

}


export class WorkingDictionaryComponent implements angular.IComponentOptions {
    controller = WorkingDictionaryController;
    controllerAs = 'me';

    template = require('./WorkingDictionary.html');

}