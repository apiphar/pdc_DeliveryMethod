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
    /// <summary>
    /// Afi Request Revisi (Ex-Cancel, Revisi)
    /// </summary>
    public class AfiRequestRevisiAndExCancelFormService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public AfiRequestRevisiAndExCancelFormService(LogisticDbContext logisticDbContext,WebEnvironmentService webEnvironmentService)
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


        public async Task<AFIApplication> GenerateNewAFI(int afiapplicationid)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == afiapplicationid);
            var newAfiApplication = new AFIApplication()
            {
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                CreatedBy = WebEnvironmentService.UserHumanName,
                UpdatedBy = WebEnvironmentService.UserHumanName,
                FakturNumber = afiApplication.FakturNumber,
                ApplicationNumber = afiApplication.ApplicationNumber,
                Name = afiApplication.Name,
                KTP = afiApplication.KTP,
                Address1 = afiApplication.Address1,
                Address2 = afiApplication.Address2,
                Address3 = afiApplication.Address3,
                Province = afiApplication.Province,
                City = afiApplication.City,
                PostalCode = afiApplication.PostalCode,
                AFIRegionCode = afiApplication.AFIRegionCode,
                Warna = afiApplication.Warna,
                AFIApplicationProcessEnumId = afiApplication.AFIApplicationProcessEnumId,
                AFIBranchCode = afiApplication.AFIBranchCode,
                ChassisModel = afiApplication.ChassisModel,
                Jenis = afiApplication.Jenis,
                Model = afiApplication.Model,
                AFISubmissionTypeEnumId = afiApplication.AFISubmissionTypeEnumId,
                Timestamp = DateTimeOffset.UtcNow,
                VehicleId = afiApplication.VehicleId
            };
            return newAfiApplication;
        }

        public async Task<int> UpdateExCancel(AfiRevisiAndExCancelForm model)
        {
            var afiApplication = await this.LogisticDbContext.AFIApplication.FirstOrDefaultAsync(Q => Q.AFIApplicationId == model.AfiApplicationId);
            var newAfiApplication = new AFIApplication()
            {
                Warna = model.Color.ToUpper(),
                Name = model.Name.ToUpper(),
                KTP = model.Ktp.ToUpper(),
                Address1 = model.Address1.ToUpper(),
                Address2 = model.Address2.ToUpper(),
                Address3 = model.Address3.ToUpper(),
                Province = model.Province.Name.ToUpper(),
                City = model.City.Name.ToUpper(),
                PostalCode = model.PostalCode,
                EffectiveUntil = model.TanggalEfektif,
                AFIRegionCode = model.RegionAFI.AFIRegionCode.ToUpper(),
                AFIApplicationProcessEnumId = 9,
                AFISubmissionTypeEnumId = 8,
                ChassisModel = afiApplication.ChassisModel,
                AFIBranchCode = afiApplication.AFIBranchCode,
                VehicleId = afiApplication.VehicleId,
                ApplicationNumber = afiApplication.ApplicationNumber,
                Jenis = afiApplication.Jenis,
                Model = afiApplication.Model,
                FakturNumber = afiApplication.FakturNumber,
                Timestamp = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.WebEnvironmentService.UserHumanName,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = WebEnvironmentService.UserHumanName
            };
            if (afiApplication.Model.ToUpper() == "CHASSIS")
            {
                newAfiApplication.ChassisModel = model.Chassis;
            }
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiA(AfiRevisiAndExCancelForm model)
        {
            var newAfiApplication = GenerateNewAFI(model.AfiApplicationId).Result;
            newAfiApplication.AFIApplicationProcessEnumId = 5;
            newAfiApplication.EffectiveUntil = model.TanggalEfektif;
            newAfiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiB(AfiRevisiAndExCancelForm model)
        {
            var newAfiApplication = GenerateNewAFI(model.AfiApplicationId).Result;
            newAfiApplication.Name = model.Name.ToUpper();
            newAfiApplication.EffectiveUntil = model.TanggalEfektif;
            newAfiApplication.AFIApplicationProcessEnumId = 5;
            newAfiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiC(AfiRevisiAndExCancelForm model)
        {
            var newAfiApplication = GenerateNewAFI(model.AfiApplicationId).Result;
            newAfiApplication.Address1 = model.Address1.ToUpper();
            newAfiApplication.Address2 = model.Address2.ToUpper();
            newAfiApplication.Address3 = model.Address3.ToUpper();
            newAfiApplication.Province = model.Province.Name.ToUpper();
            newAfiApplication.City = model.City.Name.ToUpper();
            newAfiApplication.PostalCode = model.PostalCode.ToUpper();
            newAfiApplication.EffectiveUntil = model.TanggalEfektif;
            newAfiApplication.AFIApplicationProcessEnumId = 5;
            newAfiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateAFIRevisiD(AfiRevisiAndExCancelForm model)
        {
            var newAfiApplication = GenerateNewAFI(model.AfiApplicationId).Result;
            newAfiApplication.KTP = model.Ktp.ToUpper();
            newAfiApplication.EffectiveUntil = model.TanggalEfektif;
            newAfiApplication.AFIApplicationProcessEnumId = 5;
            newAfiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiE(AfiRevisiAndExCancelForm model)
        {
            var newAfiApplication = GenerateNewAFI(model.AfiApplicationId).Result;
            newAfiApplication.Warna = model.Color.ToUpper();
            newAfiApplication.EffectiveUntil = model.TanggalEfektif;
            newAfiApplication.AFIApplicationProcessEnumId = 5;
            newAfiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAFIRevisiF(AfiRevisiAndExCancelForm model)
        {
            var newAfiApplication = GenerateNewAFI(model.AfiApplicationId).Result;
            newAfiApplication.ChassisModel = model.Chassis.ToUpper();
            newAfiApplication.EffectiveUntil = model.TanggalEfektif;
            newAfiApplication.AFIApplicationProcessEnumId = 5;
            newAfiApplication.AFISubmissionTypeEnumId = model.TipePengajuan;
            this.LogisticDbContext.Add(newAfiApplication);
            return await this.LogisticDbContext.SaveChangesAsync();
        }

    }
}
