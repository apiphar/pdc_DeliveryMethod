

export class CompanyMasterService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get<string>('/companymaster/GetData');
    }

    GetAll() {
        return this.HttpClient.get<string>('/companymaster/GetAll');
    }

    PostData(dealerCode: string, companyCode: string, name: string, npwpAddress: string, sapCode: string, phone: string, fax: string, email: string, tradeName: string, npwp: string, isDealerFinancing: boolean) {
        return this.HttpClient.post('/companymaster/create', { dealerCode, companyCode, name, npwpAddress, sapCode, phone, fax, email, tradeName,npwp, isDealerFinancing });
    }

    UpdateData(companyCode: string, dealerCode: string, name: string, npwpAddress: string, sapCode: string, phone: string, fax: string, email: string, tradeName: string, npwp: string, isDealerFinancing: boolean) {
        return this.HttpClient.post('/companymaster/edit/' + companyCode, { dealerCode, name, npwpAddress, sapCode, phone, fax, email, tradeName, npwp, isDealerFinancing  });
    }

    DeleteData(companyCode: string) {
        return this.HttpClient.post('/companymaster/delete/' + companyCode, {});
    }

}