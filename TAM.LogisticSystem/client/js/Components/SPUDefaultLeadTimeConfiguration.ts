import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as service from '../Services';

export enum SPUDefaultLeadTimeConfigurationFormType {
    Create,
    Update
}

export class SPULineStatusConfigurationController implements angular.IController {
    static $inject = ["SPUDefaultLeadTimeConfigurationService"];
    constructor(spuDefaultLeadTimeConfigurationService: service.SPUDefaultLeadTimeConfigurationService) {
        this.spuDefaultLeadTimeConfigurationService = spuDefaultLeadTimeConfigurationService;
        this.formType = SPUDefaultLeadTimeConfigurationFormType.Create;
        this.isUpdate = false;
    }

    spuDefaultLeadTimeConfigurationPage: service.SPUDefaultLeadTimeConfigurationPageViewModel;
    leadTime = new service.SPULeadTimeViewModel();
    formType: SPUDefaultLeadTimeConfigurationFormType;
    formHeaderLocationName: string;
    isUpdate: boolean;

    spuDefaultLeadTimeConfigurationService: service.SPUDefaultLeadTimeConfigurationService;
    spuDefaultLeadTimeConfigurationModel: service.SPUDefaultLeadTimeConfigurationPageViewModel;
    spuDefaultLeadTimeConfigurationFormData = new service.SPUDefaultLeadTimeConfigurationUpdateFormViewModel();
    spuCreateDefaultLeadTimeConfiguration: service.SPUCreateDefaultLeadTimeConfigurationViewModel;
    spuUpdateDefaultLeadTimeConfiguration: service.SPUUpdateDefaultLeadTimeConfigurationViewModel;
    spuDeleteDefaultLeadTimeConfiguration: service.SPUDeleteDefaultLeadTimeConfigurationViewModel;

    selectedLocation: service.SPUDefaultLeadTimeConfigurationLocationModel;
    selectedProcess: service.ProcessStatusViewModels;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string = "locationName";
    totalItems: number;

    isFormValid(form: angular.IFormController) {
        return angular.equals(form.$error, {});
    }

    $onInit() {
        this.getUpdateSPUDefaultLeadTimeConfigurations();
        
    }

    getUpdateSPUDefaultLeadTimeConfigurations() {
        this.spuDefaultLeadTimeConfigurationService.getData().then(response => {
            this.spuDefaultLeadTimeConfigurationPage = response.data;
            this.totalItems = this.spuDefaultLeadTimeConfigurationPage.spuDefaultLeadTimes.length;
        });
    }

    createSPUDefaultLeadTimeConfiguration(spuDefaultLeadTimeConfigurationForm: angular.IFormController) {
        this.checkLeadTime();
        this.spuCreateDefaultLeadTimeConfiguration = new service.SPUCreateDefaultLeadTimeConfigurationViewModel();
        this.spuCreateDefaultLeadTimeConfiguration.locationCode = this.selectedLocation.locationCode;
        this.spuCreateDefaultLeadTimeConfiguration.processForLineId = this.selectedProcess.processForLineId;
        this.spuCreateDefaultLeadTimeConfiguration.totalLeadTimeMinutes = this.calculateTotalLeadTimeMinutes();

        // Send data that need to be shown in confirmation dialog
        let jsonLeadTimeConfig = {};
        jsonLeadTimeConfig["Lokasi"] = this.selectedLocation.locationName;
        jsonLeadTimeConfig["Proses"] = this.selectedProcess.name;
        jsonLeadTimeConfig["Lead Time"] = this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit";

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", jsonLeadTimeConfig)),
            () => {
                this.spuDefaultLeadTimeConfigurationService.createSPUDefaultLeadTimeConfiguration(this.spuCreateDefaultLeadTimeConfiguration).then(response => {
                    this.spuDefaultLeadTimeConfigurationPage = response.data as service.SPUDefaultLeadTimeConfigurationPageViewModel;
                    this.totalItems = this.spuDefaultLeadTimeConfigurationPage.spuDefaultLeadTimes.length;
                    alertify.success("Data tersimpan");
                    this.resetForm(spuDefaultLeadTimeConfigurationForm);
                });
            },
            () => {}
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Set the form to update SPU default lead time form
    setUpdateForm(routingMasterCode: string, locationCode: string, locationName: string, processForLineId: number, processStatus: string, leadTime: number) {
        this.isUpdate = true;
        this.formHeaderLocationName = locationName;

        this.selectedLocation = new service.SPUDefaultLeadTimeConfigurationLocationModel();
        this.selectedLocation.locationCode = locationCode;
        this.selectedLocation.locationName = locationName;

        this.selectedProcess = new service.ProcessStatusViewModels();
        this.selectedProcess.processForLineId = processForLineId;
        this.selectedProcess.name = processStatus;

        this.spuDefaultLeadTimeConfigurationFormData.routingMasterCode = routingMasterCode;
        this.spuDefaultLeadTimeConfigurationFormData.locationCode = locationCode;
        this.spuDefaultLeadTimeConfigurationFormData.processForLineId = processForLineId;

        this.separateTotalLeadTimeMinutes(leadTime);
    }

    updateSPUDefaultLeadTimeConfiguration(spuDefaultLeadTimeConfigurationForm: angular.IFormController) {
        this.checkLeadTime();
        this.spuUpdateDefaultLeadTimeConfiguration = new service.SPUUpdateDefaultLeadTimeConfigurationViewModel();
        this.spuUpdateDefaultLeadTimeConfiguration.routingMasterCode = this.spuDefaultLeadTimeConfigurationFormData.routingMasterCode;
        this.spuUpdateDefaultLeadTimeConfiguration.oldLocationCode = this.spuDefaultLeadTimeConfigurationFormData.locationCode;
        this.spuUpdateDefaultLeadTimeConfiguration.newLocationCode = this.selectedLocation.locationCode;
        this.spuUpdateDefaultLeadTimeConfiguration.oldProcessForLineId = this.spuDefaultLeadTimeConfigurationFormData.processForLineId;
        this.spuUpdateDefaultLeadTimeConfiguration.newProcessForLineId = this.selectedProcess.processForLineId;
        this.spuUpdateDefaultLeadTimeConfiguration.totalLeadTimeMinutes = this.calculateTotalLeadTimeMinutes();

        // Send data that need to be shown in confirmation dialog
        let jsonLeadTimeConfig = {};
        jsonLeadTimeConfig["Lokasi"] = this.selectedLocation.locationName;
        jsonLeadTimeConfig["Proses"] = this.selectedProcess.name;
        jsonLeadTimeConfig["Lead Time"] = this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit";

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", jsonLeadTimeConfig)),
            () => {
                this.spuDefaultLeadTimeConfigurationService.updateSPUDefaultLeadTimeConfiguration(this.spuUpdateDefaultLeadTimeConfiguration).then(response => {
                    this.spuDefaultLeadTimeConfigurationPage = response.data as service.SPUDefaultLeadTimeConfigurationPageViewModel;
                    this.totalItems = this.spuDefaultLeadTimeConfigurationPage.spuDefaultLeadTimes.length;
                    alertify.success("Data berhasil disimpan");
                    this.resetForm(spuDefaultLeadTimeConfigurationForm);
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(routingMasterCode: string, locationCode: string, locationName: string, processForLineId: number, processStatus: string, leadTime: number) {
        this.spuDeleteDefaultLeadTimeConfiguration = new service.SPUDeleteDefaultLeadTimeConfigurationViewModel();
        this.spuDeleteDefaultLeadTimeConfiguration.routingMasterCode = routingMasterCode;
        this.spuDeleteDefaultLeadTimeConfiguration.locationCode = locationCode;
        this.spuDeleteDefaultLeadTimeConfiguration.processForLineId = processForLineId;

        // Send data that need to be shown in confirmation dialog
        let jsonLeadTimeConfig = {};
        jsonLeadTimeConfig["Lokasi"] = locationName;
        jsonLeadTimeConfig["Proses"] = processStatus;
        jsonLeadTimeConfig["Lead Time"] = this.convertToLeadTimeDay(leadTime) + " Hari " + this.convertToLeadTimeHour(leadTime) + " Jam " + this.convertToLeadTimeMinute(leadTime) + " Menit";

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", jsonLeadTimeConfig)),
            () => {
                this.spuDefaultLeadTimeConfigurationService.deleteSPUDefaultLeadTimeConfiguration(this.spuDeleteDefaultLeadTimeConfiguration)
                    .then(response => {
                        this.spuDefaultLeadTimeConfigurationPage = response.data as service.SPUDefaultLeadTimeConfigurationPageViewModel;
                        this.totalItems = this.spuDefaultLeadTimeConfigurationPage.spuDefaultLeadTimes.length;
                        alertify.success("Data berhasil dihapus");
                    });
            },
            () => {}
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    submitForm(spuDefaultLeadTimeConfigurationForm: angular.IFormController) {
        if (this.isUpdate === false) {
            this.createSPUDefaultLeadTimeConfiguration(spuDefaultLeadTimeConfigurationForm);
        }
        if (this.isUpdate === true) {
            this.updateSPUDefaultLeadTimeConfiguration(spuDefaultLeadTimeConfigurationForm);
        }
    }

    // Reset the create/update form, then set the form as a create a new SPU Default Lead Time form
    resetForm(spuDefaultLeadTimeConfigurationForm: angular.IFormController) {
        spuDefaultLeadTimeConfigurationForm.$setPristine();
        spuDefaultLeadTimeConfigurationForm.$setUntouched();
        this.spuDefaultLeadTimeConfigurationFormData = new service.SPUDefaultLeadTimeConfigurationUpdateFormViewModel();
        this.leadTime = new service.SPULeadTimeViewModel();
        this.isUpdate = false;
        this.formHeaderLocationName = "";
        this.selectedLocation = null;
        this.selectedProcess = null;
        this.leadTime.day = 0;
        this.leadTime.hour = 0;
        this.leadTime.minute = 0;
    }

    // Check the lead time day, hour, and minute. If some of them are undefined/not assigned by the user in the textbox, this method 
    // will assign them to 0
    checkLeadTime() {
        this.leadTime.day = this.leadTime.day === undefined ? 0 : this.leadTime.day;
        this.leadTime.hour = this.leadTime.hour === undefined ? 0 : this.leadTime.hour;
        this.leadTime.minute = this.leadTime.minute === undefined ? 0 : this.leadTime.minute;
    }

    // Get the total lead time minutes by calculating the lead time days, hours, and minutes
    calculateTotalLeadTimeMinutes() {
        let totalLeadTimeMinutes = (this.leadTime.day * 1440) +
            (this.leadTime.hour * 60) +
            this.leadTime.minute;

        return totalLeadTimeMinutes;
    }

    // Separate the total time minutes by days, hours, and minutes and append the values to their own property respectively
    separateTotalLeadTimeMinutes(leadTime: number) {
        this.leadTime.day = this.convertToLeadTimeDay(leadTime);
        this.leadTime.hour = this.convertToLeadTimeHour(leadTime);
        this.leadTime.minute = this.convertToLeadTimeMinute(leadTime);
    }

    convertToLeadTimeDay(leadTime: number) {
        let day = Math.floor(leadTime / 24 / 60);
        return day;
    }

    convertToLeadTimeHour(leadTime: number) {
        let hour = Math.floor(leadTime / 60 % 24);
        return hour;
    }

    convertToLeadTimeMinute(leadTime: number) {
        let minute = Math.floor(leadTime % 60);
        return minute;
    }

    checkInputNumber(value: number) {
        if (value < 0) {
            value = 0;
        } 
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {}
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["action"] = "Apakah anda yakin untuk menambahkan data :";
        else if (action.toLowerCase() == "update") convertResult["action"] = "Apakah anda yakin untuk mengubah data :";
        else if (action.toLowerCase() == "delete") convertResult["action"] = "Apakah anda yakin untuk menghapus data :";
        convertResult["grid"] = tempJson;
        return convertResult;
    }

    //set current page
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
}

let spuLineStatusConfiguration = {
    controller: SPULineStatusConfigurationController,
    controllerAs: "me",
    template: require("./SPUDefaultLeadTimeConfiguration.html")
}

export { spuLineStatusConfiguration }