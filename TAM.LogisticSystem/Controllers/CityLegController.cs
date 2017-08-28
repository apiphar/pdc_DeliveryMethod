using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class CityLegController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
