
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.TANGO.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "TLS_Authentication_Cookie")]
    public class AfiDownloadController : Controller
    {
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
