

export class MasterGroupDealerService {
    static $inject = ['$http'];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    httpService: angular.IHttpService;


    getAllTableData() {
        return this.httpService.get<MasterGroupDealerModel[]>('api/v1/MasterGroupDealerApi');
    }    

    deleteData(deletedId: string) {
        return this.httpService.delete('api/v1/MasterGroupDealerApi/'+ deletedId);
    }

    createData(createdData: MasterGroupDealerModel) {
        return this.httpService.post('api/v1/MasterGroupDealerApi/CreateData', createdData);
    }

    updateData(updatedData: MasterGroupDealerModel) {
        return this.httpService.post('api/v1/MasterGroupDealerApi/UpdateData', updatedData);
    }
}

export class MasterGroupDealerModel {
    kodeGroupDealer: string;
    groupDealer: string;
}