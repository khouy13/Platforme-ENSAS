using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Projet.Areas.Admin.Controllers
{
    [Area("Responsable")]
    [Route("appRoles")]
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles="Responsable")]
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [Route("AppRoles")]
        public IActionResult IndexRole()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
      
        [HttpGet]
       
        public IActionResult CreateRole()
        {

            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole model)
        {
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {

                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }

            return RedirectToAction("IndexRole");
        }
    }
}
