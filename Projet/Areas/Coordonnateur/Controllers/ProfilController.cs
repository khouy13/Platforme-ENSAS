using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace Projet.Areas.Coordonnateur.Controllers
{
    [Area("coordonnateur")]
    public class ProfilController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _host;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfilController(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IHostingEnvironment host, RoleManager<IdentityRole> roleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
            _host = host;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Profilfirst()
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
            
                var roles = await _userManager.GetRolesAsync(user);
                var allRoles = await _roleManager.Roles.ToListAsync();
                var roleSelectList = new SelectList(allRoles, nameof(IdentityRole.Name), nameof(IdentityRole.Name), roles);

                ViewBag.AllRoles = roleSelectList;

                if (user != null)
                {


                    var userViewModel = new ApplicationUserViewModel
                    {
                        Id = user.Id,
                        IdEnseignant = user.IdEnseignant,
                        IdVacataire = user.IdVacataire,
                        UserName = user.UserName,
                        Password = " ",
                        ImagePath = user.ImagePath,
                        Roles = roles
                    };

                    return View(userViewModel);
                }
            }

            return RedirectToAction("Profilfirst"); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.IdEnseignant = model.IdEnseignant;
                user.IdVacataire = model.IdVacataire;
                user.UserName = model.UserName;
                user.Email = model.UserName;
                

                // Gestion de l'image
                if (model.UserFile != null)
                {
                    // Supprimer l'ancienne image s'il en existe une
                    if (!string.IsNullOrEmpty(user.ImagePath) && user.ImagePath != "asset/images/profilNexistePas.jpg")
                    {
                        string oldImagePath = Path.Combine(_host.WebRootPath, user.ImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string uploadsFolder = Path.Combine(_host.WebRootPath, "asset", "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.UserFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UserFile.CopyToAsync(stream);
                    }

                    user.ImagePath = Path.Combine("asset", "images", uniqueFileName);
                }

                // Mise à jour du mot de passe si fourni
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var passwordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    user.PasswordHash = passwordHash;
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profil modifié avec succès.";
                    // Mise à jour des rôles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = currentRoles.Except(model.Roles);
                    var rolesToAdd = model.Roles.Except(currentRoles);

                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    await _userManager.AddToRolesAsync(user, rolesToAdd);

                    return RedirectToAction("Profilfirst", "Profil", new { area = "coordonnateur" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            //// Repeupler la liste des rôles en cas d'erreur
            //var allRoles = await _roleManager.Roles.ToListAsync();
            //var roleList = allRoles.Select(role => new SelectListItem
            //{
            //    Value = role.Name,
            //    Text = role.Name,
            //    Selected = model.Roles.Contains(role.Name)
            //}).ToList();
            //var AllRoles = roleList;
            //ViewBag.AllRoles = AllRoles;

            return RedirectToAction("Profilfirst");
        }
    }
}