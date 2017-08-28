import * as Service from '../services';
import * as Mustache from 'mustache';
import * as Alertify from 'alertifyjs';
import * as Moment from 'moment';

export class FormAController implements angular.IController {
    static $inject = ['FormAService'];

    formA: Service.FormAService;
    myForm: angular.IFormController;
       
    data: any;
    dataFrameNumber: any;
    
    frameNumber: string;
    formANumber: string;  
    formADate: Date;

    viewby = 1;
    itemsCount = 5;
    currentPage = 0;
    itemsPerPage = this.viewby;
    maxSize = 5;
    pageSizes = [5, 10, 15, 20];
    pageSize = this.pageSizes[0]
    totalItems: number;
 
    constructor(formA: Service.FormAService) {
        this.formA = formA;
        this.formADate = new Date();
    }

    reset(Form: angular.IFormController) {        
        this.frameNumber = null;
        this.formANumber = null;
        this.formADate = null;
    }

    refreshData(Form?: angular.IFormController) {
        this.formA.getAll().then(response => {
            this.data = response.data;

            this.itemsPerPage = 5;
            this.itemsCount = this.totalItems;
            this.totalItems = Math.ceil(response.data.length / this.pageSize);
            this.currentPage = 1;

            this.reset(Form);
        });
    }

    $onInit() {
        this.refreshData();
    }
    
    updateData(Form: angular.IFormController) {  
        this.formA.updateData(this.frameNumber, this.formADate, this.formANumber).then(response => {
            this.refreshData(Form);
        });
    }
    
    checkData(frameNumber) {
        this.frameNumber = this.frameNumber;

        this.formA.getFrameNumber(this.frameNumber).then(response => {
            this.dataFrameNumber = response.data;
            
            this.formANumber = response.data['formANumber'];
            this.formADate = new Date(response.data['formADate']);
        });
    }

    confirmationData() {
    
        let data = {
            frameNumber: this.frameNumber === null ? '\xa0' : this.frameNumber,
            formADate: Moment(this.formADate).format('L'),
            formANumber: this.formANumber,
        };
        return data;
    }

    confirmation(type: string, Form: angular.IFormController) {
        let self = this;
        Alertify.confirm('Are you sure want to ' + type + ' this details ?', Mustache.render(require('./Alertify/FormAAlertify.html'), self.confirmationData()), function () {
            if (type === 'edit') {
                self.updateData(Form);
                Form.$setPristine();
            }
            Alertify.success('Data succesfully updated');
        },
        function () { Alertify.error('Cancel') });
    } 
}

export class FormAComponent implements angular.IComponentOptions {
    controller = FormAController;
    controllerAs = 'me';

    template = require('./FormA.html');
    bindings = {
        greet: '@',
    };
} 