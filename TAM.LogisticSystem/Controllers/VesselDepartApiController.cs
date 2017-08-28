using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class VesselDepartApiController : Controller
    {
        public VesselDepartApiController(VesselDepartService vesselDepartService)
        {
            this.VesselDepartService = vesselDepartService;
        }
        private readonly VesselDepartService VesselDepartService;

        // Get all
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this.VesselDepartService.GetAll();
            return Ok(data);
        }

        // Get unit list for vessel depart detail
        [HttpGet("UnitList/{voyageNumber}")]
        public async Task<IActionResult> GetUnitList(string voyageNumber)
        {
            var data = await this.VesselDepartService.GetUnitList(true, voyageNumber);
            return Ok(data);
        }

        // Update VoyageStatus to departed
        [HttpPut("DepartVessel")]
        public async Task<IActionResult> DepartVessel([FromBody]VesselDepartSendViewModel vessel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.VesselDepartService.DepartVesselByVoyage(vessel);
            return Ok();
        }
    }
}
