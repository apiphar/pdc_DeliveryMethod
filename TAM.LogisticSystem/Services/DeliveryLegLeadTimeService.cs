using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class DeliveryLegLeadTimeService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;

        public DeliveryLegLeadTimeService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironmentService = webEnvironmentService;
        }

        /// <summary>
        /// Get all deliveryLeadTime data
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeliveryLegLeadTimeViewModel>> GetDeliveryLeadData(string code)
        {
            _ = nameof(DeliveryLeadTime.DeliveryLeadTimeId);
            _ = nameof(DeliveryLeadTime.DeliveryLegCode);
            _ = nameof(DeliveryLeadTime.DeliveryMethodCode);
            _ = nameof(DeliveryLeadTime.LeadMinutes);
            _ = nameof(DeliveryMethod.ParentDeliveryMethodCode);
            var data = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<DeliveryLegLeadTimeViewModel>(@"
SELECT dt.DeliveryLeadTimeId, dt.DeliveryLegCode, dt.DeliveryMethodCode, dt.LeadMinutes,
dm.ParentDeliveryMethodCode
FROM DeliveryLeadTime dt JOIN DeliveryMethod dm 
ON dt.DeliveryMethodCode = dm.DeliveryMethodCode
WHERE DeliveryLegCode = @deliveryLegCode
", new { deliveryLegCode = code })).ToList();
            return data;
        }


        /// <summary>
        /// to get all delivery method code
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetDeliveryMethodViewModel>> GetDeliveryMethodCode()
        {
            var deliveryLeadData = await this.LogisticDbContext.DeliveryMethod
                .AsNoTracking()
                .Select(Q => new GetDeliveryMethodViewModel
                {
                    DeliveryMethodCode = Q.DeliveryMethodCode,
                    DeliveryMethodName = Q.Name,
                    ParentDeliveryMethodCode = Q.ParentDeliveryMethodCode
                }).ToListAsync();
            return deliveryLeadData;
        }


        /// <summary>
        /// to insert data to table DeliveryLeadTime
        /// </summary>
        /// <returns></returns>
        public async Task AddDeliveryLeadData(string deliveryLegCode, string deliveryMethodCode, int leadMinutes)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var insert = new DeliveryLeadTime
            {
                DeliveryLegCode = deliveryLegCode.ToUpper(),
                DeliveryMethodCode = deliveryMethodCode.ToUpper(),
                LeadMinutes = leadMinutes,
                CreatedBy = user,
                UpdatedBy = user,
                UpdatedAt = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow
            };
            this.LogisticDbContext.DeliveryLeadTime.Add(insert);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Validasi DeliveryMethodCode apakah sudah ada
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Validate(string deliveryLegCode, string code)
        {
            var dwellingData = await this.LogisticDbContext.DeliveryLeadTime
                .FirstOrDefaultAsync(Q => Q.DeliveryLegCode == deliveryLegCode && Q.DeliveryMethodCode == code);
            if (dwellingData != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To Update one delivery lead data from table DeliveryLeadTime
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateDeliveryLead(int id, string deliveryMethodCode, int leadMinutes)
        {
            var user = this.WebEnvironmentService.UserHumanName;
            var existingDeliveryLead = await LogisticDbContext.DeliveryLeadTime.Where(Q => Q.DeliveryLeadTimeId == id).FirstOrDefaultAsync();
            var rowsAffected = 0;

            if (existingDeliveryLead != null)
            {
                existingDeliveryLead.DeliveryMethodCode = deliveryMethodCode;
                existingDeliveryLead.LeadMinutes = leadMinutes;
                existingDeliveryLead.UpdatedAt = DateTimeOffset.UtcNow;
                existingDeliveryLead.UpdatedBy = user;
                rowsAffected = await LogisticDbContext.SaveChangesAsync();
            }
            return rowsAffected;
        }

        /// <summary>
        /// to delete one delivery lead data from table DeliveryLeadTime
        /// </summary>
        /// <returns></returns>
        public async Task<int> DeleteDeliveryLead(int id)
        {
            var existingDeliveryLead = await LogisticDbContext.DeliveryLeadTime.Where(Q => Q.DeliveryLeadTimeId == id).FirstOrDefaultAsync();
            if (existingDeliveryLead != null)
            {
                LogisticDbContext.Remove(existingDeliveryLead);
            }

            var rowsAffected = await LogisticDbContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Get Location data untuk combobox
        /// </summary>
        /// <returns></returns>
        public async Task<GetLocationLeadTimeViewModel> GetLocation(string code)
        {
            _ = nameof(DeliveryLeg.Name);
            _ = nameof(DeliveryLeg.DeliveryLegCode);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            _ = nameof(DeliveryLeg.LocationFrom);
            _ = nameof(DeliveryLeg.LocationTo);

            var location = (await this.LogisticDbContext.Database.GetDbConnection().QueryAsync<GetLocationLeadTimeViewModel>(@"
SELECT dl.[Name] AS DeliveryLegName, dl.DeliveryLegCode, l.LocationCode AS NameFrom, ll.LocationCode AS NameTo, 
ll.LocationCode, l.Name AS NameLocationFrom, ll.Name AS NameLocationTo, 
dl.LocationFrom, dl.LocationTo, dl.DeliveryLegCode
FROM DeliveryLeg dl 
JOIN [Location] l ON dl.LocationFrom = l.LocationCode
JOIN [Location] ll ON dl.LocationTo = ll.LocationCode
WHERE DeliveryLegCode = @deliveryLegCode
", new { deliveryLegCode = code })).FirstOrDefault();
            return location;
        }
    }
}
