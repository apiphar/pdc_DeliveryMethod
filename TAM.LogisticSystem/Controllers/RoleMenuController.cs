using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAM.LogisticSystem.Services;
using TAM.LogisticSystem.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TAM.LogisticSystem.Controllers
{
    [Authorize(Roles = RoleMenuEnum.MenuAdministrator)]
    [AutoValidateAntiforgeryToken]
    public class RoleMenuController : Controller
    {
        private readonly RoleMenuService MenuMan;

        public RoleMenuController(RoleMenuService menuMan)
        {
            this.MenuMan = menuMan;
        }

        public async Task<IActionResult> Index()
        {
            var stuffs = await MenuMan.GetAll();
            return View(stuffs);
        }

        public class MenuMappingCreateViewModel
        {
            [Required]
            public string Role { set; get; }

            [Required]
            public string Menu { set; get; }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuMappingCreateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var exist = await MenuMan.Get(model.Role, model.Menu);
            if (exist != null)
            {
                ModelState.AddModelError(nameof(model.Menu), $"Role: {model.Role} has already mapped to menu: {model.Menu}!");
                return View(model);
            }
            // TIE: START
            //await MenuMan.CreateNew(model.Role, model.Menu);
            // TIE: END
            TempData["RoleMenuCreated"] = $"Successfully mapped role: {model.Role} to menu: {model.Menu}.";
            return RedirectToAction("Create");
        }

        [Route("[controller]/{role}/{menu}")]
        public async Task<IActionResult> Delete(string role, string menu)
        {
            var e = await MenuMan.Get(role, menu);
            if (e == null)
            {
                return NotFound();
            }
            return View(e);
        }

        [HttpPost]
        [Route("[controller]/{role}/{menu}")]
        public async Task<IActionResult> Delete(string role, string menu, object gay)
        {
            var e = await MenuMan.Get(role, menu);
            if (e == null)
            {
                return NotFound();
            }

            await MenuMan.Delete(e);
            TempData["RoleMenuDelete"] = $"Successfully deleted mapping of role: {e.AppRoleName} to menu: {e.AppMenuName}.";
            return RedirectToAction("Index");
        }
    }
}
