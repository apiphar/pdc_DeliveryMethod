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
    [Route("/api/v1/lokasi")]
    public class LocationAPIController : Controller
    {

        private readonly LocationService LocationService;
        public LocationAPIController(LocationService locationService)
        {
            this.LocationService = locationService;
        }
        [HttpGet("getalldata")]
        public async Task<IActionResult> GetAllData()
        {
            var data = await this.LocationService.GetAllData();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationViewModel model)
        {
            var isError = 0;

            if (ModelState.IsValid == false)
            {
                isError = 1;
                return BadRequest(isError);
            }
            isError = await LocationService.Add(model);

            if (isError == 2)
            {
                return BadRequest(isError);
            }

            return Ok(isError);

        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] LocationViewModel model)
        {
            var isError = 0;

            if (ModelState.IsValid == false)
            {
                isError = 1;
                return BadRequest(isError);
            }

            isError = await LocationService.Update(model);

            if (isError == 2)
            {
                return BadRequest(isError);
            }

            return Ok(isError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var isError = 0;

            isError = await LocationService.Remove(id);

            return Ok(isError);
        }
    }
}
