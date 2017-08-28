using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class MasterRitasePriceApiController : Controller
    {
        public MasterRitasePriceApiController(MasterRitasePriceService masterRitasePriceService)
        {
            _MasterRitasePriceService = masterRitasePriceService;
        }
        private readonly MasterRitasePriceService _MasterRitasePriceService;

        // TIE: START
        //// GET: api/values
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var RitaseData = await _MasterRitasePriceService.GetAllRitaseData();
        //    if (RitaseData.Count < 1)
        //    {
        //        return BadRequest();
        //    } else
        //    {
        //        return Ok(RitaseData);
        //    }
        //}

        //[HttpGet("GetDeliveryVendor")]
        //public async Task<IActionResult> GetDeliveryVendor()
        //{
        //    var data = await _MasterRitasePriceService.GetDeliveryVendorCode();
        //    if (data.Count < 1)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        return Ok(data);
        //    }
        //}

        //[HttpGet("GetDeliveryMethod")]
        //public async Task<IActionResult> GetDeliveryMethod()
        //{
        //    var data = await _MasterRitasePriceService.GetDeliveryMethodCode();
        //    if (data.Count < 1)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        return Ok(data);
        //    }
        //}

        //[HttpGet("GetCityLegCode")]
        //public async Task<IActionResult> GetCityLegCode()
        //{
        //    var data = await _MasterRitasePriceService.GetCityLegCode();
        //    if (data.Count < 1)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        return Ok(data);
        //    }
        //}

        //[HttpGet("GetCurrencySymbol")]
        //public async Task<IActionResult> GetCurrencySymbol()
        //{
        //    var data = await _MasterRitasePriceService.GetCurrencySymbol();
        //    if (data.Count < 1)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        return Ok(data);
        //    }
        //}



        //// POST api/values
        //[HttpPost("AddData")]
        //public async Task<IActionResult> Post([FromBody]MasterRitasePriceInputModel data)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    } else {
        //        await _MasterRitasePriceService.AddData(data);
        //        return Ok();
        //    }         
        //}

        //// PUT api/values/5
        //[HttpPut("UpdateData")]
        //public async Task<IActionResult> Put([FromBody] MasterRitasePriceEditModel value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }  else
        //    {
        //        await _MasterRitasePriceService.UpdateData(value);
        //        return Ok();
        //    }
        //}

        //// DELETE api/values/5
        //[HttpDelete("DeleteData/{id}")]
        //public async Task<IActionResult>  Delete(int id)
        //{
        //    await _MasterRitasePriceService.DeleteData(id);
        //    return Ok();
        //}
        // TIE: END
    }
}
