using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    public class DepartementsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DepartementsController(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
    

        // GET: Responsable/Departements
        public async Task<IActionResult> First()
        {
              return _context.Departements != null ? 
                          View(await _context.Departements.Include(u=>u.ApplicationUser).ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Departements'  is null.");
        }

       

        // GET: Responsable/Departements/Create
        public async Task<IActionResult> Create()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Chef");
            ViewBag.Chef = usersInRole;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDepartement,NomDepartementt,ApplicationUserId")] Departement departement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(First));
            }
            return View(departement);
        }

        // GET: Responsable/Departements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departements == null)
            {
                return NotFound();
            }
            var departement = await _context.Departements.FindAsync(id);
            if (departement == null)
            {
                return NotFound();
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync("Chef");
          
             
              
            ViewData["ApplicationUserId"] = new SelectList(usersInRole, "Id", "NomComplet", departement.ApplicationUserId);

          
           
            return View(departement);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDepartement,NomDepartementt,ApplicationUserId")] Departement departement)
        {
            if (id != departement.IdDepartement)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartementExists(departement.IdDepartement))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(First));
            }
            return View(departement);
        }

        // GET: Responsable/Departements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departements == null)
            {
                return NotFound();
            }

            var departement = await _context.Departements
                .FirstOrDefaultAsync(m => m.IdDepartement == id);
            if (departement == null)
            {
                return NotFound();
            }

            return View(departement);
        }

        // POST: Responsable/Departements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departements == null)
            {
                return Problem("Entity set 'AppDbContext.Departements'  is null.");
            }
            var departement = await _context.Departements.FindAsync(id);
            if (departement != null)
            {
                _context.Departements.Remove(departement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(First));
        }

        private bool DepartementExists(int id)
        {
          return (_context.Departements?.Any(e => e.IdDepartement == id)).GetValueOrDefault();
        }
    }
}
