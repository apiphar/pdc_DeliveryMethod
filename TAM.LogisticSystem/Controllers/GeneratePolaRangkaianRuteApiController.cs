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
    [Authorize]
    [Route("api/v1/generatepolarangkaianrute")]
    public class GeneratePolaRangkaianRuteApiController : Controller
    {
        private readonly GeneratePolaRangkaianRuteService GeneratePolaRangkaianRuteService;

        public GeneratePolaRangkaianRuteApiController(GeneratePolaRangkaianRuteService generatePolaRangkaianRuteService)
        {
            this.GeneratePolaRangkaianRuteService = generatePolaRangkaianRuteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataAsync()
        {
            var data = await GeneratePolaRangkaianRuteService.GetDataGeneratePolaRangkaianRute();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GeneratePolaRangkaianRuteInsertModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var recordAffected = await GeneratePolaRangkaianRuteService.Add(model.JoinData, Convert.ToDateTime(model.ValidFrom));
            if (recordAffected < 1)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok();
        }
    }
}
