using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Route("api/[controller]")]
    public class IntegrasiKalkulasiApiController : Controller
    {
        private readonly IntegrasiKalkulasi _service;

        public IntegrasiKalkulasiApiController(IntegrasiKalkulasi s)
        {
            this._service = s;
        }


        // GET: api/values
        [HttpGet("{frameNo}")]
        public async Task Get(string frameNo)
        {
            var vehicle = await this._service.GetSelectedVehicle(frameNo);
            await this._service.CalculateRoutingVehicle(vehicle);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
