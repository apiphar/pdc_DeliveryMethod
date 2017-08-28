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
    public class AfiRequestRevisiAndExCancelService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public AfiRequestRevisiAndExCancelService(LogisticDbContext logisticDbContext,WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }
        public async Task<IEnumerable<AfiRevisiAndExcCancelGridView>> GetRequestByFilter(AfiRequestRevisiSearchRO model)
        {
            var query = @"SELECT [AfiApplicationId] = a.AfiApplicationId,[DODate]=h.IssuedDate,
	                        [VehicleId] = a.VehicleId,[FrameNumber]=c.FrameNumber,[ModelCode]=f.CarModelCode,[ModelName]=f.Name,[Color]=a.Warna,[Jenis]=a.Jenis,[Model]=a.Model,[Chassis] = a.ChassisModel,
	                        [ApplicationNumber] = a.ApplicationNumber,[Branch] = (a.AFIBranchCode+' - '+l.Name), 
	                        [CustomerName] = a.Name,[KTP]=a.KTP,[Address1]=a.Address1,[Address2]=a.Address2,[Address3]=a.Address3,[City]=a.City,[Province]=a.Province,[PostalCode]=a.PostalCode,[TanggalEfektif]=a.EffectiveUntil,[Region]= (i.AFIRegionCode+' - '+i.Name)
	                        ,[ReferenceNumber]= a.FakturNumber,
	                        [TanggalAjuAFI]=a.Timestamp,
	                        [TipePengajuan] = k.AFISubmissionTypeEnumId,[TipePengajuanName]= k.Name
                            ,[ProcessId] = a.AFIApplicationProcessEnumId
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
                        JOIN BranchLocationMapping m ON m.BranchCode = j.BranchCode
                        WHERE b.AFIApplicationId IS NULL AND h.CancelledAt IS NOT NULL
                        AND ((@tanggalPengajuan IS NULL AND @sampai IS NULL)  
                        OR 
                        ((@tanggalPengajuan IS NULL AND @sampai IS NOT NULL) AND (a.[Timestamp] < @sampai))
                        OR
                        ((@tanggalPengajuan IS NOT NULL AND @sampai IS NULL) AND (a.[Timestamp] > @tanggalPengajuan))
                        OR
                        ((@tanggalPengajuan IS NOT NULL AND @sampai IS NOT NULL) AND (a.[Timestamp] BETWEEN @tanggalPengajuan AND @sampai)))
                        AND a.TamReceivedAt IS NOT NULL
                        AND ((@TipePengajuan = 'Revisi' AND a.AFIApplicationProcessEnumId = 13)
                        OR
                        (@TipePengajuan = 'Ex-Cancel' AND a.AFIApplicationProcessEnumId = 14))
                        AND m.LocationCode = @LocationCode";
            return await this.LogisticDbContext.QueryAsync<AfiRevisiAndExcCancelGridView>(query, new
            {
                tanggalPengajuan = model.TanggalPengajuan ?? null,
                sampai = model.Sampai ?? null,
                TipePengajuan = model.RbStatus,
                LocationCode = this.WebEnvironmentService.UserLocation
            });
        }
        public async Task<IEnumerable<AfiRevisiAndExcCancelGridView>> GetRequest(AfiRequestRevisiSearchRO model)
        {
            model.FrameNumber = model.FrameNumber != null ? (model.FrameNumber.Trim() != "" ? model.FrameNumber : null) : null;
            if (model.FrameNumber != null)
            {
                return await GetRequestByFrameNo(model.FrameNumber);
            }
            else
            {
                return await GetRequestByFilter(model);
            }
        }

        public async Task<IEnumerable<AfiRevisiAndExcCancelGridView>> GetRequestByFrameNo(string FrameNumber)
        {
            var query = @"SELECT [AfiApplicationId] = a.AfiApplicationId,[DODate]=h.IssuedDate,
	                        [VehicleId] = a.VehicleId,[FrameNumber]=c.FrameNumber,[ModelCode]=f.CarModelCode,[ModelName]=f.Name,[Color]=a.Warna,[Jenis]=a.Jenis,[Model]=a.Model,[Chassis] = a.ChassisModel,
	                        [ApplicationNumber] = a.ApplicationNumber,[Branch] = (a.AFIBranchCode+' - '+l.Name), 
	                        [CustomerName] = a.Name,[KTP]=a.KTP,[Address1]=a.Address1,[Address2]=a.Address2,[Address3]=a.Address3,[City]=a.City,[Province]=a.Province,[PostalCode]=a.PostalCode,[TanggalEfektif]=a.EffectiveUntil,[Region]= (i.AFIRegionCode+' - '+i.Name)
	                        ,[ReferenceNumber]= a.FakturNumber,
	                        [TanggalAjuAFI]=a.Timestamp,
	                        [TipePengajuan] = k.AFISubmissionTypeEnumId,[TipePengajuanName]= k.Name
                            ,[ProcessId] = a.AFIApplicationProcessEnumId
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
                        JOIN BranchLocationMapping m ON m.BranchCode = j.BranchCode
                        WHERE b.AFIApplicationId IS NULL AND h.CancelledAt IS NOT NULL
                        AND FrameNumber = @FrameNumber AND a.TamReceivedAt IS NOT NULL
                        AND a.AFIApplicationProcessEnumId IN(13,14) AND m.LocationCode = @LocationCode";
            return await this.LogisticDbContext.QueryAsync<AfiRevisiAndExcCancelGridView>(query, new { FrameNumber = FrameNumber,LocationCode = this.WebEnvironmentService.UserLocation });
        }
    }
}
