import * as angular from 'angular';
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as Service from '../services';
import * as mustache from 'mustache';

class MasterRitasePriceController implements angular.IController {

    static $inject = ['masterRitasePriceService', '$rootScope'];

    constructor(masterRitasePriceService: Service.MasterRitasePriceService, root: angular.IRootScopeService) {
        this.masterRitasePriceService = masterRitasePriceService;
        this.root = root;
    }



    //property
    masterRitasePriceService: Service.MasterRitasePriceService;
    masterRitasePriceViewModels: Service.MasterRitasePriceViewModel[];
    masterRitasePriceEditModel: Service.MasterRitasePriceViewModel;
    masterRitasePriceInput: Service.MasterRitasePriceInput;
    confirmRitaseModel: Service.MasterRitasePriceViewModel;
    searchInput: Service.Search;
    root: angular.IRootScopeService;
    flag: boolean; //buat flag add form dan edit form
    flagRefresh: boolean;
    deliveryVendor: Service.DeliveryVendor[];
    deliveryMethod: Service.DeliveryMethod[];
    cityLeg: Service.CityLeg[];
    currencySymbol: Service.CurrencySymbol[];

    //datepicker property
    isOpenDate = false;
    altInputFormat = ['M!/d!/yyyy'];



    //function
    $onInit() {
        this.flag = true;
        this.broadcastFlag();
        this.refreshPage();
    }

    //refresh Page
    refreshPage() {
        this.getAllRitasePriceService();
        this.getAllDataForOptionForm();
    }

    //datepicker function show & hide
    openDate() {
        this.isOpenDate = !this.isOpenDate;

    }


    //addData
    addData(form: angular.IFormController) {
        if (this.masterRitasePriceInput == null) {
            alertify.error("Form tidak boleh kosong");
        } else {
            alertify.confirm("Konfirmasi Tambah Data",
                mustache.render(require("./alertify/MasterRitasePriceAlertify.html"), this.masterRitasePriceInput),
                () => {
                    this.masterRitasePriceService.addData(this.masterRitasePriceInput).then(result => {
                        this.refreshPage();
                    }).catch(result => {
                    })
                    this.clearForm(form);
                },
                () => {

                });
        }
    }

    //clearForm Add
    clearForm(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.masterRitasePriceInput = new Service.MasterRitasePriceInput;
    }

    //clear form Edit
    clearFormEdit(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.masterRitasePriceEditModel = new Service.MasterRitasePriceViewModel;
    }

    //get all data for options form
    getAllDataForOptionForm() {
        this.getCityLeg();
        this.getDeliveryMethod();
        this.getDeliveryVendor();
        this.getCurrencySymbol();
    }

    //get all delivery vendor code
    getDeliveryVendor() {
        this.masterRitasePriceService.getDeliveryVendor().then(result => {
            this.deliveryVendor = result.data as Service.DeliveryVendor[];
        });
    }

    //get all delivery method code
    getDeliveryMethod() {
        this.masterRitasePriceService.getDeliveryMethod().then(result => {
            this.deliveryMethod = result.data as Service.DeliveryMethod[];
        });
    }

    //get all city leg code
    getCityLeg() {
        this.masterRitasePriceService.getCityLeg().then(result => {
            this.cityLeg = result.data as Service.CityLeg[];
        });
    }

    //get all currency symbol
    getCurrencySymbol() {
        this.masterRitasePriceService.getCurrencySymbol().then(result => {
            this.currencySymbol = result.data as Service.CurrencySymbol[];
        })
    }


    //broadcast for edit data
    selectEdit(data) {
        this.flag = false;
        this.masterRitasePriceEditModel = new Service.MasterRitasePriceViewModel;
        this.masterRitasePriceEditModel.cityLegCode = data.cityLegCode;
        this.masterRitasePriceEditModel.cityLegRitaseCostId = data.cityLegRitaseCostId;
        this.masterRitasePriceEditModel.currencySymbol = data.currencySymbol;
        this.masterRitasePriceEditModel.deliveryMethodCode = data.deliveryMethodCode;
        this.masterRitasePriceEditModel.deliveryVendorCode = data.deliveryVendorCode;
        this.masterRitasePriceEditModel.isSingleTrip = data.isSingleTrip;
        this.masterRitasePriceEditModel.nominal = data.nominal;
        this.masterRitasePriceEditModel.validDate = new Date(data.validDate);

    }


    //ganti edit to add form or sebalinya
    changeForm() {
        this.flag = !this.flag;
    }

    //broadcastFlag
    broadcastFlag() {
        this.root.$broadcast("flag", this.flag);
    }

    //get all ritase data
    getAllRitasePriceService() {
        this.masterRitasePriceService.getAllRitaseData().then(result => {
            this.masterRitasePriceViewModels = result.data as Service.MasterRitasePriceViewModel[];
            angular.forEach(this.masterRitasePriceViewModels, result => {
                if (result.isSingleTrip.toString() === "true") {
                    result.isSingleTrip = "Round Trip";
                } else if (result.isSingleTrip.toString() === "false") {
                    result.isSingleTrip = "Single Trip";
                }
            })
            this.totalItems = this.masterRitasePriceViewModels.length;
        }).catch(e => {
        })
    }

    //update data
    updateData(form: angular.IFormController) {     
        if (this.masterRitasePriceEditModel == null) {
            alertify.error("Form Tidak Boleh Kosong");
        } else {
            alertify.confirm("Konfirmasi Edit Data",
                mustache.render(require("./alertify/MasterRitasePriceAlertify.html"), this.masterRitasePriceEditModel),
                () => {
                    this.masterRitasePriceService.updateData(this.masterRitasePriceEditModel).then(result => {
                        this.refreshPage();
                    }).catch(result => {
                        })
                    this.clearFormEdit(form);
                },
                () => {

                });

            
        }
    }


    //delete ritase data by id
    deleteData(data) {
        this.confirmRitaseModel = data;
        alertify.confirm(
            "Anda Serius mau menghapus data dengan id " + data.cityLegRitaseCostId + " ?",
            () => {
                this.masterRitasePriceService.deleteData(data.cityLegRitaseCostId).then(message => {
                    this.refreshPage();
                }).catch(message => {
                })

            },
            () => {


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

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    }

    search(data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    }

    setPage(pageNo: number) {
        this.currentPage = pageNo;
    };

}

let MasterRitasePrice = {
    controller: MasterRitasePriceController,
    controllerAs: 'me',
    template: require("./MasterRitasePrice.html")
}

export { MasterRitasePrice };
