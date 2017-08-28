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
    public class PlanningKalenderKerjaPolaKerjaSemingguService
    {
        public PlanningKalenderKerjaPolaKerjaSemingguService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironment)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvironment = webEnvironment;
        }
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironment;

        // Get all data
        public async Task<WorkHourPageViewModel> GetAll()
        {
            var data = new WorkHourPageViewModel
            {
                WorkHourTemplates = await GetWorkHourTemplates(),
                WorkHourTemplatesDetail = await GetWorkHourTemplatesDetail(),
                ShiftList = await GetShiftList()
            };

            return data;
        }

        // Get WorkHourTemplate
        public async Task<List<WorkHourTemplateViewModel>> GetWorkHourTemplates()
        {
            var data = await this.LogisticDbContext.WorkHourTemplate
                .Select(Q => new WorkHourTemplateViewModel
                {
                    WorkHourTemplateCode = Q.WorkHourTemplateCode,
                    Description = Q.Description
                }).ToListAsync();

            return data;
        }
        // Get WorkHourTemplateDetail
        public async Task<List<WorkHourTemplateDetailViewModel>> GetWorkHourTemplatesDetail()
        {
            var data = await this.LogisticDbContext.WorkHourTemplateDetail
                .Select(Q => new WorkHourTemplateDetailViewModel
                {
                    WorkHourTemplateCode = Q.WorkHourTemplateCode,
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

        // Insert AND Update WorkHour
        public async Task InsertUpdateWorkHour(WorkHourSendViewModel WorkHourSendViewModel)
        {
            var username = this.WebEnvironment.UserHumanName;
            await this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await this.LogisticDbContext.Database.BeginTransactionAsync())
                {
                    // Check Pola
                    var WorkHourTemplate = await this.LogisticDbContext.WorkHourTemplate
                        .FirstOrDefaultAsync(Q => Q.WorkHourTemplateCode == WorkHourSendViewModel.WorkHourTemplate.WorkHourTemplateCode);
                    // Tidak mendapat Pola di DB
                    if (WorkHourTemplate == null)
                    {
                        // Add Pola
                        var newWorkHourTemplate = new WorkHourTemplate
                        {
                            WorkHourTemplateCode = WorkHourSendViewModel.WorkHourTemplate.WorkHourTemplateCode.ToUpper(),
                            Description = WorkHourSendViewModel.WorkHourTemplate.Description.ToUpper(),
                            CreatedAt = DateTimeOffset.UtcNow,
                            CreatedBy = username,
                            UpdatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = username
                        };
                        this.LogisticDbContext.WorkHourTemplate.Add(newWorkHourTemplate);
                    }
                    else
                    {
                        // Update Pola
                        WorkHourTemplate.Description = WorkHourSendViewModel.WorkHourTemplate.Description;
                        WorkHourTemplate.UpdatedAt = DateTimeOffset.UtcNow;
                        WorkHourTemplate.UpdatedBy = username;
                        this.LogisticDbContext.WorkHourTemplate.Update(WorkHourTemplate);

                        // Clear WorkHourDetail
                        await DeleteWorkHourTemplateDetail(WorkHourTemplate.WorkHourTemplateCode);
                    }
                    await this.LogisticDbContext.SaveChangesAsync();

                    // Add new Pola Seminggu
                    var WorkHourTemplateDetailSend = new List<WorkHourTemplateDetail>();
                    foreach (var WorkHourTemplateDetail in WorkHourSendViewModel.WorkHourTemplatesDetail)
                    {
                        var tempWorkHourTemplateDetail = new WorkHourTemplateDetail
                        {
                            WorkHourTemplateCode = WorkHourTemplateDetail.WorkHourTemplateCode,
                            ShiftCode = WorkHourTemplateDetail.ShiftCode.ShiftCode,
                            TimeStart = WorkHourTemplateDetail.TimeStart,
                            TimeFinish = WorkHourTemplateDetail.TimeFinish,
                            IsMonday = WorkHourTemplateDetail.IsMonday,
                            IsTuesday = WorkHourTemplateDetail.IsTuesday,
                            IsWednesday = WorkHourTemplateDetail.IsWednesday,
                            IsThursday = WorkHourTemplateDetail.IsThursday,
                            IsFriday = WorkHourTemplateDetail.IsFriday,
                            IsSaturday = WorkHourTemplateDetail.IsSaturday,
                            IsSunday = WorkHourTemplateDetail.IsSunday,
                            CreatedAt = DateTimeOffset.UtcNow,
                            CreatedBy = username,
                            UpdatedAt = DateTimeOffset.UtcNow,
                            UpdatedBy = username
                        };
                        WorkHourTemplateDetailSend.Add(tempWorkHourTemplateDetail);
                    }
                    this.LogisticDbContext.AddRange(WorkHourTemplateDetailSend);
                    await this.LogisticDbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            });
        }
        // Delete WorkHour
        public async Task DeleteWorkHourTemplate(string WorkHourTemplateCode)
        {
            var data = await this.LogisticDbContext.WorkHourTemplate
               .Where(Q => Q.WorkHourTemplateCode == WorkHourTemplateCode)
               .FirstOrDefaultAsync();

            await this.LogisticDbContext.Database.CreateExecutionStrategy().Execute(async () =>
            {
                using (var transaction = await this.LogisticDbContext.Database.BeginTransactionAsync())
                {

                    await DeleteWorkHourTemplateDetail(WorkHourTemplateCode);

                    this.LogisticDbContext.WorkHourTemplate.Remove(data);
                    await this.LogisticDbContext.SaveChangesAsync();

                    transaction.Commit();
                }
            });
        }

        // Delete WorkHourDetail
        public async Task DeleteWorkHourTemplateDetail(string WorkhourTemplateCode)
        {
            // Find WorkHourDetail
            var WorkHourTemplateDetail = await this.LogisticDbContext.WorkHourTemplateDetail
                .Where(Q => Q.WorkHourTemplateCode == WorkhourTemplateCode)
                .ToListAsync();

            this.LogisticDbContext.WorkHourTemplateDetail.RemoveRange(WorkHourTemplateDetail);
            await this.LogisticDbContext.SaveChangesAsync();
        }
    }
}
