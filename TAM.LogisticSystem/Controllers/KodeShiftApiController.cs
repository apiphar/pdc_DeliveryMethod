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
    [Authorize]
    [Route("api/v1/[controller]")]
    public class KodeShiftApiController : Controller
    {
        private readonly KodeShiftService kodeShiftService;

        public KodeShiftApiController(KodeShiftService kodeShiftService)
        {
            this.kodeShiftService = kodeShiftService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var Data = await this.kodeShiftService.GetData();
            return Ok(Data);
        }
    
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShiftCodeViewModel model)
        {     
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var data = await this.kodeShiftService.Get(model.ShiftCode);
            if (data != null)
            {
                return BadRequest("Kode shift sudah terdaftar");
            }
            await this.kodeShiftService.Add(model);
            return Ok();
        }
  
        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] ShiftCodeUpdateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid"); 
            }
            await this.kodeShiftService.Edit(id, model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await this.kodeShiftService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            await this.kodeShiftService.Remove(entity);
            return Ok();
        }
    }
}
