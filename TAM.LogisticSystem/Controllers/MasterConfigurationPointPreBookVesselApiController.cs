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
    [Route("api/v1/[controller]")]
    public class MasterConfigurationPointPreBookVesselApiController : Controller
    {
        public MasterConfigurationPointPreBookVesselApiController(MasterConfigurationPointPreBookVesselService masterConfigurationPointPreBookVesselService)
        {
            this.MasterConfigurationPointPreBookVesselService = masterConfigurationPointPreBookVesselService;
        }
        private readonly MasterConfigurationPointPreBookVesselService MasterConfigurationPointPreBookVesselService;

        // GET: /<controller>/

        // TIE: START
        //[HttpGet]

        //public async Task<IActionResult> Get()
        //{
        //    var data = await MasterConfigurationPointPreBookVesselService.GetBookVesselDataList();
        //    return Ok(data);
        //}

        //[HttpPost("AddBookVessel")]

        //public async Task<IActionResult> Post([FromBody] PointPreBookVesselListViewModel point)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return Ok("fail");
        //    }
        //    else
        //    {
        //        await MasterConfigurationPointPreBookVesselService.AddBookVesselData(point);

        //        return Ok();
        //    }

        //}

        //[HttpPut("UpdateBookVessel")]

        //public async Task<IActionResult> Put([FromBody] PointPreBookVesselListViewModel point)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return Ok("fail");
        //    }
        //    else
        //    {
        //        await MasterConfigurationPointPreBookVesselService.UpdateBookVesselData(point);
        //        return Ok();
        //    }    

        //}

        //[HttpDelete("{id}")]

        //public async Task<IActionResult> Delete(string id)
        //{
        //    await MasterConfigurationPointPreBookVesselService.DeleteBookVesselData(id);

        //    return Ok();
        //}
        // TIE: END
    }
}
