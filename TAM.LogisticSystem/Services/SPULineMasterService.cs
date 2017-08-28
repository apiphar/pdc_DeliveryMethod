using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;

namespace TAM.LogisticSystem.Services
{
    public class SPULineMasterService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public SPULineMasterService(LogisticDbContext LogisticDbContext,WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = LogisticDbContext;
            this.WebEnvService = webEnvService;
        }

        // TIE: START
        internal async Task<int> Create(SPULineMasterModel data)
        {
            var row = 0;
            var username = WebEnvService.UserHumanName;
            await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var trans = await LogisticDbContext.Database.BeginTransactionAsync();
                {
                    var SPULine = new SPULine
                    {
                        SPULineId = data.SPULineId,
                        LineNumber = data.LineNumber.ToUpper(),
                        LocationCode = data.Location.LocationCode,
                        TaktSeconds = data.TaktSeconds,
                        Post = data.Post,
                        CreatedBy = username,
                        UpdatedBy = username,
                        CreatedAt = DateTime.Now.ToUniversalTime(),
                        UpdatedAt = DateTime.Now.ToUniversalTime()
                    };
                    LogisticDbContext.Add(SPULine);
                    row = await LogisticDbContext.SaveChangesAsync();
                    trans.Commit();
                }
            });

            return row ;
        }

        public async Task<SPULineMasterViewModel> GetData()
        {
            var SPU = await LogisticDbContext.SPULine.ToListAsync();
            var SPUModelList = new List<SPULineMasterModel>();
            SPU.ForEach(Q =>
            {

                var SPUModel = new SPULineMasterModel()
                {
                    SPULineId = Q.SPULineId,
                    Location = LogisticDbContext.Location.FirstOrDefault(i => i.LocationCode == Q.LocationCode),
                    LineNumber = Q.LineNumber.ToUpper(),
                    TaktSeconds = Q.TaktSeconds,
                    Post = Q.Post
                };
                SPUModelList.Add(SPUModel);
            });
            var location = await LogisticDbContext.Location.ToListAsync();
            var SPUViewModel = new SPULineMasterViewModel()
            {
                SpuLineMaster = SPUModelList.OrderBy(Q => Q.Location.LocationCode).ThenBy(Q => Q.LineNumber).ToList(),
                Location = location
            };
            return SPUViewModel;
        }


        internal async Task<int> Remove(int id)
        {
            var existingRegion = await LogisticDbContext.SPULine.Where(x => x.SPULineId == id).FirstOrDefaultAsync();
            if (existingRegion != null)
            {
                LogisticDbContext.Remove(existingRegion);
            }

            return await LogisticDbContext.SaveChangesAsync();
        }

        internal async Task<int> Update(int id, SPULineMasterModel model)
        {
            var existingSPU = await LogisticDbContext.SPULine.Where(x => x.SPULineId == id).FirstOrDefaultAsync();
            var rowsAffected = 0;
            var username = WebEnvService.UserHumanName;
            if (existingSPU != null)
            {
                await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
                {
                    var trans = await LogisticDbContext.Database.BeginTransactionAsync();
                    {
                        existingSPU.LocationCode = model.Location.LocationCode;
                        existingSPU.LineNumber = model.LineNumber;
                        existingSPU.Post = model.Post;
                        existingSPU.TaktSeconds = model.TaktSeconds;
                        existingSPU.UpdatedBy = username;
                        existingSPU.UpdatedAt = DateTime.Now.ToUniversalTime();
                        rowsAffected = await LogisticDbContext.SaveChangesAsync();
                        trans.Commit();
                    }
                });
            }
            return rowsAffected;
        }
        // TIE: END
    }
}
