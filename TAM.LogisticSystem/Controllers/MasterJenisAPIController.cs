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
    [Route("api/v1/[controller]")]
    [Authorize]
    public class MasterJenisAPIController : Controller
    {
        private readonly MasterJenisService MasterJenisService;
        public MasterJenisAPIController(MasterJenisService masterJenisService)
        {
            this.MasterJenisService = masterJenisService;
        }

        /// <summary>
        /// Get all data from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var jenisData = await this.MasterJenisService.GetJenisData();
            return Ok(jenisData);
        }

        /// <summary>
        /// Insert data to the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MasterJenisViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var validateKodeJenis = await MasterJenisService.Validate(model.AFICarTypeCode);
            if(validateKodeJenis == true)
            {
                return BadRequest("Kode Jenis telah terdaftar");
            }
            await MasterJenisService.AddJenisData(model);
            return Ok();
        }

        /// <summary>
        /// Edit selected data
        /// </summary>
        /// <returns></returns>
        [HttpPost("{code}")]
        public async Task<IActionResult> Edit(string code, [FromBody]MasterJenisViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            await MasterJenisService.UpdateJenisData(code, model);
            return Ok();
        }

        /// <summary>
        /// delete selected data from the database
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var rowAffected = await MasterJenisService.RemoveJenisData(code);
            if (rowAffected < 1)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok();
        }
    }
}
