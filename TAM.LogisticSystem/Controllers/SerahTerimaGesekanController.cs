using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    public class SerahTerimaGesekanController : Controller
    {
        private readonly SerahTerimaGesekanService serahTerimaGesekanService;

        public SerahTerimaGesekanController(SerahTerimaGesekanService serahTerimaGesekanService)
        {
            this.serahTerimaGesekanService = serahTerimaGesekanService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
