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
    public class MasterLeadTimeService
    {
        private readonly LogisticDbContext DB;

        public MasterLeadTimeService(LogisticDbContext db)
        {
            this.DB = db;
        }

        public List<MasterLeadTimeViewModel> GetData()
        {
            var dbconnection = DB.Database.GetDbConnection();
            {
                string query = @"select  a.LocationCode
                                        ,a.RoutingMasterCode
                                        ,a.LeadMinutes
                                        ,b.Name as NamaRute
                                    
                                        from RoutingLocationLeadTime a
                                        left outer join RoutingMaster b on a.RoutingMasterCode = b.RoutingMasterCode ";

                var result = dbconnection.Query<MasterLeadTimeViewModel>(query, new
                {
                }).ToList();

                return result;
            }
        }


        public MasterLeadTimeViewModel GetKodeRute(string RoutingMasterCode)
        {
            var dbconnection = DB.Database.GetDbConnection();
            {
                string query = @"select  a.LocationCode
    ,a.RoutingMasterCode
    ,a.LeadMinutes
    ,b.Name as NamaRute
                                    
    from RoutingLocationLeadTime a
    left outer join RoutingMaster b on a.RoutingMasterCode = b.RoutingMasterCode
	where b.RoutingMasterCode = @RoutingMasterCode";

                var result = dbconnection.Query<MasterLeadTimeViewModel>(query, new
                {

                    RoutingMasterCode = RoutingMasterCode
                }).FirstOrDefault();
                return result;
            }
            }


        public List<MasterLeadTimeViewModel> GetLocation()
        {
            var dbconnection = DB.Database.GetDbConnection();
            {
                string query = @"select  a.LocationCode
                                        ,a.RoutingMasterCode
                                        ,a.LeadMinutes
                                    
                                        from RoutingLocationLeadTime a";

                var result = dbconnection.Query<MasterLeadTimeViewModel>(query).ToList();

                return result;
            }
        }

        public async Task<List<ProcessLeadTimeForLocation>> CheckLocationCodeAndRoutingCode(string locationcode,string routingmastercode)
        {
            return await DB.ProcessLeadTimeForLocation.Where(x => x.LocationCode == locationcode && x.ProcessMasterCode== routingmastercode).ToListAsync();
        }

        public async Task<int> Add(MasterLeadTimeViewModel model)
        {

            var entity = new ProcessLeadTimeForLocation();
            {
                entity.LocationCode = model.LocationCode;
                entity.ProcessMasterCode = model.RoutingMasterCode;
                entity.LeadMinutes = model.LeadMinutes;

            }
            DB.Add(entity);
            return await DB.SaveChangesAsync();
        }


        public async Task<int> Update(string LocationCode, string RoutingMasterCode, MasterLeadTimeViewModel model)
        {
            var data = await DB.ProcessLeadTimeForLocation.Where(x => x.LocationCode == LocationCode && x.ProcessMasterCode == RoutingMasterCode).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (data != null)
            {
                //data.LocationCode = model.LocationCode;
                //data.RoutingMasterCode = model.RoutingMasterCode;
                data.LeadMinutes = model.LeadMinutes;


                rowsAffected = await DB.SaveChangesAsync();
            }
            return rowsAffected;
        }

        public async Task<int> Remove(string id)
        {
            var data = await DB.ProcessLeadTimeForLocation.Where(x => x.LocationCode == id).FirstOrDefaultAsync();
            if (data != null)
            {
                DB.Remove(data);
            }
            return await DB.SaveChangesAsync();
        }

    }
}
