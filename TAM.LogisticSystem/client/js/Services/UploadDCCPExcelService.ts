

export class UploadDCCPExcelService {
    static $inject = ['$http'];

    http: angular.IHttpService;

    constructor(http: angular.IHttpService) {
        this.http = http;
    }

    uploadDCCP(file: File) {
        let excelFile = this.transformRequestToFormData(file);
        return this.http.post<number>("/api/v1/UploadDCCPExcelApi/UploadDCCP", excelFile, {
            headers: { "Content-Type": undefined }
        });
    }

    transformRequestToFormData(excelFile: File) {
        let formData: FormData = new FormData();
        formData.append("file", excelFile);
        return formData;
    }
}