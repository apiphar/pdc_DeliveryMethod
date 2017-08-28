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
    public class GesekNoRangkaApiController : Controller
    {
        private readonly GesekNoRangkaService gesekNoRangkaService;

        public GesekNoRangkaApiController(GesekNoRangkaService gesekNoRangkaService)
        {
            this.gesekNoRangkaService = gesekNoRangkaService;
        }

        /// <summary>
        /// save data gesekan ke db
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("SaveGesekan")]
        public async Task<IActionResult> SaveGesekan([FromBody]GesekNoRangkaInputModel data)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            else
            {
                await gesekNoRangkaService.SaveGesekan(data);
                return Ok();
            }
        }

        /// <summary>
        /// Get Location data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLocationData")]
        public async Task<IActionResult> GetLocationData()
        {
            var locationData = await gesekNoRangkaService.GetLocationData();
            if (locationData == null)
            {
                return BadRequest("Data Lokasi Tidak ditemukan");
            }
            else
            {
                return Ok(locationData);
            }
        }

        /// <summary>
        /// get all gesekan no rangka data
        /// </summary>
        /// <param name="frameNo"></param>
        /// <returns></returns>
        [HttpGet("CheckDataByFrameNo/{frameNo}")]
        public async Task<IActionResult> CheckDataByFrameNo(string frameNo)
        {
            var data = await gesekNoRangkaService.CheckDataByFrameNumber(frameNo);
            if (data == null)
            {
                return BadRequest("Frame No. tidak ditemukan");
            }
            else
            {
                var check = await gesekNoRangkaService.IsFrameNumberExists(frameNo);
                if (check == true)
                {
                    return BadRequest("Frame No. sudah terdaftar");
                }
                else
                {
                    return Ok(data);
                }
            }

        }

    }
}
