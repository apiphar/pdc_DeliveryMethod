using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    public class ColourController : Controller
    {
        //Controller
        [Route("colour")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        
    }
}
