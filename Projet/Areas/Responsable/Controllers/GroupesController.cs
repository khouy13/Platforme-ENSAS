using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    [Authorize(Roles = "Admin")]
    public class GroupesController : Controller
    {
        private readonly AppDbContext _context;

        public GroupesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Responsable/Groupes
        public async Task<IActionResult> Index(string searchText = "", int pg = 1)
        {
            var query = _context.Groupes.Include(g => g.niveau).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(g => g.NomGroup.Contains(searchText) || g.niveau.NomNiveau.Contains(searchText));
            }

            var totalGroups = await query.CountAsync();
            const int pageSize =6; // Nombre de groupes par page
            var pager = new Pager(totalGroups, pg, pageSize);
            var pagedGroups = await query
                .OrderBy(g => g.niveau.NomNiveau) // Tri par NomNiveau
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Pager = pager;
            ViewBag.SearchText = searchText;

            return View(pagedGroups);
        }



        // GET: Responsable/Groupes/Create
        public IActionResult Create()
        {
            ViewData["IdNiveau"] = new SelectList(_context.Niveaus, "IdNiveau", "NomNiveau");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGroupe,NomGroup,IdNiveau")] Groupe groupe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNiveau"] = new SelectList(_context.Niveaus, "IdNiveau", "IdNiveau", groupe.IdNiveau);
            return View(groupe);
        }

        // GET: Responsable/Groupes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groupes == null)
            {
                return NotFound();
            }

            var groupe = await _context.Groupes.FindAsync(id);
            if (groupe == null)
            {
                return NotFound();
            }
            ViewData["IdNiveau"] = new SelectList(_context.Niveaus, "IdNiveau", "IdNiveau", groupe.IdNiveau);
            return View(groupe);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGroupe,NomGroup,IdNiveau")] Groupe groupe)
        {
            if (id != groupe.IdGroupe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupeExists(groupe.IdGroupe))
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
            ViewData["IdNiveau"] = new SelectList(_context.Niveaus, "IdNiveau", "NomNiveau");
            return View(groupe);
        }

        // GET: Responsable/Groupes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groupes == null)
            {
                return NotFound();
            }

            var groupe = await _context.Groupes
                .Include(g => g.niveau)
                .FirstOrDefaultAsync(m => m.IdGroupe == id);
            if (groupe == null)
            {
                return NotFound();
            }

            return View(groupe);
        }

        // POST: Responsable/Groupes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groupes == null)
            {
                return Problem("Entity set 'AppDbContext.Groupes'  is null.");
            }
            var groupe = await _context.Groupes.FindAsync(id);
            if (groupe != null)
            {
                _context.Groupes.Remove(groupe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupeExists(int id)
        {
          return (_context.Groupes?.Any(e => e.IdGroupe == id)).GetValueOrDefault();
        }
    }
}
