using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    public class GradesController : Controller
    {
        private readonly AppDbContext _context;

        public GradesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Responsable/Grades
        public async Task<IActionResult> Index(int pg = 1, string? SearchText = "")
        {
            const int pageSize = 6;
            List<Grade> Grades;
            if (!string.IsNullOrEmpty(SearchText))
            {
                string searchTextLower = SearchText.ToLower();


                // Faire correspondre en ignorant la casse
                Grades = await _context.Grades
                    .Where(grade =>
                        grade.GradeNomComplet.ToLower().Contains(searchTextLower) ||
                        grade.GradeName.ToLower().Contains(searchTextLower) ||
                        grade.GradeNomComplet.ToLower().Replace(" ", "").Contains(searchTextLower.Replace(" ", "")) ||
                        grade.GradeName.ToLower().Replace(" ", "").Contains(searchTextLower.Replace(" ", ""))
                    )
                    .ToListAsync();
            }
            else
            {
                Grades = await _context.Grades.ToListAsync();
            }

                int recsCount = Grades.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var pagedGrades = Grades.Skip(recSkip).Take(pager.PageSize).ToList();
                ViewBag.Pager = pager;
                return View(pagedGrades);


            }
        
        // GET: Responsable/Grades/Create
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradeId,GradeName,GradeNomComplet")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grade);
        }

        // GET: Responsable/Grades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradeId,GradeName,GradeNomComplet")] Grade grade)
        {
            if (id != grade.GradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.GradeId))
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
            return View(grade);
        }

        // GET: Responsable/Grades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .FirstOrDefaultAsync(m => m.GradeId == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: Responsable/Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Grades == null)
            {
                return Problem("Entity set 'AppDbContext.Grades'  is null.");
            }
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradeExists(int id)
        {
          return (_context.Grades?.Any(e => e.GradeId == id)).GetValueOrDefault();
        }
    }
}
