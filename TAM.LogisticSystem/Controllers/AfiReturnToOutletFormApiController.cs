using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class AfiReturnToOutletFormApiController : Controller
    {
        private readonly AfiReturnToOutletFormService AfiReturnToOutletFormService;

        public AfiReturnToOutletFormApiController(AfiReturnToOutletFormService afiReturnToOutletFormService)
        {
            this.AfiReturnToOutletFormService = afiReturnToOutletFormService;
        }
        
        /// <summary>
        /// Get Dropdown Region And RegionAFI
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllRegion")]
        public async Task<IActionResult> GetAllRegion()
        {
            var Data = await this.AfiReturnToOutletFormService.GetRegionAndRegionAFI();
            return Ok(Data);
        }
        [HttpGet]
        [Route("CheckDataByFrame/{id}")]
        public async Task<IActionResult> CheckDataByFrame(string id)
        {
            var vehicle = this.AfiReturnToOutletFormService.CheckVehicleExists(id);
            if (vehicle == null)
            {
                return BadRequest("Frame Number tidak terdaftar");
            }

            if (this.AfiReturnToOutletFormService.CheckVehicleInAFI(vehicle.VehicleId))
            {
                return BadRequest("Frame Number sudah terpakai. Silahkan pilih Frame Number lain");
            }
            
            var Data = await this.AfiReturnToOutletFormService.GetVehicle(id);
            return Ok(Data);
        }

        [HttpPost]
        [Route("UpdateAFINormal")]
        public async Task<IActionResult> UpdateAFINormal([FromBody] AfiReturnToOutletFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var rowAffected = await this.AfiReturnToOutletFormService.UpdateAFINormal(model);
            if (rowAffected == 0)
            {
                return BadRequest("Gagal simpan data");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpPost]
        [Route("UpdateAFIRevisi")]
        public async Task<IActionResult> UpdateAFIRevisi([FromBody] AfiReturnToOutletFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var rowAffected = 0;
            //2 is REV.A
            if(model.TipePengajuan == 2)
            {
                rowAffected = await this.AfiReturnToOutletFormService.UpdateAFIRevisiA(model); 
            }
            else if (model.TipePengajuan == 3) //3 is REV.B
            {
                rowAffected = await this.AfiReturnToOutletFormService.UpdateAFIRevisiB(model);
            }
            else if (model.TipePengajuan == 4) //4 is REV.C
            {
                rowAffected = await this.AfiReturnToOutletFormService.UpdateAFIRevisiC(model);
            }
            else if (model.TipePengajuan == 5) //5 is REV.C
            {
                rowAffected = await this.AfiReturnToOutletFormService.UpdateAFIRevisiD(model);
            }
            else if (model.TipePengajuan == 6) //6 is REV.C
            {
                rowAffected = await this.AfiReturnToOutletFormService.UpdateAFIRevisiE(model);
            }
            else if (model.TipePengajuan == 7) //7 is REV.C
            {
                rowAffected = await this.AfiReturnToOutletFormService.UpdateAFIRevisiF(model);
            }

            if (rowAffected == 0)
            {
                return BadRequest("Gagal simpan data");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpPost]
        [Route("UpdateAFIExCancel")]
        public async Task<IActionResult> UpdateAFIExCancel([FromBody] AfiReturnToOutletFormModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var rowAffected = await this.AfiReturnToOutletFormService.UpdateExCancel(model);
            if (rowAffected == 0)
            {
                return BadRequest("Gagal simpan data");
            }
            return Ok("Data berhasil disimpan");
        }
    }
}
