using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class RegionService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public RegionService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<List<Region>> GetData()
        {
            return await this.LogisticDbContext.Region.ToListAsync();
        }

        internal async Task<int> Remove(string id)
        {
            //-1 meant object has child. Not allow to remove this region
            if (this.LogisticDbContext.Region.Where(x => x.ParentRegionCode == id).Count() != 0)
                return -1;

            var existingRegion = await this.LogisticDbContext.Region.Where(x => x.RegionCode == id).FirstOrDefaultAsync();
            if (existingRegion != null)
            {
                this.LogisticDbContext.Remove(existingRegion);
            }

            return await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task<int> Update(string id,RegionViewModel model)
        {
            var existingRegion = await this.LogisticDbContext.Region.Where(x => x.RegionCode == id).FirstOrDefaultAsync();
            var rowsAffected = 0;
            if (existingRegion == null)
            {
                return -1;
            }
            existingRegion.Type = model.RegionType.ToUpper();
            existingRegion.Name = model.Name.ToUpper();
            existingRegion.ParentRegionCode = model.ParentCode?.RegionCode.ToUpper();
            existingRegion.PostCode = model.PostCode.ToUpper();
            existingRegion.UpdatedAt = DateTimeOffset.UtcNow;
            existingRegion.UpdatedBy = WebEnvService.UserHumanName;
            rowsAffected = await this.LogisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

    }
}
