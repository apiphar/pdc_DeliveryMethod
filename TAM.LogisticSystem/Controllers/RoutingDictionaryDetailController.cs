using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

namespace TAM.LogisticSystem.Controllers
{
    public class RoutingDictionaryDetailController : Controller
    {
        private readonly RoutingDictionaryDetailService routingDictionaryDetailService;

        public RoutingDictionaryDetailController(RoutingDictionaryDetailService routingDictionaryDetailService)
        {
            this.routingDictionaryDetailService = routingDictionaryDetailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // TIE: START
        //public IActionResult GetRoutingDictionary()
        //{
        //    var Data = routingDictionaryDetailService.GetRoutingDictionary();
        //    return Ok(Data);
        //}
        // TIE: END

        public IActionResult GetLocation()
        {
            var Data = routingDictionaryDetailService.GetLocation();
            return Ok(Data);
        }

        public IActionResult GetDeliveryMethod()
        {
            var Data = routingDictionaryDetailService.GetDeliveryMethod();
            return Ok(Data);
        }


        [HttpPost("/routingdictionarydetail/create")]
        public async Task<IActionResult> Create([FromBody]RoutingDictionaryDetailViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            await routingDictionaryDetailService.Add(model);
            return Ok(model);
        }

        // TIE: START
        //[HttpPost("/routingdictionarydetail/edit/{id}")]
        //public async Task<IActionResult> Edit(int id, [FromBody]RoutingDictionaryDetailViewModel model)
        //{
        //    int recordAffacted = await routingDictionaryDetailService.Update(id, model);
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
        // TIE: END

        [HttpPost("/routingdictionarydetail/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await routingDictionaryDetailService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
