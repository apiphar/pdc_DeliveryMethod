using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class KodeShiftService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService web;

        public KodeShiftService(LogisticDbContext logisticDbContext, WebEnvironmentService w)
        {
            this.logisticDbContext = logisticDbContext;
            this.web = w;
        }

        public async Task<List<Shift>> GetData()
        {
            var data = await this.logisticDbContext.Shift.ToListAsync();
            return data;
        }
        public async Task<Shift> Get(string id)
        {
            return await this.logisticDbContext.Shift.FirstOrDefaultAsync(m => m.ShiftCode == id);
        }

        public async Task<int> Add(ShiftCodeViewModel model)
        {
            this.logisticDbContext.Shift.Add(new Shift
            {
                ShiftCode = model.ShiftCode.ToUpper(),
                Description = model.Description.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = this.web.UserHumanName,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = this.web.UserHumanName
            });
            return await this.logisticDbContext.SaveChangesAsync();
        }

        public async Task<int> Edit(string id, ShiftCodeUpdateViewModel model)
        {
            var existingShift = await this.logisticDbContext.Shift.Where(x => x.ShiftCode == id).FirstOrDefaultAsync();
            var rowsAffected = 0;

            if (existingShift != null)
            {
                existingShift.Description = model.Description.ToUpper();
                existingShift.UpdatedAt = DateTimeOffset.UtcNow;
                existingShift.UpdatedBy = this.web.UserHumanName;

                rowsAffected = await this.logisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }

        public async Task<int> Remove(Shift entity)
        {
            this.logisticDbContext.Shift.Remove(entity);

            return await this.logisticDbContext.SaveChangesAsync();

        }

    }
}
