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
    public class CityLegService
    {
        // Injector
        public CityLegService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        // Get all data
        public async Task<CityLegPageVIewModel> GetAll()
        {
            var data = new CityLegPageVIewModel
            {
                ViewModels = await GetViewModels(),
                CityList = await GetCityList()
            };
            return data;
        }

        // Get table data
        public async Task<List<CityLegViewModel>> GetViewModels()
        {
            var data = await this.LogisticDbContext.CityLeg
                .Select(Q => new CityLegViewModel
                {
                    CityLegCode = Q.CityLegCode,
                    CityLegName = Q.Name,
                    CityFrom = Q.CityFrom,
                    CityTo = Q.CityTo,
                    CalculatingSwappingCost = Q.CalculatingSwappingCost == true ? "Ya" : "Tidak"
                }).OrderBy(Q => Q.CityLegName)
                .ToListAsync();
            return data;
        }

        // Get dropdown data
        public async Task<List<CityListViewModel>> GetCityList()
        {
            var data = await this.LogisticDbContext.CityForLeg
                .Select(Q => new CityListViewModel
                {
                    CityForLegCode = Q.CityForLegCode,
                    CityName = Q.Name
                }).ToListAsync();
            return data;
        }

        // Send data
        public async Task<string> SendData(CityLegSendViewModel cityLegSendViewModel)
        {
            var data = await this.LogisticDbContext.CityLeg
                .Where(Q => Q.CityLegCode == cityLegSendViewModel.CityLegCode).FirstOrDefaultAsync();
            if (data != null)
            {
                return "DUPLICATE";
            }
            var username = this.WebEnvironment.UserHumanName;
            var cityLeg = new CityLeg
            {
                CityLegCode = cityLegSendViewModel.CityLegCode.ToUpper(),
                Name = cityLegSendViewModel.CityLegName.ToUpper(),
                CityFrom = cityLegSendViewModel.CityFrom,
                CityTo = cityLegSendViewModel.CityTo,
                CalculatingSwappingCost = cityLegSendViewModel.CalculatingSwappingCost,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = username,
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = username
            };
            this.LogisticDbContext.CityLeg.Add(cityLeg);
            await this.LogisticDbContext.SaveChangesAsync();
            return "SUKSES";
        }

        // Update data
        public async Task UpdateData(CityLegSendViewModel cityLegSendViewModel)
        {
            var username = this.WebEnvironment.UserHumanName;
            var data = await this.LogisticDbContext.CityLeg
                .Where(Q => Q.CityLegCode == cityLegSendViewModel.CityLegCode).FirstOrDefaultAsync();
            data.Name = cityLegSendViewModel.CityLegName.ToUpper();
            data.CityFrom = cityLegSendViewModel.CityFrom;
            data.CityTo = cityLegSendViewModel.CityTo;
            data.CalculatingSwappingCost = cityLegSendViewModel.CalculatingSwappingCost;
            data.UpdatedAt = DateTimeOffset.UtcNow;
            data.UpdatedBy = username;

            this.LogisticDbContext.CityLeg.Update(data);
            await this.LogisticDbContext.SaveChangesAsync();
        }

        // Delete data
        public async Task DeleteData(string cityLegCode)
        {
            var data = await this.LogisticDbContext.CityLeg
                .Where(Q => Q.CityLegCode == cityLegCode).FirstOrDefaultAsync();
            this.LogisticDbContext.CityLeg.Remove(data);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
