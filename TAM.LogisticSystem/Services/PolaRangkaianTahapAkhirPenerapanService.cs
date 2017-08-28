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
    public class PolaRangkaianTahapAkhirPenerapanService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public PolaRangkaianTahapAkhirPenerapanService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<PolaRangkaianTahapAkhirPenerapanViewModel> GetDataProcessHeadMapping()
        {
            var AllData = new PolaRangkaianTahapAkhirPenerapanViewModel()
            {
                RoutingDictionaryTail = await LogisticDbContext.ProcessTailTemplate.ToListAsync(),
                CarModel = await this.GetDataCarModel(),
                CarSeries = await this.GetDataCarSeries(),
                CarType = await LogisticDbContext.CarType.ToListAsync(),
                RoutingDictionaryTailVehicleMapping = await this.GetDataProcessTailTemplateMapping(),

                Dealer = await this.GetDataDealer(),
                Branch = await this.GetDataBranch()
            };

            return AllData;
        }

        public async Task<List<CarModel>> GetDataCarModel()
        {
            _ = nameof(CarModel);

            var query = @"SELECT DISTINCT
	                cm.* 
                FROM CarModel cm
	                JOIN CarSeries cs ON cm.CarModelCode = cs.CarModelCode
	                JOIN CarType ct ON cs.CarSeriesCode = ct.CarSeriesCode";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<CarModel>(query);
            return result.ToList();
        }

        public async Task<List<CarSeries>> GetDataCarSeries()
        {
            _ = nameof(CarSeries);

            var query = @"SELECT DISTINCT
	                cs.* 
                FROM CarSeries cs
	                JOIN CarType ct ON cs.CarSeriesCode = ct.CarSeriesCode";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<CarSeries>(query);
            return result.ToList();
        }

        public async Task<List<ProcessTailTemplateMapping>> GetDataProcessTailTemplateMapping()
        {
            var query = @"SELECT 
                    ct.CarSeriesCode, 
	                phtm.*
                FROM ProcessTailTemplateMapping phtm
                    JOIN CarType ct ON phtm.Katashiki = ct.Katashiki AND phtm.Suffix = ct.Suffix";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<ProcessTailTemplateMapping>(query);
            return result.ToList();
        }

        public async Task<List<Dealer>> GetDataDealer()
        {
            var query = @"SELECT DISTINCT
                    d.*
                FROM Dealer d
                    JOIN Company c ON d.DealerCode = c.DealerCode
                    JOIN Branch b ON c.CompanyCode = b.CompanyCode";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<Dealer>(query);
            return result.ToList();
        }

        public async Task<List<CompanyJoinBranchModel>> GetDataBranch()
        {
            var query = @"SELECT
                    c.DealerCode,
                    b.BranchCode,
                    [BranchName] = b.[Name]
                from Company c join Branch b on c.CompanyCode= b.CompanyCode";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<CompanyJoinBranchModel>(query);
            return result.ToList();
        }

        internal List<ProcessTailTemplateMapping> GenerateTailVehicleMapping(string processTailTemplateCode, List<CarType> carType, List<CompanyJoinBranchModel> branch)
        {
            var newGenerate = new List<ProcessTailTemplateMapping>();
            foreach (var katsu in carType)
            {
                foreach (var br in branch)
                {
                    var newGen = new ProcessTailTemplateMapping
                    {
                        ProcessTailTemplateCode = processTailTemplateCode.ToUpper(),
                        Katashiki = katsu.Katashiki.ToUpper(),
                        Suffix = katsu.Suffix.ToUpper(),
                        BranchCode = br.BranchCode.ToUpper(),
                        CreatedBy = WebEnvService.UserHumanName,
                        CreatedAt = DateTimeOffset.UtcNow,
                        UpdatedBy = WebEnvService.UserHumanName,
                        UpdatedAt = DateTimeOffset.UtcNow
                    };
                    newGenerate.Add(newGen);
                }
            }
            return newGenerate;
        }

        internal async Task<int> Add(string processTailTemplateCode, List<CarType> carType, List<CompanyJoinBranchModel> branch)
        {
            var newGenerate = GenerateTailVehicleMapping(processTailTemplateCode, carType, branch);
            var oldGenerate = LogisticDbContext.ProcessTailTemplateMapping.Where(x => x.ProcessTailTemplateCode == processTailTemplateCode).ToList();

            var maxIteration = (newGenerate.Count < oldGenerate.Count) ? newGenerate.Count : oldGenerate.Count;
            for (var i = 0; i < maxIteration; i++)
            {
                oldGenerate[i].BranchCode = newGenerate[i].BranchCode.ToUpper();
                oldGenerate[i].Katashiki = newGenerate[i].Katashiki.ToUpper();
                oldGenerate[i].Suffix = newGenerate[i].Suffix.ToUpper();
                oldGenerate[i].CreatedBy = WebEnvService.UserHumanName;
                oldGenerate[i].CreatedAt = DateTimeOffset.UtcNow;
                oldGenerate[i].UpdatedBy = WebEnvService.UserHumanName;
                oldGenerate[i].UpdatedAt = DateTimeOffset.UtcNow;
            }
            LogisticDbContext.UpdateRange(oldGenerate);
            if (newGenerate.Count < oldGenerate.Count)
            {
                for (var i = newGenerate.Count; i < oldGenerate.Count; i++)
                {
                    LogisticDbContext.Remove(oldGenerate[i]);
                }
            }
            else if (newGenerate.Count > oldGenerate.Count)
            {
                for (var i = oldGenerate.Count; i < newGenerate.Count; i++)
                {
                    LogisticDbContext.Add(newGenerate[i]);
                }
            }

            return await LogisticDbContext.SaveChangesAsync();
        }
    }
}
