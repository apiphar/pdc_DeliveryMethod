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
    public class DealerMasterApiController : Controller
    {
        public DealerMasterApiController(DealerMasterService dealerMasterService)
        {
            this.dealerMasterService = dealerMasterService;
        }
        private readonly DealerMasterService dealerMasterService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await this.dealerMasterService.GetAll();
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Edit([FromBody] DealerMasterViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await dealerMasterService.EditDealer(model);
            return Ok();
        }
    }
}
