using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class VesselArrivalService
    {
        public VesselArrivalService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }
        private readonly LogisticDbContext LogisticDbContext;

        // Get all
        public async Task<VesselArrivalPageViewModel> GetAll()
        {
            var data = new VesselArrivalPageViewModel();
            data.UnitLists = await GetUnitList();
            data.CityLists = await GetCityList();
            data.ViewModels = await GetViewModels();
            return data;
        }

        // Get unit list on load
        public async Task<List<UnitListViewModel>> GetUnitList()
        {
            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<UnitListViewModel>(@"
SELECT 
	V.FrameNumber AS 'FrameNo',
	V.Katashiki AS 'Katashiki',
	V.Suffix AS 'Suffix',
	CM.[Name] AS 'Model' ,
	CT.[Name] AS 'Tipe',
	EC.IndonesianName AS 'Warna',
	B.[Name] AS 'Branch',
    V.EstimatedPDCIn AS 'PdcIn',
    V.HasCustomer AS 'CustomerAssign',
	V.EstimatedDeliveryTime AS 'RequestedPdd',
    Vo.VoyageNumber AS 'VoyageNumber'
FROM Vehicle V join CarType CT ON CT.Katashiki = V.Katashiki JOIN
	 CarSeries CS ON CS.CarSeriesCode = CT.CarSeriesCode JOIN
	 CarModel CM ON CM.CarModelCode = CS.CarModelCode JOIN
	 ExteriorColor EC ON EC.ExteriorColorCode = V.ExteriorColorCode JOIN
	 Branch B ON B.BranchCode = V.BranchCode JOIN
	 VehicleAssignmentPerVoyage VAPV ON VAPV.VehicleId = V.VehicleId JOIN
     Voyage Vo ON Vo.VoyageNumber = VAPV.VoyageNumber
")).ToList();

            return data;
        }

        // Get city list on load for dropdown
        public async Task<List<CityListViewModel>> GetCityList()
        {
            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<CityListViewModel>(@"
SELECT
    CityCode AS 'CityId',
    [Name] AS 'CityName'
FROM City
")).ToList();

            return data;
        }

        // Get data to fill 'disabled' input
        public async Task<List<VesselArrivalViewModel>> GetViewModels()
        {
            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<VesselArrivalViewModel>(@"
SELECT DISTINCT
    v.VoyageNumber AS 'VoyageNumber',
	dv.[Name] AS 'Vendor',
	dm.[Name] AS 'Vessel'
FROM Voyage v JOIN
DeliveryVendorVehicle dvv ON dvv.DeliveryVendorVehicleId = v.DeliveryVendorVehicleId JOIN
DeliveryVendor dv ON dv.DeliveryVendorCode = dvv.DeliveryVendorCode JOIN
DeliveryMethod dm ON dm.DeliveryMethodCode = dvv.DeliveryMethodCode
")).ToList();

            return data;
        }

        // TIE: START
        //// POST data
        //public async Task<string> CreateNewVoyageDestination(VesselArrivalCreateViewModel vesselArrivalCreateViewModel)
        //{
        //    var data = await this.LogisticDbContext.VoyageDestination
        //        .Where(Q => Q.VoyageNumber == vesselArrivalCreateViewModel.VoyageNumber).FirstOrDefaultAsync();
        //    if(data != null)
        //    {
        //        return "DUPLICATE";
        //    }
        //    var voyageDestination = new VoyageDestination
        //    {
        //        VoyageNumber = vesselArrivalCreateViewModel.VoyageNumber,
        //        DestinationCity = vesselArrivalCreateViewModel.DestinationCity,
        //        EstimatedTimeArrivalDate = vesselArrivalCreateViewModel.EstimatedTimeArrival.ToUniversalTime()
        //    };

        //    this.LogisticDbContext.VoyageDestination.Add(voyageDestination);
        //    await this.LogisticDbContext.SaveChangesAsync();
        //    return "SUCCESS";
        //}
        // TIE: END
    }
}
