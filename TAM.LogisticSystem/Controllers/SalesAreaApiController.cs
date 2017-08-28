using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.TANGO.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/salesarea")]
    public class SalesAreaApiController : Controller
    {
        private readonly SalesAreaService SalesAreaService;

        public SalesAreaApiController(SalesAreaService salesAreaService)
        {
            this.SalesAreaService = salesAreaService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var Data = await SalesAreaService.GetData();
            return Ok(Data);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalesAreaViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var status = await SalesAreaService.Add(model.SalesAreaCode, model.Description);
            if (status == false)
            {
                return BadRequest("Kode Sales Area sudah terdaftar");
            }
            return Ok();
        }

        [HttpPost("{salesAreaCode}")]
        public async Task<IActionResult> Edit(string salesAreaCode, [FromBody] SalesAreaUpdateModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var status = await SalesAreaService.Update(salesAreaCode, model.Description);
            if (status == false)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok();
        }
        
        [HttpDelete("{salesAreaCode}")]
        public async Task<IActionResult> Delete(string salesAreaCode)
        {
            var status = await SalesAreaService.Remove(salesAreaCode);
            if (status == false)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok();
        }
    }
}
