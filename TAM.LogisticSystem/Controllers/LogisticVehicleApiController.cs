using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    public class LogisticVehicleApiController : Controller
    {
        private readonly LogisticVehicleService logisticVehicleService;
        private readonly WebEnvironmentService WebEnvironmentService;

        public LogisticVehicleApiController(LogisticVehicleService logisticVehicleService, WebEnvironmentService WebEnvironmentService)
        {
            this.logisticVehicleService = logisticVehicleService;
            this.WebEnvironmentService = WebEnvironmentService;
        }

        // TIE: START
        //[HttpGet("/LogisticVehicle/GetDataDeliveryMethod")]
        //public IActionResult GetDataDeliveryMethod()
        //{
        //    var dataMethod = logisticVehicleService.GetDataDeliveryMethod();
        //    return Ok(dataMethod);
        //}

        //[HttpGet("/LogisticVehicle/GetDataDeliveryMethodType")]
        //public IActionResult GetDataDeliveryMethodType()
        //{
        //    var dataMethodType = logisticVehicleService.GetDataDeliveryMethodType();

        //    return Ok(dataMethodType);
        //}

        //[HttpPost("/LogisticVehicle/create")]
        //public async Task<IActionResult> Create([FromBody] LogisticVehicleModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    int rowsAffected = await logisticVehicleService.Add(model);
        //    if (rowsAffected != 1)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //}

        //[HttpPost("/LogisticVehicle/edit/{id}")]
        //public async Task<IActionResult> Edit(string id, [FromBody] LogisticVehicleModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    int rowsAffected = await logisticVehicleService.Update(id, model);
        //    if (rowsAffected != 1)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //}

        //[HttpDelete("/LogisticVehicle/delete/{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    int rowsAffected = await logisticVehicleService.Remove(id);
        //    if (rowsAffected != 1)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //}
        // TIE: END
    }
}
