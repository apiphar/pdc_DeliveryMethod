using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Services
{
    [Authorize]
    public class AfiRequestService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public AfiRequestService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<RegionAndRegionAFIViewModel> GetRegionAndRegionAFI()
        {
            var RegionAndAFI = new RegionAndRegionAFIViewModel()
            {
                RegionList = await this.LogisticDbContext.Region.ToListAsync(),
                RegionAFIList = await this.LogisticDbContext.AFIRegion.ToListAsync()
            };
            return RegionAndAFI;
        }
        
        public async Task<int> insertCustomerAndAFI(AfiRequestInsertData data)
        {
            var row = 0;
            var branchCodeAFI = data.Branch.Remove(data.Branch.IndexOf(" -"));
            await this.LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync( async()=>
            {
                var afiApplication = new AFIApplication()
                {
                    AFIBranchCode = branchCodeAFI.ToUpper(),
                    Name = data.Name.ToUpper(),
                    KTP = data.Ktp.ToUpper(),
                    Address1 = data.Address1.ToUpper(),
                    Address2 = data.Address2.ToUpper(),
                    Address3 = data.Address3.ToUpper(),
                    Province = data.Province.Name.ToUpper(),
                    City = data.City.Name.ToUpper(),
                    PostalCode = data.PostalCode.ToUpper(),
                    AFIRegionCode = data.RegionAFI.AFIRegionCode.ToUpper(),
                    VehicleId = data.VehicleId,
                    ChassisModel = data.Chassis,
                    Timestamp = DateTime.UtcNow,
                    AFISubmissionTypeEnumId = 1,
                    CreatedBy = WebEnvService.UserHumanName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedBy = WebEnvService.UserHumanName,
                    UpdatedAt = DateTime.UtcNow,
                    EffectiveUntil = data.TanggalEfektif,
                    Warna = data.Color.ToUpper(),
                    AFIApplicationProcessEnumId = 1,
                    Jenis = data.Jenis.ToUpper(),
                    Model = data.Model.ToUpper()
                };

                LogisticDbContext.AFIApplication.Add(afiApplication);
                row = await LogisticDbContext.SaveChangesAsync();
            });
            return row;
        }
        

        public Vehicle CheckVehicleExists(string frameNumber)
        {
            var vehicle = this.LogisticDbContext.Vehicle.FirstOrDefault(i => i.FrameNumber == frameNumber);
            if (vehicle == null)
                return null;
            return vehicle;
        }
        /// <summary>
        /// Check is FrameNumber CBU AND Has Form A in ShipmentInvoiceDetail by Frame Number
        /// </summary>
        /// <param name="FrameNumber"></param>
        //public bool CheckIsVehicleCBUAndHasFormA(string FrameNumber)
        //{
        //    if(FrameNumber.Contains("MHF")==false && FrameNumber.Contains("MHK")==false)
        //    {
        //        var HasFormA = LogisticDbContext.ShipmentInvoiceDetail.FirstOrDefault(Q => Q.FrameNumber == FrameNumber && Q.FormANumber != null) !=null ?true:false;
        //        if (!HasFormA)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public bool CheckVehicleInAFI(int vehicleId)
        {
            return this.LogisticDbContext.AFIApplication.GroupBy(Q=>Q.AFIApplicationId).Select(Q=>Q.OrderByDescending(O=>O.AFIApplicationId).FirstOrDefault()).FirstOrDefault(i => i.VehicleId == vehicleId ) !=null;
        }

        //public async Task<Region> GetCurrentRegion()
        //{
        //    var LocationCode = WebEnvService.UserLocationCode;
        //    var regionCode = LogisticDbContext.Branch.FirstOrDefault(Q => Q.lo == LocationCode).RegionCode;
        //    return await LogisticDbContext.Region.FirstOrDefaultAsync(Q => Q.RegionCode == regionCode);
        //}

        public async Task<AfiRequestCheckData> GetVehicle(string frameNumber)
        {
            _ = nameof(DeliveryOrderDetail.IssuedDate);
            _ = nameof(AFIBranch.AFIBranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(CarModel.CarModelCode);
            _ = nameof(CarModel.Name);
            _ = nameof(ExteriorColor.IndonesianName);
            _ = nameof(AFICarType.Model);
            _ = nameof(AFICarType.Jenis);
            _ = nameof(Vehicle.FrameNumber);
            var query = @"SELECT a.VehicleId,[FrameNumber] = a.FrameNumber,[DODate] = f.IssuedDate,[Branch] = i.AFIBranchCode+' - '+b.Name,[CarModelCode] = h.CarModelCode, [CarModelName] = h.Name,[Color]= c.IndonesianName , [Model] = e.Model,[Name]=e.Jenis                          
                        FROM Vehicle a
                        JOIN Branch b ON a.BranchCode = b.BranchCode
                        JOIN ExteriorColor c ON a.ExteriorColorCode = c.ExteriorColorCode
                        JOIN CarType d ON a.Katashiki = d.Katashiki AND a.Suffix = d.Suffix
                        JOIN AFICarType e ON d.AFICarTypeCode = e.AFICarTypeCode 
                        JOIN DeliveryOrderDetail f ON f.VehicleId = a.VehicleId 
                        JOIN CarSeries g ON g.CarSeriesCode = d.CarSeriesCode
                        JOIN CarModel h ON h.CarModelCode = g.CarModelCode
                        JOIN AFIBranch i ON i.BranchCode = b.BranchCode
                        WHERE a.FrameNumber = @FrameNumber AND f.CancelledAt IS NOT NULL";

            var result = await this.LogisticDbContext.QueryAsync<AfiRequestCheckData>(query, new { FrameNumber = frameNumber });
            return result.FirstOrDefault();
        }
    }
}
