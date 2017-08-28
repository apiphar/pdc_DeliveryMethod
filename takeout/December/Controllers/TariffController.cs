using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class TariffController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
