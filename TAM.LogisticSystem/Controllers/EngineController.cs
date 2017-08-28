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
    public class EngineController : Controller
    {
        public readonly EngineService engineService;

        public EngineController(EngineService engineService)
        {
            this.engineService = engineService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // TIE: START
        //public IActionResult GetData()
        //{
        //    var cartypess = engineService.GetEngine();
        //    return Ok(cartypess);
        //}

        //public IActionResult GetAll()
        //{
        //    var data = engineService.Get();
        //    return Ok(data);
        //}

        //[HttpPost]
        //public async Task<IActionResult> create([FromBody]EngineViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return View(model);
        //    }
        //    var result = await engineService.Add(model);
        //    return RedirectToAction("Index");
        //}

        //[Route("/Engine/getcarmodel/{katashiki}")]
        //public IActionResult getCarModel(string Katashiki)
        //{
        //    var getCarModel = engineService.GetCarModel(Katashiki);
        //    return Ok(getCarModel);
        //}

        //public async Task<IActionResult> Edit(int id, [FromBody] EngineViewModel model)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return View(model);
        //    }

        //    int recordAffected = await engineService.Update(id, model);
        //    if (recordAffected > 0)
        //    {
        //        TempData["Status"] = 1;
        //        TempData["Message"] = "Data has benn saved.";

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["Status"] = 2;
        //        TempData["Message"] = "Data cannot be save! Because, end date must be greater than start date.";

        //        var data = new EngineViewModel
        //        {
        //            Katashiki = model.Katashiki,
        //            FrameCode = model.FrameCode,
        //            CarModelName = model.CarModelName,  
        //            EnginePrefix = model.EnginePrefix,
        //        };
        //        return Ok(data);
        //    }
        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    int rowsAffected = await engineService.Remove(id);

        //    return RedirectToAction("Index");
        //}
        // TIE: END
    }
}
