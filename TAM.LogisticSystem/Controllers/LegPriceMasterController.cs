using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class LegPriceMasterController : Controller
    {
        public readonly LegPriceMasterService legPriceMasterService;

        public LegPriceMasterController(LegPriceMasterService legPriceMasterService)
        {
            this.legPriceMasterService = legPriceMasterService;
        }

        public IActionResult Index()
        {
            return View();
        }
        

    }
}
