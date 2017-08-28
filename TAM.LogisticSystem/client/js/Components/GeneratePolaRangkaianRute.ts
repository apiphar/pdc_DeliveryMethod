
import * as Lodash from 'lodash';
import * as Service from '../services';
import * as Alertify from 'alertifyjs';
import * as Mustache from 'mustache';
import * as Moment from 'moment';

export class GeneratePolaRangkaianRuteController implements angular.IController {
    static $inject = ['GeneratePolaRangkaianRuteService'];

    generatePolaRangkaianRute: Service.GeneratePolaRangkaianRuteService;

    dataGeneratePola: any;
    loading: boolean;
    searchFilter = {};

    validFrom: Date;
    all: boolean = false;
    dateOptions = {
        minDate: new Date()
    }

    constructor(generatePolaRangkaianRute: Service.GeneratePolaRangkaianRuteService) {
        this.generatePolaRangkaianRute = generatePolaRangkaianRute;
    }

    $onInit() {
        this.getData();
    }

    // fungsi untuk mengambil data dari server
    getData() {
        this.loading = true;
        this.dataGeneratePola = null;

        this.generatePolaRangkaianRute.GetData().then(response => {
            this.dataGeneratePola = response.data;
            this.totalItems = this.dataGeneratePola.length;

            Lodash.forEach(this.dataGeneratePola, function (data) {
                data.selected = false;
            });
        }).catch(response => {
            if (response.status == "500") {
                Alertify.error("Koneksi ke server bermasalah");
            }
        }).finally(() => {
            this.loading = false;
        });
    }

    checkAll() {
        this.all = Lodash.filter(this.dataGeneratePola, ['selected', true]).length == this.dataGeneratePola.length;
    }

    // fungsi jika checkbox select all di klik maka semua checkbox akan aktif/nonaktif
    selectAll() {
        Lodash.forEach(this.dataGeneratePola, (data) => {
            data.selected = this.all;
        });
    }

    //untuk validasi field yang mandatory
    validation() {
        let generatePola = Lodash.filter(this.dataGeneratePola, ['selected', true]);

        if (generatePola.length < 1) {
            return false;
        }
        if (this.validFrom == null) {
            return false;
        }
        /*
        if (this.validFrom < new Date()) {
            Alertify.error("Berlaku Mulai tidak");
            check = false;
        }
        */
        return true;
    }

    // fungsi untuk melempar data dari client ke server untuk di generate
    generatePola(Form: angular.IFormController) {
        if (this.validation()) {
            let generatePola = Lodash.filter(this.dataGeneratePola, ['selected', true]);
            let jsonGeneralePola = {};

            jsonGeneralePola['validFrom'] = Moment(this.validFrom).format("DD-MMM-YYYY");
            jsonGeneralePola['kodePola'] = generatePola;

            Alertify.confirm(
                "Konfirmasi",
                Mustache.render(require("./alertify/GeneratePolaAlertify.html"), jsonGeneralePola),
                () => {
                    this.generatePolaRangkaianRute.PostData(generatePola, this.validFrom).then(response => {
                        Alertify.success("Data berhasil di-generate");
                    }).catch(response => {
                        if (response.status == "500"){
                            Alertify.error("Koneksi ke server bermasalah");
                        } else {
                            Alertify.error(response.data);
                        }
                    }).finally(() => {
                        this.reset(Form);
                    });
                },
                () => { }
            ).set('labels', {ok:'Ya', cancel:'Tidak'});
        }
    }

    reset(Form: angular.IFormController) {
        Form.$setPristine();
        Form.$setUntouched();

        this.validFrom = null;
        this.searchFilter = {};
        Lodash.forEach(this.dataGeneratePola, (data) => {
            data.selected = false;
        });
    }

    // Paging Sorting Searching
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    totalItems: number;
    currentPage: number = 1;
    maxSize: number = 5;

    // sorting data by column name
    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    // search data
    search(data) {
        this.totalItems = data.length;
        this.setPage(1);
    }

    // set page in html
    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };
}

export class GeneratePolaRangkaianRuteComponent implements angular.IComponentOptions {
    controller = GeneratePolaRangkaianRuteController;
    controllerAs = 'me';

    template = require('./GeneratePolaRangkaianRute.html');
}