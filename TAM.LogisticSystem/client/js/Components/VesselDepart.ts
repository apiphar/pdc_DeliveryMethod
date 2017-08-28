import * as angular from 'angular';
import * as service from '../services';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';
import * as moment from 'moment';
import * as Mustache from 'mustache';

class VesselDepartController implements angular.IController {
    static $inject = ['vesselDepartService', '$rootScope'];

    constructor(vesselDepartService: service.VesselDepartService, root: angular.IRootScopeService) {
        this.vesselDepartService = vesselDepartService;
        this.root = root;
    }
    vesselDepartService: service.VesselDepartService;
    root: angular.IRootScopeService;

    // Model Property
    vesselDepartPage: service.VesselDepartPageViewModel;
    voyageDetail: service.VesselDepartViewModel;
    vesselDepartSend: service.VesselDepartSendViewModel;

    // Date Property
    estimatedTimeDepart: Date;
    estimatedTimeArrival: Date;

    // Other Property
    voyageNumber: string;
    voyageFound: boolean = false;
    pageState: boolean = true;

    $onInit() {
        this.getAll();
        this.root.$on("Kembali", (event) => {
            this.pageState = true;
            this.getAll();
        });
    }

    // Get all
    getAll() {
        return this.vesselDepartService.getAll().then(response => {
            this.vesselDepartPage = response.data as service.VesselDepartPageViewModel;
            for (let i in this.vesselDepartPage.viewModels) {
                this.vesselDepartPage.viewModels[i].totalUnit = this.getTotalUnit(this.vesselDepartPage.viewModels[i]);
            }
            for (let i in this.vesselDepartPage.unitLists) {
                this.vesselDepartPage.unitLists[i].pdcInString = moment(this.vesselDepartPage.unitLists[i].pdcIn).format('DD-MMM-YYYY');
                this.vesselDepartPage.unitLists[i].requestedPddString = moment(this.vesselDepartPage.unitLists[i].requestedPdd).format('DD-MMM-YYYY');
            }
        }).catch(response => {
            if (response.status == "500") {
                alertify.error("Koneksi ke server bermasalah");
            }
        });
    }

    // To VesselDepartDetail
    toDetail() {
        this.pageState = false;
        this.root.$emit("Details", this.voyageNumber);
    }

    // Search specific voyage detail 
    vesselDepartSearch() {
        let tempVoyageNumber = this.voyageNumber == null ? '' : this.voyageNumber.toUpperCase();
        this.voyageDetail = _.find(this.vesselDepartPage.viewModels, ['voyageNumber', tempVoyageNumber]);
        if (this.voyageDetail == null) {
            this.voyageFound = false;
        } else if (this.voyageDetail.voyageStatus.toUpperCase() == 'DEPART' || this.voyageDetail.voyageStatus.toUpperCase() == 'ARRIVAL') {
            this.voyageFound = true;
        } else {
            this.vesselDepartSend = new service.VesselDepartSendViewModel();
            this.vesselDepartSend.voyageNumber = this.voyageNumber;
            this.vesselDepartSend.unitListId = this.voyageDetail.unitListId;
            this.voyageFound = true;
        }
    }

    getTotalUnit(item) {
        let totalUnit = item.loaded + item.assigned + item.preBookPorted + item.preBookNotPorted;
        return totalUnit;
    }

    checkValid(result: number) {
        if (this.voyageDetail == null || this.voyageDetail.totalUnit != this.voyageDetail.loaded || this.voyageDetail.totalUnit == 0 || this.voyageFound == false || result == 0) {
            return true;
        }
        return false;
    }

    confirmationData() {
        let jsonData = {};
        jsonData['Voyage Number'] = this.voyageDetail.voyageNumber;
        jsonData['Vendor'] = this.voyageDetail.vendor;
        jsonData['Vessel'] = this.voyageDetail.vessel;
        jsonData['Estimated Time Departure'] = moment(this.voyageDetail.estimatedTimeDeparture).format('DD-MMM-YYYY, HH:mm');
        jsonData['Kapasitas'] = this.voyageDetail.totalUnit + ' / ' + this.voyageDetail.capacity;
        return jsonData;
    }

    // UPDATE voyageStatus to 'DEPART'
    departVoyage(form: angular.IFormController) {
        alertify.confirm('Konfirmasi',
            Mustache.render(require("./alertify/MasterAlertify.html"), this.convertToMustacheJSON("insert", this.confirmationData())),
            () => {
                this.vesselDepartService.departVoyage(this.vesselDepartSend).then(response => {
                    alertify.success('Data berhasil disimpan');
                    this.getAll();
                    this.setPristine(form);
                }).catch(response => {
                    if (response.status == "500") {
                        alertify.error("Koneksi ke server bermasalah");
                    } else {
                        alertify.error("Data gagal disimpan");
                    }
                });
            },
            () => {

            }
        ).set('labels', {ok:'Ya', cancel:'Tidak'});
    }

    convertToMustacheJSON(action: string, json) {
        let convertResult = {
            'insert': null,
            'update': null,
            'delete': null
        }
        let tempJson = [];
        angular.forEach(json, (value, key) => {
            let temp: any = {};
            temp.key = key;
            temp.value = value;
            tempJson.push(temp);
        });

        if (action.toLowerCase() == "insert") convertResult["insert"] = 1;
        else if (action.toLowerCase() == "update") convertResult["update"] = 1;
        else if (action.toLowerCase() == "delete") convertResult["delete"] = 1;

        convertResult["grid"] = tempJson;
        return convertResult;
    }

    // Clear form
    setPristine(form: angular.IFormController) {
        form.$setPristine();
        form.$setUntouched();
        this.reset();
    }

    // Reset property to empty
    reset() {
        this.voyageDetail = new service.VesselDepartViewModel();
        this.voyageDetail = null;
        this.voyageNumber = null;
        this.voyageFound = false;
    }

    // Pagination
    pageSizes: number[] = [5, 10, 15, 20, 25];
    pageSize: number = this.pageSizes[0];
    orderState: boolean = false;
    orderString: string;
    searchResult: any = null;

    currentPage: number = 1;
    maxSize: number = 5;

    order(orderString: string) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
        this.setPage(1);
    }

    setPage(pageNumber: number) {
        this.currentPage = pageNumber;
    }

    setTotalItems(items: number) {
        if (this.voyageFound == false) {
            return 1;
        }
        return items;
    }
}

let VesselDepartComponent = {
    controller: VesselDepartController,
    controllerAs: 'me',

    template: require('./VesselDepart.html')
}

export { VesselDepartComponent }