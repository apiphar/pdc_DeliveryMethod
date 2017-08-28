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
    public class AfiReceiveDocumentApiController : Controller
    {
        public AfiReceiveDocumentService afiReceiveDocumentService;
        public AfiReceiveDocumentApiController(AfiReceiveDocumentService afiReceiveDocumentService)
        {
            this.afiReceiveDocumentService = afiReceiveDocumentService;
        }

        /// <summary>
        /// Get the AFI data base on frame Number
        /// </summary>
        /// <param name="frameNo"></param>
        /// <returns></returns>
        [HttpGet("{frameNo}")]
        public async Task<IActionResult> Get(string frameNo)
        {
            if (string.IsNullOrEmpty(frameNo))
            {
                return BadRequest("Frame Number harus diisi");
            }
            if (await this.afiReceiveDocumentService.IsProcessTAM(frameNo) == false)
            {
                return BadRequest("Frame Number belum diproses oleh TAM");
            }
            var Data = await this.afiReceiveDocumentService.GetReceiveDocument(frameNo);
            if(Data == null)
            {
                return BadRequest("Frame Number tidak ditemukan");
            }
            
            return Ok(Data);
        }


        /// <summary>
        /// Update into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ReceiveDocument")]
        [HttpPost]
        public async Task<IActionResult> ReceiveDocument([FromBody]AfiReceiveDocumentUpdate model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Data tidak valid");

            var row = await afiReceiveDocumentService.ReceiveDocument(model);
            if (model.AfiApplicationList.Count != row){
                return BadRequest("Data gagal disimpan");
            }

            return Ok("Data berhasil disimpan");
        }
    }
}
