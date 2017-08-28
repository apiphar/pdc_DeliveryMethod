using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.TANGO.Entities;
using TAM.TANGO.Models;

namespace TAM.TANGO.Services
{
    public class LocationService
    {
        private readonly TangoDbContext DB;

        public LocationService(TangoDbContext db)
        {
            this.DB = db;
        }

        public async Task<LocationSearchResult> Search(LocationSearchParameters search)
        {
            var query = DB.Location.AsNoTracking();

            if (string.IsNullOrEmpty(search.Query) == false)
            {
                query = query.Where(Q => Q.Name.Contains(search.Query));
            }

            if (search.Type.HasValue)
            {
                query = query.Where(Q => Q.LocationTypeId == search.Type.Value);
            }

            if (search.ClusterId.HasValue)
            {
                query = query.Where(Q => Q.ClusterId == search.ClusterId.Value);
            }

            if (string.IsNullOrEmpty(search.Query) == false && search.Type.HasValue && search.ClusterId.HasValue)
            {
                query = query.Where(Q => Q.Name.Contains(search.Query)  || Q.LocationTypeId == search.Type || Q.ClusterId == search.ClusterId );
            }

            var count = await query.CountAsync();
            var skip = (search.Page - 1) * search.ItemsPerPage;
            var paged = await query.OrderBy(Q => Q.Name).Skip(skip).Take(search.ItemsPerPage).ToListAsync();

            var result = new LocationSearchResult(search, count, paged);
            return result;
        }

        internal async Task<int> Add(string name, int type, int parkColumnTotal, int parkRowTotal, int clusterId)
        {
            var insert = new Location
            {
                Name = name,
                LocationTypeId = type,
                ParkingColumnTotal = parkColumnTotal,
                ParkingRowTotal = parkRowTotal,
                ClusterId = clusterId
            };

            DB.Add(insert);
            await DB.SaveChangesAsync();

            return insert.LocationId;
        }

        internal async Task<Location> Get(int id)
        {
            return await DB.Location.FindAsync(id);
        }

        internal async Task Remove(Location entity)
        {
            DB.Remove(entity);
            await DB.SaveChangesAsync();
        }

        internal async Task Update(Location entity, string name, int type,  int parkColumnTotal, int parkRowTotal, int clusterId)
        {
            entity.Name = name;
            entity.LocationTypeId = type;
            entity.ParkingColumnTotal = parkColumnTotal;
            entity.ParkingRowTotal = parkRowTotal;
            entity.ClusterId = clusterId;

            await DB.SaveChangesAsync();
        }

        public async Task<List<Location>> GetAll()
        {
            return await DB.Location.ToListAsync();
        }
    }
}
