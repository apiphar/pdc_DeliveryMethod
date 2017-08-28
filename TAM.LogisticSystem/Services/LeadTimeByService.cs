using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;

namespace TAM.LogisticSystem.Services
{
    public class LeadTimeByService
    {
        private readonly LogisticDbContext DB;

        public LeadTimeByService(LogisticDbContext db)
        {
            this.DB = db;
        }

        public List<ProcessLeadTimeByEnum> GetAll()
        {
            return DB.ProcessLeadTimeByEnum.ToList();
        }
    }
}
