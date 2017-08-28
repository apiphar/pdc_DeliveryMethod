using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace TAM.LogisticSystem.Services
{
    public class LogisticVehicleService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService webEnvironmentService;

        public LogisticVehicleService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.logisticDbContext = logisticDbContext;
            this.webEnvironmentService = webEnvironmentService;
        }

        // TIE: START
        //public List<LogisticVehicleModel> GetDataDeliveryMethod()
        //{
        //    var dbConnection = logisticDbContext.Database.GetDbConnection();
        //    string query = @"select a.DeliveryMethodCode
        //                            , a.Name
        //                      , a.NeedSJKBValidation
        //                      , a.DeliveryMethodTypeId	
        //                      , b.Name as DeliveryMethodTypeName	
        //                    from DeliveryMethod a
        //                    left outer join DeliveryMethodType b on b.DeliveryMethodTypeId = a.DeliveryMethodTypeId";

        //    var result = dbConnection.Query<LogisticVehicleModel>(query, new { }).ToList();

        //    return result;
        //}

        //public List<DeliveryMethodTypeModel> GetDataDeliveryMethodType()
        //{
        //    var dbConnection = logisticDbContext.Database.GetDbConnection();
        //    string query = @"select a.DeliveryMethodTypeId	
        //                      , a.Name as DeliveryMethodTypeName	
        //                    from DeliveryMethodType a";

        //    var result = dbConnection.Query<DeliveryMethodTypeModel>(query, new { }).ToList();

        //    return result;
        //}

        ////public List<DeliveryMethodType> GetDataDeliveryMethodType()
        ////{
        ////    return logisticDbContext.DeliveryMethodType.ToList();
        ////}

        //public async Task<int> Add(LogisticVehicleModel model)
        //{
        //    var userName = webEnvironmentService.UserHumanName;
        //    var existingDeliveryMethod = await logisticDbContext.DeliveryMethod.Where(x => x.DeliveryMethodCode == model.DeliveryMethodCode).FirstOrDefaultAsync();
        //    int rowsAffected = 0;
        //    if (existingDeliveryMethod == null)
        //    {
        //        var entity = new DeliveryMethod();
        //        {
        //            entity.DeliveryMethodCode = model.DeliveryMethodCode;
        //            entity.Name = model.Name;
        //            entity.DeliveryMethodTypeId = model.DeliveryMethodTypeId;
        //            entity.NeedSJKBValidation = model.NeedSJKBValidation;
        //            entity.CreatedAt = DateTime.UtcNow;
        //            entity.UpdatedAt = DateTime.UtcNow;
        //            entity.CreatedBy = userName;
        //            entity.UpdatedBy = userName;
        //        };
        //        logisticDbContext.Add(entity);
        //        rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }               
        //    return rowsAffected;
        //}

        //public async Task<int> Update(string id, LogisticVehicleModel model)
        //{
        //    var userName = webEnvironmentService.UserHumanName;
        //    var existingDeliveryMethod = await logisticDbContext.DeliveryMethod.Where(x => x.DeliveryMethodCode == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (existingDeliveryMethod != null)
        //    {
        //            existingDeliveryMethod.Name = model.Name;
        //            existingDeliveryMethod.DeliveryMethodTypeId = model.DeliveryMethodTypeId;
        //            existingDeliveryMethod.NeedSJKBValidation = model.NeedSJKBValidation;
        //            existingDeliveryMethod.UpdatedAt = DateTime.UtcNow;
        //            existingDeliveryMethod.UpdatedBy = userName;

        //            rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    }
        //    return rowsAffected;
        //}
        // TIE: END

        public async Task<int> Remove(string id)
        {
            var existingDeliveryMethod = await logisticDbContext.DeliveryMethod.Where(x => x.DeliveryMethodCode == id).FirstOrDefaultAsync();
            if (existingDeliveryMethod != null)
            {
                logisticDbContext.Remove(existingDeliveryMethod);
            }
            return await logisticDbContext.SaveChangesAsync();
        }
    }
}
