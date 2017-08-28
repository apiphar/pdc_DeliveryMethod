using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class AFIRestriksiAreaService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public AFIRestriksiAreaService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// get all data for data grid
        /// </summary>
        /// <returns></returns>
        public async Task<List<AFIRestriksiAreaViewModel>> GetAfiRegionsRestriction()
        {
            _ = nameof(AFIRegionRestriction.AFIRegionRestrictionId);
            _ = nameof(AFIRegionRestriction.RegionCode);
            _ = nameof(Region.Name);
            _ = nameof(AFIRegionRestriction.IsLocked);
            _ = nameof(AFIRegionRestriction.ValidFrom);
            _ = nameof(AFIRegionRestriction.ValidTo);
            _ = nameof(Region.RegionCode);
            _ = nameof(Region.Type);
            var regionsRestrictionData = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<AFIRestriksiAreaViewModel>(@"
                select arr.AFIRegionRestrictionId, arr.RegionCode, r.Name, arr.IsLocked, arr.ValidFrom, arr.ValidTo
                from AFIRegionRestriction arr
                join Region r on r.RegionCode = arr.RegionCode
                where r.Type = 'PRVN'
            ")).OrderBy(Q => Q.RegionCode).ThenBy(Q => Q.ValidFrom).ToList();
            return regionsRestrictionData;
        }

        /// <summary>
        /// get all data for detail data grid
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public async Task<List<AFIRestriksiAreaViewModel>> GetAfiRegionsRestrictionDetail(string regionCode, DateTimeOffset validFrom, DateTimeOffset validTo)
        {
            _ = nameof(AFIRegionRestriction.AFIRegionRestrictionId);
            _ = nameof(AFIRegionRestriction.RegionCode);
            _ = nameof(Region.Name);
            _ = nameof(AFIRegionRestriction.IsLocked);
            _ = nameof(AFIRegionRestriction.ValidFrom);
            _ = nameof(AFIRegionRestriction.ValidTo);
            _ = nameof(Region.RegionCode);
            _ = nameof(Region.ParentRegionCode);
            var regionsRestrictionData = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<AFIRestriksiAreaViewModel>(@"
                select arr.AFIRegionRestrictionId, arr.RegionCode, r.Name, arr.IsLocked, arr.ValidFrom, arr.ValidTo
                from AFIRegionRestriction arr
                join Region r on r.RegionCode = arr.RegionCode
                where r.ParentRegionCode = @RegionCode AND arr.ValidFrom >= @ValidFrom AND arr.ValidTo <= @ValidTo 
            ", new { RegionCode = regionCode, ValidFrom = validFrom, ValidTo = validTo })).OrderBy(Q => Q.RegionCode).ThenBy(Q => Q.ValidFrom).ToList();
            return regionsRestrictionData;
        }

        /// <summary>
        /// get all data for province combobox
        /// </summary>
        /// <returns></returns>
        public async Task<List<AFIRestriksiAreaRegionViewModel>> GetProvinces()
        {
            var provincesData = await this.LogisticDbContext.Region.AsNoTracking().Where(Q => Q.Type == "PRVN").Select(Q => new AFIRestriksiAreaRegionViewModel
            {
                RegionCode = Q.RegionCode,
                Name = Q.Name
            }).ToListAsync();
            return provincesData;
        }

        /// <summary>
        /// get all data for cities combobox
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public async Task<List<AFIRestriksiAreaRegionViewModel>> GetCities(string regionCode)
        {
            var provincesData = await this.LogisticDbContext.Region.AsNoTracking().Where(Q => Q.ParentRegionCode == regionCode).Select(Q => new AFIRestriksiAreaRegionViewModel
            {
                RegionCode = Q.RegionCode,
                Name = Q.Name
            }).ToListAsync();
            return provincesData;
        }

        /// <summary>
        /// insert data into database
        /// </summary>
        /// <param name="afiRestriksiAreaInsertModel"></param>
        /// <returns></returns>
        public async Task Create(AFIRestriksiAreaInsertModel afiRestriksiAreaInsertModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var newData = new AFIRegionRestriction
            {
                RegionCode = afiRestriksiAreaInsertModel.RegionCode,
                IsLocked = afiRestriksiAreaInsertModel.IsLocked,
                ValidFrom = afiRestriksiAreaInsertModel.ValidFrom,
                ValidTo = afiRestriksiAreaInsertModel.ValidTo,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username
            };
            this.LogisticDbContext.AFIRegionRestriction.Add(newData);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// update the data in the database
        /// </summary>
        /// <param name="afiRestriksiAreaUpdateModel"></param>
        /// <returns></returns>
        public async Task Update(AFIRestriksiAreaUpdateModel afiRestriksiAreaUpdateModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var existedData = await this.LogisticDbContext.AFIRegionRestriction.FirstOrDefaultAsync(Q => Q.AFIRegionRestrictionId == afiRestriksiAreaUpdateModel.AFIRegionRestrictionId);
            existedData.RegionCode = afiRestriksiAreaUpdateModel.RegionCode;
            existedData.IsLocked = afiRestriksiAreaUpdateModel.IsLocked;
            existedData.ValidFrom = afiRestriksiAreaUpdateModel.ValidFrom;
            existedData.ValidTo = afiRestriksiAreaUpdateModel.ValidTo;
            existedData.UpdatedAt = DateTimeOffset.UtcNow;
            existedData.UpdatedBy = username;
            this.LogisticDbContext.AFIRegionRestriction.Update(existedData);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// delete the data in the database
        /// </summary>
        /// <param name="afiRegionRestrictionId"></param>
        /// <returns></returns>
        public async Task Delete(int afiRegionRestrictionId)
        {
            var existedData = await this.LogisticDbContext.AFIRegionRestriction.FirstOrDefaultAsync(Q => Q.AFIRegionRestrictionId == afiRegionRestrictionId);
            this.LogisticDbContext.AFIRegionRestriction.Remove(existedData);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// check if the input data date range sliced with other in database
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <returns></returns>
        public async Task<bool> IsSliced(string regionCode, DateTimeOffset? validFrom, DateTimeOffset? validTo)
        {
            var existedData = await this.LogisticDbContext.AFIRegionRestriction.AsNoTracking().Where(Q => Q.RegionCode == regionCode).ToListAsync();
            foreach (var data in existedData)
            {
                if((validFrom >= data.ValidFrom && validFrom <= data.ValidTo) || (validTo >= data.ValidFrom && validTo <= data.ValidTo))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
