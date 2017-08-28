

export class InspectionPartService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetAll() {
        return this.HttpClient.get<string>('/InspectionPart/GetAll');
	}

	GetDropdownCategory() {
		return this.HttpClient.get<string>('/InspectionPart/GetDropdownCategory');
	} 
      
    GetDropdownSide() {
        return this.HttpClient.get<string>('/InspectionPart/GetDropdownSide'); 
    }

	PostData(Name: String, InspectionCategoryId: number, InspectionSideId: number, CreatedAt: Date, CreatedBy: String, UpdatedAt: Date, UpdatedBy: String) {
		return this.HttpClient.post('/inspectionpart/create', { Name, InspectionCategoryId, InspectionSideId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy });
    }

	UpdateData(InspectionPartId: number, Name: String, InspectionCategoryId: number, InspectionSideId: number, CreatedAt: Date, CreatedBy: String, UpdatedAt: Date, UpdatedBy: String) {
		console.log(Name, InspectionCategoryId, InspectionSideId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy);
		return this.HttpClient.post('/inspectionpart/edit/' + InspectionPartId, { Name, InspectionCategoryId, InspectionSideId, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy });
    }

    DeleteData(InspectionPartId: number) {
        return this.HttpClient.post('/inspectionpart/delete/' + InspectionPartId, {});
    }
}