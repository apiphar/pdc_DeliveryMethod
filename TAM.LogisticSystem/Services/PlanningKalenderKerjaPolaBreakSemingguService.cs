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
    public class PlanningKalenderKerjaPolaBreakSemingguService
    {
        public PlanningKalenderKerjaPolaBreakSemingguService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;
        
        // Get all data
        public async Task<BreakHourPageViewModel> GetAll()
        {
            var data = new BreakHourPageViewModel
            {
                BreakHourTemplates = await GetBreakHourTemplates(),
                BreakHourTemplatesDetail = await GetBreakHourTemplatesDetail(),
                ShiftList = await GetShiftList()
            };

            return data;
        }

        // Get BreakHourTemplate
        public async Task<List<BreakHourTemplateViewModel>> GetBreakHourTemplates()
        {
            var data = await this.LogisticDbContext.BreakHourTemplate
                .Select(Q => new BreakHourTemplateViewModel
                {
                    BreakHourTemplateCode = Q.BreakHourTemplateCode,
                    Description = Q.Description
                }).ToListAsync();

            return data;
        }
        // Get BreakHourTemplateDetail
        public async Task<List<BreakHourTemplateDetailViewModel>> GetBreakHourTemplatesDetail()
        {
            var data = await this.LogisticDbContext.BreakHourTemplateDetail
                .Select(Q => new BreakHourTemplateDetailViewModel
                {
                    BreakHourTemplateCode = Q.BreakHourTemplateCode,
                    ShiftCode = LogisticDbContext.Shift.Select(X => new ShiftViewModel
                    {
                        ShiftCode = X.ShiftCode,
                        Description = X.Description
                    }).FirstOrDefault(Y => Y.ShiftCode == Q.ShiftCode),
                    TimeStart = Q.TimeStart,
                    TimeFinish = Q.TimeFinish,
                    IsMonday = Q.IsMonday,
                    IsTuesday = Q.IsTuesday,
                    IsWednesday = Q.IsWednesday,
                    IsThursday = Q.IsThursday,
                    IsFriday = Q.IsFriday,
                    IsSaturday = Q.IsSaturday,
                    IsSunday = Q.IsSunday
                }).ToListAsync();

            return data;
        }
        // Get Shift List
        public async Task<List<ShiftViewModel>> GetShiftList()
        {
            var data = await this.LogisticDbContext.Shift
                .Select(Q => new ShiftViewModel
                {
                    ShiftCode = Q.ShiftCode,
                    Description = Q.Description
                }).ToListAsync();

            return data;
        }

        // Insert AND Update BreakHour
        public async Task InsertUpdateBreakHour(BreakHourSendViewModel breakHourSendViewModel)
        {
            var username = this.WebEnvironment.UserHumanName;
            await this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await this.LogisticDbContext.Database.BeginTransactionAsync())
                {
                    // Check Pola
                    var breakHourTemplate = await this.LogisticDbContext.BreakHourTemplate
                        .FirstOrDefaultAsync(Q => Q.BreakHourTemplateCode == breakHourSendViewModel.BreakHourTemplate.BreakHourTemplateCode);
                    // Tidak mendapat Pola di DB
                    if (breakHourTemplate == null)
                    {
                        // Add Pola
                        var newBreakHourTemplate = new BreakHourTemplate
                        {
                            BreakHourTemplateCode = breakHourSendViewModel.BreakHourTemplate.BreakHourTemplateCode.ToUpper(),
                            Description = breakHourSendViewModel.BreakHourTemplate.Description.ToUpper(),
                            CreatedAt = DateTimeOffset.UtcNow,
                            CreatedBy = username,
                            UpdatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = username
                        };
                        this.LogisticDbContext.BreakHourTemplate.Add(newBreakHourTemplate);
                    }
                    else
                    {
                        // Update Pola
                        breakHourTemplate.Description = breakHourSendViewModel.BreakHourTemplate.Description;
                        breakHourTemplate.UpdatedAt = DateTimeOffset.UtcNow;
                        breakHourTemplate.UpdatedBy = username;
                        this.LogisticDbContext.BreakHourTemplate.Update(breakHourTemplate);

                        // Clear BreakHourDetail
                        await DeleteBreakHourTemplateDetail(breakHourTemplate.BreakHourTemplateCode);
                    }
                    await this.LogisticDbContext.SaveChangesAsync();

                    // Add new Pola Seminggu
                    var breakHourTemplateDetailSend = new List<BreakHourTemplateDetail>();
                    foreach (var breakHourTemplateDetail in breakHourSendViewModel.BreakHourTemplatesDetail)
                    {
                        var tempBreakHourTemplateDetail = new BreakHourTemplateDetail
                        {
                            BreakHourTemplateCode = breakHourTemplateDetail.BreakHourTemplateCode,
                            ShiftCode = breakHourTemplateDetail.ShiftCode.ShiftCode,
                            TimeStart = breakHourTemplateDetail.TimeStart,
                            TimeFinish = breakHourTemplateDetail.TimeFinish,
                            IsMonday = breakHourTemplateDetail.IsMonday,
                            IsTuesday = breakHourTemplateDetail.IsTuesday,
                            IsWednesday = breakHourTemplateDetail.IsWednesday,
                            IsThursday = breakHourTemplateDetail.IsThursday,
                            IsFriday = breakHourTemplateDetail.IsFriday,
                            IsSaturday = breakHourTemplateDetail.IsSaturday,
                            IsSunday = breakHourTemplateDetail.IsSunday,
                            CreatedAt = DateTimeOffset.UtcNow,
                            CreatedBy = username,
                            UpdatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = username
                        };
                        breakHourTemplateDetailSend.Add(tempBreakHourTemplateDetail);
                    }
                    this.LogisticDbContext.AddRange(breakHourTemplateDetailSend);
                    await this.LogisticDbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            });
        }
        // Delete BreakHour
        public async Task DeleteBreakHourTemplate(string breakHourTemplateCode)
        {
            var data = await this.LogisticDbContext.BreakHourTemplate
               .Where(Q => Q.BreakHourTemplateCode == breakHourTemplateCode)
               .FirstOrDefaultAsync();

            await this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await this.LogisticDbContext.Database.BeginTransactionAsync())
                {

                    await DeleteBreakHourTemplateDetail(breakHourTemplateCode);

                    this.LogisticDbContext.BreakHourTemplate.Remove(data);
                    await this.LogisticDbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            });
        }

        // Delete BreakHourDetail
        public async Task DeleteBreakHourTemplateDetail(string breakhourTemplateCode)
        {
            // Find BreakHourDetail
            var breakHourTemplateDetail = await this.LogisticDbContext.BreakHourTemplateDetail
                .Where(Q => Q.BreakHourTemplateCode == breakhourTemplateCode)
                .ToListAsync();

            this.LogisticDbContext.BreakHourTemplateDetail.RemoveRange(breakHourTemplateDetail);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }

}
