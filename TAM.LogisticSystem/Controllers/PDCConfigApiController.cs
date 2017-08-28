using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class PDCConfigApiController : Controller
    {
        private readonly PDCConfigService pdcConfigService;
        public PDCConfigApiController(PDCConfigService pdcConfigService_)
        {
            this.pdcConfigService = pdcConfigService_;
        }

        // TIE: START
        //public IActionResult GetPDCConfig()
        //{
        //    var data = pdcConfigService.GetPDCConfig();
        //    return Ok(data);
        //}

        //public IActionResult GetPDC()
        //{
        //    var data = pdcConfigService.GetPDC();
        //    return Ok(data);
        //}

        //public async Task<IActionResult> GetId(string id)
        //{
        //    var data = await pdcConfigService.Get(id);
        //    return Ok(data);
        //}

        //[HttpPost("/pdcconfigapi/create")]
        //public async Task<IActionResult> Create([FromBody]PDCConfigViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return NotFound(model);
        //    }

        //    int recordAffected = await pdcConfigService.Add(model.LocationCode, model.MaintenanceDay, model.CarCarrierQuotaPerDay,
        //    model.NonCarCarrierQuotaPerDay, model.LeadDayPreDeliveryService);

        //    return Ok();          
        //}

        //[HttpPost("/pdcconfigapi/edit/{id}")]
        //public async Task<IActionResult> Edit(int id, [FromBody]PDCConfigViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return View(model);
        //    }

        //    int recordaffected = await pdcConfigService.Update(id, model);

        //    return Ok();
        //}

        //[HttpPost("/pdcconfigapi/delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    int rowsAffected = await pdcConfigService.Remove(id);
        //    return Ok();
        //}
        // TIE: END
    }
}
