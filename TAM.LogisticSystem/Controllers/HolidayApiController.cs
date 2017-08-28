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
    [Route("api/v1/HolidayApi")]
    [Authorize]
    public class HolidayApiController : Controller
    {
        private readonly HolidayService HolidayService;

        public HolidayApiController(HolidayService holidayService)
        {
            this.HolidayService = holidayService;
        }

        [HttpGet]
        [Route("GetData")]
        public async Task<IActionResult> GetData (){

            var dataKalender = await  HolidayService.GetData();
            return Ok(dataKalender);
        }

       
       [HttpPost]
       [Route("SaveAddData")]
        public async Task<IActionResult> SaveAddData([FromBody] List<HolidayViewModel> added)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data gagal disimpan");
            }
            else
            {
                await HolidayService.SaveAddData(added);

                return Ok();
            }
            
        }

        [HttpPost]
        [Route("SaveDelData")]
        public async Task<IActionResult> SaveDelData([FromBody] List<HolidayViewModel> deleted)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data gagal disimpan");
            }
            else
            {
                await HolidayService.SaveDelData(deleted);

                return Ok();
            }
            
        }

        
        [HttpGet]
        [Route("PopulateLocations")]
        public async Task<IActionResult> PopulateLocations()
        {
            var Data = await HolidayService.PopulateLocation();

            return Ok(Data);
        }

        [HttpGet]
        [Route("PopulateYears")]
        public IActionResult PopulateYears()
        {
            var Data = new List<int>();

            for (var y = DateTime.Now.Year; y < DateTime.Now.AddYears(5).Year; y++)
                Data.Add(y);

            return Ok(Data);
        }
       
    }
}
