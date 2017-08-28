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
    [Authorize]
    public class WorkingDictionaryAPIController : Controller
    {
        public WorkingDictionaryAPIController(PlanningKalenderKerjaPolaKerjaSemingguService workHourService)
        {
            this.WorkHourService = workHourService;
        }
        private PlanningKalenderKerjaPolaKerjaSemingguService WorkHourService;

        // Get all data
        [HttpGet("GetData")]
        public async Task<IActionResult> GetAll()
        {
            var data = await this.WorkHourService.GetAll();
            return Ok(data);
        }

        // Add WorkHour
        [HttpPost("InsertUpdateWorkHour")]
        public async Task<IActionResult> InsertUpdateIdleDictionary([FromBody] WorkHourSendViewModel workHour)
        {
            await this.WorkHourService.InsertUpdateWorkHour(workHour);
            return Ok();
        }

        // Delete WorkHour
        [HttpDelete("DeleteWorkHour/{workHourTemplateCode}")]
        public async Task<IActionResult> DeleteIdleDictionary(string workHourTemplateCode)
        {
            await this.WorkHourService.DeleteWorkHourTemplate(workHourTemplateCode);
            return Ok();
        }

    }
}
