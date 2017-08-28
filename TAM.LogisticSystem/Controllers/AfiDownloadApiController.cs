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
    public class AfiDownloadApiController : Controller
    {
        public AfiDownloadService AfiDownloadService;
        public AfiDownloadApiController(AfiDownloadService AfiDownloadService)
        {
            this.AfiDownloadService = AfiDownloadService;
        }

        [Route("GetSubmission")]
        [HttpPost]
        public IActionResult GetSubmission([FromBody] AfiDownloadSearch model)
        {
            var data = this.AfiDownloadService.GetSubmission(model);
            return Ok(data);
        }
        [Route("GetAllBranch")]
        [HttpGet]
        public async Task<IActionResult> GetAllBranch()
        {
            var data = await this.AfiDownloadService.GetAllBranch();
            return Ok(data);
        }

        [Route("Download")]
        [HttpPost]
        public IActionResult Download([FromBody] List<AfiGridDownload> model)
        {
            var row = this.AfiDownloadService.ProcessTAM(ref model);
            if(row < 1)
            {
                return BadRequest("Data gagal disimpan");
            }
            var result = this.AfiDownloadService.GetGenerateTextResult(model);
            return Ok(result);
        }
    }
}
