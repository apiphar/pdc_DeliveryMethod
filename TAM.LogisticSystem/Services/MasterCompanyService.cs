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
    public class MasterCompanyService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public MasterCompanyService(LogisticDbContext tangoDBcontext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = tangoDBcontext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// retrieve all data from company table
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterCompanyViewModel>> GetCompanies()
        {
            _ = nameof(Company.CompanyCode);
            _ = nameof(Company.DealerCode);
            _ = nameof(Dealer.Name);
            _ = nameof(Company.Name);
            _ = nameof(Company.NPWPAddress);
            _ = nameof(Company.SAPCode);
            _ = nameof(Company.Phone);
            _ = nameof(Company.Fax);
            _ = nameof(Company.Email);
            _ = nameof(Company.TradeName);
            _ = nameof(Company.NPWP);
            _ = nameof(Company.IsDealerFinancing);
            _ = nameof(Company.TermOfPaymentDay);
            var companiesData = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<MasterCompanyViewModel>(@"
                select c.CompanyCode, c.DealerCode, d.Name as DealerName, c.Name as CompanyName, c.NPWPAddress, c.SAPCode, c.Phone, c.Fax, c.Email, c.TradeName, c.NPWP, c.IsDealerFinancing, c.TermOfPaymentDay
                from Company c
                join Dealer d on d.DealerCode = c.DealerCode
            ")).ToList();
            return companiesData;
        }

        public async Task<List<MasterCompanyDealerComboBoxModel>> GetDealers()
        {
            var dealersData = await this.LogisticDbContext.Dealer.AsNoTracking().Select(x => new MasterCompanyDealerComboBoxModel
            {
                DealerCode = x.DealerCode,
                Name = x.Name
            }).ToListAsync();
            return dealersData;
        }

        /// <summary>
        /// update data based on form data
        /// </summary>
        /// <param name="masterCompanyInsertUpdateModel"></param>
        /// <returns></returns>
        public async Task Update(MasterCompanyInsertUpdateModel masterCompanyInsertUpdateModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var masterCompanyUpdateData = await this.LogisticDbContext.Company.AsNoTracking().FirstOrDefaultAsync(x => x.CompanyCode == masterCompanyInsertUpdateModel.CompanyCode);
            masterCompanyUpdateData.Phone = masterCompanyInsertUpdateModel.Phone;
            masterCompanyUpdateData.DealerCode = masterCompanyInsertUpdateModel.DealerCode.ToUpper();
            masterCompanyUpdateData.Fax = masterCompanyInsertUpdateModel.Fax;
            masterCompanyUpdateData.Name = masterCompanyInsertUpdateModel.CompanyName.ToUpper();
            masterCompanyUpdateData.Email = masterCompanyInsertUpdateModel.Email.ToUpper();
            masterCompanyUpdateData.NPWPAddress = masterCompanyInsertUpdateModel.NPWPAddress.ToUpper();
            masterCompanyUpdateData.SAPCode = masterCompanyInsertUpdateModel.SAPCode.ToUpper();
            masterCompanyUpdateData.TradeName = masterCompanyInsertUpdateModel.TradeName.ToUpper();
            masterCompanyUpdateData.NPWP = masterCompanyInsertUpdateModel.NPWP;
            masterCompanyUpdateData.IsDealerFinancing = masterCompanyInsertUpdateModel.IsDealerFinancing;
            masterCompanyUpdateData.TermOfPaymentDay = masterCompanyInsertUpdateModel.TermOfPaymentDay;
            masterCompanyUpdateData.UpdatedBy = username;
            masterCompanyUpdateData.UpdatedAt = DateTimeOffset.UtcNow;
            this.LogisticDbContext.Update(masterCompanyUpdateData);
            await LogisticDbContext.SaveChangesAsync();
        }
    }
}
