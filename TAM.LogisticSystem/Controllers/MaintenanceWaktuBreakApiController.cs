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
    public class MaintenanceWaktuBreakApiController : Controller
    {
        private readonly MaintenanceWaktuBreakService maintenanceWaktuBreakService;
        public MaintenanceWaktuBreakApiController(MaintenanceWaktuBreakService maintenanceWaktuBreakService_)
        {
            this.maintenanceWaktuBreakService = maintenanceWaktuBreakService_;
        }

        // TIE: START
        //public IActionResult GetMaintenanceWaktuBreak()
        //{
        //    var data = maintenanceWaktuBreakService.GetMaintenanceWaktuBreak();
        //    return Ok(data);
        //}

        //public IActionResult GetLocation()
        //{
        //    var data = maintenanceWaktuBreakService.GetLocation();
        //    return Ok(data);
        //}

        //public IActionResult GetShift()
        //{
        //    var data = maintenanceWaktuBreakService.GetShift();
        //    return Ok(data);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody]MaintenanceWaktuBreakViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if(await maintenanceWaktuBreakService.Add(model) > 0 )
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    else { return NotFound();  }
        //}

        //[HttpPost("/maintenancewaktubreakapi/edit/{id}")]
        //public async Task<IActionResult> Edit(int id, [FromBody]MaintenanceWaktuBreakViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if(await maintenanceWaktuBreakService.Update(id, model) > 0)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    else { return NotFound(); }
        //}

        //[HttpPost("/maintenancewaktubreakapi/delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    int rowsAffected = await maintenanceWaktuBreakService.Remove(id);      
        //    return Ok();
        //}
        // TIE: END
    }
}
