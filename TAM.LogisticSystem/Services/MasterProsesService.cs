using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace TAM.LogisticSystem.Services
{
    public class MasterProsesService
    {
        private readonly LogisticDbContext DB;

        public MasterProsesService(LogisticDbContext db)
        {
            this.DB = db;
        }

        public List<ProcessLeadTimeByEnum> GetRoutingLeadTimeByList()
        {
            return DB.ProcessLeadTimeByEnum.ToList();
        }

        public List<ProcessMaster> GetRoutingMasterList()
        {
            return DB.ProcessMaster.ToList();
        }

        public void Save(MasterProsesViewModel model, string user)
        {
            var entity = new ProcessMaster();
            {
                entity.ProcessMasterCode = model.processMasterCode;
                entity.Name = model.name;
                entity.IsScan = model.isScan;
                entity.ProcessLeadTimeByEnumId = model.processLeadTimeByEnumId;
                entity.BufferMinutes = model.bufferMinutes;
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.CreatedBy = user;
                entity.UpdatedBy = user;
            };

            DB.Add(entity);

            DB.SaveChanges();
        }

        public void Update(string code, MasterProsesViewModel model, string user)
        {
            var data = DB.ProcessMaster.Where(x => x.ProcessMasterCode == code).FirstOrDefault();

            if (data != null)
            {
                data.Name = model.name;
                data.IsScan = model.isScan;
                data.BufferMinutes = model.bufferMinutes;
                data.ProcessLeadTimeByEnumId = model.processLeadTimeByEnumId;
                data.UpdatedAt = DateTime.UtcNow;
                data.UpdatedBy = user;

                DB.SaveChanges();
            }
        }

        public void Delete(string code)
        {
            var data = new ProcessMaster { ProcessMasterCode = code };

            DB.ProcessMaster.Attach(data);
            DB.ProcessMaster.Remove(data);
            DB.SaveChanges();
        }
    }
}
