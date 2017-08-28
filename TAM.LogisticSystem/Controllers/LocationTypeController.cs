using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    public class LocationTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
