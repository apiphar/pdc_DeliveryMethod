using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.TANGO.Entities;

namespace TAM.TANGO.Services
{
    public class InspectionAreaService
    {
        private readonly TangoDbContext DB;

        public InspectionAreaService(TangoDbContext db)
        {
            this.DB = db;
        }

        public async Task<List<InspectionArea>> GetAll()
        {
            return await DB.InspectionArea.ToListAsync();
        }
    }
}
