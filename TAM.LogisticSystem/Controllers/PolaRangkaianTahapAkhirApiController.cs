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
    [Route("api/v1/polarangkaiantahapakhir")]
    public class PolaRangkaianTahapAkhirApiController : Controller
    {
        private readonly PolaRangkaianTahapAkhirService PolaRangkaianTahapAkhirService;

        public PolaRangkaianTahapAkhirApiController(PolaRangkaianTahapAkhirService polaRangkaianTahapAkhirService)
        {
            this.PolaRangkaianTahapAkhirService = polaRangkaianTahapAkhirService;
        }

        /// <summary>
        /// digunakan untuk mengambil data-data untuk UI Pola Rangkaian Tahap Akhir
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var AllData = await PolaRangkaianTahapAkhirService.GetDataProcessTailTemplate();
            return Ok(AllData);
        }

        /// <summary>
        /// digunakan untuk insert header dan detail ke database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PolaRangkaianTahapAkhirInsertModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var recordAffected = await PolaRangkaianTahapAkhirService.Add(model.Header, model.Detail);
            if (recordAffected < 1)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok();
        }

        /// <summary>
        /// untuk delete data dari routing dictionary tail
        /// </summary>
        /// <param name="routingDictionaryTailCode"></param>
        /// <returns></returns>
        [HttpDelete("{routingDictionaryTailCode}")]
        public async Task<IActionResult> DeleteHeader(string routingDictionaryTailCode)
        {
            int recordAffected = recordAffected = await PolaRangkaianTahapAkhirService.RemoveHeader(routingDictionaryTailCode);
            if (recordAffected < 1)
            {
                return BadRequest("Data tidak valid");
            }
            return Ok();
        }
    }
}
