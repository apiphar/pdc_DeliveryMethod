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
    public class ConfigurationPlanningService
    {
        private readonly LogisticDbContext logisticDbContext;

        public ConfigurationPlanningService (LogisticDbContext tangoDBContext)
        {
            this.logisticDbContext = tangoDBContext;
        }

        public List<CarSeries> GetSeries()
        {
            var data = logisticDbContext.CarSeries.ToList();
            return data;
        }

        // TIE: START
        //public List<ConfigurationPlanningViewModel> GetAll()
        //{
        //    List<ConfigurationPlanningViewModel> lists = new List<ConfigurationPlanningViewModel>();
        //    var DbConnection = logisticDbContext.Database.GetDbConnection();

        //    string query = @"select RoutingMasterCode
        //                        ,CONCAT(RoutingMasterCode, ', ' ,name) as RoutingMasterCodeAndName
        //                  ,case when DoMonthlyCarCarrierPlan=1 and DoDailyCarCarrierPlan=0 then 'Kode Routing untuk MCCP Report'
        //                  when DoDailyCarCarrierPlan=1 and DoMonthlyCarCarrierPlan=0 then 'Kode Routing untuk DCCP Report'
        //                  when DoMonthlyCarCarrierPlan=1 and DoDailyCarCarrierPlan=1 then 'Kode Routing untuk MCCP Report & Kode Routing untuk DCCP Report' 
        //                  end as Config
        //                    from routingMaster
        //                    where DoMonthlyCarCarrierPlan=1 or DoDailyCarCarrierPlan=1";

        //    var data = DbConnection.Query<ConfigurationPlanningViewModel>(query);
        //    lists = data.ToList();

        //    return lists;
        //}

        //public List<ConfigurationPlanningViewModel> GetRoutingMasterCode()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select RoutingMasterCode, Name
        //                                from RoutingMaster";

        //        var result = dbconnection.Query<ConfigurationPlanningViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public async Task<int> SetMCCPFalse(bool mccp, ConfigurationPlanningViewModel model)
        //{
        //    var entity = await logisticDbContext.RoutingMaster.Where(x => x.DoMonthlyCarCarrierPlan == mccp).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (entity != null)
        //    {
        //        entity.DoMonthlyCarCarrierPlan = model.DoMonthlyCarCarrierPlan = false;
        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }

        //    return rowsAffected;
        //}

        //public async Task<int> SetDCCPFalse(bool dccp, ConfigurationPlanningViewModel model)
        //{
        //    var entity = await logisticDbContext.RoutingMaster.Where(x => x.DoDailyCarCarrierPlan == dccp).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (entity != null)
        //    {
        //        entity.DoDailyCarCarrierPlan = model.DoDailyCarCarrierPlan = false;
        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }

        //    return rowsAffected;
        //}

        //public async Task<int> UpdateMCCP(string id1, ConfigurationPlanningViewModel model)
        //{
        //    var entity = await logisticDbContext.RoutingMaster.Where(x => x.RoutingMasterCode == id1).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (entity != null)
        //    {
        //        entity.DoMonthlyCarCarrierPlan = model.DoMonthlyCarCarrierPlan=true;
        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }

        //    return rowsAffected;
        //}

        //public async Task<int> UpdateDCCP(string id2, ConfigurationPlanningViewModel model)
        //{
        //    var entity = await logisticDbContext.RoutingMaster.Where(x => x.RoutingMasterCode == id2).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (entity != null)
        //    {
        //        entity.DoDailyCarCarrierPlan = model.DoDailyCarCarrierPlan=true;
        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }

        //    return rowsAffected;
        //}
        // TIE: END
    }
}
