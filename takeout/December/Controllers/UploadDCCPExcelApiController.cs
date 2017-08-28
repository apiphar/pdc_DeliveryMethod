using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Data;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class UploadDCCPExcelApiController : Controller
    {
        private readonly ExcelUploadService ExcelUploadService;
        private readonly UploadDCCPExcelService UploadDCCPExcelService;

        public UploadDCCPExcelApiController(UploadDCCPExcelService uploadDCCPExcelService, ExcelUploadService excelUploadService)
        {
            this.UploadDCCPExcelService = uploadDCCPExcelService;
            this.ExcelUploadService = excelUploadService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadDCCP([FromForm]IFormFile file)
        {
            if (file.ContentType != "application/vnd.ms-excel" && file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest();
            }
            ExcelPackage package = new ExcelPackage(file.OpenReadStream());
            var rows = await this.UploadDCCPExcelService.UploadDCCP(package);
            return Ok(rows);
        }
    }
}
