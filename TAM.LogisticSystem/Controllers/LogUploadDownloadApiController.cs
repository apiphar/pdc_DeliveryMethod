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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class LogUploadDownloadApiController : Controller
    {
        private LogUploadDownloadService LogUploadDownloadService;

        public LogUploadDownloadApiController(LogUploadDownloadService LogUploadDownloadService)
        {
            this.LogUploadDownloadService = LogUploadDownloadService;
        }

        // TIE: START
        //[Route("GetAllLogUploadDownload")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllLogUploadDownload()
        //{
        //    var Data = await LogUploadDownloadService.GetAllLogUploadDownload();
        //    return Ok(Data);
        //}
        //[Route("GetLastLog/{master}")]
        //[HttpGet]
        //public async Task<IActionResult> GetLastLog(string master)
        //{
        //    var Data = await LogUploadDownloadService.GetLastLog(master);
        //    return Ok(Data);
        //}
        // TIE: END
    }
}
