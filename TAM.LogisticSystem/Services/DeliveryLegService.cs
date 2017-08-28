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
    public class DeliveryLegService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        public DeliveryLegService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }

        public async Task<DeliveryLegPageVIewModel> GetAll()
        {
            var data = new DeliveryLegPageVIewModel
            {
                ViewModels = await GetDeliveryLeg(),
                DeliveryLegLocations = await GetLocation(),
                CityLegCodes = await GetCityLegCode()
            };
            return data;
        }

        public async Task<List<DeliveryLegViewModel>> GetDeliveryLeg()
        {
            var data = await this.LogisticDbContext.DeliveryLeg.AsNoTracking()
                .Select(Q => new DeliveryLegViewModel
                {
                    DeliveryLegCode = Q.DeliveryLegCode,
                    Name = Q.Name,
                    LocationFrom = Q.LocationFrom,
                    LocationTo = Q.LocationTo,
                    CityLegCode = Q.CityLegCode,
                    BufferMinutes = Q.BufferMinutes,
                    NeedSJKB = Q.NeedSJKB == true ? "Ya" : "Tidak"
                }).ToListAsync();
            return data;
        }

        public async Task<List<DeliveryLegLocationViewModel>> GetLocation()
        {
            var data = await this.LogisticDbContext.Location.Select(Q => new DeliveryLegLocationViewModel
            {
                LocationCode = Q.LocationCode,
                Name = Q.Name
            }).ToListAsync();

            return data;
        }

        public async Task<List<DeliveryLegCityListViewModel>> GetCityLegCode()
        {
            var data = await this.LogisticDbContext.CityLeg.Select(Q => new DeliveryLegCityListViewModel
            {
                CityLegCode = Q.CityLegCode,
                CityLegName = Q.Name
            }).ToListAsync();
            return data;
        }

        /// <summary>
        /// Create or Update to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> SendData(DeliveryLegCreateOrUpdateRequest model)
        {
            var data = await this.LogisticDbContext.DeliveryLeg.Where(Q => Q.DeliveryLegCode == model.DeliveryLegCode).FirstOrDefaultAsync();
            if (data != null)
            {
                return "DUPLICATE";
            }
            var username = this.WebEnvironment.UserHumanName;
            var deliveryLeg = new DeliveryLeg
            {
                DeliveryLegCode = model.DeliveryLegCode.ToUpper(),
                Name = model.Name.ToUpper(),
                LocationFrom = model.LocationFrom,
                LocationTo = model.LocationTo,
                CityLegCode = model.CityLegCode,
                BufferMinutes = model.BufferMinutes,
                NeedSJKB = model.NeedSJKB,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username
            };
            this.LogisticDbContext.Add(deliveryLeg);
            await this.LogisticDbContext.SaveChangesAsync();
            return "SUKSES";
        }

        public async Task UpdateData(DeliveryLegCreateOrUpdateRequest model)
        {
            var username = this.WebEnvironment.UserHumanName;
            var deliveryLeg = await this.LogisticDbContext.DeliveryLeg
                .FirstOrDefaultAsync(Q => Q.DeliveryLegCode == model.DeliveryLegCode);

            deliveryLeg.DeliveryLegCode = model.DeliveryLegCode.ToUpper();
            deliveryLeg.Name = model.Name.ToUpper();
            deliveryLeg.LocationFrom = model.LocationFrom;
            deliveryLeg.LocationTo = model.LocationTo;
            deliveryLeg.CityLegCode = model.CityLegCode;
            deliveryLeg.BufferMinutes = model.BufferMinutes;
            deliveryLeg.NeedSJKB = model.NeedSJKB;
            deliveryLeg.UpdatedAt = DateTimeOffset.UtcNow;
            deliveryLeg.UpdatedBy = username;
            this.LogisticDbContext.Update(deliveryLeg);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        public async Task Remove(string id)
        {
            var data = await this.LogisticDbContext.DeliveryLeg.Where(x => x.DeliveryLegCode == id).FirstOrDefaultAsync();
            if (data != null)
            {
                this.LogisticDbContext.Remove(data);
            }
            await LogisticDbContext.SaveChangesAsync();
        }
    }
}
