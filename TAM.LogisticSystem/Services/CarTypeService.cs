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
    public class CarTypeService
    {
        private readonly LogisticDbContext logisticDbContext;        
        private readonly WebEnvironmentService WebEnvService;

        public CarTypeService(LogisticDbContext tangoDb, WebEnvironmentService webEnvService)
        {
            this.logisticDbContext = tangoDb;
            this.WebEnvService = webEnvService;
        }

        //TIE: START
        public List<CarTypeViewModel> getAllCarType()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            var query = @"select a.*	
, b.Name as CarSeriesName
,c.AfiCarTypeCode
from CarType a
left outer join CarSeries b on b.CarSeriesCode = a.CarSeriesCode
LEFT OUTER JOIN aficartype c ON c.aficartypecode = a.aficartypecode
order by a.Katashiki ASC";

            var result = dbconnection.Query<CarTypeViewModel>(query, new { }).ToList();

            return result;
        }

        public List<CarSeriesModel> getAllCarSeries()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            var query = @"select a.CarSeriesCode
                              , a.Name as CarSeriesName
                            from CarSeries a";

            var result = dbconnection.Query<CarSeriesModel>(query, new { }).ToList();
            return result;
        }

        public List<AfiCarTypeModel> getAllAfiCarType()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            var query = @"select a.AfiCarTypeCode
                             , a.jenis as aficartypeName
                          from AfiCarType a";

            var result = dbconnection.Query<AfiCarTypeModel>(query, new { }).ToList();
            return result;
        }

        public async Task<int> Add(CarTypeViewModel model)
        {
            var rowsAffected = 0;
            var username = WebEnvService.UserHumanName;
            var existingCarType = await logisticDbContext.CarType.Where(x => x.Katashiki == model.Katashiki && x.Suffix == model.Suffix).FirstOrDefaultAsync();
            if (existingCarType == null)
            {
                var entity = new CarType();
                {
                    entity.Katashiki = model.Katashiki;
                    entity.Suffix = model.Suffix;
                    entity.CarSeriesCode = model.CarSeriesCode;
                    entity.Name = model.Name;
                    //entity.CarCategoryId = model.CarCategoryId;
                    //entity.HSCode = model.HSCode = null;
                    entity.AFICarTypeCode = model.AfiCarTypeCode;
                    entity.EngineDescription = model.EngineDescription;
                    entity.EngineVolume = model.EngineVolume;
                    entity.SteerPosition = model.SteerPosition;
                    entity.WheelDiameter = model.WheelDiameter;
                    entity.WheelSize = model.WheelSize;
                    entity.Assembly = model.Assembly;
                    if (entity.Assembly == null)
                    {
                        entity.Assembly = " ";
                    }
                    entity.IsFreeTaxZone = model.IsFTZ;
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.CreatedBy = username;
                    entity.UpdatedBy = username;
                };
                logisticDbContext.Add(entity);
                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }

        public async Task<int> Update(string katashiki, string suffix, CarTypeViewModel model)
        {
            var rowsAffected = 0;
            var username = WebEnvService.UserHumanName;
            var existingCarType = await logisticDbContext.CarType.Where(x => x.Katashiki == katashiki && x.Suffix == suffix).FirstOrDefaultAsync();

            if (existingCarType != null)
            {
                existingCarType.CarSeriesCode = model.CarSeriesCode;
                existingCarType.Name = model.Name;
                //existingCarType.CarCategoryId = model.CarCategoryId;
                //existingCarType.HSCode = model.HSCode = null;
                existingCarType.AFICarTypeCode = model.AfiCarTypeCode;
                existingCarType.EngineDescription = model.EngineDescription;
                existingCarType.EngineVolume = model.EngineVolume;
                existingCarType.SteerPosition = model.SteerPosition;
                existingCarType.WheelDiameter = model.WheelDiameter;
                existingCarType.WheelSize = model.WheelSize;
                existingCarType.Assembly = model.Assembly;
                existingCarType.IsFreeTaxZone = model.IsFTZ;
                existingCarType.UpdatedAt = DateTime.UtcNow;
                existingCarType.UpdatedBy = username;
                

                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }
        //TIE: END

        public async Task<int> Remove(string katashiki, string suffix)
        {
            var existingCarType = await logisticDbContext.CarType.Where(x => x.Katashiki == katashiki && x.Suffix == suffix).FirstOrDefaultAsync();
            if (existingCarType != null)
            {
                logisticDbContext.Remove(existingCarType);
            }
            return await logisticDbContext.SaveChangesAsync();
        }
    }
}
