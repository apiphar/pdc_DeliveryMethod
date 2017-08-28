using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class DeliveryUnitLoadingService
    {
        public DeliveryUnitLoadingService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvirontmentService)
        {
            LogisticDbContext = logisticDbContext;
            WebEnvirontmentService = webEnvirontmentService;

        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvirontmentService;

        /// <summary>
        /// Function untuk mendapatkan data Voyage yangstatusnya blm loaded
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryUnitLoadingViewModel>> GetUnitLoadingModel()
        {
            _ = nameof(DeliveryVendorVehicle.Capacity);
            _ = nameof(Voyage.VoyageNumber);
            _ = nameof(VoyageNodeSourceDetail.VoyageNodeSourceId);
            _ = nameof(DeliveryMethod.Name);
            _ = nameof(DeliveryVendor.Name);
            _ = nameof(Voyage.DepartureDate);
            _ = nameof(VoyageNodeSourceDetail.VehicleVoyageStatusEnumId);
            var unitLoadingData = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryUnitLoadingViewModel>($@"
 SELECT 
                 DVV.Capacity AS [Capacity], 
                 V.VoyageNumber AS [VoyageNumber],  
     VNSD.VoyageNodeSourceId AS VoyageNodeSourceId, 
                 DM.Name AS [Vessel],  
                 DV.Name AS [Vendor], 
                 V.DepartureDate AS [EstimationDeparture], 
                 COUNT(CASE WHEN VVSE.Name='Assigned' THEN 1 END) AS [TotalUnitAssign], 
                 COUNT(CASE WHEN VVSE.Name='Ported' THEN 1 END) AS [TotalUnitPreBookedInPorted], 
                 COUNT(CASE WHEN VVSE.Name='Prebooked' THEN 1 END) AS [TotalUnitPreBookedNotPorted]                  
                FROM  
                     Voyage V JOIN DeliveryVendorVehicle DVV on V.DeliveryVendorVehicleId = DVV.DeliveryVendorVehicleId 
                     JOIN DeliveryVendor DV on DVV.DeliveryVendorCode = DV.DeliveryVendorCode 
         JOIN DeliveryMethod DM ON DVV.DeliveryMethodCode = DM.DeliveryMethodCode 
         JOIN VoyageNode VN on VN.VoyageNumber = V.VoyageNumber 
         JOIN VoyageNodeSource VNS on VNS.VoyageNodeId = VN.VoyageNodeId 
         JOIN VoyageNodeSourceDetail VNSD on VNSD.VoyageNodeSourceId = VNS.VoyageNodeSourceId 
         JOIN VoyageStatusEnum VSE on VSE.VoyageStatusEnumId = V.VoyageStatusEnumId    
		 JOIN VehicleVoyageStatusEnum VVSE ON VNSD.VehicleVoyageStatusEnumId = VVSE.VehicleVoyageStatusEnumId
                WHERE VSE.Name IN('Assign','Loading')
                GROUP BY DVV.Capacity,V.VoyageNumber,DM.Name,DV.Name, V.DepartureDate,VNSD.VoyageNodeSourceId
")).ToList();
            return unitLoadingData;

        }

        /// <summary>
        /// Function untuk mendapatkan data detail unit car yang statusnya sudah Loaded
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryUnitLoadingDetailViewModel>> GetAllUnitLoadingDetailModel(string voyageNumber)
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
            var unitLoadingDetailModels = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryUnitLoadingDetailViewModel>($@"
SELECT  
                     V.FrameNumber as [FrameNumber], 
                     V.Katashiki as [Katashiki], 
                     V.Suffix as [Suffix], 
                     CM.[Name] as [Model], 
                     CT.[Name] as [Tipe], 
                     EC.IndonesianName as [Warna], 
                     B.[Name] as [Branch], 
                        V.HasCustomer as [CustomerAssign], 
                     V.RequestedDeliveryTime as [RequestedPDD], 
      L.Name as [Location], 
      VVSE.Name as [Status], 
                        VSE.VoyageStatusEnumId as [StatusId], 
                        V.VehicleId as [VehicleId], 
                        V.EstimatedPDCIn as [EstimatedPDCIn] 
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
                    WHERE VOY.VoyageNumber = @voyageNumber and VVSE.Name != 'Departed' 
                    ORDER BY V.EstimatedPDCIn
", new { voyageNumber = voyageNumber })).ToList();
            return unitLoadingDetailModels;
        }

        /// <summary>
        /// Function untuk mendapatkan semua frame Number yang ada berdasarkan voyage number yang sudah di dapatkan
        /// </summary>
        /// <param name="voyageNumber"></param>
        /// <returns></returns>
        public async Task<List<DeliveryUnitLoadingFrameNumberInput>> GetFrameNumber(string voyageNumber)
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
            var frameNumbers = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryUnitLoadingFrameNumberInput>($@"
SELECT  
                     V.FrameNumber as [FrameNumber], 
                     V.Katashiki as [Katashiki], 
                     V.Suffix as [Suffix], 
                     CM.[Name] as [Model], 
                     CT.[Name] as [Tipe], 
                     EC.IndonesianName as [Warna], 
                     B.[Name] as [Branch], 
                        V.HasCustomer as [CustomerAssign], 
                     V.RequestedDeliveryTime as [RequestedDeliveryTime],
      VVSE.Name as [Status],                     
                        V.VehicleId as [VehicleId], 
                        V.EstimatedPDCIn as [EstimatedPDCIn] 
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
                    WHERE VOY.VoyageNumber = @voyageNumber and VVSE.Name = 'Assigned' 
                    ORDER BY V.EstimatedPDCIn
", new { voyageNumber = voyageNumber })).ToList();
            var frameNumbe = frameNumbers;
            return frameNumbers;
        }
        /// <summary>
        /// Function untuk merubah status dari setiap voyage menjadi loaded
        /// </summary>
        /// <param name="voyageNumber"></param>
        /// <returns></returns>
        public async Task LoadedData(DeliveryUnitLoadingFrameNumberUpdate data)
        {
            await LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await LogisticDbContext.Database.BeginTransactionAsync())
                {
                    var idSelected = await LogisticDbContext.VehicleVoyageStatusEnum.Where(q => q.Name == "Assigned")
                    .Select(b => b.VehicleVoyageStatusEnumId).FirstOrDefaultAsync();

                    var idUpdate = await LogisticDbContext.VehicleVoyageStatusEnum.Where(q => q.Name == "Loading")
                    .Select(b => b.VehicleVoyageStatusEnumId).FirstOrDefaultAsync();

                    var idVoyageUpdate = await LogisticDbContext.VoyageStatusEnum.Where(q => q.Name == "Loading")
                    .Select(b => b.VoyageStatusEnumId).FirstOrDefaultAsync();
                    var listUpdate = new List<VoyageNodeSourceDetail>();
                    foreach (var selected in data.VehicleId)
                    {
                        var vehicleSelected = await LogisticDbContext.VoyageNodeSourceDetail
                                       .FirstOrDefaultAsync(q => q.VehicleId == selected && q.VehicleVoyageStatusEnumId == idSelected);
                        vehicleSelected.VehicleVoyageStatusEnumId = idUpdate;
                        vehicleSelected.UpdatedAt = DateTimeOffset.UtcNow;
                        vehicleSelected.UpdatedBy = WebEnvirontmentService.UserHumanName;
                        listUpdate.Add(vehicleSelected);
                    }
                    LogisticDbContext.VoyageNodeSourceDetail.UpdateRange(listUpdate);
                    var voyageNumb = await LogisticDbContext.Voyage.FirstOrDefaultAsync(q => q.VoyageNumber == data.VoyageNumber);
                    voyageNumb.VoyageStatusEnumId = idVoyageUpdate;
                    LogisticDbContext.Voyage.Update(voyageNumb);
                    await LogisticDbContext.SaveChangesAsync();
                    transaction.Commit();
                }
            });
        }
    }
}
