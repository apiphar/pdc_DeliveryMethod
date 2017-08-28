

export class DwellingTimeService {
    static $inject = ['$http'];

    httpService: angular.IHttpService;

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
    }

    getDwellingData() {
        return this.httpService.get<DwellingTimeViewModel[]>("api/v1/DwellingTimeAPI");
    }
    getLocationCode() {
        return this.httpService.get<GetDwellingLocationViewModel[]>("api/v1/DwellingTimeAPI/GetLocationDwelling");
    }

    postDwellingData(locationFrom: string, locationTo: string, leadMinutes: number) {
        return this.httpService.post("api/v1/DwellingTimeAPI", { locationFrom, locationTo, leadMinutes });
    }

    updateDwellingData(locationFrom: string, locationTo: string, leadMinutes: number) {
        return this.httpService.post("api/v1/DwellingTimeAPI/UpdateDwelling", { locationFrom, locationTo, leadMinutes });
    }

    deleteDwellingData(locationFrom: string, locationTo: string) {
        console.log(locationFrom, locationTo)
        return this.httpService.put("/api/v1/DwellingTimeAPI/DeleteDwelling", { locationFrom, locationTo });
    }
}

export class DwellingTimeViewModel {
    locationFrom: string;
    locationTo: string;
    locationNameFrom: string;
    locationNameTo: string;
    leadMinutes: number;
    day: number;
    hour: number;
    minute: number;
    waktu: string;
}

export class GetDwellingLocationViewModel {
    locationCode: string;
    name: string;
}