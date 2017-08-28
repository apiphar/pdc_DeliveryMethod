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
    [Route("api/v1/polarangkaiantahapawal")]
    public class PolaRangkaianTahapAwalApiController : Controller
    {
        private readonly PolaRangkaianTahapAwalService PolaRangkaianTahapAwalService;

        public PolaRangkaianTahapAwalApiController(PolaRangkaianTahapAwalService polaRangkaianTahapAwalService)
        {
            this.PolaRangkaianTahapAwalService = polaRangkaianTahapAwalService;
        }

        /// <summary>
        /// digunakan untuk mengambil data-data untuk UI Pola Rangkaian Tahap Awal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var AllData = await PolaRangkaianTahapAwalService.GetDataProcessHeadTemplate();
            return Ok(AllData);
        }

        /// <summary>
        /// digunakan untuk insert header dan detail ke database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PolaRangkaianTahapAwalInsertModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var recordAffected = await PolaRangkaianTahapAwalService.Add(model.Header, model.Detail);
            if (recordAffected < 1)
            {
                return BadRequest("Incorrect request model");
            }
            return Ok("Success to Insert Data");
        }

        /// <summary>
        /// untuk delete data dari routing dictionary head
        /// </summary>
        /// <param name="routingDictionaryHeadCode"></param>
        /// <returns></returns>
        [HttpDelete("{routingDictionaryHeadCode}")]
        public async Task<IActionResult> DeleteHeader(string routingDictionaryHeadCode)
        {
            var recordAffected = await PolaRangkaianTahapAwalService.RemoveHeader(routingDictionaryHeadCode);
            if (recordAffected < 1)
            {
                return BadRequest("Incorrect request model");
            }
            return Ok("Success to Delete Data");
        }
    }
}
