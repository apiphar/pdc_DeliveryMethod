using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.TANGO.Models;
using TAM.TANGO.Services;

namespace TAM.TANGO.Controllers
{
    [Route("api/pdi")]
    public class PdiApiController : Controller
    {
        private PDIService pdiService;

        public PdiApiController(PDIService pdiService)
        {
            this.pdiService = pdiService;
        }

        [HttpPost]
        [Route("InspectionChecklist")]
        public IActionResult InspectionChecklist(InspectionParameter inspectionMasterParameters)
        {
            var data = pdiService.GetInspectionMasterChecklist(inspectionMasterParameters);
            return Ok(data);
        }

        [HttpPost]
        [Route("ActiveUser")]
        public IActionResult ActiveUser()
        {
            // Using fake data, because there no existing API for get current user information
            return Ok(new ActiveUser()
            {
                Username = "Mbem",
                Department = "VLD Department",
                Division = "VLD Division"
            });
        }

        [HttpPost]
        [Route("VehicleData")]
        public async Task<IActionResult> VehicleData(string frameNumber)
        {
            var vehicleData = await pdiService.GetVehicleData(frameNumber);
            return Ok(vehicleData);
        }

        [HttpPost]
        [Route("SubmitInspection")]
        public async Task<IActionResult> SubmitInspection(InspectionData inspectionData)
        {
            var data = await pdiService.SubmitInspectionData(inspectionData);
            return Ok(data);
        }
    }
}
