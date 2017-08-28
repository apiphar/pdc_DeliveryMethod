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
    public class DwellingTimeService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public DwellingTimeService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        public async Task<List<DwellingTimeViewModel>> GetDwellingData()
        {
            _ = nameof(Dwelling.LocationFrom);
            _ = nameof(Dwelling.LocationTo);
            _ = nameof(Location.Name);
            var dwellingData = (await this.LogisticDbContext.Database.GetDbConnection()
                .QueryAsync<DwellingTimeViewModel>(@"
SELECT
d.LocationFrom, 
d.LocationTo, 
l.Name AS LocationNameFrom, 
ll.Name AS LocationNameTo, 
d.LeadMinutes
FROM Dwelling d
JOIN [Location] l ON d.LocationFrom = l.LocationCode
JOIN [Location] ll on d.LocationTo = ll.LocationCode")).ToList();
            return dwellingData;
        }

        public async Task<List<GetDwellingLocationViewModel>> GetLocationCode()
        {
            var locationData = await this.LogisticDbContext.Location
                .AsNoTracking()
                .Select(Q => new GetDwellingLocationViewModel
                {
                    LocationCode = Q.LocationCode,
                    Name = Q.Name
                }).ToListAsync();
            return locationData;
        }

        public async Task AddDwellingData(InsertDwellingViewModel model)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var insert = new Dwelling
            {
                LocationFrom = model.LocationFrom.ToUpper(),
                LocationTo = model.LocationTo.ToUpper(),
                LeadMinutes = model.LeadMinutes,
                CreatedBy = user,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = user,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            this.LogisticDbContext.Dwelling.Add(insert);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<bool> Validate(InsertDwellingViewModel model)
        {
            var data = await this.LogisticDbContext.Dwelling.FirstOrDefaultAsync(Q => Q.LocationFrom == model.LocationFrom && Q.LocationTo == model.LocationTo);
            if (data == null)
            {
                return true;
            }
            return false;
        }

        public async Task<int> UpdateDwellingData(InsertDwellingViewModel model)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var entity = await LogisticDbContext.Dwelling.Where(Q => Q.LocationFrom == model.LocationFrom && Q.LocationTo == model.LocationTo).FirstOrDefaultAsync();
            var rowsAffected = 0;

            if (entity != null)
            {
                entity.LeadMinutes = model.LeadMinutes;
                entity.UpdatedBy = user;
                entity.UpdatedAt = DateTimeOffset.UtcNow;

                rowsAffected = await LogisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }

        public async Task<int> RemoveDwellingData(string locationFrom, string locationTo)
        {
            var entity = await LogisticDbContext.Dwelling.Where(Q => Q.LocationFrom == locationFrom && Q.LocationTo == locationTo).FirstOrDefaultAsync();
            if (entity != null)
            {
                LogisticDbContext.Remove(entity);
            }
            var rowsAffected = await LogisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }
    }
}
