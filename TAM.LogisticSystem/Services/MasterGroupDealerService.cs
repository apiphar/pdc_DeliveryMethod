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
    public class MasterGroupDealerService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public MasterGroupDealerService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        public async Task<List<MasterGroupDealerViewModel>> GetAllTableData()
        {
            var tableData = await LogisticDbContext.DealerType.Select(Q => new MasterGroupDealerViewModel {
                KodeGroupDealer = Q.DealerTypeCode,
                GroupDealer = Q.Name                
            }).ToListAsync();
            return tableData;
        }        

        public async Task<DealerType> CheckCode(string code)
        {
            var checkCode = await LogisticDbContext.DealerType.FirstOrDefaultAsync(Q => Q.DealerTypeCode == code.ToUpper());
            return checkCode;
        }

        public async Task DeleteData(string id)
        {
            var deletedGroupDealer = await LogisticDbContext.DealerType.FirstOrDefaultAsync(Q => Q.DealerTypeCode == id);

            this.LogisticDbContext.DealerType.Remove(deletedGroupDealer);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task CreateData(MasterGroupDealerViewModel masterGroupDealerViewModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var newGroupDealer = new DealerType() {
                DealerTypeCode = masterGroupDealerViewModel.KodeGroupDealer.Trim(' ').ToUpper(),
                Name = masterGroupDealerViewModel.GroupDealer.ToUpper(),
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username   
            };

            this.LogisticDbContext.DealerType.Add(newGroupDealer);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task UpdateData(MasterGroupDealerViewModel masterGroupDealerViewModel)
        {
            var username = this.WebEnvironmentService.UserHumanName;
            var updateGroupDealer = await LogisticDbContext.DealerType.FirstOrDefaultAsync(Q => Q.DealerTypeCode == masterGroupDealerViewModel.KodeGroupDealer);
            updateGroupDealer.Name = masterGroupDealerViewModel.GroupDealer.ToUpper();
            updateGroupDealer.UpdatedAt = DateTimeOffset.UtcNow;
            updateGroupDealer.UpdatedBy = username;

            this.LogisticDbContext.DealerType.Update(updateGroupDealer);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
