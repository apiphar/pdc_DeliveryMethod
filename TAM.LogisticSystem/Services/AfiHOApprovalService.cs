using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Services
{
    [Authorize]
    public class AfiHOApprovalService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public AfiHOApprovalService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<List<AfiBranchViewModel>> GetAllBranch()
        {
            _ = nameof(Company.DealerCode);
            var dealerCode = (await this.LogisticDbContext.QueryAsync<string>($@"
SELECT
	c.DealerCode
FROM
	UserMapping um
	JOIN BranchLocationMapping blm ON blm.LocationCode = um.LocationCode
	JOIN Branch b ON b.BranchCode = blm.BranchCode
	JOIN Company c ON c.CompanyCode = b.CompanyCode
WHERE
	um.Username = @username
", new { username = this.WebEnvService.Username })).FirstOrDefault();

            _ = nameof(AFIBranch.AFIBranchCode);
            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            var query = @"SELECT [AFIBranchCode] = a.AFIBranchCode,[BranchCode]=b.BranchCode,[Name]=b.Name
                            FROM AFIBranch a
                            JOIN Branch b ON a.BranchCode = b.BranchCode
	                        JOIN Company c ON b.CompanyCode = c.CompanyCode
	                        JOIN Dealer d ON c.DealerCode = d.DealerCode
                            WHERE d.DealerCode = @dealerCode";
            var result = await this.LogisticDbContext.QueryAsync<AfiBranchViewModel>(query, new { dealerCode = dealerCode });
            return result;
        }

        public int ReturnToOutlet(List<AfiHOApprovalSubmission> AFIApplication)
        {
            var row = 0;
            AFIApplication.ForEach(
            data =>
                {
                    var existingSubmission = this.LogisticDbContext.AFIApplication.Find(data.AFIApplicationId);
                    existingSubmission.AFIApplicationProcessEnumId = data.TipePengajuanName.ToUpper() == "NORMAL" ? 2 : (data.TipePengajuanName.ToUpper().Contains("REV") ? 6 : (data.TipePengajuanName.ToUpper() == ".R" ? 10 : existingSubmission.AFIApplicationProcessEnumId));
                    existingSubmission.UpdatedAt = DateTime.UtcNow;
                    existingSubmission.UpdatedBy = WebEnvService.UserHumanName;
                }
            );
            row = this.LogisticDbContext.SaveChanges();
            return row == AFIApplication.Count ? 1 : 0;

        }

        public int ProcessToTam(List<AfiHOApprovalSubmission> AFIApplication)
        {
            var row = 0;
            AFIApplication.ForEach(
                data =>
                {
                    var existingSubmission = this.LogisticDbContext.AFIApplication.Find(data.AFIApplicationId);
                    existingSubmission.AFIApplicationProcessEnumId = data.TipePengajuanName.ToUpper() == "NORMAL" ? 3 : (data.TipePengajuanName.ToUpper().Contains("REV") ? 7 : (data.TipePengajuanName.ToUpper() == ".R" ? 11 : existingSubmission.AFIApplicationProcessEnumId));
                    existingSubmission.UpdatedAt = DateTime.UtcNow;
                    existingSubmission.UpdatedBy = WebEnvService.UserHumanName;
                }
            );

            row = this.LogisticDbContext.SaveChanges();
            return row == AFIApplication.Count ? 1 : 0;

        }

        public async Task<List<AfiGridViewModel>> GetAFIHoApproval(AfiHOApprovalSearch model)
        {
            _ = nameof(Company.DealerCode);
            var dealerCode = (await this.LogisticDbContext.QueryAsync<string>($@"
SELECT
	c.DealerCode
FROM
	UserMapping um
	JOIN BranchLocationMapping blm ON blm.LocationCode = um.LocationCode
	JOIN Branch b ON b.BranchCode = blm.BranchCode
	JOIN Company c ON c.CompanyCode = b.CompanyCode
WHERE
	um.Username = @username
", new { username = this.WebEnvService.Username })).FirstOrDefault();

            var query = "";
            model.frameNo = model.frameNo != null ? (model.frameNo.Trim() != "" ? model.frameNo : null) : null;
            _ = nameof(AFIApplication.AFIApplicationId);
            _ = nameof(DeliveryOrderDetail.VehicleId);
            _ = nameof(DeliveryOrderDetail.IssuedDate);
            _ = nameof(AFIApplication.VehicleId);
            _ = nameof(Vehicle.FrameNumber);
            _ = nameof(CarModel.Name);
            _ = nameof(AFIApplication.Warna);
            _ = nameof(AFIApplication.Jenis);
            _ = nameof(AFIApplication.Model);
            _ = nameof(AFIApplication.ChassisModel);
            _ = nameof(AFIApplication.ApplicationNumber);
            _ = nameof(AFIApplication.AFIBranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(AFIApplication.Name);
            _ = nameof(AFIApplication.KTP);
            _ = nameof(AFIApplication.Address1);
            _ = nameof(AFIApplication.Address2);
            _ = nameof(AFIApplication.Address3);
            _ = nameof(AFIApplication.City);
            _ = nameof(AFIApplication.Province);
            _ = nameof(AFIApplication.PostalCode);
            _ = nameof(AFIApplication.EffectiveUntil);
            _ = nameof(AFIRegion.AFIRegionCode);
            _ = nameof(AFIRegion.Name);
            _ = nameof(AFIApplication.FakturNumber);
            _ = nameof(AFIApplication.Timestamp);
            _ = nameof(AFISubmissionTypeEnum.AFISubmissionTypeEnumId);
            _ = nameof(AFISubmissionTypeEnum.Name);
            if (model.frameNo != null)
            {
                query = @"SELECT [AfiApplicationId] = a.AfiApplicationId,[DODate]=h.IssuedDate,
	                        [VehicleId] = a.VehicleId,[FrameNumber]=c.FrameNumber,[ModelName]=f.Name,[Color]=a.Warna,[Jenis]=a.Jenis,[Model]=a.Model,[Chassis] = a.ChassisModel,
	                        [ApplicationNumber] = a.ApplicationNumber,[Branch] = (a.AFIBranchCode+' - '+l.Name), 
	                        [CustomerName] = a.Name,[KTP]=a.KTP,[Address1]=a.Address1,[Address2]=a.Address2,[Address3]=a.Address3,[City]=a.City,[Province]=a.Province,[PostalCode]=a.PostalCode,[TanggalEfektif]=a.EffectiveUntil,[Region]= (i.AFIRegionCode+' - '+i.Name)
	                        ,[ReferenceNumber]= a.FakturNumber,
	                        [TanggalAjuAFI]=a.Timestamp,
	                        [TipePengajuan] = k.AFISubmissionTypeEnumId,[TipePengajuanName]= k.Name
                        FROM AfiApplication a
                        LEFT JOIN AFIApplication b ON(a.AFIApplicationId < b.AFIApplicationId AND a.VehicleId = b.VehicleId)
                        JOIN Vehicle c ON c.VehicleId = a.VehicleId
                        JOIN CarType d ON d.Katashiki = c.Katashiki AND d.Suffix = c.Suffix
                        JOIN CarSeries e ON e.CarSeriesCode = d.CarSeriesCode
                        JOIN CarModel f ON f.CarModelCode = e.CarModelCode
                        JOIN DeliveryOrderDetail h ON h.VehicleId = a.VehicleId
                        JOIN AFIRegion i ON i.AFIRegionCode = a.AFIRegionCode
                        JOIN AFIBranch j ON j.AFIBranchCode = a.AFIBranchCode
                        JOIN AFISubmissionTypeEnum k ON k.AFISubmissionTypeEnumId = a.AFISubmissionTypeEnumId
                        JOIN Branch l ON l.BranchCode = j.BranchCode
						JOIN Company m on m.CompanyCode = l.CompanyCode
                        WHERE a.AFIApplicationProcessEnumId IN (1,5,9) AND h.CancelledAt IS NOT NULL AND b.AFIApplicationId IS NULL
						AND FrameNumber = @FrameNumber
                        AND m.DealerCode = @dealerCode";
                return (await this.LogisticDbContext.QueryAsync<AfiGridViewModel>(query, new { FrameNumber = model.frameNo, dealerCode = dealerCode })).Select(Q => { Q.TanggalAjuAFI = Q.TanggalAjuAFI.ToLocalTime(); return Q; }).ToList();
            }
            else
            {
                query = @"SELECT [AfiApplicationId] = a.AfiApplicationId,[DODate]=h.IssuedDate,
	                        [VehicleId] = a.VehicleId,[FrameNumber]=c.FrameNumber,[ModelName]=f.Name,[Color]=a.Warna,[Jenis]=a.Jenis,[Model]=a.Model,[Chassis] = a.ChassisModel,
	                        [ApplicationNumber] = a.ApplicationNumber,[Branch] = (a.AFIBranchCode+' - '+l.Name), 
	                        [CustomerName] = a.Name,[KTP]=a.KTP,[Address1]=a.Address1,[Address2]=a.Address2,[Address3]=a.Address3,[City]=a.City,[Province]=a.Province,[PostalCode]=a.PostalCode,[TanggalEfektif]=a.EffectiveUntil,[Region]= (i.AFIRegionCode+' - '+i.Name)
	                        ,[ReferenceNumber]= a.FakturNumber,
	                        [TanggalAjuAFI]=a.Timestamp,
	                        [TipePengajuan] = k.AFISubmissionTypeEnumId,[TipePengajuanName]= k.Name
                        FROM AfiApplication a
                        LEFT JOIN AFIApplication b ON(a.AFIApplicationId < b.AFIApplicationId AND a.VehicleId = b.VehicleId)
                        JOIN Vehicle c ON c.VehicleId = a.VehicleId
                        JOIN CarType d ON d.Katashiki = c.Katashiki AND d.Suffix = c.Suffix
                        JOIN CarSeries e ON e.CarSeriesCode = d.CarSeriesCode
                        JOIN CarModel f ON f.CarModelCode = e.CarModelCode
                        JOIN DeliveryOrderDetail h ON h.VehicleId = a.VehicleId
                        JOIN AFIRegion i ON i.AFIRegionCode = a.AFIRegionCode
                        JOIN AFIBranch j ON j.AFIBranchCode = a.AFIBranchCode
                        JOIN AFISubmissionTypeEnum k ON k.AFISubmissionTypeEnumId = a.AFISubmissionTypeEnumId
                        JOIN Branch l ON l.BranchCode = j.BranchCode
						JOIN Company m on m.CompanyCode = l.CompanyCode
                        WHERE  (@BranchCode IS NULL OR (a.AFIBranchCode = @BranchCode))
                        AND ((@TipePengajuan = 'Normal' AND a.AFISubmissionTypeEnumId = 1)
	                        OR
	                        (@TipePengajuan = 'Revisi' AND a.AFISubmissionTypeEnumId IN(2,3,4,5,6,7))
	                        OR
	                        (@TipePengajuan = 'Ex-Cancel' AND a.AFISubmissionTypeEnumId = 8))
                        AND ((@type = 'CBU' AND (FrameNumber NOT LIKE '%MHK%' AND FrameNumber NOT LIKE '%MHF%'))
	                        OR
	                        (@type = 'MHK' AND FrameNumber LIKE '%MHK%')
	                        OR
	                        (@type = 'CKD' AND FrameNumber LIKE '%MHF%'))
                        AND ((@tanggalPengajuan IS NULL AND @sampai IS NULL)  
                        OR 
                        ((@tanggalPengajuan IS NULL AND @sampai IS NOT NULL) AND (a.[Timestamp] < @sampai))
                        OR
                        ((@tanggalPengajuan IS NOT NULL AND @sampai IS NULL) AND (a.[Timestamp] > @tanggalPengajuan))
                        OR
                        ((@tanggalPengajuan IS NOT NULL AND @sampai IS NOT NULL) AND (a.[Timestamp] BETWEEN @tanggalPengajuan AND @sampai)))
                        AND a.AFIApplicationProcessEnumId IN (1,5,9)
                            AND h.CancelledAt IS NOT NULL AND b.AFIApplicationId IS NULL
                        AND m.DealerCode = @dealerCode";
                return (await this.LogisticDbContext.QueryAsync<AfiGridViewModel>(query,
                    new
                    {
                        BranchCode = model.branch?.AFIBranchCode ?? null,
                        tanggalPengajuan = model.tanggalPengajuan?.Date ?? null,
                        sampai = model.sampai?.Date ?? null,
                        TipePengajuan = model.statusPengajuan,
                        type = model.type,
                        dealerCode = dealerCode
                    })).Select(Q => { Q.TanggalAjuAFI = Q.TanggalAjuAFI.ToLocalTime(); return Q; }).ToList();
            }
        }
    }
}
