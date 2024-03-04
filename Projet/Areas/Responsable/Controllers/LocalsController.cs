using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordenateur.Controllers;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using Rotativa.AspNetCore;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
 
    public class LocalsController : Controller
    {

        private readonly AppDbContext _context;
        private readonly ILogger<LocalsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser user;
        public LocalsController(AppDbContext context, ILogger<LocalsController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager; 
        }

        [Authorize(Roles = "Directeur,Secritaire, Admin")]
        // GET: Responsable/Locals
        public async Task<IActionResult> First(int pg = 1,int? LId=null)
        {
            const int pageSize = 6; // Nombre de locaux par page
            List<Local> locals;
           
            if (LId.HasValue)
            {
                locals = await _context.Locals.Include(t=>t.TypeLocal)
                    .Where(l => l.IdLocal==LId)
                    .ToListAsync();
                ViewBag.locals2 = new SelectList(locals, "IdLocal", "NomLocal", LId);
            }
            else
            {
                locals = await _context.Locals.Include(t =>t.TypeLocal).ToListAsync();
                ViewBag.locals2 = new SelectList(locals, "IdLocal", "NomLocal", LId);
            }

            // Pagination
            int totalLocals = locals.Count;
            var pager = new Pager(totalLocals, pg, pageSize);
            int skip = (pg - 1) * pageSize;
            var pagedLocals = locals.Skip(skip).Take(pageSize).ToList();

            ViewBag.Pager = pager;
           

            return View(pagedLocals);
        }


        [Authorize(Roles = "Directeur,Secritaire")]
        // GET: Responsable/Locals/Create
        public IActionResult Create()
        {
            ViewBag.typeLocals = new SelectList(_context.TypeLocals.ToList(), "IdTypeLocal", "Nom");

            return View();
        }
         



        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Directeur, Admin")]
        public async Task<IActionResult> Create([Bind("IdLocal,NomLocal,CapaciteLocal,DescriptionLocal,IdTypeLocal")] Local local)
        {
            if (ModelState.IsValid)
            {
                string userId = HttpContext.Session.GetString("UserId");
                if (userId != null)
                {
                    user = _userManager.Users

                       .FirstOrDefault(u => u.Id == userId);
                }
                if (user != null )
                {

                    _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération d'ajout de:{local.NomLocal}.");

                }
                _context.Add(local);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(First));
            }
            return View(local);
        }

        // GET: Responsable/Locals/Edit/5

        [Authorize(Roles = "Directeur, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Locals == null)
            {
                return NotFound();
            }

            var local = await _context.Locals.FindAsync(id);
 

            if (local == null)
            {
                return NotFound();
            }
            ViewBag.typeLocals = new SelectList(_context.TypeLocals.ToList(), "IdTypeLocal", "Nom", local.IdTypeLocal);
            return View(local);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Directeur, Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("IdLocal,NomLocal,CapaciteLocal,DescriptionLocal,IdTypeLocal")] Local local)
        {
            if (id != local.IdLocal)
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
                        user = _userManager.Users

                           .FirstOrDefault(u => u.Id == userId);
                    }
                    _context.Update(local);
                    if (user != null )
                    {

                        _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de Modification de:{local.NomLocal} .");

                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalExists(local.IdLocal))
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
            return View(local);
        }

        // GET: Responsable/Locals/Delete/5

        [Authorize(Roles = "Directeur, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Locals == null)
            {
                return NotFound();
            }

            var local = await _context.Locals
                .FirstOrDefaultAsync(m => m.IdLocal == id);
            if (local == null)
            {
                return NotFound();
            }

            return View(local);
        }

        // POST: Responsable/Locals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Directeur, Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Locals == null)
            {
                return Problem("Entity set 'AppDbContext.Locals'  is null.");
            }
            var local = await _context.Locals.FindAsync(id);
            if (local != null)
            {
                _context.Locals.Remove(local);
                await _context.SaveChangesAsync();
            }
            string userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                user = _userManager.Users

                   .FirstOrDefault(u => u.Id == userId);
            }
          
            if (user != null)
            {

                _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de Supprission de:{local.NomLocal} .");

            }
           
            return RedirectToAction(nameof(First));
        }

        private bool LocalExists(int id)
        {
          return (_context.Locals?.Any(e => e.IdLocal == id)).GetValueOrDefault();
        }
        //EmploiLocal:pour L emploi du temps du Local:
        [HttpGet]

        public IActionResult EmploiLocal(int idL, int? IdS)
        {
            var emplois = _context.EmploiTemps
                .Include(e => e.Enseignant)
                .Include(g => g.Groupe)
                .Include(n => n.Niveau)
                .Include(l => l.Local).Include(e=>e.TypeEnseignement).Include(m=>m.Matiere)
                .Where(e => e.IdLocal == idL && (IdS == null || e.IdSemestre == IdS))
                .ToList();

            var semestres = _context.semestres.ToList();
            ViewBag.Semestres = semestres;
            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;
            int? latestSemestreId = _context.EmploiTemps
                .Where(e => e.IdLocal == idL && e.IdSemestre != null)
                .OrderByDescending(e => e.IdSemestre)
                .Select(e => e.IdSemestre)
                .FirstOrDefault();

            int selectedSemesterOrDefault = IdS ?? latestSemestreId ?? 0;
            ViewBag.selectedSemester = selectedSemesterOrDefault;
            
            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            var local = _context.Locals.Find(idL);

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.Local = local;
            ViewBag.localId = idL;
            return View(emplois);
        }
        //GeneratePdf
        [HttpGet]

        [Authorize(Roles = "Directeur,Secritaire, Admin")]
        public IActionResult GeneratePdf(int IdL, int IdS)
        {
            var emplois = _context.EmploiTemps
           .Include(e => e.Enseignant).Include(v=>v.Vacataire)
           .Include(g => g.Groupe)
           .Include(m => m.Matiere)
           .Include(l => l.Local).Include(e=>e.Niveau)
           .Include(t => t.TypeEnseignement)
           .Where(e => e.IdLocal == IdL && e.IdSemestre == IdS)
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
            var local = _context.Locals.FirstOrDefault(l=>l.IdLocal== IdL);


            // Créez un modèle avec les données
            var model = new EmploiViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
               
                NomLocal = local?.NomLocal ?? "",
                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdf", model);
        }
        //ExamEmploiLocal
        [HttpGet]

        [Authorize(Roles = "Directeur,Secritaire, Admin")]
        public async Task<IActionResult> ExamEmploiLocal(int idL, int? IdEx)
        {
            var emplois = new List<EmploiExam>();
            ViewBag.examens = new SelectList(await _context.Examens.Include(s=>s.semestre).ToListAsync(), "IdExamen", "NumeroExamenWithDSAndSemestre", IdEx);

            if (IdEx.HasValue)
            {
                emplois = await _context.EmploiExams
                    .Include(e => e.matiere)
                    .Include(n => n.niveau)
                    .Include(e => e.EmploiExamLocals).ThenInclude(el => el.Local)
                    .Include(e => e.EmploiExamEnseignants).ThenInclude(ee => ee.Enseignant)
                    .Include(e => e.EmploiExamVacataires).ThenInclude(ev => ev.Vacataire)
                    .Where(e => e.IdExamen == IdEx && e.EmploiExamLocals.Any(el => el.IdLocal == idL))
                    .ToListAsync();
            }

            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;

            var jours = await _context.Jours.ToListAsync();
            var seances = await _context.Seances.ToListAsync();
            var local = await _context.Locals.FindAsync(idL);

            ViewBag.idExam = IdEx;
            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.Local = local;
            ViewBag.localId = idL;

            return View(emplois);
        }
        //GeneratePdf

        [Authorize(Roles = "Directeur,Secritaire, Admin")]
        [HttpGet]
        [ActionName("ExamLocalPdf")]
        public IActionResult GeneratePdf(int IdL, int? IdEx)
        {
           var emplois = _context.EmploiExams
                   .Include(e => e.matiere)
                   .Include(n => n.niveau)
                   .Include(e => e.EmploiExamLocals).ThenInclude(el => el.Local)
                   .Include(e => e.EmploiExamEnseignants).ThenInclude(ee => ee.Enseignant)
                   .Include(e => e.EmploiExamVacataires).ThenInclude(ev => ev.Vacataire)
                   .Where(e => e.IdExamen == IdEx && e.EmploiExamLocals.Any(el => el.Local.IdLocal ==IdL))
                   .ToList();
            int anneeDebut;
            int anneeFin;
            var exam = _context.Examens.FirstOrDefault(e => e.IdExamen == IdEx);
          
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
            var semestre = _context.semestres.FirstOrDefault(s => s.IdSemestre ==exam.IdSemestre);
            var local = _context.Locals.FirstOrDefault(l => l.IdLocal == IdL);


            // Créez un modèle avec les données
            var model = new EmploiExamViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                Examen = exam,
                Type = emplois[0].typeEmploi,
                NomLocal = local?.NomLocal ?? "",
                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdfLocalExam", model);
        }
    }
}
