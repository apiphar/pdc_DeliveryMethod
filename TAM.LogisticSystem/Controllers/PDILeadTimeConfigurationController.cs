using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    public class PDILeadTimeConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
