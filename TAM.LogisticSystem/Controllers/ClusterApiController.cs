using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.TANGO.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]

    public class ClusterApiController : Controller
    {
        private ClusterService clusterService;

        public ClusterApiController(ClusterService clusterService)
        {
            this.clusterService = clusterService;
        }

        [HttpGet("/Cluster/GetDataCluster")]
        public async Task<IActionResult> GetDataCluster()
        {
            var dataCluster = await clusterService.GetDataCluster();
            return Ok(dataCluster);
        }

        [HttpPost("/Cluster/create")]
        public async Task<IActionResult> Create([FromBody] ClusterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await clusterService.Create(model);
            if (rowsAffected == 0)
            {
                return BadRequest("Kode Cluster telah terdaftar");
            }
            if (rowsAffected > 1)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpPost("/Cluster/edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] ClusterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await clusterService.Update(id, model);
            if (rowsAffected != 1)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpDelete("/Cluster/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var rowsAffected = await clusterService.Remove(id);
            if (rowsAffected != 1)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok("Data berhasil dihapus");
        }
    }
}
