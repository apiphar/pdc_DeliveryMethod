using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class RoutingProductionLeadTimeController : Controller
    {
        private readonly RoutingProductionLeadTimeService routingProductionLeadTimeService;

        public RoutingProductionLeadTimeController(RoutingProductionLeadTimeService routingProductionLeadTimeService)
        {
            this.routingProductionLeadTimeService = routingProductionLeadTimeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // TIE: START
        //public IActionResult GetData()
        //{
        //    var Data = routingProductionLeadTimeService.GetData();
        //    return Ok(Data);
        //}

        //[Route("/routingProductionLeadTime/GetKodeRute/{RoutingMasterCode}/")]
        //public IActionResult GetKodeRute(string RoutingMasterCode)
        //{

        //    var Data = routingProductionLeadTimeService.GetKodeRute(RoutingMasterCode);
        //    return Ok(Data);
        //}

        //public IActionResult GetKatashiki()
        //{
        //    var location = routingProductionLeadTimeService.GetKatashiki();
        //    return Ok(location);
        //}

        //public IActionResult GetRoutingMasterCode()
        //{
        //    var Datas = routingProductionLeadTimeService.GetRoutingMasterCode();
        //    return Ok(Datas);
        //}
        //public IActionResult GetDropDownLocationCode()
        //{
        //    var Datas = routingProductionLeadTimeService.GetLocationCode();
        //    return Ok(Datas);
        //}


        //[Route("/routingProductionLeadTime/getsuffix/{Katashiki}")]
        //public IActionResult getSuffix(string Katashiki)
        //{
        //    var getKatahshiki = routingProductionLeadTimeService.GetSuffix(Katashiki);
        //    return Ok(getKatahshiki);
        //}

        //[Route("/routingProductionLeadTime/getcarmodel/{Katashiki}/{Suffix}/")]
        //public IActionResult getCarModel(string Katashiki, string Suffix)
        //{
        //    var getCarModel = routingProductionLeadTimeService.GetCarModel(Katashiki, Suffix);
        //    return Ok(getCarModel);
        //}


        //[HttpPost("/routingProductionLeadTime/create")]
        //public async Task<IActionResult> Create([FromBody]RoutingProductionLeadTimeViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return View(model);
        //    }
        //    int data = await routingProductionLeadTimeService.Add(model);
        //    return Ok(data);
        //}

        //[HttpPost("/routingProductionLeadTime/edit/{id}")]
        //public async Task<IActionResult> Edit(int id, [FromBody]RoutingProductionLeadTimeViewModel model)
        //{
        //    int recordAffacted = await routingProductionLeadTimeService.Update(id, model);
        //    if (recordAffacted > 0)
        //    {
        //        TempData["Status"] = 1;
        //        TempData["Message"] = "Data has been saved.";
        //    }
        //    else
        //    {
        //        TempData["Status"] = 2;
        //        TempData["Message"] = "Data cannot be saved!";
        //    }
        //    return RedirectToAction("Index");
        //}

        //[HttpPost("/routingProductionLeadTime/delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await routingProductionLeadTimeService.Remove(id);
        //    return RedirectToAction("Index");
        //}
        // TIE: END
    }
}
