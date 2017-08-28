using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class SerahTerimaGesekanApiController : Controller
    {
        private readonly SerahTerimaGesekanService SerahTerimaGesekanService;

        public SerahTerimaGesekanApiController(SerahTerimaGesekanService SerahTerimaGesekanService)
        {
            this.SerahTerimaGesekanService = SerahTerimaGesekanService;
        }

        /// <summary>
        /// Get All Serah Terima Gesekan
        /// </summary>
        /// <returns></returns>
        [HttpGet("SerahTerimaGesekan")]
        public async Task<IActionResult> SerahTerimaGesekan()
        {
            var Data = await SerahTerimaGesekanService.GetAllSerahTerimaGesekan();
            return Ok(Data);
        }
        /// <summary>
        /// Generate Excel And Insert ScratchHandOver and Update Scratch (HandOverNumber Column)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Generate")]
        public async Task<IActionResult> Generate([FromBody] SerahTerimaGesekanInputViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            else
            {
                var checkedResult = await SerahTerimaGesekanService.InsertAndUpdateScratchHandOver(model);
                if (checkedResult == false)
                {
                    return BadRequest("No. Surat telah terdaftar");
                }
                else
                {
                    var dataExcel = model.ExcelModel;
                    var fileExcel = SerahTerimaGesekanService.ExportExcel(dataExcel);
                    var guid = Guid.NewGuid().ToString();
                    TempData[guid] = fileExcel;
                    return Ok(guid);
                }
            }
        }

        [HttpGet("Download/{guid}")]
        public IActionResult Download(string guid)
        {
            if (TempData[guid] == null)
            {
                return new EmptyResult();
            }

            var fileExcel = TempData[guid] as byte[];
            var filename = string.Format("SerahTerima_{0:ddMMyyyy_HHmmss}.xlsx", DateTime.Now);
            return File(fileExcel, "application/vnd.ms-excel", filename);

        }
    }
}

