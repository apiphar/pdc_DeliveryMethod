using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Data;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class CBUFinalizePIBApiController : Controller
    {
        private readonly CBUFinalizePIBService CBUFinalizePIBService;
        private readonly ExcelUploadService ExcelUploadService;

        public CBUFinalizePIBApiController(CBUFinalizePIBService cbuFinalizePIBService, ExcelUploadService excelUploadService)
        {
            this.CBUFinalizePIBService = cbuFinalizePIBService;
            this.ExcelUploadService = excelUploadService;
        }
        // GET all import data
        [HttpGet]
        [Route("GetAllImportData")]
        public async Task<IActionResult> GetAllImportData()
        {
            var CBUFinalizePIBTableData = await this.CBUFinalizePIBService.GetAllData();
            return Ok(CBUFinalizePIBTableData);
        }

        //GET all data PIB yang ingin di finalize
        [HttpGet]
        [Route("GetAllPreFinalizeData/{noAju}")]
        public async Task<IActionResult> GetAllPreFinalizeData(string noAju)
        {
            var CBUFinalizePIBTableData = await this.CBUFinalizePIBService.GetAllPreFinalizeData(noAju);
            return Ok(CBUFinalizePIBTableData);
        }

        //GET currency
        [HttpGet]
        [Route("GetCurrencyData")]
        public async Task<IActionResult> GetCurrencyData()
        {
            var currencyData = await this.CBUFinalizePIBService.GetCurrency();
            return Ok(currencyData);
        }

        //GET Percentage Data
        [HttpGet]
        [Route("GetPercentageData")]
        public async Task<IActionResult> GetPercentageData()
        {
            var percentageData = await this.CBUFinalizePIBService.GetPercentage();
            return Ok(percentageData);
        }

        //Import Data from Excel
        [HttpPost]
        [Route("ImportFromExcel")]
        public IActionResult ImportFromExcel([FromForm]IFormFile file)
        {
            if (file.ContentType != "application/vnd.ms-excel" && file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest();
            }
            ExcelPackage package = new ExcelPackage(file.OpenReadStream());
            DataTable dt = ExcelUploadService.toDataTable(package);
            return Ok(dt);
        }

        //Finalize PIB
        [HttpPost]
        [Route("Finalize")]
        public async Task<IActionResult> Finalize([FromBody]FinalizePIBViewModel finalizePIBViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.CBUFinalizePIBService.FinalizePIB(finalizePIBViewModel);
            return Ok();
        }
    }
}
