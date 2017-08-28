using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class PIODefaultLeadTimeConfigurationApiController : Controller
    {
        public PIODefaultLeadTimeConfigurationApiController(PIODefaultLeadTimeConfigurationService pioDefaultLeadTimeConfigurationService)
        {
            this.PIODefaultLeadTimeConfigurationService = pioDefaultLeadTimeConfigurationService;
        }

        private readonly PIODefaultLeadTimeConfigurationService PIODefaultLeadTimeConfigurationService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var configPage = await this.PIODefaultLeadTimeConfigurationService.GetPIODefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create([FromBody]CreatePIODefaultLeadTimeViewModel createDefaultLeadTime)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            if(await this.PIODefaultLeadTimeConfigurationService.CreatePIODefaultLeadTime(createDefaultLeadTime) == false)
            {
                return BadRequest("duplicate");
            }
            
            var configPage = await this.PIODefaultLeadTimeConfigurationService.GetPIODefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdatePIODefaultLeadTimeViewModel updateDefaultLeadTime)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.PIODefaultLeadTimeConfigurationService.UpdatePIODefaultLeadTime(updateDefaultLeadTime);
            var configPage = await this.PIODefaultLeadTimeConfigurationService.GetPIODefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string locationCode, int processForLineId, string routingMasterCode)
        {
            if (locationCode == null || processForLineId == 0 || routingMasterCode == null)
            {
                return BadRequest();
            }
            if (await this.PIODefaultLeadTimeConfigurationService.DeletePIODefaultLeadTime(routingMasterCode, locationCode, processForLineId) == false)
            {
                return BadRequest();
            }
            var configPage = await this.PIODefaultLeadTimeConfigurationService.GetPIODefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }
    }
}
