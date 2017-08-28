var tangoApp = angular.module('TangoApp', []);

tangoApp.controller('PDIController', ['$scope', 'PDIService', function ($scope, pdiService) {
    var _this = this;

    pdiService.initializePage($scope);
    pdiService.initializeData($scope);

    $scope.buttonInspectionArea_onClick = function () {
        pdiService.buttonInspectionArea_onClick();
    }

    $scope.buttonInspectionAreaBack_onClick = function () {
        pdiService.buttonInspectionAreaBack_onClick();
    }

    $scope.buttonInspectionAreaChecklist_onClick = function (prefix, areaId, buttonId) {
        pdiService.buttonInspectionAreaChecklist_onClick(prefix, areaId, buttonId, $scope);
    }

    $scope.buttonInspectionMasterChecklistSubmit_onClick = function () {
        pdiService.buttonInspectionMasterChecklistSubmit_onClick($scope);
    }

    $scope.frameNumber_onChange = function () {
        pdiService.frameNumber_onChange($scope);
    }

    $scope.buttonExecute_onClick = function () {
        pdiService.buttonExecute_onClick($scope);
    }

    $scope.validateInputs = function () {
        pdiService.validateInputs($scope);
    }

    $scope.buttonUpdateParking_onClick = function () {
        pdiService.buttonUpdateParking_onClick();
    }

    $scope.buttonClear_onClick = function () {
        pdiService.buttonClear_onClick($scope);
    }
}]);