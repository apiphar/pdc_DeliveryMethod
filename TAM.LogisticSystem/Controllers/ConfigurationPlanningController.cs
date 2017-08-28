using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class ConfigurationPlanningController : Controller
    {
        private readonly ConfigurationPlanningService configurationPlanningService;

        public ConfigurationPlanningController(ConfigurationPlanningService configurationPlanningService_)
        {
            this.configurationPlanningService = configurationPlanningService_;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
