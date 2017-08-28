using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class CancelDeliveryRequestApiController : Controller
    {
        public CancelDeliveryRequestApiController(CancelDeliveryRequestService cancelDeliveryRequestService)
        {
            this.CancelDeliveryRequestService = cancelDeliveryRequestService;
        }

        private readonly CancelDeliveryRequestService CancelDeliveryRequestService;

        /// <summary>
        /// Get All Location
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllCancelDeliveryRequestLocation")]
        public async Task<IActionResult> GetAllLocation()
        {
            var locationList = await this.CancelDeliveryRequestService.GetAllLocation();
            return Ok(locationList);
        }

        /// <summary>
        /// Get All Delivery Request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            var data = await this.CancelDeliveryRequestService.GetAllData();
            return Ok(data);
        }

        /// <summary>
        /// Cancel Delivery Request
        /// </summary>
        /// <param name="deliveryRequestNumber"></param>
        /// <returns></returns>
        [HttpPost("CancelDeliveryRequest")]
        public async Task <IActionResult> CancelDeliveryRequest([FromBody]CancelDeliveryRequestViewModel model)
        {
            var isError = 0;

            if (ModelState.IsValid == false)
            {
                isError = 1;
                return BadRequest(isError);
            }

            isError = await this.CancelDeliveryRequestService.CancelDeliveryRequest(model.DeliveryRequestNumber);

            return Ok(isError);
        }
    }
}
