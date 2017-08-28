using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class MasterCompanyAPIController : Controller
    {
        private readonly MasterCompanyService MasterCompanyService;

        public MasterCompanyAPIController(MasterCompanyService masterCompanyService)
        {
            this.MasterCompanyService = masterCompanyService;
        }

        [HttpGet("Companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companiesData = await this.MasterCompanyService.GetCompanies();
            return Ok(companiesData);
        }

        [HttpGet("Dealers")]
        public async Task<IActionResult> GetDealers()
        {
            var dealersData = await this.MasterCompanyService.GetDealers();
            return Ok(dealersData);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]MasterCompanyInsertUpdateModel masterCompanyInsertUpdateModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            await MasterCompanyService.Update(masterCompanyInsertUpdateModel);
            return Ok();
        }
    }
}
