using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;

namespace TAM.LogisticSystem.Services
{
    public class HolidayService
    {
        private readonly LogisticDbContext LogisticDbContext;
        private readonly WebEnvironmentService WebEnvironmentService;
        public HolidayService(LogisticDbContext logisticDbContext, WebEnvironmentService webEnvironmentService)
        {
            this.WebEnvironmentService = webEnvironmentService;
            this.LogisticDbContext = logisticDbContext;
        }

        /// <summary>
        /// untuk ambil data dari DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<HolidayViewModel>> GetData()
        {
            _ = nameof(Holiday.HolidayDate);
            _ = nameof(Location.LocationCode);
            _ = nameof(Location.Name);
            var kalender = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<HolidayViewModel>(
            @"select dateadd(dd,1,h.holidayDate) as HolidayDate, h.LocationCode, l.Name
            from Holiday h
            join Location l on l.LocationCode=h.LocationCode
            order by h.CreatedAt

            ")).ToList();
            return kalender;
        }
        /// <summary>
        /// save data ke DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task SaveAddData(List<HolidayViewModel> added)
        {
            var userName = this.WebEnvironmentService.UserHumanName;
            foreach (var x in added)
            {
                var kalender = new Holiday
                {
                    HolidayDate = x.HolidayDate.ToUniversalTime(),
                    LocationCode = x.LocationCode,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = userName

                };
                LogisticDbContext.Holiday.Add(kalender);
            }
            await LogisticDbContext.SaveChangesAsync();
        }

        public async Task SaveDelData(List<HolidayViewModel> deleted)
        {
            foreach (var x in deleted)
            {

                var test = await this.LogisticDbContext.Holiday.FirstOrDefaultAsync();
                var kalender = await LogisticDbContext.Holiday.FirstOrDefaultAsync(Q => Q.HolidayDate == x.HolidayDate && Q.LocationCode == x.LocationCode);

                LogisticDbContext.Holiday.Remove(kalender);
            }

            await LogisticDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// dapetin data lokasi
        /// </summary>
        /// <returns></returns>
        public async Task<List<HolidayViewModel>> PopulateLocation()
        {
            //var dataHoliday1 = (await LogisticDbContext.Database.GetDbConnection().QueryAsync<HolidayViewModel>(
            //    @"select locationCode as LocationCode, name as Name
            //        from Location"
            //    )).ToList();
            var dataHoliday = await this.LogisticDbContext.Location.Select(Q => new HolidayViewModel
            {
                LocationCode = Q.LocationCode,
                Name = Q.Name
            }).ToListAsync();
            return dataHoliday;
        }
    }
}
