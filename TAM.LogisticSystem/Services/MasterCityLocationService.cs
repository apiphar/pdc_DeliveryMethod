using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MasterCityLocationService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public MasterCityLocationService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        public async Task<List<MasterCityLocationViewModel>> GetAllTableData()
        {
            var tableData = await LogisticDbContext.CityForLeg.Select(Q => new MasterCityLocationViewModel
            {
                KodeCityLocation = Q.CityForLegCode,
                CityLocation = Q.Name
            }).ToListAsync();
            return tableData;
        }

        public async Task<CityForLeg> CheckCode(string code)
        {
            var checkCode = await LogisticDbContext.CityForLeg.FirstOrDefaultAsync(Q => Q.CityForLegCode == code.ToUpper());
            return checkCode;
        }

        public async Task DeleteData(string id)
        {
            var deletedCityLocation = await LogisticDbContext.CityForLeg.FirstOrDefaultAsync(Q => Q.CityForLegCode == id);

            this.LogisticDbContext.CityForLeg.Remove(deletedCityLocation);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task CreateData(MasterCityLocationViewModel masterCityLocationViewModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var newCityLocation = new CityForLeg()
            {
                CityForLegCode = masterCityLocationViewModel.KodeCityLocation.Trim(' ').ToUpper(),
                Name = masterCityLocationViewModel.CityLocation.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username
            };

            this.LogisticDbContext.CityForLeg.Add(newCityLocation);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task UpdateData(MasterCityLocationViewModel masterCityLocationViewModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var updateCityLocation = await LogisticDbContext.CityForLeg.FirstOrDefaultAsync(Q => Q.CityForLegCode == masterCityLocationViewModel.KodeCityLocation);
            updateCityLocation.Name = masterCityLocationViewModel.CityLocation.ToUpper();
            updateCityLocation.UpdatedAt = DateTimeOffset.UtcNow;
            updateCityLocation.UpdatedBy = username;

            this.LogisticDbContext.CityForLeg.Update(updateCityLocation);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
