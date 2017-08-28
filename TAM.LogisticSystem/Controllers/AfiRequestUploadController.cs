
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize]
    public class AfiRequestUploadController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
