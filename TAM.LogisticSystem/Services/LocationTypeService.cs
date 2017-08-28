using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using System.Collections.Generic;
using Dapper;
using System;

namespace TAM.LogisticSystem.Services
{
    public class LocationTypeService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService _WebEnvironmentService;
        public LocationTypeService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this._WebEnvironmentService = webEnvironmentService;
        }


        /// <summary>
        /// method menambah location type ke dalam DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddNewLocationType(LocationTypeViewModel model)
        {
            var username = _WebEnvironmentService.UserHumanName;
            var entity = new LocationType
            {
                LocationTypeCode = model.LocationTypeCode.Trim().ToUpper(),
                Name = model.Name.ToUpper(),
                HasResponsibility = model.HasResponsibility,
                NeedSJKBTarikan = model.NeedSjkbTarikan,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username
            };
            this.LogisticDbContext.LocationType.Add(entity);
            await this.LogisticDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// update location type in DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateLocationType(LocationTypeViewModel model)
        {
            var updated = await this.LogisticDbContext.LocationType.FirstOrDefaultAsync(Q => Q.LocationTypeCode == model.LocationTypeCode);
            var username = _WebEnvironmentService.UserHumanName;
            updated.HasResponsibility = model.HasResponsibility;
            updated.NeedSJKBTarikan = model.NeedSjkbTarikan;
            updated.Name = model.Name.ToUpper();
            updated.UpdatedBy = username;
            updated.UpdatedAt = DateTimeOffset.UtcNow;
            this.LogisticDbContext.LocationType.Update(updated);
            await this.LogisticDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// method to remove location type from database
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<int> RemoveLocationType(string code)
        {
            var selected = await this.LogisticDbContext.LocationType.FirstOrDefaultAsync(Q => Q.LocationTypeCode == code);
            var result = this.LogisticDbContext.LocationType.Remove(selected);
            return await this.LogisticDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// get all location type data
        /// </summary>
        /// <returns></returns>
        public async Task<List<LocationTypeViewModel>> GetAllLocationType()
        {
            var selected = await this.LogisticDbContext.LocationType.AsNoTracking().Select(Q=>new LocationTypeViewModel
            {
                LocationTypeCode = Q.LocationTypeCode.ToUpper(),
                Name = Q.Name.ToUpper(),
                HasResponsibility = Q.HasResponsibility,
                NeedSjkbTarikan = Q.NeedSJKBTarikan
            }).ToListAsync();
            return selected;
        }
        /// <summary>
        /// cek 
        /// </summary>
        /// <param name="locationTypeCode"></param>
        /// <returns></returns>
        public async Task<bool> IsLocationTypeExist(string locationTypeCode)
        {
            var data = await this.LogisticDbContext.LocationType.FirstOrDefaultAsync(Q => Q.LocationTypeCode == locationTypeCode);
            if (data != null)
            {
                return true;
            }
            return false;
        }
    }
}
