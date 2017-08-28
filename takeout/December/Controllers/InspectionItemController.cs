using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class InspectionItemController : Controller
    {
        private InspectionItemService inspectionItemService;

        public InspectionItemController(InspectionItemService inspectionItemService)
        {
            this.inspectionItemService = inspectionItemService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Data = inspectionItemService.GetAll();
            return Ok(Data);
        }

        [HttpGet]
        public IActionResult GetDropdownSide()
        {
            var Data = inspectionItemService.GetSide();
            return Ok(Data);
        }

        [HttpPost("/inspectionitem/create")]
        public async Task<IActionResult> Create([FromBody] InspectionItemViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await inspectionItemService.Add(model);
            return RedirectToAction("Index");
        }

        [HttpPost("/inspectionitem/edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] InspectionItemViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await inspectionItemService.Update(id, model);
            return RedirectToAction("Index", model); 
        }

        [HttpPost("/inspectionitem/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await inspectionItemService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}






