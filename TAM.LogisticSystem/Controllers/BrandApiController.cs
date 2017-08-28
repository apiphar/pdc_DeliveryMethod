using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]

    public class BrandApiController : Controller
    {
        private BrandService BrandService;

        public BrandApiController(BrandService brandService)
        {
            this.BrandService = brandService;
        }

        [HttpGet("/Brand/GetDataBrand")]
        public async Task<IActionResult> GetDataBrand()
        {
            var Data = await BrandService.GetDataBrand();
            return Ok(Data);
        }

        [HttpPost("/Brand/Create")]
        public async Task<IActionResult> Create([FromBody] BrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await BrandService.Add(model);
            if (rowsAffected == 0)
            {
                return BadRequest("Kode Brand telah terdaftar");
            }
            if (rowsAffected > 1)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpPost("/Brand/Edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] BrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await BrandService.Update(id, model);
            if (rowsAffected != 1)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpDelete("/Brand/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var rowsAffectted = await BrandService.Remove(id);
            if (rowsAffectted != 1)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok("Data berhasil dihapus");
        }
    }
}
