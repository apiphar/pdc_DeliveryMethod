using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class DwellingTimeAPIController : Controller
    {
        private readonly DwellingTimeService DwellingTimeService;
        public DwellingTimeAPIController(DwellingTimeService dwellingTimeService)
        {
            this.DwellingTimeService = dwellingTimeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var dwellingData = await this.DwellingTimeService.GetDwellingData();
            return Ok(dwellingData);
        }

        [HttpGet]
        [Route("GetLocationDwelling")]
        public async Task<IActionResult> GetLocations()
        {
            var getLocations = await this.DwellingTimeService.GetLocationCode();

            return Ok(getLocations);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InsertDwellingViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if (model.LocationFrom == model.LocationTo)
            {
                return BadRequest("Lokasi awal tidak boleh sama dengan Lokasi tujuan");
            }
            var validate = await DwellingTimeService.Validate(model);
            if (validate == false)
            {
                return BadRequest("Lokasi awal dan Lokasi akhir tidak boleh duplikat");
            }
            await DwellingTimeService.AddDwellingData(model);
            return Ok();
        }

        [HttpPost("UpdateDwelling")]
        public async Task<IActionResult> Edit([FromBody]InsertDwellingViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            await DwellingTimeService.UpdateDwellingData(model);
            return Ok();
        }

        [HttpPut("DeleteDwelling")]
        public async Task<IActionResult> Delete([FromBody]DwellingTimeViewModel model)
        {
            var rowAffected = await DwellingTimeService.RemoveDwellingData(model.LocationFrom, model.LocationTo);
            if (rowAffected < 1)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok();
        }
    }
}
