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
    [Authorize]
    [Route("api/v1/[controller]")]
    public class PDILeadTimeConfigurationApiController : Controller
    {
        public PDILeadTimeConfigurationApiController(PDILeadTimeConfigurationService pdiLeadTimeConfigurationService)
        {
            this.PDILeadTimeConfigurationService = pdiLeadTimeConfigurationService;
        }

        private readonly PDILeadTimeConfigurationService PDILeadTimeConfigurationService;

        [HttpGet("PDILeadTimeConfigurations")]
        public async Task<IActionResult> GetPDILeadTimeConfigurations()
        {
            var pdiLeadTimeConfigurations = await this.PDILeadTimeConfigurationService.GetPDILeadTimeConfigurations();
            var carModels = await this.PDILeadTimeConfigurationService.GetCarModels();
            var carSeries = await this.PDILeadTimeConfigurationService.GetCarSeries();
            var carTypes = await this.PDILeadTimeConfigurationService.GetCarTypes();
            var katashikis = await this.PDILeadTimeConfigurationService.GetKatashikis();
            var locations = await this.PDILeadTimeConfigurationService.GetLocations();
            var allData = new PDILeadTimeConfigurationAllGetModel
            {
                PDILeadTimeConfigurationViewModels = pdiLeadTimeConfigurations,
                PDILeadTimeConfigurationCarModels = carModels,
                PDILeadTimeConfigurationCarSeries = carSeries,
                PDILeadTimeConfigurationCarTypes = carTypes,
                PDILeadTimeConfigurationKatashikis = katashikis,
                PDILeadTimeConfigurationLocations = locations
            };
            return Ok(allData);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<PDILeadTimeConfigurationCreateModel> createPDILeadTimeConfigs)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            foreach(var pdiLeadTimeConfig in createPDILeadTimeConfigs)
            {
                if(await PDILeadTimeConfigurationService.CheckCombinationExistence(pdiLeadTimeConfig.LocationCode, pdiLeadTimeConfig.Katashiki, pdiLeadTimeConfig.Suffix) == false)
                {
                    return BadRequest("Kombinasi Lokasi, Katashiki, dan Suffix telah terdaftar");
                }
            }

            await this.PDILeadTimeConfigurationService.CreatePDILeadTimeConfigurations(createPDILeadTimeConfigs);

            var pdiLeadTimeConfigurations = await this.PDILeadTimeConfigurationService.GetPDILeadTimeConfigurations();
            return Ok(pdiLeadTimeConfigurations);
        }

        [HttpPut("{pdiLeadTimeId}")]
        public async Task<IActionResult> Update(int pdiLeadTimeId, [FromBody] PDILeadTimeConfigurationUpdateModel updatedPDILeadTimeConfig)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            await this.PDILeadTimeConfigurationService.UpdatePDILeadTimeConfiguration(pdiLeadTimeId, updatedPDILeadTimeConfig);

            var pdiLeadTimeConfigurations = await this.PDILeadTimeConfigurationService.GetPDILeadTimeConfigurations();
            return Ok(pdiLeadTimeConfigurations);
        }

        [HttpDelete("{pdiLeadTimeId}")]
        public async Task<IActionResult> Delete(int pdiLeadTimeId)
        {
            await this.PDILeadTimeConfigurationService.DeletePDILeadTimeConfiguration(pdiLeadTimeId);

            var pdiLeadTimeConfigurations = await this.PDILeadTimeConfigurationService.GetPDILeadTimeConfigurations();
            return Ok(pdiLeadTimeConfigurations);
        }
    }
}
