using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    public class LogisticVendorApiController : Controller
    {
        private LogisticVendorService LogisticVendorMan;

        public LogisticVendorApiController(LogisticVendorService LogisticVendorMan)
        {
            this.LogisticVendorMan = LogisticVendorMan;
        }
        
        [Route("logistic-vendor/API/get-all")]
        public async Task<IActionResult> GetAll()
        {
            var model = await LogisticVendorMan.GetAll();
            return Ok(model);
        }

        [Route("logistic-vendor/API/get-location")]
        public async Task<IActionResult> GetLocation()
        {
            var model = await LogisticVendorMan.GetLocation();
            return Ok(model);
        }

        [HttpPost("logistic-vendor/API/create")]
        public async Task<IActionResult> Create([FromBody] DeliveryVendorCreateModel model)
        {
            

            if (ModelState.IsValid == false)
            {
                
                return BadRequest();
            }

            await this.LogisticVendorMan.Add(model);
            return Ok();
        }


        [HttpPost("logistic-vendor/API/edit")]
        public async Task<IActionResult> Edit([FromBody] DeliveryVendorCreateModel model)
        {
            var entity = await LogisticVendorMan.Get(model.DeliveryVendorCode);
            

            if (ModelState.IsValid == false)
            {
               
                return BadRequest();
            }

            await this.LogisticVendorMan.Update(entity, model);
            return Ok();
        }


        [HttpDelete("logistic-vendor/API/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var isError = 0;

            var entity = await LogisticVendorMan.Get(id);
            if (entity == null)
            {
                isError = 1;
                return NotFound(isError);
            }

            isError = await LogisticVendorMan.Remove(entity);
            return Ok(isError);
        }
    }
}
