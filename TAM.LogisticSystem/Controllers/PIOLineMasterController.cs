using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class PIOLineMasterController : Controller
    {
        private PIOLineMasterService PIOLineMasterService;

        public PIOLineMasterController(PIOLineMasterService PIOLineMasterService)
        {
            this.PIOLineMasterService = PIOLineMasterService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

    }
}
