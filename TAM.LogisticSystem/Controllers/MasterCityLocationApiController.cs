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
    public class MasterCityLocationApiController : Controller
    {
        private readonly MasterCityLocationService MasterCityLocationService;

        public MasterCityLocationApiController(MasterCityLocationService masterCityLocationService)
        {
            this.MasterCityLocationService = masterCityLocationService;
        }

        // Get all Table Data
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tableData = await MasterCityLocationService.GetAllTableData();
            return Ok(tableData);
        }

        // Create new group Dealer
        [HttpPost]
        [Route("CreateData")]
        public async Task<IActionResult> CreateData([FromBody]MasterCityLocationViewModel masterCityLocationViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if (await MasterCityLocationService.CheckCode(masterCityLocationViewModel.KodeCityLocation) != null)
            {
                return BadRequest("Kode City Location sudah terdaftar");
            }
            await this.MasterCityLocationService.CreateData(masterCityLocationViewModel);
            return Ok();
        }

        [HttpPost]
        [Route("UpdateData")]
        public async Task<IActionResult> UpdateData([FromBody]MasterCityLocationViewModel masterCityLocationViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.MasterCityLocationService.UpdateData(masterCityLocationViewModel);
            return Ok();
        }

        // Delete Group Dealer
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await this.MasterCityLocationService.DeleteData(id);
            return Ok();
        }
    }
}
