using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/LocationTypeApi")]
    [Authorize]
    public class LocationTypeApiController : Controller
    {
        private readonly LocationTypeService _LocationTypeService;

        public LocationTypeApiController(LocationTypeService locationTypeService)
        {
            this._LocationTypeService = locationTypeService;
        }
        /// <summary>
        /// get all location type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var locationTypeList = await _LocationTypeService.GetAllLocationType();
            return Ok(locationTypeList);
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LocationTypeViewModel locationTypeModel)
        {
            var isExist = await _LocationTypeService.IsLocationTypeExist(locationTypeModel.LocationTypeCode);
            if (ModelState.IsValid == false)
            {
                return BadRequest("INVALID");
            }
            if (isExist)
            {
                return BadRequest("EXIST");
            }
            await _LocationTypeService.AddNewLocationType(locationTypeModel);
            return Ok();
        }
        /// <summary>
        /// Update location type
        /// </summary>
        /// <param name="locationTypeModel"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<IActionResult> Put([FromBody]LocationTypeViewModel locationTypeModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            await _LocationTypeService.UpdateLocationType(locationTypeModel);
            return Ok();
        }
        /// <summary>
        /// Delete location type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _LocationTypeService.RemoveLocationType(id);
            return Ok();
        }
    }
}
