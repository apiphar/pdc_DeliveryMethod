using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class FormAService
    {
        private readonly LogisticDbContext logisticDbContext;

        public FormAService(LogisticDbContext db)
        {
            this.logisticDbContext = db;
        }

        // TIE: START
        //public List<FormAModel> GetAll()
        //{
        //    var dbconnection = logisticDbContext.Database.GetDbConnection();
        //    string query = @"select a.FrameNumber
        //                            , Convert(varchar(50), a.FormADate,105) as FormADate
        //                            , a.FormANumber
        //                    from ShipmentInvoiceDetail a";

        //    var result = dbconnection.Query<FormAModel>(query, new {}).ToList();
        //    return result;
        //}

        //public FormAModel GetFrameNumber(string frameNumber)
        //{
        //    var dbConnection = logisticDbContext.Database.GetDbConnection();
        //    {
        //        string query = @"select a.FrameNumber
        //                            , Convert(varchar(50), a.FormADate,105) as FormADate
        //                            , a.FormANumber
        //                    from ShipmentInvoiceDetail a
        //                    WHERE a.FrameNumber = @FrameNumber";

        //        var result = dbConnection.Query<FormAModel>(query, new { FrameNumber = frameNumber }).FirstOrDefault();
        //        return result;
        //    }
        //}

        //public async Task Update(string id, FormAModel model)
        //{
        //    var date = model.FormADate.ToLocalTime();
        //    var existingCluster = await logisticDbContext.ShipmentInvoiceDetail.Where(x => x.FrameNumber == id).FirstOrDefaultAsync();

        //    if (existingCluster != null)
        //    {
        //        existingCluster.FormADate = date;
        //        existingCluster.FormANumber = model.FormANumber;

        //        await logisticDbContext.SaveChangesAsync();
        //    }            
        //}
        // TIE: END
    }
}
