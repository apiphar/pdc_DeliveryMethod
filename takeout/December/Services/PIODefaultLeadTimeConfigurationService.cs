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
    public class PIODefaultLeadTimeConfigurationService
    {
        public PIODefaultLeadTimeConfigurationService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }

        private readonly LogisticDbContext LogisticDbContext;

        /// <summary>
        /// Get the list of PIO Default Lead Time, Location, and Process List from the database for 
        /// PIO Default Lead Time Configuration menu.
        /// </summary>
        /// <returns></returns>
        public async Task<PIODefaultLeadTimeConfigurationPageViewModel> GetPIODefaultLeadTimeConfigurationPage()
        {
            var defaultLeadTimeConfig = new PIODefaultLeadTimeConfigurationPageViewModel();
            defaultLeadTimeConfig.PIODefaultLeadTimes = (await this.LogisticDbContext.
                Database.GetDbConnection().QueryAsync<PIODefaultLeadTimeConfigurationViewModel>(@"
SELECT l.[Name] AS LocationName, pfl.[Name] AS ProcessStatus, ldltc.LeadMinutes AS LeadTime, 
l.LocationCode, pfl.ProcessForLineId, ldltc.RoutingMasterCode 
FROM LineDefaultLeadTimeConfiguration ldltc
JOIN [Location] l ON ldltc.LocationCode = l.LocationCode
JOIN ProcessForLine pfl ON ldltc.ProcessForLineId = pfl.ProcessForLineId 
WHERE ldltc.RoutingMasterCode = 'PIO'
")).ToList();

            defaultLeadTimeConfig.Locations = await this.LogisticDbContext.Location
                .Select(Q => new PIODefaultLeadTimeConfigurationLocationModel {
                    LocationCode = Q.LocationCode,
                    LocationName = Q.Name
                }).ToListAsync();

            defaultLeadTimeConfig.ProcessStatusList = await this.LogisticDbContext.ProcessForLine
                .ToListAsync();
            return defaultLeadTimeConfig;
        }

        /// <summary>
        /// Create a new PIO Default Lead Time data
        /// </summary>
        /// <param name="createPIODefaultLeadTime"></param>
        /// <returns></returns>
        public async Task<bool> CreatePIODefaultLeadTime(CreatePIODefaultLeadTimeViewModel createPIODefaultLeadTime)
        {
            var checkDuplicate = await this.LogisticDbContext.LineDefaultLeadTimeConfiguration
                .FirstOrDefaultAsync(Q => Q.RoutingMasterCode == "PIO" && Q.LocationCode == createPIODefaultLeadTime.LocationCode && Q.ProcessForLineId == createPIODefaultLeadTime.ProcessForLineId);

            if(checkDuplicate != null)
            {
                return false;
            }

            var newLineDefaultLeadTimeConfiguration = new LineDefaultLeadTimeConfiguration
            {
                RoutingMasterCode = "PIO",
                LocationCode = createPIODefaultLeadTime.LocationCode,
                ProcessForLineId = createPIODefaultLeadTime.ProcessForLineId,
                LeadMinutes = createPIODefaultLeadTime.TotalLeadTimeMinutes
            };

            this.LogisticDbContext.LineDefaultLeadTimeConfiguration.Add(newLineDefaultLeadTimeConfiguration);
            await this.LogisticDbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update the existing PIO Default Lead Time data
        /// </summary>
        /// <param name="updatePIODefaultLeadTime"></param>
        /// <returns></returns>
        public async Task UpdatePIODefaultLeadTime(UpdatePIODefaultLeadTimeViewModel updatePIODefaultLeadTime)
        {
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
                routingMasterCode = updatePIODefaultLeadTime.RoutingMasterCode,
                oldLocationCode = updatePIODefaultLeadTime.OldLocationCode,
                newLocationCode = updatePIODefaultLeadTime.NewLocationCode,
                oldProcessForLineId = updatePIODefaultLeadTime.OldProcessForLineId,
                newProcessForLineId = updatePIODefaultLeadTime.NewProcessForLineId,
                leadTime = updatePIODefaultLeadTime.TotalLeadTimeMinutes
            });
        }

        /// <summary>
        /// Delete the existing PIO Default Lead Time data
        /// </summary>
        /// <param name="lineDefaultLeadTimeConfigurationId"></param>
        /// <returns></returns>
        public async Task<bool> DeletePIODefaultLeadTime(string routingMasterCode, string locationCode, int processForLineId)
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
