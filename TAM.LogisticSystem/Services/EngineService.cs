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
    public class EngineService
    {
        private readonly LogisticDbContext logisticDbContext;
        public EngineService(LogisticDbContext logisticDbContext)
        {
            this.logisticDbContext = logisticDbContext;
        }

        // TIE: START
        //public List<EngineViewModel> Get()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select a.Katashiki
        //                              ,a.Suffix
        //                     from CarType a";

        //        var result = dbconnection.Query<EngineViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public List<EngineViewModel> GetEngine()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select a.KatashikiValidationId,
        //                                a.engineprefix,
        //                                a.FrameCode,   
        //                                b.katashiki,
        //                                e.CarModelCode,
        //                                e.Name as CarModelName
        //                        from KatashikiValidation a
        //                        left join KatashikiValidation b on a.KatashikiValidationId = b.KatashikiValidationId
        //                        left join  CarType c on a.Katashiki = c.Katashiki
        //                        left join CarSeries d on c.CarSeriesCode = d.CarSeriesCode
        //                        left join CarModel e on  d.CarModelCode = e.CarModelCode";

        //        var result = dbconnection.Query<EngineViewModel>(query, new
        //        {

        //        }).ToList();

        //        return result;
        //    }
        //}

        //public List<KatashikiValidation> GetData()
        //{
        //    var data = logisticDbContext.KatashikiValidation.ToList();
        //    return data;
        //}

        //public List<EngineViewModel> GetCarModel(string Katashiki)
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    string query = @"select a.Katashiki
        //                    , a.Suffix
        //                    , b.CarModelCode
        //                    , c.Name CarModelName
        //                 from CarType a
        //                 left outer join CarSeries b on a.CarSeriesCode = b.CarSeriesCode
        //                    left outer join CarModel c on b.CarModelCode = c.CarModelCode
        //                 where a.Katashiki = @Katashiki";
        //    var result = dbconnection.Query<EngineViewModel>(query, new
        //    {
        //        Katashiki = Katashiki,

        //    }).ToList();
        //    return result;
        //}

        //public async Task<int> Add(EngineViewModel model)
        //{
        //    var entity = new KatashikiValidation();
        //    {

        //        entity.Katashiki = model.Katashiki;
        //        entity.EnginePrefix = model.EnginePrefix;
        //        entity.FrameCode = model.FrameCode;
        //        entity.CreatedAt = model.CreatedAt;
        //        entity.CreatedBy = model.CreatedBy = "Jati";
        //        entity.UpdatedAt = model.UpdatedAt;
        //        entity.UpdatedBy = model.UpdatedBy = "jati";
        //    };

        //    logisticDbContext.Add(entity);
        //    return await logisticDbContext.SaveChangesAsync();
        //}

        //public async Task<int> Update(int id, EngineViewModel model)
        //{
        //    var existingEngine = await logisticDbContext.KatashikiValidation.Where(x => x.KatashikiValidationId == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (existingEngine != null)
        //    {
        //        existingEngine.Katashiki = model.Katashiki;
        //        //existingPermit.Suffix = model.Suffix;
        //        existingEngine.EnginePrefix = model.EnginePrefix;
        //        existingEngine.FrameCode = model.FrameCode;

        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }
        //    return rowsAffected;
        //}

        //public async Task<int> Remove(int id)
        //{
        //    var existingEngine = await logisticDbContext.KatashikiValidation.Where(x => x.KatashikiValidationId == id).FirstOrDefaultAsync();
        //    if (existingEngine != null)
        //    {
        //        logisticDbContext.Remove(existingEngine);
        //    }

        //    return await logisticDbContext.SaveChangesAsync();
        //}
        // TIE: END
    }
}
