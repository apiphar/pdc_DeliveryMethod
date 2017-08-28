export class MasterLeadTimeLocationService {
    static $inject = ['$http'];

    $http: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.$http = $http;
    }

    //get all data for the table
    getMasterLeadTimeLocationData() {
        return this.$http.get('/api/v1/MasterLeadTimeLocationAPI');
    }

    //get all location code
    getLocations() {
        return this.$http.get('/api/v1/MasterLeadTimeLocationAPI/GetLocationsCode');
    }

    //get routing master code and its name
    getRoutes() {
        return this.$http.get('/api/v1/MasterLeadTimeLocationAPI/GetRoutingMasterData');
    }

    //create new data
    createNewRoutingLocationLeadTimeData(masterLeadTimeLocationInsertUpdateModel: MasterLeadTimeLocationInsertUpdateModel) {
        return this.$http.post('/api/v1/MasterLeadTimeLocationAPI', masterLeadTimeLocationInsertUpdateModel);
    }

    //update the data in the selected row
    updateRow(masterLeadTimeLocationInsertUpdateModel: MasterLeadTimeLocationInsertUpdateModel) {
        return this.$http.put('/api/v1/MasterLeadTimeLocationAPI/Update', masterLeadTimeLocationInsertUpdateModel);
    }

    //delete the data in selected row
    deleteRow(locationCode: string, processMasterCode: string) {
        return this.$http.delete('/api/v1/MasterLeadTimeLocationAPI/Delete', { params: { locationCode: locationCode, processMasterCode: processMasterCode } });
    }
}

export class MasterLeadTimeLocationViewModel {
    locationCode: string;
    locationName: string;
    locationString: string;
    processMasterCode: string;
    routeName: string;
    leadMinutes: number;
    day: number;
    hour: number;
    minute: number;
    leadMinutesString: string;
}

export class MasterLeadTimeLocationInsertUpdateModel {
    locationCode: string;
    processMasterCode: string;
    leadMinutes: number;
}

export class MasterLeadTimeLocationRouteComboBoxModel {
    processMasterCode: string;
    name: string;
}

export class MasterLeadTimeLocationLocationComboBoxModel {
    locationCode: string;
    name: string;
}