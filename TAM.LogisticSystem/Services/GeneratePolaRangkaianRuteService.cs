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
    public class GeneratePolaRangkaianRuteService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public GeneratePolaRangkaianRuteService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<List<GeneratePolaRangkaianRuteViewModel>> GetDataGeneratePolaRangkaianRute()
        {
            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(ProcessHeadTemplateMapping.Katashiki);
            _ = nameof(ProcessHeadTemplateMapping.Suffix);
            _ = nameof(ProcessHeadTemplateMapping.ProcessHeadTemplateCode);
            _ = nameof(ProcessTailTemplateMapping.ProcessTailTemplateCode);

            var query = @"SELECT
                    rdtvm.BranchCode,
                    [BranchName] = b.[Name],
                    rdhvm.Katashiki,
                    rdhvm.Suffix,
                    [KodeTahapAwal] = rdhvm.ProcessHeadTemplateCode,
                    [KodeTahapAkhir] = rdtvm.ProcessTailTemplateCode
                FROM Branch b
                    JOIN ProcessTailTemplateMapping rdtvm ON b.BranchCode = rdtvm.BranchCode
                    JOIN ProcessHeadTemplateMapping rdhvm ON rdtvm.Katashiki = rdhvm.Katashiki AND rdtvm.Suffix = rdhvm.Suffix
                ORDER BY b.BranchCode";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<GeneratePolaRangkaianRuteViewModel>(query);
            return result.ToList();
        }
        internal async Task<int> Add(List<GeneratePolaRangkaianRuteViewModel> data, DateTime validFrom)
        {
            _ = nameof(ProcessHeadTemplateDetail.ProcessMasterCode);
            _ = nameof(ProcessHeadTemplateDetail.LocationCode);
            _ = nameof(ProcessHeadTemplateDetail.DeliveryMethodCode);
            _ = nameof(ProcessHeadTemplateDetail.Ordering);
            _ = nameof(ProcessTailTemplateDetail.ProcessMasterCode);
            _ = nameof(ProcessTailTemplateDetail.LocationCode);
            _ = nameof(ProcessTailTemplateDetail.DeliveryMethodCode);
            _ = nameof(ProcessTailTemplateDetail.Ordering);

            var row = 0;
            await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await LogisticDbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
                {
                    foreach (var generate in data)
                    {
                        var insertHeader = LogisticDbContext.ProcessDictionary.Where(
                            x => x.BranchCode == generate.BranchCode &&
                            x.Katashiki == generate.Katashiki &&
                            x.Suffix == generate.Suffix
                            ).FirstOrDefault();

                        if (insertHeader == null)
                        {
                            insertHeader = new ProcessDictionary
                            {
                                BranchCode = generate.BranchCode,
                                Katashiki = generate.Katashiki,
                                Suffix = generate.Suffix,
                                ValidFrom = validFrom,
                                CreatedBy = WebEnvService.UserHumanName,
                                CreatedAt = DateTimeOffset.UtcNow,
                                UpdatedBy = WebEnvService.UserHumanName,
                                UpdatedAt = DateTimeOffset.UtcNow
                            };
                            LogisticDbContext.Add(insertHeader);
                        }
                        else
                        {
                            insertHeader.ValidFrom = validFrom;
                            insertHeader.UpdatedBy = WebEnvService.UserHumanName;
                            insertHeader.UpdatedAt = DateTimeOffset.UtcNow;
                        }
                        row += await LogisticDbContext.SaveChangesAsync();
                        
                        var query = @"SELECT DISTINCT
                                [RoutingMasterCode] = rdhd.ProcessMasterCode,
                                rdhd.LocationCode,
                                rdhd.DeliveryMethodCode,
                                Ordering
                            FROM ProcessHeadTemplate rdh
                                JOIN ProcessHeadTemplateDetail rdhd ON rdh.ProcessHeadTemplateCode = rdhd.ProcessHeadTemplateCode
                                JOIN ProcessHeadTemplateMapping rdhvm ON rdhvm.ProcessHeadTemplateCode = rdh.ProcessHeadTemplateCode
                            WHERE 
                                rdhvm.Katashiki = @Katashiki AND 
                                rdhvm.Suffix = @Suffix AND 
                                rdh.ProcessHeadTemplateCode = @ProcessHeadTemplateCode
                            UNION
                            SELECT DISTINCT
                                [RoutingMasterCode] = rdtd.ProcessMasterCode,
                                rdtd.LocationCode,
                                rdtd.DeliveryMethodCode,
                                Ordering = Ordering + LastOrdering.[Last]
                            FROM ProcessTailTemplate rdt
                                JOIN ProcessTailTemplateMapping rdtvm ON rdtvm.ProcessTailTemplateCode = rdt.ProcessTailTemplateCode
                                JOIN ProcessTailTemplateDetail rdtd ON rdt.ProcessTailTemplateCode = rdtd.ProcessTailTemplateCode,
                                (
							        SELECT DISTINCT TOP 1
							        'Last' = Ordering
							        FROM ProcessHeadTemplate rdh
							        JOIN ProcessHeadTemplateDetail rdhd ON rdh.ProcessHeadTemplateCode = rdhd.ProcessHeadTemplateCode
							        JOIN ProcessHeadTemplateMapping rdhvm ON rdhvm.ProcessHeadTemplateCode = rdh.ProcessHeadTemplateCode
							        WHERE 
                                        rdhvm.Katashiki = @Katashiki AND 
                                        rdhvm.Suffix = @Suffix AND
                                        rdh.ProcessHeadTemplateCode = @ProcessHeadTemplateCode
							        ORDER BY Ordering DESC
                                )AS LastOrdering
                            WHERE 
                                rdtvm.Katashiki = @Katashiki AND 
                                rdtvm.Suffix = @Suffix AND
                                rdt.ProcessTailTemplateCode = @ProcessTailTemplateCode
                            ORDER BY Ordering";
                        var result = LogisticDbContext.Query<GeneratePolaRangkaianRuteInsertModel>(query, new {
                            Katashiki = insertHeader.Katashiki,
                            Suffix = insertHeader.Suffix,
                            ProcessHeadTemplateCode = generate.KodeTahapAwal,
                            ProcessTailTemplateCode = generate.KodeTahapAkhir
                        }).ToList();
                        
                        var detail = LogisticDbContext.ProcessDictionaryDetail.Where(x => x.ProcessDictionaryId == insertHeader.ProcessDictionaryId).ToList();
                        LogisticDbContext.ProcessDictionaryDetail.RemoveRange(detail);
                        row += await LogisticDbContext.SaveChangesAsync();

                        foreach (var generateDetail in result)
                        {
                            var insertDetail = new ProcessDictionaryDetail
                            {
                                ProcessDictionaryId = insertHeader.ProcessDictionaryId,
                                LocationCode = generateDetail.LocationCode,
                                ProcessMasterCode = generateDetail.RoutingMasterCode,
                                DeliveryMethodCode = generateDetail.DeliveryMethodCode,
                                Ordering = generateDetail.Ordering,
                                CreatedBy = WebEnvService.UserHumanName,
                                CreatedAt = DateTimeOffset.UtcNow,
                                UpdatedBy = WebEnvService.UserHumanName,
                                UpdatedAt = DateTimeOffset.UtcNow
                            };

                            LogisticDbContext.Add(insertDetail);
                        }
                        row += await LogisticDbContext.SaveChangesAsync();
                    }
                    trans.Commit();
                }
            });

            return row;
        }
    }
}
