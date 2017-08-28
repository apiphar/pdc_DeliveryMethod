using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class AfiRequestRevisiAndExCancelFormApiController : Controller
    {
        public AfiRequestRevisiAndExCancelFormService AfiRequestRevisiAndExCancelFormService;
        public AfiRequestRevisiAndExCancelFormApiController(AfiRequestRevisiAndExCancelFormService afiRequestRevisiFormService)
        {
            this.AfiRequestRevisiAndExCancelFormService = afiRequestRevisiFormService;
        }
        /// <summary>
        /// Get Dropdown Region And RegionAFI
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllRegion")]
        public async Task<IActionResult> GetAllRegion()
        {
            var Data = await this.AfiRequestRevisiAndExCancelFormService.GetRegionAndRegionAFI();
            return Ok(Data);
        }
        [HttpPost]
        [Route("UpdateAFIRevisi")]
        public async Task<IActionResult> UpdateAFIRevisi([FromBody] AfiRevisiAndExCancelForm model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var rowAffected = 0;
            //2 is REV.A
            if (model.TipePengajuan == 2)
            {
                rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateAFIRevisiA(model);
            }
            else if (model.TipePengajuan == 3) //3 is REV.B
            {
                rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateAFIRevisiB(model);
            }
            else if (model.TipePengajuan == 4) //4 is REV.C
            {
                rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateAFIRevisiC(model);
            }
            else if (model.TipePengajuan == 5) //5 is REV.C
            {
                rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateAFIRevisiD(model);
            }
            else if (model.TipePengajuan == 6) //6 is REV.C
            {
                rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateAFIRevisiE(model);
            }
            else if (model.TipePengajuan == 7) //7 is REV.C
            {
                rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateAFIRevisiF(model);
            }

            if (rowAffected == 0)
            {
                return BadRequest("Gagal simpan data");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpPost]
        [Route("UpdateAFIExCancel")]
        public async Task<IActionResult> UpdateAFIExCancel([FromBody] AfiRevisiAndExCancelForm model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var rowAffected = await this.AfiRequestRevisiAndExCancelFormService.UpdateExCancel(model);
            if (rowAffected == 0)
            {
                return BadRequest("Gagal simpan data");
            }
            return Ok("Data berhasil disimpan");
        }
    }
}
