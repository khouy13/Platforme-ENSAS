using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Identity;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
   
    public class VacatairesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VacatairesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser user;
        public VacatairesController(AppDbContext context, ILogger <VacatairesController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Responsable/Vacataires
        public async Task<IActionResult> First(int pg = 1,int? VId=null, int? DId=null, int? GId = null)
        {
            try
            {
                const int pageSize = 6;
                List<Vacataire> vacataires;
                ViewBag.GId = GId;
                ViewBag.Dept = DId;
                var Departement = _context.Departements.ToList();
                ViewBag.Departements = new SelectList(Departement, "IdDepartement", "NomDepartementt", DId);
                var Grades = _context.Grades.OrderBy(e => e.GradeName).ToList();
                ViewBag.Grades = new SelectList(Grades, "GradeId", "GradeName", GId);
               if (DId.HasValue)
                {
                   if (VId.HasValue){

                    vacataires = await _context.vacataires
                                    .Where(p => p.IdDepartement == DId && p.IdVacataire == VId).Include(g => g.gradeEnseigant)
                                    .ToListAsync();
                    }
                    else if (GId.HasValue)
                    {
                        vacataires = await _context.vacataires
                                                          .Where(p => p.IdDepartement == DId && p.GradeId == GId).Include(g => g.gradeEnseigant).OrderBy(n => n.Nom)
                                                          .ToListAsync();
                    }
                    else
                    {
                        vacataires = await _context.vacataires
                                  .Where(p => p.IdDepartement == DId).Include(g => g.gradeEnseigant).OrderBy(n =>n.Nom)
                                  .ToListAsync();
                    }

                }


              else  if (VId.HasValue)
                {
                   

                    vacataires = await _context.vacataires.Include(g => g.gradeEnseigant).Include(d => d.departement).OrderBy(n => n.Nom)
                     .Where(p =>p.IdVacataire== VId)
                     .ToListAsync();
                  
                }
               else if (GId.HasValue)
                {
                    vacataires = await _context.vacataires
                                                       .Where(p => p.GradeId == GId).Include(g => g.gradeEnseigant).OrderBy(n => n.Nom)
                                                       .ToListAsync();

                }



                else
                {
                    vacataires = await _context.vacataires.Include(g=>g.gradeEnseigant).Include(d=>d.departement).OrderBy(n => n.Nom).ToListAsync();
                  
                }


                if (DId.HasValue)
                {
                    ViewBag.vacataires2 = new SelectList(_context.vacataires.Where(e=>e.IdDepartement==DId).ToList().OrderBy(e => e.Nom), "IdVacataire", "NomComplet", VId);

                }
                else
                {
                    ViewBag.vacataires2 = new SelectList(_context.vacataires.ToList().OrderBy(e => e.Nom), "IdVacataire", "NomComplet", VId);

                }
                // Pagination
                int recsCount = vacataires.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var pagedVacataires = vacataires.Skip(recSkip).Take(pager.PageSize).ToList();

                ViewBag.Pager = pager;
              
                return View(pagedVacataires);
            }
            catch (Exception ex)
            {
                return Problem($"An error occurred: {ex.Message}");
            }
        }




        // GET: Responsable/Vacataires/Create
        [Authorize(Roles = "Directeur,Coordonnateur,Admin,Chef")]
        public IActionResult Create(int? DId=null)
        {
            ViewBag.Dept = DId;
            ViewBag.Departements = _context.Departements.ToList();
            ViewBag.Grades = _context.Grades.OrderBy(e=>e.GradeNomComplet).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Directeur,Coordonnateur,Admin,Chef")]
        public async Task<IActionResult> Create([Bind("IdVacataire,Nom,Prenom,GradeId,Email,specialité,IdDepartement")] Vacataire vacataire)
        {
            if (ModelState.IsValid)
            {
                string userId = HttpContext.Session.GetString("UserId");
                if (userId != null)
                {
                    user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                }
                if (user != null)
                {

                    _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération d'ajout du Vacataire {vacataire.NomComplet}");

                }
                _context.Add(vacataire);
                await _context.SaveChangesAsync();


                if (User.IsInRole("Chef") || User.IsInRole("Coordonnateur"))
                {
                    return RedirectToAction("First", new { DId = vacataire.IdDepartement });
                }
                else
                {
                    return RedirectToAction("First");
                }
            }
            return View(vacataire);
        }

        // GET: Responsable/Vacataires/Edit/5
        [Authorize(Roles = "Directeur,Coordonnateur,Admin,Chef")]
        public async Task<IActionResult> Edit(int? id, int? DId = null)
        {
            ViewBag.Dept = DId;
            if (id == null || _context.vacataires == null)
            {
                return NotFound();
            }
             var departements = _context.Departements.ToList();
          
            ViewBag.Departements = departements;
         
            var vacataire = await _context.vacataires.FindAsync(id);
            ViewBag.Departements = new SelectList(departements, "IdDepartement", "NomDepartementt",vacataire.IdDepartement);

            var Grades = _context.Grades.OrderBy(e=>e.GradeName).ToList();
            ViewBag.grades = new SelectList(Grades, "GradeId", "GradeNomComplet", vacataire.GradeId);
            if (vacataire == null)
            {
                return NotFound();
            }
            return View(vacataire);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Directeur,Coordonnateur,Admin,Chef")]
        public async Task<IActionResult> Edit(int id, [Bind("IdVacataire,Nom,Prenom,GradeId,Email,IdDepartement,specialité")] Vacataire vacataire)
        {
            if (id != vacataire.IdVacataire)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacataire);

                    string userId = HttpContext.Session.GetString("UserId");
                    if (userId != null)
                    {
                        user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
                    }
                    if (user != null)
                    {

                        _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de Modification du Vacataire {vacataire.NomComplet}");

                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacataireExists(vacataire.IdVacataire))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (User.IsInRole("Chef") || User.IsInRole("Coordonnateur"))
                {
                    return RedirectToAction("First", new { DId = vacataire.IdDepartement });
                }
                else
                {
                    return RedirectToAction("First");
                }
            }
            return View(vacataire);
        }

        // GET: Responsable/Vacataires/Delete/5
        [Authorize(Roles = "Directeur,Coordonnateur,Admin,Chef")]
        public async Task<IActionResult> Delete(int? id, int? DId = null)
        {
            ViewBag.Dept = DId;
            if (id == null || _context.vacataires == null)
            {
                return NotFound();
            }

            var vacataire = await _context.vacataires
                .FirstOrDefaultAsync(m => m.IdVacataire == id);
            if (vacataire == null)
            {
                return NotFound();
            }

            return View(vacataire);
        }

        // POST: Responsable/Vacataires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Directeur,Coordonnateur,Admin,Chef")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.vacataires == null)
            {
                return Problem("Entity set 'AppDbContext.vacataires'  is null.");
            }
            var vacataire = await _context.vacataires.FindAsync(id);
            if (vacataire != null)
            {
                _context.vacataires.Remove(vacataire);
            }
            string userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            }
            if (user != null)
            {

                _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de Suppression du Vacataire {vacataire.NomComplet}");

            }
            await _context.SaveChangesAsync();
            if (User.IsInRole("Chef") || User.IsInRole("Coordonnateur"))
            {
                return RedirectToAction("First", new { DId = vacataire.IdDepartement });
            }
            else
            {
                return RedirectToAction("First");
            }
        }

        private bool VacataireExists(int id)
        {
          return (_context.vacataires?.Any(e => e.IdVacataire == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IActionResult EmploiVacataire(int IdVac, int? IdS)
        {
            var emplois = _context.EmploiTemps
                .Include(e => e.Enseignant)
                .Include(g => g.Groupe)
                .Include(n => n.Niveau)
                .Include(M => M.Matiere)
                .Include(l => l.Local)
                .Where(e => e.IdVacataire == IdVac && (IdS == null || e.IdSemestre == IdS))
                .ToList();

            var semestres = _context.semestres.ToList();
            ViewBag.Semestres = semestres;
            ViewBag.IdVac = IdVac;
            int? latestSemestreId = _context.EmploiTemps
                .Where(e => e.IdVacataire == IdVac && e.IdSemestre != null)
                .OrderByDescending(e => e.IdSemestre)
                .Select(e => e.IdSemestre)
                .FirstOrDefault();

            int selectedSemesterOrDefault = IdS ?? latestSemestreId ?? 0;
            ViewBag.selectedSemester = selectedSemesterOrDefault;
            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;
            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            var vacataire = _context.vacataires.Find(IdVac);

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.vacataire = vacataire;

            return View(emplois);
        }
        //GeneratePdf
        [HttpGet]
        public IActionResult GeneratePdf(int IdVac, int? IdS)
        {
            var emplois = _context.EmploiTemps
           
           .Include(g => g.Groupe)
           .Include(m => m.Matiere)
           .Include(l => l.Local)
           .Include(t => t.TypeEnseignement)
           .Where(e => e.IdVacataire == IdVac && e.IdSemestre == IdS)
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
            var Vacataire = _context.vacataires.FirstOrDefault(l => l.IdVacataire == IdVac);


            // Créez un modèle avec les données
            var model = new EmploiViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                NomVacataire = Vacataire?.NomComplet ?? "Aucun vacataire disponible",

            Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdf", model);
        }
    }
}
