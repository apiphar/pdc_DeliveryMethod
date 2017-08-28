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
    public class PDILeadTimeConfigurationService
    {
        public PDILeadTimeConfigurationService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        /// <summary>
        /// Get PDI Lead Time Configurations
        /// </summary>
        /// <returns></returns>
        public async Task<List<PDILeadTimeConfigurationViewModel>> GetPDILeadTimeConfigurations()
        {
            _ = nameof(PDILeadTime.PDILeadTimeId);
            _ = nameof(PDILeadTime.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(PDILeadTime.Katashiki);
            _ = nameof(PDILeadTime.Suffix);
            _ = nameof(PDILeadTime.TaktSeconds);
            _ = nameof(PDILeadTime.Post);
            var pdiLeadTimeConfigurations = (await this.LogisticDbContext.Database.GetDbConnection()
                .QueryAsync<PDILeadTimeConfigurationViewModel>(@"
                    SELECT plt.PDILeadTimeId, plt.LocationCode, l.[Name] AS [LocationName], plt.Katashiki, 
                    plt.Suffix, plt.TaktSeconds, plt.Post
                    FROM PDILeadTime plt
                    JOIN [Location] l ON plt.LocationCode = l.LocationCode
            ")).ToList();
            return pdiLeadTimeConfigurations;
        }

        /// <summary>
        /// Get Car Models
        /// </summary>
        /// <returns></returns>
        public async Task<List<PDILeadTimeConfigurationCarModelViewModel>> GetCarModels()
        {
            var carModels = await this.LogisticDbContext.CarModel.AsNoTracking()
                .Select(Q => new PDILeadTimeConfigurationCarModelViewModel
                {
                    CarModelCode = Q.CarModelCode,
                    CarModelName = Q.Name
                })
                .ToListAsync();
            return carModels;
        }

        /// <summary>
        /// Get Car Series
        /// </summary>
        /// <returns></returns>
        public async Task<List<PDILeadTimeConfigurationCarSeriesViewModel>> GetCarSeries()
        {
            var carSeries = await this.LogisticDbContext.CarSeries.AsNoTracking()
                .Select(Q => new PDILeadTimeConfigurationCarSeriesViewModel
                {
                    CarSeriesCode = Q.CarSeriesCode,
                    CarSeriesName = Q.Name,
                    CarModelCode = Q.CarModelCode
                })
                .ToListAsync();
            return carSeries;
        }

        /// <summary>
        /// Get Car Types
        /// </summary>
        /// <returns></returns>
        public async Task<List<PDILeadTimeConfigurationCarTypeViewModel>> GetCarTypes()
        {
            var carTypes = await this.LogisticDbContext.CarType.AsNoTracking()
                .Select(Q => new PDILeadTimeConfigurationCarTypeViewModel
                {
                    CarSeriesCode = Q.CarSeriesCode,
                    Katashiki = Q.Katashiki,
                    Suffix = Q.Suffix
                })
                .ToListAsync();
            return carTypes;
        }

        /// <summary>
        /// Get the list of location code and location name
        /// </summary>
        /// <returns></returns>
        public async Task<List<PDILeadTimeConfigurationLocationViewModel>> GetLocations()
        {
            var locations = await this.LogisticDbContext.Location.AsNoTracking()
                .Select(Q => new PDILeadTimeConfigurationLocationViewModel
                {
                    LocationCode = Q.LocationCode,
                    LocationName = Q.Name
                })
                .ToListAsync();
            return locations;
        }

        public async Task<List<PDILeadTimeConfigurationKatashikiModel>> GetKatashikis()
        {
            //_ = nameof(CarType.Katashiki);
            //_ = nameof(CarType.CarSeriesCode);
            //var katashikis = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<PDILeadTimeConfigurationKatashikiModel>(@"
            //    select distinct Katashiki, CarSeriesCode
            //    from CarType
            //")).ToList();
            var katashikis = await this.LogisticDbContext.CarType.AsNoTracking().GroupBy(x => new
            {
                x.Katashiki,
                x.CarSeriesCode
            }).Select(x => new PDILeadTimeConfigurationKatashikiModel
            {
                Katashiki = x.Key.Katashiki,
                CarSeriesCode = x.Key.CarSeriesCode
            }).ToListAsync();
            return katashikis;
        }

        public async Task<bool> CheckCombinationExistence(string locationCode, string katashiki, string suffix)
        {
            var existenceData = await this.LogisticDbContext.PDILeadTime.FirstOrDefaultAsync(x => x.LocationCode == locationCode && x.Katashiki == katashiki && x.Suffix == suffix);
            if(existenceData == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create a several new PDI Lead Time Configurations
        /// </summary>
        /// <param name="createPDILeadTimeConfigs"></param>
        /// <returns></returns>
        public async Task CreatePDILeadTimeConfigurations(List<PDILeadTimeConfigurationCreateModel> createPDILeadTimeConfigs)
        {
            var newPDILeadTimeConfigs = new List<PDILeadTime>();
            var username = WebEnvironmentService.UserHumanName;
            foreach (var newPDILeadTimeConfig in createPDILeadTimeConfigs)
            {
                newPDILeadTimeConfigs.Add(new PDILeadTime
                {
                    LocationCode = newPDILeadTimeConfig.LocationCode,
                    Katashiki = newPDILeadTimeConfig.Katashiki,
                    Suffix = newPDILeadTimeConfig.Suffix,
                    TaktSeconds = newPDILeadTimeConfig.TotalTaktTimes,
                    Post = newPDILeadTimeConfig.Post,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = username,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    UpdatedBy = username
                });
            }

            this.LogisticDbContext.PDILeadTime.AddRange(newPDILeadTimeConfigs);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Update a PDI Lead Time Configuration based on PDIKatsuDictionaryDetailId
        /// </summary>
        /// <param name="pdiLeadTimeId"></param>
        /// <param name="updatedPDILeadTimeConfig"></param>
        /// <returns></returns>
        public async Task UpdatePDILeadTimeConfiguration(int pdiLeadTimeId, PDILeadTimeConfigurationUpdateModel updatedPDILeadTimeConfig)
        {
            var username = WebEnvironmentService.UserHumanName;
            var pdiLeadTimeConfig = await this.LogisticDbContext.PDILeadTime
                .FirstOrDefaultAsync(x => x.PDILeadTimeId == pdiLeadTimeId);
            pdiLeadTimeConfig.LocationCode = updatedPDILeadTimeConfig.LocationCode;
            pdiLeadTimeConfig.TaktSeconds = updatedPDILeadTimeConfig.TotalTaktTimes;
            pdiLeadTimeConfig.Post = updatedPDILeadTimeConfig.Post;
            pdiLeadTimeConfig.UpdatedAt = DateTimeOffset.UtcNow;
            pdiLeadTimeConfig.UpdatedBy = username;

            this.LogisticDbContext.PDILeadTime.Update(pdiLeadTimeConfig);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a PDI Lead Time Configuration based on PDIKatsuDictionaryDetailId
        /// </summary>
        /// <param name="pdiLeadTimeId"></param>
        /// <returns></returns>
        public async Task DeletePDILeadTimeConfiguration(int pdiLeadTimeId)
        {
            var deletedPDILeadTimeConfig = await this.LogisticDbContext.PDILeadTime.FirstOrDefaultAsync(Q => Q.PDILeadTimeId == pdiLeadTimeId);
            this.LogisticDbContext.PDILeadTime.Remove(deletedPDILeadTimeConfig);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
