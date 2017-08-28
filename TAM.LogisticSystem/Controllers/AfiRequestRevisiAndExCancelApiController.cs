using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class AfiRequestRevisiAndExCancelApiController : Controller
    {
        public AfiRequestRevisiAndExCancelService AfiRequestRevisiAndExCancelService;
        public AfiRequestRevisiAndExCancelApiController(AfiRequestRevisiAndExCancelService afiRequestRevisiService)
        {
            this.AfiRequestRevisiAndExCancelService = afiRequestRevisiService;
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
            var Data = await this.AfiRequestRevisiAndExCancelService.GetRequest(model);

            return Ok(Data);
        }
    }
}
