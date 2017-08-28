using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/defectmaintenance")]
    public class DefectMaintenanceApiController : Controller
    {
        private readonly DefectMaintenanceService DevMan;

        public DefectMaintenanceApiController(DefectMaintenanceService devMan)
        {
            this.DevMan = devMan;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var Data = await DevMan.GetData();
            return Ok(Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DefectMaintenanceViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Invalid model request");
            }

            await DevMan.Add(model);
            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] DefectMaintenanceViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Invalid Model Request");
            }

            await DevMan.Edit(id, model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await DevMan.Get(id);
            if (entity == null)
            {
                return NotFound("Defect Part not found");
            }

            await DevMan.Remove(entity);
            return Ok();
        }
    }
}
