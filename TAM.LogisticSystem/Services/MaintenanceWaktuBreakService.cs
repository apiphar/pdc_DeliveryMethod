using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class MaintenanceWaktuBreakService
    {
        private readonly LogisticDbContext logisticDbContext;

        public MaintenanceWaktuBreakService(LogisticDbContext tangoDBContext)
        {
            this.logisticDbContext = tangoDBContext;
        }

        // TIE: START
        //public List<MaintenanceWaktuBreakViewModel> GetMaintenanceWaktuBreak()
        //{
        //    List<MaintenanceWaktuBreakViewModel> lists = new List<MaintenanceWaktuBreakViewModel>();
        //    var DbConnection = logisticDbContext.Database.GetDbConnection();

        //    string query = @"SELECT DataSource.IdleTimeCustomId
        //                      ,DataSource.LocationCode
        //                      ,DataSource.ShiftCode
        //                      ,DataSource.Name
        //                      ,DataSource.[Description]
        //                      ,DATEADD(MILLISECOND,DATEDIFF(MILLISECOND,getutcdate(),GETDATE()),DataSource.DateFrom) as DateFrom
        //                      ,REPLACE(CONVERT(VARCHAR, DateFrom, 106),' ','-') as ConvertDateFrom
        //                      ,DATEADD(MILLISECOND,DATEDIFF(MILLISECOND,getutcdate(),GETDATE()),DataSource.DateTo) as DateTo
        //                      ,REPLACE(CONVERT(VARCHAR, DateTo, 106),' ','-') as ConvertDateTo
        //                    FROM (SELECT a.IdleTimeCustomId,
        //                                a.LocationCode,
        //                                a.ShiftCode,
        //                             b.Name,
        //                             c.[Description],
        //                             a.DateFrom,			
        //                             a.DateTo
        //                      FROM IdleTimeCustom as a
        //                            LEFT JOIN Location as b on a.LocationCode = b.LocationCode
        //                            LEFT JOIN [Shift] as c on a.ShiftCode= c.ShiftCode) as DataSource";

        //    var data = DbConnection.Query<MaintenanceWaktuBreakViewModel>(query);
        //    lists = data.ToList();

        //    return lists;
        //}

        //public List<MaintenanceWaktuBreakViewModel> GetLocation()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select LocationCode, Name
        //                                from Location";

        //        var result = dbconnection.Query<MaintenanceWaktuBreakViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public List<MaintenanceWaktuBreakViewModel> GetShift()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select ShiftCode, Description
        //                                from Shift";

        //        var result = dbconnection.Query<MaintenanceWaktuBreakViewModel>(query, new
        //        {
        //        }).ToList();

        //        return result;
        //    }
        //}

        //public async Task<int> Add(MaintenanceWaktuBreakViewModel model)
        //{
        //    var viewIrisan = await this.logisticDbContext.LocationBreakHour.Where(Q => Q.LocationCode == model.LocationCode && Q.ShiftCode == model.ShiftCode).ToListAsync();
        //    if (IrisanJam(TimeZoneInfo.ConvertTimeToUtc(model.DateFrom), TimeZoneInfo.ConvertTimeToUtc(model.DateTo), viewIrisan) > 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        logisticDbContext.Add(new LocationBreakHour
        //        {
        //            LocationCode = model.LocationCode,
        //            ShiftCode = model.ShiftCode,
        //            DateFrom = TimeZoneInfo.ConvertTimeToUtc(model.DateFrom),
        //            DateTo = TimeZoneInfo.ConvertTimeToUtc(model.DateTo)
        //        });
        //        return await logisticDbContext.SaveChangesAsync();
        //    }             
        //}

        //public async Task<int> Update(int id, MaintenanceWaktuBreakViewModel model)
        //{
        //    var viewIrisan = await this.logisticDbContext.LocationBreakHour.Where(Q => Q.LocationCode == model.LocationCode && Q.ShiftCode == model.ShiftCode).ToListAsync();
        //    if (IrisanJam(TimeZoneInfo.ConvertTimeToUtc(model.DateFrom), TimeZoneInfo.ConvertTimeToUtc(model.DateTo), viewIrisan) > 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        var entity = await logisticDbContext.LocationBreakHour.Where(x => x.IdleTimeCustomId == id).FirstOrDefaultAsync();
        //        int rowsAffected = 0;

        //        if (entity != null)
        //        {
        //            entity.LocationCode = model.LocationCode;
        //            entity.ShiftCode = model.ShiftCode;
        //            entity.DateFrom = TimeZoneInfo.ConvertTimeToUtc(model.DateFrom);
        //            entity.DateTo = TimeZoneInfo.ConvertTimeToUtc(model.DateTo);

        //            rowsAffected = await logisticDbContext.SaveChangesAsync();
        //        }
        //        return rowsAffected;
        //    }         
        //}

        //public async Task<int> Remove(int id)
        //{
        //    var entity = await logisticDbContext.LocationBreakHour.Where(x => x.LocationBreakHourId == id).FirstOrDefaultAsync();
        //    if (entity != null)
        //    {
        //        logisticDbContext.Remove(entity);
        //    }

        //    int rowsAffected = await logisticDbContext.SaveChangesAsync();
        //    return rowsAffected;
        //}

        //public int IrisanJam(DateTime jamMulai, DateTime jamSelesai, List<LocationBreakHour> listData)
        //{
        //    var flagintersection = 0;
        //    foreach (var jam in listData)
        //    {
        //        if (jam.DateFrom <= jamMulai && jamMulai >= jamSelesai)
        //        {
        //            flagintersection++;
        //        }
        //        else if (jam.DateFrom > jamMulai && jam.DateFrom < jamSelesai)
        //        {
        //            flagintersection++;
        //        }
        //        else if (jam.DateTo > jamMulai && jam.DateTo < jamSelesai)
        //        {
        //            flagintersection++;
        //        }
        //    }
        //    return flagintersection;
        //}
        // TIE: END
    }
}
