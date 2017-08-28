import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as _ from 'lodash';
import * as moment from 'moment';
import * as angular from 'angular';


class DeliveryUnitLoadingController implements angular.IController {

    static $inject = ['deliveryUnitLoadingService', '$rootScope'];


    constructor(deliveryUnitLoadingService: service.DeliveryUnitLoadingService, $rootScope: angular.IRootScopeService) {
        this.deliveryUnitLoadingService = deliveryUnitLoadingService;
        this.$rootScope = $rootScope;
    }

    //service
    deliveryUnitLoadingService: service.DeliveryUnitLoadingService;
    $rootScope: angular.IRootScopeService;

    //property
    deliveryUnitLoadingModels: service.UnitLoadingModel[];
    deliveryUnitLoadingModel: service.UnitLoadingModel;
    frameNumberInputModels: service.InputFrameNumber[];
    frameNumberInputSelected: Array<service.InputFrameNumber> = [];
    frameNumberUpdateModel = new service.UpdateFrameNumber();
    searchInput: service.SearchDataPreview;
    frameNumberInput: string[] = [];
    frameNumberSelected: string[] = [];
    vehicleIdSelected: number[] = [];
    search: string;
    frameNumberInputField: string;
    frameNumberTextArea: string;
    totalFrameNumber: number;
    flagForm: boolean;
    flag: boolean;
    totalFrameNumberPreBooked: number;
    errorMessage: string;
    errorMessageVoyage: string;
    loader: boolean;
    totalFrameNumberAssign: number;
    errorListFrameNo: string[] = [];
    regexCode: RegExp = /^[A-Za-z0-9]+$/;
    btnDisable: boolean;
    error: boolean;

    // Paging Sorting Searching
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

    searchTable(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };


    //untuk mengubah data yang akan di CRUD ke dalam template json untuk alertify
    //@param action insert,update,delete(salah satu) *case insensitive
    //@param json json data -> { label: value, label2: value2 }

    // template alertify confirmation
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


    //function

    //broadcast component to detail
    broadcast() {
        this.$rootScope.$broadcast('voyageNumber', this.search);
        this.flag = false;
    }


    //check Voyage Number tidak boleh kosong
    checkEmptyVoyageNumber() {
        if (this.search == null || this.deliveryUnitLoadingModel == null) {
            return false;
        } else {
            return true;
        }
    }


    //update Vehicle in Voyage To Loaded status
    updateDataLoaded(Form: angular.IFormController) {
        if (this.checkEmptyVoyageNumber() == false && (this.totalFrameNumber < 1 || this.totalFrameNumber == null)) {
            this.errorMessageVoyage = "Voyage No harus diisi";
            this.errorMessage = "Frame No harus diisi";
        } else if (this.checkEmptyVoyageNumber() == false) {
            this.errorMessageVoyage = "Voyage No harus diisi";
        } else if (this.totalFrameNumber < 1 || this.totalFrameNumber == null) {
            this.errorMessage = "Frame No harus diisi";
        } else {
            alertify.confirm(
                "Konfirmasi",
                mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", null)),
                () => {
                    this.btnDisable = false;
                    this.deliveryUnitLoadingService.updateDataLoaded(this.frameNumberUpdateModel).then(result => {
                        this.deliveryUnitLoadingModels = result.data as service.UnitLoadingModel[];
                        alertify.success("Data berhasil disimpan")
                        this.clearAllField(Form);
                    }).catch(result => {
                    });
                },
                () => { }
            ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
        }

    }


    //on broadcast
    onBroadcast() {
        this.$rootScope.$on('flag', (event, data) => {
            this.clearingField();
            this.search = data;
            this.flag = true;
            this.searchUnitLoadingData(data);
        })

    }

    //clearing selected field
    clearingSelectedField() {
        this.deliveryUnitLoadingModel = new service.UnitLoadingModel();
        this.totalFrameNumber = null;
        this.frameNumberSelected = new Array<string>();
        this.frameNumberInput = new Array<string>();
        this.frameNumberInputSelected = new Array<service.InputFrameNumber>();
        this.totalFrameNumberPreBooked = null;
        this.frameNumberInputModels = new Array<service.InputFrameNumber>();
        this.totalFrameNumberAssign = null;
        this.errorListFrameNo = new Array<string>();
        this.searchInput = new service.SearchDataPreview();
        this.btnDisable = false;
    }

    //clearing field
    clearingField() {
        this.search = null;
        this.frameNumberTextArea = null;
        this.frameNumberInputField = null;
        this.clearingSelectedField();
    }

    //set clear all field
    clearAllField(form: angular.IFormController) {
        this.clearForm(form);
        this.clearingField();
        this.showText();
        this.errorMessage = null;
        this.errorMessageVoyage = null;
        this.flagForm = true;
    }

    //Convert Date To string
    convertDateToString() {
        angular.forEach(this.frameNumberInputModels, data => {
            if (data.requestedDeliveryTime != null) {
                data.dateTemp = moment(data.requestedDeliveryTime).format('DD-MMM-YYYY ');
            }
            if (data.estimatedPDCIn != null) {
                data.dateTempEPDC = moment(data.estimatedPDCIn).format('DD-MMM-YYYY ');
            }
        })
    }

    //get Frame Number
    getFrameNumbers(voyageNumber: string) {
        this.deliveryUnitLoadingService.getFrameNumbers(voyageNumber).then(result => {
            this.frameNumberInputModels = result.data;
            this.order('dateTempEPDC');
            this.convertDateToString();
            this.loader = false;
        }).catch(result => {
        });
    }


    //show & Hide Multiple and Single Input Frame Number
    showText() {
        this.flagForm = !this.flagForm;
        this.frameNumberTextArea = null;
        this.frameNumberInputField = null;
        this.errorListFrameNo = null;
        this.errorListFrameNo = new Array<string>();
        this.errorMessage = null;
    }


    //multiple Input Frame Number
    inputFrameNumberTextArea(frameNumber: string, form: angular.IFormController) {
        this.errorListFrameNo = new Array<string>();
        this.errorMessage = "";
        let frameNumbersTemp: string[] = [];
        if (frameNumber == null || frameNumber == "") {
            this.errorMessage = "Frame No. Harus diisi";
        } else {
            frameNumbersTemp = frameNumber.split("\n");
            angular.forEach(frameNumbersTemp, data => {
                if (this.checkSymbolInput(data) == false) {
                    this.errorListFrameNo.push(data + ". harus berformat alphanumeric");
                } else if (data.length < 17) {
                    this.errorListFrameNo.push(data + ". tidak boleh kurang dari 17 karakter");
                } else if (data.length > 17) {
                    this.errorListFrameNo.push(data + ". tidak boleh lebih dari 17 karakter");
                } else {
                    this.frameNumberInput.push(data);
                    this.searchFrameNumber(this.frameNumberInput);
                    this.frameNumberInput = new Array<string>();
                }
            })
        }
    }


    //check input tidak boleh symbol
    checkSymbolInput(frameNumber: string) {
        if (!frameNumber.match(this.regexCode)) {
            return false;
        } else {
            return true;
        }
    }

    //single Input Frame Number
    inputFrameNumber(frameNumber: string, form: angular.IFormController) {
        this.errorMessage = "";
        this.errorListFrameNo = new Array<string>();
        if (frameNumber.length < 1 || frameNumber == null) {
            this.errorMessage = "Frame No. harus diisi";
        } else if (this.checkSymbolInput(frameNumber) == false) {
            this.errorMessage = "Frame No. harus berformat alphanumeric";
        } else if (frameNumber.length < 17) {
            this.errorMessage = "Frame No. tidak boleh kurang dari 17 karakter";
        } else if (frameNumber.length > 17) {
            this.errorMessage = "Frame No. tidak boleh lebih dari 17 karakter";
        } else {
            this.frameNumberInput.push(frameNumber);
            this.searchFrameNumber(this.frameNumberInput);
            this.frameNumberInput = new Array<string>();
        }

    }

    //set Pristine and Untouched Form
    clearForm(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
    }



    //search UnitLoading Data Clientside
    searchUnitLoadingData(voyageNumber: string) {
        if (this.search == null || this.search.length < 1) {
            this.errorMessageVoyage = "";
            this.clearingSelectedField();
        } else if (this.checkSymbolInput(this.search) == false) {
            this.errorMessageVoyage = "";
            this.clearingSelectedField();
        } else if (this.search.length > 16) {
            this.errorMessageVoyage = "";
            this.clearingSelectedField();
        } else {
            this.frameNumberUpdateModel = new service.UpdateFrameNumber();
            this.deliveryUnitLoadingModel = _.find(this.deliveryUnitLoadingModels, ['voyageNumber', voyageNumber.toUpperCase()]);
            if (this.deliveryUnitLoadingModel != null) {
                this.frameNumberUpdateModel.voyageNumber = this.deliveryUnitLoadingModel.voyageNumber;
                this.totalFrameNumberAssign = this.deliveryUnitLoadingModel.totalUnitAssign;
                if (this.totalFrameNumberAssign < 1) {
                    this.btnDisable = false;
                }
                this.totalFrameNumberPreBooked = 0;
                this.totalFrameNumberPreBooked = this.totalFrameNumberAssign + this.deliveryUnitLoadingModel.totalUnitPreBookedInPorted + this.deliveryUnitLoadingModel.totalUnitPreBookedNotPorted;
                this.errorMessageVoyage = null;
                this.loader = true;
                this.getFrameNumbers(this.deliveryUnitLoadingModel.voyageNumber);
            } else {
                this.errorMessageVoyage = "Voyage No. tidak ditemukan";
                this.clearingSelectedField();
            }
        }
    }


    //search Frame Numbers
    searchFrameNumber(frameNumberInput: string[]) {
        let index: number = 0;
        angular.forEach(frameNumberInput, data => {
            index = _.findIndex(this.frameNumberInputModels, ['frameNumber', data]);
            if (index < 0) {
                this.errorListFrameNo.push(data + ". tidak ditemukan");
            } else {
                if (this.frameNumberExistsCheck(data, this.frameNumberSelected) == true) {
                    this.errorListFrameNo.push(data + " sudah di assign");
                } else {
                    this.frameNumberSelected.push(data);
                    this.vehicleIdSelected.push(this.frameNumberInputModels[index].vehicleId);
                    this.frameNumberInputSelected.push(this.frameNumberInputModels[index]);
                    alertify.success("Frame No : " + data.toString() + " berhasil di scan");
                    this.totalFrameNumber = this.frameNumberSelected.length;
                    this.totalFrameNumberPreBooked++;
                    this.totalFrameNumberAssign -= 1;
                    this.totalFrameNumberPreBooked -= 1;
                    if (this.totalFrameNumberAssign == 0) {
                        this.btnDisable = true;
                    }
                    this.frameNumberInputField = null;
                    this.errorMessage = null;
                }
            }
        })
        this.frameNumberUpdateModel.vehicleId = this.vehicleIdSelected;
    }

    //validation if frameNumber already exists
    frameNumberExistsCheck(frameNumber: string, frameNumberSelected: string[]) {
        if (_.indexOf(frameNumberSelected, frameNumber) < 0) {
            return false;
        } else {
            return true;
        }
    }
    //get UnitLoading Data list
    getUnitLoadingData() {
        this.deliveryUnitLoadingService.getUnitLoadingModels().then(result => {
            this.deliveryUnitLoadingModels = result.data as service.UnitLoadingModel[];
        }).catch(result => {
        });
    }

    $onInit() {
        this.btnDisable = false;
        this.flag = true;
        this.onBroadcast();
        this.getUnitLoadingData();
        this.flagForm = true;
    }
}

let DeliveryUnitLoadingComponent = {
    controller: DeliveryUnitLoadingController,
    controllerAs: 'me',
    template: require('./DeliveryUnitLoadingComponent.html')
}

export { DeliveryUnitLoadingComponent }