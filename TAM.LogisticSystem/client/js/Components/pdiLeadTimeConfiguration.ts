import * as angular from "angular";
import * as service from "../services/PDILeadTimeConfigurationService";
import * as alertify from "alertifyjs";
import * as mustache from 'mustache';
import * as _ from 'lodash';

export class PDILeadTimeController implements angular.IController {
    static $inject = ["pdiLeadTimeConfigurationService", "$rootScope"];

    constructor(pdiLeadTimeConfigurationLeadTimeService: service.PDILeadTimeConfigurationService, $rootscope: angular.IRootScopeService) {
        this.pdiLeadTimeConfigurationService = pdiLeadTimeConfigurationLeadTimeService;
        this.$rootscope = $rootscope;
        this.isUpdate = false;
    }

    katashikiSuffixCheckboxes: service.PDILeadTimeConfigurationCarModelViewModel[];

    locations: service.PDILeadTimeConfigurationLocationViewModel[];

    carModels: service.PDILeadTimeConfigurationCarModelViewModel[];
    carSeries: service.PDILeadTimeConfigurationCarSeriesViewModel[];
    carTypes: service.PDILeadTimeConfigurationCarTypeViewModel[];
    carTypeKatashikis: service.PDILeadTimeConfigurationCarTypeKatashikiViewModel[];
    carTypeSuffixes: service.PDILeadTimeConfigurationCarTypeSuffixViewModel[];
    katashikis: service.PDILeadTimeConfigurationKatashikiModel[];

    pdiLeadTimeConfigurations: service.PDILeadTimeConfigurationViewModel[];
    submitForm: service.SubmitPDILeadTimeConfigurationFormViewModel;

    isKatashikiSuffixChecked: boolean = false;
    isUpdate: boolean;

    pdiLeadTimeConfigurationService: service.PDILeadTimeConfigurationService;

    selectAllCheckbox: boolean = false;

    taktTimesForm = new service.PDILeadTimeConfigurationTaktTimesViewModel();
    totalTaktTimes: number;

    katashikiSuffixHeader: string;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string;
    totalItems: number;

    jsonConfirmData = {};
    $rootscope: angular.IRootScopeService;
    pageState: boolean = true;

    searchTable = {};

    expandAll: boolean = true;

    $onInit() {
        this.submitForm = new service.SubmitPDILeadTimeConfigurationFormViewModel;
        this.retrievePDILeadTimeConfigurationData();
        this.$rootscope.$on('Kembali', (event) => {
            this.pageState = true;
            this.submitForm = new service.SubmitPDILeadTimeConfigurationFormViewModel;
            this.retrievePDILeadTimeConfigurationData();
        });
        this.taktTimesForm.hour = 0;
        this.taktTimesForm.minute = 0;
        this.taktTimesForm.second = 0;
        this.submitForm.post = 0;
    }

    //for download
    download(result: any) {
        let tempData = [];
        angular.forEach(result, (data) => {
            tempData.push(data.pdiKatsuDictionaryDetailId);
        });
        this.pageState = false;
        let info: any = {};
        info.master = "PDIKatsuDictionary";
        info.title = "PDI - Lead Time Configuration";
        info.tipe = "3";
        this.$rootscope.$emit("UploadDownload", tempData, info);
    }

    //for upload
    upload() {
        let info: any = {};
        info.master = "PDIKatsuDictionary";
        info.title = "PDI - Lead Time Configuration";
        info.tipe = "2";
        this.pageState = false;
        this.$rootscope.$emit("UploadDownload", null, info);
    }

    // Chain the AJAX promises to obtain the necessary data for creating the Katashiki-Suffix checkboxes
    retrievePDILeadTimeConfigurationData() {
        this.pdiLeadTimeConfigurationService.getPDILeadTimeConfigurations().then(response => {
            this.pdiLeadTimeConfigurations = response.data.pdiLeadTimeConfigurationViewModels as service.PDILeadTimeConfigurationViewModel[];
            this.carModels = response.data.pdiLeadTimeConfigurationCarModels as service.PDILeadTimeConfigurationCarModelViewModel[];
            this.carSeries = response.data.pdiLeadTimeConfigurationCarSeries as service.PDILeadTimeConfigurationCarSeriesViewModel[];
            this.carTypes = response.data.pdiLeadTimeConfigurationCarTypes as service.PDILeadTimeConfigurationCarTypeViewModel[];
            this.katashikis = response.data.pdiLeadTimeConfigurationKatashikis as service.PDILeadTimeConfigurationKatashikiModel[];
            this.locations = response.data.pdiLeadTimeConfigurationLocations as service.PDILeadTimeConfigurationLocationViewModel[];
            this.generateLocationString();
            this.generateTaktTimeString();
            this.setCarTypeKatashikis();
            this.setCarTypeSuffixes();
            this.combineKatashikiSuffix();
        }).catch(response => {
            if (response.status === 500) {
                alertify.error('Koneksi ke server bermasalah')
            }
        });
    }

    // Get the Car Type Katashikis for creating the Katashiki-Suffix checkboxes
    setCarTypeKatashikis() {
        this.carTypeKatashikis = new Array<service.PDILeadTimeConfigurationCarTypeKatashikiViewModel>();
        angular.forEach(this.katashikis, (katashikiData) => {
            let carTypeKatashiki = new service.PDILeadTimeConfigurationCarTypeKatashikiViewModel();
            carTypeKatashiki.katashiki = katashikiData.katashiki;
            carTypeKatashiki.carSeriesCode = katashikiData.carSeriesCode;
            carTypeKatashiki.isChecked = false;
            carTypeKatashiki.isExpand = false;
            this.carTypeKatashikis.push(carTypeKatashiki);
        });
    }

    // Get the Car Type Suffixes for creating the Katashiki-Suffix checkboxes
    setCarTypeSuffixes() {
        this.carTypeSuffixes = new Array<service.PDILeadTimeConfigurationCarTypeSuffixViewModel>();
        angular.forEach(this.carTypes, (carType) => {
            let carTypeSuffix = new service.PDILeadTimeConfigurationCarTypeSuffixViewModel();
            carTypeSuffix.katashiki = carType.katashiki;
            carTypeSuffix.suffix = carType.suffix;
            carTypeSuffix.isChecked = false;
            this.carTypeSuffixes.push(carTypeSuffix);
        });
    }

    // Combine the  for creating the Katashiki-Suffix checkboxes
    combineKatashikiSuffix() {
        this.katashikiSuffixCheckboxes = new Array<service.PDILeadTimeConfigurationCarModelViewModel>();
        angular.forEach(this.carModels, (carModel) => {
            let carSeriesList = new Array<service.PDILeadTimeConfigurationCarSeriesViewModel>();
            carModel.carSeries = this.carSeries.filter((carSeries) =>
                carModel.carModelCode === carSeries.carModelCode
            );
            carModel.isExpand = false;
            angular.forEach(carModel.carSeries, (carSeries) => {
                let carTypeKatashikis = new Array<service.PDILeadTimeConfigurationCarTypeKatashikiViewModel>();
                carSeries.carTypeKatashikis = this.carTypeKatashikis.filter((carTypeKatashiki) =>
                    carSeries.carSeriesCode === carTypeKatashiki.carSeriesCode
                );
                carSeries.isExpand = false;
                angular.forEach(carSeries.carTypeKatashikis, (carTypeKatashiki) => {
                    let carTypeSuffixes = new Array<service.PDILeadTimeConfigurationCarTypeSuffixViewModel>();
                    carTypeKatashiki.suffixes = this.carTypeSuffixes.filter((carSuffix) =>
                        carTypeKatashiki.katashiki === carSuffix.katashiki
                    );
                    carTypeKatashikis.push(carTypeKatashiki);
                });
                carSeriesList.push(carSeries);
            });
            this.katashikiSuffixCheckboxes.push(carModel);
        });
    }

    // Set the Select All checkbox and set all checkboxes linked to it
    setAllCheckboxes() {
        if (this.expandAll === false) {
            this.expandAll = true;
        }
        angular.forEach(this.katashikiSuffixCheckboxes, (carModel) => {
            carModel.isChecked = this.selectAllCheckbox;
            carModel.isExpand = this.selectAllCheckbox;
            this.setCarSeriesCheckboxes(carModel);
        });
    }

    // Set the Car Series checkboxes
    setCarSeriesCheckboxes(carModel: service.PDILeadTimeConfigurationCarModelViewModel) {
        carModel.isExpand = carModel.isChecked;
        angular.forEach(carModel.carSeries, (carSeries) => {
            carSeries.isChecked = carModel.isChecked;
            carSeries.isExpand = carSeries.isChecked;
            this.setCarTypeKatashikiCheckboxes(carSeries, carModel);
        });
    }

    // Set the Car Type Katashiki checkboxes
    setCarTypeKatashikiCheckboxes(carSeries: service.PDILeadTimeConfigurationCarSeriesViewModel, carModel: service.PDILeadTimeConfigurationCarModelViewModel) {
        carSeries.isExpand = carSeries.isChecked;
        angular.forEach(carSeries.carTypeKatashikis, (carTypeKatashiki) => {
            carTypeKatashiki.isChecked = carSeries.isChecked;
            carTypeKatashiki.isExpand = carTypeKatashiki.isChecked;
            this.setCarTypeSuffixCheckboxes(carTypeKatashiki, carSeries, carModel);
        });
        if (carSeries.isChecked === true) {
            carModel.isChecked = true;
        }
        if (carSeries.isChecked === false) {
            let unchecked = 0;
            angular.forEach(carModel.carSeries, (carSeries) => {
                if (carSeries.isChecked === false) {
                    unchecked += 1;
                }
            });
            if (carModel.carSeries.length === unchecked) {
                carModel.isChecked = false;
                carModel.isExpand = false;
            }
        }
    }

    // Set the Car Type Suffix checkboxes
    setCarTypeSuffixCheckboxes(carTypeKatashiki: service.PDILeadTimeConfigurationCarTypeKatashikiViewModel, carSeries: service.PDILeadTimeConfigurationCarSeriesViewModel, carModel: service.PDILeadTimeConfigurationCarModelViewModel) {
        carTypeKatashiki.isExpand = carTypeKatashiki.isChecked;
        angular.forEach(carTypeKatashiki.suffixes, (carTypeSuffix) => {
            carTypeSuffix.isChecked = carTypeKatashiki.isChecked;
        });
        if (carTypeKatashiki.isChecked === true) {
            carSeries.isChecked = true;
            carModel.isChecked = true;
        }
        if (carTypeKatashiki.isChecked === false) {
            let unchecked = 0;
            angular.forEach(carSeries.carTypeKatashikis, (carTypeKatashiki) => {
                if (carTypeKatashiki.isChecked === false) {
                    unchecked += 1;
                }
            });
            if (carSeries.carTypeKatashikis.length === unchecked) {
                carSeries.isChecked = false;
                carSeries.isExpand = false;
                unchecked = 0;
                angular.forEach(carModel.carSeries, (carSeries) => {
                    if (carSeries.isChecked === false) {
                        unchecked += 1;
                    }
                });
                if (carModel.carSeries.length === unchecked) {
                    carModel.isChecked = false;
                    carModel.isExpand = false;
                }
            }
            this.selectAllCheckbox = false;
        }
        let katashikiSuffixPairs = this.getCheckedKatashikisSuffixes();
        if (katashikiSuffixPairs.length === 0) {
            this.isKatashikiSuffixChecked = false;
        }
        else {
            this.isKatashikiSuffixChecked = true;
        }
    }

    //check if suffix check or uncheck
    checkSelectedSuffix(carTypeSuffix: service.PDILeadTimeConfigurationCarTypeSuffixViewModel, carTypeKatashiki: service.PDILeadTimeConfigurationCarTypeKatashikiViewModel, carSeries: service.PDILeadTimeConfigurationCarSeriesViewModel, carModel: service.PDILeadTimeConfigurationCarModelViewModel) {
        if (carTypeSuffix.isChecked === true) {
            carTypeKatashiki.isChecked = true;
            carSeries.isChecked = true;
            carModel.isChecked = true;
        }
        if (carTypeSuffix.isChecked === false) {
            this.selectAllCheckbox = false;
            let unchecked = 0;
            angular.forEach(carTypeKatashiki.suffixes, (suffix) => {
                if (suffix.isChecked === false) {
                    unchecked += 1;
                }
            });
            if (carTypeKatashiki.suffixes.length === unchecked) {

                carTypeKatashiki.isChecked = false;
                carTypeKatashiki.isExpand = false;
                unchecked = 0;
                angular.forEach(carSeries.carTypeKatashikis, (carTypeKatashiki) => {
                    if (carTypeKatashiki.isChecked === false) {
                        unchecked += 1;
                    }
                });
                if (carSeries.carTypeKatashikis.length === unchecked) {
                    carSeries.isChecked = false;
                    carSeries.isExpand = false;
                    unchecked = 0;
                    angular.forEach(carModel.carSeries, (carSeries) => {
                        if (carSeries.isChecked === false) {
                            unchecked += 1;
                        }
                    });
                    if (carModel.carSeries.length === unchecked) {
                        carModel.isChecked = false;
                        carModel.isExpand = false;
                    }
                }
            }
            this.selectAllCheckbox = false;
        }
        let katashikiSuffixPairs = this.getCheckedKatashikisSuffixes();
        if (katashikiSuffixPairs.length === 0) {
            this.isKatashikiSuffixChecked = false;
        }
        else {
            this.isKatashikiSuffixChecked = true;
        }
    }

    // Get the list of checked Katashiki-Suffix
    getCheckedKatashikisSuffixes() {
        let katashikiSuffixPairs = new Array<service.PDILeadTimeConfigurationKatashikiSuffixPairViewModel>();
        angular.forEach(this.katashikiSuffixCheckboxes, (carModel) => {
            angular.forEach(carModel.carSeries, (carSeries) => {
                angular.forEach(carSeries.carTypeKatashikis, (carTypeKatashiki) => {
                    let checkedCarTypeSuffix = carTypeKatashiki.suffixes.filter((carTypeSuffix) =>
                        carTypeSuffix.isChecked === true
                    );
                    angular.forEach(checkedCarTypeSuffix, (carTypeSuffix) => {
                        let katashikiSuffixPair = new service.PDILeadTimeConfigurationKatashikiSuffixPairViewModel();
                        katashikiSuffixPair.katashiki = carTypeKatashiki.katashiki;
                        katashikiSuffixPair.suffix = carTypeSuffix.suffix
                        katashikiSuffixPairs.push(katashikiSuffixPair);
                    });
                });
            });
        });
        return katashikiSuffixPairs;
    }

    // Check if Takt hour, minute, and second are null/undefined and set them to 0 if they are null/undefined
    checkTaktTimes() {
        this.taktTimesForm.hour = this.taktTimesForm.hour === undefined ? 0 : this.taktTimesForm.hour;
        this.taktTimesForm.minute = this.taktTimesForm.minute === undefined ? 0 : this.taktTimesForm.minute;
        this.taktTimesForm.second = this.taktTimesForm.second === undefined ? 0 : this.taktTimesForm.second;
    }

    // Calculate the total of Takt Times
    calculateTotalTaktTimes() {
        this.totalTaktTimes = (this.taktTimesForm.hour * 3600) +
            (this.taktTimesForm.minute * 60) +
            this.taktTimesForm.second;
    }

    //generate takt time string detail
    generateTaktTimeString() {
        angular.forEach(this.pdiLeadTimeConfigurations, (pdiLeadTimeConfiguration) => {
            let taktHour = '';
            let taktMinute = '';
            let taktSecond = '';
            taktHour = Math.floor(pdiLeadTimeConfiguration.taktSeconds / 3600) + ' Jam ';
            taktMinute = Math.floor(pdiLeadTimeConfiguration.taktSeconds / 60 % 60) + ' Menit ';
            taktSecond = Math.floor(pdiLeadTimeConfiguration.taktSeconds % 60) + ' Detik';
            pdiLeadTimeConfiguration.taktTimeString = (taktHour + taktMinute + taktSecond).trim();
        });
    }

    //generate location string
    generateLocationString() {
        angular.forEach(this.pdiLeadTimeConfigurations, (pdiLeadTimeConfiguration) => {
            pdiLeadTimeConfiguration.locationString = pdiLeadTimeConfiguration.locationCode + ' - ' + pdiLeadTimeConfiguration.locationName;
        });
    }

    //cehck location , katashiki, and suffix combination existence
    checkDataExistence(dataToSubmit: service.SubmitPDILeadTimeConfigurationFormViewModel[]) {
        let found = 0;
        angular.forEach(dataToSubmit, (data) => {
            var existedData = _.find(this.pdiLeadTimeConfigurations, { 'locationCode': data.locationCode, 'katashiki': data.katashiki, 'suffix': data.suffix });
            if (existedData != null) {
                found += 1;
            } 
        });
        if (found > 0) {
            return false;
        }
        return true;
    }

    // Create/Update PDI Lead Time Configuration(s) data on submit
    formOnSubmit(form: angular.IFormController) {
        this.checkTaktTimes();
        this.calculateTotalTaktTimes();
        if (this.isUpdate === false) {
            this.createPDILeadTimeConfigurations(form);
        }
        else {
            this.updatePDILeadTimeConfiguration(form);
        }
    }

    // Create and send a new PDI Lead Time Configuration data based on the submitted form data
    createPDILeadTimeConfigurations(form: angular.IFormController) {
        let katashikiSuffixPairs = this.getCheckedKatashikisSuffixes();
        let createForms = new Array<service.SubmitPDILeadTimeConfigurationFormViewModel>();
        this.jsonConfirmData['KatashikiSuffix'] = [];
        angular.forEach(katashikiSuffixPairs, (katashikiSuffixPair) => {
            let createForm = new service.SubmitPDILeadTimeConfigurationFormViewModel();
            createForm.katashiki = katashikiSuffixPair.katashiki;
            createForm.suffix = katashikiSuffixPair.suffix;
            createForm.locationCode = this.submitForm.locationCode;
            createForm.post = this.submitForm.post;
            createForm.totalTaktTimes = this.totalTaktTimes;
            createForms.push(createForm);
            this.jsonConfirmData['KatashikiSuffix'].push({ katashiki: katashikiSuffixPair.katashiki, suffix: katashikiSuffixPair.suffix });
        });
        this.jsonConfirmData['Lokasi'] = this.submitForm.locationCode + ' - ' + (_.find(this.locations, ['locationCode', this.submitForm.locationCode])).locationName;
        this.jsonConfirmData['Takt Time'] = this.taktTimesForm.hour + ' Jam ' + this.taktTimesForm.minute + ' Menit ' + this.taktTimesForm.second + ' Detik';
        this.jsonConfirmData['Jumlah Pos'] = this.submitForm.post + '';
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/PDILeadTimeConfigurationAlertify.html'), this.jsonConfirmData),
            () => {
                if (this.checkDataExistence(createForms) === false) {
                    alertify.error('Kombinasi Lokasi, Katashiki, dan Suffix telah terdaftar');
                }
                else {
                    this.pdiLeadTimeConfigurationService.createPDILeadTimeConfigurations(createForms).then(response => {
                        alertify.success('Data berhasil disimpan');
                        this.pdiLeadTimeConfigurations = response.data as service.PDILeadTimeConfigurationViewModel[];
                        this.resetForm(form);
                        this.generateTaktTimeString();
                        this.generateLocationString();
                    }).catch(response => {
                        if (response.status === 400) {
                            alertify.error(response.data);
                            return false;
                        }
                        if (response.status === 500) {
                            alertify.error('Koneksi ke server bermasalah');
                            return false;
                        }
                        alertify.error('Data gagal disimpan')
                        return false;
                    });
                } 
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Set update form
    setUpdateForm(pdiLeadTimeConfig: service.PDILeadTimeConfigurationViewModel) {
        this.selectAllCheckbox = false;
        this.expandAll = true;
        this.setAllCheckboxes();
        this.submitForm = new service.SubmitPDILeadTimeConfigurationFormViewModel();
        this.submitForm.locationCode = pdiLeadTimeConfig.locationCode;
        this.submitForm.post = pdiLeadTimeConfig.post;
        this.submitForm.pdiLeadTimeId = pdiLeadTimeConfig.pdiLeadTimeId;
        this.taktTimesForm = new service.PDILeadTimeConfigurationTaktTimesViewModel();
        this.separateTotalTaktTimes(pdiLeadTimeConfig.taktSeconds);
        this.isUpdate = true;
        this.katashikiSuffixHeader = pdiLeadTimeConfig.katashiki + " - " + pdiLeadTimeConfig.suffix;
        let katashikiModel = _.find(this.carTypeKatashikis, ['katashiki', pdiLeadTimeConfig.katashiki]);
        let suffixModel = _.find(this.carTypeSuffixes, { suffix: pdiLeadTimeConfig.suffix, katashiki: pdiLeadTimeConfig.katashiki });
        let carSeriesModel = _.find(this.carSeries, ['carSeriesCode', katashikiModel.carSeriesCode]);
        let carModel = _.find(this.carModels, ['carModelCode', carSeriesModel.carModelCode]);
        carModel.isChecked = true;
        carModel.isExpand = true;
        carSeriesModel.isChecked = true;
        carSeriesModel.isExpand = true;
        katashikiModel.isChecked = true;
        katashikiModel.isExpand = true;
        suffixModel.isChecked = true;
    }

    // Update a PDI Lead Time Configuration data based on PDIKatsuDictionaryDetailId and the submitted form data
    updatePDILeadTimeConfiguration(form: angular.IFormController) {
        this.jsonConfirmData['Katashiki - Suffix'] = this.katashikiSuffixHeader;
        this.jsonConfirmData['Lokasi'] = this.submitForm.locationCode + ' - ' + (_.find(this.locations, ['locationCode', this.submitForm.locationCode])).locationName;
        this.jsonConfirmData['Takt Time'] = this.taktTimesForm.hour + ' Jam ' + this.taktTimesForm.minute + ' Menit ' + this.taktTimesForm.second + ' Detik';
        this.jsonConfirmData['Jumlah Pos'] = this.submitForm.post + '';
        alertify.confirm("Konfirmasi", mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON("update", this.jsonConfirmData)),
            () => {
                let updatedPDILeadTimeConfig = new service.UpdatePDILeadTimeConfigurationFormViewModel();
                updatedPDILeadTimeConfig.locationCode = this.submitForm.locationCode;
                updatedPDILeadTimeConfig.pdiLeadTimeId = this.submitForm.pdiLeadTimeId;
                updatedPDILeadTimeConfig.post = this.submitForm.post;
                updatedPDILeadTimeConfig.totalTaktTimes = this.totalTaktTimes;
                this.pdiLeadTimeConfigurationService.updatePDILeadTimeConfiguration(updatedPDILeadTimeConfig).then(response => {
                    alertify.success('Data berhasil disimpan')
                    this.resetForm(form);
                    this.pdiLeadTimeConfigurations = response.data as service.PDILeadTimeConfigurationViewModel[];
                    this.generateTaktTimeString();
                    this.generateLocationString();
                }).catch(response => {
                    if (response.status === 400) {
                        alertify.error(response.data);
                        return false;
                    }
                    if (response.status === 500) {
                        alertify.error('Koneksi ke server bermasalah');
                        return false;
                    }
                    alertify.error('Data gagal disimpan');
                    return false;
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Separate the total Takt Times to hour, minute, and second
    separateTotalTaktTimes(totalTaktTimes: number) {
        this.taktTimesForm.hour = Math.floor(totalTaktTimes / 3600);
        this.taktTimesForm.minute = Math.floor(totalTaktTimes / 60 % 60);
        this.taktTimesForm.second = Math.floor(totalTaktTimes % 60);
    }

    // Set create form and clear the input data in the form
    resetForm(form: angular.IFormController) {
        this.isUpdate = false;
        this.selectAllCheckbox = false;
        this.expandAll = true;
        this.setAllCheckboxes();
        this.clearForm(form);
    }

    // Clear the input data in the form
    clearForm(form: angular.IFormController) {
        this.submitForm = new service.SubmitPDILeadTimeConfigurationFormViewModel();
        this.submitForm.post = 0;
        this.taktTimesForm = new service.PDILeadTimeConfigurationTaktTimesViewModel();
        this.taktTimesForm.hour = 0;
        this.taktTimesForm.minute = 0;
        this.taktTimesForm.second = 0;
        this.isUpdate = false;
        this.searchTable = {};
        this.jsonConfirmData = {};
        form.$setPristine();
        form.$setUntouched();
    }

    // Delete a PDI Lead Time Configuration data based on PDIKatsuDictionaryDetailId
    deletePDILeadTimeConfiguration(pdiLeadTimeConfigurationViewModel: service.PDILeadTimeConfigurationViewModel, form: angular.IFormController) {
        this.jsonConfirmData['Lokasi'] = pdiLeadTimeConfigurationViewModel.locationString;
        this.jsonConfirmData['Katashiki - Suffix'] = pdiLeadTimeConfigurationViewModel.katashiki + ' - ' + pdiLeadTimeConfigurationViewModel.suffix;
        this.jsonConfirmData['Takt Time'] = Math.floor(pdiLeadTimeConfigurationViewModel.taktSeconds / 3600) + ' Jam ' + Math.floor(pdiLeadTimeConfigurationViewModel.taktSeconds / 60 % 60) + ' Menit ' + Math.floor(pdiLeadTimeConfigurationViewModel.taktSeconds % 60) + ' Detik';
        this.jsonConfirmData['Jumlah Pos'] = pdiLeadTimeConfigurationViewModel.post + '';
        alertify.confirm('Konfirmasi', mustache.render(require('./alertify/MasterAlertify.html'), this.convertToMustacheJSON('delete', this.jsonConfirmData)),
            () => {
                this.pdiLeadTimeConfigurationService.deletePDILeadTimeConfiguration(pdiLeadTimeConfigurationViewModel.pdiLeadTimeId).then(response => {
                    alertify.success('Data berhasil dihapus');
                    this.pdiLeadTimeConfigurations = response.data as service.PDILeadTimeConfigurationViewModel[];
                    this.resetForm(form);
                    this.generateTaktTimeString();
                    this.generateLocationString();
                }).catch(response => {
                    if (response.status === 500) {
                        alertify.error('Koneksi ke server bermasalah');
                    }
                    else {
                        alertify.error('Data gagal dihapus');
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    //convert data to json for confirmation modal
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

    //set current page
    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    //ordering the table
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    //search the table data
    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }
}

let PDILeadTimeConfiguration = {
    controller: PDILeadTimeController,
    controllerAs: "me",
    template: require("./PDILeadTimeConfiguration.html")
};

export { PDILeadTimeConfiguration };