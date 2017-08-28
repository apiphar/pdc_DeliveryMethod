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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class VesselArrivalApiController : Controller
    {
        public VesselArrivalApiController(VesselArrivalService vesselArrivalService)
        {
            this.VesselArrivalService = vesselArrivalService;
        }
        private readonly VesselArrivalService VesselArrivalService;
        
        // Load all GET method
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.VesselArrivalService.GetAll();
            return Ok(data);
        }

        // TIE: START
        //// POST data
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]VesselArrivalCreateViewModel vesselArrivalCreateViewModel)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest();
        //    }
        //    var message = await this.VesselArrivalService.CreateNewVoyageDestination(vesselArrivalCreateViewModel);
        //    return Ok(message);
        //}
        // TIE: END
    }
}
