using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;

namespace TAM.LogisticSystem.Services
{
    public class VesselDepartService
    {
        public VesselDepartService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        public async Task<VesselDepartPageViewModel> GetAll()
        {
            var data = new VesselDepartPageViewModel
            {
                ViewModels = await GetViewModels(),
                UnitLists = await GetUnitList(false, "")
            };
            return data;
        }

        // Check UnitList column validity
        public void CheckUnitListValidity()
        {
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(CarModel.Name);
            _ = nameof(CarType.Name);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(Branch.Name);
            _ = nameof(Vehicle.EstimatedPDCIn);
            _ = nameof(Vehicle.HasCustomer);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(Voyage.VoyageNumber);
        }

        // Get data for UnitList
        public async Task<List<UnitListViewModel>> GetUnitList(bool isToDetail, string voyageNumber)
        {
            CheckUnitListValidity();
            var query = @"
SELECT DISTINCT
	v.FrameNumber AS 'FrameNo',
	v.Katashiki AS 'Katashiki',
	v.Suffix AS 'Suffix',
	cm.[Name] AS 'Model',
	ct.[Name] AS 'Tipe',
	ec.IndonesianName AS 'Warna',
	b.[Name] AS 'Branch',
	v.EstimatedPDCIn AS 'PdcIn',
	v.HasCustomer AS 'CustomerAssign',
	v.RequestedDeliveryTime AS 'RequestedPdd',
	vo.VoyageNumber AS 'VoyageNumber',
	vvse.[Name] AS 'Status'
FROM Vehicle v JOIN CarType ct ON ct.Katashiki = v.Katashiki JOIN
	 CarSeries cs ON cs.CarSeriesCode = ct.CarSeriesCode JOIN
	 CarModel cm ON cm.CarModelCode = cs.CarModelCode JOIN
	 ExteriorColor ec ON ec.ExteriorColorCode = v.ExteriorColorCode JOIN
	 Branch b ON b.BranchCode = v.BranchCode JOIN
	 VoyageNodeSourceDetail vnsd ON vnsd.VehicleId = v.VehicleId JOIN
	 VehicleVoyageStatusEnum vvse ON vvse.VehicleVoyageStatusEnumId = vnsd.VehicleVoyageStatusEnumId JOIN
	 VoyageNodeSource vns ON vns.VoyageNodeSourceId = vnsd.VoyageNodeSourceId JOIN
	 VoyageNode vn ON vn.VoyageNodeId = vns.VoyageNodeId JOIN
	 Voyage vo ON vo.VoyageNumber = vn.VoyageNumber
";
            if (isToDetail == true)
            {
                query = query + "WHERE vnsd.VehicleVoyageStatusEnumId = 6 AND vo.VoyageStatusEnumId = 5 AND vo.VoyageNumber = '" + voyageNumber + "'";
            } else
            {
                query = query + "WHERE vnsd.VehicleVoyageStatusEnumId = 5";
            }
            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<UnitListViewModel>(query)).ToList();
            return data;
        }

        // Check ViewModels validity
        public void CheckViewModelsValidity()
        {
            _ = nameof(Voyage.VoyageNumber);
            _ = nameof(DeliveryVendor.Name);
            _ = nameof(DeliveryVendorVehicle.PoliceNumberOrVesselName);
            _ = nameof(Voyage.DepartureDate);
            _ = nameof(DeliveryVendorVehicle.Capacity);
            _ = nameof(VoyageStatusEnum.Name);
        }

        // Get data to fill on 'disabled' input
        public async Task<List<VesselDepartDetailViewModel>> GetViewModels()
        {
            CheckViewModelsValidity();
            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<VesselDepartDetailViewModel>(@"
SELECT
    v.VoyageNumber AS 'VoyageNumber',
	dv.[Name] AS 'Vendor',
	dvv.[PoliceNumberOrVesselName] AS 'Vessel',
	v.DepartureDate AS 'EstimatedTimeDeparture',
    dvv.[Capacity] AS 'Capacity',
    COUNT(CASE WHEN vnsd.VehicleVoyageStatusEnumId = 2 THEN 1 END) AS 'PreBookNotPorted',
	COUNT(CASE WHEN vnsd.VehicleVoyageStatusEnumId = 3 THEN 1 END) AS 'PreBookPorted',
	COUNT(CASE WHEN vnsd.VehicleVoyageStatusEnumId = 4 THEN 1 END) AS 'Assigned',
	COUNT(CASE WHEN vnsd.VehicleVoyageStatusEnumId = 5 THEN 1 END) AS 'Loaded',
    vs.[name] AS 'VoyageStatus',
	vnsd.VoyageNodeSourceId AS 'UnitListId'
FROM	Voyage v JOIN
        VoyageStatusEnum vs ON vs.VoyageStatusEnumId = v.VoyageStatusEnumId JOIN
		DeliveryVendorVehicle dvv ON dvv.DeliveryVendorVehicleId = v.DeliveryVendorVehicleId JOIN
		DeliveryVendor dv ON dv.DeliveryVendorCode = dvv.DeliveryVendorCode JOIN
		VoyageNode vn ON vn.VoyageNumber = v.VoyageNumber JOIN
		VoyageNodeSource vns ON vns.VoyageNodeId = vn.VoyageNodeId JOIN
		VoyageNodeSourceDetail vnsd ON vnsd.VoyageNodeSourceId = vns.VoyageNodeSourceId
GROUP BY v.VoyageNumber, dv.[Name], dvv.[PoliceNumberOrVesselName], v.DepartureDate, dvv.[Capacity], vs.[name], vnsd.VoyageNodeSourceId
")).ToList();

            return data;
        }

        // Update data to 'Departed'
        public async Task DepartVesselByVoyage(VesselDepartSendViewModel vessel)
        {
            var username = WebEnvironment.UserHumanName;
            await this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await this.LogisticDbContext.Database.BeginTransactionAsync())
                {
                    var vesselToUpdate = await this.LogisticDbContext.Voyage
                        .Where(Q => Q.VoyageNumber == vessel.VoyageNumber)
                        .FirstOrDefaultAsync();
                    vesselToUpdate.VoyageStatusEnumId = 5; // Change to 'Departed' status
                    vesselToUpdate.UpdatedAt = DateTimeOffset.UtcNow;
                    vesselToUpdate.UpdatedBy = username;
                    this.LogisticDbContext.Voyage.Update(vesselToUpdate);
                    await this.LogisticDbContext.SaveChangesAsync();

                    var unitList = await this.LogisticDbContext.VoyageNodeSourceDetail
                        .Where(Q => Q.VoyageNodeSourceId == vessel.UnitListId)
                        .ToListAsync();
                    foreach (var unit in unitList)
                    {
                        unit.VehicleVoyageStatusEnumId = 6;
                        unit.UpdatedAt = DateTimeOffset.UtcNow;
                        unit.UpdatedBy = username;
                    }
                    this.LogisticDbContext.VoyageNodeSourceDetail.UpdateRange(unitList);
                    await this.LogisticDbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            });
        }
    }
}
