using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

namespace TAM.LogisticSystem.Controllers
{
    public class RoutingDictionaryController : Controller
    {
        private readonly RoutingDictionaryService routingDictionaryService;
        private readonly RoutingDictionaryDetailService routingDictionaryDetailService;


        public RoutingDictionaryController(RoutingDictionaryDetailService routingDictionaryDetailService)
        {
            this.routingDictionaryDetailService = routingDictionaryDetailService;
        }

        public RoutingDictionaryController(RoutingDictionaryService routingDictionaryService)
        {
            this.routingDictionaryService = routingDictionaryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRoutingDictionary()
        {
            var Data = routingDictionaryService.GetRoutingDictionary();
            return Ok(Data);
        }

        public IActionResult GetVehicle()
        {
            var Data = routingDictionaryService.GetVehicle();
            return Ok(Data);
        }

        public IActionResult GetBranch()
        {
            var Data = routingDictionaryService.GetBranch();
            return Ok(Data);
        }

        public IActionResult GetDealer()
        {
            var Data = routingDictionaryService.GetDealer();
            return Ok(Data);
        }

        //public IActionResult GetRoutingDictionaryDetail()
        //{
        //    var Data = routingDictionaryDetailService.GetRoutingDictionaryDetail();
        //    return Ok(Data);
        //}

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

        [HttpPost("/routingdictionary/create")]
        public async Task<IActionResult> CreateHeader([FromBody]RoutingDictionaryViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            await routingDictionaryService.Add(model);
            return Ok(model);
        }

        // TIE: START
        //[HttpPost("/routingdictionary/edit/{id}")]
        //public async Task<IActionResult> EditHeader(int id, [FromBody]RoutingDictionaryViewModel model)
        //{
        //    int recordAffacted = await routingDictionaryService.Update(id, model);
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

        //[HttpPost("/routingdictionary/delete/{id}")]
        //public async Task<IActionResult> DeleteHeader(int id)
        //{
        //    await routingDictionaryService.Remove(id);
        //    return RedirectToAction("Index");
        //}


        //[HttpPost("/routingdictionarydetail/create")]
        //public async Task<IActionResult> CreateDetail([FromBody]RoutingDictionaryDetailViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return View(model);
        //    }
        //    await routingDictionaryDetailService.Add(model);
        //    return Ok(model);
        //}

        //[HttpPost("/routingdictionarydetail/edit/{id}")]
        //public async Task<IActionResult> EditDetail(int id, [FromBody]RoutingDictionaryDetailViewModel model)
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
        public async Task<IActionResult> DeleteDetail(int id)
        {
            await routingDictionaryDetailService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
