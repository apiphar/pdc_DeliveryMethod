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
    [Route("api/v1/[controller]")]
    public class MccpApiController : Controller
    {
        private MCCPService mccpService;

        public MccpApiController(MCCPService mccpService)
        {
            this.mccpService = mccpService;
        }
        [HttpGet("getdata")]
        public IActionResult GetData()
        {
            var data = mccpService.GetAllData();
            return Ok(data);
        }
        [HttpGet("getbranchdata")]
        public IActionResult GetBranchData()
        {
            var data = mccpService.GetBranchData();
            return Ok(data);
        }
        [HttpGet("getdealerdata")]
        public IActionResult GetDealerData()
        {
            var data = mccpService.GetDealerData();
            return Ok(data);
        }
        [HttpGet("getlocationdata")]
        public IActionResult GetLocationData()
        {
            var data = mccpService.GetLocationData();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MCCPViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return Ok("error");
            }
            await mccpService.Add(model);
            return Ok("success");

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] MCCPViewModel model)
        {
            int recordAffacted = await mccpService.Update(id, model);
            if (recordAffacted > 0)
            {
                TempData["Status"] = 1;
                TempData["Message"] = "Data has been saved.";
            }
            else
            {
                TempData["Status"] = 2;
                TempData["Message"] = "Data cannot be saved!";
            }

            return Ok(TempData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await mccpService.Remove(id);
            return Ok();
        }
    }
}
