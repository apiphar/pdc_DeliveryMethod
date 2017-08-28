using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using TAM.Passport.SDK;
using Newtonsoft.Json;
using TAM.LogisticSystem.Entities;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/dms")]
    public class DMSController : Controller
    {
        private readonly DMSService DMSService;
        private readonly WebEnvironmentService EnvMan;
        private readonly PassportApi PassportApiSDK;

        public DMSController(DMSService dmsService, WebEnvironmentService envMan)
        {
            DMSService = dmsService;
            EnvMan = envMan;

            PassportApiSDK = new PassportApi(envMan.TamPassportUrl, envMan.TamPassportAppId);
        }

        [HttpPost("FindVehicleLocation")]
        public async Task<IActionResult> FindLocation([FromBody]DMSFindVehicleModel model)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            var findLocation = await DMSService.GetLocation(model);
            if (findLocation == null)
            {
                return NotFound("Frame No. tidak terdaftar");
            }
            else if (findLocation.Count == 0)
            {
                return BadRequest("Frame No. tidak ditemukan");
            }
            return Ok(findLocation);
        }

        [HttpPost("SendDOUpdate")]
        public async Task<IActionResult> SendDOUpdate([FromBody] int deliveryOrderDetailId)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            var model = await DMSService.GetDOUpdateAsync(deliveryOrderDetailId);

            var json = JsonConvert.SerializeObject(model);
            var token = PassportApiSDK.LoginAsync(EnvMan.TLSUser, EnvMan.TLSPassword);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(EnvMan.DMSDomainUrl, content);

            if (response.IsSuccessStatusCode == false)
            {
                return StatusCode((int)response.StatusCode);
            }

            await DMSService.DOSentToDMSAsync(deliveryOrderDetailId);
            return Ok();
        }

        [HttpPost("SendDriverConfirmation")]
        public async Task<IActionResult> SendDriverConfirmation([FromBody]DMSSendDriverConfirmation dmsSendDriverConfirmation)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest("Pastikan data benar");
            }

            var errorMessage = await DMSService.DriverConfirmationValidation(dmsSendDriverConfirmation);
            if (errorMessage != null)
            {
                return BadRequest(errorMessage);
            }

            var retrieveDriverConfirmation = await DMSService.GetDriverConfirmation(dmsSendDriverConfirmation);
            if (retrieveDriverConfirmation == null)
            {
                return NotFound("Frame Number tidak diterdaftar");
            }
            return Ok(retrieveDriverConfirmation);
        }

        [HttpPost("CalculateDeliveryRequest")]
        public async Task<IActionResult> CalculateDeliveryRequest([FromBody]DMSSendBookingUnitKalkulasiPDD dmsSendBookingUnitKalkulasiPDD)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest("Pastikan data benar");
            }

            var errorMessage = DMSService.CalculateDeliveryRequestValidation(dmsSendBookingUnitKalkulasiPDD);
            if (errorMessage != null)
            {
                return BadRequest(errorMessage);
            }

            if (dmsSendBookingUnitKalkulasiPDD.DeliveryCategory != 1 && dmsSendBookingUnitKalkulasiPDD.DeliveryCategory != 2)
            {
                return BadRequest("Delivery Category salah");
            }

            var retrieveBookingUnitKalkulasiPDD = await DMSService.GetBookingUnitKalkulasiPDD(dmsSendBookingUnitKalkulasiPDD);
            if (retrieveBookingUnitKalkulasiPDD == null)
            {
                return NotFound("Frame Number tidak diterdaftar");
            }
            return Ok(retrieveBookingUnitKalkulasiPDD);
        }

        [HttpPost("ConfirmDeliveryRequest")]
        public async Task<IActionResult> ConfirmDeliveryRequest([FromBody]DMSSendBookingUnitKonfirmasiPDD dmsSendBookingUnitKonfirmasiPDD)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest("Pastikan data benar");
            }

            var errorMessage = DMSService.ConfirmationDeliveryRequestValidation(dmsSendBookingUnitKonfirmasiPDD);
            if (errorMessage != null)
            {
                return BadRequest(errorMessage);
            }

            var vehicle = new Vehicle();

            if (!string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.FrameNumber))
            {
                vehicle = await DMSService.GetVehicleAsync(dmsSendBookingUnitKonfirmasiPDD.FrameNumber);
            }
            else if (!string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.RRN) && dmsSendBookingUnitKonfirmasiPDD.DTPLOD.HasValue)
            {
                vehicle = await DMSService.GetVehicleAsync(dmsSendBookingUnitKonfirmasiPDD.RRN, dmsSendBookingUnitKonfirmasiPDD.DTPLOD.Value);
            }

            if (vehicle == null)
            {
                return NotFound("Frame Number tidak terdaftar");
            }

            var retrieveBookingUnitKonfirmasiPDD = await DMSService.GetBookingUnitKonfirmasiPDD(vehicle, dmsSendBookingUnitKonfirmasiPDD);
            if (retrieveBookingUnitKonfirmasiPDD == null)
            {
                return BadRequest("Tanggal Requested PDD tidak boleh lebih kecil dari tanggal Earliest PDD");
            }
            return Ok(retrieveBookingUnitKonfirmasiPDD);
        }

        [HttpPost("CalculateSwapping")]
        public async Task<IActionResult> QuotationOB([FromBody] DMSQuotationOBModel model)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            var quotationOB = await DMSService.GetQuotationOB(model);
            if (quotationOB == null)
            {
                return NotFound("Frame Number tidak diterdaftar");
            }
            return Ok(quotationOB);
        }

        [HttpPost("ConfirmSwapping")]
        public async Task<IActionResult> ApprovalReceivedQuotation([FromBody] DMSApprovalReceivedQuotationModel model)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            var quotationOB = await DMSService.GetApprovalReceivedQuotation(model);
            if (quotationOB == null)
            {
                return NotFound("Frame Number cannot be found");
            }
            return Ok(quotationOB);
        }

        [HttpPost("SendKaroseri")]
        public async Task<IActionResult> SendKaroseri([FromBody]DMSSendKaroseriModel model)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            var validateDeliveryRequest = await DMSService.ValidateOpenDR(model.FrameNumber);
            if (validateDeliveryRequest == true)
            {
                return BadRequest("Mobil masih dalam status Open DR");
            }

            if (model.DeliveryRequestType != 3)
            {
                return BadRequest("Tipe Delivery bukan karoseri");
            }

            var validateKaroseriLocation = await DMSService.ValidateKaroseriLocation(model.LocationCode);
            if (validateKaroseriLocation == false)
            {
                return BadRequest("Lokasi bukan karoseri");
            }

            if (model.PickUpDateTime.CompareTo(model.DeliveryRequestDate) < 0)
            {
                return BadRequest("Pickup date tidak boleh lebih awal dari Delivery request date");
            }

            if (model.VehicleReturnDate.CompareTo(model.PickUpDateTime) < 0)
            {
                return BadRequest("Vehicle return date tidak boleh lebih awal dari Pickup date");
            }

            var validatePickupDate = await DMSService.ValidatePickupDate(model.FrameNumber, model.PickUpDateTime.ToUniversalTime());
            if (validatePickupDate == false)
            {
                return BadRequest("Pickup date yang direquest lebih awal dari Earliest PDD");
            }

            if (model.DriverReturnType != 1 && model.DriverReturnType != 2)
            {
                return BadRequest("Driver Type harus diisi 1 atau 2");
            }

            if (model.DriverReturnType == 1 && string.IsNullOrEmpty(model.DriverId))
            {
                return BadRequest("Driver ID harus terisi");
            }

            if (model.DriverReturnType == 1 && string.IsNullOrEmpty(model.DriverName))
            {
                return BadRequest("Driver Name harus terisi");
            }

            var karoseri = await DMSService.SendKaroseri(model);
            return Ok(karoseri);
        }

        [HttpPost("SendUrgentMemo")]
        public async Task<IActionResult> SentUrgentMemo([FromBody]DMSSendKaroseriModel model)
        {
            var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
            if (authorizationKey[0] != "Bearer")
            {
                return BadRequest("User not authorized");
            }

            var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

            if (claims == null)
            {
                return BadRequest("User not authorized");
            }

            var validateDeliveryRequest = await DMSService.ValidateOpenDR(model.FrameNumber);
            if (validateDeliveryRequest == false)
            {
                return BadRequest("Frame Number tidak terdaftar");
            }

            if (model.DeliveryRequestType < 1 || model.DeliveryRequestType > 4)
            {
                return BadRequest("Tipe Delivery Request hanya boleh diubah dengan angka : 1, 2, 3, dan 4");
            }

            var validateUrgentMemoLocation = await DMSService.ValidateUrgentMemoLocation(model.LocationCode);
            if (validateUrgentMemoLocation == false)
            {
                return BadRequest("Lokasi bukan Borrowing");
            }

            if (model.PickUpDateTime.CompareTo(model.DeliveryRequestDate) < 0)
            {
                return BadRequest("Pick Up Date tidak boleh lebih awal dari Delivery Request Date");
            }

            if (model.VehicleReturnDate.CompareTo(model.PickUpDateTime) < 0)
            {
                return BadRequest("Vehicle Return Date tidak boleh lebih awal dari Pick Up Date");
            }

            var validatePickupDate = await DMSService.ValidatePickupDate(model.FrameNumber, model.PickUpDateTime.ToUniversalTime());
            if (validatePickupDate == false)
            {
                return BadRequest("Pick Up Date tidak boleh lebih awal dari Earliest PDD");
            }

            if (model.DriverReturnType != 1 && model.DriverReturnType != 2)
            {
                return BadRequest("Driver Type harus diisi 1 atau 2");
            }

            if (model.DriverReturnType == 1 && string.IsNullOrEmpty(model.DriverId))
            {
                return BadRequest("Driver ID harus diisi");
            }

            if (model.DriverReturnType == 1 && string.IsNullOrEmpty(model.DriverName))
            {
                return BadRequest("Driver Name harus diisi");
            }

            var urgentMemo = await DMSService.SentUrgentMemo(model);
            return Ok(urgentMemo);
        }

        //#region WithoutPassportAuthentication
        //[HttpPost("FindVehicleLocationWithoutAuthentication")]
        //public async Task<IActionResult> FindVehicleLocationWithoutAuthentication([FromBody]DMSFindVehicleModel model)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var findLocation = await DMSService.GetLocation(model);
        //    if (findLocation == null)
        //    {
        //        return NotFound("Frame No. tidak terdaftar");
        //    }
        //    else if (findLocation.Count == 0)
        //    {
        //        return BadRequest("Frame No. in belum memiliki pola rangkaian");
        //    }
        //    return Ok(findLocation);
        //}

        //[HttpPost("SendDOUpdateWithoutAuthentication")]
        //public async Task<IActionResult> SendDOUpdateWithoutAuthentication([FromBody] int deliveryOrderDetailId)
        //{
        //    var model = await DMSService.GetDOUpdateAsync(deliveryOrderDetailId);

        //    var json = JsonConvert.SerializeObject(model);
        //    var token = PassportApiSDK.LoginAsync(EnvMan.TLSUser, EnvMan.TLSPassword);

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await client.PostAsync(EnvMan.DMSDomainUrl, content);

        //    if (response.IsSuccessStatusCode == false)
        //    {
        //        return StatusCode((int)response.StatusCode);
        //    }

        //    await DMSService.DOSentToDMSAsync(deliveryOrderDetailId);
        //    return Ok();
        //}

        //[HttpPost("SendDriverConfirmationWithoutAuthentication")]
        //public async Task<IActionResult> SendDriverConfirmationWithoutAuthentication([FromBody]DMSSendDriverConfirmation dmsSendDriverConfirmation)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var retrieveDriverConfirmation = await DMSService.GetDriverConfirmation(dmsSendDriverConfirmation);
        //    if (retrieveDriverConfirmation == null)
        //    {
        //        return NotFound("Frame Number cannot be found");
        //    }
        //    return Ok(retrieveDriverConfirmation);
        //}

        //[HttpPost("CalculateDeliveryRequestWithoutAuthentication")]
        //public async Task<IActionResult> CalculateDeliveryRequestWithoutAuthentication([FromBody]DMSSendBookingUnitKalkulasiPDD dmsSendBookingUnitKalkulasiPDD)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    if (dmsSendBookingUnitKalkulasiPDD.DeliveryCategory != 1 && dmsSendBookingUnitKalkulasiPDD.DeliveryCategory != 2)
        //    {
        //        return BadRequest("Delivery Category salah");
        //    }

        //    var retrieveBookingUnitKalkulasiPDD = await DMSService.GetBookingUnitKalkulasiPDD(dmsSendBookingUnitKalkulasiPDD);
        //    if (retrieveBookingUnitKalkulasiPDD == null)
        //    {
        //        return NotFound("Frame Number cannot be found");
        //    }
        //    return Ok(retrieveBookingUnitKalkulasiPDD);
        //}

        //[HttpPost("ConfirmDeliveryRequestWithoutAuthentication")]
        //public async Task<IActionResult> ConfirmDeliveryRequestWithoutAuthentication([FromBody]DMSSendBookingUnitKonfirmasiPDD dmsSendBookingUnitKonfirmasiPDD)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var vehicle = new Vehicle();

        //    if (!string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.FrameNumber))
        //    {
        //        vehicle = await DMSService.GetVehicleAsync(dmsSendBookingUnitKonfirmasiPDD.FrameNumber);
        //    }
        //    else if (!string.IsNullOrEmpty(dmsSendBookingUnitKonfirmasiPDD.RRN) && dmsSendBookingUnitKonfirmasiPDD.DTPLOD.HasValue)
        //    {
        //        vehicle = await DMSService.GetVehicleAsync(dmsSendBookingUnitKonfirmasiPDD.RRN, dmsSendBookingUnitKonfirmasiPDD.DTPLOD.Value);
        //    }

        //    if (vehicle == null)
        //    {
        //        return NotFound("Frame Number tidak terdaftar");
        //    }

        //    var retrieveBookingUnitKonfirmasiPDD = await DMSService.GetBookingUnitKonfirmasiPDD(vehicle, dmsSendBookingUnitKonfirmasiPDD);
        //    if (retrieveBookingUnitKonfirmasiPDD == null)
        //    {
        //        return BadRequest("Tanggal Requested PDD lebih kecil dari tanggal Earliest PDD");
        //    }
        //    return Ok(retrieveBookingUnitKonfirmasiPDD);
        //}

        //[HttpPost("CalculateSwappingWithoutAuthentication")]
        //public async Task<IActionResult> CalculateSwappingWithoutAuthentication([FromBody] DMSQuotationOBModel model)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var quotationOB = await DMSService.GetQuotationOB(model);
        //    if (quotationOB == null)
        //    {
        //        return NotFound("Frame Number cannot be found");
        //    }
        //    return Ok(quotationOB);
        //}

        //[HttpPost("ConfirmSwappingWithoutAuthentication")]
        //public async Task<IActionResult> ConfirmSwappingWithoutAuthentication([FromBody] DMSApprovalReceivedQuotationModel model)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var quotationOB = await DMSService.GetApprovalReceivedQuotation(model);
        //    if (quotationOB == null)
        //    {
        //        return NotFound("Frame Number cannot be found");
        //    }
        //    return Ok(quotationOB);
        //}

        //[HttpPost("SendKaroseriWithoutAuthentication")]
        //public async Task<IActionResult> SendKaroseriWithoutAuthentication([FromBody]DMSSendKaroseriModel model)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var validateDeliveryRequest = await DMSService.ValidateOpenDR(model.FrameNumber);
        //    if (validateDeliveryRequest == true)
        //    {
        //        return BadRequest("Mobil masih dalam status Open DR");
        //    }

        //    if (model.DeliveryRequestType != 3)
        //    {
        //        return BadRequest("Tipe Delivery bukan karoseri");
        //    }

        //    var validateKaroseriLocation = await DMSService.ValidateKaroseriLocation(model.LocationCode);
        //    if (validateKaroseriLocation == false)
        //    {
        //        return BadRequest("Lokasi bukan karoseri");
        //    }

        //    var validateDeliveryRequestDate = await DMSService.ValidateRequestDate(model.FrameNumber, model.PickUpDateTime.ToUniversalTime());
        //    if (validateDeliveryRequestDate == false)
        //    {
        //        return BadRequest("Delivery yang direquest lebih awal dari Earliest PDD");
        //    }

        //    if (model.DriverReturnType != 1 && model.DriverReturnType != 2)
        //    {
        //        return BadRequest("Driver Type ditolak");
        //    }

        //    var karoseri = await DMSService.SendKaroseri(model);
        //    return Ok(karoseri);
        //}

        //[HttpPost("SendUrgentMemoWithoutAuthentication")]
        //public async Task<IActionResult> SentUrgentMemoWithoutAuthentication([FromBody]DMSSendKaroseriModel model)
        //{
        //    //var authorizationKey = Request.Headers["Authorization"].ToString().Split(' ');
        //    //if (authorizationKey[0] != "Bearer")
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    //var claims = await PassportApiSDK.VerifyTokenAsync(authorizationKey[1]);

        //    //if (claims == null)
        //    //{
        //    //    return BadRequest("User not authorized");
        //    //}

        //    var validateDeliveryRequest = await DMSService.ValidateOpenDR(model.FrameNumber);
        //    if (validateDeliveryRequest == false)
        //    {
        //        return BadRequest("Delivery Request belum ada");
        //    }

        //    if (model.DeliveryRequestType < 1 && model.DeliveryRequestType > 4)
        //    {
        //        return BadRequest("Tipe Delivery tidak valid");
        //    }

        //    var validateUrgentMemoLocation = await DMSService.ValidateUrgentMemoLocation(model.LocationCode);
        //    if (validateUrgentMemoLocation == false)
        //    {
        //        return BadRequest("Lokasi bukan borrowing");
        //    }

        //    var validateDeliveryRequestDate = await DMSService.ValidateRequestDate(model.FrameNumber, model.PickUpDateTime.ToUniversalTime());
        //    if (validateDeliveryRequestDate == false)
        //    {
        //        return BadRequest("Delivery yang direquest lebih awal dari Earliest PDD");
        //    }

        //    if (model.DriverReturnType != 1 && model.DriverReturnType != 2)
        //    {
        //        return BadRequest("Driver Type ditolak");
        //    }

        //    var karoseri = await DMSService.SentUrgentMemo(model);
        //    return Ok(karoseri);
        //}
        //#endregion
    }
}
