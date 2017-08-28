using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;

namespace TAM.LogisticSystem.Controllers
{
    public class CompanyMasterController : Controller
    {
        public readonly CompanyMasterService companyMasterService;

        public CompanyMasterController(CompanyMasterService companyMasterService)
        {
            this.companyMasterService = companyMasterService;
        }

       public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            var Data = companyMasterService.GetCompany();
            return Ok(Data);
        }
        public IActionResult GetAll()
        {
            var Data = companyMasterService.Get();
            return Ok(Data);
        }

        public async Task<IActionResult> create([FromBody]CompanyMasterViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return Ok(model);
            }
            var result = await companyMasterService.Add(model);
            return Ok();
        }

        [HttpPost("CompanyMaster/edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] CompanyMasterViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return Ok(model);
            }

            int recordAffected = await companyMasterService.Update(id, model);
            if (recordAffected > 0)
            {
                TempData["Status"] = 1;
                TempData["Message"] = "Data has benn saved.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Status"] = 2;
                TempData["Message"] = "Data cannot be save! Because, end date must be greater than start date.";

                var data = new CompanyMasterViewModel
                {
                    
                    DealerCode = model.DealerCode,
                    Name = model.Name,
                    IsDealerFinancing = model.IsDealerFinancing,                   
                };
                return Ok(data);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            int rowsAffected = await companyMasterService.Remove(id);

            return Ok("Index");
        }

    }
}
