using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.TANGO.Entities;
using TAM.TANGO.Services;
using TAM.TANGO.Models;
using TAM.TANGO.Helpers;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.TANGO.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class RoutingGroupController : Controller
    {
        private readonly RoutingGroupService RoutingGroupMan;

        public RoutingGroupController(RoutingGroupService routingGroupMan)
        {
            this.RoutingGroupMan = routingGroupMan;
        }

        public async Task<IActionResult> Index(RoutingGroupSearchParameter search)
        {
            var model = await RoutingGroupMan.Search(search);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoutingGroupCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await RoutingGroupMan.Add(model.Name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await RoutingGroupMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new RoutingGroupCreateOrUpdateRequest
            {
                Name = entity.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoutingGroupCreateOrUpdateRequest model)
        {
            var entity = await RoutingGroupMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await RoutingGroupMan.Update(entity, model.Name);
            return RedirectToAction("Edit");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await RoutingGroupMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePos(int id)
        {
            var entity = await RoutingGroupMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            await RoutingGroupMan.Remove(entity);
            return RedirectToAction("Index");
        }
         
    }
}
