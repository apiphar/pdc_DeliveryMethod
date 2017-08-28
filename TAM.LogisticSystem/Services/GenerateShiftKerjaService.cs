using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class GenerateShiftKerjaService
    {
        public GenerateShiftKerjaService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        public async Task<GenerateShiftKerjaPageViewModel> GetAll()
        {
            var data = new GenerateShiftKerjaPageViewModel();
            data.WorkHourTemplates = await GetWorkingDictionary();
            data.LocationCodes = await GetLocationCode();
            return data;
        }

        public async Task<List<WorkHourTemplateViewModel>> GetWorkingDictionary()
        {
            var data = await this.LogisticDbContext.WorkHourTemplate.AsNoTracking()
                .Select(Q => new WorkHourTemplateViewModel
                {
                    WorkHourTemplateCode = Q.WorkHourTemplateCode,
                    Description = Q.Description
                }).ToListAsync();

            return data;
        }

        public async Task<List<string>> GetLocationCode()
        {
            var data = await this.LogisticDbContext.Location.AsNoTracking()
                .Select(Q => Q.LocationCode)
                .ToListAsync();

            return data;
        }

        public async Task GenerateData(LocationWorkHourViewModel locationWorkHourViewModel)
        {
            var data = new List<LocationWorkHour>();
            var username = this.WebEnvironment.UserHumanName;
            var shiftList = await this.LogisticDbContext.WorkHourTemplateDetail
                .Where(Q => Q.WorkHourTemplateCode == locationWorkHourViewModel.WorkHourTemplateCode)
                .Select(Q => Q.ShiftCode)
                .ToListAsync();

            foreach (var shift in shiftList)
            {
                data.Add(new LocationWorkHour
                {
                    LocationCode = locationWorkHourViewModel.LocationCode,
                    ShiftCode = shift,
                    Start = locationWorkHourViewModel.ValidFrom,
                    Finish = locationWorkHourViewModel.ValidTo,
                    CreatedAt = DateTimeOffset.Now,
                    CreatedBy = username,
                    UpdatedAt = DateTimeOffset.Now,
                    UpdatedBy = username
                });
            }
            this.LogisticDbContext.LocationWorkHour.AddRange(data);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
