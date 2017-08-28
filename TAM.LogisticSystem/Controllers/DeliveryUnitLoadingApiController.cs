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
    [Route("api/v1/[controller]")]
    public class DeliveryUnitLoadingApiController : Controller
    {

        public DeliveryUnitLoadingApiController(DeliveryUnitLoadingService deliveryUnitLoadingService)
        {
            _DeliveryUnitLoadingService = deliveryUnitLoadingService;
        }
        private readonly DeliveryUnitLoadingService _DeliveryUnitLoadingService;
        // GET: api/values

        [HttpGet("GetUnitLoadingDetailModels/{voyageNumber}")]
        public async Task<IActionResult> Get(string voyageNumber)
        {
            if (string.IsNullOrEmpty(voyageNumber)==false)
            {
                var unitLoadingDetailModels = await _DeliveryUnitLoadingService.GetAllUnitLoadingDetailModel(voyageNumber);
                if (unitLoadingDetailModels == null)
                {
                    return BadRequest("Data Not Found");
                }
                else
                {
                    return Ok(unitLoadingDetailModels);
                }
            }
            else {
                return BadRequest("Voyage No Tidak ditemukan");
            }
            


        }

        [HttpGet("GetFrameNumbers/{voyageNumber}")]
        public async Task<IActionResult> GetFrameNumbers(string voyageNumber)
        {
            if (string.IsNullOrEmpty(voyageNumber))
            {
                return BadRequest("Voyage Number tidak boleh kosong");
            }
            else
            {
                var frameNumbers = await _DeliveryUnitLoadingService.GetFrameNumber(voyageNumber);
                if (frameNumbers.Count < 0)
                {
                    return BadRequest("Wrong Voyage Number");
                }
                else
                {
                    return Ok(frameNumbers);
                }
            }

        }

        // GET api/values/5
        [HttpGet("GetUnitLoadingModels")]
        public async Task<IActionResult> GetUnitLoadingModels()
        {
            var unitLoadingModels = await _DeliveryUnitLoadingService.GetUnitLoadingModel();
            if (unitLoadingModels == null)
            {
                return BadRequest("Data Tidak Ditemukan");
            }
            else
            {
                return Ok(unitLoadingModels);
            }
        }


        [HttpPut("UpdateFrameNumber")]
        public async Task<IActionResult> UpdateData([FromBody] DeliveryUnitLoadingFrameNumberUpdate voyageNumber)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data Tidak Valid");
            }
            else
            {
                await _DeliveryUnitLoadingService.LoadedData(voyageNumber);
                var unitLoadingModels = await _DeliveryUnitLoadingService.GetUnitLoadingModel();
                return Ok(unitLoadingModels);
            }
        }


    }
}
