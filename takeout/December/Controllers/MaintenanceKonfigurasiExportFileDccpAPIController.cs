using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class MaintenanceKonfigurasiExportFileDccpAPIController : Controller
    {
        private readonly MaintenanceKonfigurasiExportFileDccpService maintenanceKonfigurasiExportFileDccpService;

        public MaintenanceKonfigurasiExportFileDccpAPIController(MaintenanceKonfigurasiExportFileDccpService maintenanceKonfigurasiExportFileDccpService)
        {
            this.maintenanceKonfigurasiExportFileDccpService = maintenanceKonfigurasiExportFileDccpService;
        }

        /// <summary>
        /// get All model for init
        /// </summary>
        /// <returns></returns>
        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allModelConfig = await this.maintenanceKonfigurasiExportFileDccpService.GetAllConfig();
            return Ok(allModelConfig);
        }


        /// <summary>
        /// Add new MOdel Controller
        /// </summary>
        /// <param name="AddedModel"></param>
        /// <returns></returns>
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MaintenanceKonfigurasiExportFileDccpPostViewModel AddedModel)
        {
            if (ModelState.IsValid)
            {
                await this.maintenanceKonfigurasiExportFileDccpService.AddConfig(AddedModel);
                var allModelConfig = await this.maintenanceKonfigurasiExportFileDccpService.GetAllConfig();
                return Ok(allModelConfig);
            }else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update Data for excel config
        /// </summary>
        /// <param name="UpdateModel"></param>
        /// <returns></returns>
        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]MaintenanceKonfigurasiExportFileDccpViewModel UpdateModel)
        {
            if (ModelState.IsValid)
            {
                await this.maintenanceKonfigurasiExportFileDccpService.UpdateData(UpdateModel);
                var allModelConfig = await this.maintenanceKonfigurasiExportFileDccpService.GetAllConfig();
                return Ok(allModelConfig);
            }else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Delete excel config 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this.maintenanceKonfigurasiExportFileDccpService.DeleteSingleData(id) > 0)
            {
                var allModelConfig = await this.maintenanceKonfigurasiExportFileDccpService.GetAllConfig();
                return Ok(allModelConfig);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
