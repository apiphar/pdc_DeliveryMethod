using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class ReportGesekanApiController : Controller
    {
        private readonly ReportGesekanService ReportGesekanService;
        private readonly IExcelExportHelperService ExcelService;

        public ReportGesekanApiController(ReportGesekanService ReportGesekanService,IExcelExportHelperService ExcelService)
        {
            this.ReportGesekanService = ReportGesekanService;
            this.ExcelService = ExcelService;
        }
        /// <summary>
        /// Get All Report Gesekan
        /// </summary>
        /// <returns></returns>
        [Route("ReportGesekan")]
        [HttpPost]
        public async Task<IActionResult> ReportGesekan([FromBody]ScratchReportGesekan model)
        {
            var Data = await ReportGesekanService.GetAllReportGesekan(model);
            return Ok(Data);
        }
        /// <summary>
        /// Render Excel file and return GUID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Download")]
        [HttpPost]
        public IActionResult Download([FromBody] List<SerahTerimaGesekanViewModel> model)
        {
            string guid = Guid.NewGuid().ToString();
            byte[] a = ExcelService.ExportExcel(model, "Report Gesekan");
            TempData[guid] = a;
            return Ok(guid);
        }

        /// <summary>
        /// return File Download by GUID
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        [Route("Download/{fileGuid}")]
        [HttpGet]
        public virtual IActionResult Download(string fileGuid)
        {
            if (TempData[fileGuid] == null)
            {
                return new EmptyResult();

            }
            byte[] data = TempData[fileGuid] as byte[];
            string filename = String.Format("ReportGesekan_{0:ddMMyyyy_HHmmss}.xlsx", DateTime.Now);
            return File(data, "application/vnd.ms-excel", filename);
        }
    }
}
