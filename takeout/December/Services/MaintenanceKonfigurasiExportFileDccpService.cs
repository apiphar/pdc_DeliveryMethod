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
    public class MaintenanceKonfigurasiExportFileDccpService
    {
        private readonly LogisticDbContext context;

        public MaintenanceKonfigurasiExportFileDccpService(LogisticDbContext dbContext)
        {
            this.context = dbContext;
        }
        /// <summary>
        /// get all config
        /// </summary>
        /// <returns></returns>
        public async Task<List<MaintenanceKonfigurasiExportFileDccpViewModel>> GetAllConfig()
        {
            var allConfig = (await this.context.Database.GetDbConnection().QueryAsync<MaintenanceKonfigurasiExportFileDccpViewModel>($@"Select 
            DailyCarCarrierPlanExcelConfigurationId  as Id,
            Description,
            RangeStart,
            RangeEnd 

            From DailyCarCarrierPlanExcelConfiguration")).ToList();
            return allConfig;
        }


        /// <summary>
        /// add new config
        /// </summary>
        /// <param name="addedMOdel"></param>
        /// <returns></returns>
        public async Task AddConfig(MaintenanceKonfigurasiExportFileDccpPostViewModel addedMOdel)
        {
            var newAddedEntities = new DailyCarCarrierPlanExcelConfiguration();
            newAddedEntities.Description = addedMOdel.Description;
            newAddedEntities.RangeEnd = addedMOdel.RangeEnd;
            newAddedEntities.RangeStart = addedMOdel.RangeStart;
            this.context.DailyCarCarrierPlanExcelConfiguration.Add(newAddedEntities);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete selected Config
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteSingleData(int id)
        {   
            var selectedData = await this.context.DailyCarCarrierPlanExcelConfiguration.Where(Q => Q.DailyCarCarrierPlanExcelConfigurationId == id).FirstOrDefaultAsync();
            this.context.Remove(selectedData);
            return await this.context.SaveChangesAsync();
        }


        /// <summary>
        /// Update Selected Data Service
        /// </summary>
        /// <param name="updateMOdel"></param>
        /// <returns></returns>
        public async Task UpdateData(MaintenanceKonfigurasiExportFileDccpViewModel updateMOdel)
        {
            var selectedUpdateModel = await this.context.DailyCarCarrierPlanExcelConfiguration.Where(Q => Q.DailyCarCarrierPlanExcelConfigurationId == updateMOdel.Id).FirstOrDefaultAsync();
            selectedUpdateModel.Description = updateMOdel.Description;
            selectedUpdateModel.RangeEnd = updateMOdel.RangeEnd;
            selectedUpdateModel.RangeStart = updateMOdel.RangeStart;

            this.context.DailyCarCarrierPlanExcelConfiguration.Update(selectedUpdateModel);
            await this.context.SaveChangesAsync();
        }
    }
}
