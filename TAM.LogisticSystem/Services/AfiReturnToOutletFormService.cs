using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    [Authorize]
    public class AfiReturnToOutletFormService
    {
        public LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public AfiReturnToOutletFormService(LogisticDbContext logisticDbContext,WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
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
        public bool CheckVehicleInAFI(int vehicleId)
        {
            return this.LogisticDbContext.AFIApplication.GroupBy(Q => Q.AFIApplicationId).Select(Q => Q.OrderByDescending(O => O.AFIApplicationId).FirstOrDefault()).FirstOrDefault(i => i.VehicleId == vehicleId) != null;
        }
        public Vehicle CheckVehicleExists(string FrameNumber)
        {
            var query = @"SELECT COUNT(a.VehicleId)
                            FROM Vehicle a
                            JOIN DeliveryOrderDetail b ON a.VehicleId = b.VehicleId
                            WHERE b.CancelledAt IS NOT NULL AND a.FrameNumber = @FrameNumber";
            var row = this.LogisticDbContext.Query<int>(query, new { FrameNumber = FrameNumber }).FirstOrDefault();
            if(row == 1)
            {
                return this.LogisticDbContext.Vehicle.FirstOrDefault(i => i.FrameNumber == FrameNumber);
            }
            return null;
        }
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

        public async Task<int> UpdateAFINormal(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.VehicleId = model.VehicleId;
            afiApplication.AFIBranchCode = model.Branch.Split('-')[0];
            afiApplication.Warna = model.Color.ToUpper();
            afiApplication.Name = model.Name.ToUpper();
            afiApplication.KTP = model.Ktp.ToUpper();
            afiApplication.Address1 = model.Address1.ToUpper();
            afiApplication.Address2 = model.Address2.ToUpper();
            afiApplication.Address3 = model.Address3.ToUpper();
            afiApplication.Province = model.Province.Name.ToUpper();
            afiApplication.City = model.City.Name.ToUpper();
            afiApplication.PostalCode = model.PostalCode;
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIRegionCode = model.RegionAFI.AFIRegionCode.ToUpper();
            if (afiApplication.Model.ToUpper() == "CHASSIS")
            {
                afiApplication.ChassisModel = model.Chassis.ToUpper();
            }
            afiApplication.AFIApplicationProcessEnumId = 1;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateExCancel(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.Warna = model.Color.ToUpper();
            afiApplication.Name = model.Name.ToUpper();
            afiApplication.KTP = model.Ktp.ToUpper();
            afiApplication.Address1 = model.Address1.ToUpper();
            afiApplication.Address2 = model.Address2.ToUpper();
            afiApplication.Address3 = model.Address3.ToUpper();
            afiApplication.Province = model.Province.Name.ToUpper();
            afiApplication.City = model.City.Name.ToUpper();
            afiApplication.PostalCode = model.PostalCode;
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIRegionCode = model.RegionAFI.AFIRegionCode.ToUpper();
            if (afiApplication.Model.ToUpper() == "CHASSIS")
            {
                afiApplication.ChassisModel = model.Chassis.ToUpper();
            }
            afiApplication.AFIApplicationProcessEnumId = 9;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiA(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIApplicationProcessEnumId = 5;
            afiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiB(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.Name = model.Name.ToUpper();
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIApplicationProcessEnumId = 5;
            afiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiC(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.Address1 = model.Address1.ToUpper();
            afiApplication.Address2 = model.Address2.ToUpper();
            afiApplication.Address3 = model.Address3.ToUpper();
            afiApplication.Province = model.Province.Name.ToUpper();
            afiApplication.City = model.City.Name.ToUpper();
            afiApplication.PostalCode = model.PostalCode.ToUpper();
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIApplicationProcessEnumId = 5;
            afiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateAFIRevisiD(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.KTP = model.Ktp.ToUpper();
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIApplicationProcessEnumId = 5;
            afiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiE(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.Warna = model.Color.ToUpper();
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIApplicationProcessEnumId = 6;
            afiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiF(AfiReturnToOutletFormModel model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            afiApplication.ChassisModel = model.Chassis.ToUpper();
            afiApplication.EffectiveUntil = model.TanggalEfektif;
            afiApplication.AFIApplicationProcessEnumId = 5;
            afiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            afiApplication.UpdatedAt = DateTimeOffset.UtcNow;
            afiApplication.UpdatedBy = this.WebEnvironmentService.UserHumanName;
            return await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
