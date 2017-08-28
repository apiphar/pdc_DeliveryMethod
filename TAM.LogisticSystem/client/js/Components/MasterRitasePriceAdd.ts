
import * as alertify from 'alertifyjs';
import * as lodash from 'lodash';
import * as Service from '../services';

class MasterRitasePriceAddController implements angular.IController {

    static $inject = ['masterRitasePriceService', '$rootScope'];

    constructor(masterRitasePriceService: Service.MasterRitasePriceService, root: angular.IRootScopeService) {
        this.masterRitasePriceService = masterRitasePriceService;
        this.root = root;
    }



    //property
    masterRitasePriceService: Service.MasterRitasePriceService;
    masterRitasePriceViewModels: Service.MasterRitasePriceViewModel[];
    masterRitasePriceInput: Service.MasterRitasePriceInput;
    confirmRitaseModel: Service.MasterRitasePriceViewModel;
    searchInput: Service.Search;
    root: angular.IRootScopeService;
    flag: boolean; //buat flag add form dan edit form
    flagRefresh: boolean;
    deliveryVendor: Service.DeliveryVendor[];
    deliveryMethod: Service.DeliveryMethod[];
    cityLeg: Service.CityLeg[];


    //function
    $onInit() {
        this.onData()
        this.getAllDataForOptionForm();


    }

    //broadcast flagRefresh
    broadcastFlagRefresh() {
        this.flagRefresh = true;
        this.root.$broadcast('flagRefresh', this.flagRefresh);
    }

    //$on flag
    onData() {
        this.root.$on('flag', (event, data) => {
            this.flag = data;
        });
    }

    //clearForm
    clearForm(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.masterRitasePriceInput = null;
    }

    //addData
    addData(form: angular.IFormController) {
this.masterRitasePriceService.addData(this.masterRitasePriceInput);
        alertify.confirm(
            "Anda Serius untuk Tambah Data :",
            "Kode Vendor : " + this.masterRitasePriceInput.deliveryVendorCode +
            "</br>Kode City Leg : " + this.masterRitasePriceInput.cityLegCode.toString() +
            "</br>Kode Moda : " + this.masterRitasePriceInput.deliveryMethodCode.toString() +
            "</br>Jenis Pengiriman : " + this.masterRitasePriceInput.isSingleTrip.toString() +
            "</br>Berlaku Mulai : " + this.masterRitasePriceInput.validDate.toString() +
            "</br>Currency : " + this.masterRitasePriceInput.currencySymbol.toString() +
            "</br>Nominal : " + this.masterRitasePriceInput.nominal.toString(),
            () => {
                console.log(this.masterRitasePriceInput);
                this.masterRitasePriceService.addData(this.masterRitasePriceInput).then(result => {
                    console.log("Add data succes");
                    this.broadcastFlagRefresh();
                }).catch(result => {
                    console.log("Add data failed");
                })
                //this.clearForm(form);

            },
            () => {
               
               
                

            });


    }

    //get all data for options form
    getAllDataForOptionForm() {
        this.getCityLeg();
        this.getDeliveryMethod();
        this.getDeliveryVendor();
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


}

let MasterRitasePriceAdd = {
    controller: MasterRitasePriceAddController,
    controllerAs: 'me',
    template: require("./MasterRitasePriceAdd.html")
}

export { MasterRitasePriceAdd };
