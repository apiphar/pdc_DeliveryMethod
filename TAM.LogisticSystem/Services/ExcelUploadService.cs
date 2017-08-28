using Dapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Interfaces;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    // TIE: START
    // public class ExcelUploadService : IExcelUploadService
    public class ExcelUploadService
    // TIE: END
    {
        private readonly LogisticDbContext dbContext;
        public ExcelUploadService(LogisticDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // TIE: START
        //public DataTable toDataTable(ExcelPackage package)
        //{
        //    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        //    DataTable Dt = new DataTable();
        //    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //    {
        //        Dt.Columns.Add(firstRowCell.Text.Replace(" ",""));
        //    }
        //    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //    {
        //        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //        var newRow = Dt.NewRow();
        //        foreach (var cell in row)
        //        {
        //            newRow[cell.Start.Column - 1] = cell.Text;
        //        }
        //        Dt.Rows.Add(newRow);
        //    }
        //    return Dt;
        //}
        // TIE: END
    }
}
