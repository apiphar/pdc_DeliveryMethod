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
    public class SPULineMasterApiController : Controller
    {
        private readonly SPULineMasterService SPULineMasterService;

        public SPULineMasterApiController(SPULineMasterService SPULineMasterService)
        {
            this.SPULineMasterService = SPULineMasterService;
        }

        [Route("SPULineMaster")]
        [HttpGet]
        public async Task<IActionResult> SPULineMaster()
        {
            var data = await SPULineMasterService.GetData();
            return Ok(data);
        }

        [Route("PostData")]
        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] SPULineMasterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var row = await SPULineMasterService.Create(model);
            if (row < 1)
            {
                return BadRequest("Gagal menyimpan data");
            }
            return Ok("Data berhasil disimpan");
        }


        [Route("UpdateSPULineMaster/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateSPULineMaster(int id, [FromBody] SPULineMasterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await SPULineMasterService.Update(id, model);
            if (rowsAffected < 1)
            {
                return BadRequest("Gagal mengubah data");
            }
            return Ok("Data berhasil diubah");
        }


        [Route("DeleteSPULineMaster/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSPULineMaster(int id)
        {
            var rowsAffected = await SPULineMasterService.Remove(id);
            if (rowsAffected < 1)
            {
                return BadRequest("Gagal menghapus data");
            }
            return Ok("Data berhasil dihapus");
        }
    }
}
