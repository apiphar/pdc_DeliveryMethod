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
    public class RegionApiController : Controller
    {
        private readonly RegionService RegionService;

        public RegionApiController(RegionService RegionService)
        {
            this.RegionService = RegionService;
        }

        [Route("GetData")]
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await RegionService.GetData();
            return Ok(data);
        }
        [Route("DeleteRegion/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRegion(string id)
        {
            var rowsAffected = await RegionService.Remove(id);
            if (rowsAffected == -1)
            {
                return BadRequest("Data masih digunakan oleh region lain");
            }
            if(rowsAffected == 0)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok("Data berhasil dihapus");
        }

        [Route("UpdateRegion/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateRegion(string id, [FromBody] RegionViewModel model)
        {
            var rowsAffected = await RegionService.Update(id, model);
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if (rowsAffected == -1)
            {
                return BadRequest("Data tidak terdaftar");
            }
            if(rowsAffected == 0)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }
    }
}
