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
    public class MasterLeadTimeLocationService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public MasterLeadTimeLocationService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// get all locations data for dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterLeadTimeLocationViewModel>> GetMasterLeadTimeLocationData()
        {
            _ = nameof(ProcessLeadTimeForLocation.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(ProcessLeadTimeForLocation.ProcessMasterCode);
            _ = nameof(ProcessMaster.Name);
            _ = nameof(ProcessLeadTimeForLocation.LeadMinutes);
            var masterLeadTimeLocationData = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<MasterLeadTimeLocationViewModel>(@"
                select pltfl.LocationCode, pltfl.ProcessMasterCode, l.Name as LocationName, pm.Name as RouteName, pltfl.LeadMinutes
                from ProcessLeadTimeForLocation pltfl
                join ProcessMaster pm on pm.ProcessMasterCode = pltfl.ProcessMasterCode
                join Location l on l.LocationCode = pltfl.LocationCode
            ")).ToList();
            return masterLeadTimeLocationData;
        }

        /// <summary>
        /// get all locations data for dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterLeadTimeLocationLocationComboBoxModel>> GetLocations()
        {
            var locations = await this.LogisticDbContext.Location.Select(x => new MasterLeadTimeLocationLocationComboBoxModel
            {
                LocationCode = x.LocationCode,
                Name = x.Name
            }).ToListAsync();
            return locations;
        }

        
        /// <summary>
        /// get all routing code and its name
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterLeadTimeLocationRouteComboBoxModel>> GetRoutes()
        {
            var routingMasterData = await this.LogisticDbContext.ProcessMaster.Select(x => new MasterLeadTimeLocationRouteComboBoxModel
            {
                ProcessMasterCode = x.ProcessMasterCode,
                Name = x.Name
            }).ToListAsync();
            return routingMasterData;
        }

        public async Task<ProcessLeadTimeForLocation> CheckExistingCode(string locationCode, string processMasterCode)
        {
            var existingCode = await this.LogisticDbContext.ProcessLeadTimeForLocation.FirstOrDefaultAsync(x => x.LocationCode == locationCode && x.ProcessMasterCode == processMasterCode);
            return existingCode;
        }

        /// <summary>
        /// insert new data to database
        /// </summary>
        /// <param name="masterLeadTimeLocationInsertUpdateModel"></param>
        /// <returns></returns>
        public async Task InsertMasterLeadTimeLocationData(MasterLeadTimeLocationInsertUpdateModel masterLeadTimeLocationInsertUpdateModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var newRoutingLocationLeadTime = new ProcessLeadTimeForLocation
            {
                LocationCode = masterLeadTimeLocationInsertUpdateModel.LocationCode,
                ProcessMasterCode = masterLeadTimeLocationInsertUpdateModel.ProcessMasterCode,
                LeadMinutes = masterLeadTimeLocationInsertUpdateModel.LeadMinutes,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username

            };
            this.LogisticDbContext.ProcessLeadTimeForLocation.Add(newRoutingLocationLeadTime);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// update sent data
        /// </summary>
        /// <param name="masterLeadTimeLocationInsertUpdateModel"></param>
        /// <returns></returns>
        public async Task UpdateMasterLeadTimeLocationData(MasterLeadTimeLocationInsertUpdateModel masterLeadTimeLocationInsertUpdateModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var updateRoutingLocationLeadTime = await this.LogisticDbContext.ProcessLeadTimeForLocation.Where(x => x.LocationCode == masterLeadTimeLocationInsertUpdateModel.LocationCode).FirstOrDefaultAsync();
            updateRoutingLocationLeadTime.LocationCode = masterLeadTimeLocationInsertUpdateModel.LocationCode;
            updateRoutingLocationLeadTime.ProcessMasterCode = masterLeadTimeLocationInsertUpdateModel.ProcessMasterCode;
            updateRoutingLocationLeadTime.LeadMinutes = masterLeadTimeLocationInsertUpdateModel.LeadMinutes;
            updateRoutingLocationLeadTime.UpdatedAt = DateTimeOffset.UtcNow;
            updateRoutingLocationLeadTime.UpdatedBy = username;
            this.LogisticDbContext.ProcessLeadTimeForLocation.Update(updateRoutingLocationLeadTime);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// delete data based on location and routing code
        /// </summary>
        /// <param name="lokasi"></param>
        /// <param name="kodeRute"></param>
        /// <returns></returns>
        public async Task DeleteMasterLeadTimeLocationData(string locationCode, string processMasterCode)
        {
            var deleteRoutingLocationLeadTimeData = await this.LogisticDbContext.ProcessLeadTimeForLocation.Where(x => x.LocationCode == locationCode && x.ProcessMasterCode == processMasterCode).FirstOrDefaultAsync();
            this.LogisticDbContext.ProcessLeadTimeForLocation.Remove(deleteRoutingLocationLeadTimeData);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
