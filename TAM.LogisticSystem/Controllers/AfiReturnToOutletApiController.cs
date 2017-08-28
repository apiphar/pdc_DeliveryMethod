using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class AfiReturnToOutletApiController : Controller
    {
        public AfiReturnToOutletService AfiReturnToOutletService;

        public AfiReturnToOutletApiController(AfiReturnToOutletService afiReturnToOutletService)
        {
            this.AfiReturnToOutletService = afiReturnToOutletService;
        }
        
        /// <summary>
        /// return to outlet tombol Cari 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [Route("GetRequest")]
        [HttpPost]
        public async Task<IActionResult> GetRequest([FromBody]AfiRequestRevisiSearchRO model)
        {
            var Data = await this.AfiReturnToOutletService.GetRequest(model);

            return Ok(Data);
        }
        
    }
}
