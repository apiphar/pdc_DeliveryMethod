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
    public class InspectionPartController : Controller
    {
        private InspectionPartService inspectionPartService;

        public InspectionPartController(InspectionPartService inspectionItemService)
        {
            this.inspectionPartService = inspectionItemService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Data = inspectionPartService.GetAll();
            return Ok(Data);
		}

		[HttpGet]
		public IActionResult GetDropdownCategory()
		{
			var Data = inspectionPartService.GetCategory();
			return Ok(Data);
		}

		[HttpGet]
        public IActionResult GetDropdownSide()
        {
            var Data = inspectionPartService.GetSide();
            return Ok(Data);
        }

        [HttpPost("/inspectionpart/create")]
        public async Task<IActionResult> Create([FromBody] InspectionPartViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await inspectionPartService.Add(model);
            return RedirectToAction("Index");
        }

        [HttpPost("/inspectionpart/edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] InspectionPartViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await inspectionPartService.Update(id, model);
            return RedirectToAction("Index", model); 
        }

        [HttpPost("/inspectionpart/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await inspectionPartService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}






