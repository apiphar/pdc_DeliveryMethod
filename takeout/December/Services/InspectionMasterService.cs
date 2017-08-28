using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace TAM.LogisticSystem.Services
{
    public class InspectionMasterService
    {
        private readonly LogisticDbContext DB;

        public InspectionMasterService(LogisticDbContext db)
        {
            this.DB = db;
        }

        public async Task<List<InspectionMaster>> GetAll()
        {
            return await DB.InspectionMaster.ToListAsync();
        }
    }
}
