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
    public class DccpReadinessVolumeApiController : Controller
    {
        private readonly DccpReadinessVolumeService dccpReadinessVolumeService;

        public DccpReadinessVolumeApiController(DccpReadinessVolumeService dccpReadinessVolumeService)
        {
            this.dccpReadinessVolumeService = dccpReadinessVolumeService;
        }
        // GET: api/values

        /// <summary>
        ///  Get all Data from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var GetAllDccpInfo = await this.dccpReadinessVolumeService.GetAllDccpVolume();
            return Ok(GetAllDccpInfo);
        }

        /// <summary>
        /// Edit Dccp Readiness volume unit adjust
        /// </summary>
        /// <param name="dccpReadinessVolumeModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DccpReadinessVolumeViewModel dccpReadinessVolumeModel)
        {
            if (ModelState.IsValid)
            {
                await this.dccpReadinessVolumeService.EditDccp(dccpReadinessVolumeModel);
                var refreshData = await this.dccpReadinessVolumeService.GetAllDccpVolume();
                return Ok(refreshData);
            }
            else
            {
                return BadRequest();
            }
            
        }
        /// <summary>
        /// Delete Dccp Volume
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Post(int id)
        {
            await this.dccpReadinessVolumeService.Delete(id);
            var AllDCCPVolume = await this.dccpReadinessVolumeService.GetAllDccpVolume();
            return Ok(AllDCCPVolume);
        }
    }
}
