using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    //[Route("api/v1/[controller]")]
    public class DeliveryShippingScheduleApiController : Controller
    {
        private readonly DeliveryShippingScheduleService DeliveryShippingScheduleService;

        public DeliveryShippingScheduleApiController(DeliveryShippingScheduleService deliveryShippingScheduleService)
        {
            this.DeliveryShippingScheduleService = deliveryShippingScheduleService;
        }

        [HttpGet("api/v1/DeliveryShippingScheduleApi")]
        public async Task<IActionResult> Get()
        {
            var deliveryShippingScheduleViewModel = await this.DeliveryShippingScheduleService.Init();
            return Ok(deliveryShippingScheduleViewModel);
        }

        [HttpPost("api/v1/DeliveryShippingScheduleApi")]
        public async Task<IActionResult> Save([FromBody]DeliveryShippingScheduleSaveModel deliveryShippingScheduleSaveModel)
        {
            await this.DeliveryShippingScheduleService.Save(deliveryShippingScheduleSaveModel);
            return Ok();
        }
    }
}
