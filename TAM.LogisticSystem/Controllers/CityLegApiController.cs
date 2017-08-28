using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CityLegApiController : Controller
    {
        private readonly CityLegService CityLegService;

        public CityLegApiController(CityLegService cityLegService)
        {
            CityLegService = cityLegService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.CityLegService.GetAll();
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Post([FromBody]CityLegSendViewModel cityLegSendViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            if (await CityLegService.SendData(cityLegSendViewModel) == "DUPLICATE")
            {
                return BadRequest("DUPLICATE");
            }
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]CityLegSendViewModel cityLegSendViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await CityLegService.UpdateData(cityLegSendViewModel);
            return Ok();
        }

        [HttpDelete("Delete/{cityLegCode}")]
        public async Task<IActionResult> Delete(string cityLegCode)
        {
            await CityLegService.DeleteData(cityLegCode);
            return Ok();
        }
    }
}
