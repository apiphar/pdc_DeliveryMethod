using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class CreateLogisticPlanApiController : Controller
    {
        private readonly CreateLogisticPlanService service;

        public CreateLogisticPlanApiController(CreateLogisticPlanService service)
        {
            this.service = service;
        }

        [HttpPost]///("{tanggal/bulan/tahun}")
        public async Task<IActionResult> Post()//string tanggal, string bulan, string tahun
        {
            var allData = await this.service.GetModel();
            var temporalModel = this.service.GetTemporalModel(allData);
            return Ok(temporalModel);
        }

        // TIE: START
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var allData = await this.service.GetModel();
        //    var temporalModel = await this.service.GetTemporalModel(allData);
        //    await this.service.InsertVehicleRouting(temporalModel);
        //    return Ok(temporalModel);
        //}
        // TIE: END
    }
}
