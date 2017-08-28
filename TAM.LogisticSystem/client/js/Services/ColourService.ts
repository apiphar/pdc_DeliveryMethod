

export class ColourService {
    static $inject = ['$http'];

    HttpClient: angular.IHttpService;

    constructor($http: angular.IHttpService) {
        this.HttpClient = $http;
    }

    getAllData() {
        return this.HttpClient.get<string>('/API/v1/colourapi');
    }

    createData(ColorCode: string, ColorType: string, IndonesianName: string, EnglishName: string) {
        return this.HttpClient.post('/API/v1/colourapi/create', { ColorCode, ColorType, IndonesianName, EnglishName });
    }

    updateData(ColorCode: string, ColorType: string, IndonesianName: string, EnglishName: string) {
        return this.HttpClient.post('/API/v1/colourapi/edit/' + ColorCode, { ColorCode, ColorType, IndonesianName, EnglishName });
    }

    deleteData(ColorCode: string, ColorType: string) {
        return this.HttpClient.delete('/API/v1/colourapi/delete/' + ColorCode + '/' + ColorType);
    }
    
}