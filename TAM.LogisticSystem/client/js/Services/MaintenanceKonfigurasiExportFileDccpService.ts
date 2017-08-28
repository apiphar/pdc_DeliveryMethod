
export class MaintenanceKonfigurasiExportFileDccpService {
    static $inject = ["$http"];
    constructor(httpClient: angular.IHttpService) {
        this.httpClient = httpClient;
    }
    httpClient: angular.IHttpService;
    //getAllData Service
    getAllData() {
        return this.httpClient.get<MaintenanceKonfigurasiExportFileDccpModel[]>("api/v1/MaintenanceKonfigurasiExportFileDccpAPI");
    }

    //delete selected 
    deleteSelectedData(id: number) {
        return this.httpClient.delete("api/v1/MaintenanceKonfigurasiExportFileDccpAPI/" + id);
    }

    //post new config service
    postData(postModel: MaintenanceKonfigurasiExportFileDccpPostModel) {
        return this.httpClient.post("api/v1/MaintenanceKonfigurasiExportFileDccpAPI", postModel);
    }

    //update config service
    updateData(updateModel: MaintenanceKonfigurasiExportFileDccpModel) {
        console.log(updateModel);
        return this.httpClient.put("api/v1/MaintenanceKonfigurasiExportFileDccpAPI", updateModel);
    }
}
export class MaintenanceKonfigurasiExportFileDccpModel {
    id: number;
    description: string;
    rangeStart: string;
    rangeEnd: string;
}
export class MaintenanceKonfigurasiExportFileDccpPostModel {
    description: string;
    rangeStart: string;
    rangeEnd: string;
}