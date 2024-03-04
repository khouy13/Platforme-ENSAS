using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    public class TypeLocalsController : Controller
    {
        private readonly AppDbContext _context;

        public TypeLocalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Responsable/TypeLocals
        public async Task<IActionResult> Index()
        {
              return _context.TypeLocals != null ? 
                          View(await _context.TypeLocals.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.TypeLocals'  is null.");
        }

        // GET: Responsable/TypeLocals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TypeLocals == null)
            {
                return NotFound();
            }

            var typeLocal = await _context.TypeLocals
                .FirstOrDefaultAsync(m => m.IdTypeLocal == id);
            if (typeLocal == null)
            {
                return NotFound();
            }

            return View(typeLocal);
        }

        // GET: Responsable/TypeLocals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Responsable/TypeLocals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTypeLocal,Nom")] TypeLocal typeLocal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeLocal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeLocal);
        }

        // GET: Responsable/TypeLocals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TypeLocals == null)
            {
                return NotFound();
            }

            var typeLocal = await _context.TypeLocals.FindAsync(id);
            if (typeLocal == null)
            {
                return NotFound();
            }
            return View(typeLocal);
        }

        // POST: Responsable/TypeLocals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTypeLocal,Nom")] TypeLocal typeLocal)
        {
            if (id != typeLocal.IdTypeLocal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeLocal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeLocalExists(typeLocal.IdTypeLocal))
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
            return View(typeLocal);
        }

        // GET: Responsable/TypeLocals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TypeLocals == null)
            {
                return NotFound();
            }

            var typeLocal = await _context.TypeLocals
                .FirstOrDefaultAsync(m => m.IdTypeLocal == id);
            if (typeLocal == null)
            {
                return NotFound();
            }

            return View(typeLocal);
        }

        // POST: Responsable/TypeLocals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TypeLocals == null)
            {
                return Problem("Entity set 'AppDbContext.TypeLocals'  is null.");
            }
            var typeLocal = await _context.TypeLocals.FindAsync(id);
            if (typeLocal != null)
            {
                _context.TypeLocals.Remove(typeLocal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeLocalExists(int id)
        {
          return (_context.TypeLocals?.Any(e => e.IdTypeLocal == id)).GetValueOrDefault();
        }
    }
}
