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
    //[Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Authorize]
    [Route("api/[controller]")]
    public class MasterModelSeriesAPIController : Controller
    {

        private MasterModelSeriesService masterModelSeriesService;

        public MasterModelSeriesAPIController(MasterModelSeriesService masterModelSeriesService)
        {
            this.masterModelSeriesService = masterModelSeriesService;
        }

        [HttpGet("/mastermodelseries/GetDataMasterSeries")]
        public IActionResult GetDataMasterSeries()
        {
            var data = masterModelSeriesService.MasterSeriesGetData();

            return Ok(data);
        }
        [HttpGet("/mastermodelseries/GetDropdownCarModel")]
        public IActionResult GetDropdownCarModel()
        {
            var data = masterModelSeriesService.GetCarModel();
            return Ok(data);
        }


        [HttpGet("/mastermodelseries/CekPola/{pola}")]
        public IActionResult CekPola(string pola)
        {
            var data = masterModelSeriesService.CekModelCode(pola);
            return Ok(data);
        }

        [HttpPost("/mastermodelseries/create")]
        public IActionResult Create([FromBody] MasterModelSeriesCreateOrUpdate model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var result = masterModelSeriesService.Add(model);

            return Ok(result);
        }



        [HttpPost("/mastermodelseries/edit/{id}")]
        public IActionResult Edit(string id, [FromBody] MasterModelSeriesCreateOrUpdate model)
        {
            var entity = masterModelSeriesService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var result = masterModelSeriesService.Update(entity, model.Name, model.carModelCode);
            if (result >= 1)
            {
                TempData["Success"] = "Success Update Data";
            }
            else
            {
                TempData["Error"] = "Error Update Data";
            }
            return Ok(result);
        }


      

        // DELETE api/values/5
        [HttpDelete("/mastermodelseries/delete/{id}")]
        public IActionResult Delete(string id)
        {
            var entity = masterModelSeriesService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            var result = masterModelSeriesService.Remove(entity);
            if (result >= 1)
            {
                TempData["Success"] = "Success Delete Data";
            }
            else
            {
                TempData["Error"] = "Error Delete Data";
            }
            return Ok(result);
        }
    }
}
