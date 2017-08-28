export class DealerMasterPageViewModel {
    viewModels: DealerMasterViewModel[];
    dealerTypeCodes: DealerMasterTypeCode[];
}

export class DealerMasterViewModel {
    dealerCode: string;
    dealerName: string;
    dealerAddress: string;
    dealerTypeCode: DealerMasterTypeCode;
    dealerTypeString: string;
}

export class DealerMasterTypeCode {
    dealerTypeCode: string;
    dealerTypeName: string;
}

export class DealerMasterService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAll() {
        return this.HttpClient.get<DealerMasterPageViewModel>('../api/v1/DealerMasterApi');
    }

    updateData(dealermasterSend: DealerMasterViewModel) {
        return this.HttpClient.put('../api/v1/DealerMasterApi/update', dealermasterSend);
    }
}