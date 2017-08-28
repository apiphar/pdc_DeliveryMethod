using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class UnitAssignService
    {
        //private readonly
        private readonly LogisticDbContext _context;
        private readonly WebEnvironmentService _env;

        public UnitAssignService(LogisticDbContext context, WebEnvironmentService env)
        {
            this._context = context;
            this._env = env;
        }

        //Get All the Unit Details from DB
        public async Task<List<UnitAssignUnitListModel>> GetAllDetails(string VoyageInput)
        {
            _ = nameof(Voyage.VoyageNumber);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(CarModel.Name);
            _ = nameof(CarType.Name);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(Branch.Name);
            _ = nameof(Vehicle.HasCustomer);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(Location.Name);
            _ = nameof(VoyageStatusEnum.Name);
            _ = nameof(VoyageStatusEnum.VoyageStatusEnumId);
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(Vehicle.EstimatedPDCIn);

            var data = (await _context.Database.GetDbConnection().QueryAsync<UnitAssignUnitListModel>(
                @"
					SELECT
	                    V.FrameNumber as FrameNumber,
	                    V.Katashiki as Katashiki,
	                    V.Suffix as Suffix,
	                    CM.[Name] as Model,
	                    CT.[Name] as Tipe,
	                    EC.IndonesianName as Warna,
	                    B.[Name] as Branch,
                        V.HasCustomer as CustomerAssign,
	                    V.RequestedDeliveryTime as RequestedPDD,
						L.Name as [Location],
						VVSE.Name as [Status],
                        VVSE.VehicleVoyageStatusEnumId as StatusId,
                        V.VehicleId as VehicleId,
                        V.EstimatedPDCIn as ETD
                    FROM 
                         Vehicle V join CarType CT on CT.Katashiki = V.Katashiki and V.Suffix = CT.Suffix join
	                     CarSeries CS on CS.CarSeriesCode = CT.CarSeriesCode join
	                     CarModel CM on CM.CarModelCode = CS.CarModelCode join
	                     ExteriorColor EC on EC.ExteriorColorCode = V.ExteriorColorCode join
	                     Branch B on B.BranchCode = V.BranchCode join
						 VoyageNodeSourceDetail VNSD on VNSD.VehicleId = V.VehicleId join
						 VehicleVoyageStatusEnum VVSE on VVSE.VehicleVoyageStatusEnumId = VNSD.VehicleVoyageStatusEnumId join
						 VoyageNodeSource VNS on VNS.VoyageNodeSourceId = VNSD.VoyageNodeSourceId join
						 VoyageNode VN on VN.VoyageNodeId = VNS.VoyageNodeId join
						 Voyage Voy on Voy.VoyageNumber = VN.VoyageNumber join
						 VoyageStatusEnum VSE on VOY.VoyageStatusEnumId = VSE.VoyageStatusEnumId join
						 Location L on L.LocationCode = VOY.DepartureLocationCode 
                    WHERE VOY.VoyageNumber = @voyage and VVSE.Name = 'Ported'
                    ORDER BY V.EstimatedPDCIn
                ", new { voyage = VoyageInput.ToUpper() })).Select(Q => {
                                Q.RequestedPDDString = Q.RequestedPDD.ToLocalTime().ToString("dd-MMM-yyyy");
                                Q.ETDString = Q.ETD.ToLocalTime().ToString("dd-MMM-yyyy");
                                return Q;
                }).ToList();

            return data;
        }

        //Get All The Voyage Data + Unit List data from DB
        public async Task<UnitAssignDataModel> GetAllData(string VoyageInput)
        {
            var AllData = new UnitAssignDataModel();

            var preBookedID = await this._context.VehicleVoyageStatusEnum.Where(Q => Q.Name == "Prebooked").Select(Q => Q.VehicleVoyageStatusEnumId).FirstOrDefaultAsync();
            var portedID = await this._context.VehicleVoyageStatusEnum.Where(Q => Q.Name == "Ported").Select(Q => Q.VehicleVoyageStatusEnumId).FirstOrDefaultAsync();
            var assignedID = await this._context.VehicleVoyageStatusEnum.Where(Q => Q.Name == "Assigned").Select(Q => Q.VehicleVoyageStatusEnumId).FirstOrDefaultAsync();

            _ = nameof(DeliveryVendorVehicle.Capacity);
            _ = nameof(Voyage.VoyageNumber);
            _ = nameof(VoyageNodeSourceDetail.VoyageNodeSourceId);
            _ = nameof(DeliveryMethod.Name);
            _ = nameof(DeliveryVendor.Name);
            _ = nameof(Voyage.DepartureDate);
            _ = nameof(VoyageNodeSourceDetail.VehicleVoyageStatusEnumId);

            var voyage = (await _context.Database.GetDbConnection().QueryAsync<UnitAssignVoyageModel>(
                @"
                SELECT
	                DVV.Capacity AS [CapacityVessel],
                    V.VoyageNumber as Voyage,
					VNSD.VoyageNodeSourceId AS VoyageNodeSourceId,
	                DM.Name AS [Vessel], 
	                DV.Name AS [Vendor],
	                V.DepartureDate AS [EstimationTimeDeparture],
	                COUNT(CASE WHEN VNSD.VehicleVoyageStatusEnumId=@assignedID THEN 1 END) AS [TotalUnitAssign],
	                COUNT(CASE WHEN VNSD.VehicleVoyageStatusEnumId=@portedID THEN 1 END) AS [TotalUnitPreBookedInPorted],
	                COUNT(CASE WHEN VNSD.VehicleVoyageStatusEnumId=@preBookedID THEN 1 END) AS [TotalUnitPreBookedNotPorted],
                    COUNT(CASE WHEN VNSD.VehicleVoyageStatusEnumId=@assignedID THEN 1 END) + COUNT(CASE WHEN VNSD.VehicleVoyageStatusEnumId=@portedID THEN 1 END) + COUNT(CASE WHEN VNSD.VehicleVoyageStatusEnumId=@preBookedID THEN 1 END) AS TotalAll
                FROM 
                     Voyage V JOIN DeliveryVendorVehicle DVV on V.DeliveryVendorVehicleId = DVV.DeliveryVendorVehicleId
			                  JOIN DeliveryVendor DV on DVV.DeliveryVendorCode = DV.DeliveryVendorCode
							  JOIN DeliveryMethod DM ON DVV.DeliveryMethodCode = DM.DeliveryMethodCode
							  JOIN VoyageNode VN on VN.VoyageNumber = V.VoyageNumber
							  JOIN VoyageNodeSource VNS on VNS.VoyageNodeId = VN.VoyageNodeId
							  JOIN VoyageNodeSourceDetail VNSD on VNSD.VoyageNodeSourceId = VNS.VoyageNodeSourceId
							  JOIN VoyageStatusEnum VSE on VSE.VoyageStatusEnumId = V.VoyageStatusEnumId 						
                WHERE VSE.Name = 'Created' and V.VoyageNumber = @voyage
                GROUP BY DVV.Capacity,V.VoyageNumber,DM.Name,DV.Name, V.DepartureDate, VNSD.VoyageNodeSourceId 
                ", new { preBookedID = preBookedID, portedID = portedID, assignedID = assignedID, voyage = VoyageInput.ToUpper() })).FirstOrDefault();

            AllData.AllVoyage = voyage;
            if (AllData.AllVoyage != null)
                AllData.AllUnit = await this.GetAllDetails(VoyageInput);

            return AllData;
        }

        //Get All Unit List By Voyage Number for The detail component
        public async Task<List<UnitAssignUnitListModel>> GetDetailByVoyage(string VoyageInput)
        {
            _ = nameof(Voyage.VoyageNumber);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(CarModel.Name);
            _ = nameof(CarType.Name);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(Branch.Name);
            _ = nameof(Vehicle.HasCustomer);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(Location.Name);
            _ = nameof(VoyageStatusEnum.Name);
            _ = nameof(VoyageStatusEnum.VoyageStatusEnumId);
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(Vehicle.EstimatedPDCIn);

            var data = (await _context.Database.GetDbConnection().QueryAsync<UnitAssignUnitListModel>(
                @"
                    SELECT
	                    V.FrameNumber as FrameNumber,
	                    V.Katashiki as Katashiki,
	                    V.Suffix as Suffix,
	                    CM.[Name] as Model,
	                    CT.[Name] as Tipe,
	                    EC.IndonesianName as Warna,
	                    B.[Name] as Branch,
                        V.HasCustomer as CustomerAssign,
	                    V.RequestedDeliveryTime as RequestedPDD,
						L.Name as [Location],
						VVSE.Name as [Status],
                        VVSE.VehicleVoyageStatusEnumId as StatusId,
                        V.VehicleId as VehicleId,
                        V.EstimatedPDCIn as ETD
                    FROM 
                         Vehicle V join CarType CT on CT.Katashiki = V.Katashiki and V.Suffix = CT.Suffix join
	                     CarSeries CS on CS.CarSeriesCode = CT.CarSeriesCode join
	                     CarModel CM on CM.CarModelCode = CS.CarModelCode join
	                     ExteriorColor EC on EC.ExteriorColorCode = V.ExteriorColorCode join
	                     Branch B on B.BranchCode = V.BranchCode join
						 VoyageNodeSourceDetail VNSD on VNSD.VehicleId = V.VehicleId join
						 VehicleVoyageStatusEnum VVSE on VVSE.VehicleVoyageStatusEnumId = VNSD.VehicleVoyageStatusEnumId join
						 VoyageNodeSource VNS on VNS.VoyageNodeSourceId = VNSD.VoyageNodeSourceId join
						 VoyageNode VN on VN.VoyageNodeId = VNS.VoyageNodeId join
						 Voyage Voy on Voy.VoyageNumber = VN.VoyageNumber join
						 VoyageStatusEnum VSE on VOY.VoyageStatusEnumId = VSE.VoyageStatusEnumId join
						 Location L on L.LocationCode = VOY.DepartureLocationCode 
                    WHERE VOY.VoyageNumber = @voyage and VVSE.Name IN ('Prebooked','Ported','Assigned')
                    ORDER BY V.EstimatedPDCIn
                ", new { voyage = VoyageInput.ToUpper() })).Select(Q => {
                            Q.RequestedPDDString = Q.RequestedPDD.ToLocalTime().ToString("dd-MMM-yyyy");
                            Q.ETDString = Q.ETD.ToLocalTime().ToString("dd-MMM-yyyy");
                            return Q;
                }).ToList();
            return data;
        }

        //Save The Data into the DB in the VehicleAssignmentPerVoyage
        public async Task<int> SaveData(UnitAssignDataModel data)
        {
            var assignedID = await this._context.VehicleVoyageStatusEnum.Where(Q => Q.Name == "Assigned").Select(Q => Q.VehicleVoyageStatusEnumId).FirstOrDefaultAsync();

            foreach (var item in data.AllUnit)
            {
                var OldData = await this._context.VoyageNodeSourceDetail.Where(Q => Q.VehicleId == item.VehicleId && Q.VoyageNodeSourceId == data.AllVoyage.VoyageNodeSourceId).FirstOrDefaultAsync();
                OldData.VehicleVoyageStatusEnumId = assignedID;
                OldData.UpdatedAt = DateTimeOffset.UtcNow;
                OldData.UpdatedBy = this._env.UserHumanName;

                this._context.VoyageNodeSourceDetail.Update(OldData);
            }
            return await this._context.SaveChangesAsync();
        }
    }
}
