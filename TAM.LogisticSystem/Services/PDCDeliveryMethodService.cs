using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class PDCDeliveryMethodService
    {
        public PDCDeliveryMethodService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        public async Task<List<PDCDeliveryViewModel>> GetAll()
        {
            var lists = new List<PDCDeliveryViewModel>();
            var DbConnection = LogisticDbContext.Database.GetDbConnection();

            _ = nameof(Branch.BranchCode);
            _ = nameof(Branch.Name);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(DeliveryMethod.DeliveryMethodCode);
            _ = nameof(DeliveryMethod.Name);

            var query = @"select l.LocationCode,
                            l.Name as LocationName,
                            concat (l.LocationCode, ' - ', l.Name) as LocationData,
                            b.BranchCode,
                            b.Name as BranchName,
                            concat(b.BranchCode, ' - ', b.Name) as BranchData,
                            dm.DeliveryMethodCode,
                            dm.Name as DeliveryMethodName,
                            concat(dm.DeliveryMethodCode,' - ',dm.Name) as DeliveryMethodData
                            from PreDeliveryCenterDelivery pdc
                            left join Location l on pdc.LocationCode = l.LocationCode
                            left join Branch b on pdc.BranchCode = b.BranchCode
                            left join DeliveryMethod dm on pdc.DeliveryMethodCode = dm.DeliveryMethodCode
                            where l.Name like '%PDC%'
            ";
            var result = await DbConnection.QueryAsync<PDCDeliveryViewModel>(query);

            lists = result.ToList();
            return lists;
        }

        public async Task<PreDeliveryCenterDelivery> Get(string locationCode, string branchCode)
        {
            return await LogisticDbContext.PreDeliveryCenterDelivery.FirstOrDefaultAsync(m => m.LocationCode == locationCode && m.BranchCode == branchCode);
        }

        public async Task<List<PDCBranchModel>> GetBranches()
        {
            var data = await this.LogisticDbContext.Branch.AsNoTracking()
                .Select(Q => new PDCBranchModel
                {
                    BranchCode = Q.BranchCode,
                    BranchName = Q.Name,
                    BranchData = Q.BranchCode + " - " + Q.Name
                }).ToListAsync();
            return data;
        }

        public async Task<List<PDCLocationModel>> GetLocations()
        {
            var data = await this.LogisticDbContext.Location.AsNoTracking()
                .Select(Q => new PDCLocationModel
                {
                    LocationCode = Q.LocationCode,
                    Name = Q.Name,
                    LocationData = Q.LocationCode + " - " + Q.Name
                }).Where(x => x.Name.StartsWith("PDC")).ToListAsync();
            return data;
        }

        public async Task<List<PDCDeliveryMethodModel>> GetDeliveries()
        {
            var data = await this.LogisticDbContext.DeliveryMethod.AsNoTracking()
                .Select(Q => new PDCDeliveryMethodModel
                {
                    DeliveryMethodCode = Q.DeliveryMethodCode,
                    Name = Q.Name,
                    DeliveryMethodData = Q.DeliveryMethodCode + " - " + Q.Name

                }).ToListAsync();
            return data;
        }

        internal async Task<int> Add(List<PDCDeliveryCreateUpdateViewModel> model)
        {
            var row = 0;
            await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await LogisticDbContext.Database.BeginTransactionAsync();
                {
                    var existing = await LogisticDbContext.PreDeliveryCenterDelivery.AsNoTracking().Where(x => x.LocationCode == model[0].LocationCode).ToListAsync();
                    LogisticDbContext.PreDeliveryCenterDelivery.RemoveRange(existing);
                    await LogisticDbContext.SaveChangesAsync();
                    var insertDetailList = new List<PreDeliveryCenterDelivery>();
                    foreach (var createPDC in model)
                    {
                        var insertDetail = new PreDeliveryCenterDelivery
                        {
                            BranchCode = createPDC.BranchCode,
                            DeliveryMethodCode = createPDC.DeliveryMethodCode,
                            LocationCode = createPDC.LocationCode,
                            CreatedBy = WebEnvironment.UserHumanName,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = WebEnvironment.UserHumanName,
                            UpdatedAt = DateTimeOffset.UtcNow
                        };
                        //LogisticDbContext.Add(insertDetail);
                        insertDetailList.Add(insertDetail);
                    }
                    LogisticDbContext.PreDeliveryCenterDelivery.AddRange(insertDetailList);

                    row = await LogisticDbContext.SaveChangesAsync();
                    trans.Commit();
                }
            });
            return row;
        }

        public async Task<bool> Delete(string locationCode, string branchCode)
        {
            var data = await this.LogisticDbContext.PreDeliveryCenterDelivery
                .FirstOrDefaultAsync(Q => Q.LocationCode == locationCode && Q.BranchCode == branchCode);
            if (data == null)
            {
                return false;
            }
            this.LogisticDbContext.PreDeliveryCenterDelivery.Remove(data);
            await this.LogisticDbContext.SaveChangesAsync();
            return true;
        }
    }
}
