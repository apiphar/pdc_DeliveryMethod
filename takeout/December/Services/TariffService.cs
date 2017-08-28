using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class TariffService
    {
        private readonly LogisticDbContext logisticDbContext;

        public TariffService(LogisticDbContext tangoDb)
        {
            this.logisticDbContext = tangoDb;
        }

        public List<TariffViewModel> GetAllData()
        {
            var data = logisticDbContext.HarmonizeTariff.Join(
                logisticDbContext.Harmonize,
                HarmonizeTariff => HarmonizeTariff.HSCode,
                Harmonize => Harmonize.HSCode,
                (HarmonizeTariff, Harmonize) => new TariffViewModel
                {
                   TariffId = HarmonizeTariff.HarmonizeTariffId,
                   HSCode=HarmonizeTariff.HSCode,
                   BM=HarmonizeTariff.BeaMasukPercentage,
                   EffectiveDateFrom=Harmonize.ValidFrom,
                   PPH=Harmonize.PphPercentage,
                   PPn=Harmonize.PpnPercentage,
                   PPnBM=Harmonize.PpnbmPercentage,
                   Scheme=HarmonizeTariff.Schema
                }).ToList();

            return data;
        }
        
        public async Task<int> Add(TariffViewModel model)
        {
            var entity = new Harmonize();
            {
                entity.HSCode = model.HSCode;
                entity.PphPercentage = model.PPH;
                entity.PpnPercentage = model.PPn;
                entity.PpnbmPercentage = model.PPnBM;
                entity.ValidFrom = model.EffectiveDateFrom;
                entity.ValidUntil = model.EffectiveDateFrom.AddYears(10);
                entity.CreatedBy = "system";
                entity.UpdatedBy = "system";
            };
            logisticDbContext.Add(entity);
            foreach(SchemeTable item in model.schemeTable)
            {
                var data = new HarmonizeTariff();
                {
                    data.HSCode = model.HSCode;
                    data.BeaMasukPercentage = item.BM;
                    data.Schema = item.Scheme;
                    data.CreatedBy = "system";
                    data.UpdatedBy = "system";
                };
                logisticDbContext.Add(data);
            }
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task<int> Update(int id,TariffViewModel model)
        {
            var entity = logisticDbContext.Harmonize.Where(x=>x.HSCode.Equals(model.HSCode)).FirstOrDefault();
            if (entity != null)
            {
                entity.HSCode = model.HSCode;
                entity.PphPercentage = model.PPH;
                entity.PpnPercentage = model.PPn;
                entity.PpnbmPercentage = model.PPnBM;
                entity.ValidFrom = model.EffectiveDateFrom;
                entity.ValidUntil = model.EffectiveDateFrom.AddYears(10);
                entity.UpdatedBy = "system";
                var scheme = logisticDbContext.HarmonizeTariff.Where(x => x.HarmonizeTariffId == id).FirstOrDefault();
                if (scheme != null)
                {
                    scheme.BeaMasukPercentage = model.BM;
                    scheme.Schema = model.Scheme;
                    scheme.UpdatedBy = "system";
                }
            }
        
            return await logisticDbContext.SaveChangesAsync();
        }
        public async Task<int> Remove(int id)
        {
            var data = await logisticDbContext.HarmonizeTariff.Where(x => x.HarmonizeTariffId==id).FirstOrDefaultAsync();
            if (data != null)
            {
                logisticDbContext.Remove(data);
            }

            int rowsAffected = await logisticDbContext.SaveChangesAsync();
            return rowsAffected;
        }


    }
}
