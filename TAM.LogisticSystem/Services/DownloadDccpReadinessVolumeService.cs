using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.ComponentModel;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;

namespace TAM.LogisticSystem.Services
{
    public class DownloadDccpReadinessVolumeService
    {
        public DownloadDccpReadinessVolumeService(LogisticDbContext logisticDbContext)
        {
            _TangoDbContext = logisticDbContext;
        }
        private readonly LogisticDbContext _TangoDbContext;
        public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        /// <summary>
        /// get data dccp readiness volume yg sesuai date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<DownloadDccpReadinessVolumeModel>> GetDbDccp(DateTime date)
        {
            
            var con = _TangoDbContext.Database.GetDbConnection();
            {
                var selected = (await con.QueryAsync<DownloadDccpReadinessVolumeModel>(@"
                    SELECT
                        dccp.DailyCarCarrierPlanId as [DccpId],
                        dccp.TransInOutDate as [TransInOutDate],
                        dccp.LocationFrom as [LocationFrom],
                        dccp.LocationTo as [LocationTo],
                        dccp.Trip as [Trip],
                        dccp.[Load] as [Load],
                        dccp.ShiftCode as [ShiftCode],
                        dccp.UnitReadyAdjusted as [Adjusted],
                        dccp.UnitReadyQuantity as [Quantity],
                        dccp.EstimatedUnit as [EstimatedUnit]
                    FROM DailyCarCarrierPlan dccp
                    WHERE CAST(dccp.TransInOutDate as date) = CAST(@date as date)
                ", new { date = date})).ToList();
                return selected;

            }
        }

        // TIE: START
        ///// <summary>
        ///// convert to data table
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public DataTable ListToDataTable<T>(List<T> data)
        //{
        //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        //    DataTable dataTable = new DataTable();

        //    for (int i = 0; i < properties.Count; i++)
        //    {
        //        PropertyDescriptor property = properties[i];
        //        dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
        //    }

        //    object[] values = new object[properties.Count];
        //    foreach (T item in data)
        //    {
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            values[i] = properties[i].GetValue(item);
        //        }

        //        dataTable.Rows.Add(values);
        //    }
        //    return dataTable;
        //}
        ///// <summary>
        ///// export ke byte file
        ///// </summary>
        ///// <param name="dataTable"></param>
        ///// <param name="heading"></param>
        ///// <param name="showSrNo"></param>
        ///// <returns></returns>
        //public byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false)
        //{
        //    byte[] result = null;
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
        //        int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

        //        if (showSrNo)
        //        {
        //            DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
        //            dataColumn.SetOrdinal(0);
        //            int index = 1;
        //            foreach (DataRow item in dataTable.Rows)
        //            {
        //                item[0] = index;
        //                index++;
        //            }
        //        }


        //        // add the content into the Excel file 
        //        workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

        //        // autofit width of cells with small content 
        //        int columnIndex = 1;
        //        foreach (DataColumn column in dataTable.Columns)
        //        {
        //            //ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
        //            int maxLength = column.MaxLength;
        //            /*columnCells.Max(cell => cell.Value.ToString().Count());*/
        //            if (maxLength < 150)
        //            {
        //                workSheet.Column(columnIndex).AutoFit();
        //            }
        //            columnIndex++;
        //        }

        //        // format header - bold, yellow on black 
        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
        //        {
        //            r.Style.Font.Color.SetColor(Color.White);
        //            r.Style.Font.Bold = true;
        //            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            r.Style.Fill.BackgroundColor.SetColor(Color.Black);
        //        }

        //        // format cells - add borders 
        //        using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
        //        {
        //            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //            r.Style.Border.Top.Color.SetColor(Color.Black);
        //            r.Style.Border.Bottom.Color.SetColor(Color.Black);
        //            r.Style.Border.Left.Color.SetColor(Color.Black);
        //            r.Style.Border.Right.Color.SetColor(Color.Black);
        //        }

        //        if (!String.IsNullOrEmpty(heading))
        //        {
        //            workSheet.Cells["A1"].Value = heading;
        //            workSheet.Cells["A1"].Style.Font.Size = 20;

        //            workSheet.InsertColumn(1, 1);
        //            workSheet.InsertRow(1, 1);
        //            workSheet.Column(1).Width = 5;
        //        }

        //        result = package.GetAsByteArray();
        //    }

        //    return result;
        //}
        ///// <summary>
        ///// terima data list dan sekaligus export ke byte file
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <param name="Heading"></param>
        ///// <param name="showSlno"></param>
        ///// <returns></returns>
        //public byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false)
        //{
        //    return ExportExcel(ListToDataTable<T>(data), Heading, showSlno);
        //}
        ///// <summary>
        ///// get guid
        ///// </summary>
        ///// <returns></returns>
        //public string GetGuid()
        //{
        //    string handle = Guid.NewGuid().ToString();
        //    return handle;
        //}
        // TIE: END
    }
}
