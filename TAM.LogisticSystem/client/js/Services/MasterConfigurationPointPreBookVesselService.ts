

export class MasterConfigurationPointPreBookVesselService {
    static $inject = ["$http"];

    constructor(httpService: angular.IHttpService) {
        this.httpService = httpService;
        console.log(httpService);
    }

    httpService: angular.IHttpService;

    //kalo post kasi ada parameter object, buat add ke DB

    getPointPreBookVessel() {
        return this.httpService.get<AllDataNeeded>("api/v1/MasterConfigurationPointPreBookVesselApi")
    }

    
    addBookVessel(bookVessel: PointPreBookVesselList) {
        return this.httpService.post("api/v1/MasterConfigurationPointPreBookVesselApi/AddBookVessel", bookVessel);
    }

    editBookVessel(bookVessel: PointPreBookVesselList) {
        return this.httpService.put("api/v1/MasterConfigurationPointPreBookVesselApi/UpdateBookVessel", bookVessel);
    }

    deletePointPreBookVessel(locationCode: string) {
        return this.httpService.delete("/api/v1/MasterConfigurationPointPreBookVesselApi/" + locationCode);
    }

    
}

//export model class yg bakal dipake di html
export class PointPreBookVesselList {
    locationCode: string;
    pointPreBookVesselName: string;
    pointPreBookVesselId: string;
}

export class MasterRoutingNeeded {
    pointPreBookVesselName: string;
    pointPreBookVesselId: string;
}

export class AllDataNeeded {
    pointPreBookVesselsList: PointPreBookVesselList[];
    locationCodes: string[];
    masterRoutingDataNeeded: MasterRoutingNeeded[];
}

//export class Excel{
//    excel: File;
//}

