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
    public class CityMasterService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;
        public CityMasterService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <returns></returns>
        public async Task<int>AddData(CityMasterViewModel model)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var insert = new CityForShipment();
                {
                insert.CityForShipmentCode = model.CityCode.ToUpper();
                insert.Name = model.Name.ToUpper();
                insert.CreatedBy = user;
                insert.CreatedAt = DateTimeOffset.UtcNow;
                insert.UpdatedBy = user;
                insert.UpdatedAt = DateTimeOffset.UtcNow;
            }
            LogisticDbContext.Add(insert);
            return await LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get All City data from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<CityMasterViewModel>> GetCityData()
        {
            var cityData = await this.LogisticDbContext.CityForShipment
                .AsNoTracking()
                .Select(Q => new CityMasterViewModel
                {
                    CityCode = Q.CityForShipmentCode,
                    Name = Q.Name
                }).ToListAsync();
            return cityData;
        }

        /// <summary>
        /// Validasi CityFormShipmentCode (cek apakah sudah ada)
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ValidateCodeCity(string code)
        {
            var cityData = await this.LogisticDbContext.CityForShipment.FirstOrDefaultAsync(Q => Q.CityForShipmentCode == code);
            if (cityData != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete selected data from database
        /// </summary>
        /// <returns></returns>
        public async Task<int> RemoveCityData(string cityCode)
        {
            var existingCityData = await LogisticDbContext.CityForShipment.Where(Q => Q.CityForShipmentCode == cityCode).FirstOrDefaultAsync();
            if (existingCityData != null)
            {
                LogisticDbContext.Remove(existingCityData);
            }

            var rowsAffected = await LogisticDbContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Update selected data to database
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateCityData(string cityCode, CityMasterViewModel model)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var existingCityData = await LogisticDbContext.CityForShipment.Where(Q => Q.CityForShipmentCode == cityCode).FirstOrDefaultAsync();
            var rowsAffected = 0;

            if (existingCityData != null)
            {
                existingCityData.CityForShipmentCode = model.CityCode.ToUpper();
                existingCityData.Name = model.Name.ToUpper();
                existingCityData.UpdatedAt = DateTimeOffset.UtcNow;
                existingCityData.UpdatedBy = user;
                rowsAffected = await LogisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }
    }
}
