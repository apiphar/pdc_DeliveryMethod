using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class MasterLeadTimeLocationAPIController : Controller
    {
        private readonly MasterLeadTimeLocationService MasterLeadTimeLocationService;

        public MasterLeadTimeLocationAPIController(MasterLeadTimeLocationService masterLeadTimeLocationService)
        {
            this.MasterLeadTimeLocationService = masterLeadTimeLocationService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var masterLeadTimeLocationsData = await this.MasterLeadTimeLocationService.GetMasterLeadTimeLocationData();
            return Ok(masterLeadTimeLocationsData);
        }
        
        [HttpGet("GetLocationsCode")]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await this.MasterLeadTimeLocationService.GetLocations();
            return Ok(locations);
        }

       [HttpGet("GetRoutingMasterData")]
        public async Task<IActionResult> GetRoutes()
        {
            var routingMasterData = await this.MasterLeadTimeLocationService.GetRoutes();
            return Ok(routingMasterData);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MasterLeadTimeLocationInsertUpdateModel masterLeadTimeLocationInsertUpdateModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if (this.MasterLeadTimeLocationService.CheckExistingCode(masterLeadTimeLocationInsertUpdateModel.LocationCode, masterLeadTimeLocationInsertUpdateModel.ProcessMasterCode) != null)
            {
                return BadRequest("Kombinasi Lokasi dan Kode Rute telah terdaftar");
            }
            await this.MasterLeadTimeLocationService.InsertMasterLeadTimeLocationData(masterLeadTimeLocationInsertUpdateModel);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]MasterLeadTimeLocationInsertUpdateModel masterLeadTimeLocationInsertUpdateModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            await this.MasterLeadTimeLocationService.UpdateMasterLeadTimeLocationData(masterLeadTimeLocationInsertUpdateModel);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string locationCode, string processMasterCode)
        {
            await this.MasterLeadTimeLocationService.DeleteMasterLeadTimeLocationData(locationCode, processMasterCode);
            return Ok();
        }
    }
}
