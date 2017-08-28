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
    public class AfiReceiveDocumentService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService _env;

        public AfiReceiveDocumentService(LogisticDbContext logisticDbContext, WebEnvironmentService e)
        {
            this.LogisticDbContext = logisticDbContext;
            this._env = e;
        }

        public async Task<bool> IsProcessTAM(string framenumber)
        {
            var query = @"SELECT COUNT(a.AFIApplicationId)
                        FROM 
                        AfiApplication a
                        LEFT JOIN AFIApplication b ON(a.AFIApplicationId < b.AFIApplicationId AND a.VehicleId = b.VehicleId)
                        JOIN Vehicle c ON c.VehicleId = a.VehicleId
                        WHERE b.AFIApplicationId IS NULL AND a.AFIApplicationProcessEnumId IN(4,8,12)
                        AND c.FrameNumber = @framenumber";
            var result =  await this.LogisticDbContext.QueryAsync<int>(query, new { framenumber = framenumber });
            return result.FirstOrDefault() == 1;
        }

        public async Task<int> ReceiveDocument(AfiReceiveDocumentUpdate Data)
        {
            var ApplicationIdList = new List<int>();
            var row = 0;
            await this.LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async()=>
            {
                var trans = await this.LogisticDbContext.Database.BeginTransactionAsync();
                {
                    ApplicationIdList.AddRange(Data.AfiApplicationList.Select(Q => Q.AfiApplicationId));
                    var AfiApplicationList = await this.LogisticDbContext.AFIApplication.Where(Q => ApplicationIdList.Contains(Q.AFIApplicationId)).ToListAsync();
                    AfiApplicationList.ForEach(
                        a =>
                        {
                            a.UpdatedAt = DateTimeOffset.UtcNow;
                            a.UpdatedBy = this._env.UserHumanName;
                            a.TamReceivedAt = DateTimeOffset.UtcNow;
                            a.AFIApplicationProcessEnumId = Data.rbStatus == "Revisi" ? 13 : (Data.rbStatus == "Ex-Cancel" ? 14 : a.AFIApplicationProcessEnumId);
                        }
                    );
                    row = await this.LogisticDbContext.SaveChangesAsync();
                    trans.Commit();
                }
            });
            return row;
        }
        public async Task<AfiGridViewModel> GetReceiveDocument(string frameNoInput)
        {
            _ = nameof(AFIApplication.AFIApplicationId);
            _ = nameof(DeliveryOrderDetail.VehicleId);
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

            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<AfiGridViewModel>(
                @"
                            SELECT  [AfiApplicationId] = a.AfiApplicationId,
                                    [DODate]=h.IssuedDate,
	                                [VehicleId] = a.VehicleId,
                                    [FrameNumber]=c.FrameNumber,
                                    [ModelName]=f.Name,
                                    [Color]=a.Warna,
                                    [Jenis]=a.Jenis,
                                    [Model]=a.Model,
                                    [Chassis] = a.ChassisModel,
                                    [ApplicationNumber] = a.ApplicationNumber,
                                    [Branch] = (a.AFIBranchCode+' - '+l.Name), 
                                    [CustomerName] = a.Name,
                                    [KTP]=a.KTP,
                                    [Address1]=a.Address1,
                                    [Address2]=a.Address2,
                                    [Address3]=a.Address3,
                                    [City]=a.City,
                                    [Province]=a.Province,
                                    [PostalCode]=a.PostalCode,
                                    [TanggalEfektif]=a.EffectiveUntil,
                                    [Region]= (i.AFIRegionCode+' - '+i.Name),
                                    [ReferenceNumber]= a.FakturNumber,
	                                [TanggalAjuAFI]=a.Timestamp,
	                                [TipePengajuan] = k.AFISubmissionTypeEnumId,[TipePengajuanName]= k.Name
                            FROM 
                                    AfiApplication a
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
                            WHERE 
                                    a.AFIApplicationProcessEnumId IN (4,8,12)
                                    AND (SELECT COUNT(NomorSuratPengantarFaktur) FROM SuratPengantarFakturDetail WHERE VehicleId = a.VehicleId) = 0
                                    AND a.TamReceivedAt IS NULL AND b.AFIApplicationId IS NULL and h.CancelledAt IS NOT NULL
							        AND c.FrameNumber = @frameNo
                ", new { frameNo = frameNoInput })).FirstOrDefault();

            return data;
        }
    }
}
