using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class GenerateShiftKerjaApiController : Controller
    {
        public GenerateShiftKerjaApiController(GenerateShiftKerjaService generateShiftKerjaService)
        {
            this.GenerateShiftKerjaService = generateShiftKerjaService;
        }
        private readonly GenerateShiftKerjaService GenerateShiftKerjaService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.GenerateShiftKerjaService.GetAll();
            return Ok(data);
        }

        [HttpPost("GenerateData")]
        public async Task<IActionResult> GenerateData([FromBody] LocationWorkHourViewModel workingTimeCreateViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.GenerateShiftKerjaService.GenerateData(workingTimeCreateViewModel);
            return Ok();
        }
    }
}
