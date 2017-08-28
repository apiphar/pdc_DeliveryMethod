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
    public class InspectionPartService
    {
        private readonly LogisticDbContext logisticDbContext;

        public InspectionPartService(LogisticDbContext tangoDb)
        {
            this.logisticDbContext = tangoDb;
        }        

        public List<InspectionPartViewModel> GetAll()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            {
				string query = @"SELECT a.InspectionPartId 
                                , a.Name
								, b.InspectionCategoryId
                                , b.Name as Category
                                , c.InspectionSideId
                                , c.name as SideName
                                , a.CreatedAt, a.CreatedBy
                                , a.UpdatedAt, a.UpdatedBy
                                from InspectionPart a
                                LEFT JOIN InspectionCategory b ON b.InspectionCategoryId = a.InspectionCategoryId
                                LEFT JOIN InspectionSide c ON c.InspectionSideId = a.InspectionSideId";

				var result = dbconnection.Query<InspectionPartViewModel>(query, new
				{
				}).ToList();

                return result;
            }
		}

		public List<InspectionCategory> GetCategory()
		{
			var categories = logisticDbContext.InspectionCategory.OrderBy(x => x.Name).Select(x => new InspectionCategory()
			{
				Name = x.Name,
				InspectionCategoryId = x.InspectionCategoryId
			}).ToList();

			categories.Insert(0, new InspectionCategory()
			{
				InspectionCategoryId = 0,
				Name = "(Please Choose One)",
			});

			return categories;
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

        public async Task<int> Add(InspectionPartViewModel model)
        {
            var entity = new InspectionPart();
            {
                entity.Name = model.Name;
                entity.InspectionCategoryId = model.InspectionCategoryId;
                entity.InspectionSideId = model.InspectionSideId;
				entity.CreatedAt = model.CreatedAt;
				entity.CreatedBy = model.CreatedBy;
				entity.UpdatedAt = model.UpdatedAt;
				entity.UpdatedBy = model.UpdatedBy;
			};

            logisticDbContext.Add(entity);
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task Update(string id, InspectionPartViewModel model)
        {
            var existingTariff = await logisticDbContext.InspectionPart.Where(x => x.InspectionPartCode == id).FirstOrDefaultAsync();

            if (existingTariff != null)
            {
                existingTariff.Name = model.Name;
                existingTariff.InspectionCategoryId = model.InspectionCategoryId;
                existingTariff.InspectionSideId = model.InspectionSideId;

                await logisticDbContext.SaveChangesAsync();
            }
        }

        public async Task Remove(string id)
        {
            var existingCarType = await logisticDbContext.InspectionPart.Where(x => x.InspectionPartCode == id).FirstOrDefaultAsync();
            if (existingCarType != null)
            {
                logisticDbContext.Remove(existingCarType);
            }

            await logisticDbContext.SaveChangesAsync();
        }
    }
}
