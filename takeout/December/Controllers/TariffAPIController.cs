using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/Tariff")]
    public class TariffAPIController : Controller
    {
        private readonly TariffService TariffMan;

        public TariffAPIController(TariffService TariffMan)
        {
            this.TariffMan = TariffMan;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TariffViewModel param)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await TariffMan.Add(param);
            return Ok() ;
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> Create(int id, [FromBody]TariffViewModel param)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await TariffMan.Update(id,param);
            return Ok();
        }
        public IActionResult GetData()
        {
            var Data = TariffMan.GetAllData();
            return Ok(Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int rowsAffected = await TariffMan.Remove(id);
            if (rowsAffected > 0)
            {
                TempData["Status"] = 1;
                TempData["Message"] = "Data has been deleted.";
            }
            else
            {
                TempData["Status"] = 2;
                TempData["Message"] = "Data cannot be deleted, because data is being used!";
            }

            return Ok(TempData);
        }
    }
}
