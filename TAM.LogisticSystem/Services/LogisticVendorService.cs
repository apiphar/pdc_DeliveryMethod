using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;
using System;
using Microsoft.AspNetCore.Mvc;

namespace TAM.LogisticSystem.Services
{
    public class LogisticVendorService
    {
        private readonly LogisticDbContext DB;
        private readonly WebEnvironmentService W;

        public LogisticVendorService(LogisticDbContext db, WebEnvironmentService w)
        {
            this.DB = db;
            this.W = w;
        }


        internal async Task<int> Add(DeliveryVendorCreateModel model)
        {
            var user = W.UserHumanName;

            var checkDeliveryVendor = await DB.DeliveryVendor
                .Where(Q => Q.DeliveryVendorCode == model.DeliveryVendorCode)
                .FirstOrDefaultAsync();

            if (checkDeliveryVendor != null)
            {
                return 2;
            }

            var insert = new DeliveryVendor
            {
                DeliveryVendorCode = model.DeliveryVendorCode.ToUpper(),
                Name = model.Name.ToUpper(),
                Address = model.Address.ToUpper(),
                LocationCode = model.Location.LocationCode,
                SAPCode = model.SAPCode.ToUpper(),
                Account = model.Account.ToUpper(),
                CreatedBy = user,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = user,
                UpdatedAt = DateTime.UtcNow
            };

            DB.Add(insert);
            await DB.SaveChangesAsync();

            return 0;
        }

        internal async Task<DeliveryVendor> Get(string id)
        {
            return await DB.DeliveryVendor.FindAsync(id);
        }

        internal async Task<int> Remove(DeliveryVendor entity)
        {
            DB.Remove(entity);
            await DB.SaveChangesAsync();

            return 0;
        }

        internal async Task<int> Update(DeliveryVendor entity, DeliveryVendorCreateModel model)
        {
            var user = W.UserHumanName;

            entity.DeliveryVendorCode = model.DeliveryVendorCode.ToUpper();
            entity.Name = model.Name.ToUpper();
            entity.Address = model.Address.ToUpper();
            entity.LocationCode = model.Location.LocationCode;
            entity.SAPCode = model.SAPCode.ToUpper();
            entity.Account = model.Account.ToUpper();
            entity.UpdatedBy = user;
            entity.UpdatedAt = DateTime.UtcNow;

            await DB.SaveChangesAsync();
            return 0;
        }

        public async Task<List<DeliveryVendorViewModel>> GetAll()
        {
            var model = (await DB.Database.GetDbConnection().QueryAsync<DeliveryVendorViewModel>(@"
SELECT 
	 dv.DeliveryVendorCode,
	 dv.Name,
	 dv.Address,
	 dv.SAPCode,
	 dv.Account,
     dv.LocationCode
FROM DeliveryVendor dv
")).ToList();

            return model;
        }


        public async Task<List<DeliveryVendorLocationModel>> GetLocation()
        {
            var model = (await DB.Database.GetDbConnection().QueryAsync<DeliveryVendorLocationModel>(@"
SELECT
    l.LocationCode,
    l.Name
FROM Location l
")).ToList();

            return model;
        }

    }
}
