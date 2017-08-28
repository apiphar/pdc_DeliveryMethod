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
    public class PolaRangkaianTahapAkhirService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public PolaRangkaianTahapAkhirService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<PolaRangkaianTahapAkhirViewModel> GetDataProcessTailTemplate()
        {
            var allData = new PolaRangkaianTahapAkhirViewModel()
            {
                RoutingDictionaryTail = await LogisticDbContext.ProcessTailTemplate.ToListAsync(),
                RoutingDictionaryTailDetail = await this.GetDataRoutingDictionaryTailDetail(),
                Location = await LogisticDbContext.Location.ToListAsync(),
                DeliveryMethod = await LogisticDbContext.DeliveryMethod.ToListAsync(),
                RoutingMaster = await LogisticDbContext.ProcessMaster.ToListAsync()
            };

            return allData;
        }

        public async Task<List<RoutingDictionaryTailDetailModel>> GetDataRoutingDictionaryTailDetail()
        {
            _ = nameof(ProcessTailTemplateDetail.ProcessTailTemplateCode);
            _ = nameof(ProcessTailTemplateDetail.Ordering);
            _ = nameof(ProcessMaster.ProcessMasterCode);
            _ = nameof(ProcessMaster.Name);
            _ = nameof(DeliveryMethod.DeliveryMethodCode);
            _ = nameof(DeliveryMethod.Name);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);

            var query = @"SELECT
	                [RoutingDictionaryTailCode] = ProcessTailTemplateCode,
	                Ordering,
	                rdhd.ProcessMasterCode,
	                [RoutingMasterName] = rm.[Name],
	                dm.DeliveryMethodCode,
	                [DeliveryMethodName] = dm.[Name],
	                l.LocationCode,
                    [LocationName] = l.[Name]
                FROM ProcessTailTemplateDetail rdhd
	                JOIN ProcessMaster rm ON rdhd.ProcessMasterCode = rm.ProcessMasterCode
	                LEFT JOIN DeliveryMethod dm ON rdhd.DeliveryMethodCode = dm.DeliveryMethodCode
                    JOIN [Location] l ON rdhd.LocationCode = l.LocationCode";

            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<RoutingDictionaryTailDetailModel>(query);
            return result.ToList();
        }

        internal async Task<int> Add(InsertPolaHeaderTailModel header, List<InsertPolaDetailTailModel> detail)
        {
            var row = 0;
            await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await LogisticDbContext.Database.BeginTransactionAsync();
                {
                    var existingHeader = await LogisticDbContext.ProcessTailTemplate.Where(x => x.ProcessTailTemplateCode == header.RoutingDictionaryTailCode.ToUpper()).FirstOrDefaultAsync();
                    if (existingHeader != null)
                    {
                        existingHeader.Description = header.Description.ToUpper();
                        existingHeader.UpdatedBy = WebEnvService.UserHumanName;
                        existingHeader.UpdatedAt = DateTimeOffset.UtcNow;

                        var existingDetail = LogisticDbContext.ProcessTailTemplateDetail.Where(x => x.ProcessTailTemplateCode == header.RoutingDictionaryTailCode).ToList();
                        if (existingDetail != null)
                        {
                            for (var i = existingDetail.Count - 1; i >= 0; i--)
                            {
                                LogisticDbContext.Remove(existingDetail[i]);
                            }
                            row += LogisticDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        var insertHead = new ProcessTailTemplate
                        {
                            ProcessTailTemplateCode = header.RoutingDictionaryTailCode.ToUpper(),
                            Description = header.Description.ToUpper(),
                            CreatedBy = WebEnvService.UserHumanName,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = WebEnvService.UserHumanName,
                            UpdatedAt = DateTimeOffset.UtcNow
                        };
                        LogisticDbContext.Add(insertHead);
                    }

                    foreach (var detailData in detail)
                    {
                        var insertDetail = new ProcessTailTemplateDetail
                        {
                            ProcessTailTemplateCode = header.RoutingDictionaryTailCode.ToUpper(),
                            ProcessMasterCode = detailData.ProcessMasterCode.ToUpper(),
                            DeliveryMethodCode = detailData.DeliveryMethodCode?.ToUpper(),
                            LocationCode = detailData.LocationCode.ToUpper(),
                            Ordering = detailData.Ordering,
                            CreatedBy = WebEnvService.UserHumanName,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = WebEnvService.UserHumanName,
                            UpdatedAt = DateTimeOffset.UtcNow
                        };
                        LogisticDbContext.Add(insertDetail);
                    }
                    row += LogisticDbContext.SaveChanges();
                    trans.Commit();
                }
            });

            return row;
        }

        internal async Task<int> RemoveHeader(string processTailTemplateCode)
        {
            var rowsAffected = 0;
            await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await LogisticDbContext.Database.BeginTransactionAsync();
                {
                    var detail = await LogisticDbContext.ProcessTailTemplateDetail.Where(x => x.ProcessTailTemplateCode == processTailTemplateCode).ToListAsync();
                    foreach (var detailData in detail)
                    {
                        LogisticDbContext.Remove(detailData);
                    }

                    rowsAffected += await LogisticDbContext.SaveChangesAsync();
                    //===============================================
                    var mapping = await LogisticDbContext.ProcessTailTemplateMapping.Where(x => x.ProcessTailTemplateCode == processTailTemplateCode).ToListAsync();

                    foreach (var mappingData in mapping)
                    {
                        LogisticDbContext.Remove(mappingData);
                    }

                    rowsAffected += await LogisticDbContext.SaveChangesAsync();
                    //===============================================
                    var existingData = await LogisticDbContext.ProcessTailTemplate.Where(x => x.ProcessTailTemplateCode == processTailTemplateCode).FirstOrDefaultAsync();
                    if (existingData != null)
                    {
                        LogisticDbContext.Remove(existingData);
                    }

                    rowsAffected += await LogisticDbContext.SaveChangesAsync();
                    trans.Commit();
                }
            });

            return rowsAffected;
        }
    }
}
