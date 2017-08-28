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
    public class PolaRangkaianTahapAwalPenerapanService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public PolaRangkaianTahapAwalPenerapanService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<PolaRangkaianTahapAwalPenerapanViewModel> GetDataProcessHeadMapping()
        {
            var AllData = new PolaRangkaianTahapAwalPenerapanViewModel()
            {
                RoutingDictionaryHead = await LogisticDbContext.ProcessHeadTemplate.ToListAsync(),
                CarModel = await this.GetDataCarModel(),
                CarSeries = await this.GetDataCarSeries(),
                CarType = await LogisticDbContext.CarType.ToListAsync(),
                RoutingDictionaryHeadVehicleMapping = await this.GetDataProcessHeadTemplateMapping(),
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

        public async Task<List<ProcessHeadTemplateMapping>> GetDataProcessHeadTemplateMapping()
        {
            var query = @"SELECT 
                    ct.CarSeriesCode, 
	                phtm.*
                FROM ProcessHeadTemplateMapping phtm
                    JOIN CarType ct ON phtm.Katashiki = ct.Katashiki AND phtm.Suffix = ct.Suffix";
            var result = await LogisticDbContext.Database.GetDbConnection().QueryAsync<ProcessHeadTemplateMapping>(query);
            return result.ToList();
        }

        internal async Task<int> Add(string processHeadTemplateCode, List<CarType> carType)
        {
            var delete = LogisticDbContext.ProcessHeadTemplateMapping.Where(x => x.ProcessHeadTemplateCode == processHeadTemplateCode).ToList();
            LogisticDbContext.RemoveRange(delete);
            LogisticDbContext.SaveChanges();

            foreach (var katsu in carType)
            {
                var insert = new ProcessHeadTemplateMapping
                {
                    ProcessHeadTemplateCode = processHeadTemplateCode,
                    Katashiki = katsu.Katashiki,
                    Suffix = katsu.Suffix,
                    CreatedBy = WebEnvService.UserHumanName,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedBy = WebEnvService.UserHumanName,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                LogisticDbContext.Add(insert);
            }
            
            return await LogisticDbContext.SaveChangesAsync();
        }
    }
}
