using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    [Authorize(Roles = "Admin")]
    public class SemestresController : Controller
    {
        private readonly AppDbContext _context;

        public SemestresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Responsable/Semestres
        public async Task<IActionResult> Index()
        {
              return _context.semestres != null ? 
                          View(await _context.semestres.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.semestres'  is null.");
        }

        // GET: Responsable/Semestres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.semestres == null)
            {
                return NotFound();
            }

            var semestre = await _context.semestres
                .FirstOrDefaultAsync(m => m.IdSemestre == id);
            if (semestre == null)
            {
                return NotFound();
            }

            return View(semestre);
        }

        // GET: Responsable/Semestres/Create
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSemestre,NomSemestre,SemaineDebut,DateDebut,DateFin,SemaineFin")] Semestre semestre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(semestre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(semestre);
        }

        // GET: Responsable/Semestres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.semestres == null)
            {
                return NotFound();
            }

            var semestre = await _context.semestres.FindAsync(id);
            if (semestre == null)
            {
                return NotFound();
            }
            return View(semestre);
        }

        // POST: Responsable/Semestres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSemestre,NomSemestre,SemaineDebut,DateDebut,DateFin,SemaineFin")] Semestre semestre)
        {
            if (id != semestre.IdSemestre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semestre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemestreExists(semestre.IdSemestre))
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
            return View(semestre);
        }

        // GET: Responsable/Semestres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.semestres == null)
            {
                return NotFound();
            }

            var semestre = await _context.semestres
                .FirstOrDefaultAsync(m => m.IdSemestre == id);
            if (semestre == null)
            {
                return NotFound();
            }

            return View(semestre);
        }

        // POST: Responsable/Semestres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.semestres == null)
            {
                return Problem("Entity set 'AppDbContext.semestres'  is null.");
            }
            var semestre = await _context.semestres.FindAsync(id);
            if (semestre != null)
            {
                _context.semestres.Remove(semestre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemestreExists(int id)
        {
          return (_context.semestres?.Any(e => e.IdSemestre == id)).GetValueOrDefault();
        }
    }
}
