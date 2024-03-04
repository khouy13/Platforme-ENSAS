using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Microsoft.AspNetCore.Identity;
using Projet.Data;
using Microsoft.AspNetCore.Authorization;

namespace Projet.Areas.Responsable.Controllers
{
  
        [Area("Responsable")]
    [Authorize(Roles = "Admin")]
    public class FilieresController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly AppDbContext _context;

            public FilieresController(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _context = context;
                _userManager = userManager;
                _roleManager = roleManager;
            }
        public async Task<IActionResult> Index(string searchText = "", int pg = 1)
        {
            var filieresQuery = _context.Filieres.Include(f => f.Departement).Include(l=>l.ApplicationUser).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                filieresQuery = filieresQuery.Where(f => f.NomFiliere.Contains(searchText) || f.Departement.NomDepartementt.Contains(searchText));
            }

            var totalFilieres = await filieresQuery.CountAsync();
            const int pageSize = 5; // Nombre de filières par page
            var pager = new Pager(totalFilieres, pg, pageSize);
            int skip = (pg - 1) * pageSize;

            var pagedFilieres = await filieresQuery.Skip(skip).Take(pageSize).OrderBy(f => f.Departement.NomDepartementt).ToListAsync();

            ViewBag.Pager = pager;
            ViewBag.SearchText = searchText;

            return View(pagedFilieres);
        }




        // GET: Responsable/Filieres/Create
        public async Task<IActionResult> Create()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Coordonnateur");
            ViewBag.Departements = await _context.Departements.ToListAsync();
            ViewBag.Coordenateur = usersInRole;
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["IdDepartement"] = new SelectList(_context.Departements, "IdDepartement", "IdDepartement");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFiliere,NomFiliere,IdDepartement,ApplicationUserId")] Filiere filiere)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filiere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", filiere.ApplicationUserId);
            ViewData["IdDepartement"] = new SelectList(_context.Departements, "IdDepartement", "NomDepartementt", filiere.IdDepartement);
            return View(filiere);
        }

        // GET: Responsable/Filieres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Coordonnateur");
            var usersWithEnseignant = _context.Users
                .Include(u => u.Enseignant) // Inclure la navigation vers l'entité "Enseignant"
                .Where(u => usersInRole.Contains(u)) // Filtrer les utilisateurs ayant le rôle "Coordonnateur"
                .ToList();

            ViewBag.Coordenateur = usersWithEnseignant;
            if (id == null || _context.Filieres == null)
            {
                return NotFound();
            }

            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users.Include(e=>e.Enseignant), "Id", "Id", filiere.ApplicationUserId);
            ViewData["IdDepartement"] = new SelectList(_context.Departements, "IdDepartement", "NomDepartementt", filiere.IdDepartement);
            return View(filiere);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("IdFiliere,NomFiliere,IdDepartement,ApplicationUserId")] Filiere filiere)
        {
            ViewBag.Departements = await _context.Departements.ToListAsync();
            var usersInRole = await _userManager.GetUsersInRoleAsync("Coordonnateur");
            ViewBag.Coordenateur = usersInRole;
            if (id != filiere.IdFiliere)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingFiliere = await _context.Filieres.FindAsync(id);
                    if (existingFiliere == null)
                    {
                        return NotFound();
                    }
                    // Mise à jour des propriétés
                    existingFiliere.NomFiliere = filiere.NomFiliere;
                    existingFiliere.IdDepartement = filiere.IdDepartement;
                    existingFiliere.ApplicationUserId = filiere.ApplicationUserId;

                    _context.Update(existingFiliere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FiliereExists(filiere.IdFiliere))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(filiere);
        }



        // GET: Responsable/Filieres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Filieres == null)
            {
                return NotFound();
            }

            var filiere = await _context.Filieres
                .Include(f => f.ApplicationUser)
                .Include(f => f.Departement)
                .FirstOrDefaultAsync(m => m.IdFiliere == id);
            if (filiere == null)
            {
                return NotFound();
            }

            return View(filiere);
        }

        // POST: Responsable/Filieres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Filieres == null)
            {
                return Problem("Entity set 'AppDbContext.Filieres'  is null.");
            }
            var filiere = await _context.Filieres.FindAsync(id);
            if (filiere != null)
            {
                _context.Filieres.Remove(filiere);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FiliereExists(int id)
        {
          return (_context.Filieres?.Any(e => e.IdFiliere == id)).GetValueOrDefault();
        }
    }
}
