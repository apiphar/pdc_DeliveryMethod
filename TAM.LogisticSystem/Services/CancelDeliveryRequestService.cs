using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class CancelDeliveryRequestService
    {
        public CancelDeliveryRequestService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService, IDistributedCache iDistributedCache)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
            this.DistributedCache = iDistributedCache;

        }

        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;
        private readonly IDistributedCache DistributedCache;

        /// <summary>
        /// Get All Data
        /// </summary>
        /// <returns></returns>
        /// 

        public async Task<CancelDeliveryRequestPageViewModel> GetAllData()
        {
            var data = new CancelDeliveryRequestPageViewModel()
            {
                CancelDeliveryRequest = await this.InitiateCancelDeliveryRequest(),
                CancelDeliveryRequestLocation = await this.GetAllLocation()
            };

            return data;
        }

        /// <summary>
        /// Get All Location untuk mendapatkan Name & Address & Tipe
        /// </summary>
        /// <returns></returns>
        public async Task<List<CancelDeliveryRequestLocationModel>> GetAllLocation()
        {
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.LocationTypeCode);
            _ = nameof(LocationType.Name);
            _ = nameof(Location.Name);
            _ = nameof(Location.Address);
            _ = nameof(LocationType.LocationTypeCode);

            var locationList = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<CancelDeliveryRequestLocationModel>(@"
SELECT 
	l.LocationCode,
	l.LocationTypeCode,
	lt.Name AS [LocationTypeName],
	l.Name AS [LocationName],
	l.Address AS [LocationAddress]
FROM Location l JOIN LocationType lt
	ON l.LocationTypeCode = lt.LocationTypeCode
")).ToList();

            return locationList;
        }

        /// <summary>
        /// Get All Delivery Request
        /// </summary>
        /// <returns></returns>
        public async Task<List<CancelDeliveryRequestViewModel>> GetAllCancelDeliveryRequest()
        {
            _ = nameof(DeliveryRequest.DeliveryRequestNumber);
            _ = nameof(DeliveryRequest.CancelledAt);
            _ = nameof(DeliveryRequest.DeliveryRequestTypeEnumId);
            _ = nameof(DeliveryRequest.DeliveryRequestTransitTypeEnumId);
            _ = nameof(DeliveryRequest.RequestedDeliveryTimeToBranch);
            _ = nameof(DeliveryRequest.PickUpDate);
            _ = nameof(DeliveryRequest.PickUpIdentityIsKtp);
            _ = nameof(DeliveryRequest.PickUpIdentityCardNumber);
            _ = nameof(DeliveryRequest.PickUpIdentityName);
            _ = nameof(DeliveryRequest.PickUpConfirmationCode);
            _ = nameof(DeliveryRequest.DirectEstimatedPDCOut);
            _ = nameof(DeliveryRequest.DirectCustomerName);
            _ = nameof(DeliveryRequest.DirectCustomerAddress);
            _ = nameof(DeliveryRequest.DirectCustomerCity);
            _ = nameof(DeliveryRequest.DirectSalesmanName);
            _ = nameof(DeliveryRequest.DirectSalesmanContactNumber);
            _ = nameof(DeliveryRequest.TransitLocationCode);
            _ = nameof(DeliveryRequest.TransitReturnDate);
            _ = nameof(DeliveryRequest.TransitReturnToOtherPdc);
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(ExteriorColor.EnglishName);
            _ = nameof(CarType.Name);
            _ = nameof(CarModel.Name);
            _ = nameof(Branch.Name);
            _ = nameof(Vehicle.HasCustomer);
            _ = nameof(Vehicle.EstimatedPDCIn);
            _ = nameof(Vehicle.RequestedDeliveryTime);
            _ = nameof(ProcessMaster.Name);
            _ = nameof(Location.Name);
            _ = nameof(VehicleRouting.ScanTime);
            _ = nameof(ExteriorColor.ExteriorColorCode);
            _ = nameof(Vehicle.ExteriorColorCode);
            _ = nameof(Branch.BranchCode);
            _ = nameof(Vehicle.BranchCode);
            _ = nameof(VehicleRouting.VehicleId);
            _ = nameof(VehicleRouting.ProcessMasterCode);
            _ = nameof(ProcessMaster.ProcessMasterCode);
            _ = nameof(VehicleRouting.LocationCode);
            _ = nameof(Location.LocationCode);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(CarType.Katashiki);
            _ = nameof(CarType.Suffix);
            _ = nameof(CarSeries.CarSeriesCode);
            _ = nameof(CarType.CarSeriesCode);
            _ = nameof(CarSeries.CarModelCode);
            _ = nameof(CarModel.CarModelCode);
            _ = nameof(DeliveryRequest.VehicleId);

            var deliveryRequest = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<CancelDeliveryRequestViewModel>(@"

SELECT 
	dr.DeliveryRequestNumber,
	dr.CancelledAt,
    dr.CreatedAt,
	dr.DeliveryRequestTypeEnumId AS [DeliveryRequestTypeId],
    dr.DeliveryRequestTransitTypeEnumId AS [DeliveryRequestTransitTypeId],
	dr.RequestedDeliveryTimeToBranch,
	dr.PickUpDate,
	dr.PickUpIdentityIsKtp,
	dr.PickUpIdentityCardNumber AS [DriverId],
	dr.PickUpIdentityName AS [DriverName],
	dr.PickUpConfirmationCode AS [ConfirmationCode],
	dr.DirectEstimatedPDCOut AS [EstimasiPDCOut],
	dr.DirectCustomerName AS [CustomerName],
	dr.DirectCustomerAddress AS [CustomerAddress],
	dr.DirectCustomerCity AS [CustomerCity],
	dr.DirectSalesmanName AS [SalesmanName],
	dr.DirectSalesmanContactNumber AS [SalesmanContactNo],
	dr.TransitLocationCode AS [LocationCode],
	dr.TransitReturnDate,
	dr.TransitReturnToOtherPdc AS [OtherPdcLocationCode],
    v.VehicleId,
    v.FrameNumber,
    v.Katashiki,
    v.Suffix,
    ec.EnglishName AS [Warna],
    ct.Name AS [Tipe],
    cm.Name AS [Model],
    b.Name AS [Branch],
    v.HasCustomer AS [CustomerAssign],
    v.EstimatedPDCIn,
    v.RequestedDeliveryTime AS [RequestedPDD],
    pm.Name AS [PosisiTerakhir],
    l.Name AS [LokasiTerakhir],
    vr.ScanTime
FROM Vehicle v
JOIN ExteriorColor ec
    ON v.ExteriorColorCode = ec.ExteriorColorCode
JOIN Branch b
    ON v.BranchCode = b.BranchCode
JOIN VehicleRouting vr
    ON vr.VehicleId = v.VehicleId
JOIN ProcessMaster pm
    ON vr.ProcessMasterCode = pm.ProcessMasterCode
JOIN Location l
    ON vr.LocationCode = l.LocationCode
JOIN CarType ct
    ON v.Katashiki = ct.Katashiki
    AND v.Suffix = ct.Suffix
JOIN CarSeries cs
    ON ct.CarSeriesCode = cs.CarSeriesCode
JOIN CarModel cm
    ON cs.CarModelCode = cm.CarModelCode
JOIN DeliveryRequest dr
	ON dr.VehicleId = v.VehicleId
WHERE vr.ScanTime IS NULL
")).ToList();
            return deliveryRequest;
        }
        /// <summary>
        /// Get Delivery Request dari Redis
        /// </summary>
        /// <returns></returns>
        public async Task<List<CancelDeliveryRequestViewModel>> InitiateCancelDeliveryRequest()
        {
            var cancelDeliveryRequestCache = await this.DistributedCache.GetObjectAsync<List<CancelDeliveryRequestViewModel>>("BatalDeliveryRequest_CancelDeliveryRequestViewModel");
            if (cancelDeliveryRequestCache == null)
            {
                var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

                await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);
                cancelDeliveryRequestCache = cancelDeliveryRequest;
            }

            return cancelDeliveryRequestCache;
        }
        /// <summary>
        /// Get All Delivery Request (model yang dipakai untuk update Redis Delivery Request)
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryRequestModel>> GetAllDeliveryRequest()
        {
            _ = nameof(DeliveryRequest.DeliveryRequestNumber);
            _ = nameof(DeliveryRequest.CancelledAt);
            _ = nameof(DeliveryRequest.VehicleId);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Branch.BranchCode);
            _ = nameof(DeliveryRequest.DeliveryRequestTypeEnumId);
            _ = nameof(DeliveryRequest.CreatedAt);
            _ = nameof(Vehicle.VehicleId);
            _ = nameof(Vehicle.BranchCode);

            var deliveryRequest = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryRequestModel>(@"

SELECT 
	dr.DeliveryRequestNumber,
    dr.CancelledAt,
    dr.VehicleId,
    v.FrameNumber,
	b.BranchCode,
	dr.DeliveryRequestTypeEnumId,
	dr.CreatedAt
FROM DeliveryRequest dr
	JOIN Vehicle v ON v.VehicleId = dr.VehicleId
	JOIN Branch b ON b.BranchCode = v.BranchCode")).ToList();

            return deliveryRequest;
        }

        /// <summary>
        /// Cancel Delivery Request
        /// </summary>
        /// <param name="deliveryRequestNumber"></param>
        /// <returns></returns>
        public async Task<int> CancelDeliveryRequest(string deliveryRequestNumber)
        {
            var deliveryRequest = await LogisticDbContext.DeliveryRequest
                .Where(Q => Q.DeliveryRequestNumber == deliveryRequestNumber)
                .FirstOrDefaultAsync();

            if (deliveryRequest == null)
            {
                return 2;
            }

            if (deliveryRequest.CancelledAt != null)
            {
                return 3;
            }

            deliveryRequest.CancelledAt = DateTimeOffset.UtcNow;
            deliveryRequest.UpdatedAt = DateTimeOffset.UtcNow;
            deliveryRequest.UpdatedBy = WebEnvironmentService.UserHumanName;

            

            LogisticDbContext.DeliveryRequest.Update(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();
            //get semua data (model) DR yang kemudian digunakan untuk update Redis DR
            var deliveryRequestData = await this.GetAllDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);
            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);

            return 0;
        }
    }
}
