using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System.Security.Cryptography;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    [Authorize(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        private readonly IHostingEnvironment _host;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public ApplicationUsersController(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment host)
        {
            this._context=context;
            this._userManager=userManager;
            this._roleManager=roleManager;

            this._host=host;
        }
        // GET: ApplicationController

        public async Task<IActionResult> Index(int pg = 1,string? UId="")
        {
            const int pageSize =5; // Nombre d'utilisateurs par page
            List<ApplicationUser> users;

            if (!string.IsNullOrEmpty(UId))
            {
             
                users = await _userManager.Users.Where(e=>e.Id==UId)
                    
                    .ToListAsync();
            }
            else
            {
                users = await _userManager.Users.ToListAsync();
            }
            ViewBag.users = new SelectList(users, "Id", "NomComplet", UId);
            // Pagination
            int totalUsers = users.Count;
            var pager = new Pager(totalUsers, pg, pageSize);
            int skip = (pg - 1) * pageSize;
            var pagedUsers = users.Skip(skip).Take(pageSize).ToList();

            var usersWithRoles = new List<ApplicationUserViewModel>();

            foreach (var user in pagedUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userViewModel = new ApplicationUserViewModel
                {
                    Id = user.Id,
                    IdEnseignant = user.IdEnseignant,
                    IdVacataire = user.IdVacataire,
                    Vacataire=user.Vacataire,
                    Enseignant=user.Enseignant,
                    UserName = user.UserName,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                   
                    ImagePath = user.ImagePath,
                    Roles = roles
                };

                usersWithRoles.Add(userViewModel);
            }

            ViewBag.Pager = pager;
        

            return View(usersWithRoles);
        }


        // GET: ApplicationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            var utilisateur=await _userManager.Users.Include(e=>e.Enseignant).Include(v=>v.Vacataire).FirstOrDefaultAsync(i=>i.Id==id);
            ViewBag.utilisateur=utilisateur;
            ViewData["email"] = new SelectList(_context.Enseignants,"Email", "Email");
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Créer la SelectList avec les rôles et leur sélection
            var roleSelectList = new SelectList(allRoles, nameof(IdentityRole.Name), nameof(IdentityRole.Name), roles);

            ViewBag.AllRoles = roleSelectList;

            var userViewModel = new ApplicationUserViewModel
            {
                Id = user.Id,
                IdEnseignant=user.IdEnseignant,
                IdVacataire=user.IdVacataire,
                UserName = user.UserName ,
                Password = user.PasswordHash,
                ImagePath = user.ImagePath,
                Roles = roles
            };
            ViewBag.EnseignatList = _context.Enseignants.ToList();
            ViewBag.vacataire = _context.vacataires.ToList();

            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                var ens=_context.Enseignants.FirstOrDefault(e=>e.IdEnseignant==user.IdEnseignant);
                var vac=_context.vacataires.FirstOrDefault(v=>v.IdVacataire==user.IdVacataire);
                if (ens!=null)
                {
                    ens.Email = model.UserName;
                }
                else if(vac!=null)
                {
                    vac.Email = model.UserName;
                }
                await _context.SaveChangesAsync();
                if (user == null)
                {
                    return NotFound();
                }

                user.IdEnseignant = model.IdEnseignant;
                user.IdVacataire = model.IdVacataire;
                user.UserName = model.UserName;
                user.Email =model.UserName;

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
                    // Mise à jour des rôles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = currentRoles.Except(model.Roles);
                    var rolesToAdd = model.Roles.Except(currentRoles);

                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    await _userManager.AddToRolesAsync(user, rolesToAdd);

                    return RedirectToAction("Index", "ApplicationUsers", new { area = "Responsable" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Repeupler la liste des rôles en cas d'erreur
            var allRoles = await _roleManager.Roles.ToListAsync();
            var roleList = allRoles.Select(role => new SelectListItem
            {
                Value = role.Name,
                Text = role.Name,
                Selected = model.Roles.Contains(role.Name)
            }).ToList();

            ViewBag.AllRoles = roleList;

            return RedirectToAction("Index");
        }


        // GET: ApplicationController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                ImagePath = user.ImagePath
            };

            return View(userViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var filieres = _context.Filieres.Where(f => f.ApplicationUserId == user.Id).ToList();

            foreach (var filiere in filieres)
            {
                filiere.ApplicationUserId = null; // Définissez la clé étrangère sur null
            }

            // Enregistrez les modifications dans la base de données
            _context.SaveChanges();

            // Supprimer l'image du dossier
            if (!string.IsNullOrEmpty(user.ImagePath) && user.ImagePath != "asset/images/profilNexistePas.jpg")
            {
                string imagePath = Path.Combine(_host.WebRootPath, user.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(id);
        }


    }
}
