using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class SPUDefaultLeadTimeConfigurationApiController : Controller
    {
        private readonly SPUDefaultLeadTimeConfigurationService SPUDefaultLeadTimeConfigurationService;

        public SPUDefaultLeadTimeConfigurationApiController(SPUDefaultLeadTimeConfigurationService spuDefaultLeadTimeConfigurationService)
        {
            this.SPUDefaultLeadTimeConfigurationService = spuDefaultLeadTimeConfigurationService;
        }

        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var configPage = await this.SPUDefaultLeadTimeConfigurationService.GetSPUDefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }

        [ValidateModelAttribute]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]SPUDefaultLeadTimeConfigurationCreateViewModel createDefaultLeadTime)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.SPUDefaultLeadTimeConfigurationService.CreateSPUDefaultLeadTime(createDefaultLeadTime);
            var configPage = await this.SPUDefaultLeadTimeConfigurationService.GetSPUDefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] SPUDefaultLeadTimeConfigurationUpdateViewModel updateDefaultLeadTime)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.SPUDefaultLeadTimeConfigurationService.UpdateSPUDefaultLeadTime(updateDefaultLeadTime);
            var configPage = await this.SPUDefaultLeadTimeConfigurationService.GetSPUDefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string locationCode, int processForLineId, string routingMasterCode)
        {
            if (await this.SPUDefaultLeadTimeConfigurationService.DeleteSPUDefaultLeadTime(routingMasterCode, locationCode, processForLineId) == false)
            {
                return BadRequest();
            }
            var configPage = await this.SPUDefaultLeadTimeConfigurationService.GetSPUDefaultLeadTimeConfigurationPage();
            return Ok(configPage);
        }
    }
}
