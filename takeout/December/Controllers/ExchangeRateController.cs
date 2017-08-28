using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TAM.LogisticSystem.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class ExchangeRateController : Controller
    {
        ExchangeRate exchangeRate = new ExchangeRate();
        private readonly ExchangeRateService exchangeRateService;

        public ExchangeRateController(ExchangeRateService currencyservice)
        {
            this.exchangeRateService = currencyservice;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
