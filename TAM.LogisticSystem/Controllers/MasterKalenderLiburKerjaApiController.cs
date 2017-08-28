using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/[controller]")]
    public class MasterKalenderLiburKerjaApiController : Controller
    {   

        public MasterKalenderLiburKerjaApiController(MasterKalenderLiburKerjaService masterKalenderLiburKerjaService)
        {
            this.masterKalenderLiburKerjaService = masterKalenderLiburKerjaService;
        }
        private readonly MasterKalenderLiburKerjaService masterKalenderLiburKerjaService;



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var dataKalender = await this.masterKalenderLiburKerjaService.PopulateLocation();
            return Ok(dataKalender);
        }

        // TIE: START
        //[HttpPost]
        //public async Task<IActionResult> Post(string location, List<DateTime> dates)
        //{
        //    await this.masterKalenderLiburKerjaService.SaveData(location, dates);
        //    return Ok();
        //}
        // TIE: END
    }
}
