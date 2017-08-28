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
    [Route("api/v1/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class MasterGroupDealerApiController : Controller
    {
        private readonly MasterGroupDealerService MasterGroupDealerService;

        public MasterGroupDealerApiController(MasterGroupDealerService masterGroupDealerService)
        {
            this.MasterGroupDealerService = masterGroupDealerService;
        }

        // Get all Table Data
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tableData = await MasterGroupDealerService.GetAllTableData();
            return Ok(tableData);
        }

        // Create new group Dealer
        [HttpPost]
        [Route("CreateData")]
        public async Task<IActionResult> CreateData([FromBody]MasterGroupDealerViewModel masterGroupDealerViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }
            if (await MasterGroupDealerService.CheckCode(masterGroupDealerViewModel.KodeGroupDealer) != null)
            {
                return BadRequest("Kode Group Dealer sudah terdaftar");
            }
            await this.MasterGroupDealerService.CreateData(masterGroupDealerViewModel);
            return Ok();
        }

        [HttpPost]
        [Route("UpdateData")]
        public async Task<IActionResult> UpdateData([FromBody]MasterGroupDealerViewModel masterGroupDealerViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
            await this.MasterGroupDealerService.UpdateData(masterGroupDealerViewModel);
            return Ok();
        }

        // Delete Group Dealer
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await this.MasterGroupDealerService.DeleteData(id);
            return Ok();
        }
    }
}
