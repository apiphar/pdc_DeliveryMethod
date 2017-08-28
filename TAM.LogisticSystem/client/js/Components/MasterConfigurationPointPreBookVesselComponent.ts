
import * as service from '../services';
import * as alertify from 'alertifyjs';

class MasterConfigurationPointPreBookVesselController implements angular.IController {
    static $inject = ["masterConfigurationPointPreBookVesselService"];

    constructor(masterConfigurationPointPreBookVesselService: service.MasterConfigurationPointPreBookVesselService) {
        this.masterConfigurationPointPreBookVesselService = masterConfigurationPointPreBookVesselService;
    }

    masterConfigurationPointPreBookVesselService: service.MasterConfigurationPointPreBookVesselService;

    //buat inject model yg di view
    pointPreBookVesselList: service.PointPreBookVesselList;
    pointPreBookVesselsList: service.PointPreBookVesselList[];

    allDatas: service.AllDataNeeded;
    
    //selecttoedit
    editCheck: boolean;
    


    //selecttoedit end

    





    //method yg dipanggil untuk render pertama kali
    $onInit() {

        this.getBookVessel();
        

        
    }

    //method yg bisa digunain di page, via service ofcourse

    //untuk pagination
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
    //pagination end



    getBookVessel() {
        this.masterConfigurationPointPreBookVesselService.getPointPreBookVessel().then(response => {
            this.allDatas = response.data as service.AllDataNeeded;
            this.totalItems = this.allDatas.pointPreBookVesselsList.length;
            

        });


    }

   

    addBookVessel(pointPreBookVesselList,form: angular.IFormController) {
        this.masterConfigurationPointPreBookVesselService.addBookVessel(this.pointPreBookVesselList).then(response => {
            if (response.data == "fail") {
                alertify.error("Add Fail")
            }
            else {
                this.getBookVessel();
                alertify.success('Add Success');
        
                this.resetBookVessel(form);
            }
            
        });
    }

    

    selectEditBookVessel(bookVessel) {
        
        this.pointPreBookVesselList = new service.PointPreBookVesselList;
        this.pointPreBookVesselList.locationCode = bookVessel.locationCode;
        

        this.editCheck = true;

    }

    updateBookVessel(pointPreBookVesselList, form: angular.IFormController) {
        
        this.masterConfigurationPointPreBookVesselService.editBookVessel(this.pointPreBookVesselList).
            then(response => {
                if (response.data == "fail") {
                    alertify.error("Update Fail")
                }
                else {
                    alertify.success("Update Success");
                    this.getBookVessel();
                    this.resetBookVessel(form);

                }
            });
        
    }



    deleteBookVessel(bookVessel, form: angular.IFormController) {
        console.log(form);
        alertify.confirm("Warning!", "Confirm Delete?",
            () => {
                this.masterConfigurationPointPreBookVesselService.deletePointPreBookVessel(bookVessel.locationCode).then(response => {
                    this.getBookVessel();

                    alertify.success('Delete Success');
                    this.resetBookVessel(form);
                });

            },
            () => {
                alertify.error('Cancel');
            });


    }



    

    //UploadFileExcel() {

    //}
    //DownloadFileExcel() {

    //}

    

    resetBookVessel(form: angular.IFormController) {

        form.$setPristine();
        form.$setUntouched();
        this.editCheck = false;
        this.pointPreBookVesselList = new service.PointPreBookVesselList;

    }
}

let MasterConfigurationPointPreBookVesselComponent = {
    controller: MasterConfigurationPointPreBookVesselController,
    controllerAs: "me",
    template: require("./MasterConfigurationPointPreBookVesselComponent.html")
}

export { MasterConfigurationPointPreBookVesselComponent }