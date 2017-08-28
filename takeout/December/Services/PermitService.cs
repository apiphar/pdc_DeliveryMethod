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
    public class PermitService
    {
        private readonly LogisticDbContext logisticDbContext;

        public PermitService(LogisticDbContext logisticDbContext)
        {
            this.logisticDbContext = logisticDbContext;
        }

        public List<PermitViewModel> Get()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            {
                string query = @"select a.Katashiki
	                                     ,a.Suffix
	                            from CarType a";

                var result = dbconnection.Query<PermitViewModel>(query, new
                {
                }).ToList();

                return result;
            }
		}

		public async Task<Permit> Get(int id)
		{
			return await logisticDbContext.Permit.FirstOrDefaultAsync(m => m.PermitId == id);
		}

		public List<PermitViewModel> GetSuffix(string Katashiki)
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            string query = @"select a.Katashiki
	                          ,a.Suffix
	                          from CarType a
	                          where a.Katashiki = @Katashiki";

            var result = dbconnection.Query<PermitViewModel>(query, new
            {
                Katashiki = Katashiki
            }).ToList();
            return result;

        }

        public List<PermitViewModel> GetPermit()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            {
                string query = @"select b.Katashiki
                                        , b.Suffix
                                        , a.PermitId
                                        , a.Quota
                                        , a.EffectiveFrom
                                        , a.EffectiveUntil
                                        , d.CarModelCode
                                        , e.Name
                                        FROM Permit a
                                        left outer join Permit b on a.PermitId = b.PermitId
                                        left outer join CarType c on a.Katashiki = c.Katashiki and a.Suffix = c.Suffix
                                        left outer join CarSeries d on c.CarSeriesCode = d.CarSeriesCode
                                        left outer join CarModel e on d.CarModelCode = e.CarModelCode";

                var result = dbconnection.Query<PermitViewModel>(query, new
                {

                }).ToList();

                return result;
            }
        }

        public PermitViewModel GetCarModel(string Katashiki, string Suffix)
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            string query = @"select a.Katashiki
                            , a.Suffix
                            , b.CarSeriesCode
                            , b.CarModelCode
                            , c.Name
	                        from CarType a
	                        left outer join CarSeries b on a.CarSeriesCode = b.CarSeriesCode
                            left outer join CarModel c on b.CarModelCode = c.CarModelCode
	                        where a.Katashiki = @Katashiki and a.Suffix = @Suffix";
            var result = dbconnection.Query<PermitViewModel>(query, new
            {
                Katashiki = Katashiki,
                Suffix = Suffix
            }).FirstOrDefault();
            return result;
        }

        public async Task<int> Update(int id, PermitViewModel model)
        {
            var existingPermit = await logisticDbContext.Permit.Where(x => x.PermitId == id).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (existingPermit != null)
            {
                existingPermit.Quota = model.Quota;
                existingPermit.ValidFrom = model.EffectiveFrom;
                existingPermit.ValidTo = model.EffectiveUntil;
                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }

            return rowsAffected;

        }


        public List<Permit> GetData()
        {
            var data = logisticDbContext.Permit.ToList();
            return data;
        }

        public async Task<int> Add(PermitViewModel model)
        {
            var entity = new Permit();
            {
                entity.PermitId = model.PermitId;
                entity.Katashiki = model.Katashiki;
                entity.Suffix = model.Suffix;
                entity.Quota = model.Quota;
                entity.ValidFrom = model.EffectiveFrom;
                entity.ValidTo = model.EffectiveUntil;
				entity.CreatedAt = model.CreatedAt;
				entity.CreatedBy = model.CreatedBy;
				entity.UpdatedAt = model.UpdatedAt;
				entity.UpdatedBy = model.UpdatedBy;
            };

            logisticDbContext.Add(entity);
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task<int> Remove(Permit entity)
        {
            logisticDbContext.Remove(entity);
            return await logisticDbContext.SaveChangesAsync();
        }



    }

}
