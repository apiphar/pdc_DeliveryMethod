using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using Dapper;

namespace TAM.LogisticSystem.Services
{
    public class MasterKalenderLiburKerjaService
    {
        private readonly LogisticDbContext LogisticDbContext;

        public MasterKalenderLiburKerjaService(LogisticDbContext logisticDbContext)
        {
            this.LogisticDbContext = logisticDbContext;
        }


        public async Task<List<Holiday>> GetData()
        {
            return await LogisticDbContext.Holiday.ToListAsync();
        }

        // TIE: START
        ////simpen date kalender ke DB
        //public async Task<int> SaveData(string locationCode, List<DateTime> selectedDates)
        //{
        //    foreach (var date in selectedDates)
        //    {
        //        var holiday = await LogisticDbContext.Holiday.SingleOrDefaultAsync(h => h.HolidayId == date);

        //        if (holiday == null)
        //        {
        //            holiday = new Holiday
        //            {
        //                HolidayId = date,
        //                LocationCode = locationCode
        //            };
        //            LogisticDbContext.Add(holiday);
        //        }
        //    }

        //    var year = selectedDates.First().Date.Year;
        //    var holidays = await LogisticDbContext.Holiday.Where(h => h.HolidayId.Date.Year == year).ToListAsync();

        //    foreach (var hld in holidays.Where(h => !selectedDates.Contains(h.HolidayId)))
        //    {
        //        LogisticDbContext.Holiday.Remove(hld);
        //    }

        //    return await LogisticDbContext.SaveChangesAsync();
        //}


        ////buang yang date lama yg ga kepake
        //public async Task<int> Remove(DateTime id)
        //{
        //    var existingHoliday = await LogisticDbContext.Holiday.Where(x => x.HolidayId == id).SingleOrDefaultAsync();

        //    if (existingHoliday != null)
        //    {
        //        LogisticDbContext.Remove(existingHoliday);
        //    }

        //    int rowsAffected = await LogisticDbContext.SaveChangesAsync();

        //    return rowsAffected;
        //}
        // TIE: END

        //ambil list location
        public async Task<List<string>> PopulateLocation()
        {
            var locations = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<string>(
                @"select LocationCode from Location"
                )).ToList();

            return locations;
        }
    }
}

