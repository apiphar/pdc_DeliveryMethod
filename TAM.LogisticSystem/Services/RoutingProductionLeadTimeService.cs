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
    public class RoutingProductionLeadTimeService
    {
        private readonly LogisticDbContext DB;

        public RoutingProductionLeadTimeService(LogisticDbContext db)
        {
            this.DB = db;
        }

        // TIE: START
        //public List<RoutingProductionLeadTimeViewModel> GetData()
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    {
        //        string query = @"SELECT
        //                            a.RoutingDictionaryProductionId
        //                            ,f.LocationCode
        //                            ,a.Katashiki
        //                            ,a.Suffix
        //                            ,a.RoutingMasterCode
        //                            ,a.Ordering
        //                            ,e.CarModelCode
        //                            ,a.LeadMinutes
        //                            ,b.Name AS NamaRute
        //                            ,e.Name AS NamaType
        //	,f.Name AS NamaLocation

        //                            from RoutingDictionaryProduction a
        //                            LEFT OUTER JOIN dbo.RoutingMaster b ON  a.RoutingMasterCode = b.RoutingMasterCode
        //                            LEFT OUTER JOIN dbo.CarType c ON a.Katashiki = c.Katashiki
        //                            LEFT OUTER JOIN dbo.CarSeries d ON c.CarSeriesCode = d.CarSeriesCode
        //                            LEFT OUTER JOIN dbo.CarModel e ON d.CarModelCode = e.CarModelCode
        //	LEFT OUTER JOIN dbo.Location f ON a.LocationCode = f.LocationCode";

        //        var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}


        //public RoutingProductionLeadTimeViewModel GetKodeRute(string RoutingMasterCode)
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    {
        //        string query = @"select a.RoutingMasterCode
        //                                ,a.Name AS NamaRute
        //                                from RoutingMaster a
        //                                where a.RoutingMasterCode = @RoutingMasterCode";

        //        var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //        {

        //            RoutingMasterCode = RoutingMasterCode
        //        }).FirstOrDefault();
        //        return result;
        //    }
        //}


        //     public List<RoutingProductionLeadTimeViewModel> GetKatashiki()
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    {
        //        string query = @"select a.Katashiki
        //                         ,a.Suffix
        //                         from CarType a";

        //        var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public List<RoutingProductionLeadTimeViewModel> GetRoutingMasterCode()
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    {
        //        string query = @"select a.RoutingMasterCode
        //                                ,a.Name AS NamaRute
        //                                from RoutingMaster a";

        //        var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public List<RoutingProductionLeadTimeViewModel> GetLocationCode()
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    {
        //        string query = @"select a.LocationCode
        //                     ,a.Name AS NamaLocation
        //                     from Location a";

        //        var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public List<RoutingProductionLeadTimeViewModel> GetSuffix(string Katashiki)
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    string query = @"select a.Katashiki
        //                         ,a.Suffix
        //                         from CarType a
        //                   where a.Katashiki = @Katashiki";

        //    var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //    {
        //        Katashiki = Katashiki
        //    }).ToList();
        //    return result;

        //}

        //public RoutingProductionLeadTimeViewModel GetCarModel(string Katashiki, string Suffix)
        //{
        //    var dbconnection = DB.Database.GetDbConnection();
        //    string query = @"select a.Katashiki
        //                    , a.Suffix
        //                    , b.CarSeriesCode
        //                    , b.CarModelCode
        //                    , c.Name AS NamaType
        //                 from CarType a
        //                 left outer join CarSeries b on a.CarSeriesCode = b.CarSeriesCode
        //                    left outer join CarModel c on b.CarModelCode = c.CarModelCode
        //                 where a.Katashiki = @Katashiki and a.Suffix = @Suffix";
        //    var result = dbconnection.Query<RoutingProductionLeadTimeViewModel>(query, new
        //    {
        //        Katashiki = Katashiki,
        //        Suffix = Suffix
        //    }).FirstOrDefault();
        //    return result;
        //}



        //public async Task<int> Add(RoutingProductionLeadTimeViewModel model)
        //{
        //    var entity = new RoutingDictionaryProduction
        //    {
        //        LocationCode = model.LocationCode,
        //        Katashiki = model.Katashiki,
        //        Suffix = model.Suffix,
        //        RoutingMasterCode = model.RoutingMasterCode,
        //        Ordering = model.Ordering,
        //        LeadMinutes = model.LeadMinutes
        //    };
        //    DB.Add(entity);
        //    await DB.SaveChangesAsync();

        //    return entity.RoutingDictionaryProductionId;
        //}


        //public async Task<int> Update(int id, RoutingProductionLeadTimeViewModel model)
        //{
        //    var data = await DB.RoutingDictionaryProduction.Where(x => x.RoutingDictionaryProductionId == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (data != null)
        //    {

        //        data.LocationCode = model.LocationCode;
        //        data.Katashiki = model.Katashiki;
        //        data.Suffix = model.Suffix;
        //        data.RoutingMasterCode = model.RoutingMasterCode;
        //        data.Ordering = model.Ordering;
        //        data.LeadMinutes = model.LeadMinutes;


        //        rowsAffected = await DB.SaveChangesAsync();
        //    }
        //    return rowsAffected;
        //}

        //public async Task<int> Remove(int id)
        //{
        //    var data = await DB.RoutingDictionaryProduction.Where(x => x.RoutingDictionaryProductionId == id).FirstOrDefaultAsync();
        //    if (data != null)
        //    {
        //        DB.Remove(data);
        //    }
        //    return await DB.SaveChangesAsync();
        //}
        // TIE: END
    }
}
