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
    public class DeliveryRequestService
    {
        public DeliveryRequestService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService, IDistributedCache distributedCache)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
            this.DistributedCache = distributedCache;
        }

        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;
        private readonly IDistributedCache DistributedCache;

        /// <summary>
        /// Generate Confirmation Code randomly
        /// </summary>
        /// <returns></returns>
        public string GenerateConfirmationCode()
        {
            var charList = "0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = charList[random.Next(charList.Length)];
            }

            var deliveryRequestSelfPickConfirmationCode = new string(stringChars);

            return deliveryRequestSelfPickConfirmationCode;
        }
        /// <summary>
        /// Generate Return Pdc Date in Self Pick To Others
        /// </summary>
        /// <param name="deliveryTransitToOthers"></param>
        /// <returns></returns>
        public DateTime GenerateReturnPdcDate(ReturnPdcDateModel returnPdcDateModel)
        {
            var returnPdcDate = returnPdcDateModel.Date;

            returnPdcDate = returnPdcDate.AddDays(returnPdcDateModel.LeadTimeDay);
            returnPdcDate = returnPdcDate.AddHours(returnPdcDateModel.LeadTimeHour);
            returnPdcDate = returnPdcDate.AddMinutes(returnPdcDateModel.LeadTimeMinute);

            return returnPdcDate;
        }
        /// <summary>
        /// Generate Location Type in Transit To Others
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryRequestLocationModel>> GetLocationType()
        {
            var locationType = await LogisticDbContext.LocationType
                .Where(Q => Q.Name == "Karoseri" || Q.Name == "Body Paint" || Q.Name == "Borrowing")
                .Select(Q => new DeliveryRequestLocationModel
                {
                    LocationType = Q.Name,
                    LocationTypeCode = Q.LocationTypeCode
                }).ToListAsync();

            return locationType;
        }
        /// <summary>
        /// Get All Location Name from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryRequestLocationNameModel>> GetAllLocationName()
        {
            _ = nameof(Location.Name);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.LocationTypeCode);
            _ = nameof(LocationType.LocationTypeCode);

            var locationName = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryRequestLocationNameModel>(@"

SELECT
	l.Name AS [LocationName],
    l.LocationTypeCode,
    l.LocationCode
FROM Location l JOIN LocationType lt
	ON l.LocationTypeCode = lt.LocationTypeCode")).ToList();

            return locationName;
        }
        /// <summary>
        /// Get All Location Address from DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryRequestTransitToOthersModel>> GetAllLocationAddress()
        {

            var locationAddress = await LogisticDbContext.Location
                .Select(Q => new DeliveryRequestTransitToOthersModel
                {
                    LocationAddress = Q.Address,
                    LocationCode = Q.LocationCode
                }).ToListAsync();

            return locationAddress;
        }
        /// <summary>
        /// Get Location for Return To Others PDC Combo box
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryRequestOtherPdcLocationModel>> GetOtherPdcLocation()
        {
            var otherPdcLocation = await LogisticDbContext.Location
                .Select(Q => new DeliveryRequestOtherPdcLocationModel
                {
                    OtherPdcLocation = Q.Name,
                    OtherPdcLocationCode = Q.LocationCode
                }).ToListAsync();

            return otherPdcLocation;
        }
        /// <summary>
        /// Get All Data for Delivery Request ()
        /// </summary>
        /// <returns></returns>
        public async Task<DeliveryRequestPageViewModel> GetAllDeliveryRequestPageView()
        {
            var data = new DeliveryRequestPageViewModel
            {
                DeliveryRequest = await this.InitiateDeliveryRequest(),
                DeliveryRequestCar = await this.GetAllDeliveryCar(),
                DeliveryRequestLocationType = await this.GetLocationType(),
                DeliveryRequestLocationName = await this.GetAllLocationName(),
                DeliveryRequestLocationAddress = await this.GetAllLocationAddress(),
                DeliveryRequestOtherPdcLocation = await this.GetOtherPdcLocation()
            };

            return data;
        }

        /// <summary>
        /// Get All Car from DB WHERE Car's ScanTime is NULL => untuk dapetin Posisi Terakhir & Lokasi Terakhir
        /// </summary>
        /// <param name="frameNumber"></param>
        /// <returns></returns>
        public async Task<List<DeliveryRequestCarModel>> GetAllDeliveryCar()
        {
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

            var vehicle = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryRequestCarModel>(@"

SELECT 
    v.FrameNumber,
    v.VehicleId,
    v.Katashiki,
    v.Suffix,
    ec.EnglishName AS [Warna],
    ct.Name AS [Tipe],
    cm.Name AS [Model],
    b.Name AS [Branch],
    b.BranchCode,
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
WHERE ScanTime IS NULL")).ToList();
            return vehicle;
        }


        /// <summary>
        /// Get All Delivery Request Data (untuk validasi Frame Number)
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
        /// Get Delivery Request Data from Redis
        /// </summary>
        /// <returns></returns>

        public async Task<List<DeliveryRequestModel>> InitiateDeliveryRequest()
        {
            var deliveryRequestCache = await this.DistributedCache.GetObjectAsync<List<DeliveryRequestModel>>("DeliveryRequest_DeliveryRequestModel");
            if (deliveryRequestCache == null)
            {
                var deliveryRequest = await this.GetAllDeliveryRequest();

                await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequest);
                deliveryRequestCache = deliveryRequest;
            } 

            return deliveryRequestCache;
        }

        /// <summary>
        /// Get Car Data from Redis
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryRequestCarModel>> InitiateDeliveryCar()
        {
            var deliveryRequestCarCache = await this.DistributedCache.GetObjectAsync<List<DeliveryRequestCarModel>>("DeliveryRequest_DeliveryRequestCarModel");
            if (deliveryRequestCarCache == null)
            {
                var deliveryRequestCar = await this.GetAllDeliveryCar();

                await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestCarModel", deliveryRequestCar);
                deliveryRequestCarCache = deliveryRequestCar;
            }

            return deliveryRequestCarCache;
        }

        /// <summary>
        /// Get Sequential Number from Redis
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="tipeDR"></param>
        /// <returns></returns>
        public async Task<int> GetSequentialNumber(string branchCode, string tipeDR)
        {
            var date = DateTime.Today;

            var sequentialNumber = await this.DistributedCache.GetObjectAsync<int>($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]");

            if (sequentialNumber == 0)
            {
                sequentialNumber = 1;
            }
            else
            {
                sequentialNumber += 1;
            }

            return sequentialNumber;
        }

        /// <summary>
        /// Check if Frame No. already requested
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public async Task<bool> IsAlreadyRequested(int vehicleId)
        {
            var isAlreadyRequested = await LogisticDbContext.DeliveryRequest
                .Where(Q => Q.VehicleId == vehicleId && Q.CancelledAt == null)
                .FirstOrDefaultAsync();

            if (isAlreadyRequested != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get All Delivery Request Data Where Car's Scan Time is NULL (dipake untuk update Redis di Batal DR)
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
        /// Create Delivery Request - Normal
        /// </summary>
        /// <param name="deliveryRequestNormalCreateModel"></param>
        /// <returns></returns>
        public async Task<int> CreateDeliveryNormalRequest(DeliveryRequestNormalCreateModel model)
        {
            var vehicleId = await LogisticDbContext.Vehicle
           .Where(Q => Q.FrameNumber == model.DeliveryRequest.FrameNumber)
           .Select(Q => Q.VehicleId)
           .FirstOrDefaultAsync();

            if (vehicleId == 0)
            {
                return 2;
            }

            if (await this.IsAlreadyRequested(vehicleId))
            {
                return 9;
            }

            var deliveryRequest = new DeliveryRequest
            {
                DeliveryRequestNumber = model.DeliveryRequest.DeliveryRequestNumber.ToUpper(),
                VehicleId = vehicleId,
                DeliveryRequestTypeEnumId = 1,
                RequestedDeliveryTimeToBranch = model.DeliveryRequestNormal.RequestedDeliveryTimeToBranch.ToUniversalTime(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.WebEnvironmentService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName
            };

            LogisticDbContext.DeliveryRequest.Add(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            var branchCode = model.DeliveryRequest.BranchCode;
            var tipeDR = "NR";
            var date = DateTime.Today;

            //get data untuk update ke Redis (Delivery Request)
            var deliveryRequestData = await this.GetAllDeliveryRequest();
            //get data untuk update ke Redis (Batal Delivery Request)
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);
            await this.DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]", model.DeliveryRequest.SequentialNumber);
            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);

            return 0;
        }
        /// <summary>
        /// Create Delivery Request - Self Pick
        /// </summary>
        /// <param name="deliveryRequestSelfPickCreateModel"></param>
        /// <returns></returns>
        public async Task<int> CreateDeliverySelfPickRequest(DeliveryRequestSelfPickCreateModel model)
        {

            var vehicleId = await LogisticDbContext.Vehicle
           .Where(Q => Q.FrameNumber == model.DeliveryRequest.FrameNumber)
           .Select(Q => Q.VehicleId)
           .FirstOrDefaultAsync();

            if (vehicleId == 0)
            {
                return 2;
            }

            if (await this.IsAlreadyRequested(vehicleId))
            {
                return 9;
            }

            var isKtp = true;

            if (model.DeliveryRequestSelfPick.DriverType == "SIM")
            {
                isKtp = false;
            }

            var deliveryRequest = new DeliveryRequest
            {
                DeliveryRequestNumber = model.DeliveryRequest.DeliveryRequestNumber.ToUpper(),
                VehicleId = vehicleId,
                DeliveryRequestTypeEnumId = 2,
                PickUpConfirmationCode = model.DeliveryRequestSelfPick.ConfirmationCode.ToUpper(),
                PickUpIdentityIsKtp = isKtp,
                PickUpDate = model.DeliveryRequestSelfPick.PickUpDate.ToUniversalTime(),
                PickUpIdentityCardNumber = model.DeliveryRequestSelfPick.DriverId.ToUpper(),
                PickUpIdentityName = model.DeliveryRequestSelfPick.DriverName.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.WebEnvironmentService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName
            };

            LogisticDbContext.DeliveryRequest.Add(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            var branchCode = model.DeliveryRequest.BranchCode;
            var tipeDR = "SP";
            var date = DateTime.Today;

            var deliveryRequestData = await this.GetAllDeliveryRequest();
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);
            await this.DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]", model.DeliveryRequest.SequentialNumber);
            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);

            return 0;
        }
        /// <summary>
        /// Create Delivery Request - Direct Delivery
        /// </summary>
        /// <param name="deliveryRequestDirectDeliveryCreateModel"></param>
        /// <returns></returns>
        public async Task<int> CreateDeliveryDirectDeliveryRequest(DeliveryRequestDirectDeliveryCreateModel model)
        {
            var vehicleId = await LogisticDbContext.Vehicle
           .Where(Q => Q.FrameNumber == model.DeliveryRequest.FrameNumber)
           .Select(Q => Q.VehicleId)
           .FirstOrDefaultAsync();

            if (vehicleId == 0)
            {
                return 2;
            }

            if (await this.IsAlreadyRequested(vehicleId))
            {
                return 9;
            }

            var deliveryRequest = new DeliveryRequest
            {
                DeliveryRequestNumber = model.DeliveryRequest.DeliveryRequestNumber.ToUpper(),
                VehicleId = vehicleId,
                DeliveryRequestTypeEnumId = 3,
                DirectEstimatedPDCOut = model.DeliveryRequestDirectDelivery.EstimasiPDCOut.ToUniversalTime(),
                DirectCustomerName = model.DeliveryRequestDirectDelivery.CustomerName.ToUpper(),
                DirectCustomerAddress = model.DeliveryRequestDirectDelivery.CustomerAddress.ToUpper(),
                DirectCustomerCity = model.DeliveryRequestDirectDelivery.CustomerCity.ToUpper(),
                DirectSalesmanName = model.DeliveryRequestDirectDelivery.SalesmanName.ToUpper(),
                DirectSalesmanContactNumber = model.DeliveryRequestDirectDelivery.SalesmanContactNo.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.WebEnvironmentService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName
            };

            LogisticDbContext.DeliveryRequest.Add(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            var branchCode = model.DeliveryRequest.BranchCode;
            var tipeDR = "DD";
            var date = DateTime.Today;

            var deliveryRequestData = await this.GetAllDeliveryRequest();
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);
            await this.DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]", model.DeliveryRequest.SequentialNumber);
            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);

            return 0;
        }
        /// <summary>
        /// Create Delivery Request - Transit To Others - Self Pick To Others
        /// </summary>
        /// <param name="deliveryRequestTransitToOthersSelfPickToOthersCreateModel"></param>
        /// <returns></returns>
        public async Task<int> CreateDeliveryTransitToOthersSelfPickToOthersRequest(DeliveryRequestTransitToOthersSelfPickToOthersCreateModel model)
        {
            var vehicleId = await LogisticDbContext.Vehicle
            .Where(Q => Q.FrameNumber == model.DeliveryRequest.FrameNumber)
            .Select(Q => Q.VehicleId)
            .FirstOrDefaultAsync();

            if (vehicleId == 0)
            {
                return 2;
            }

            if (await this.IsAlreadyRequested(vehicleId))
            {
                return 9;
            }

            var isKtp = true;

            if (model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliverySelfPickToOthers.DriverType == "SIM")
            {
                isKtp = false;
            }

            var deliveryRequest = new DeliveryRequest
            {
                DeliveryRequestNumber = model.DeliveryRequest.DeliveryRequestNumber.ToUpper(),
                VehicleId = vehicleId,
                DeliveryRequestTypeEnumId = 4,
                TransitLocationCode = model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliveryTransitToOthers.DeliveryLocationName.LocationCode.ToUpper(),
                PickUpDate = model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliveryTransitToOthers.PickUpDate.ToUniversalTime(),
                DeliveryRequestTransitTypeEnumId = 4,
                PickUpIdentityCardNumber = model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliverySelfPickToOthers.DriverId.ToUpper(),
                PickUpIdentityIsKtp = isKtp,
                PickUpConfirmationCode = model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliverySelfPickToOthers.ConfirmationCode.ToUpper(),
                PickUpIdentityName = model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliverySelfPickToOthers.DriverName.ToUpper(),
                TransitReturnDate = model.DeliveryRequestTransitToOthersSelfPickToOthers.DeliverySelfPickToOthers.ReturnPdcDate.ToUniversalTime(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.WebEnvironmentService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName
            };

            LogisticDbContext.DeliveryRequest.Add(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            var branchCode = model.DeliveryRequest.BranchCode;
            var tipeDR = "TO";
            var date = DateTime.Today;

            var deliveryRequestData = await this.GetAllDeliveryRequest();
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);
            await this.DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]", model.DeliveryRequest.SequentialNumber);
            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);

            return 0;
        }
        /// <summary>
        /// Create Delivery Request - Transit To Others - Normal - Return To PDC / Return To Others PDC
        /// </summary>
        /// <param name="deliveryRequestTransitToOthersNormalReturnToPdcCreateModel"></param>
        /// <returns></returns>
        public async Task<int> CreateDeliveryTransitToOthersNormalReturnToPdcRequest(DeliveryRequestTransitToOthersNormalReturnToPdcCreateModel model)
        {
            var vehicleId = await LogisticDbContext.Vehicle
           .Where(Q => Q.FrameNumber == model.DeliveryRequest.FrameNumber)
           .Select(Q => Q.VehicleId)
           .FirstOrDefaultAsync();

            if (vehicleId == 0)
            {
                return 2;
            }

            if (await this.IsAlreadyRequested(vehicleId))
            {
                return 9;
            }

            var deliveryRequest = new DeliveryRequest
            {
                DeliveryRequestNumber = model.DeliveryRequest.DeliveryRequestNumber.ToUpper(),
                VehicleId = vehicleId,
                DeliveryRequestTypeEnumId = 4,
                TransitLocationCode = model.DeliveryRequestTransitToOthersNormalReturnToPdc.DeliveryTransitToOthers.DeliveryLocationName.LocationCode.ToUpper(),
                PickUpDate = model.DeliveryRequestTransitToOthersNormalReturnToPdc.DeliveryTransitToOthers.PickUpDate.ToUniversalTime(),
                DeliveryRequestTransitTypeEnumId = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.WebEnvironmentService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName
            };

            if (model.DeliveryRequestTransitToOthersNormalReturnToPdc.DeliveryTransitToOthersNormalModel.DeliveryTransitType == "ReturnToOthersPDC")
            {
                deliveryRequest.TransitReturnToOtherPdc = model.DeliveryRequestTransitToOthersNormalReturnToPdc.DeliveryTransitToOthersNormalModel.DeliveryOtherPdcLocation.OtherPdcLocationCode.ToUpper();
                deliveryRequest.DeliveryRequestTransitTypeEnumId = 2;
            }

            LogisticDbContext.DeliveryRequest.Add(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            var branchCode = model.DeliveryRequest.BranchCode;
            var tipeDR = "TO";
            var date = DateTime.Today;

            var deliveryRequestData = await this.GetAllDeliveryRequest();
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);
            await this.DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]", model.DeliveryRequest.SequentialNumber);
            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);

            return 0;
        }
        /// <summary>
        /// Create Delivery Request - Transit To Others - Normal - Self Pick From Others
        /// </summary>
        /// <param name="deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel"></param>
        /// <returns></returns>
        public async Task<int> CreateDeliveryTransitToOthersNormalSelfPickFromOthersRequest(DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel model)
        {
            var vehicleId = await LogisticDbContext.Vehicle
           .Where(Q => Q.FrameNumber == model.DeliveryRequest.FrameNumber)
           .Select(Q => Q.VehicleId)
           .FirstOrDefaultAsync();

            if (vehicleId == 0)
            {
                return 2;
            }

            if (await this.IsAlreadyRequested(vehicleId))
            {
                return 9;
            }

            var isKtp = true;

            if (model.DeliveryRequestTransitToOthersNormalSelfPickFromOthers.DeliveryRequestTransitToOthersNormalModel.DeliverySelfPickFromOthers.DriverType == "SIM")
            {
                isKtp = false;
            }

            var deliveryRequest = new DeliveryRequest
            {
                DeliveryRequestNumber = model.DeliveryRequest.DeliveryRequestNumber.ToUpper(),
                VehicleId = vehicleId,
                DeliveryRequestTypeEnumId = 4,
                TransitLocationCode = model.DeliveryRequestTransitToOthersNormalSelfPickFromOthers.DeliveryTransitToOthers.DeliveryLocationName.LocationCode.ToUpper(),
                PickUpDate = model.DeliveryRequestTransitToOthersNormalSelfPickFromOthers.DeliveryTransitToOthers.PickUpDate.ToUniversalTime(),
                DeliveryRequestTransitTypeEnumId = 3,
                PickUpIdentityCardNumber = model.DeliveryRequestTransitToOthersNormalSelfPickFromOthers.DeliveryRequestTransitToOthersNormalModel.DeliverySelfPickFromOthers.DriverId.ToUpper(),
                PickUpIdentityIsKtp = isKtp,
                PickUpIdentityName = model.DeliveryRequestTransitToOthersNormalSelfPickFromOthers.DeliveryRequestTransitToOthersNormalModel.DeliverySelfPickFromOthers.DriverName.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.WebEnvironmentService.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName
            };

            LogisticDbContext.DeliveryRequest.Add(deliveryRequest);
            await LogisticDbContext.SaveChangesAsync();

            var branchCode = model.DeliveryRequest.BranchCode;
            var tipeDR = "TO";
            var date = DateTime.Today;

            var deliveryRequestData = await this.GetAllDeliveryRequest();
            var cancelDeliveryRequest = await this.GetAllCancelDeliveryRequest();

            await this.DistributedCache.SetObjectAsync("DeliveryRequest_DeliveryRequestModel", deliveryRequestData);
            await this.DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {date}]", model.DeliveryRequest.SequentialNumber);
            await this.DistributedCache.SetObjectAsync("BatalDeliveryRequest_CancelDeliveryRequestViewModel", cancelDeliveryRequest);

            return 0;
        }
    }
}

