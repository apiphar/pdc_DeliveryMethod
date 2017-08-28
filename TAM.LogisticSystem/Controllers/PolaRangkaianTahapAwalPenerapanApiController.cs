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
    [Route("api/v1/polarangkaiantahapawalpenerapan")]
    public class PolaRangkaianTahapAwalPenerapanApiController : Controller
    {
        private readonly PolaRangkaianTahapAwalPenerapanService PolaRangkaianTahapAwalPenerapanService;

        public PolaRangkaianTahapAwalPenerapanApiController(PolaRangkaianTahapAwalPenerapanService polaRangkaianTahapAwalPenerapanService)
        {
            this.PolaRangkaianTahapAwalPenerapanService = polaRangkaianTahapAwalPenerapanService;
        }

        /// <summary>
        /// digunakan untuk mengambil data-data untuk UI Pola Rangkaian Tahap Awal Penerapan
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var AllData = await PolaRangkaianTahapAwalPenerapanService.GetDataProcessHeadMapping();
            return Ok(AllData);
        }

        /// <summary>
        /// digunakan untuk insert data ke database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PolaRangkaianTahapAwalPenerapanViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var recordAffected = await PolaRangkaianTahapAwalPenerapanService.Add(model.RoutingDictionaryHeadCode, model.CarType);
            if (recordAffected < 1)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok("Success to Insert Data");
        }
    }
}
