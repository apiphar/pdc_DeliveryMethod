using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class GenerateJamBreakApiController : Controller
    {
        public GenerateJamBreakApiController(GenerateJamBreakService generateJamBreakService)
        {
            this.GenerateJamBreakService = generateJamBreakService;
        }
        private readonly GenerateJamBreakService GenerateJamBreakService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.GenerateJamBreakService.GetAll();
            return Ok(data);
        }

        [HttpPost("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate([FromBody] LocationBreakHourSendViewModel locationBreakHour)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var hasDuplicate = await this.GenerateJamBreakService.CheckDuplicate(locationBreakHour);
            if (hasDuplicate != null)
            {
                return BadRequest("DUPLICATE");
            }
            return Ok();
        }

        [HttpPost("GenerateData")]
        public async Task<IActionResult> GenerateData([FromBody] LocationBreakHourSendViewModel locationBreakHour) 
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.GenerateJamBreakService.GenerateData(locationBreakHour);
            return Ok();
        }
    }
}
