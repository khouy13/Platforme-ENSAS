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
    public class NiveauxController : Controller
    {
        private readonly AppDbContext _context;

        public NiveauxController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchText = "", int pg = 1, string orderBy = "filiere")
        {
            var niveauxQuery = _context.Niveaus.Include(n => n.filiere).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                niveauxQuery = niveauxQuery
                    .Where(n => n.NomNiveau.Contains(searchText) || n.filiere.NomFiliere.Contains(searchText));
            }

            var totalNiveaux = await niveauxQuery.CountAsync();
            const int pageSize = 5; // Nombre de niveaux par page
            var pager = new Pager(totalNiveaux, pg, pageSize);
            int skip = (pg - 1) * pageSize;

            // Tri des niveaux en fonction de la valeur de orderBy
            var pagedNiveaux = orderBy switch
            {
                "filiere" => await niveauxQuery.Skip(skip).Take(pageSize).OrderBy(n => n.filiere.NomFiliere).ToListAsync(),
                "nonfiliere" => await niveauxQuery.Skip(skip).Take(pageSize).OrderBy(n => n.NomNiveau).ToListAsync(),
                _ => await niveauxQuery.Skip(skip).Take(pageSize).ToListAsync() // Tri par défaut si orderBy n'est pas reconnu
            };

            ViewBag.Pager = pager;
            ViewBag.SearchText = searchText;
            ViewBag.OrderBy = orderBy; // Passer la valeur orderBy à la vue

            return View(pagedNiveaux);
        }




        // GET: Responsable/Niveaux/Create
        public IActionResult Create()
        {
            

            ViewData["IdFiliere"] = new SelectList(_context.Filieres, "IdFiliere", "NomFiliere");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNiveau,NomNiveau,IdFiliere")] Niveau niveau)
        {
            if (ModelState.IsValid)
            {
                _context.Add(niveau);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdFiliere"] = new SelectList(_context.Filieres, "IdFiliere", "NomFiliere");
            return View(niveau);
        }

        // GET: Responsable/Niveaux/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Niveaus == null)
            {
                return NotFound();
            }

            var niveau = await _context.Niveaus.FindAsync(id);
            if (niveau == null)
            {
                return NotFound();
            }
            ViewData["IdFiliere"] = new SelectList(_context.Filieres, "IdFiliere", "NomFiliere");
            return View(niveau);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNiveau,NomNiveau,IdFiliere")] Niveau niveau)
        {
            if (id != niveau.IdNiveau)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(niveau);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NiveauExists(niveau.IdNiveau))
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
            ViewData["IdFiliere"] = new SelectList(_context.Filieres, "IdFiliere", "IdFiliere", niveau.IdFiliere);
            return View(niveau);
        }

        // GET: Responsable/Niveaux/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Niveaus == null)
            {
                return NotFound();
            }

            var niveau = await _context.Niveaus
                .Include(n => n.filiere)
                .FirstOrDefaultAsync(m => m.IdNiveau == id);
            if (niveau == null)
            {
                return NotFound();
            }

            return View(niveau);
        }

        // POST: Responsable/Niveaux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Niveaus == null)
            {
                return Problem("Entity set 'AppDbContext.Niveaus'  is null.");
            }
            var niveau = await _context.Niveaus.FindAsync(id);
            if (niveau != null)
            {
                _context.Niveaus.Remove(niveau);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NiveauExists(int id)
        {
          return (_context.Niveaus?.Any(e => e.IdNiveau == id)).GetValueOrDefault();
        }
    }
}
