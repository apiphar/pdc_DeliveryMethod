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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class MaintenanceShiftKerjaApiController : Controller
    {
        private readonly MaintenanceShiftKerjaService service;

        public MaintenanceShiftKerjaApiController(MaintenanceShiftKerjaService service)
        {
            this.service = service;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allViewModel = await this.service.GetMaintenanceShiftKerja();
            return Ok(allViewModel);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MaintenaceShiftKerjaPostModel postModel)
        {
            if (ModelState.IsValid)
            {
                if (await this.service.PostData(postModel) > 0)
                {
                    var allViewModel = await this.service.GetMaintenanceShiftKerja();
                    return Ok(allViewModel);
                }
                else
                {
                    return BadRequest("Batas Waktu yang diinput harus diluar batas waktu sebelumnya");
                }
            }
            else
            {
                return BadRequest("Data tidak valid");
            }
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]MaintenaceShiftKerjaPostModel postModel)
        {
            if (ModelState.IsValid)
            {
                if (await this.service.UpdateData(postModel) > 0)
                {
                    var allViewModel = await this.service.GetMaintenanceShiftKerja();
                    return Ok(allViewModel);
                }
                else
                {
                    return BadRequest("Batas Waktu yang diinput harus diluar batas waktu sebelumnya");
                }
            }
            else { return BadRequest("Data tidak valid"); }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (await this.service.DeleteData(id) > 0)
            {
                var allViewModel = await this.service.GetMaintenanceShiftKerja();
                return Ok(allViewModel);
            }
            else
            {
                return BadRequest("Data gagal dihapus");
            }
        }

    }
}
