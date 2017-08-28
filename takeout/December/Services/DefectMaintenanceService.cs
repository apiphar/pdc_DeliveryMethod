using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class DefectMaintenanceService
    {
        private readonly LogisticDbContext logisticDbContext;

        public DefectMaintenanceService (LogisticDbContext logisticDbContext)
        {
            this.logisticDbContext = logisticDbContext;
        }

        public async Task<DefectMaintenanceSearchResult> Search(DefectMaintenanceSearchParameters SearchParameter)
        {
            var query = logisticDbContext.Defect.AsNoTracking();

            if (string.IsNullOrEmpty(SearchParameter.Name) == false)
            {
                query = query.Where(Q => Q.Name.Contains(SearchParameter.Name));
            }
            var count = await query.CountAsync();
            var skip = (SearchParameter.Page - 1) * SearchParameter.ItemsPerPage;
            var paged = await query.OrderBy(Q => Q.Name).Skip(skip).Take(SearchParameter.ItemsPerPage).ToListAsync();

            var result = new DefectMaintenanceSearchResult(SearchParameter, count, paged);
            return result;
        }

        public async Task<List<Defect>> GetData()
        {
            var data = await logisticDbContext.Defect.ToListAsync();
            return data;
        }

        public async Task<int> Add(DefectMaintenanceViewModel model)
        {
            logisticDbContext.Add(new Defect
            {
                Name = model.Name,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = model.CreatedBy = "Jati",
                UpdatedBy = model.UpdatedBy = "Jati"

            });
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task<Defect> Get(string id)
        {
            return await logisticDbContext.Defect.FirstOrDefaultAsync(m => m.DefectCode == id);
        }

        public async Task<int> Edit(string id, DefectMaintenanceViewModel model)
        {
            var existingDefect = await logisticDbContext.Defect.Where(x => x.DefectCode == id).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (existingDefect != null)
            {
                existingDefect.Name = model.Name;
                existingDefect.UpdatedAt = DateTime.UtcNow;
                existingDefect.UpdatedBy = "Jati";

                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }

            return rowsAffected;
        }

        public async Task<int> Remove(Defect entity)
        {
            logisticDbContext.Remove(entity);
            return await logisticDbContext.SaveChangesAsync();
        }
    }
}
