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
    [Route("api/v1/cartype")]

    public class CarTypeApiController : Controller
    {
        private CarTypeService carTypeService;

        public CarTypeApiController(CarTypeService CarTypeService)
        {
            this.carTypeService = CarTypeService;
        }

        //TIE: START
        [Route("GetAllCarType")]
        [HttpGet]
        public IActionResult GetAllCarType()
        {
            var dataCarType = carTypeService.getAllCarType();
            return Ok(dataCarType);
        }

        [Route("GetAllCarSeries")]
        [HttpGet]
        public IActionResult GetAllSeries()
        {
            var dataSeries = carTypeService.getAllCarSeries();
            return Ok(dataSeries);
        }

        [Route("getAllAfiCarType")]
        [HttpGet]
        public IActionResult GetAllModel()
        {
            var dataCategory = carTypeService.getAllAfiCarType();
            return Ok(dataCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rowsAffected = await carTypeService.Add(model);
            if (rowsAffected != 1)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CarTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rowsAffected = await carTypeService.Update(model.Katashiki, model.Suffix, model);
            if (rowsAffected != 1)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string katashiki, string suffix)
        {
            var rowsAffected = await carTypeService.Remove(katashiki, suffix);
            if (rowsAffected != 1)
            {
                return BadRequest();
            }
            return Ok();
        }
        //TIE: END
    }
}






