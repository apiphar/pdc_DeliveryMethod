using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class DeliveryLegLeadTimeAPIController : Controller
    {
        private readonly DeliveryLegLeadTimeService DeliveryLegLeadTimeService;
        public DeliveryLegLeadTimeAPIController(DeliveryLegLeadTimeService deliveryLegLeadTimeService)
        {
            this.DeliveryLegLeadTimeService = deliveryLegLeadTimeService;
        }

        /// <summary>
        /// Get all data from database
        /// </summary>
        /// <returns></returns>
        [HttpGet("{deliveryLegCode}")]
        public async Task<IActionResult> Get(string deliveryLegCode)
        {
            var deliveryLeadData = await this.DeliveryLegLeadTimeService.GetDeliveryLeadData(deliveryLegCode);

            return Ok(deliveryLeadData);
        }

        /// <summary>
        /// Get all DeliveryMethodCode from table DeliveryLeg (untuk combobox)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeliveryMethodCode")]
        public async Task<IActionResult> GetDeliveryMethodCode()
        {
            var deliveryMethodCodes = await this.DeliveryLegLeadTimeService.GetDeliveryMethodCode();

            return Ok(deliveryMethodCodes);
        }
        /// <summary>
        /// Untuk mendapatkan LocationFrom dan LocationTo berdasarkan DeliveryLegCode yang dikirim Master delivery leg
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/v1/DeliveryLegLeadTimeApi/GetLocation/{code}")]
        public async Task<IActionResult> GetLocation(string code)
        {
            var location = await this.DeliveryLegLeadTimeService.GetLocation(code);

            return Ok(location);
        }

        /// <summary>
        /// Insert one delivery lead time data to table DeliveryLeadTime
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InsertDeliveryLegLeadTimeViewModel model)
        {
            var username = User.Identity.Name;
            if (ModelState.IsValid == false)
                if (ModelState.IsValid == false)
                {
                    return BadRequest("Data tidak valid");
                }
            var validate = await this.DeliveryLegLeadTimeService.Validate(model.DeliveryLegCode, model.DeliveryMethodCode);
            if (validate == true)
            {
                return BadRequest("Kode Moda telah terdaftar");
            }
            await DeliveryLegLeadTimeService.AddDeliveryLeadData(model.DeliveryLegCode, model.DeliveryMethodCode, model.LeadMinutes);
            return Ok();
        }

        /// <summary>
        /// Update one delivery lead time data from table DeliveryLeadTime
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody]DeliveryLegLeadTimeViewModel model)
        {
            var username = User.Identity.Name;
            if (ModelState.IsValid == false)
                if (ModelState.IsValid == false)
                {
                    return BadRequest("Data tidak valid");
                }
            var validate = await this.DeliveryLegLeadTimeService.Validate(model.DeliveryLegCode, model.DeliveryMethodCode);
            if (validate == true)
            {
                return BadRequest("Kode Moda telah terdaftar");
            }
            await DeliveryLegLeadTimeService.UpdateDeliveryLead(id, model.DeliveryMethodCode, model.LeadMinutes);
            return Ok();
        }

        /// <summary>
        /// Delete one delivery lead time data from table DeliveryLeadTime
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rowAffected = await DeliveryLegLeadTimeService.DeleteDeliveryLead(id);
            if (rowAffected < 1)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok();
        }

    }
}
