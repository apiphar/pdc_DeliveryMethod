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
    public class LegPriceMasterApiController : Controller
    {

        public LegPriceMasterApiController(LegPriceMasterService legPriceMasterService)
        {
            this.LegPriceMasterService = legPriceMasterService;
        }

        private readonly LegPriceMasterService LegPriceMasterService;

        // GET: api/values
        [HttpGet("GetAllCityLegCost")]
        public async Task<IActionResult> GetAllCityLegCost()
        {
            var legPriceViewModel = await this.LegPriceMasterService.GetAllCityLegCost();
            return Ok(legPriceViewModel);
        }

        [HttpGet("GetAllDeliveryVendor")]
        public async Task<IActionResult> GetAllDeliveryVendor()
        {
            var deliveryVendorModel = await this.LegPriceMasterService.GetAllDeliveryVendor();
            return Ok(deliveryVendorModel);
        }

        [HttpGet("GetAllCityLeg")]
        public async Task<IActionResult> GetAllCityLeg()
        {
            var cityLegModel = await this.LegPriceMasterService.GetAllCityLeg();
            return Ok(cityLegModel);
        }

        [HttpGet("GetDeliveryMethod")]
        public async Task<IActionResult> GetDeliveryMethod()
        {
            var deliveryMethodList = await this.LegPriceMasterService.GetDeliveryMethod();
            return Ok(deliveryMethodList);
        }

        [HttpGet("GetAllCarSeries")]
        public async Task<IActionResult> GetAllCarSeries()
        {
            var carSeriesList = await this.LegPriceMasterService.GetAllCarSeries();
            return Ok(carSeriesList);
        }

        [HttpGet("GetAllCurrency")]
        public async Task<IActionResult> GetAllCurrency()
        {
            var currencyList = await this.LegPriceMasterService.GetAllCurrency();
            return Ok(currencyList);
        }

        // TIE: START
        //[HttpPost("Create")]
        //public async Task<IActionResult> Create([FromBody]LegPriceMasterCreateModel model)
        //{
        //    var isError = 0;

        //    if (ModelState.IsValid == false)
        //    {
        //        isError = 1;
        //        return Ok(isError);
        //    }
        //    isError = await LegPriceMasterService.Add(model);
        //    return Ok(isError);
        //}

        //[HttpPost("Edit/{id}")]
        //public async Task<IActionResult> Edit(string id, [FromBody] LegPriceMasterCreateModel model)
        //{
        //    var isError = 0;

        //    if (ModelState.IsValid == false)
        //    {
        //        isError = 1;
        //        return Ok(isError);
        //    }

        //    int recordAffected = await LegPriceMasterService.Update(id, model);
        //    if (recordAffected > 0)
        //    {
        //        TempData["Status"] = 1;
        //        TempData["Message"] = "Data berhasil disimpan";

        //        return Ok(isError);
        //    }
        //    else
        //    {
        //        TempData["Status"] = 2;
        //        TempData["Message"] = "Data gagal disimpan";
        //        isError = 1;
        //        var data = new LegPriceMasterViewModel
        //        {
        //            DeliveryMethodCode = model.DeliveryMethod.DeliveryMethodCode,
        //            DeliveryVendorCode = model.DeliveryVendor.DeliveryVendorCode,
        //            CarSeriesCode = model.CarSeries.CarSeriesCode,
        //            CityLegCode = model.CityLeg.CityLegCode,
        //            ValidDate = model.ValidDate,
        //        };
        //        return Ok(isError);
        //    }

        //}

        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    int rowsAffected = await LegPriceMasterService.Remove(id);

        //    return Ok();
        //}
        // TIE: END

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
