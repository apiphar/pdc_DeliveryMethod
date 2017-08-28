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
    public class SPUDefaultLeadTimeConfigurationService
    {

        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public SPUDefaultLeadTimeConfigurationService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        /// <summary>
        /// Get the list of SPU Default Lead Time, Location, and Process List from the database for 
        /// SPU Default Lead Time Configuration menu.
        /// </summary>
        /// <returns></returns>
        public async Task<SPUDefaultLeadTimeConfigurationPageViewModel> GetSPUDefaultLeadTimeConfigurationPage()
        {
            var defaultLeadTimeConfig = new SPUDefaultLeadTimeConfigurationPageViewModel();
            defaultLeadTimeConfig.SPUDefaultLeadTimes = (await this.LogisticDbContext.
                Database.GetDbConnection().QueryAsync<SPUDefaultLeadTimeConfigurationViewModel>(@"
SELECT l.[Name] AS LocationName, pfl.[Name] AS ProcessStatus, ldltc.LeadMinutes AS LeadTime, 
l.LocationCode, pfl.ProcessForLineId, ldltc.RoutingMasterCode 
FROM LineDefaultLeadTimeConfiguration ldltc
JOIN [Location] l ON ldltc.LocationCode = l.LocationCode
JOIN ProcessForLine pfl ON ldltc.ProcessForLineId = pfl.ProcessForLineId 
WHERE ldltc.RoutingMasterCode = 'SPU'
")).ToList();

            defaultLeadTimeConfig.Locations = await this.LogisticDbContext.Location
                .Select(Q => new SPUDefaultLeadTimeConfigurationLocationModel {
                    LocationCode = Q.LocationCode,
                    LocationName = Q.Name
                }).ToListAsync();

            defaultLeadTimeConfig.ProcessStatusList = await this.LogisticDbContext.ProcessForLine
                .ToListAsync();
            return defaultLeadTimeConfig;
        }

        /// <summary>
        /// Create a new SPU Default Lead Time data
        /// </summary>
        /// <param name="createSPUDefaultLeadTime"></param>
        /// <returns></returns>
        public async Task CreateSPUDefaultLeadTime(SPUDefaultLeadTimeConfigurationCreateViewModel createSPUDefaultLeadTime)
        {
            var username = WebEnvService.UserHumanName;
            var newLineDefaultLeadTimeConfiguration = new LineDefaultLeadTimeConfiguration
            {
                RoutingMasterCode = "SPU",
                LocationCode = createSPUDefaultLeadTime.LocationCode,
                ProcessForLineId = createSPUDefaultLeadTime.ProcessForLineId,
                LeadMinutes = createSPUDefaultLeadTime.TotalLeadTimeMinutes,

            };

            this.LogisticDbContext.LineDefaultLeadTimeConfiguration.Add(newLineDefaultLeadTimeConfiguration);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Update the existing SPU Default Lead Time data
        /// </summary>
        /// <param name="updateSPUDefaultLeadTime"></param>
        /// <returns></returns>
        public async Task UpdateSPUDefaultLeadTime(SPUDefaultLeadTimeConfigurationUpdateViewModel updateSPUDefaultLeadTime)
        {
            var username = WebEnvService.UserHumanName;
            await this.LogisticDbContext.Database.GetDbConnection().ExecuteAsync($@"
UPDATE
    LineDefaultLeadTimeConfiguration
SET
    LocationCode = @newLocationCode,
    ProcessForLineId = @newProcessForLineId,
    LeadMinutes = @leadTime
WHERE
    LocationCode = @oldLocationCode
    AND ProcessForLineId = @oldProcessForLineId
    AND RoutingMasterCode = @routingMasterCode
", new {
                routingMasterCode = updateSPUDefaultLeadTime.RoutingMasterCode,
                oldLocationCode = updateSPUDefaultLeadTime.OldLocationCode,
                newLocationCode = updateSPUDefaultLeadTime.NewLocationCode,
                oldProcessForLineId = updateSPUDefaultLeadTime.OldProcessForLineId,
                newProcessForLineId = updateSPUDefaultLeadTime.NewProcessForLineId,
                leadTime = updateSPUDefaultLeadTime.TotalLeadTimeMinutes
            });
        }

        /// <summary>
        /// Delete the existing SPU Default Lead Time data
        /// </summary>
        /// <param name="lineDefaultLeadTimeConfigurationId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSPUDefaultLeadTime(string routingMasterCode, string locationCode, int processForLineId)
        {
            var deletedLineDefaultLeadTimeConfiguration = await this.LogisticDbContext.LineDefaultLeadTimeConfiguration
                .FirstOrDefaultAsync(Q => Q.RoutingMasterCode == routingMasterCode && Q.LocationCode == locationCode && Q.ProcessForLineId == processForLineId);
            if(deletedLineDefaultLeadTimeConfiguration == null)
            {
                return false;
            }
            this.LogisticDbContext.LineDefaultLeadTimeConfiguration.Remove(deletedLineDefaultLeadTimeConfiguration);
            await this.LogisticDbContext.SaveChangesAsync();
            return true;
        }
    }
}
