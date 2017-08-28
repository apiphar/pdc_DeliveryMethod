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
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    [Route("api/v1/[controller]")]
    public class PIOLineMasterApiController : Controller
    {
        private readonly PIOLineMasterService PIOLineMasterService;

        public PIOLineMasterApiController(PIOLineMasterService PIOLineMasterService)
        {
            this.PIOLineMasterService = PIOLineMasterService;
        }

        // TIE: START
        //[ValidateModelAttribute]
        //[Route("PostData")]
        //[HttpPost]
        //public async Task<IActionResult> PostData([FromBody]PIOLineMasterModel model)
        //{
        //    int row = await PIOLineMasterService.Create(model);
        //    if (row < 1)
        //    {
        //        return BadRequest("Gagal menyimpan data");
        //    }
        //    return Ok("Data berhasil disimpan");
        //}

        //[Route("PIOLineMaster")]
        //[HttpGet]
        //public async Task<IActionResult> PIOLineMaster()
        //{
        //    var data = await PIOLineMasterService.GetData();
        //    return Ok(data);
        //}

        //[Route("DeletePIOLineMaster/{id}")]
        //[HttpDelete]
        //public async Task<IActionResult> DeletePIOLineMaster(int id)
        //{
        //    int rowsAffected = await PIOLineMasterService.Remove(id);
        //    if (rowsAffected < 1)
        //    {
        //        return BadRequest("Gagal menghapus data");
        //    }
        //    return Ok("Data berhasil dihapus");
        //}
        //[ValidateModelAttribute]
        //[Route("UpdatePIOLineMaster/{id}")]
        //[HttpPost]
        //public async Task<IActionResult> UpdatePIOLineMaster(int id, [FromBody] PIOLineMasterModel model)
        //{
        //    int rowsAffected = await PIOLineMasterService.Update(id, model);
        //    if (rowsAffected < 1)
        //    {
        //        return BadRequest("Gagal menghapus data");
        //    }
        //    return Ok("Data berhasil diubah");
        //}
        // TIE: END
    }
}
