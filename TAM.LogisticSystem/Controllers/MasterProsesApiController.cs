using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/masterproses")]
    public class MasterProsesApiController : Controller
    {
        private MasterProsesService MasterProsesService;

        public MasterProsesApiController(MasterProsesService MasterProsesService)
        {
            this.MasterProsesService = MasterProsesService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = MasterProsesService.GetRoutingMasterList();

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MasterProsesViewModel model)
        {
            if (ModelState.IsValid)
            {
                MasterProsesService.Save(model, User.Identity.Name);
                return Ok();
            }
            else return BadRequest("Data tidak valid");
        }

        [HttpPut("{code}")]
        public IActionResult Put(string code, [FromBody] MasterProsesViewModel model)
        {
            if (ModelState.IsValid)
            {
                MasterProsesService.Update(code, model, User.Identity.Name);
                return Ok();
            }
            else return BadRequest("Data tidak valid");
        }

        [HttpDelete("{code}")]
        public void Delete(string code)
        {
            MasterProsesService.Delete(code);
        }
    }
}
