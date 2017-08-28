

export class MasterCompanyService {
    static $inject = ['$http'];

    $http: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.$http = $http;
    }

    //get all companies data
    getCompanies() {
        return this.$http.get('/api/v1/MasterCompanyAPI/Companies');
    }

    //get all dealers data
    getDealers() {
        return this.$http.get('api/v1/MasterCompanyAPI/Dealers');
    }

    //update data based on form data
    updateData(masterCompanyData: MasterCompanyViewModel) {
        console.log('masuk');
        return this.$http.put('/api/v1/MasterCompanyAPI/Update', masterCompanyData);
    }
}

export class MasterCompanyViewModel {
    dealerCode: string;
    dealerName: string;
    dealerString: string;
    companyCode: string;
    companyName: string;
    npwpAddress: string;
    sapCode: string;
    phone: string;
    fax: string;
    email: string;
    tradeName: string;
    npwp: string;
    isDealerFinancing: boolean;
    termOfPaymentDay: number;
    termOfPaymentDayString: string;
}

export class MasterCompanyDealerComboBoxModel {
    dealerCode: string;
    name: string;
}