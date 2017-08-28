

export class ItemCheckService
{
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService)
    {
        this.HttpClient = $http;
    }

    GetData()
    {
        return this.HttpClient.get<string>('/inspectionitem/GetAll');
    }

    GetDropdownItem() {
        return this.HttpClient.get<string>('/inspectionitem/GetDropdownSide');
    }

    PostData(Name: String, InspectionItemId: number, InspectionMasterId: number)
    {
        return this.HttpClient.post('/inspectionitem/create', { Name, InspectionItemId, InspectionMasterId });
    } 

    UpdateData(InspectionPartId: number, Name: String, InspectionItemId: number, InspectionMasterId: number)
    {
        return this.HttpClient.post('/inspectionitem/edit/' + InspectionPartId, { Name, InspectionItemId, InspectionMasterId });
    } 

    DeleteData(InspectionPartId: number) {
        return this.HttpClient.post('/inspectionitem/delete/' + InspectionPartId, {});
    } 
}