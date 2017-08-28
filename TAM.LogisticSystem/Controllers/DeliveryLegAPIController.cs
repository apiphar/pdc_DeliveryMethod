using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class DeliveryLegAPIController : Controller
    {
        public DeliveryLegAPIController(DeliveryLegService deliveryLegService)
        {
            this.DeliveryLegService = deliveryLegService;
        }
        private readonly DeliveryLegService DeliveryLegService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.DeliveryLegService.GetAll();
            return Ok(data);
        }

        [HttpPost("SendData")]
        public async Task<IActionResult> Create([FromBody]DeliveryLegCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var message = await this.DeliveryLegService.SendData(model);
            if (message == "DUPLICATE")
            {
                return BadRequest(message);
            }
            return Ok();
        }

        [HttpPut("UpdateData")]
        public async Task<IActionResult> Update([FromBody]DeliveryLegCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.DeliveryLegService.UpdateData(model);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await DeliveryLegService.Remove(id);
            return Ok();
        }
    }
}
