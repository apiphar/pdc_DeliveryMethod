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
    public class CompanyMasterService
    {
        private readonly LogisticDbContext logisticDbContext;

        public CompanyMasterService(LogisticDbContext logisticDbContext)
        {
            this.logisticDbContext = logisticDbContext;
        }

        public List<CompanyMasterViewModel> GetCompany()
        {
            
            var DbConnection = logisticDbContext.Database.GetDbConnection();

            string query = @"SELECT a.CompanyCode,
 a.DealerCode, 
 a.Name ,
 a.IsDealerFinancing ,
 a.NPWPAddress,
 a.NPWP,
 a.email,
 a.TradeName,
 a.Phone,
 a.Fax,
 a.SAPCode,
 a.CreatedAt,
 a.CreatedBy,
 a.UpdatedAt,
 a.UpdatedBy                      
        FROM Company as a
        left join Dealer as b on a.DealerCode = b.DealerCode";

            var result = DbConnection.Query<CompanyMasterViewModel>(query, new
            {
                
            }).ToList();

            return result;
        }

        public List<Dealer> Get()
        {
            var data = logisticDbContext.Dealer.ToList();
            return data;
        }

        public List<CompanyMasterViewModel> GetDealerCode()
        {
            var dbconnection = logisticDbContext.Database.GetDbConnection();
            {
                string query = @"select a.DealerCode
                                        from Dealer as a";

                var result = dbconnection.Query<CompanyMasterViewModel>(query, new
                {
                }).ToList();

                return result;
            }
        }

        public async Task<int> Add(CompanyMasterViewModel model)
        {
            var entity = new Company();
            {
                entity.NPWP = model.NPWP;
                entity.DealerCode = model.DealerCode;
                entity.CompanyCode = model.CompanyCode;
                entity.Name = model.Name;
                entity.NPWPAddress = model.NPWPAddress;
                entity.Phone = model.Phone;
                entity.Fax = model.Fax;
                entity.SAPCode = model.SAPCode;
                entity.Email = model.Email;
                entity.TradeName = model.TradeName;
                entity.IsDealerFinancing = model.IsDealerFinancing;
                entity.CreatedAt = model.CreatedAt;
                entity.CreatedBy = model.CreatedBy = "Kodok1";
                entity.UpdatedAt = model.UpdatedAt;
                entity.UpdatedBy = model.UpdatedBy = "Kodok2";

            };

            logisticDbContext.Add(entity);
            return await logisticDbContext.SaveChangesAsync();
        }

        public async Task<int> Update(string id, CompanyMasterViewModel model)
        {
            var existingCompany = await logisticDbContext.Company.Where(x => x.CompanyCode == id).FirstOrDefaultAsync();
            int rowsAffected = 0;

            if (existingCompany != null)
            {
                existingCompany.NPWP = model.NPWP;
                existingCompany.NPWPAddress = model.NPWPAddress;
                existingCompany.Phone = model.Phone;
                existingCompany.Email = model.Email;
                existingCompany.Fax = model.Fax;
                existingCompany.SAPCode = model.SAPCode;
                existingCompany.DealerCode = model.DealerCode;
                existingCompany.Name = model.Name;
                existingCompany.TradeName = model.TradeName;
                existingCompany.IsDealerFinancing = model.IsDealerFinancing;
               
                rowsAffected = await logisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }

        public async Task<int> Remove(string id)
        {
            var existingCompany = await logisticDbContext.Company.Where(x => x.CompanyCode == id).FirstOrDefaultAsync();
            if (existingCompany != null)
            {
                logisticDbContext.Remove(existingCompany);
            }

            return await logisticDbContext.SaveChangesAsync();
        }

    }
}
