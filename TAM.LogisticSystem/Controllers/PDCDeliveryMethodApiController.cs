using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class PDCDeliveryMethodApiController : Controller
    {
        public PDCDeliveryMethodService pdcDeliveryService;

        public PDCDeliveryMethodApiController(PDCDeliveryMethodService pdcDeliveryService)
        {
            this.pdcDeliveryService = pdcDeliveryService;
        }

        [Route("GetId/{locationCode}/{branchCode}")]
        [HttpGet]
        public async Task<IActionResult> GetId(string locationCode, string branchCode)
        {
            var data = await pdcDeliveryService.Get(locationCode, branchCode);
            return Ok(data);
        }

        [Route("GetAllData")]
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            var data = await pdcDeliveryService.GetAll();
            return Ok(data);
        }

        [Route("GetBranches")]
        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var data = await pdcDeliveryService.GetBranches();
            return Ok(data);
        }

        [Route("GetLocations")]
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var data = await pdcDeliveryService.GetLocations();
            return Ok(data);
        }

        [Route("GetDeliveries")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveries()
        {
            var data = await pdcDeliveryService.GetDeliveries();
            return Ok(data);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<PDCDeliveryCreateUpdateViewModel> model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var message = await this.pdcDeliveryService.Add(model);
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string locationCode, string branchCode)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            var data = await this.pdcDeliveryService.Delete(locationCode, branchCode);
            return Ok(data);

        }
    }
}
