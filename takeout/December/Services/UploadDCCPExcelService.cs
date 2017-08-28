using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    public class UploadDCCPExcelService
    {
        private readonly ExcelUploadService ExcelUploadService;
        private readonly LogisticDbContext LogisticDbContext;

        public UploadDCCPExcelService(ExcelUploadService excelUploadService, LogisticDbContext logisticDbContext)
        {
            this.ExcelUploadService = excelUploadService;
            this.LogisticDbContext = logisticDbContext;
        }

        public async Task<int> UploadDCCP(ExcelPackage package)
        {
            var dt = this.ExcelUploadService.toDataTable(package);
            var rows = dt.Rows.Cast<DataRow>().ToList();

            foreach (var row in rows)
            {
                var excelDate = row["TransInOutDate"].ToString();
                //var doubleDate = double.Parse(excelDate);
                var date = DateTime.Parse(excelDate);
                var newDCCP = new DailyCarCarrierPlan
                {
                    TransInOutDate = date,
                    
                    Trip = Convert.ToInt32(row["Trip"]),
                    Load = Convert.ToInt32(row["Load"]),
                    ShiftCode = row["ShiftCode"].ToString(),
                    UnitReadyQuantity = Convert.ToInt32(row["UnitReadyQuantity"]),
                    EstimatedUnit = Convert.ToInt32(row["EstimatedUnit"]),
                    LocationFrom = row["LocationFrom"].ToString(),
                    LocationTo = row["LocationTo"].ToString()
                };

                LogisticDbContext.DailyCarCarrierPlan.Add(newDCCP);
            }
            
            await LogisticDbContext.SaveChangesAsync();

            return rows.Count();
        }
    }
}
