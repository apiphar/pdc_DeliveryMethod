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
    public class InspectionItemService
    {
        private readonly LogisticDbContext logisticDbContext;

        public InspectionItemService(LogisticDbContext tangoDb)
        {
            this.logisticDbContext = tangoDb;
        }        

        public List<InspectionItemViewModel> GetAll()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            {
                string query = @"select a.InspectionItemId
                                  , a.Name as ItemName
                                  , a.Category
                                  , b.InspectionSideId
                                  , b.name as SideName
                                from InspectionItem a
                                left outer join InspectionSide b on b.InspectionSideId = a.InspectionSideId";

                var result = dbconnection.Query<InspectionItemViewModel>(query, new
                {
                }).ToList();

                return result;
            }
        }

        public List<InspectionSide> GetSide()
        {
            var sides = logisticDbContext.InspectionSide.OrderBy(x => x.Name).Select(x => new InspectionSide()
            {
                Name = x.Name,
                InspectionSideId = x.InspectionSideId
            }).ToList();

            sides.Insert(0, new InspectionSide()
            {
                InspectionSideId = 0,
                Name = "(Please Choose One)",
            });

            return sides;
        }

        public async Task<int> Add(InspectionItemViewModel model)
        {
            var entity = new InspectionItem();
            {
                entity.Name = model.ItemName;
                //entity.Category = model.Category;
                //entity.InspectionSideId = model.InspectionSideId;
            };

            logisticDbContext.Add(entity);
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task Update(string id, InspectionItemViewModel model)
        {
            var existingTariff = await logisticDbContext.InspectionItem.Where(x => x.InspectionItemCode == id).FirstOrDefaultAsync();

            if (existingTariff != null)
            {
                existingTariff.Name = model.ItemName;
                //existingTariff.Category = model.Category;
                //existingTariff.InspectionSideId = model.InspectionSideId;

                await logisticDbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(string id)
        {
            var existingCarType = await logisticDbContext.InspectionItem.Where(x => x.InspectionItemCode == id).FirstOrDefaultAsync();
            if (existingCarType != null)
            {
                logisticDbContext.Remove(existingCarType);
            }

            await logisticDbContext.SaveChangesAsync();
        }
    }
}
