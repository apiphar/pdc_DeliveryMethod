
export class PenyesuaianTanggalProduksiService {
    static $inject = ["$http"];
    constructor(httpClient: angular.IHttpService) {
        this.httpClient = httpClient;
    }
    httpClient: angular.IHttpService;

    getAll() {
        return this.httpClient.get<TotalModel>("api/v1/PenyesuaianTanggalProduksiApi");
    }
    postData(postData: PenyesuaianTanggalProduksiModel) {
        return this.httpClient.post("api/v1/PenyesuaianTanggalProduksiApi",postData)
    }
    updateData(putData: PenyesuaianTanggalProduksiModel) {
        return this.httpClient.put("api/v1/PenyesuaianTanggalProduksiApi", putData);
    }
    deleteData(plant: string) {
        return this.httpClient.delete("api/v1/PenyesuaianTanggalProduksiApi/" + plant)
    }
}
export class  PenyesuaianTanggalProduksiModel{
    id: number;
    plant: string;
    nama: string;
    bukanAkhirBulan: boolean;
    akhirBulan: boolean
    dateStart: Date;
    dateEnd: Date;
    penampungDateEnd: string;
    penampungDateStart: string;
    penampungAkhirBulan: string;
    penampungBab: string;
}
export class ListPlant {
    plant: string;
    nama: string;
    id: number;

}
export class TotalModel {
    allPlantViewModel: ListPlant[];
    allViewModel: PenyesuaianTanggalProduksiModel[];
}