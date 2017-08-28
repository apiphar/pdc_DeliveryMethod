using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.TANGO.Entities;
using TAM.TANGO.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TAM.TANGO.Services
{
    public class DealerService
    {
        private readonly TangoDbContext DB;

        public DealerService(TangoDbContext db)
        {
            this.DB = db;
        }

        public async Task<DealerSearchResult> Search(DealerSearchParameters search)
        {
            var query = DB.Dealer.AsNoTracking();

            if (string.IsNullOrEmpty(search.Query) == false)
            {
                query = query.Where(Q => Q.Name.Contains(search.Query));
            }
            

            var count = await query.CountAsync();
            var skip = (search.Page - 1) * search.ItemsPerPage;
            var paged = await query.OrderBy(Q => Q.Name).Skip(skip).Take(search.ItemsPerPage).ToListAsync();

            var result = new DealerSearchResult(search, count, paged);
            return result;
        }

        internal async Task<int> Add(string name)
        {
            var insert = new Dealer
            {
                Name = name
            };

            DB.Add(insert);
            await DB.SaveChangesAsync();

            return insert.DealerId;
        }

        internal async Task<Dealer> Get(int id)
        {
            return await DB.Dealer.FindAsync(id);
        }

        internal async Task Remove(Dealer entity)
        {
            DB.Remove(entity);
            await DB.SaveChangesAsync();
        }

        internal async Task Update(Dealer entity, string name)
        {
            entity.Name = name;
            await DB.SaveChangesAsync();
        }
    }
}
