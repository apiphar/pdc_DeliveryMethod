using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class MasterWarnaVehicleApiController : Controller
    {
        private readonly MasterWarnaVehicleService MasterWarnaVehicleService;

        public MasterWarnaVehicleApiController(MasterWarnaVehicleService masterWarnaVehicleService)
        {
            this.MasterWarnaVehicleService = masterWarnaVehicleService;
        }

        /// <summary>
        /// Get All Vehicle Color Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vehicleColorList = await MasterWarnaVehicleService.GetAllVehicleColor();
            return Ok(vehicleColorList);
        }

        /// <summary>
        /// Get All Brand Data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllKodeBrand")]
        public async Task<IActionResult> GetAllKodeBrand()
        {
            var kodeBrandList = await MasterWarnaVehicleService.GetAllKodeBrand();
            return Ok(kodeBrandList);
        }

        /// <summary>
        /// Get All Model Data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllKodeModel")]
        public async Task<IActionResult> GetAllKodeModel()
        {
            var kodeModelList = await MasterWarnaVehicleService.GetAllKodeModel();
            return Ok(kodeModelList);
        }

        /// <summary>
        /// Get All Color Data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllKodeWarna")]
        public async Task<IActionResult> GetAllKodeWarna()
        {
            var kodeWarnaList = await MasterWarnaVehicleService.GetAllKodeWarna();
            return Ok(kodeWarnaList);
        }

        // TIE: START
        ///// <summary>
        ///// Create Vehicle Color by KodeWarnaVehicle
        ///// </summary>
        ///// <param name="vehicleColorModel"></param>
        ///// <returns></returns>
        // [HttpPost]
        // public async Task<IActionResult> Post([FromBody]MasterWarnaVehicleCreateModel vehicleColorModel)
        // {
        //     var invalid = await MasterWarnaVehicleService.IsVehicleColorExist(vehicleColorModel.KodeWarnaVehicle);
        //     if (ModelState.IsValid == false || invalid)
        //     {
        //         return Ok("FALSE");
        //     }
        //     await MasterWarnaVehicleService.AddNewVehicleColor(vehicleColorModel);
        //     return Ok("TRUE");
        // }

        // /// <summary>
        // /// Edit Vehicle Color Data by KodeWarnaVehicle
        // /// </summary>
        // /// <param name="vehicleColorModel"></param>
        // /// <returns></returns>
        // [HttpPost("edit")]
        // public async Task<IActionResult> Put([FromBody]MasterWarnaVehicleCreateModel vehicleColorModel)
        // {
        //     var valid = await MasterWarnaVehicleService.IsVehicleColorExist(vehicleColorModel.KodeWarnaVehicle);
        //     if (ModelState.IsValid == false || valid == false)
        //     {
        //         return Ok("FALSE");
        //     }
        //     await MasterWarnaVehicleService.UpdateVehicleColor(vehicleColorModel);
        //     return Ok("TRUE");
        // }

        // /// <summary>
        // /// Delete Vehicle Color Data by KodeWarnaVehicle
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [HttpDelete("delete/{id}")]
        // public async Task<IActionResult> Delete(string id)
        // {
        //     var deleted = await MasterWarnaVehicleService.RemoveVehicleColor(id);
        //     return Ok();
        // }
        // TIE: END
    }
}
