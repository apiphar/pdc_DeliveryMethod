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
    public class AFIRestriksiAreaAPIController : Controller
    {
        private readonly AFIRestriksiAreaService AFIRestriksiAreaService;

        public AFIRestriksiAreaAPIController(AFIRestriksiAreaService afiRestriksiAreaService)
        {
            this.AFIRestriksiAreaService = afiRestriksiAreaService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var regionsRestriction = await this.AFIRestriksiAreaService.GetAfiRegionsRestriction();
            var provinces = await this.AFIRestriksiAreaService.GetProvinces();
            var allRequestedData = new AFIRestriksiAreaGetAllModel
            {
                AFIRegionsRestriction = regionsRestriction,
                Regions = provinces
            };
            return Ok(allRequestedData);
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> GetDetail(string regionCode, DateTimeOffset validFrom, DateTimeOffset validTo)
        {
            var regionsRestriction = await this.AFIRestriksiAreaService.GetAfiRegionsRestrictionDetail(regionCode, validFrom, validTo);
            var cities = await this.AFIRestriksiAreaService.GetCities(regionCode);
            var allRequestedData = new AFIRestriksiAreaGetAllModel
            {
                AFIRegionsRestriction = regionsRestriction,
                Regions = cities
            };
            return Ok(allRequestedData);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AFIRestriksiAreaInsertModel afiRestriksiAreaInsertModel)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if(await AFIRestriksiAreaService.IsSliced(afiRestriksiAreaInsertModel.RegionCode, afiRestriksiAreaInsertModel.ValidFrom, afiRestriksiAreaInsertModel.ValidTo) == false)
            {
                return BadRequest("Batas waktu yang diinput harus di luar batas waktu sebelumnya");
            }
            await this.AFIRestriksiAreaService.Create(afiRestriksiAreaInsertModel);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]AFIRestriksiAreaUpdateModel afiRestriksiAreaUpdateModel)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            await this.AFIRestriksiAreaService.Update(afiRestriksiAreaUpdateModel);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{afiRegionRestrictionId}")]
        public async Task<IActionResult> Delete(int afiRegionRestrictionId)
        {
            await this.AFIRestriksiAreaService.Delete(afiRegionRestrictionId);
            return Ok(200);
        }
    }
}
