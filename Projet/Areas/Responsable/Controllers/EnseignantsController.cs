using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using OfficeOpenXml;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using Rotativa.AspNetCore;
using Projet.Areas.Coordenateur.Controllers;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;

namespace Projet.Areas.Admin.Controllers
{
    [Area("Responsable")]
  
    public class EnseignantsController : Controller
    {
  
        private readonly AppDbContext _context;
        private readonly ILogger<EnseignantsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser user;
        public EnseignantsController(AppDbContext context, ILogger<EnseignantsController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
           

        }

        // GET: Admin/Enseignants
        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]

        public async Task<IActionResult> First(int pg = 1,int? EId = null,int? DId = null,int? GId=null)
        {
         

            ViewBag.Dept = DId ;
            ViewBag.GId = GId;
            var Departement = _context.Departements.ToList();
            ViewBag.Departements = new SelectList(Departement, "IdDepartement", "NomDepartementt", DId);
            var Grades = _context.Grades.ToList();
            ViewBag.Grades = new SelectList(Grades, "GradeId", "GradeName", GId);
            try
            {
                const int pageSize = 6;
                List<Enseignant> enseignants;

                  if (DId.HasValue)
                
                {
                    if (EId.HasValue)
                    {
                     enseignants = await _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).OrderBy(n=>n.NomEnseignant)
                     .Where(p => p.IdDepartement == DId && p.IdEnseignant == EId)
                     .ToListAsync();
                    }
                    else if (GId.HasValue)
                    {
                        enseignants = await _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).OrderBy(n => n.NomEnseignant)
                                                .Where(p => p.GradeId == GId && p.IdDepartement == DId)
                                                .ToListAsync();
                    }
                    else { 
                    enseignants = await _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).OrderBy(n => n.NomEnseignant)
                        .Where(p => p.IdDepartement == DId)
                        .ToListAsync();
                    }
                }
                else if (EId.HasValue)
                {
                 
                    enseignants = await _context.Enseignants.Include(e => e.departement).Include(g=>g.gradeEnseigant).OrderBy(n => n.NomEnseignant)
                        .Where(p =>p.IdEnseignant==EId)
                        .ToListAsync();
                }
              
                else if (GId.HasValue)
                {
                    enseignants = await _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).OrderBy(n => n.NomEnseignant)
                                            .Where(p => p.GradeId == GId)
                                            .ToListAsync();
                }
                else
                {
                    enseignants = await _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).OrderBy(n => n.NomEnseignant).ToListAsync();
                }



                if (DId.HasValue)
                {
                    ViewBag.enseignants2 = new SelectList(_context.Enseignants.Where(e=>e.IdDepartement==DId).OrderBy(e => e.NomEnseignant).ToList(), "IdEnseignant", "NomComplet", EId);
                }
                else
                {
                    ViewBag.enseignants2 = new SelectList(_context.Enseignants.OrderBy(e =>e.NomEnseignant).ToList(), "IdEnseignant", "NomComplet", EId);

                }
                // Pagination
                int recsCount = enseignants.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var pagedEnseignants = enseignants.Skip(recSkip).Take(pager.PageSize).ToList();

                ViewBag.Pager = pager;
            

                return View(pagedEnseignants);
            }
            catch (Exception ex)
            {
                return Problem($"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        public async Task<IActionResult> Create(int? DId = null)
        {
            ViewBag.Dept = DId;
            ViewBag.Departements=_context.Departements.ToList();
            ViewBag.Grades = _context.Grades.OrderBy(e=>e.GradeNomComplet).ToList();
            return View();
        }


        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEnseignant,NomEnseignant,PrenomEnseignant,SpecialiteEnseignant,IdDepartement,GradeId,Email")] Enseignant enseignant)
        {
            if (ModelState.IsValid)
            {

                string userId = HttpContext.Session.GetString("UserId");

                if (userId != null)
                {
                    user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                }
                // Vérification d'unicité de l'adresse e-mail
                bool isEmailUnique = await _context.Enseignants.AllAsync(e => e.Email != enseignant.Email);

                if (!isEmailUnique)
                {
                    ModelState.AddModelError("Email", "L'adresse e-mail existe déjà.");
                   
                    return View(enseignant);
                }

                _context.Add(enseignant);
                await _context.SaveChangesAsync();
                if (user != null )
                {

                    _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération d'ajout de l 'Enseignant {enseignant.NomComplet}");

                }

                if (User.IsInRole("Chef") || User.IsInRole("Coordonnateur"))
                {
                    return RedirectToAction("First", new { DId = enseignant.IdDepartement });
                }
                else
                {
                    return RedirectToAction("First");
                }
                   
            }

            return View(enseignant);
        }


        // GET: Admin/Enseignants/Edit/5
        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        public async Task<IActionResult> Edit(int? id ,int ? DId=null)
        {
            ViewBag.Dept = DId;
            if (id == null || _context.Enseignants == null)
            {
                return NotFound();
            }

            var enseignant = await _context.Enseignants.Include(d => d.departement).FirstOrDefaultAsync(e => e.IdEnseignant == id);
            if (enseignant == null)
            {
                return NotFound();
            }

            var departements = _context.Departements.ToList();
          
            
            ViewBag.Departements = new SelectList(departements, "IdDepartement", "NomDepartementt", enseignant.IdDepartement);
             var Grades = _context.Grades.ToList();
            ViewBag.grades= new SelectList(Grades, "GradeId", "GradeNomComplet", enseignant.GradeId);
            return View(enseignant); // Assurez-vous de transmettre l'objet enseignant à la vue
        }



        [HttpPost]
        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        public async Task<IActionResult> Edit(int id, [Bind("IdEnseignant,NomEnseignant,PrenomEnseignant,Email,SpecialiteEnseignant,IdDepartement,GradeId")] Enseignant enseignant)
        {
            if (id != enseignant.IdEnseignant)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string userId = HttpContext.Session.GetString("UserId");

                    if (userId != null)
                    {
                        user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                    }
                    _context.Update(enseignant);
                    await _context.SaveChangesAsync();
                    if (user != null)
                    {

                        _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de Modification de l 'Enseignant {enseignant.NomComplet}");

                    }
                    if (User.IsInRole("Chef") || User.IsInRole("Coordonnateur"))
                    {
                        // Inclure DId dans les paramètres de redirection
                        return RedirectToAction("First", "Enseignants", new { DId = enseignant.IdDepartement });
                    }
                    else
                    {
                        return RedirectToAction("First", "Enseignants");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnseignantExists(enseignant.IdEnseignant))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                
             
            }
            return View(enseignant);
        }


        // GET: Admin/Enseignants/Delete/5
       
        public async Task<IActionResult> Delete(int? id, int? DId = null)
        {
            ViewBag.Dept = DId;
            if (id == null || _context.Enseignants == null)
            {
                return NotFound();
            }
           
            var enseignant = await _context.Enseignants.Include(d=>d.departement)
        .FirstOrDefaultAsync(m => m.IdEnseignant == id);
            if (enseignant == null)
            {
                return NotFound();
            }

            return View(enseignant);
        }

        // POST: Admin/Enseignants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enseignants == null)
            {
                return Problem("Entity set 'AppDbContext.Enseignants'  is null.");
            }
            var enseignant = await _context.Enseignants.FindAsync(id);
            if (enseignant != null)
            {
                _context.Enseignants.Remove(enseignant);
            }
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            }
            if (user != null)
            {

                _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de Suppression de l 'Enseignant {enseignant.NomComplet}");

            }

            await _context.SaveChangesAsync();
            if (User.IsInRole("Chef") || User.IsInRole("Coordonnateur"))
            {
                return RedirectToAction("First", new { DId = enseignant.IdDepartement });
            }
            else
            {
                return RedirectToAction("First", "Enseignants");
            }
         
        }

        private bool EnseignantExists(int id)
        {
          return (_context.Enseignants?.Any(e => e.IdEnseignant == id)).GetValueOrDefault();
        }

        //Excel to DataBase:
        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        public async Task<IActionResult> ImportAndStoreEnseignants(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return RedirectToAction("First", "Enseignants");
            }

            var enseignantsList = new List<Enseignant>();
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
                                enseignantsList.Add(new Enseignant
                                {
                                    NomEnseignant = worksheet.Cells[row, 1].Value?.ToString()?.Trim(),
                                    PrenomEnseignant = worksheet.Cells[row, 2].Value?.ToString()?.Trim(),
                                    Email= worksheet.Cells[row, 3].Value?.ToString()?.Trim(),
                                    SpecialiteEnseignant = worksheet.Cells[row, 4].Value?.ToString()?.Trim(),
                                    IdDepartement = int.Parse(worksheet.Cells[row, 5].Value?.ToString()?.Trim()),
                                   
                                });
                            }
                            catch (FormatException)
                            {
                                ModelState.AddModelError("Excel", "Invalid data format in the Excel file.");
                                return RedirectToAction("First", "Enseignants");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("File", "Error processing the file.");
                    return RedirectToAction("First", "Enseignants");
                }
            }

            _context.Enseignants.AddRange(enseignantsList);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Data imported and saved successfully.";
            return RedirectToAction("First", "Enseignants");
        }

        //Pour Emploi Enseignant :

        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        [HttpGet]
        public IActionResult EmploiEnseignant(int IdEns, int? IdS)
        {
            var emplois = _context.EmploiTemps
                .Include(e => e.Enseignant)
                .Include(g => g.Groupe)
                .Include(n => n.Niveau)
                .Include(M => M.Matiere)
                .Include(l => l.Local)
                .Where(e => e.IdEnseignant == IdEns && (IdS == null || e.IdSemestre == IdS))
                .ToList();

            var semestres = _context.semestres.ToList();
            ViewBag.Semestres = semestres;
            ViewBag.IdE = IdEns;

            int? latestSemestreId = _context.EmploiTemps
                .Where(e => e.IdEnseignant == IdEns && e.IdSemestre != null)
                .OrderByDescending(e => e.IdSemestre)
                .Select(e => e.IdSemestre)
                .FirstOrDefault();

            int selectedSemesterOrDefault = IdS ?? latestSemestreId ?? 0;
            ViewBag.selectedSemester = selectedSemesterOrDefault;
            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;
            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            var enseignant = _context.Enseignants.Find(IdEns);

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.Enseignant = enseignant;

            return View(emplois);
        }
        [Authorize(Roles = "Directeur, Coordonnateur, Admin,Chef")]
        //GeneratePdf
        [HttpGet]
        public IActionResult GeneratePdf(int IdEns, int? IdS)
        {
            var emplois = _context.EmploiTemps

           .Include(g => g.Groupe)
           .Include(m => m.Matiere)
           .Include(l => l.Local).Include(e=>e.Niveau)
           .Include(t => t.TypeEnseignement)
           .Where(e => e.IdEnseignant == IdEns && e.IdSemestre == IdS)
           .ToList();
            int anneeDebut;
            int anneeFin;

            if (DateTime.Now.Month >= 9)
            {
                anneeDebut = DateTime.Now.Year;
                anneeFin = anneeDebut + 1;
            }
            else
            {
                anneeFin = DateTime.Now.Year;
                anneeDebut = anneeFin - 1;
            }
            var nomAnneeScolaire = $"{anneeDebut}/{anneeFin}";
            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            var semestre = _context.semestres.FirstOrDefault(s => s.IdSemestre == IdS);
            var enseignant = _context.Enseignants.FirstOrDefault(l => l.IdEnseignant == IdEns);


            // Créez un modèle avec les données
            var model = new EmploiViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                NomEnseignant = enseignant?.NomComplet ?? "Aucun Enseignant disponible",

                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdf", model);
        }

    }
}
