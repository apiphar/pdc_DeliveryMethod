import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as mustache from 'mustache';
import * as moment from 'moment';
import * as service from '../Services';

export enum FormType {
    Create,
    Update
}

export class PIOLineStatusConfigurationController implements angular.IController {
    static $inject = ["PIODefaultLeadTimeConfigurationService"];
    constructor(pioDefaultLeadTimeConfigurationService: service.PIODefaultLeadTimeConfigurationService) {
        this.pioDefaultLeadTimeConfigurationService = pioDefaultLeadTimeConfigurationService;
        this.formType = FormType.Create;
        this.isUpdate = false;
    }

    pioDefaultLeadTimeConfigurationPage: service.PIODefaultLeadTimeConfigurationPageViewModel;
    leadTime: service.PIOLeadTimeViewModel = new service.PIOLeadTimeViewModel();
    tempLeadTime: service.PIOLeadTimeViewModel = new service.PIOLeadTimeViewModel();
    formType: FormType;
    formHeaderLocationName: string;
    isUpdate: boolean;
    isLoading: boolean;

    pioDefaultLeadTimeConfigurationService: service.PIODefaultLeadTimeConfigurationService;
    pioDefaultLeadTimeConfigurationModel: service.PIODefaultLeadTimeConfigurationPageViewModel;
    pioDefaultLeadTimeConfigurationFormData = new service.PIODefaultLeadTimeConfigurationUpdateFormViewModel();
    pioCreateDefaultLeadTimeConfiguration: service.PIOCreateDefaultLeadTimeConfigurationViewModel;
    pioUpdateDefaultLeadTimeConfiguration: service.PIOUpdateDefaultLeadTimeConfigurationViewModel;
    pioDeleteDefaultLeadTimeConfiguration: service.PIODeleteDefaultLeadTimeConfigurationViewModel;

    selectedLocation: service.PIODefaultLeadTimeConfigurationLocationModel;
    selectedProcess: service.ProcessStatusViewModel;

    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    maxSize: number = 5;
    currentPage: number = 1;
    orderState: boolean = false;
    orderString: string = "locationName";
    totalItems: number;


    $onInit() {
        this.getUpdatePIODefaultLeadTimeConfigurations();
        this.leadTime.day = 0;
        this.leadTime.hour = 0;
        this.leadTime.minute = 0;

        this.isLoading = true;
    }

    getUpdatePIODefaultLeadTimeConfigurations() {
        this.pioDefaultLeadTimeConfigurationService.getData().then(response => {
            this.pioDefaultLeadTimeConfigurationPage = response.data;
            this.totalItems = this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes.length;

            angular.forEach(this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes, (value, key) => {
                this.separateTempTotalLeadMinutes(value.leadTime);
                value.leadTimeToString = (this.tempLeadTime.day + " Hari " + this.tempLeadTime.hour + " Jam " + this.tempLeadTime.minute + " Menit").toString();
            });

            this.isLoading = false;
        });
    }

    createPIODefaultLeadTimeConfiguration(pioDefaultLeadTimeConfigurationForm: angular.IFormController) {
        this.checkLeadTime();
        this.pioCreateDefaultLeadTimeConfiguration = new service.PIOCreateDefaultLeadTimeConfigurationViewModel();

        if (this.selectedLocation.locationCode == null) {
            this.selectedLocation.locationCode = "";
        }
        else {
            this.pioCreateDefaultLeadTimeConfiguration.locationCode = this.selectedLocation.locationCode;
        }

        if (this.selectedProcess.processForLineId == 0) {
            this.selectedProcess.processForLineId = 0;
        }
        else {
            this.pioCreateDefaultLeadTimeConfiguration.processForLineId = this.selectedProcess.processForLineId;
        }
        this.pioCreateDefaultLeadTimeConfiguration.totalLeadTimeMinutes = this.calculateTotalLeadTimeMinutes();

        // Send data that need to be shown in confirmation dialog
        let jsonLeadTimeConfig = {};
        jsonLeadTimeConfig["Lokasi"] = this.selectedLocation.locationName;
        jsonLeadTimeConfig["Proses"] = this.selectedProcess.name;
        jsonLeadTimeConfig["Lead Time"] = this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit";

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", jsonLeadTimeConfig)),
            () => {
                this.pioDefaultLeadTimeConfigurationService.createPIODefaultLeadTimeConfiguration(this.pioCreateDefaultLeadTimeConfiguration).then(response => {
                    this.pioDefaultLeadTimeConfigurationPage = response.data as service.PIODefaultLeadTimeConfigurationPageViewModel;

                    angular.forEach(this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes, (value, key) => {
                        this.separateTotalLeadTimeMinutes(value.leadTime);
                        value.leadTimeToString = (this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit").toString();
                    });

                    this.totalItems = this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes.length;
                    alertify.success("Data tersimpan");
                    this.resetForm(pioDefaultLeadTimeConfigurationForm);
                }).catch(response => {
                    if (response.data == "duplicate") {
                        alertify.error("Data sudah tersedia");
                    }
                    else {
                        alertify.error("Data gagal disimpan");
                    }
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    // Set the form to update PIO default lead time form
    setUpdateForm(routingMasterCode: string, locationCode: string, locationName: string, processForLineId: number, processStatus: string, leadTime: number) {
        this.isUpdate = true;
        this.formHeaderLocationName = locationName;

        this.selectedLocation = new service.PIODefaultLeadTimeConfigurationLocationModel();
        this.selectedLocation.locationCode = locationCode;
        this.selectedLocation.locationName = locationName;

        this.selectedProcess = new service.ProcessStatusViewModel();
        this.selectedProcess.processForLineId = processForLineId;
        this.selectedProcess.name = processStatus;

        this.pioDefaultLeadTimeConfigurationFormData.routingMasterCode = routingMasterCode;
        this.pioDefaultLeadTimeConfigurationFormData.locationCode = locationCode;
        this.pioDefaultLeadTimeConfigurationFormData.processForLineId = processForLineId;

        this.separateTotalLeadTimeMinutes(leadTime);
    }

    updatePIODefaultLeadTimeConfiguration(pioDefaultLeadTimeConfigurationForm: angular.IFormController) {
        this.checkLeadTime();
        this.pioUpdateDefaultLeadTimeConfiguration = new service.PIOUpdateDefaultLeadTimeConfigurationViewModel();
        this.pioUpdateDefaultLeadTimeConfiguration.routingMasterCode = this.pioDefaultLeadTimeConfigurationFormData.routingMasterCode;
        this.pioUpdateDefaultLeadTimeConfiguration.oldLocationCode = this.pioDefaultLeadTimeConfigurationFormData.locationCode;
        this.pioUpdateDefaultLeadTimeConfiguration.newLocationCode = this.selectedLocation.locationCode;
        this.pioUpdateDefaultLeadTimeConfiguration.oldProcessForLineId = this.pioDefaultLeadTimeConfigurationFormData.processForLineId;
        this.pioUpdateDefaultLeadTimeConfiguration.newProcessForLineId = this.selectedProcess.processForLineId;
        this.pioUpdateDefaultLeadTimeConfiguration.totalLeadTimeMinutes = this.calculateTotalLeadTimeMinutes();

        // Send data that need to be shown in confirmation dialog
        let jsonLeadTimeConfig = {};
        jsonLeadTimeConfig["Lokasi"] = this.selectedLocation.locationName;
        jsonLeadTimeConfig["Proses"] = this.selectedProcess.name;
        jsonLeadTimeConfig["Lead Time"] = this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit";

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("update", jsonLeadTimeConfig)),
            () => {
                this.pioDefaultLeadTimeConfigurationService.updatePIODefaultLeadTimeConfiguration(this.pioUpdateDefaultLeadTimeConfiguration).then(response => {
                    this.pioDefaultLeadTimeConfigurationPage = response.data as service.PIODefaultLeadTimeConfigurationPageViewModel;

                    angular.forEach(this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes, (value, key) => {
                        this.separateTotalLeadTimeMinutes(value.leadTime);
                        value.leadTimeToString = (this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit").toString();
                    });

                    this.totalItems = this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes.length;
                    alertify.success("Data berhasil disimpan");
                    this.resetForm(pioDefaultLeadTimeConfigurationForm);
                }).catch(response => {
                    alertify.error("Data gagal disimpan");
                });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    deleteData(pioDefaultLeadTimeConfigurationForm: angular.IFormController, routingMasterCode: string, locationCode: string, locationName: string, processForLineId: number, processStatus: string, leadTime: number) {
        this.pioDeleteDefaultLeadTimeConfiguration = new service.PIODeleteDefaultLeadTimeConfigurationViewModel();
        this.pioDeleteDefaultLeadTimeConfiguration.routingMasterCode = routingMasterCode;
        this.pioDeleteDefaultLeadTimeConfiguration.locationCode = locationCode;
        this.pioDeleteDefaultLeadTimeConfiguration.processForLineId = processForLineId;

        // Send data that need to be shown in confirmation dialog
        let jsonLeadTimeConfig = {};
        jsonLeadTimeConfig["Lokasi"] = locationName;
        jsonLeadTimeConfig["Proses"] = processStatus;
        jsonLeadTimeConfig["Lead Time"] = this.convertToLeadTimeDay(leadTime) + " Hari " + this.convertToLeadTimeHour(leadTime) + " Jam " + this.convertToLeadTimeMinute(leadTime) + " Menit";

        let self = this;

        alertify.confirm(
            "Konfirmasi",
            mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("delete", jsonLeadTimeConfig)),
            () => {
                this.pioDefaultLeadTimeConfigurationService.deletePIODefaultLeadTimeConfiguration(this.pioDeleteDefaultLeadTimeConfiguration)
                    .then(response => {
                        this.pioDefaultLeadTimeConfigurationPage = response.data as service.PIODefaultLeadTimeConfigurationPageViewModel;

                        angular.forEach(this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes, (value, key) => {
                            this.separateTotalLeadTimeMinutes(value.leadTime);
                            value.leadTimeToString = (this.leadTime.day + " Hari " + this.leadTime.hour + " Jam " + this.leadTime.minute + " Menit").toString();
                        });

                        this.totalItems = this.pioDefaultLeadTimeConfigurationPage.pioDefaultLeadTimes.length;
                        self.resetForm(pioDefaultLeadTimeConfigurationForm);
                        alertify.success("Data berhasil dihapus");
                    }).catch(response => {
                        console.log(response);
                        alertify.error("Data gagal dihapus");
                    });
            },
            () => { }
        ).set('labels', { ok: 'Ya', cancel: 'Tidak' });
    }

    submitForm(pioDefaultLeadTimeConfigurationForm: angular.IFormController) {
        if (this.isUpdate === false) {
            this.createPIODefaultLeadTimeConfiguration(pioDefaultLeadTimeConfigurationForm);
        }
        if (this.isUpdate === true) {
            this.updatePIODefaultLeadTimeConfiguration(pioDefaultLeadTimeConfigurationForm);
        }
    }

    // Reset the create/update form, then set the form as a create a new PIO Default Lead Time form
    resetForm(pioDefaultLeadTimeConfigurationForm: angular.IFormController) {
        pioDefaultLeadTimeConfigurationForm.$setPristine();
        pioDefaultLeadTimeConfigurationForm.$setUntouched();
        this.pioDefaultLeadTimeConfigurationFormData = new service.PIODefaultLeadTimeConfigurationUpdateFormViewModel();
        this.leadTime.day = 0;
        this.leadTime.hour = 0;
        this.leadTime.minute = 0;
        this.isUpdate = false;
        this.formHeaderLocationName = "";
        this.selectedLocation = null;
        this.selectedProcess = null;
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

    separateTempTotalLeadMinutes(leadTime: number) {
        this.tempLeadTime.day = this.convertToLeadTimeDay(leadTime);
        this.tempLeadTime.hour = this.convertToLeadTimeHour(leadTime);
        this.tempLeadTime.minute = this.convertToLeadTimeMinute(leadTime);
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
        console.log(value);
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

let pioLineStatusConfiguration = {
    controller: PIOLineStatusConfigurationController,
    controllerAs: "me",
    template: require("./PIODefaultLeadTimeConfiguration.html")
}

export { pioLineStatusConfiguration }