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
    [Route("api/v1/DownloadDccpReadinessVolumeApi")]
    public class DownloadDccpReadinessVolumeApiController : Controller
    {
        public DownloadDccpReadinessVolumeApiController(DownloadDccpReadinessVolumeService downloadDccpReadinessVolumeService)
        {
            _DownloadDccpReadinessVolumeService = downloadDccpReadinessVolumeService;
        }
        private readonly DownloadDccpReadinessVolumeService _DownloadDccpReadinessVolumeService;

        /// <summary>
        /// get guid dan count data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(DownloadDate data)
        {
            //var date = DateTime.Now;
            var dbList = await _DownloadDccpReadinessVolumeService.GetDbDccp(data.Date);
            if(dbList.Count != 0)
            {
                var byteDataTable = _DownloadDccpReadinessVolumeService.ExportExcel(dbList, "DCCP - Readiness Volume " + data.Date.ToShortDateString(), true);
                var guid = _DownloadDccpReadinessVolumeService.GetGuid();
                TempData[guid] = byteDataTable;
                var count = dbList.Count;
                var obj = new DownloadDccpReadinessVolumeViewModel
                {
                    Count = count,
                    Guid = guid
                };
                //return File(byteDataTable, "application/vnd.ms-excel", "DCCP_Readiness_Volume.xlsx");
                //return Guid to TS service
                return Ok(obj);
            }
            return Ok();
        }
        //terima Guid dari API
        [HttpGet("Download/{guid}")]
        public IActionResult Download(string guid)
        {
            if (TempData[guid] != null)
            {
                byte[] data = TempData[guid] as byte[];
                return File(data, "application/vnd.ms-excel", "DCCP_Readiness_Volume.xlsx");
            }
            return BadRequest();

        }


    }
}
