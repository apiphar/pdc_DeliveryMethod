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
    public class DccpReadinessVolumeService
    {
        private readonly LogisticDbContext DbContext;

        public DccpReadinessVolumeService(LogisticDbContext context)
        {
            this.DbContext = context;
        }

        /// <summary>
        /// Get all Data about Dccp Readiness Volume
        /// </summary>
        /// <returns></returns>
        public async Task<List<DccpReadinessVolumeViewModel>> GetAllDccpVolume()
        {
            var listDccpModel = (await this.DbContext.Database.GetDbConnection().QueryAsync<DccpReadinessVolumeViewModel>($@"
           Select
            DailyCarCarrierPlanId as Id,
             REPLACE (CONVERT (Varchar(max),TransInOutDate,106), ' ', '-')as Tanggal,
            TransInOutDate,
            LocationFrom as Asal,
            LocationTo as Tujuan,
            Trip as Trip,
            Load as [Load],
            ShiftCode as [Shift],
            UnitReadyQuantity as Qty,
            UnitReadyAdjusted as Adjust,
            EstimatedUnit as Estimasi
            from DailyCarCarrierPlan 
            where  CONVERT(Varchar(max),TransInOutDate,103) = CONVERT(Varchar(max),GETDATE(),103)
")).ToList();
            return listDccpModel;
        }

        /// <summary>
        /// Edit selected Dccp
        /// </summary>
        /// <param name="dccpReadinessVolumeViewModel"></param>
        /// <returns></returns>
        public async Task EditDccp(DccpReadinessVolumeViewModel dccpReadinessVolumeViewModel)
        {
            var selectedDccp = DbContext.DailyCarCarrierPlan.Where(Q => Q.DailyCarCarrierPlanId == dccpReadinessVolumeViewModel.Id).FirstOrDefault();
            selectedDccp.UnitReadyAdjusted = dccpReadinessVolumeViewModel.Adjust;
            DbContext.DailyCarCarrierPlan.Update(selectedDccp);
            await DbContext.SaveChangesAsync();
        }


        /// <summary>
        /// delete selected Dccp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            var selectedDccp = DbContext.DailyCarCarrierPlan.Where(Q => Q.DailyCarCarrierPlanId == id).FirstOrDefault();
            DbContext.DailyCarCarrierPlan.Remove(selectedDccp);
            await DbContext.SaveChangesAsync();
        }
    }
}
