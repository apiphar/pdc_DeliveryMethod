using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/leadtimeby")]
    public class LeadTimeByApiController : Controller
    {
        private LeadTimeByService leadTimeByService;

        public LeadTimeByApiController(LeadTimeByService leadTimeByService)
        {
            this.leadTimeByService = leadTimeByService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var data = leadTimeByService.GetAll();

            return Ok(data);
        }
    }
}
