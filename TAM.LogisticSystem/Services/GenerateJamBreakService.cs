using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class GenerateJamBreakService
    {
        public GenerateJamBreakService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        // Get all data
        public async Task<GenerateJamBreakPageViewModel> GetAll()
        {
            var data = new GenerateJamBreakPageViewModel
            {
                BreakHourTemplates = await GetBreakHourTemplate(),
                Location = await GetLocationCodeList()
            };
            return data;
        }

        // GET all BreakHourTemplate
        public async Task<List<BreakHourTemplateViewModel>> GetBreakHourTemplate()
        {
            var data = await this.LogisticDbContext.BreakHourTemplate
                .Select(Q => new BreakHourTemplateViewModel
                {
                    BreakHourTemplateCode = Q.BreakHourTemplateCode,
                    Description = Q.Description
                }).ToListAsync();

            return data;
        }

        // GET locationCode from Location
        public async Task<List<GenerateHourLocationViewModel>> GetLocationCodeList()
        {
            var data = await this.LogisticDbContext.Location
                .Select(Q => new GenerateHourLocationViewModel
                {
                    LocationCode = Q.LocationCode,
                    LocationName = Q.Name
                }).ToListAsync();

            return data;
        }

        // Check for duplicate data
        public async Task<LocationBreakHour> CheckDuplicate(LocationBreakHourSendViewModel locationBreakHour)
        {
            var data = new LocationBreakHour();
            for (var date = locationBreakHour.ValidFrom; locationBreakHour.ValidTo.CompareTo(date) >= 0; date = date.AddDays(1.0))
            {
                data = await this.LogisticDbContext.LocationBreakHour
                    .Where(Q => Q.Start.Date == date.Date && Q.LocationCode == locationBreakHour.Location.LocationCode)
                    .FirstOrDefaultAsync();
                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }

        // POST data to LocationBreakHour
        public async Task GenerateData(LocationBreakHourSendViewModel locationBreakHour)
        {
            var data = new List<LocationBreakHour>();
            var username = this.WebEnvironment.UserHumanName;
            var breakHourTemplateDetail = await this.LogisticDbContext.BreakHourTemplateDetail
                .Where(Q => Q.BreakHourTemplateCode == locationBreakHour.BreakHourTemplateCode)
                .ToListAsync();

            await this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await this.LogisticDbContext.Database.BeginTransactionAsync())
                {
                    for (var date = locationBreakHour.ValidFrom; locationBreakHour.ValidTo.CompareTo(date) >= 0; date = date.AddDays(1.0))
                    {
                        var duplicate = await this.LogisticDbContext.LocationBreakHour
                            .Where(Q => Q.Start.Date == date.Date && Q.LocationCode == locationBreakHour.Location.LocationCode)
                            .ToListAsync();
                        if (duplicate != null)
                        {
                            this.LogisticDbContext.LocationBreakHour.RemoveRange(duplicate);
                            await this.LogisticDbContext.SaveChangesAsync();
                        }

                        foreach (var breakHour in breakHourTemplateDetail)
                        {
                            var dayList = GetCheckedDate(breakHour);
                            foreach (var day in dayList)
                            {
                                if (date.DayOfWeek.ToString() == day)
                                {
                                    var toSend = new LocationBreakHour
                                    {
                                        LocationCode = locationBreakHour.Location.LocationCode,
                                        ShiftCode = breakHour.ShiftCode,
                                        Start = date.AddHours(breakHour.TimeStart.Hours),
                                        Finish = date.AddHours(breakHour.TimeFinish.Hours),
                                        CreatedAt = DateTimeOffset.UtcNow,
                                        CreatedBy = username,
                                        UpdatedAt = DateTimeOffset.UtcNow,
                                        UpdatedBy = username
                                    };
                                    data.Add(toSend);
                                }
                            }
                        }
                    }
                    this.LogisticDbContext.LocationBreakHour.AddRange(data);
                    await this.LogisticDbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            });
        }

        // Get Checked Date
        public List<string> GetCheckedDate(BreakHourTemplateDetail breakHour)
        {
            var dayList = new List<string>();
            if(breakHour.IsMonday == true)
            {
                dayList.Add("Monday");
            }
            if (breakHour.IsTuesday == true)
            {
                dayList.Add("Tuesday");
            }
            if (breakHour.IsWednesday == true)
            {
                dayList.Add("Wednesday");
            }
            if (breakHour.IsThursday == true)
            {
                dayList.Add("Thursday");
            }
            if (breakHour.IsFriday == true)
            {
                dayList.Add("Friday");
            }
            if (breakHour.IsSaturday == true)
            {
                dayList.Add("Saturday");
            }
            if (breakHour.IsSunday == true)
            {
                dayList.Add("Sunday");
            }
            return dayList;
        }

        public async Task DeleteLocationBreakHour(string locationCode)
        {
            var data = await this.LogisticDbContext.LocationBreakHour
                .Where(Q => Q.LocationCode == locationCode)
                .ToListAsync();
            this.LogisticDbContext.RemoveRange(data);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
