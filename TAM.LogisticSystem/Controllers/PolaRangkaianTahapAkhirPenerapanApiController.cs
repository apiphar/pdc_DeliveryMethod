using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/polarangkaiantahapakhirpenerapan")]
    public class PolaRangkaianTahapAkhirPenerapanApiController : Controller
    {
        private readonly PolaRangkaianTahapAkhirPenerapanService PolaRangkaianTahapAkhirPenerapanService;

        public PolaRangkaianTahapAkhirPenerapanApiController(PolaRangkaianTahapAkhirPenerapanService polaRangkaianTahapAkhirPenerapanService)
        {
            this.PolaRangkaianTahapAkhirPenerapanService = polaRangkaianTahapAkhirPenerapanService;
        }

        /// <summary>
        /// Fungsi untuk mengambil data yang dibutuhkan pada UI
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var AllData = await PolaRangkaianTahapAkhirPenerapanService.GetDataProcessHeadMapping();

            return Ok(AllData);
        }

        //Fungsi untuk men-generate data dari client
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PolaRangkaianTahapAkhirPenerapanViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var recordAffected = await PolaRangkaianTahapAkhirPenerapanService.Add(model.RoutingDictionaryTailCode, model.CarType, model.Branch);
            if (recordAffected < 1)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok();
        }
    }
}
