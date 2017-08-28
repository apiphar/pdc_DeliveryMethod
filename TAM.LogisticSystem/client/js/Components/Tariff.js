"use strict";
var SchemeTable = (function () {
    function SchemeTable(_scheme, _bm) {
        this.scheme = _scheme;
        this.bm = _bm;
    }
    return SchemeTable;
}());
exports.SchemeTable = SchemeTable;
var TariffController = (function () {
    function TariffController(_sService) {
        this._sService = _sService;
        this.SchemeData = [];
        this.UpdateScheme = [];
        this.pageSizes = [5, 10, 15, 20];
        this.pageSize = this.pageSizes[0];
        this.orderState = false;
        this.searchResult = null;
        this.currentPage = 1;
        this.maxSize = 5;
        this.sService = _sService;
    }
    TariffController.prototype.refreshData = function () {
        var _this = this;
        this.sService.GetData().then(function (response) {
            _this.AllTariffData = response.data;
            _this.totalItems = _this.AllTariffData.length;
        });
        this.reset();
    };
    TariffController.prototype.$onInit = function () {
        this.refreshData();
    };
    TariffController.prototype.AddData = function () {
        this.SchemeData.push(new SchemeTable(this.Scheme, this.BM));
        console.log(this.SchemeData);
        this.Scheme = 0;
        this.BM = 0;
    };
    TariffController.prototype.DeleteData = function (item) {
        var index = this.SchemeData.indexOf(item);
        this.SchemeData.splice(index, 1);
    };
    TariffController.prototype.createData = function () {
        var _this = this;
        this.sService.Add(this.HSCode, this.PPH, this.PPn, this.PPnBM, this.EffectiveFrom, this.SchemeData).then(function (response) {
            _this.refreshData();
        });
    };
    TariffController.prototype.updateData = function () {
        var _this = this;
        this.sService.Update(this.tariffId, this.HSCode, this.PPH, this.PPn, this.PPnBM, this.EffectiveFrom, this.Scheme, this.BM).then(function (response) {
            _this.refreshData();
        });
    };
    TariffController.prototype.Remove = function (data) {
        var _this = this;
        this.sService.DeleteData(data.tariffId).then(function (response) {
            _this.refreshData();
        });
    };
    TariffController.prototype.SelectEdit = function (data) {
        this.tariffId = data.tariffId;
        this.HSCode = data.hsCode;
        this.PPH = data.pph;
        this.PPn = data.pPn;
        this.PPnBM = data.pPnBM;
        this.EffectiveFrom = data.effectiveFrom;
        this.SchemeData = [];
        this.Scheme = data.scheme;
        this.BM = data.bm;
        this.isUpdate = 1;
    };
    TariffController.prototype.reset = function () {
        this.HSCode = null;
        this.PPH = 0;
        this.PPn = 0;
        this.PPnBM = 0;
        this.EffectiveFrom = null;
        this.SchemeData = [];
        this.isUpdate = 0;
        this.Scheme = "";
        this.BM = 0;
        this.isUpdate = 0;
    };
    TariffController.prototype.order = function (orderString) {
        this.orderState = !this.orderState;
        this.orderString = orderString;
    };
    TariffController.prototype.search = function (data) {
        this.totalItems = data.result.length;
        this.setPage(1);
    };
    TariffController.prototype.setPage = function (pageNo) {
        this.currentPage = pageNo;
    };
    return TariffController;
}());
TariffController.$inject = ['TariffService'];
exports.TariffController = TariffController;
var TariffComponent = (function () {
    function TariffComponent() {
        this.controller = TariffController;
        this.controllerAs = 'me';
        this.template = require('./Tariff.html');
    }
    return TariffComponent;
}());
exports.TariffComponent = TariffComponent;
