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
    public class AfiHOApprovalApiController : Controller
    {
        public AfiHOApprovalService afiHOApprovalService;
        public AfiHOApprovalApiController(AfiHOApprovalService afiHOApprovalService)
        {
            this.afiHOApprovalService = afiHOApprovalService;
        }


        [HttpPost]
        [Route("GetAFIHoApproval")]
        public IActionResult GetAFIHoApproval([FromBody] AfiHOApprovalSearch model)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var data = afiHOApprovalService.GetAFIHoApproval(model);
            return Ok(data.Result);
        }

        [HttpPost]
        [Route("ReturnToOutlet")]
        public IActionResult ReturnToOutlet([FromBody] List<AfiHOApprovalSubmission> model)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var row = afiHOApprovalService.ReturnToOutlet(model);
            if (row == 0)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }
        [HttpPost]
        [Route("ProcessToTam")]
        public IActionResult ProcessToTam([FromBody] List<AfiHOApprovalSubmission> model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            var row = afiHOApprovalService.ProcessToTam(model);
            if (row == 0)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }
        [HttpGet]
        [Route("GetAllBranch")]
        public async Task<IActionResult> GetAllBranch()
        {
            var data = await afiHOApprovalService.GetAllBranch();
            return Ok(data);
        }
    }
}
