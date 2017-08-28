using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using System.IO;
using System.Net.Http;
using System.Net;
using OfficeOpenXml;
using System.Data;
using System.Drawing;
using OfficeOpenXml.Style;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class FormARequestApiController : Controller
    {
        private readonly FormARequestService FormARequestService;

        public FormARequestApiController(FormARequestService formARequestService)
        {
            this.FormARequestService = formARequestService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> InvoicesList()
        {
            var invoices = await this.FormARequestService.GetInvoicesList();
            return Ok(invoices);
        }

        // TIE: START
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> UpdatePIB([FromBody]List<FormARequestInvoicesViewModel> updateInvoicesList)
        //{
        //    var dataTable = await this.FormARequestService.UpdatePIB(updateInvoicesList);

        //    string handle = Guid.NewGuid().ToString();

        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        string heading = "";
        //        bool showSrNo = false;
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
        //            r.Style.Font.Bold = true;
        //            r.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            r.Style.Fill.BackgroundColor.SetColor(Color.White);
        //            r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;

        //            r.Style.Border.Bottom.Color.SetColor(Color.Black);
        //            r.Style.Border.Top.Color.SetColor(Color.Black);
        //        }

        //        // Set headers of col 1,2,5,6 to background green
        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, 2])
        //        {
        //            r.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            r.Style.Fill.BackgroundColor.SetColor(Color.LimeGreen);
        //            r.Style.Font.Bold = true;
        //        }

        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 5, startRowFrom, 6])
        //        {
        //            r.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            r.Style.Fill.BackgroundColor.SetColor(Color.LimeGreen);
        //            r.Style.Font.Bold = true;
        //        }

        //        // format cells - add left and right borders 
        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
        //        {
        //            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //            r.Style.Border.Left.Color.SetColor(Color.Black);
        //            r.Style.Border.Right.Color.SetColor(Color.Black);

        //            r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }

        //        // add border-bottom for bottom rows
        //        using (ExcelRange r = workSheet.Cells[dataTable.Rows.Count + startRowFrom, startRowFrom, dataTable.Rows.Count + startRowFrom, dataTable.Columns.Count])
        //        {
        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Bottom.Color.SetColor(Color.Black);
        //        }

        //        if (!String.IsNullOrEmpty(heading))
        //        {
        //            workSheet.Cells["A1"].Value = heading;
        //            workSheet.Cells["A1"].Style.Font.Size = 20;

        //            workSheet.InsertColumn(1, 1);
        //            workSheet.InsertRow(1, 1);
        //            workSheet.Column(1).Width = 5;
        //        }

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            package.SaveAs(ms);
        //            ms.Position = 0;
        //            TempData[handle] = ms.ToArray();
        //        }
        //    }

        //    var formModel = new FormARequestFormModel
        //    {
        //        FileName = "FormARequest",
        //        GUID = handle
        //    };

        //    return Ok(formModel);
        //}

        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult Download(string guid, string fileName)
        //{
        //    if (TempData[guid] != null)
        //    {
        //        byte[] data = TempData[guid] as byte[];
        //        return File(data, "application/vnd.ms-excel", "FormARequest.xlsx");
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        // TIE: END
    }
}
