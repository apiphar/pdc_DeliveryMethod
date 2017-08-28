using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.TANGO.Entities;
using TAM.TANGO.Models;
using System.Collections.Generic;


namespace TAM.TANGO.Services
{
    public class RoutingGroupService
    {
        private readonly TangoDbContext DB;

        public RoutingGroupService(TangoDbContext tangoDb)
        {
            this.DB = tangoDb;
        }

        public async Task<RoutingGroupSearchResult> Search (RoutingGroupSearchParameter SearchParameter)
        {
            var query = DB.RoutingGroup.AsNoTracking();

            if(string.IsNullOrEmpty(SearchParameter.Query) == false)
            {
                query = query.Where(Q => Q.Name.Contains(SearchParameter.Query));
            }

            if (SearchParameter.RoutingGroupId.HasValue)
            {
                query = query.Where(Q => Q.RoutingGroupId == SearchParameter.RoutingGroupId.Value);
            }

            var count = await query.CountAsync();
            var skip = (SearchParameter.Page - 1) * SearchParameter.ItemsPerPage;
            var paged = await query.OrderBy(Q => Q.Name).Skip(skip).Take(SearchParameter.ItemsPerPage).ToListAsync();

            var result = new RoutingGroupSearchResult(SearchParameter, count, paged);
            return result;
        }

        public async Task<int> Add(string name)
        {
            DB.Add(new RoutingGroup
            {
                Name = name
            });
            return await DB.SaveChangesAsync();
        }

        public  async Task<RoutingGroup> Get(int id)
        {
            return await DB.RoutingGroup.FirstOrDefaultAsync(m => m.RoutingGroupId == id);
        }

        public async Task Update(RoutingGroup entity, string name)
        {
            entity.Name = name;
            await DB.SaveChangesAsync();
        }

        public async Task<int> Remove(RoutingGroup entity)
        {
            DB.Remove(entity);
            return await DB.SaveChangesAsync();
        }

        public async Task<List<RoutingGroup>> GetAll()
        {
            return await DB.RoutingGroup.ToListAsync();
        }


    }
}
