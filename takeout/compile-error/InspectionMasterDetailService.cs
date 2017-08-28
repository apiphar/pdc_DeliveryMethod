using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.TANGO.Entities;
using TAM.TANGO.Models; 


namespace TAM.TANGO.Services
{
    public class InspectionMasterDetailService
    {
        private readonly TangoDbContext DB;

        public InspectionMasterDetailService(TangoDbContext db)
        {
            this.DB = db;
        }

        public async Task<InspectionMasterDetailSearchResult> Search(InspectionMasterDetailSearchParameters search)
        {
            var query = DB.InspectionMasterDetail.AsNoTracking();

            if (string.IsNullOrEmpty(search.Query) == false)
            {
                query = query.Where(Q => Q.Name.Contains(search.Query));
            }
            if (search.Type.HasValue)
            {
                query = query.Where(Q => Q.InspectionAreaId == search.Type.Value);
            }

            var count = await query.CountAsync();
            var skip = (search.Page - 1) * search.ItemsPerPage;
            var paged = await query.OrderBy(Q => Q.Name).Skip(skip).Take(search.ItemsPerPage).ToListAsync();

            var result = new InspectionMasterDetailSearchResult(search, count, paged);
            return result;
        }

        internal async Task<int> Add(InspectionMasterDetailCreateOrUpdateRequest model)
        {
            var insert = new InspectionMasterDetail
            {
                InspectionMasterId = model.InspectionMasterId,
                InspectionAreaId = model.InspectionAreaId,
                Name = model.Name,
                Description = model.Description
            };

            DB.Add(insert);
            int recordAffected = await DB.SaveChangesAsync();
            return recordAffected;
        }

        internal async Task<InspectionMasterDetail> Get(int Id)
        {
            return await DB.InspectionMasterDetail.FindAsync(Id);
        }

        internal async Task Remove(InspectionMasterDetail entity)
        {
            DB.Remove(entity);
            await DB.SaveChangesAsync();
        }
        internal async Task<int> Update(InspectionMasterDetail entity, InspectionMasterDetailCreateOrUpdateRequest model)
        {
            entity.InspectionMasterId = model.InspectionMasterId;
            entity.InspectionAreaId = model.InspectionAreaId;
            entity.Name = model.Name;
            entity.Description = model.Description;

            return await DB.SaveChangesAsync();
        }

        public async Task<List<InspectionMasterDetail>> GetAll()
        {
            return await DB.InspectionMasterDetail.ToListAsync();
        }
        
    }
}
