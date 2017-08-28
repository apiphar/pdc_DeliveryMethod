using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Services
{
    // TIE: START
    // public class ExportHelperService :IExcelExportHelperService
    public class ExportHelperService
    // TIE: END
    {
        
        public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // TIE: START
        //public DataTable ListHeaderDataTable(List<InformationSchemaModel> data)
        //{
        //    DataTable dataTable = new DataTable();
        //    foreach (InformationSchemaModel item in data)
        //    {
        //        dataTable.Columns.Add(item.COLUMN_NAME);
        //    }
        //    object[] values = new object[data.Count];
        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        values[i] = "example" + (i + 1);
        //    }
        //    dataTable.Rows.Add(values);
        //    return dataTable;
        //}

        //public DataTable DynamicToDataTable(dynamic Data)
        //{
        //    DataTable dataTable = new DataTable();
        //    int Totalrow = 0;
        //    foreach (var item in Data[0])
        //    {
        //        if (item.Name != "messageError" && item.Name != "$$hashKey")
        //        {
        //            Type type = item.Value.Value != null ? (item.Value.Value).GetType() : null;
        //            if (type != null)
        //                dataTable.Columns.Add(item.Name, type);
        //            else
        //                dataTable.Columns.Add(item.Name);
        //        }
        //        Totalrow++;
        //    }
        //    object[] values = new object[dataTable.Columns.Count];
        //    foreach (var row in Data)
        //    {
        //        int i = 0;
        //        foreach (var cell in row)
        //        {
        //            if (cell.Name != "messageError" && cell.Name != "$$hashKey")
        //            {
        //                values[i] = cell.Value.Value;
        //                i++;
        //            }
        //        }
        //        dataTable.Rows.Add(values);
        //    }
        //    return dataTable;
        //}

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

        //public void GenerateWorkSheet(ExcelPackage package,DataTable dataTable,string WorksheetTitle, string heading = "", bool showSrNo = false)
        //{
        //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(WorksheetTitle);
        //    int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;
        //    if (showSrNo)
        //    {
        //        DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
        //        dataColumn.SetOrdinal(0);
        //        int index = 1;
        //        foreach (DataRow item in dataTable.Rows)
        //        {
        //            item[0] = index;
        //            index++;
        //        }
        //    }
        //    // add the content into the Excel file 
        //    worksheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

        //    // autofit width of cells with small content 
        //    int columnIndex = 1;
        //    foreach (DataColumn column in dataTable.Columns)
        //    {
        //        //ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
        //        int maxLength = column.MaxLength;
        //        /*columnCells.Max(cell => cell.Value.ToString().Count());*/
        //        if (maxLength < 150)
        //        {
        //            worksheet.Column(columnIndex).AutoFit();
        //        }
        //        columnIndex++;
        //    }

        //    // format header - bold, yellow on black 
        //    using (ExcelRange r = worksheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
        //    {
        //        r.Style.Font.Color.SetColor(Color.White);
        //        r.Style.Font.Bold = true;
        //        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        r.Style.Fill.BackgroundColor.SetColor(Color.Black);
        //    }

        //    // format cells - add borders 
        //    using (ExcelRange r = worksheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
        //    {
        //        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //        r.Style.Border.Top.Color.SetColor(Color.Black);
        //        r.Style.Border.Bottom.Color.SetColor(Color.Black);
        //        r.Style.Border.Left.Color.SetColor(Color.Black);
        //        r.Style.Border.Right.Color.SetColor(Color.Black);
        //    }

        //    if (!String.IsNullOrEmpty(heading))
        //    {
        //        worksheet.Cells["A1"].Value = heading;
        //        worksheet.Cells["A1"].Style.Font.Size = 20;

        //        worksheet.InsertColumn(1, 1);
        //        worksheet.InsertRow(1, 1);
        //        worksheet.Column(1).Width = 5;
        //    }
        //}
        //public byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false)
        //{
        //    byte[] result = null;
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        GenerateWorkSheet(package, dataTable, String.Format("{0} Data", heading), String.Format("{0} Data", heading));
        //        result = package.GetAsByteArray();
        //    }

        //    return result;
        //}
        //public byte[] ExportExcel(DataTable data,DataTable schemaDt,string title)
        //{
        //    byte[] result = null;
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        GenerateWorkSheet(package, data, String.Format("Master {0} Data",title), String.Format("Master {0} Data", title));
        //        GenerateWorkSheet(package, schemaDt, "INFORMATION SCHEMA","INFORMATION SCHEMA");

        //        result = package.GetAsByteArray();
        //    }

        //    return result;
        //}

        //public byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false)
        //{
        //    return ExportExcel(ListToDataTable<T>(data), Heading, showSlno);
        //}
        // TIE: END
    }

    public interface IExcelExportHelperService
    {
        /// <summary>
        /// Generate Template (Just header) from ColumnName of Information Schema to Datatable
        /// </summary>
        /// <returns></returns>
        DataTable ListHeaderDataTable(List<InformationSchemaModel> data);
        /// <summary>
        /// Dynamic to DataTable
        /// </summary>
        /// <returns></returns>
        DataTable DynamicToDataTable(dynamic Data);
        /// <summary>
        /// Automatically generate List to Datatable, just define your list of class e.g:List<Region> to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        DataTable ListToDataTable<T>(List<T> data);
        /// <summary>
        /// Export Excel with Information schema in worksheet page 2 (ColumnName,Nullable,datatype,max length)
        /// </summary>
        /// <returns></returns>
        byte[] ExportExcel(DataTable data,DataTable schemaDt, string title);
        /// <summary>
        /// Automatically Export excel from List<T> with Heading (string) , and Show numbering int the first column 
        /// </summary>
        /// <returns></returns>
        byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false);
        /// <summary>
        /// Automatically Export excel from Datatable with Heading (string) , and Show numbering int the first column 
        /// </summary>
        /// <returns></returns>
        byte[] ExportExcel(DataTable data, string Heading = "", bool showSlno = false);
    }
}
