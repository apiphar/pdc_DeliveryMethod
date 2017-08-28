

export class PermitService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    GetAllData() {
        return this.HttpClient.get<string>('/Permit/GetData');
    }

    GetSuffix(katashiki: String) {
        return this.HttpClient.get<string>('/Permit/getsuffix/' + katashiki);
    }

    GetCarModel(katashiki: String, suffix: String) {
        return this.HttpClient.get<JSON>('/Permit/getcarmodel/' + katashiki + '/' + suffix);
    }

    GetAll() {
        return this.HttpClient.get<string>('/Permit/GetAll');
    }
    
	PostData(katashiki: string, suffix: string, effectiveFrom: string, effectiveUntil: string, Quota: number, permitId: string, Name: string, CarModelCode: string, CreatedAt: Date, CreatedBy: string, UpdatedAt: Date, UpdatedBy: string) {
		console.log('FROM:\n' + effectiveFrom + '\n\nUNTIL:\n' + effectiveUntil + '\n\n\n\n');
		return this.HttpClient.post('/Permit/create', { katashiki, suffix, permitId, Quota, Name, effectiveFrom, effectiveUntil, CarModelCode, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy });
    }

	UpdateData(katashiki: string, suffix: string, effectiveFrom: string, effectiveUntil: string, Quota: number, permitId: string, Name: string, CarModelCode: string, UpdatedAt: Date, UpdatedBy: string) {
		return this.HttpClient.post('/Permit/edit/' + permitId, { katashiki, suffix, permitId, Quota, Name, effectiveFrom, effectiveUntil, CarModelCode, UpdatedAt, UpdatedBy });
	}

	DeleteData(PermitId: string) {
		return this.HttpClient.post('/permit/delete/' + PermitId, {});
	}

}