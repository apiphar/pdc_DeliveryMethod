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
    public class ReportGesekanService
    {
        private readonly LogisticDbContext logisticDbContext;

        public ReportGesekanService(LogisticDbContext logisticDbContext)
        {
            this.logisticDbContext = logisticDbContext;
        }

        public async Task<List<SerahTerimaGesekanViewModel>> GetAllReportGesekan(ScratchReportGesekan Data)
        {
            var con = logisticDbContext.Database.GetDbConnection();
            {
                var query = @"SELECT a.ScratchId 
                                    ,b.FrameNumber,
	                                [TanggalGesek] = a.ScratchedAt,
	                                [JumlahGesek] = f.NumberOfScratch,
	                                [Lokasi] = h.Name,
	                                b.Katashiki,
	                                b.Suffix,
	                                [ModelName] = e.Name,
	                                [Color] = g.IndonesianName,
	                                [TanggalSerahTerima] =i.Date,
	                                [NoSurat] = a.ScratchHandOverNumber
                                FROM Scratch a
                                JOIN Vehicle b ON a.VehicleId = b.VehicleId
                                JOIN CarType c ON c.Katashiki = b.Katashiki AND c.Suffix = b.Suffix
                                JOIN CarSeries d ON c.CarSeriesCode = d.CarSeriesCode
                                JOIN CarModel e ON e.CarModelCode = d.CarModelCode
                                JOIN ScratchConfiguration f ON f.CarModelCode = d.CarModelCode
                                JOIN ExteriorColor g ON g.ExteriorColorCode = b.ExteriorColorCode
                                JOIN [Location] h ON h.LocationCode = a.LocationCode
                                LEFT JOIN ScratchHandOver i ON i.ScratchHandOverNumber = a.ScratchHandOverNumber";
                IEnumerable<SerahTerimaGesekanViewModel> result = new List<SerahTerimaGesekanViewModel>();
                //if frameNumber is not null
                if (!string.IsNullOrEmpty(Data.FrameNumber))
                {
                    query += " WHERE FrameNumber = @frameNumber";
                    result = await con.QueryAsync<SerahTerimaGesekanViewModel>(query,new {frameNumber = Data.FrameNumber });
                    return result.ToList();
                }
                
                result = await con.QueryAsync<SerahTerimaGesekanViewModel>(query);
                //if date from and to not null
                if(Data.TanggalFrom!=null && Data.TanggalTo!=null)
                {
                    //result = //result.Where(Q => Q.TanggalSerahTerima >= Data.TanggalFrom && Q.TanggalSerahTerima <= Data.TanggalTo);
                }
                return result.ToList();
            }
        }


    }
}
