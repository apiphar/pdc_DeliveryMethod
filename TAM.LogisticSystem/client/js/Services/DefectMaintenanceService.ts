

export class DefectMaintenanceService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetData() {
        return this.HttpClient.get<string>('/defectmaintenance/GetData');
    }


    PostData(Name: String, CreatedAt: Date, CreatedBy: String, UpdatedAt: Date, UpdatedBy: String) {
        return this.HttpClient.post('/defectmaintenance/create', { Name, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy });
    } 

    UpdateData(DefectId: number, Name: String) {
        return this.HttpClient.post('/defectmaintenance/edit/' + DefectId, { Name });
    } 

    DeleteData(DefectId: number) {
        return this.HttpClient.post('/defectmaintenance/delete/' + DefectId,{});
    }

}