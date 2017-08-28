

export class MasterCityLocationService {
    static $inject = ['$http'];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    httpService: angular.IHttpService;


    getAllTableData() {
        return this.httpService.get<MasterCityLocationModel[]>('api/v1/MasterCityLocationApi');
    }

    deleteData(deletedId: string) {
        return this.httpService.delete('api/v1/MasterCityLocationApi/' + deletedId);
    }

    createData(createdData: MasterCityLocationModel) {
        return this.httpService.post('api/v1/MasterCityLocationApi/CreateData', createdData);
    }

    updateData(updatedData: MasterCityLocationModel) {
        return this.httpService.post('api/v1/MasterCityLocationApi/UpdateData', updatedData);
    }
}

export class MasterCityLocationModel {
    kodeCityLocation: string;
    cityLocation: string;
}