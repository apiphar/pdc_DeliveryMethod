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
    public class PolaRangkaianTahapAwalService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public PolaRangkaianTahapAwalService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<PolaRangkaianTahapAwalViewModel> GetDataProcessHeadTemplate()
        {
            var allData = new PolaRangkaianTahapAwalViewModel()
            {
                RoutingDictionaryHead = await LogisticDbContext.ProcessHeadTemplate.ToListAsync(),
                RoutingDictionaryHeadDetail = await this.GetDataRoutingDictionaryHeadDetail(),
                Location = await LogisticDbContext.Location.ToListAsync(),
                DeliveryMethod = await LogisticDbContext.DeliveryMethod.ToListAsync(),
                RoutingMaster = await LogisticDbContext.ProcessMaster.ToListAsync()
            };

            return allData;
        }

        private async Task<List<RoutingDictionaryHeadDetailModel>> GetDataRoutingDictionaryHeadDetail()
        {
            _ = nameof(ProcessHeadTemplateDetail.ProcessHeadTemplateCode);
            _ = nameof(ProcessHeadTemplateDetail.Ordering);
            _ = nameof(ProcessMaster.ProcessMasterCode);
            _ = nameof(ProcessMaster.Name);
            _ = nameof(DeliveryMethod.DeliveryMethodCode);
            _ = nameof(DeliveryMethod.Name);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);

            var query = @"SELECT
	                [RoutingDictionaryHeadCode] = ProcessHeadTemplateCode,
	                Ordering,
	                rdhd.ProcessMasterCode,
	                [RoutingMasterName] = rm.[Name],
	                dm.DeliveryMethodCode,
	                [DeliveryMethodName] = dm.[Name],
	                l.LocationCode,
                    [LocationName] = l.[Name]
                FROM ProcessHeadTemplateDetail rdhd
	                JOIN ProcessMaster rm ON rdhd.ProcessMasterCode = rm.ProcessMasterCode
	                LEFT JOIN DeliveryMethod dm ON rdhd.DeliveryMethodCode = dm.DeliveryMethodCode
                    JOIN [Location] l ON rdhd.LocationCode = l.LocationCode";

            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<RoutingDictionaryHeadDetailModel>(query);
            return result.ToList();
        }

        internal async Task<int> Add(InsertPolaHeaderModel header, List<InsertPolaDetailModel> detail)
        {
            var row = 0;
            await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await LogisticDbContext.Database.BeginTransactionAsync();
                {
                    var existingHeader = await LogisticDbContext.ProcessHeadTemplate.Where(x => x.ProcessHeadTemplateCode == header.RoutingDictionaryHeadCode).FirstOrDefaultAsync();
                    if (existingHeader != null)
                    {
                        existingHeader.Description = header.Description;
                        existingHeader.UpdatedBy = WebEnvService.UserHumanName;
                        existingHeader.UpdatedAt = DateTimeOffset.UtcNow;

                        var existingDetail = LogisticDbContext.ProcessHeadTemplateDetail.Where(x => x.ProcessHeadTemplateCode == header.RoutingDictionaryHeadCode).ToList();
                        if (existingDetail != null)
                        {
                            for (var i = existingDetail.Count - 1; i >= 0; i--)
                            {
                                LogisticDbContext.Remove(existingDetail[i]);
                            }
                            LogisticDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        var insertHead = new ProcessHeadTemplate
                        {
                            ProcessHeadTemplateCode = header.RoutingDictionaryHeadCode,
                            Description = header.Description,
                            CreatedBy = WebEnvService.UserHumanName,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = WebEnvService.UserHumanName,
                            UpdatedAt = DateTimeOffset.UtcNow
                        };
                        LogisticDbContext.Add(insertHead);
                    }

                    foreach (var detailData in detail)
                    {
                        var insertDetail = new ProcessHeadTemplateDetail
                        {
                            ProcessHeadTemplateCode = header.RoutingDictionaryHeadCode,
                            ProcessMasterCode = detailData.ProcessMasterCode,
                            DeliveryMethodCode = detailData.DeliveryMethodCode,
                            LocationCode = detailData.LocationCode,
                            Ordering = detailData.Ordering,
                            CreatedBy = WebEnvService.UserHumanName,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = WebEnvService.UserHumanName,
                            UpdatedAt = DateTimeOffset.UtcNow
                        };
                        LogisticDbContext.Add(insertDetail);
                    }
                    row = LogisticDbContext.SaveChanges();
                    trans.Commit();
                }
            });

            return row;
        }

        internal async Task<int> RemoveHeader(string processHeadTemplateCode)
        {
            var rowsAffected = 0;
            var detail = await LogisticDbContext.ProcessHeadTemplateDetail.Where(x => x.ProcessHeadTemplateCode == processHeadTemplateCode).ToListAsync();
            foreach (var detailData in detail)
            {
                LogisticDbContext.Remove(detailData);
            }

            rowsAffected += await LogisticDbContext.SaveChangesAsync();
            //===============================================
            var mapping = await LogisticDbContext.ProcessHeadTemplateMapping.Where(x => x.ProcessHeadTemplateCode == processHeadTemplateCode).ToListAsync();

            foreach (var mappingData in mapping)
            {
                LogisticDbContext.Remove(mappingData);
            }

            rowsAffected += await LogisticDbContext.SaveChangesAsync();
            //===============================================
            var existingData = await LogisticDbContext.ProcessHeadTemplate.Where(x => x.ProcessHeadTemplateCode == processHeadTemplateCode).FirstOrDefaultAsync();
            if (existingData != null)
            {
                LogisticDbContext.Remove(existingData);
            }

            rowsAffected += await LogisticDbContext.SaveChangesAsync();

            return rowsAffected;
        }
    }
}
