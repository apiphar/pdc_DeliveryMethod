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
    [Route("api/v1/[controller]")]
    public class ManufacturingApiController : Controller
    {
        private readonly MasterManufacturingService _service;
    
        public ManufacturingApiController(MasterManufacturingService manufacturingService)
        {
            this._service = manufacturingService;
        }

        /// <summary>
        /// Get All Data from DB to the view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allData = await this._service.GetAll();
            return Ok(allData);
        }       

        /// <summary>
        /// Update The Data 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
       [Route("Update")]
       [HttpPost]
        public async Task<IActionResult> Update([FromBody]ManufacturingUpdateViewModel data)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            await this._service.Update(data);

            var allData = await this._service.GetAll();
            return Ok(allData);
        }

        /// <summary>
        /// Save Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ManufacturingUpdateViewModel data)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            else
            {
                var isSuccess = await this._service.Save(data);
                if (isSuccess == 0)
                {
                    return BadRequest("Kode Manufacturing telah terdaftar");
                }
                var allData = await this._service.GetAll();
                return Ok(allData);
            } 
        }

        /// <summary>
        /// Delete The Data
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var isSuccess = await this._service.Delete(code);
            if (isSuccess != 1)
            {
                return BadRequest();
            }

            var allData = await this._service.GetAll();
            return Ok(allData);
        }
    }
}
