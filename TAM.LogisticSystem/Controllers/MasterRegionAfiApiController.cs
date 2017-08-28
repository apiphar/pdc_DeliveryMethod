using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("/api/v1/[controller]")]
    public class MasterRegionAfiApiController : Controller
    {
        private readonly MasterRegionAfiService MasterRegionAfiService;
        private readonly LogisticDbContext LogisticDbContext;
        public MasterRegionAfiApiController(MasterRegionAfiService masterRegionAfiService, LogisticDbContext logisticDbContext)
        {
            this.MasterRegionAfiService = masterRegionAfiService;
            this.LogisticDbContext = logisticDbContext;
        }

        [HttpGet("getalldataregion")]
        public async Task<IActionResult> GetAllRegionData()
        {
            var regionAfi = await this.MasterRegionAfiService.GetAllRegionData();
            return Ok(regionAfi);
        }

        [HttpGet("getalldataafi")]
        public async Task<IActionResult> GetAllRegionAFIData()
        {
            var regionAfi = await this.MasterRegionAfiService.GetAllRegionAfiData();
            return Ok(regionAfi);
        }
        [HttpGet("getpostcode")]
        public async Task<IActionResult> GetPostCode()
        {
            var regionAfi = await this.MasterRegionAfiService.GetPostCode();
            return Ok(regionAfi);
        }

        [HttpPost("AddData")]
        public async Task<IActionResult> AddRegionAfiData(string posCode,[FromBody]MasterRegionAFIViewModel regionAfi)
        {
            if (ModelState.IsValid == false)
            {
                return View(regionAfi);
            }
            await MasterRegionAfiService.AddRegionAfiData(posCode,regionAfi);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRegionAfiData(string id)
        {
            await MasterRegionAfiService.DeleteRegionAfiData(id);
            return Ok();
        }

        [HttpPost("UpdateRegionAfi")]
        public async Task<IActionResult> UpdateRegionAfiData([FromBody] MasterRegionAfiPostViewModel Update)
        {
            await MasterRegionAfiService.UpdateRegionAfiData(Update);
            return Ok();
        }
    }
}
