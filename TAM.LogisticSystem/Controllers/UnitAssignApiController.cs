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
    public class UnitAssignApiController : Controller
    {
        //private readonly
        private readonly UnitAssignService _service;

        public UnitAssignApiController(UnitAssignService UAS)
        {
            this._service = UAS;
        }

        /// <summary>
        /// Get All Voyage Data and return it to client side
        /// </summary>
        /// <returns></returns>
        [Route("GetAllVoyage/{voyage}")]
        [HttpGet]
        public async Task<IActionResult> GetAllVoyage(string voyage)
        {
           var data = await this._service.GetAllData(voyage);

            if (data.AllVoyage == null)
                return BadRequest();
            return Ok(data);
        }

        /// <summary>
        /// This Function for store the data in the table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UnitAssignDataModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Data tidak valid");
            }

            foreach (var item in model.AllUnit)
            {
                if(item.Status != "Ported")
                {
                    return BadRequest("Data tidak valid");
                }
            }

            var status = await this._service.SaveData(model);
            if(status < 0)
            {
                return BadRequest();
            }

            var data = await this._service.GetAllData(model.AllVoyage.Voyage);
            return Ok(data);
        }

        /// <summary>
        /// when user click details, it shows all the unit by the voyage user just entered
        /// </summary>
        /// <param name="voyage"></param>
        /// <returns></returns>
        [Route("ViewDetail/{voyage}")]
        [HttpGet]
        public async Task<IActionResult> ViewDetail(string voyage)
        {
            var data = await this._service.GetDetailByVoyage(voyage);

            if (data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }



    }
}
