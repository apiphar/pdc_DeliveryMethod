using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/v1/[controller]")]
    public class MdpApiController : Controller
    {
        private readonly MdpApiServices service;

        public MdpApiController(MdpApiServices service)
        {
            this.service = service;
        }

        // TIE: START
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody]List<MdpApiModel> listMdpApiModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var aa = await this.service.InsertAllMDPModel(listMdpApiModel);
        //        if (aa.Count()==1)
        //        {
        //            return Ok(aa);
        //        }
        //        else
        //        {
        //            return BadRequest(aa);
        //        }

        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        // TIE: END
    }
}
