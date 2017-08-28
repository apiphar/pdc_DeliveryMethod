using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Net.Http;
using Accelist.SDK.REST;
using Newtonsoft.Json;
using TAM.LogisticSystem.Enums;
using Microsoft.Extensions.Caching.Distributed;

namespace TAM.LogisticSystem.Services
{
    public class DMSService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;
        private readonly IDistributedCache DistributedCache;

        public DMSService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService, IDistributedCache distributedCache)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
            this.DistributedCache = distributedCache;
        }

        private Random Random = new Random();
        public string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }


        public async Task<List<DMSFindLocationModel>> GetLocation(DMSFindVehicleModel model)
        {
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(VehicleRouting.LocationCode);
            _ = nameof(VehicleRouting.ScanTime);
            _ = nameof(VehicleRouting.VehicleId);
            var con = LogisticDbContext.Database.GetDbConnection();
            {
                var selectedList = new List<DMSFindLocationModel>();
                //check if frame number is not registered
                var checkFrameNumber = await LogisticDbContext.Vehicle.FirstOrDefaultAsync(Q => Q.FrameNumber == model.FrameNumber);
                if (checkFrameNumber == null)
                {
                    return null;
                }
                //get all data from last scan time location by frame number
                var selected = (await con.QueryFirstOrDefaultAsync<DMSFindLocationModel>(@"
                    SELECT
                    vr.locationcode as [Location],
                    vr.scantime as [DateLocation]
                    FROM vehiclerouting vr, (
                        SELECT top 1
                        vr.locationcode
                        FROM vehicleRouting vr 
                        JOIN vehicle v ON v.vehicleId = vr.VehicleId
                        WHERE frameNumber = @frameNumber AND vr.scantime IS NOT NULL
                        ORDER BY vr.ordering DESC) as lc
                    WHERE vr.locationcode = lc.locationCode AND vr.scantime IS NOT NULL
                    ORDER by vr.scanTime ASC
                    ", new { frameNumber = model.FrameNumber }));

                if (selected == null)
                {
                    selected = await LogisticDbContext.Vehicle
                        .Join(LogisticDbContext.VehicleRouting, X => X.VehicleId, Y => Y.VehicleId, (X, Y) => new { X, Y })
                        .Where(Q => Q.X.FrameNumber == model.FrameNumber && Q.Y.Ordering == 1)
                        .Select(Q => new DMSFindLocationModel
                    {
                        DateLocation = Q.X.REVPLOD,
                        Location = Q.Y.LocationCode
                    }).FirstOrDefaultAsync();
                    if (selected == null)
                    {
                        return selectedList;
                    }
                }
                selected.FrameNumber = model.FrameNumber;
                selectedList.Add(selected);
                return selectedList;
            }
        }

        public async Task<List<DMSRetrieveBookingUnitKalkulasiPDD>> GetBookingUnitKalkulasiPDD(DMSSendBookingUnitKalkulasiPDD dmsSendBookingUnitKalkulasiPDD)
        {
            var retrieveBookingUnitKalkulasiPDDList = new List<DMSRetrieveBookingUnitKalkulasiPDD>();
            _ = nameof(Vehicle.EstimatedPDCOut);
            _ = nameof(Swapping.EstimatedPDD);
            var searchVehicle = new Vehicle();
            if (!string.IsNullOrEmpty(dmsSendBookingUnitKalkulasiPDD.FrameNumber))
            {
                searchVehicle = await LogisticDbContext.Vehicle.FirstOrDefaultAsync(Q => Q.FrameNumber == dmsSendBookingUnitKalkulasiPDD.FrameNumber);
            }
            else if (!string.IsNullOrEmpty(dmsSendBookingUnitKalkulasiPDD.RRN) && dmsSendBookingUnitKalkulasiPDD.DTPLOD.HasValue)
            {
                searchVehicle = await LogisticDbContext.Vehicle.FirstOrDefaultAsync(Q => Q.RRN == dmsSendBookingUnitKalkulasiPDD.RRN && Q.ProductionYear == dmsSendBookingUnitKalkulasiPDD.DTPLOD.Value.Year);
            }
            if (searchVehicle == null)
            {
                return null;
            }

            if (dmsSendBookingUnitKalkulasiPDD.DeliveryCategory == 1)
            {
                retrieveBookingUnitKalkulasiPDDList.Add(new DMSRetrieveBookingUnitKalkulasiPDD
                {
                    EarliestPDD = (DateTime)searchVehicle.EstimatedArrivalBranch.Value.DateTime,
                    StatusFormA = false //dummy
                });
            }
            else if (dmsSendBookingUnitKalkulasiPDD.DeliveryCategory == 2)
            {
                retrieveBookingUnitKalkulasiPDDList.Add(new DMSRetrieveBookingUnitKalkulasiPDD
                {
                    EarliestPDD = (DateTime)searchVehicle.EstimatedPDCOut.Value.DateTime,
                    StatusFormA = false //dummy
                });
            }
            else
            {
                return null;
            }

            return retrieveBookingUnitKalkulasiPDDList;
        }

        public async Task<Vehicle> GetVehicleAsync(string frameNumber)
        {
            var searchVehicle = await LogisticDbContext.Vehicle.FirstOrDefaultAsync(Q => Q.FrameNumber == frameNumber);

            if (searchVehicle == null)
            {
                return null;
            }

            return searchVehicle;
        }

        public async Task<Vehicle> GetVehicleAsync(string RRN, DateTime DTPLOD)
        {
            var searchVehicle = await LogisticDbContext.Vehicle.FirstOrDefaultAsync(Q => Q.RRN == RRN && Q.ProductionYear == DTPLOD.Year);

            if (searchVehicle == null)
            {
                return null;
            }

            return searchVehicle;
        }

        public async Task<List<DMSRetrieveBookingUnitKonfirmasiPDD>> GetBookingUnitKonfirmasiPDD(Vehicle vehicle, DMSSendBookingUnitKonfirmasiPDD dmsSendBookingUnitKonfirmasiPDD)
        {
            var retrieveBookingUnitKonfirmasiPDDList = new List<DMSRetrieveBookingUnitKonfirmasiPDD>();
            if (dmsSendBookingUnitKonfirmasiPDD.DeliveryCategory == 1)
            {
                if (dmsSendBookingUnitKonfirmasiPDD.RequestedPDD < (DateTime)vehicle.EstimatedArrivalBranch.Value.DateTime)
                {
                    return null;
                }
                retrieveBookingUnitKonfirmasiPDDList.Add(new DMSRetrieveBookingUnitKonfirmasiPDD
                {
                    EarliestPDD = (DateTime)vehicle.EstimatedArrivalBranch.Value.DateTime,
                    StatusFormA = false //dummy
                });
            }
            else if (dmsSendBookingUnitKonfirmasiPDD.DeliveryCategory == 2)
            {
                if (dmsSendBookingUnitKonfirmasiPDD.RequestedPDD < (DateTime)vehicle.EstimatedPDCOut.Value.DateTime)
                {
                    return null;
                }
                retrieveBookingUnitKonfirmasiPDDList.Add(new DMSRetrieveBookingUnitKonfirmasiPDD
                {
                    EarliestPDD = (DateTime)vehicle.EstimatedPDCOut.Value.DateTime,
                    StatusFormA = false //dummy
                });
            }
            else
            {
                return null;
            }

            vehicle.RequestedDeliveryTime = dmsSendBookingUnitKonfirmasiPDD.RequestedPDD;
            vehicle.PaketAksesorisTAM = dmsSendBookingUnitKonfirmasiPDD.PaketAksesorisTAM;
            vehicle.UpdatedAt = DateTimeOffset.UtcNow;
            vehicle.UpdatedBy = "DMS";

            LogisticDbContext.Update(vehicle);
            await LogisticDbContext.SaveChangesAsync();

            return retrieveBookingUnitKonfirmasiPDDList;
        }

        public async Task<string> DriverConfirmationValidation(DMSSendDriverConfirmation dmsSendDriverConfirmation)
        {
            if (string.IsNullOrEmpty(dmsSendDriverConfirmation.FrameNumber))
            {
                return "Frame Number tidak boleh kosong";
            }
            var vehicle = await GetVehicleAsync(dmsSendDriverConfirmation.FrameNumber);
            if (vehicle == null)
            {
                return "Frame Number tidak ditemukan";
            }
            var deliveryRequest = await this.LogisticDbContext.DeliveryRequest.AsNoTracking().FirstOrDefaultAsync(Q => Q.VehicleId == vehicle.VehicleId);
            if (deliveryRequest == null)
            {
                return "Frame Number ini belum di memiliki Delivery Request";
            }
            if (dmsSendDriverConfirmation.PickUpDateTime < (DateTime)vehicle.EstimatedPDCOut.Value.DateTime)
            {
                return "Pick Up Date tidak boleh lebih kecil dari Earliest PDD";
            }
            if (string.IsNullOrEmpty(dmsSendDriverConfirmation.DriverId))
            {
                return "Driver ID tidak boleh kosong";
            }
            if (string.IsNullOrEmpty(dmsSendDriverConfirmation.DriverName))
            {
                return "Driver Name tidak boleh kosong";
            }
            if (dmsSendDriverConfirmation.PickUpDateTime == null)
            {
                return "Pick Up Date tidak boleh kosong";
            }
            if (deliveryRequest.DeliveryRequestTypeEnumId != (int)DeliveryRequestType.SelfPick)
            {
                return "Delivery Request Category harus Direct Delivery";
            }
            return null;
        }

        public string CalculateDeliveryRequestValidation(DMSSendBookingUnitKalkulasiPDD dmsSendBookingUnitKalkulasiPDD)
        {
            if (string.IsNullOrEmpty(dmsSendBookingUnitKalkulasiPDD.FrameNumber) && string.IsNullOrEmpty(dmsSendBookingUnitKalkulasiPDD.RRN))
            {
                return "Frame Number atau RRN salah satu harus diisi";
            }
            if (!string.IsNullOrEmpty(dmsSendBookingUnitKalkulasiPDD.FrameNumber) && !string.IsNullOrEmpty(dmsSendBookingUnitKalkulasiPDD.RRN))
            {
                return "Hanya boleh isi salah satu antara Frame Number atau RRN";
            }
            return null;
        }

        public string ConfirmationDeliveryRequestValidation(DMSSendBookingUnitKonfirmasiPDD dmsSendBookingUnitKonfirmasiPDD)
        {
            if (string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.FrameNumber) && string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.RRN))
            {
                return "Frame Number atau RRN salah satu harus diisi";
            }
            if (!string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.FrameNumber) && !string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.RRN))
            {
                return "Hanya boleh isi salah satu antara Frame Number atau RRN";
            }
            return null;
        }

        public async Task<List<DMSRetrieveDriverConfirmation>> GetDriverConfirmation(DMSSendDriverConfirmation dmsSendDriverConfirmation)
        {
            var retrieveDriverConfirmationList = new List<DMSRetrieveDriverConfirmation>();
            var confirmationCode = RandomString(8);

            _ = nameof(Vehicle.EstimatedPDCOut);
            _ = nameof(DeliveryRequest.PickUpConfirmationCode);
            _ = nameof(DeliveryRequest.PickUpIdentityCardNumber);
            _ = nameof(DeliveryRequest.PickUpDate);
            var query = @"
                        SELECT
							DR.*
                        FROM Vehicle V
							JOIN DeliveryRequest DR ON DR.VehicleId = V.VehicleId
                        WHERE V.FrameNumber = @frameNumber AND DR.CancelledAt IS NULL AND DR.ClosedAt IS NULL
                    ";
            var entity = await LogisticDbContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<DeliveryRequest>(query, new
            {
                frameNumber = dmsSendDriverConfirmation.FrameNumber,
            });
            while (await LogisticDbContext.DeliveryRequest.AnyAsync(Q => Q.PickUpConfirmationCode == confirmationCode && Q.CancelledAt == null && Q.ClosedAt == null))
            {
                confirmationCode = RandomString(8);
            }
            if (entity == null)
            {
                return null;
            }
            entity.PickUpDate = dmsSendDriverConfirmation.PickUpDateTime;
            entity.PickUpIdentityName = dmsSendDriverConfirmation.DriverName;
            entity.PickUpIdentityCardNumber = dmsSendDriverConfirmation.DriverId;
            entity.PickUpConfirmationCode = confirmationCode;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = "DMS";

            this.LogisticDbContext.Update(entity);
            await this.LogisticDbContext.SaveChangesAsync();

            //retrieve

            var retrieveDriverConfirmation = new DMSRetrieveDriverConfirmation()
            {
                FrameNumber = dmsSendDriverConfirmation.FrameNumber,
                DriverId = dmsSendDriverConfirmation.DriverId,
                PickUpDateTime = dmsSendDriverConfirmation.PickUpDateTime,
                ConfirmationCode = entity.PickUpConfirmationCode
            };

            retrieveDriverConfirmationList.Add(retrieveDriverConfirmation);

            return retrieveDriverConfirmationList;
        }

        public async Task<List<DMSSentQuotationModel>> GetQuotationOB(DMSQuotationOBModel model)
        {
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.PhysicalLocationCode);

            var data = new List<DMSSentQuotationModel>();
            data.Add(new DMSSentQuotationModel
            {
                Price = 25000000,
                LocationUnit = "LOC001"
            });
            return data;
        }

        public async Task<List<DMSDOUpdateViewModel>> GetDOUpdateAsync(int deliveryOrderDetailId)
        {
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.EnginePrefix);
            _ = nameof(Vehicle.EngineNumber);
            _ = nameof(Vehicle.Katashiki);
            _ = nameof(Vehicle.Suffix);
            _ = nameof(Vehicle.ExteriorColorCode);
            _ = nameof(DeliveryOrderDetail.WholePriceBeforeTax);
            _ = nameof(DeliveryOrderDetail.ValueAddedTax);
            _ = nameof(DeliveryOrderDetail.LuxuryTax);
            _ = nameof(DeliveryOrderDetail.CancellationNumber);
            _ = nameof(DeliveryOrderDetail.DeliveryOrderNumber);
            _ = nameof(DeliveryOrder.CreatedAt);
            _ = nameof(Vehicle.BranchCode);
            _ = nameof(Vehicle.RRN);
            _ = nameof(Vehicle.EstimatedPDCOut);
            _ = nameof(DeliveryOrderDetail.InvoicePrice);
            _ = nameof(DeliveryOrderDetail.DiscountPrice);
            _ = nameof(DeliveryOrderDetail.PPH22);
            _ = nameof(Vehicle.DTPLOD);
            var DOUpdateData = await LogisticDbContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<DMSDOUpdateViewModel>(@"
                SELECT
	                V.FrameNumber,
	                V.EnginePrefix,
	                V.EngineNumber,
	                V.Katashiki,
	                V.Suffix,
	                V.ExteriorColorCode,
	                DOD.WholePriceBeforeTax AS WPBT,
	                DOD.ValueAddedTax,
	                DOD.LuxuryTax,
	                DOD.CancellationNumber,
	                DOD.DeliveryOrderNumber,
	                CONVERT(DATETIME2, DO.CreatedAt, 1) AS DeliveryOrderDate,
	                V.KeyNumber,
	                CONVERT(DATETIME2, DATEADD(day, C.TermOfPaymentDay, DO.CreatedAt), 1) AS DeliveryOrderDueDate,
	                DO.DebitAdviceNumber,
	                V.BranchCode,
	                V.RRN,
	                DOD.InvoicePrice AS InvoicedPrice,
	                DOD.DiscountPrice,
	                DOD.PPH22,
	                CONVERT(DATETIME2, V.DTPLOD, 1) AS DTPLOD
                FROM Vehicle V
                    JOIN DeliveryOrderDetail DOD ON DOD.VehicleId = V.VehicleId
                    JOIN DeliveryOrder DO ON DO.DeliveryOrderNumber = DOD.DeliveryOrderNumber
                    JOIN Branch B ON B.BranchCode = V.BranchCode
                    JOIN Company C ON C.CompanyCode = B.CompanyCode
                WHERE DOD.IsSentToDMS = 0 AND DOD.DeliveryOrderDetailId = @deliveryDetailId
            ", new { deliveryDetailId = deliveryOrderDetailId });
            var DOUpdateDataList = new List<DMSDOUpdateViewModel>();
            DOUpdateDataList.Add(DOUpdateData);
            return DOUpdateDataList;
        }

        public async Task DOSentToDMSAsync(int deliveryOrderDetailId)
        {
            var entity = await LogisticDbContext.DeliveryOrderDetail.Where(Q => Q.DeliveryOrderDetailId == deliveryOrderDetailId).FirstOrDefaultAsync();

            entity.IsSentToDMS = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedBy = "DMS";

            LogisticDbContext.Update(entity);
            await LogisticDbContext.SaveChangesAsync();
        }

        public async Task<List<DMSSentApprovalReceivedQuotationModel>> GetApprovalReceivedQuotation(DMSApprovalReceivedQuotationModel model)
        {
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(Vehicle.PhysicalLocationCode);

            var data = new List<DMSSentApprovalReceivedQuotationModel>();
            data.Add(new DMSSentApprovalReceivedQuotationModel
            {
                Price = 25000000,
                LocationUnit = "LOC001",
                ChangeLocationFlag = true
            });
            return data;
        }
        public async Task<bool> ValidateOpenDR(string frameNumber)
        {
            var vehicleId = await LogisticDbContext.Vehicle.Where(Q => Q.FrameNumber == frameNumber)
                .Select(Q => Q.VehicleId).FirstOrDefaultAsync();

            var deliveryRequest = await LogisticDbContext.DeliveryRequest.Where(Q => Q.VehicleId == vehicleId)
                .Select(Q => new DMSValidateKaroseriModel
                {
                    CancelledAt = Q.CancelledAt,
                    CloseAt = Q.ClosedAt
                }).FirstOrDefaultAsync();

            if (deliveryRequest == null || deliveryRequest.CancelledAt != null || deliveryRequest.CloseAt != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidatePickupDate(string frameNo, DateTimeOffset PickUpUniversalDate)
        {
            var selected = await LogisticDbContext.Vehicle.Where(Q => Q.FrameNumber == frameNo)
                .Select(Q => Q.EstimatedPDCOut).FirstOrDefaultAsync();

            if (selected == null || PickUpUniversalDate.CompareTo((DateTimeOffset)selected) < 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ValidateKaroseriLocation(string karoseriLocationCode)
        {
            var con = LogisticDbContext.Database.GetDbConnection();

            return await LogisticDbContext.Location
                .Where(Q => Q.LocationTypeCode == "KARO" && Q.LocationCode == karoseriLocationCode)
                .AnyAsync();
        }

        public async Task<bool> ValidateUrgentMemoLocation(string karoseriLocationCode)
        {
            var con = LogisticDbContext.Database.GetDbConnection();

            return await LogisticDbContext.Location
                .Where(Q => (Q.LocationTypeCode == "KARO" || Q.LocationTypeCode == "BP") && Q.LocationCode == karoseriLocationCode)
                .AnyAsync();
        }

        public async Task<int> GetSequentialNumber(string branchCode, string tipeDR, DateTimeOffset deliveryRequestLocalDate)
        {
            var sequentialNumber = await DistributedCache.GetObjectAsync<int>($"DeliveryRequest[{branchCode}, {tipeDR}, {deliveryRequestLocalDate}]");
            return sequentialNumber + 1;
        }

        public async Task SaveSequentialNumber(string branchCode, string tipeDR, DateTimeOffset deliveryRequestLocalDate, int sequentialNumber)
        {
            await DistributedCache.SetObjectAsync($"DeliveryRequest[{branchCode}, {tipeDR}, {deliveryRequestLocalDate}]", sequentialNumber);
        }

        public async Task<List<DMSRetrieveKaroseriModel>> SendKaroseri(DMSSendKaroseriModel model)
        {
            var vehicle = await LogisticDbContext.Vehicle.Where(Q => Q.FrameNumber == model.FrameNumber)
                .Select(Q => new DMSVehicleKaroseriModel
                {
                    VehicleId = Q.VehicleId,
                    BranchCode = Q.BranchCode
                }).FirstOrDefaultAsync();

            //var sequentialNumber = await GetSequentialNumber(vehicle.BranchCode, "TO", model.DeliveryRequestDate);
            //var deliveryRequestNumber = vehicle.BranchCode + "/TO/" + model.DeliveryRequestDate.ToString("yyyyMMdd") + "/" + sequentialNumber.ToString();
            var deliveryRequestNumber = vehicle.BranchCode + "/TO/" + model.DeliveryRequestDate.ToString("yyyyMMdd") + "/" + RandomString(3);

            var random = RandomString(8);
            while (await LogisticDbContext.DeliveryRequest.Where(Q => Q.CancelledAt == null && Q.ClosedAt == null && Q.PickUpConfirmationCode == random).AnyAsync())
            {
                random = RandomString(8);
            }

            var entity = new DeliveryRequest()
            {
                DeliveryRequestNumber = deliveryRequestNumber,
                VehicleId = vehicle.VehicleId,
                DeliveryRequestTypeEnumId = (int)DeliveryRequestType.TransitToOthers,
                PickUpDate = model.PickUpDateTime.ToUniversalTime(),
                TransitLocationCode = model.LocationCode,
                TransitReturnDate = model.VehicleReturnDate.ToUniversalTime(),
                DeliveryRequestTransitTypeEnumId = (int)(model.DriverReturnType == 1 ? DeliveryRequestTransitType.SelfPickToOthers : DeliveryRequestTransitType.NormalSelfPickFromOthers),
                PickUpIdentityIsKtp = false,
                PickUpIdentityCardNumber = model.DriverId,
                PickUpIdentityName = model.DriverName,
                PickUpConfirmationCode = random,

                CreatedAt = model.DeliveryRequestDate.ToUniversalTime(),
                CreatedBy = "DMS",
                UpdatedAt = model.DeliveryRequestDate.ToUniversalTime(),
                UpdatedBy = "DMS"
            };

            LogisticDbContext.Add(entity);
            await LogisticDbContext.SaveChangesAsync();
            //await SaveSequentialNumber(vehicle.BranchCode, "TO", model.DeliveryRequestDate, sequentialNumber);

            var retrieveData = new List<DMSRetrieveKaroseriModel>();
            retrieveData.Add(new DMSRetrieveKaroseriModel
            {
                FrameNumber = model.FrameNumber,
                UrgenMemoStatus = true,
                ConfirmationCode = model.DriverReturnType == 1 ? random : null
            });

            return retrieveData;
        }

        public async Task<List<DMSRetrieveKaroseriModel>> SentUrgentMemo(DMSSendKaroseriModel model)
        {
            var vehicle = await LogisticDbContext.Vehicle.Where(Q => Q.FrameNumber == model.FrameNumber).FirstOrDefaultAsync();
            var entity = await LogisticDbContext.DeliveryRequest
                .FirstOrDefaultAsync(Q => Q.VehicleId == vehicle.VehicleId && Q.CancelledAt == null && Q.ClosedAt == null);

            var random = RandomString(8);
            while (await LogisticDbContext.DeliveryRequest.Where(Q => Q.CancelledAt == null && Q.ClosedAt == null && Q.PickUpConfirmationCode == random).AnyAsync())
            {
                random = RandomString(8);
            }

            entity.DeliveryRequestTypeEnumId = (int)(DeliveryRequestType.TransitToOthers);
            entity.PickUpDate = model.PickUpDateTime.ToUniversalTime();
            entity.TransitLocationCode = model.LocationCode;
            entity.TransitReturnDate = model.VehicleReturnDate.ToUniversalTime();
            entity.DeliveryRequestTransitTypeEnumId = (int)(model.DriverReturnType == 1 ? DeliveryRequestTransitType.SelfPickToOthers : DeliveryRequestTransitType.NormalSelfPickFromOthers);
            entity.PickUpIdentityIsKtp = false;
            entity.PickUpIdentityCardNumber = model.DriverId;
            entity.PickUpIdentityName = model.DriverName;

            entity.UpdatedAt = model.DeliveryRequestDate.ToUniversalTime();
            entity.UpdatedBy = "DMS";

            LogisticDbContext.Update(entity);
            await LogisticDbContext.SaveChangesAsync();

            var retrieveData = new List<DMSRetrieveKaroseriModel>();
            retrieveData.Add(new DMSRetrieveKaroseriModel
            {
                FrameNumber = model.FrameNumber,
                UrgenMemoStatus = true,
                ConfirmationCode = model.DriverReturnType == 1 ? random : null
            });

            return retrieveData;
        }
    }
}