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
    public class DealerMasterService
    {
        private readonly LogisticDbContext logisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;
        public DealerMasterService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.logisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }

        public async Task<DealerMasterPageViewModel> GetAll()
        {
            var data = new DealerMasterPageViewModel
            {
                ViewModels = await GetDealer(),
                DealerTypeCodes = await GetDealerTypeCode()
            };
            return data;
        }

        public async Task<List<DealerMasterViewModel>> GetDealer()
        {
            var data = await this.logisticDbContext.Dealer
                .Select(Q => new DealerMasterViewModel
                {
                    DealerCode = Q.DealerCode,
                    DealerName = Q.Name,
                    DealerAddress = Q.Address,
                    DealerTypeCode = this.logisticDbContext.DealerType.Where(X => X.DealerTypeCode == Q.DealerTypeCode)
                    .Select(Y => new DealerMasterTypeCode
                    {
                        DealerTypeCode = Y.DealerTypeCode,
                        DealerTypeName = Y.Name
                    }).FirstOrDefault()
                }).ToListAsync();

            return data;
        }

        public async Task<List<DealerMasterTypeCode>> GetDealerTypeCode()
        {
            var data = await this.logisticDbContext.DealerType
                .Select(Q => new DealerMasterTypeCode
                {
                    DealerTypeCode = Q.DealerTypeCode,
                    DealerTypeName = Q.Name
                })
                .ToListAsync();

            return data;
        }

        public async Task EditDealer(DealerMasterViewModel model)
        {
            var username = this.WebEnvironment.UserHumanName;
            var data = await this.logisticDbContext.Dealer
                .Where(Q => Q.DealerCode == model.DealerCode).FirstOrDefaultAsync();
            data.Name = model.DealerName.ToUpper();
            data.Address = model.DealerAddress.ToUpper();
            data.DealerTypeCode = model.DealerTypeCode.DealerTypeCode;
            data.UpdatedBy = username;
            data.UpdatedAt = DateTimeOffset.UtcNow;
            this.logisticDbContext.Dealer.Update(data);
            await this.logisticDbContext.SaveChangesAsync();
        }
    }
}
