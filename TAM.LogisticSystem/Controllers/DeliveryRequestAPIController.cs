using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class DeliveryRequestApiController : Controller
    {

        public DeliveryRequestApiController(DeliveryRequestService deliveryRequestService)
        {
            this.DeliveryRequestService = deliveryRequestService;   
        }

        private readonly DeliveryRequestService DeliveryRequestService;

        [HttpGet("GetConfirmationCode")]
        public IActionResult GetConfirmationCode()
        {
            var confirmationCode = this.DeliveryRequestService.GenerateConfirmationCode();
            return Ok(confirmationCode);
        }

        [HttpGet("GetLocationType")]
        public async Task<IActionResult> GetLocationType()
        {
            var locationType = await this.DeliveryRequestService.GetLocationType();
            return Ok(locationType);
        }

        [HttpGet("GetReturnPdcDate")]
        public IActionResult GetReturnPdcDate(ReturnPdcDateModel ReturnPdcDateModel)
        {
            var returnPdcDate = this.DeliveryRequestService.GenerateReturnPdcDate(ReturnPdcDateModel);
            return Ok(returnPdcDate);
        }

        [HttpGet("GetOtherPdcLocation")]
        public async Task<IActionResult> GetOtherPdcLocation()
        {
            var otherPdcLocation = await this.DeliveryRequestService.GetOtherPdcLocation();
            return Ok(otherPdcLocation);
        }

        [HttpGet("GetAllLocationName")]
        public async Task<IActionResult> GetAllLocationName()
        {
            var locationName = await this.DeliveryRequestService.GetAllLocationName();
            return Ok(locationName);
        }

        [HttpGet("GetAllLocationAddress")]
        public async Task<IActionResult> GetAllLocationAddress()
        {
            var locationAddress = await this.DeliveryRequestService.GetAllLocationAddress();
            return Ok(locationAddress);
        }

        [HttpGet("GetAllDeliveryCarRequest")]
        public async Task<IActionResult> GetAllCar()
        {
            var deliveryRequestCarModel = await this.DeliveryRequestService.GetAllDeliveryCar();
            return Ok(deliveryRequestCarModel);

        }

        [HttpGet("GetAllDeliveryRequestPageView")]
        public async Task<IActionResult> GetAllDeliveryRequest()
        {
            var deliveryRequest = await this.DeliveryRequestService.GetAllDeliveryRequestPageView();
            return Ok(deliveryRequest);

        }

        [HttpGet("GetSequentialNumber/{branchCode}/{tipeDR}")]
        public async Task<IActionResult> GetSequentialNumber(string branchCode, string tipeDR)
        {
            var sequentialNumber = await this.DeliveryRequestService.GetSequentialNumber(branchCode, tipeDR);
            return Ok(sequentialNumber);

        }

        [HttpPost("Normal")]
        public async Task<IActionResult> CreateDeliveryRequestNormal([FromBody]DeliveryRequestNormalCreateModel deliveryRequestNormalCreateModel)
        {
            var isError = 1;

            if (ModelState.IsValid == false)
            {
                return BadRequest(isError);
            }
            isError = await this.DeliveryRequestService.CreateDeliveryNormalRequest(deliveryRequestNormalCreateModel);

            return Ok(isError);
        }

        [HttpPost("SelfPick")]
        public async Task<IActionResult> CreateDeliveryRequestSelfPick([FromBody]DeliveryRequestSelfPickCreateModel deliveryRequestSelfPickCreateModel)
        {
            var isError = 1;

            if (ModelState.IsValid == false)
            {
                return BadRequest(isError);
            }

            isError = await this.DeliveryRequestService.CreateDeliverySelfPickRequest(deliveryRequestSelfPickCreateModel);
            return Ok(isError);
        }

        [HttpPost("DirectDelivery")]
        public async Task<IActionResult> CreateDeliveryRequestDirectDelivery([FromBody]DeliveryRequestDirectDeliveryCreateModel deliveryRequestDirectDeliveryCreateModel)
        {
            var isError = 1;

            if (ModelState.IsValid == false)
            {
                return BadRequest(isError);
            }

            isError = await this.DeliveryRequestService.CreateDeliveryDirectDeliveryRequest(deliveryRequestDirectDeliveryCreateModel);

            return Ok(isError);
        }

        [HttpPost("TransitToOthers/SelfPickToOthers")]
        public async Task<IActionResult> CreateDeliveryRequestTransitToOthersSelfPickToOthers([FromBody]DeliveryRequestTransitToOthersSelfPickToOthersCreateModel deliveryRequestTransitToOthersSelfPickToOthersCreateModel)
        {
            var isError = 1;

            if (ModelState.IsValid == false)
            {

                return BadRequest(isError);
            }
            isError = await this.DeliveryRequestService.CreateDeliveryTransitToOthersSelfPickToOthersRequest(deliveryRequestTransitToOthersSelfPickToOthersCreateModel);

            return Ok(isError);
        }

        [HttpPost("TransitToOthers/Normal/ReturnToPDC")]
        public async Task<IActionResult> CreateDeliveryTransitToOthersNormalReturnToPdcRequest([FromBody]DeliveryRequestTransitToOthersNormalReturnToPdcCreateModel deliveryRequestTransitToOthersNormalReturnToPdcCreateModel)
        {
            var isError = 1;

            if (ModelState.IsValid == false)
            {
                return BadRequest(isError);
            }

            isError = isError = await this.DeliveryRequestService.CreateDeliveryTransitToOthersNormalReturnToPdcRequest(deliveryRequestTransitToOthersNormalReturnToPdcCreateModel);

            return Ok(isError);
        }

        [HttpPost("TransitToOthers/Normal/SelfPickFromOthers")]
        public async Task<IActionResult> CreateDeliveryTransitToOthersNormalSelfPickFromOthersRequest([FromBody]DeliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel)
        {
            var isError = 1;

            if (ModelState.IsValid == false)
            {
                return BadRequest(isError);
            }

            isError = await this.DeliveryRequestService.CreateDeliveryTransitToOthersNormalSelfPickFromOthersRequest(deliveryRequestTransitToOthersNormalSelfPickFromOthersCreateModel);

            return Ok(isError);
        }
    }
}
