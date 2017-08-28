using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class MasterLeadTimeController : Controller
    {
        private readonly MasterLeadTimeService masterLeadTimeService;

        public MasterLeadTimeController(MasterLeadTimeService masterLeadTimeService)
        {
            this.masterLeadTimeService = masterLeadTimeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            var Data = masterLeadTimeService.GetData();
            return Ok(Data);
        }

        public IActionResult GetDropDownLocation()
        {
            var Datas = masterLeadTimeService.GetLocation();
            return Ok(Datas);
        }


        [Route("/masterLeadTime/GetKodeRute/{RoutingMasterCode}/")]
        public IActionResult GetKodeRute(string RoutingMasterCode)
        {

            var Data = masterLeadTimeService.GetKodeRute(RoutingMasterCode);
            return Ok(Data);
        }

        public IActionResult GetAll()
        {
            var location = masterLeadTimeService.GetLocation();
            return Ok(location);
        }


        [HttpPost("/masterLeadTime/create")]
        public async Task<IActionResult> Create([FromBody]MasterLeadTimeViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            await masterLeadTimeService.Add(model);
            return Ok(model);
        }

        [HttpPost("/masterLeadTime/edit/{LocationCode}/{RoutingMasterCode}")]
        public async Task<IActionResult> Edit(string LocationCode, string RoutingMasterCode, [FromBody]MasterLeadTimeViewModel model)
        {
            int recordAffacted = await masterLeadTimeService.Update(LocationCode, RoutingMasterCode, model);

            //Error PayLoad

            //if (recordAffacted > 0)
            //{
            //    TempData["Status"] = 1;
            //    TempData["Message"] = "Data has been saved.";
            //}
            //else
            //{
            //    TempData["Status"] = 2;
            //    TempData["Message"] = "Data cannot be saved!";
            //}
            return Ok(model);
        }

        [Route("/masterLeadTime/check-location-code-and-routing-code/{locationcode}/{routingmastercode}")]
        public async Task<IActionResult> CheckLocationCodeAndRoutingCode(string locationcode, string routingmastercode)
        {
            var data = await masterLeadTimeService.CheckLocationCodeAndRoutingCode(locationcode, routingmastercode);
            return Ok(data);
        }
        [HttpPost("/masterLeadTime/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await masterLeadTimeService.Remove(id);
            return RedirectToAction("Index");
        }

    }
}
