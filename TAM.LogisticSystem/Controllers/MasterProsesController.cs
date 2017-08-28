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
    public class MasterProsesController : Controller
    {
        private MasterProsesService MasterProsesService;

        public MasterProsesController(MasterProsesService MasterProsesService)
        {
            this.MasterProsesService = MasterProsesService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
    }
}
