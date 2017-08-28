using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Models;
using TAM.LogisticSystem.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    public class PermitController : Controller
    {
        private readonly PermitService permitService;

        public PermitController(PermitService permitService)
        {
            this.permitService = permitService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult GetData()
        {
            var permit = permitService.GetPermit();
            return Ok(permit);
        }

        //public IActionResult GetAll()
        //{
        //    var katashiki = permitService.Get();
        //    return Ok(katashiki);
        //}

        [Route("/Permit/getsuffix/{katashiki}")]
        public IActionResult getSuffix(string Katashiki)
        {
            var getKatahshiki = permitService.GetSuffix(Katashiki);
            return Ok(getKatahshiki);
        }

        [Route("/Permit/getcarmodel/{katashiki}/{suffix}")]
        public IActionResult getCarModel(string Katashiki, string Suffix)
        {
            var getCarModel = permitService.GetCarModel(Katashiki, Suffix);
            return Ok(getCarModel);
        }


        [HttpPost]     
        public async Task<IActionResult> Create([FromBody]PermitViewModel model)
        {
            if(ModelState.IsValid == false)
            {
                return View(model);
            }
            var result =await permitService.Add(model);
            return Ok(result);
        }

        //public async Task<IActionResult> Edit(string id, [FromBody] PermitViewModel model)
        //{
        //    int recordAffacted = await permitService.Update(id,model);

        //    return RedirectToAction("Index");
        //}

        [HttpPost("/Permit/edit/{id}")]
        public async Task<IActionResult> Edit(int id,[FromBody] PermitViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            int recordAffected = await permitService.Update(id,model);
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

                var data = new PermitViewModel
                {
                    Katashiki=model.Katashiki,
                    Suffix = model.Suffix,
                    PermitId = model.PermitId,
                    CarModelCode = model.CarModelCode,
                    Name = model.Name,
                    Quota = model.Quota,
                    EffectiveFrom = model.EffectiveFrom,
                    EffectiveUntil = model.EffectiveUntil,
                };
                return Ok(data);
            }
		}

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> DeletePost(int id)
		{
			var entity = await permitService.Get(id);
			if (entity == null)
			{
				return NotFound();
			}

			await permitService.Remove(entity);
			return RedirectToAction("Index");
		}

	}
}
