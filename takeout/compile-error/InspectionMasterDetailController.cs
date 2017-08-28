using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.TANGO.Services;
using TAM.TANGO.Models;
using static TAM.TANGO.Helpers.MvcRenderingExtensions;

namespace TAM.TANGO.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class InspectionMasterDetailController : Controller
    {
        private readonly InspectionMasterDetailService InspectionMasterDetailService;

        public InspectionMasterDetailController(InspectionMasterDetailService inspectionMasterDetailService)
        {
            this.InspectionMasterDetailService = inspectionMasterDetailService;
        }

        public async Task<IActionResult> Index(InspectionMasterDetailSearchParameters search)
        {
            var model = await InspectionMasterDetailService.Search(search);
            return View(model);
        }

        public IActionResult Create()
        {       
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InspectionMasterDetailCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            int recordAffected = await InspectionMasterDetailService.Add(model);
            if (recordAffected > 0)
            {
                ViewBag.Status = true;
                ViewBag.Message = "Data has been saved.";
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "Data cannot be saved!";
            }

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await InspectionMasterDetailService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new InspectionMasterDetailCreateOrUpdateRequest
            {
                InspectionMasterId = entity.InspectionMasterId,
                InspectionAreaId = entity.InspectionAreaId,
                Name = entity.Name,
                Description = entity.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, InspectionMasterDetailCreateOrUpdateRequest model)
        {
            var entity = await InspectionMasterDetailService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            int recordAffected = await InspectionMasterDetailService.Update(entity, model);
            if (recordAffected > 0)
            {
                ViewBag.Status = true;
                ViewBag.Message = "Data has been saved.";
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "Data cannot be saved!";
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await InspectionMasterDetailService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var entity = await InspectionMasterDetailService.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            await InspectionMasterDetailService.Remove(entity);
            return RedirectToAction("Index");
        }
    }
}
