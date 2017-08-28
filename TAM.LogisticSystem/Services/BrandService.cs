using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;

namespace TAM.LogisticSystem.Services
{
    public class BrandService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public BrandService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        public async Task<List<BrandViewModel>> GetDataBrand()
        {
            var brands = await this.LogisticDbContext.Brand
                .Select(Q => new BrandViewModel
                {
                    BrandCode = Q.BrandCode,
                    Name = Q.Name
                })
                .ToListAsync();
            return brands;
        }

        internal async Task<int> Add(BrandViewModel model)
        {
            var rowsAfected = 2;
            var username = WebEnvService.UserHumanName;
            var existingBrand = await this.LogisticDbContext.Brand.Where(x => x.BrandCode == model.BrandCode).FirstOrDefaultAsync();
            if (existingBrand != null)
            {
                return 0;
            }
            if (existingBrand == null)
            {
                var insert = new Brand
                {
                    BrandCode = model.BrandCode.ToUpper(),
                    Name = model.Name,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = username,
                    UpdatedBy = username
                };
                this.LogisticDbContext.Add(insert);
            }
            rowsAfected = await this.LogisticDbContext.SaveChangesAsync();
            return rowsAfected;
        }

        internal async Task<int> Update(string id, BrandViewModel model)
        {
            var rowsAffected = 0;
            var username = WebEnvService.UserHumanName;
            var existingBrand = await this.LogisticDbContext.Brand.Where(x => x.BrandCode == id).FirstOrDefaultAsync();
            if (existingBrand != null)
            {
                existingBrand.Name = model.Name;
                existingBrand.UpdatedAt = DateTimeOffset.UtcNow;
                existingBrand.UpdatedBy = username;
            }
            rowsAffected = await this.LogisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }

        internal async Task<int> Remove(string id)
        {
            var rowsAffected = 0;
            var existingBrand = await this.LogisticDbContext.Brand.Where(x => x.BrandCode == id).FirstOrDefaultAsync();
            if (existingBrand != null)
            {
                this.LogisticDbContext.Remove(existingBrand);
            }
            rowsAffected = await this.LogisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }
    }
}
