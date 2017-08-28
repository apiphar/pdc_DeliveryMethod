using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class MasterPlafondAPIController : Controller
    {
        private readonly MasterPlafondService MasterPlafondService;
        public MasterPlafondAPIController(MasterPlafondService masterPlafondService)
        {
            this.MasterPlafondService = masterPlafondService;
        }

        // TIE: START
        ////to get all plafond data from table PlafondMaster
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var plafonds = await this.MasterPlafondService.GetPlafondData();

        //    return Ok(plafonds);
        //}
        ////untuk mendapatkan company code yang belum diinsert ke table PlafondMaster
        //[HttpGet]
        //[Route("GetCompanyKode")]
        //public async Task<IActionResult> GetCompanyKode()
        //{
        //    var companyCodes = await this.MasterPlafondService.GetCompanyKode();

        //    return Ok(companyCodes);
        //}
        ////to insert one plafond data to table PlafondMaster
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]MasterPlafondViewModel model)
        //{
        //    var check = await MasterPlafondService.CheckCodeCompany(model.KodeCompany);

        //    if(check == true)
        //    {
        //        return BadRequest("Kode Company sudah terdaftar");
        //    }
        //    if(model.Plafond <= 0)
        //    {
        //        return BadRequest("Data Tidak Valid");
        //    }
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest("Data Tidak Valid");
        //    }
        //    await MasterPlafondService.AddPlafondData(model.KodeCompany, model.Plafond);
        //    return Ok();
        //}
        ////to Update one plafond data from table PlafondMaster
        //[HttpPost("{id}")]
        //public async Task<IActionResult> Edit(int id,[FromBody] decimal plafond)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest("Data Tidak Valid");
        //    }
        //    await MasterPlafondService.UpdatePlafond(id, plafond);
        //    return Ok();
        //}
        ////to delete one plafond data from table PlafondMaster
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    int rowAffected = await MasterPlafondService.DeletePlafond(id);
        //    if (rowAffected < 1)
        //    {
        //        return BadRequest("Data Tidak Valid");
        //    }
        //    return Ok();
        //}
        // TIE: END
    }
}
