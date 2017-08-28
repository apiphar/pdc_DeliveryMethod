using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.TANGO.Entities;
using TAM.TANGO.Services;
using TAM.TANGO.Models;

namespace TAM.TANGO.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class DealerController : Controller
    {
        private DealerService DealerMan;

        public DealerController(DealerService delaerMan)
        {
            this.DealerMan = delaerMan;
        }

        // GET: Dealer
        public async Task<IActionResult> Index(DealerSearchParameters search)
        {
            var model = await DealerMan.Search(search);
            return View(model);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DealerCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await DealerMan.Add(model.Name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await DealerMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new DealerCreateOrUpdateRequest
            {
                Name = entity.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DealerCreateOrUpdateRequest model)
        {
            var entity = await DealerMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await DealerMan.Update(entity, model.Name);
            return RedirectToAction("Edit");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await DealerMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            var entity = await DealerMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            await DealerMan.Remove(entity);
            return RedirectToAction("Index");
        }
    }
}
