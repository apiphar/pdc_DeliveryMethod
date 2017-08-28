using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class KonfigurasiGesekanApiController : Controller
    {
        private readonly KonfigurasiGesekanService konfigurasiGesekanService;
        private readonly AuthenticationService auth;

        public KonfigurasiGesekanApiController(KonfigurasiGesekanService konfigurasiGesekanService, AuthenticationService auth)
        {
            this.konfigurasiGesekanService = konfigurasiGesekanService;
            this.auth = auth;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // TIE: START
        //[Route("SaveConfiguration")]
        //[HttpPost]
        //public IActionResult SaveConfiguration([FromBody]ScratchConfigInsertData Data)
        //{
        //    int row = konfigurasiGesekanService.SaveConfiguration(Data);
        //    if (row < 1)
        //    {
        //        return BadRequest("Error while Saving Data");
        //    }
        //    return Ok(row);
        //}

        //[Route("CarModel")]
        //[HttpGet]
        //public async Task<IActionResult> CarModel()
        //{
        //    var user = auth.GetCurrentTangoUser();
        //    var Data = await konfigurasiGesekanService.GetCarModel(user.LocationCode);
        //    return Ok(Data);
        //}
        //[Route("DealerBranch")]
        //[HttpGet]
        //public async Task<IActionResult> DealerBranch()
        //{
        //    var user = auth.GetCurrentTangoUser();
        //    var Data = await konfigurasiGesekanService.GetDealerBranch(user.LocationCode);
        //    return Ok(Data);
        //}
        //[Route("ExistsScratch")]
        //[HttpGet]
        //public async Task<IActionResult> ExistsScratch()
        //{
        //    var user = auth.GetCurrentTangoUser();
        //    var Data = await konfigurasiGesekanService.GetKonfigurasiGesekanDt(user.LocationCode);
        //    return Ok(Data);
        //}
        // TIE: END
    }
}
