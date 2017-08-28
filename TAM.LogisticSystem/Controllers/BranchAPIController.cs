using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]

    public class BranchAPIController : Controller
    {
        private BranchService branchService;

        public BranchAPIController(BranchService branchService)
        {
            this.branchService = branchService;
        }

        [HttpGet("/Branch/GetDataBranch")]
        public async Task<IActionResult> GetDataBranch()
        {
            var dataBranch = await branchService.getDataBranch();
            return Ok(dataBranch);
        }

        [HttpGet("/Branch/GetDataSalesArea")]
        public async Task<IActionResult> GetDataSalesArea()
        {
            var dataSalesArea = await branchService.getDataSalesArea();
            return Ok(dataSalesArea);
        }

        [HttpGet("/Branch/GetDataDestination")]
        public async Task<IActionResult> GetDataDestination()
        {
            var dataDestination = await branchService.getDataDestination();
            return Ok(dataDestination);
        }

        [HttpGet("/Branch/GetDataRegion")]
        public async Task<IActionResult> GetDataRegion()
        {
            var dataRegion = await branchService.getDataRegion();
            return Ok(dataRegion);
        }

        [HttpGet("/Branch/GetDataCompany")]
        public async Task<IActionResult> GetDataCompany()
        {
            var dataCompany = await branchService.getDataCompany();
            return Ok(dataCompany);
        }

        [HttpGet("/Branch/GetDataLocation")]
        public async Task<IActionResult> GetDataLocation()
        {
            var dataLocation = await branchService.getDataLocation();
            return Ok(dataLocation);
        }

        //[HttpGet("/Branch/GetDataLocation2/{locationcode}")]
        //public IActionResult GetDataLocation2(string locationcode)
        //{
        //    var dataLocation = branchService.getDataLocation2(locationcode);
        //    return Ok(dataLocation);
        //}

        [HttpGet("/Branch/GetDataCluster")]
        public async Task<IActionResult> GetDataCluster()
        {
            var dataCluster = await branchService.getDataCluster();
            return Ok(dataCluster);
        }

        [HttpPost("/Branch/Create")]
        public async Task<IActionResult> Create([FromBody] BranchModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await branchService.Add(model);
            if (rowsAffected == 0)
            {
                return BadRequest("Kode Branch telah terfadtar");
            }
            if (rowsAffected == 2)
            {
                return BadRequest("Kode AFI Branch telah terfadtar");
            }
            if (rowsAffected > 2)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpPost("/Branch/Edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] BranchModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data tidak valid");
            }
            var rowsAffected = await branchService.Update(id, model);
            if (rowsAffected != 2)
            {
                return BadRequest("Data gagal disimpan");
            }
            return Ok("Data berhasil disimpan");
        }

        [HttpDelete("/Branch/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var rowsAffected = await branchService.Remove(id);
            if (rowsAffected != 1)
            {
                return BadRequest("Data gagal dihapus");
            }
            return Ok("Data berhasil dihapus");
        }
    }
}
