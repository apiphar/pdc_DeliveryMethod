using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class ExchangerateApiController : Controller
    {
        ExchangeRate exchangeRate = new ExchangeRate();
        private readonly ExchangeRateService exchangeRateService;

        public ExchangerateApiController(ExchangeRateService currencyservice)
        {
            this.exchangeRateService = currencyservice;
        }

        public IActionResult GetExchangeRates()
        {
            var data = exchangeRateService.GetExchangeRates();
            return Ok(data);
        }

        public IActionResult GetCurrencys()
        {
            var data = exchangeRateService.GetCurrencys();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ExchangeRateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return NotFound();
            }

            int recordaffected = await exchangeRateService.Add(model.CurrencySymbol, model.ToRupiah, model.ValidFrom, model.ValidUntil, model.CreatedAt, model.CreatedBy, model.UpdatedAt, model.UpdatedBy);
            if (recordaffected > 0)
            {
                return Ok();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost("/exchangeRateapi/edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody]ExchangeRateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return NotFound();
            }

            int recordaffected = await exchangeRateService.Update(id, model);
            return Ok();
        }


        [HttpDelete("/exchangeRateapi/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int rowsAffected = await exchangeRateService.Remove(id);
            return Ok();
        }
    }
}
