
import * as service from '../services';
import * as model from '../models';
import * as alertify from 'alertifyjs';
export class DownloadDccpReadinessVolumeController implements angular.IController {
    static $inject = ['DownloadDccpReadinessVolumeService'];
    constructor(downloadDccpReadinessVolumeService: service.DownloadDccpReadinessVolumeService) {
        this.downloadDccpReadinessVolumeService = downloadDccpReadinessVolumeService;
    }
    downloadDccpReadinessVolumeService: service.DownloadDccpReadinessVolumeService;
    dateSelected: service.DownloadDate;
    countData: number;
    popupDate: model.AngularUIBootstrapDatepickerPopup;
    format: string;
    isDownload: boolean;
    $onInit() {
        this.popupDate = new model.AngularUIBootstrapDatepickerPopup();
        this.format = 'dd-MMMM-yyyy';
        this.dateSelected = new service.DownloadDate();
        this.isDownload = false;
    }
    //datepicker
    open1() {
        this.popupDate.opened = true;
    }
    //download file dccp
    downloadDccp() {
        
        this.downloadDccpReadinessVolumeService.downloadExcel(this.dateSelected).then(response => {
            if (response.data.guid !== undefined) {
                this.countData = response.data.count;
                this.isDownload = true;
                window.location.assign('/api/v1/DownloadDccpReadinessVolumeApi/Download/'+ response.data.guid);
            }
            else {
                alertify.error('Tidak ada data dengan tanggal pencarian ' + this.dateSelected.date);
                this.isDownload = false;
            }
        }).catch(() => {
            alertify.error('Data Tidak Ditemukan');
            this.isDownload = false;
        });
    }
}
let DownloadDccpReadinessVolume = {
    controller: DownloadDccpReadinessVolumeController,
    controllerAs: 'me',
    template: require('./DownloadDccpReadinessVolume.html')
}
export { DownloadDccpReadinessVolume }