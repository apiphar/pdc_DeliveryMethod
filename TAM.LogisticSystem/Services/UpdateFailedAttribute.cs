using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Storage;
using Hangfire.Logging;
using Hangfire.Client;
using Hangfire.Server;
using TAM.LogisticSystem.Entities;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TAM.LogisticSystem.Services
{
    // TIE: START
    // public class UpdateFailedAttribute : JobFilterAttribute, IElectStateFilter
    public class UpdateFailedAttribute : JobFilterAttribute
    // TIE: END
    {
        // TIE: START
        // private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        // TIE: END
        private readonly LogisticDbContext dbcontext;
        private static int _jobId;
        public static int JobId { get { return _jobId; } set { _jobId = value; } }
        public UpdateFailedAttribute(LogisticDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        // TIE: START
        //public void OnStateElection(ElectStateContext context)
        //{
        //    _jobId = int.Parse(context.BackgroundJob.Id);
        //    var failedState = context.CandidateState as FailedState;
        //    if (failedState != null)
        //    {
        //        var log = dbcontext.LogUploadDownload.FirstOrDefault(Q => Q.JobId == _jobId);
        //        log.Status = (log.IsUploadProcess ? "Upload" : "Download") + " Gagal";
        //        log.EndTime = DateTime.UtcNow;
        //        log.UpdatedAt = DateTime.UtcNow;
        //        log.UpdatedBy = "SYSTEM";
        //        dbcontext.SaveChanges();
        //    }
        //}
        // TIE: END
    }
}

