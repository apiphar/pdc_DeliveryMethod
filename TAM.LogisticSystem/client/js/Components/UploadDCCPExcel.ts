
import * as service from '../services';

class UploadDCCPExcelController implements angular.IController {
    static $inject = ["UploadDCCPExcelService"];

    uploadDCCPExcelService: service.UploadDCCPExcelService;

    file: File;
    recordUploaded: number;
    currentDate: Date;

    constructor(uploadDCCPExcelService: service.UploadDCCPExcelService) {
        this.uploadDCCPExcelService = uploadDCCPExcelService;

        this.currentDate = new Date();
    }

    uploadDCCP() {
        this.uploadDCCPExcelService.uploadDCCP(this.file).then(response => {
            this.recordUploaded = response.data;
        });
    }
}

let UploadDCCPExcel = {
    controller: UploadDCCPExcelController,
    controllerAs: 'me',
    template: require('./UploadDCCPExcel.html')
}

export { UploadDCCPExcel }