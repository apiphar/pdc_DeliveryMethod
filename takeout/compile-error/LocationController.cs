using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAM.TANGO.Entities;
using TAM.TANGO.Services;
using TAM.TANGO.Models;
using TAM.TANGO.Helpers;

namespace TAM.TANGO.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class LocationController : Controller
    {
        private readonly LocationService LocationMan;

        public LocationController(LocationService locationMan)
        {
            this.LocationMan = locationMan;
        }
        
        public async Task<IActionResult> Index(LocationSearchParameters search)
        {
            var model = await LocationMan.Search(search);
            return View(model);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(LocationCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await LocationMan.Add(model.Name, model.Type, model.ParkingColumnTotal, model.ParkingRowTotal, model.ClusterId);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await LocationMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new LocationCreateOrUpdateRequest
            {
                Name = entity.Name,
                Type = entity.LocationTypeId,
                ParkingRowTotal = entity.ParkingRowTotal,
                ParkingColumnTotal = entity.ParkingColumnTotal,
                ClusterId = entity.ClusterId

            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, LocationCreateOrUpdateRequest model)
        {
            var entity = await LocationMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await LocationMan.Update(entity, model.Name, model.Type, model.ParkingColumnTotal, model.ParkingRowTotal, model.ClusterId);
            return RedirectToAction("Edit");
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await LocationMan.Get(id);
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
            var entity = await LocationMan.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            await LocationMan.Remove(entity);
            return RedirectToAction("Index");
        }
        
    }
}
