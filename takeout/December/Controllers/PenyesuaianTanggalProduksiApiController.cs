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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class PenyesuaianTanggalProduksiApiController : Controller
    {
        private readonly PenyesuaianTanggalProduksiService penyesuaianTanggalProduksiService;

        public PenyesuaianTanggalProduksiApiController(PenyesuaianTanggalProduksiService penyesuaianTanggalProduksiService)
        {
            this.penyesuaianTanggalProduksiService = penyesuaianTanggalProduksiService;
        }
        /// <summary>
        /// GetAll Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allDataProduksi = await this.penyesuaianTanggalProduksiService.GetAllData();
            return Ok(allDataProduksi);
        }
        /// <summary>
        /// Create New Data
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PenyesuaianTanggalProduksiPostViewModel postModel)
        {
            await this.penyesuaianTanggalProduksiService.PostData(postModel);
            var allDataProduksi = await this.penyesuaianTanggalProduksiService.GetAllData();
            return Ok(allDataProduksi);
            
        }

        /// <summary>
        /// Editing Exisitng Data
        /// </summary>
        /// <param name="putModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]PenyesuaianTanggalProduksiPostViewModel putModel)
        {
            await this.penyesuaianTanggalProduksiService.UpdateData(putModel);
            var allDataProduksi = await this.penyesuaianTanggalProduksiService.GetAllData();
            return Ok(allDataProduksi);
        }

        /// <summary>
        /// Delete Selected Data
        /// </summary>
        /// <param name="plant"></param>
        /// <returns></returns>
        [HttpDelete("{plant}")]
        public async Task<IActionResult> Delete(string plant)
        {
            if(await this.penyesuaianTanggalProduksiService.DeleteData(plant) > 0)
            {
                var allDataProduksi = await this.penyesuaianTanggalProduksiService.GetAllData();
                return Ok(allDataProduksi);

            }
            else
            {
                return BadRequest();
            }
        }
    }
}
