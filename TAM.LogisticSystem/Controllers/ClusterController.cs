using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    public class ClusterController : Controller
    {
        [HttpGet("/Cluster")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
