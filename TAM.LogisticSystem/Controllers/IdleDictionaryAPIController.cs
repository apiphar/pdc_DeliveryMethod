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
    [Route("api/v1/[controller]")]
    [Authorize]
    public class IdleDictionaryAPIController : Controller
    {
        public IdleDictionaryAPIController(PlanningKalenderKerjaPolaBreakSemingguService kalenderKerjaPolaBreakSemingguService)
        {
            this.PolaBreakSemingguService = kalenderKerjaPolaBreakSemingguService;
        }
        private readonly PlanningKalenderKerjaPolaBreakSemingguService PolaBreakSemingguService;

        // Get all data
        [HttpGet("GetData")]
        public async Task<IActionResult> GetAll()
        {
            var data = await this.PolaBreakSemingguService.GetAll();
            return Ok(data);
        }

        // Add BreakHour
        [HttpPost("InsertUpdateIdleDictionary")]
        public async Task<IActionResult> InsertUpdateIdleDictionary([FromBody] BreakHourSendViewModel breakHour)
        {
            await this.PolaBreakSemingguService.InsertUpdateBreakHour(breakHour);
            return Ok();
        }

        // Delete BreakHour
        [HttpDelete("DeleteIdleDictionary/{idleDictionaryCode}")]
        public async Task<IActionResult> DeleteIdleDictionary(string breakHourTemplateCode)
        {
            await this.PolaBreakSemingguService.DeleteBreakHourTemplate(breakHourTemplateCode);
            return Ok();
        }
    }
}
