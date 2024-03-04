using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordenateur.Controllers;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;

namespace Projet.Areas.Coordonnateur.Controllers
{
    [Area("Coordonnateur")]
   
    public class ExamenController : Controller
    {
        private readonly AppDbContext _context;
        private ApplicationUser user;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EmploiController> _logger;
        public ExamenController(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger<EmploiController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        [Authorize(Roles = "Directeur, Coordonnateur,Enseignant,Chef,Admin")]
        // GET: Coordonnateur/Examen
        public async Task<IActionResult> DsExam(int pg = 1)
        {
            const int pageSize = 6;
            var appDbContext = _context.Examens.Include(e => e.semestre);
          var examens= await appDbContext.OrderBy(e=>e.DateExamen).ToListAsync();

            // Pagination
            int recsCount = examens.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var pagedExamens = examens.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;


            return View(pagedExamens);
        }


        [Authorize(Roles = "Directeur,Admin")]
        // GET: Coordonnateur/Examen/Create
        public IActionResult Create()
        {
            ViewData["IdSemestre"] = new SelectList(_context.semestres, "IdSemestre", "NomSemestre");
            return View();
        }


        [Authorize(Roles = "Directeur,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExamen,DateExamen,NumeroExamen,IdSemestre")] Examen examen)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }
            if (ModelState.IsValid)
            {
                _context.Add(examen);
                await _context.SaveChangesAsync();
                if (user != null && user.Enseignant != null)
                {

                    _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération d'ajout d un examens ");

                }
                return RedirectToAction(nameof(DsExam));
            }
            ViewData["IdSemestre"] = new SelectList(_context.semestres, "IdSemestre", "NomSemestre", examen.IdSemestre);
            return View(examen);
        }

        [Authorize(Roles = "Directeur,Admin")]
        // GET: Coordonnateur/Examen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Examens == null)
            {
                return NotFound();
            }

            var examen = await _context.Examens.FindAsync(id);
            if (examen == null)
            {
                return NotFound();
            }
            ViewData["IdSemestre"] = new SelectList(_context.semestres, "IdSemestre", "NomSemestre", examen.IdSemestre);
            return View(examen);
        }


        [Authorize(Roles = "Directeur,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExamen,DateExamen,NumeroExamen,IdSemestre")] Examen examen)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }
            if (id != examen.IdExamen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examen);
                    if (user != null && user.Enseignant != null)
                    {

                        _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération de modifacation d un examens ");

                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamenExists(examen.IdExamen))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DsExam));
            }
            ViewData["IdSemestre"] = new SelectList(_context.semestres, "IdSemestre", "NomSemestre", examen.IdSemestre);
            return View(examen);
        }

        [Authorize(Roles = "Directeur,Admin")]

        // GET: Coordonnateur/Examen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Examens == null)
            {
                return NotFound();
            }

            var examen = await _context.Examens
                .Include(e => e.semestre)
                .FirstOrDefaultAsync(m => m.IdExamen == id);
            if (examen == null)
            {
                return NotFound();
            }

            return View(examen);
        }

        [Authorize(Roles = "Directeur,Admin")]
        // POST: Coordonnateur/Examen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }
            if (_context.Examens == null)
            {
                return Problem("Entity set 'AppDbContext.Examens'  is null.");
            }
            var examen = await _context.Examens.FindAsync(id);
            if (examen != null)
            {
                _context.Examens.Remove(examen);
                if (user != null && user.Enseignant != null)
                {

                    _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération de suppression d un examens ");

                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DsExam));
        }

        private bool ExamenExists(int id)
        {
          return (_context.Examens?.Any(e => e.IdExamen == id)).GetValueOrDefault();
        }
    }
}
