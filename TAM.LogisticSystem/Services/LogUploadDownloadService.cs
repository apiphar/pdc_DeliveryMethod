using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class LogUploadDownloadService
    {
        private readonly LogisticDbContext dbContext;
        public LogUploadDownloadService(LogisticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // TIE: START
        //public async Task<List<LogUploadDownload>> GetAllLogUploadDownload()
        //{
        //    var LogList = await dbContext.LogUploadDownload.OrderByDescending(Q => Q.LogUploadDownloadId).ToListAsync();
        //    return LogList.Select(Q => { Q.StartTime = Q.StartTime?.ToLocalTime(); Q.EndTime = Q.EndTime?.ToLocalTime(); return Q; }).ToList();
        //}

        //public async Task<LogUploadDownload> GetLastLog(string master)
        //{
        //    return await dbContext.LogUploadDownload.OrderByDescending(Q => Q.LogUploadDownloadId).FirstOrDefaultAsync();
        //}
        // TIE: END
    }
}
