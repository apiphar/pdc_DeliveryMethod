using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CityMasterAPIController : Controller
    {
        private readonly CityMasterService CityMasterService;
        public CityMasterAPIController(CityMasterService cityMasterService)
        {
            this.CityMasterService = cityMasterService;
        }

        /// <summary>
        /// Get all data Master city from table City
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cityData = await this.CityMasterService.GetCityData();

            return Ok(cityData);
        }

        /// <summary>
        /// Insert one master city data to table City
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CityMasterViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Gagal Insert Data");
            }
            var validate = await CityMasterService.ValidateCodeCity(model.CityCode);
            if (validate == true)
            {
                return BadRequest("Kode City telah terdaftar");
            }
            await CityMasterService.AddData(model);
            return Ok("Berhasil Insert Data");
        }

        /// <summary>
        /// Update one master city data to table City
        /// </summary>
        /// <returns></returns>
        [HttpPost("{code}")]
        public async Task<IActionResult> Edit(string code, [FromBody]CityMasterViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Gagal Update Data");
            }

            await CityMasterService.UpdateCityData(code, model);
            return Ok("Berhasil Update Data");
        }

        /// <summary>
        /// Delete one master city data to table City
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var rowAffected = await CityMasterService.RemoveCityData(code);
            if (rowAffected < 1)
            {
                return BadRequest("Gagal Delete Data");
            }
            return Ok("Berhasil Delete Data");
        }
    }
}
