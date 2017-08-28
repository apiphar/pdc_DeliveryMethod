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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class DeliveryUnitAdvanceApiController : Controller
    {

        private readonly DeliveryUnitAdvanceService DeliveryUnitAdvanceService;

        public DeliveryUnitAdvanceApiController(DeliveryUnitAdvanceService deliveryUnitAdvanceService)
        {
            this.DeliveryUnitAdvanceService = deliveryUnitAdvanceService;
        }
        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await DeliveryUnitAdvanceService.GetUnitAdvanceData();
            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }

        //post form model dari ts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DeliveryUnitAdvanceViewModel deliveryUnitAdvanceViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if (deliveryUnitAdvanceViewModel == null)
            {
                return BadRequest();
            }
            await DeliveryUnitAdvanceService.SubmitUnitAdvanceData(deliveryUnitAdvanceViewModel);
            return Ok();
        }
    }
}
