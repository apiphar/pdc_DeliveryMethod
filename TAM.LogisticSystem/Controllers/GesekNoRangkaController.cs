
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class GesekNoRangkaController : Controller
    {
        [Authorize]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

    }
}
