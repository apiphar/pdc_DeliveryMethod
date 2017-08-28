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
    public class MasterPlafondService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public MasterPlafondService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        // TIE: START
        ////To get all data from table PlafondMaster
        //public async Task<List<MasterPlafondViewModel>> GetPlafondData()
        //{
        //    var plafonds = await this.LogisticDbContext.PlafondMaster
        //        .AsNoTracking()
        //        .Select(Q => new MasterPlafondViewModel
        //        {
        //            PlafondMasterId = Q.PlafondMasterId,
        //            KodeCompany = Q.CompanyCode,
        //            Plafond = Q.Plafond
        //        })
        //        .ToListAsync();
        //    return plafonds;
        //}

        ////untuk mendapatkan CompanyCode yang belum di insert ke table PlafondMaster
        //public async Task<List<CompanyCodeMasterPlafondViewModel>> GetCompanyKode()
        //{
        //    var companyCodes = await this.LogisticDbContext.Company.Select(Q => new CompanyCodeMasterPlafondViewModel { KodeCompany = Q.CompanyCode, Name = Q.Name }).ToListAsync();
        //    return companyCodes;
        //}

        ////untuk check apakah CompanyCode sudah ada atau belum
        //public async Task<bool> CheckCodeCompany(string code)
        //{
        //    var check = await LogisticDbContext.PlafondMaster.Where(x => x.CompanyCode == code).FirstOrDefaultAsync();
        //    if(check != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ////To insert one plafond data to table PlafondMaster
        //public async Task AddPlafondData(string kodeCompany, decimal plafond)
        //{
        //    var user = this.WebEnvironmentService.UserHumanName;
        //    var insert = new PlafondMaster
        //    {
        //        CompanyCode = kodeCompany,
        //        Plafond = plafond,
        //        Balance = plafond,
        //        CreatedBy = user,
        //        UpdatedBy = user,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow
        //    };
        //    this.LogisticDbContext.PlafondMaster.Add(insert);
        //    await this.LogisticDbContext.SaveChangesAsync();
        //}

        ////To Update one plafond data from table PlafondMaster
        //public async Task<int> UpdatePlafond(int id, decimal plafond)
        //{
        //    var user = this.WebEnvironmentService.UserHumanName;
        //    var existingPlafond = await LogisticDbContext.PlafondMaster.Where(x => x.PlafondMasterId == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;

        //    if (existingPlafond != null)
        //    {
        //        if (existingPlafond.Outstanding <= 0)
        //        {
        //            existingPlafond.Plafond = plafond;
        //            existingPlafond.Balance = plafond;
        //        }
        //        else
        //        {
        //            existingPlafond.Plafond = plafond;
        //            existingPlafond.Balance = existingPlafond.Balance + 
        //            (existingPlafond.Plafond - existingPlafond.Balance - existingPlafond.Outstanding);
        //        }
        //        existingPlafond.UpdatedBy = user;
        //        existingPlafond.UpdatedAt = DateTime.UtcNow;
        //        rowsAffected = await LogisticDbContext.SaveChangesAsync();
        //    }
        //    return rowsAffected;
        //}

        ////To Delete one plafond data from table PlafondMaster
        //public async Task<int> DeletePlafond(int id)
        //{
        //    var existingPlafond = await LogisticDbContext.PlafondMaster.Where(Q => Q.PlafondMasterId == id).FirstOrDefaultAsync();
        //    if (existingPlafond != null)
        //    {
        //        LogisticDbContext.Remove(existingPlafond);
        //    }

        //    int rowsAffected = await LogisticDbContext.SaveChangesAsync();

        //    return rowsAffected;
        //}
        // TIE: END
    }
}
