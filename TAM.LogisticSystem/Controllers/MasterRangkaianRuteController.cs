using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;

namespace TAM.LogisticSystem.Controllers
{
    public class MasterRangkaianRuteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
