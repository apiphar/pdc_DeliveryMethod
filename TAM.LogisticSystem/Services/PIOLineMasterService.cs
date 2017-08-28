using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class PIOLineMasterService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvService;

        public PIOLineMasterService(LogisticDbContext logisticDbContext,WebEnvironmentService webEnvService)
        {
            this.LogisticDbContext = logisticDbContext;
            this.WebEnvService = webEnvService;
        }

        // TIE: START
        //internal async Task<int> Create(PIOLineMasterModel data)
        //{
        //    int row = 0;
        //    var username = WebEnvService.UserHumanName;
        //    await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        //    {
        //        var trans = await LogisticDbContext.Database.BeginTransactionAsync();
        //        {
        //            PIOLineDictionary pio = new PIOLineDictionary
        //            {
        //                PIOLineDictionaryId = data.PIOLineDictionaryId,
        //                LineNumber = data.LineNumber,
        //                LocationCode = data.Location.LocationCode,
        //                TaktSeconds = data.TaktSeconds,
        //                Post = data.Post,
        //                CreatedBy = username,
        //                UpdatedBy = username,
        //                CreatedAt = DateTime.Now.ToUniversalTime(),
        //                UpdatedAt = DateTime.Now.ToUniversalTime()
        //            };
        //            LogisticDbContext.Add(pio);
        //            row = await LogisticDbContext.SaveChangesAsync();
        //            trans.Commit();
        //        }
        //    });

        //    return row ;
        //} 

        //public async Task<PIOLineMasterViewModel> GetData()
        //{
        //    var pio = await LogisticDbContext.PIOLineDictionary.ToListAsync();
        //    List<PIOLineMasterModel> PioModelList = new List<PIOLineMasterModel>();
        //    pio.ForEach(Q =>
        //    {

        //        PIOLineMasterModel PioModel = new PIOLineMasterModel()
        //        {
        //            PIOLineDictionaryId = Q.PIOLineDictionaryId,
        //            Location = LogisticDbContext.Location.FirstOrDefault(i => i.LocationCode == Q.LocationCode),
        //            LineNumber = Q.LineNumber,
        //            TaktSeconds = Q.TaktSeconds,
        //            Post = Q.Post
        //        };
        //        PioModelList.Add(PioModel);
        //    });
        //    var location = await LogisticDbContext.Location.ToListAsync();
        //    PIOLineMasterViewModel PioViewModel = new PIOLineMasterViewModel()
        //    {
        //        PioLineMaster = PioModelList.OrderBy(Q=>Q.Location.LocationCode).ThenBy(Q=>Q.LineNumber).ToList(),
        //        Location = location
        //    };
        //    return PioViewModel ;
        //}

        //internal async Task<int> Remove(int id)
        //{
        //    var existingRegion = await LogisticDbContext.PIOLineDictionary.Where(x => x.PIOLineDictionaryId == id).FirstOrDefaultAsync();
        //    if (existingRegion != null)
        //    {
        //        LogisticDbContext.Remove(existingRegion);
        //    }

        //    return await LogisticDbContext.SaveChangesAsync();
        //}

        //internal async Task<int> Update(int id, PIOLineMasterModel model)
        //{
        //    var existingPIO = await LogisticDbContext.PIOLineDictionary.Where(x => x.PIOLineDictionaryId == id).FirstOrDefaultAsync();
        //    int rowsAffected = 0;
        //    var username = WebEnvService.UserHumanName;
        //    if (existingPIO != null)
        //    {
        //        await LogisticDbContext.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        //        {
        //            var trans = await LogisticDbContext.Database.BeginTransactionAsync();
        //            {
        //                existingPIO.LocationCode = model.Location.LocationCode;
        //                existingPIO.LineNumber = model.LineNumber ;
        //                existingPIO.Post = model.Post;
        //                existingPIO.TaktSeconds = model.TaktSeconds;
        //                existingPIO.UpdatedBy = username;
        //                existingPIO.UpdatedAt = DateTime.Now.ToUniversalTime();
        //                rowsAffected = await LogisticDbContext.SaveChangesAsync();
        //                trans.Commit();
        //            }
        //        });
        //    }
        //    return rowsAffected;
        //}
        // TIE: END
    }
}
