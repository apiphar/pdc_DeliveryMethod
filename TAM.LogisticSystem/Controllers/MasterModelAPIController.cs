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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/[controller]")]
    public class MasterModelAPIController : Controller
    {

        private MasterModelService masterModelService;


        public MasterModelAPIController(MasterModelService masterModelService)
        {
            this.masterModelService = masterModelService;
        }

        // GET: api/values
        [HttpGet("/mastermodel/GetDataMasterModel")]
        public IActionResult GetDataMasterModel()
        {
            var data = masterModelService.GetMasterModelData();
            return Ok(data);
        }

        [HttpGet("/mastermodel/GetDropdownBrand")]
        public IActionResult GetDropdownBrand()
        {
            var data = masterModelService.GetBrand();
            return Ok(data);
        }


        [HttpGet("/mastermodel/GetDropdownManufacturing")]
        public IActionResult GetDropdownManufacturing()
        {
            var data = masterModelService.GetManufacturing();
            return Ok(data);
        }

        [HttpGet("/mastermodel/CekPola/{pola}")]
        public IActionResult CekPola(string pola)
        {
            var data = masterModelService.CekMOdelCode(pola);
            return Ok(data);
        }

        [HttpPost("/mastermodel/create")]
        public  IActionResult Create([FromBody] MasterModelCreateOrUpdate model)
        {
            
            var result =  masterModelService.Add(model.MasterModelId, model.Name, model.BrandCode, model.PlantCode);
            if(result == 0)
            {
                return BadRequest("Kode Model telah terdaftar");
            }
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("/mastermodel/edit/{id}")]
        public IActionResult Edit(string id, [FromBody] MasterModelCreateOrUpdate model)
        {
            
            var entity = masterModelService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var result = masterModelService.Update(entity, model.Name, model.BrandCode,model.PlantCode);

            return Ok(result);
        }

     

        // DELETE api/values/5
        [HttpDelete("/mastermodel/delete/{id}")]
        public IActionResult Delete(string id)
        {
            var entity = masterModelService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            var result = masterModelService.Remove(entity);

            return Ok(result);
        }
    }
}
