using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordenateur.Controllers;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]

    public class MatieresController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MatieresController> _logger;
        public MatieresController(AppDbContext context, ILogger<MatieresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Responsable/Matieres
        public async Task<IActionResult> First(int pg = 1, int? MId = null, int? NiveauFilter = null)
        {
            try
            {
                const int pageSize = 8;

                // Récupérer la liste des niveaux pour la vue
                var niveaus = await _context.Niveaus.ToListAsync();
                ViewBag.Niveaus = new SelectList(niveaus, "IdNiveau", "NomNiveau", NiveauFilter);
                ViewBag.NiveauFilter = NiveauFilter;
              
                ViewBag.matieres = new SelectList(_context.Matieres.ToList(),"IdMatiere", "NomMatiere", MId);
                List<Matiere> matieres;

                // Filtrer les matières en fonction des paramètres MId et NiveauFilter
                if (MId.HasValue)
                {
                    matieres = await _context.Matieres.Include(e=>e.Enseignant).Include(v=>v.Vacataire).Include(m => m.MatiereNiveaus)
                                                      .ThenInclude(mn => mn.Niveau)
                                                      .Where(p => p.IdMatiere == MId)
                                                      .ToListAsync();
                }
                else if (NiveauFilter.HasValue)
                {
                    matieres = await _context.Matieres.Include(e => e.Enseignant).Include(v => v.Vacataire).Include(m => m.MatiereNiveaus)
                                                      .ThenInclude(mn => mn.Niveau)
                                                      .Where(m => m.MatiereNiveaus.Any(mn => mn.Niveau.IdNiveau == NiveauFilter.Value))
                                                      .ToListAsync();
                }
                else
                {
                    matieres = await _context.Matieres.Include(e => e.Enseignant).Include(v => v.Vacataire).Include(m => m.MatiereNiveaus)
                                                      .ThenInclude(mn => mn.Niveau)
                                                      .ToListAsync();
                }

                // Pagination
                int recsCount = matieres.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var pagedMatieres = matieres.Skip(recSkip).Take(pager.PageSize).ToList();

                ViewBag.Pager = pager;
                ViewBag.NiveauFilter = NiveauFilter;

                return View(pagedMatieres);
            }
            catch (Exception ex)
            {
                return Problem($"An error occurred: {ex.Message}");
            }
        }


        [Authorize(Roles = "Admin,Directeur")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Niveau = await _context.Niveaus.ToListAsync();
            ViewBag.enseignats = await _context.Enseignants.ToListAsync();
            ViewBag.vacataires= await _context.vacataires.ToListAsync();
         
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Directeur")]
        public async Task<IActionResult> Create([Bind("IdMatiere,NomMatiere,IdNiveau,IdEnseignant,IdVacataire")] Matiere matiere, List<int>? MatiereNiveaus)
        {
            if (ModelState.IsValid)
            {
                matiere.MatiereNiveaus= new List<MatiereNiveau>();
                foreach (var NiveauId in MatiereNiveaus)
                {
                    matiere.MatiereNiveaus.Add(new MatiereNiveau {IdNiveau= NiveauId });
                }
                _context.Add(matiere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(First));
            }

            // Si la validation échoue, réinitialisez ViewBag.Niveau et retournez à la vue avec l'objet matiere
            ViewBag.Niveau = await _context.Niveaus.ToListAsync();
            return View();
        }


        // GET: Responsable/Matieres/Edit/5
        [Authorize(Roles = "Admin,Directeur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Matieres == null)
            {
                return NotFound();
            }
            ViewBag.Niveau = await _context.Niveaus.Include(e=>e.NiveauMatieres).ToListAsync();
     
            var matiere = await _context.Matieres.Include(e=>e.MatiereNiveaus).ThenInclude(e=>e.Niveau).ThenInclude(e=>e.NiveauMatieres).FirstOrDefaultAsync(e=>e.IdMatiere==id);
            ViewBag.enseignats = new SelectList(_context.Enseignants.ToList(), "IdEnseignant", "NomComplet", matiere.IdEnseignant);
            ViewBag.vacataires= new SelectList(_context.vacataires.ToList(), "IdVacataire", "NomComplet", matiere.IdVacataire);
            if (matiere == null)
            {
                return NotFound();
            }
            return View(matiere);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Directeur")]
        public async Task<IActionResult> Edit(int id, [Bind("IdMatiere,NomMatiere,IdVacataire,IdEnseignant")] Matiere matiere, List<int>? MatiereNiveaus)
        {
            if (id != matiere.IdMatiere)
            {
                return NotFound();
            }

             if (ModelState.IsValid)
            {
                try
                {
                    // Charger la matière existante avec ses relations
                    var existingMatiere = await _context.Matieres
                        .Include(m => m.MatiereNiveaus) // Charger les relations MatiereNiveaus
                        .FirstOrDefaultAsync(m => m.IdMatiere == id);

                    if (existingMatiere == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour les propriétés de la matière
                    existingMatiere.NomMatiere = matiere.NomMatiere;
                    existingMatiere.IdEnseignant = matiere.IdEnseignant;
                    existingMatiere.IdVacataire= matiere.IdVacataire;
                    // Retirer les anciennes associations
                    existingMatiere.MatiereNiveaus.Clear();

                    // Ajouter les nouvelles associations
                    if (MatiereNiveaus != null)
                    {
                        foreach (var niveauId in MatiereNiveaus)
                        {
                            existingMatiere.MatiereNiveaus.Add(new MatiereNiveau { IdNiveau = niveauId });
                        }
                    }

                    _context.Update(existingMatiere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatiereExists(matiere.IdMatiere))
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

            return View(matiere);
        }



        // GET: Responsable/Matieres/Delete/5
        [Authorize(Roles = "Admin,Directeur")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Matieres == null)
            {
                return NotFound();
            }

            var matiere = await _context.Matieres.Include(d => d.MatiereNiveaus).ThenInclude(e=>e.Niveau)
                .FirstOrDefaultAsync(m => m.IdMatiere == id);
            if (matiere == null)
            {
                return NotFound();
            }

            return View(matiere);
        }
        [Authorize(Roles = "Admin,Directeur")]
        // POST: Responsable/Matieres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Retrieve the matiere
                var matiere = await _context.Matieres.Include(m => m.MatiereNiveaus).ThenInclude(n => n.Niveau).ThenInclude(e => e.NiveauMatieres).FirstOrDefaultAsync(m => m.IdMatiere == id);

                if (matiere == null)
                {
                    return NotFound();
                }
                var MatiereNiveau = _context.MatiereNiveaus.Where(e => e.IdMatiere == id).ToList();
                var emploitempsMatiere=_context.EmploiTemps.Where(e=>e.IdMatiere==id).ToList();
                var emploitempsExam = _context.EmploiExams.Where(e => e.IdMatiere == id).ToList();
                if (emploitempsExam != null)
                {
                    _context.EmploiTemps.RemoveRange(emploitempsMatiere);
                }
                if(emploitempsExam != null)
                {
                    _context.EmploiExams.RemoveRange(emploitempsExam);
                }
                // Remove associations (MatiereNiveaus)
                if (MatiereNiveau != null)
                {
                    _context.MatiereNiveaus.RemoveRange(MatiereNiveau);

                    _logger.LogInformation("An error occurred while deleting matiere");
                }
                // Remove the matiere
                _context.Matieres.Remove(matiere);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(First));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"An error occurred while deleting: {ex.Message}");
                return Problem($"An error occurred while deleting: {ex.Message}");
            }

        }

        private bool MatiereExists(int id)
        {
            return (_context.Matieres?.Any(e => e.IdMatiere == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Admin,Directeur")]
        public async Task<IActionResult> ImportAndStoreMatiers(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return RedirectToAction("First", "Matieres");
            }

            var MatieresList = new List<Matiere>();
            using (var stream = new MemoryStream())
            {
                try
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                MatieresList.Add(new Matiere
                                {
                                    NomMatiere = worksheet.Cells[row, 1].Value?.ToString()?.Trim(),


                                });
                            }
                            catch (FormatException)
                            {
                                ModelState.AddModelError("Excel", "Invalid data format in the Excel file.");
                                return RedirectToAction("First", "Matieres");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("File", "Error processing the file.");
                    return RedirectToAction("First", "Matieres");
                }
            }

            _context.Matieres.AddRange(MatieresList);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Data imported and saved successfully.";
            return RedirectToAction("First", "Matieres");
        }
        //EmploiMatiere

        [HttpGet]

        public IActionResult EmploiMatiere(int id)
        {
            var emplois = _context.EmploiTemps.Include(e => e.Enseignant).Include(g => g.Groupe).Include(n => n.Niveau).Include(M => M.Matiere).Include(t=>t.TypeEnseignement).Include(l => l.Local).Where(e => e.IdMatiere == id).ToList();

            var jours = _context.Jours.ToList();

            var seances = _context.Seances.ToList();

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;

            var Matiere = _context.Matieres.Find(id);
            ViewBag.Matiere = Matiere;



            return View(emplois);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Directeur")]
        public IActionResult MatieresCommuns()
        {
            return View();
        }
    }
}
