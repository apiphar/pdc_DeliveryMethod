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
    public class ColourAPIController : Controller
    {
        private ColourService ColourMan;

        public ColourAPIController(ColourService ColourMan)
        {
            this.ColourMan = ColourMan;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await ColourMan.GetAll();
            return Ok(model);
        }
        

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ColourCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var data = 0;

            if (model.ColorType == "Interior")
            {
                data = await ColourMan.AddInterior(model.ColorCode, model.IndonesianName, model.EnglishName);
            }
            else if (model.ColorType == "Exterior")
            {
                data = await ColourMan.AddExterior(model.ColorCode, model.IndonesianName, model.EnglishName);
            }
            if (data == 0)
            {
                return BadRequest("Kode Warna sudah terdaftar");
            }
            return Ok(data);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] ColourCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            var inentity = await ColourMan.GetInterior(id);
            if (inentity != null)
            {
                if (model.ColorType.Equals("Interior"))
                {
                    await ColourMan.UpdateInterior(inentity, model.IndonesianName, model.EnglishName);
                }else
                {
                    await ColourMan.RemoveInterior(inentity);
                    await ColourMan.AddExterior(model.ColorCode, model.IndonesianName, model.EnglishName);
                }
                
            }
            else
            {
                var exentity = await ColourMan.GetExterior(id);
                if (exentity != null)
                {
                    if (model.ColorType.Equals("Exterior"))
                    {
                        await ColourMan.UpdateExterior(exentity, model.IndonesianName, model.EnglishName);
                    }
                    else
                    {
                        await ColourMan.RemoveExterior(exentity);
                        await ColourMan.AddInterior(model.ColorCode, model.IndonesianName, model.EnglishName);
                    }

                }else
                {
                    return BadRequest();
                }

            }
            return Ok();
        }

        [HttpDelete("delete/{id}/{colortype}")]
        public async Task<IActionResult> Delete(string id, string colortype)
        {
            if (colortype == "Interior")
            {
                var entity = await ColourMan.GetInterior(id);

                if (entity == null)
                {
                    return BadRequest("Data tidak valid");
                }

                var isSuccess = await ColourMan.RemoveInterior(entity);
                if (isSuccess != 1)
                {
                    return BadRequest("Data masih digunakan");
                }

            }
            else if (colortype == "Exterior")
            {
                var entity = await ColourMan.GetExterior(id);

                if (entity == null)
                {
                    return BadRequest("Data tidak valid");
                }

                var isSuccess = await ColourMan.RemoveExterior(entity);
                if (isSuccess != 1)
                {
                    return BadRequest("Data masih digunakan");
                }
            }

            return Ok();
        }
    }
}
