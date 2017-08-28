using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class SalesAreaService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public SalesAreaService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<List<SalesAreaViewModel>> GetData()
        {
            return await LogisticDbContext.SalesArea.Select(Q => new SalesAreaViewModel {
                SalesAreaCode = Q.SalesAreaCode,
                Description = Q.Description
            }).ToListAsync();
        }
        internal async Task<bool> Add(string areaId, string description)
        {
            var existingCode = await LogisticDbContext.SalesArea.FirstOrDefaultAsync(Q => Q.SalesAreaCode == areaId);
            if (existingCode != null)
            {
                return false;
            }

            var insert = new SalesArea
            {
                SalesAreaCode = areaId.ToUpper(),
                Description = description.ToUpper(),
                CreatedBy = WebEnvService.UserHumanName,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = WebEnvService.UserHumanName,
                UpdatedAt = DateTime.UtcNow
            };

            LogisticDbContext.Add(insert);
            await LogisticDbContext.SaveChangesAsync();
            return true;
        }

        internal async Task<bool> Update(string id, string description)
        {
            var existingSalesArea = await LogisticDbContext.SalesArea.Where(x => x.SalesAreaCode == id).FirstOrDefaultAsync();
            if (existingSalesArea != null)
            {
                existingSalesArea.Description = description.ToUpper();
                existingSalesArea.UpdatedBy = WebEnvService.UserHumanName;
                existingSalesArea.UpdatedAt = DateTime.UtcNow;
            }
            if (await LogisticDbContext.SaveChangesAsync() == 1) return true;
            else return false;
        }

        internal async Task<bool> Remove(string id)
        {
            var existingSalesArea = await LogisticDbContext.SalesArea.Where(x => x.SalesAreaCode == id).FirstOrDefaultAsync();
            if (existingSalesArea != null)
            {
                LogisticDbContext.Remove(existingSalesArea);
            }
            if (await LogisticDbContext.SaveChangesAsync() == 1) return true;
            else return false;
        }
    }
}
