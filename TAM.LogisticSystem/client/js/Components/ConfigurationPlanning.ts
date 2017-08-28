import * as Service from '../services';
import * as mustache from 'mustache';
import * as alertify from 'alertifyjs';
import * as _ from 'lodash';

export class ConfigurationPlanningController implements angular.IController {
    static $inject = ['ConfigurationPlanningService'];

    //declare IFormController
    MyForm: angular.IFormController;

    data: any;
    dataRoutingMasterMCCP: any;
    dataRoutingMasterDCCP: any;
    jmlHari: number;
    routingMastersMCCP: any;
    routingMastersDCCP: any;

    mccp: boolean;
    dccp: boolean;

    ConfigurationPlanning: Service.ConfigurationPlanningService;

    constructor(configurationPlanning: Service.ConfigurationPlanningService) {
        this.ConfigurationPlanning = configurationPlanning;
    }

    refreshData() {
        this.ConfigurationPlanning.getAll().then(response => {
            this.data = response.data;
        });

        this.ConfigurationPlanning.getRoutingMaster().then(response => {
            this.dataRoutingMasterMCCP = response.data;
        });

        this.ConfigurationPlanning.getRoutingMaster().then(response => {
            this.dataRoutingMasterDCCP = response.data;
        });
    }

    clearForm() {
        this.routingMastersMCCP = null;
        this.routingMastersDCCP = null;
        this.jmlHari = null;
        this.refreshData();
    }

    $onInit() {
        this.refreshData();
    }

    updateData() {
        this.mccp = true;
        this.dccp = true;
        this.ConfigurationPlanning.updateData(this.routingMastersMCCP["routingMasterCode"], this.routingMastersDCCP["routingMasterCode"], this.mccp, this.dccp).then(response => {
            this.clearForm();
            this.refreshData();
        });
    }

    selectDelete(data, MyForm: angular.IFormController) {
        console.log(this.getAllSelectedData)
        this.getAllSelectedData(data);
        this.confirmationDialog('Apakah anda yakin untuk menghapus data ?', 'Delete', MyForm)
    }

    deleteData() {
        this.mccp = true;
        this.dccp = true;
        this.ConfigurationPlanning.deleteData(this.mccp, this.dccp).then(response => {
            this.clearForm();
            this.refreshData();
        });
    }

    getAllSelectedData(data) {
        console.log(this.routingMastersMCCP,this.routingMastersDCCP)
        this.routingMastersMCCP = _.find(this.dataRoutingMasterMCCP, ['routingMasterCode', data.routingMasterCode]);
        this.routingMastersDCCP = _.find(this.dataRoutingMasterDCCP, ['routingMasterCode', data.routingMasterCode]);

    }

    alertifyData() {
        var data = {
            routingMasterCodeMCCP: (!this.routingMastersMCCP) ? '' : this.routingMastersMCCP["routingMasterCode"],
            nameMCCP: (!this.routingMastersMCCP) ? '' : this.routingMastersMCCP["name"],
            routingMasterCodeDCCP: (!this.routingMastersDCCP) ? '' : this.routingMastersDCCP["routingMasterCode"],
            nameDCCP: (!this.routingMastersDCCP) ? '' : this.routingMastersDCCP["name"],
            jmlHari: this.jmlHari,
        };
        return data;
    }

    confirmationDialog(Message: string, Params: string, MyForm: angular.IFormController) {
        let self = this;
        if (Params == 'Delete') {
            self.deleteData();
            MyForm.$setPristine();
            alertify.success('Data berhasil dihapus');
        }

        else if (self.routingMastersMCCP == null && self.routingMastersDCCP == null) {
            MyForm.$setPristine();
            alertify.error('CCP report is based on ETP Initialized of Routing Master ID harus dipilih, DCCP report is based on ETP Adjusted & Actual Time of Routing Master ID harus dipilih');
        }

        else if (self.routingMastersMCCP == null ){
            MyForm.$setPristine();
            alertify.error('MCCP report is based on ETP Initialized of Routing Master ID harus dipilih');
        }

        else if (self.routingMastersDCCP == null ){
            MyForm.$setPristine();
            alertify.error('DCCP report is based on ETP Adjusted & Actual Time of Routing Master ID harus dipilih');
        }

        else
            alertify.confirm(Message,
                mustache.render(require("./alertify/ConfigurationPlanningAlertify.html"), this.alertifyData()),
                function () {
                    if (Params == 'Update') {
                        self.updateData();
                        MyForm.$setPristine();
                        alertify.success('Data berhasil disimpan');
                    }
                    else {
                        MyForm.$setPristine();
                    }
                },
                function () {
                    alertify.error('Cancel');
                }
            );
    }
}

export class ConfigurationPlanningComponent implements angular.IComponentOptions {
    controller = ConfigurationPlanningController;
    controllerAs = 'me';

    template = require('./ConfigurationPlanning.html');
    bindings = {
        greet: '@'
    };
}