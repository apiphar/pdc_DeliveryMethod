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
    public class ConfigurationPlanningApiController : Controller
    {
        private readonly ConfigurationPlanningService configurationPlanningService;

        public ConfigurationPlanningApiController(ConfigurationPlanningService configurationPlanningService_)
        {
            this.configurationPlanningService = configurationPlanningService_;
        }

        // TIE: START
        //public IActionResult GetAll()
        //{
        //    var data = configurationPlanningService.GetAll();
        //    return Ok(data);
        //}

        //public IActionResult GetRoutingMaster()
        //{
        //    var data = configurationPlanningService.GetRoutingMasterCode();
        //    return Ok(data);
        //}

        //[HttpPost("/configurationplanningapi/delete/{mccp}/{dccp}")]
        //public async Task<IActionResult> Delete(bool mccp, bool dccp, [FromBody]ConfigurationPlanningViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return NotFound();
        //    }

        //    await configurationPlanningService.SetDCCPFalse(dccp, model);
        //    await configurationPlanningService.SetMCCPFalse(mccp, model);
        //    return Ok();
        //}

        //[HttpPost("/configurationplanningapi/edit/{id1}/{id2}/{mccp}/{dccp}")]
        //public async Task<IActionResult> Edit(string id1, string id2, bool mccp, bool dccp, [FromBody]ConfigurationPlanningViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return NotFound();
        //    }

        //    await configurationPlanningService.SetDCCPFalse(dccp, model);
        //    await configurationPlanningService.SetMCCPFalse(mccp, model);
        //    await configurationPlanningService.UpdateMCCP(id1, model);
        //    await configurationPlanningService.UpdateDCCP(id2, model);
        //    return Ok();
        //}
        // TIE: END
    }
}
