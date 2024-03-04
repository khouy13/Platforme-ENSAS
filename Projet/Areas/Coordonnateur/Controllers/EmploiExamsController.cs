using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using Projet.Areas.Coordenateur.Controllers;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using Rotativa.AspNetCore;

namespace Projet.Areas.Coordonnateur.Controllers
{
    [Area("Coordonnateur")]
   
    public class EmploiExamsController : Controller
    {
        private readonly AppDbContext _context;
        private ApplicationUser user;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EmploiExamsController> _logger;
        public EmploiExamsController(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger<EmploiExamsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public async Task<IActionResult> EmploiExam(int? IdN, int? IdS, int? IdEx,string? Type)
        {
            var smestre = IdS;
            ViewBag.semesre = IdS;
            //IdEx:c est IdExam
            if (!IdS.HasValue)
            {
                    var dernierSemestre = _context.EmploiExams.Where(e => e.IdNiveau == IdN).OrderByDescending(s => s.IdSemestre).FirstOrDefault();
                
                    var dernierSemestre2 = await _context.semestres
                                    .Where(s => s.EmploisExamSemestre.Any(e => e.IdNiveau == IdN))
                                    .OrderByDescending(s => s.IdSemestre)
                                    .FirstOrDefaultAsync();
                

                if (dernierSemestre != null)
                {
                    IdS = dernierSemestre?.IdSemestre;
                   
                    
                }
                else
                {
                    IdS = dernierSemestre2?.IdSemestre;
                    
                   
                }
                    

            }
            if (!IdEx.HasValue)
            {
                // Filtrer les examens par IdSemestre et IdNiveau dans les emplois d'examen
                var examen = await _context.Examens
                    .Where(e => e.IdSemestre == IdS && e.EmploiExamen.Any(emploi => emploi.IdNiveau == IdN))
                    .OrderByDescending(e => e.IdExamen)
                    .FirstOrDefaultAsync();
                
              // Récupérer l'IdExamen de l'examen trouvé (ou null si aucun n'est trouvé)
              IdEx = examen?.IdExamen;
            }
            if (string.IsNullOrEmpty(Type))
            {
                // Obtenez le dernier Type s'il est manquant
                Type = await _context.EmploiExams
                    .Where(e => e.IdNiveau == IdN && e.IdSemestre == IdS && e.IdExamen == IdEx)
                    .OrderByDescending(e => e.IdEmploiExam)
                    .Select(e => e.typeEmploi)
                    .FirstOrDefaultAsync();
            }
            ViewBag.selectedSemester = IdS;
            ViewBag.Type = Type;

            var emplois = await _context.EmploiExams
               .Include(e => e.matiere)
               .ThenInclude(e=>e.Enseignant)
                .Include(m=>m.matiere).ThenInclude(v=>v.Vacataire)
               .Include(ex => ex.examen)
               .Include(e => e.EmploiExamLocals)
                   .ThenInclude(el => el.Local)
               .Include(e => e.EmploiExamEnseignants)
                   .ThenInclude(ee => ee.Enseignant)
               .Include(e => e.EmploiExamVacataires)
                   .ThenInclude(ev => ev.Vacataire)
                .Where(e => e.IdNiveau == IdN && e.IdSemestre == IdS &&  e.IdExamen == IdEx && e.typeEmploi==Type)
                .ToListAsync();

           
            var matieres=_context.Matieres.Include(e=>e.MatiereNiveaus).ThenInclude(e=>e.Niveau).ThenInclude(e=>e.NiveauMatieres).Where(e=>e.MatiereNiveaus.Any(e=>e.IdNiveau== IdN)).ToList();
            ViewBag.matieres = matieres;

            var exam = await _context.Examens.Include(e => e.semestre).ToListAsync();
            ViewData["Examens"] = new SelectList(exam, "IdExamen", "NumeroExamenWithDSAndSemestre", IdEx);
            var semestres = _context.semestres.ToList();
            ViewData["Semestres"] = new SelectList(semestres, "IdSemestre", "NomSemestre", IdS);
            var jours = await _context.Jours.ToListAsync();
            var seances = await _context.Seances.ToListAsync();

            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;
           
           

            var niveau = await _context.Niveaus.FirstOrDefaultAsync(e => e.IdNiveau == IdN);
            ViewBag.niveau = niveau;
            ViewBag.selectedSemestre = await _context.semestres.FirstOrDefaultAsync(e => e.IdSemestre == IdS);
           
            

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.idNeveau = IdN;
            ViewBag.idExam = IdEx;
           
            return View(emplois);
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public IActionResult Create(int? IdN, int? IdSeance, int? IdJour, int IdS, int IdEx,string Type)
        {

            var enseignat = _context.Enseignants.ToList();
            ViewBag.Enseignants = enseignat;
            var Vacataires = _context.vacataires.ToList();
            ViewBag.Vacataires = Vacataires;
            var locals = _context.Locals.ToList();
          
            ViewBag.Locals = locals;

            var jour = _context.Jours.Where(e => e.IdJour == IdJour).FirstOrDefault();
            var seance = _context.Seances.Where(m => m.IdSeance == IdSeance).FirstOrDefault();
            var matieres = _context.Matieres
                            .Where(m => m.MatiereNiveaus.Any(mn => mn.IdNiveau == IdN))
                            .ToList();


            ViewData["IdMatiere"] = new SelectList(matieres, "IdMatiere", "NomMatiere");


            ViewBag.NomJour = jour;
            ViewBag.Nomseance = seance;
            ViewBag.idNeveau = IdN;
            ViewBag.Seances = IdSeance;
            ViewBag.Jours = IdJour;
            ViewBag.IdExamen = IdEx;
            ViewBag.IdSemestre = IdS;
            ViewBag.Type = Type;
            return View();
        }
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmploiExam emploiExam, List<int>? EmploiExamEnseignants, List<int>? EmploiExamVacataires, List<int>? EmploiExamLocals)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                   
                   .FirstOrDefault(u => u.Id == userId);
            }
            var IdS = emploiExam.IdSemestre;
            var IdJour = emploiExam.IdJour;
           
            var IdSeance = emploiExam.IdSeance;
            var IdEx = emploiExam.IdExamen;
            var IdN=emploiExam.IdNiveau;
            var Type = emploiExam.typeEmploi;
            var niveauNom = _context.Niveaus
               .Where(n => n.IdNiveau == IdN)
               .Select(n => n.NomNiveau)
               .FirstOrDefault();
            var matiere = _context.Matieres.FirstOrDefault(e => e.IdMatiere == emploiExam.IdMatiere);
            var nonjour = _context.Jours
                .Where(n => n.IdJour == IdJour)
                .Select(n => n.NomJour)
                .FirstOrDefault();
            var nonseance = _context.Seances
                .Where(n => n.IdSeance == IdSeance)
                .Select(n => n.NomSeance)
                .FirstOrDefault();
            bool teacherOccupied = _context.EmploiExams.Any(e =>
              e.EmploiExamEnseignants.Any(enseignant => EmploiExamEnseignants.Contains(enseignant.IdEnseignant)) &&
       
              e.IdSeance == IdSeance &&
              e.typeEmploi == Type &&
              
              e.IdJour == emploiExam.IdJour &&
              e.IdExamen == emploiExam.IdExamen &&
              e.IdSemestre == emploiExam.IdSemestre

  );

            var teacherOccupiedEN = _context.EmploiExams.Include(e=>e.EmploiExamEnseignants).ThenInclude(e=>e.Enseignant).Include(n=>n.niveau).Include(m=>m.matiere).FirstOrDefault(e =>
                e.EmploiExamEnseignants.Any(enseignant => EmploiExamEnseignants.Contains(enseignant.IdEnseignant)) &&
                e.IdSeance == IdSeance &&
                e.typeEmploi == Type &&
                e.IdJour == emploiExam.IdJour &&
                e.IdExamen == emploiExam.IdExamen &&
                e.IdSemestre == emploiExam.IdSemestre
            );





           var vacataireOccupied = _context.EmploiExams.Include(e =>e.EmploiExamVacataires).ThenInclude(e => e.Vacataire).Include(n => n.niveau).Include(m => m.matiere).FirstOrDefault(e =>
                e.EmploiExamVacataires.Any(vacataire => EmploiExamVacataires.Contains(vacataire.IdVacataire)) &&
                e.IdSeance == IdSeance &&
                 e.typeEmploi == Type &&
                e.IdJour == emploiExam.IdJour &&
                 e.IdExamen == emploiExam.IdExamen &&
                e.IdSemestre == emploiExam.IdSemestre 
                );

           var existingSession = _context.EmploiExams.Include(e=>e.matiere).Include(n=>n.niveau).Include(e=>e.Jour).Include(s=>s.Seance).FirstOrDefault(e =>
                           e.typeEmploi == Type &&
                            e.IdSemestre == emploiExam.IdSemestre &&
                            e.IdExamen == emploiExam.IdExamen &&
                             e.IdNiveau == emploiExam.IdNiveau &&
                             e.IdSemestre == emploiExam.IdSemestre &&
                             e.IdMatiere== emploiExam.IdMatiere
                            );




            var localOccupied = _context.EmploiExams.Include(e => e.EmploiExamLocals).ThenInclude(e=>e.Local).Include(e => e.matiere).Include(n=>n.niveau).FirstOrDefault(e =>
                e.EmploiExamLocals.Any(local => EmploiExamLocals.Contains(local.IdLocal)) &&
                e.IdSeance == IdSeance &&
                e.IdJour == emploiExam.IdJour &&
                 e.typeEmploi == Type &&
                e.IdSemestre == emploiExam.IdSemestre &&
                e.IdExamen == emploiExam.IdExamen 
                );
            

          

            var emploiExams = _context.EmploiExams
                .Include(e => e.matiere)
                .Include(e => e.EmploiExamEnseignants)
                .Include(e => e.EmploiExamVacataires)
                .Include(e => e.EmploiExamLocals)
                .ToList();

            bool commonExam = emploiExams
                .Any(e =>
                    e.IdSeance == emploiExam.IdSeance &&
                    e.IdSemestre == emploiExam.IdSemestre &&
                     e.typeEmploi == Type &&
                     e.IdMatiere==emploiExam.IdMatiere &&

                    e.IdExamen == emploiExam.IdExamen &&
                    e.IdJour == emploiExam.IdJour &&
                    (
                        e.EmploiExamEnseignants.Any(enseignant => EmploiExamEnseignants.Contains(enseignant.IdEnseignant)) ||
                        e.EmploiExamVacataires.Any(vacataire => EmploiExamVacataires.Contains(vacataire.IdVacataire))
                    ) &&
                    e.EmploiExamLocals.Any(local => EmploiExamLocals.Contains(local.IdLocal))
                );

            if (existingSession!=null)
            {
                TempData["ExamMessage"] = $"La matière {(existingSession.matiere!=null? existingSession.matiere.NomMatiere:"")} pour le niveau {(existingSession.niveau != null ? existingSession.niveau.NomNiveau : "")}" +
                    $" est déjà programmée le {(existingSession.Jour != null ? existingSession.Jour.NomJour : "")} de {existingSession.Seance.dateDebut} à {existingSession.Seance.dateFin}.";
                //existingSession
                return RedirectToAction("Create", new { IdN, IdSeance, IdJour, IdS, IdEx, Type }); 
            }

            if (commonExam)
            {
                var examexsiste = _context.EmploiExams.FirstOrDefault(e => e.IdMatiere == e.IdMatiere);
                examexsiste.isComunExam = true;

                emploiExam.EmploiExamEnseignants = new List<EmploiExamEnseignant>();
                emploiExam.isComunExam = true;
                foreach (var enseignantId in EmploiExamEnseignants)
                {
                    emploiExam.EmploiExamEnseignants.Add(new EmploiExamEnseignant { IdEnseignant = enseignantId });
                }

                emploiExam.EmploiExamVacataires = new List<EmploiExamVacataire>();
                foreach (var vacataireId in EmploiExamVacataires)
                {
                    emploiExam.EmploiExamVacataires.Add(new EmploiExamVacataire { IdVacataire = vacataireId });
                }

                emploiExam.EmploiExamLocals = new List<EmploiExamLocal>();
                foreach (var localId in EmploiExamLocals)
                {
                    emploiExam.EmploiExamLocals.Add(new EmploiExamLocal { IdLocal = localId });
                }

                _context.Add(emploiExam);
                if (user != null)
                {

                    _logger.LogInformation($"L'utilisateur {(user?.NomComplet ?? "Inconnu")} a effectué une opération d' Ajout d'Emploi du Examen {(examexsiste?.examen?.NumeroExamen)} {matiere?.NomMatiere ?? ""} Pour {(examexsiste?.niveau?.NomNiveau ?? "Inconnu")}");

                }
                await _context.SaveChangesAsync();
               
                return RedirectToAction("EmploiExam", new { IdN, IdS, IdEx , Type });

            }




           


          if ( vacataireOccupied !=null)
            {

                string enseignantsMessage = "";
                string matiereMessage = "";
                string niveauMessage = " ";


                if (vacataireOccupied.EmploiExamVacataires != null)
                {
                    foreach (var vacataire in vacataireOccupied.EmploiExamVacataires)
                    {
                        if (EmploiExamVacataires.Contains(vacataire.IdVacataire))
                        {
                            enseignantsMessage += vacataire.Vacataire != null
                                ? vacataire.Vacataire.NomComplet + ", "
                                : "inconnu, ";
                        }
                    }

                    if (vacataireOccupied.matiere != null)
                    {
                        matiereMessage += vacataireOccupied.matiere.NomMatiere != null
                            ? vacataireOccupied.matiere.NomMatiere
                            : "une matière inconnue";
                    }

                    if (vacataireOccupied.niveau != null)
                    {
                        niveauMessage += vacataireOccupied.niveau.NomNiveau != null
                            ? vacataireOccupied.niveau.NomNiveau
                            : "un niveau inconnu";
                    }

                }

                TempData["ExamMessage"] = $"{enseignantsMessage.TrimEnd(',', ' ')} est déjà occupés en surveillance d'examen. Il surveille l'examen de {matiereMessage} avec {niveauMessage} pendant cette séance .";

                return RedirectToAction("Create", new { IdN, IdSeance, IdJour, IdS, IdEx , Type });
            }




            if ( localOccupied!=null )

            {
                string localMessage = "";
                string matiereMessage = "";
                string niveauMessage = " ";


                if (localOccupied.EmploiExamLocals != null)
                {
                    foreach (var local in localOccupied.EmploiExamLocals)
                    {
                        if (EmploiExamLocals.Contains(local.IdLocal))
                        {
                            localMessage += local.Local!= null
                                ? local.Local.NomLocal+ ", "
                                : "";
                        }
                    }

                    if (localOccupied.matiere != null)
                    {
                        matiereMessage += localOccupied.matiere.NomMatiere != null
                            ? localOccupied.matiere.NomMatiere
                            : "une matière inconnue";
                    }

                    if (localOccupied.niveau != null)
                    {
                        niveauMessage += localOccupied.niveau.NomNiveau != null
                            ? localOccupied.niveau.NomNiveau
                            : "un niveau inconnu";
                    }

                }


                TempData["ExamMessage"] = $"{localMessage.TrimEnd(',', ' ')} est déjà occupé. Il est réservé pour l'examen de {matiereMessage} pour {niveauMessage} pendant cette séance.";


                return RedirectToAction("Create", new { IdN, IdSeance, IdJour, IdS, IdEx , Type });
            }
           if (teacherOccupied)
            {
                    string enseignantsMessage = "";
                    string matiereMessage = "";
                    string niveauMessage = " ";
                  

                if (teacherOccupiedEN.EmploiExamEnseignants != null)
                    {
                        foreach (var enseignant in teacherOccupiedEN.EmploiExamEnseignants)
                        {
                            if (EmploiExamEnseignants.Contains(enseignant.IdEnseignant))
                            {
                                enseignantsMessage += enseignant.Enseignant != null
                                    ? enseignant.Enseignant.NomComplet + ", "
                                    : "inconnu, ";
                            }
                        }

                        if (teacherOccupiedEN.matiere != null)
                        {
                            matiereMessage += teacherOccupiedEN.matiere.NomMatiere != null
                                ? teacherOccupiedEN.matiere.NomMatiere
                                : "une matière inconnue";
                        }

                        if (teacherOccupiedEN.niveau != null)
                        {
                            niveauMessage += teacherOccupiedEN.niveau.NomNiveau != null
                                ? teacherOccupiedEN.niveau.NomNiveau
                                : "un niveau inconnu";
                        }
                   
                }
                  

                    TempData["ExamMessage"] = $"{enseignantsMessage.TrimEnd(',', ' ')} est déjà occupé. Il surveille l'examen de {matiereMessage} avec {niveauMessage} pendant cette séance .";
                 return RedirectToAction("Create", new { IdN, IdSeance, IdJour, IdS, IdEx, Type });













            }

            emploiExam.EmploiExamEnseignants = new List<EmploiExamEnseignant>();
            foreach (var enseignantId in EmploiExamEnseignants)
            {
                emploiExam.EmploiExamEnseignants.Add(new EmploiExamEnseignant { IdEnseignant = enseignantId });
            }

            emploiExam.EmploiExamVacataires = new List<EmploiExamVacataire>();
            foreach (var vacataireId in EmploiExamVacataires)
            {
                emploiExam.EmploiExamVacataires.Add(new EmploiExamVacataire { IdVacataire = vacataireId });
            }

            emploiExam.EmploiExamLocals = new List<EmploiExamLocal>();
            foreach (var localId in EmploiExamLocals)
            {
                emploiExam.EmploiExamLocals.Add(new EmploiExamLocal { IdLocal = localId });
            }

            _context.Add(emploiExam);
            if (user != null)
            {

                _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération d'ajout d un Examen  {emploiExam?.examen?.NumeroExamen} {matiere?.NomMatiere ?? ""} Pour {niveauNom}.");

            }
            await _context.SaveChangesAsync();
            
            return RedirectToAction("EmploiExam", new { IdN, IdS, IdEx , Type });
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        // Edit GET action
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emploiExam = await _context.EmploiExams
                .Include(e => e.EmploiExamEnseignants)
                .Include(e => e.EmploiExamVacataires)
                .Include(e => e.EmploiExamLocals)
                .FirstOrDefaultAsync(e => e.IdEmploiExam == id);

            if (emploiExam == null)
            {
                return NotFound();
            }
            var IdJour = emploiExam.IdJour;
            var IdSeance=emploiExam.IdSeance;
            var IdNiveau = emploiExam.IdNiveau;
            var IdSemestre = emploiExam.IdSemestre;
            var type = emploiExam.typeEmploi;
            var IdExam = emploiExam.IdExamen;
            var enseignants = await _context.Enseignants.ToListAsync();
            var vacataires = await _context.vacataires.ToListAsync();
            var locals = await _context.Locals.ToListAsync();
            var matieres = _context.Matieres
                            .Where(m => m.MatiereNiveaus.Any(mn => mn.IdNiveau == emploiExam.IdNiveau))
                            .ToList();


            ViewData["IdMatiere"] = new SelectList(matieres, "IdMatiere", "NomMatiere", emploiExam.IdMatiere);
            var jours= await _context.Jours.ToListAsync();
            var seances = await _context.Seances.ToListAsync();
            ViewData["IdJour"] = new SelectList(jours, "IdJour", "NomJour", emploiExam.IdJour);
            ViewData["IdSeance"] = new SelectList(seances, "IdSeance", "NomSeance", emploiExam.IdSeance);

             ViewBag.IdSemestre = IdSemestre;
             ViewBag.Seances = IdSeance;
             ViewBag.Jours = IdJour;
             ViewBag.idNeveau = IdNiveau;
             ViewBag.IdExamen = IdExam;
             ViewBag.Enseignants = enseignants;
             ViewBag.Vacataires = vacataires;
             ViewBag.Type = type;
             ViewBag.Locals = locals;
           
            return View(emploiExam);
        }
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmploiExam emploiExam, List<int> EmploiExamEnseignants, List<int> EmploiExamVacataires, List<int> EmploiExamLocals)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                  
                   .FirstOrDefault(u => u.Id == userId);
            }
            if (id != emploiExam.IdEmploiExam)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                try
                {
                    
                  
                    var IdS = emploiExam.IdSemestre;
                    var IdJour = emploiExam.IdJour;
                    var IdSeance = emploiExam.IdSeance;
                    var IdEx = emploiExam.IdExamen;
                    var IdN = emploiExam.IdNiveau;
                    var Type = emploiExam.typeEmploi;
                    var niveauNom = _context.Niveaus
                         .Where(n => n.IdNiveau == IdN)
                         .Select(n => n.NomNiveau)
                         .FirstOrDefault();
                    var nonjour = _context.Jours
                        .Where(n => n.IdJour == IdJour)
                        .Select(n => n.NomJour)
                        .FirstOrDefault();
                    var nonseance = _context.Seances
                        .Where(n => n.IdSeance == IdSeance)
                        .Select(n => n.NomSeance)
                        .FirstOrDefault();
                    var existingEmploiExam = await _context.EmploiExams
                       .Include(e => e.EmploiExamEnseignants)
                       .Include(e => e.EmploiExamVacataires)
                       .Include(e => e.EmploiExamLocals)
                       .FirstOrDefaultAsync(e => e.IdEmploiExam == id);

                   

                    var teacherOccupiedEN = _context.EmploiExams.Include(e => e.EmploiExamEnseignants).ThenInclude(e => e.Enseignant).Include(n => n.niveau).Include(m => m.matiere).FirstOrDefault(e =>
             e.EmploiExamEnseignants.Any(enseignant => EmploiExamEnseignants.Contains(enseignant.IdEnseignant)) &&
                           e.IdSeance == IdSeance &&
                           e.IdJour == emploiExam.IdJour &&
                           e.typeEmploi == emploiExam.typeEmploi &&
                           e.IdExamen == emploiExam.IdExamen &&
                           e.IdSemestre == emploiExam.IdSemestre &&
                           e.IdEmploiExam != id);





                    var existingSession = _context.EmploiExams.Include(e => e.matiere).Include(n => n.niveau).Include(e => e.Jour).Include(s => s.Seance).FirstOrDefault(e =>
                                               e.typeEmploi == Type &&
                                               e.IdEmploiExam != id &&
                                               e.IdSemestre == emploiExam.IdSemestre &&
                                               e.IdExamen == emploiExam.IdExamen &&
                                               e.IdNiveau == emploiExam.IdNiveau &&
                                               e.IdJour != emploiExam.IdJour &&
                                               e.IdSeance != emploiExam.IdSeance &&
                                               e.IdSemestre == emploiExam.IdSemestre &&
                                               e.IdMatiere == emploiExam.IdMatiere 
                                              );





                    var vacataireOccupied = _context.EmploiExams.Include(e => e.EmploiExamVacataires).ThenInclude(e => e.Vacataire).Include(n => n.niveau).Include(m => m.matiere).FirstOrDefault(e =>
                         e.EmploiExamVacataires.Any(vacataire => EmploiExamVacataires.Contains(vacataire.IdVacataire)) &&
                        e.IdSeance == IdSeance &&
                           e.typeEmploi == emploiExam.typeEmploi &&
                        e.IdJour == emploiExam.IdJour &&
                        e.IdSemestre == emploiExam.IdSemestre &&
                        e.IdEmploiExam != id);




              

                    var localOccupied = _context.EmploiExams.Include(e => e.EmploiExamLocals).ThenInclude(e => e.Local).Include(e => e.matiere).Include(n => n.niveau).FirstOrDefault(e =>
                        e.EmploiExamLocals.Any(local => EmploiExamLocals.Contains(local.IdLocal)) &&
                        e.IdSeance == IdSeance &&
                        e.IdJour == emploiExam.IdJour &&
                        e.typeEmploi == emploiExam.typeEmploi &&
                        e.IdSemestre == emploiExam.IdSemestre &&
                        e.IdExamen == emploiExam.IdExamen &&
                        e.IdEmploiExam != id);


                    var emploiExams = _context.EmploiExams
                        .Include(e => e.matiere)
                        .Include(e => e.EmploiExamEnseignants)
                        .Include(e => e.EmploiExamVacataires)
                        .Include(e => e.EmploiExamLocals)
                        .ToList();

                    bool commonExam = emploiExams
                        .Any(e =>
                            e.IdSeance == emploiExam.IdSeance &&
                            e.IdSemestre == emploiExam.IdSemestre &&
                            e.typeEmploi == emploiExam.typeEmploi &&
                            e.IdExamen == emploiExam.IdExamen &&
                             e.IdEmploiExam != id &&
                            e.IdMatiere==emploiExam.IdMatiere &&
                            e.IdJour == emploiExam.IdJour &&
                            (
                                e.EmploiExamEnseignants.Any(enseignant => EmploiExamEnseignants.Contains(enseignant.IdEnseignant)) ||
                                e.EmploiExamVacataires.Any(vacataire => EmploiExamVacataires.Contains(vacataire.IdVacataire))
                            ) &&
                            e.EmploiExamLocals.Any(local => EmploiExamLocals.Contains(local.IdLocal))
                        ); 
                    if (existingSession != null)
                    {
                        TempData["ExamMessage"] = $"La matière {(existingSession.matiere != null ? existingSession.matiere.NomMatiere : "")} pour le niveau {(existingSession.niveau != null ? existingSession.niveau.NomNiveau : "")}" +
                            $" est déjà programmée le {(existingSession.Jour != null ? existingSession.Jour.NomJour : "")} de {existingSession.Seance.dateDebut} à {existingSession.Seance.dateFin}.";
                        //existingSession
                          return RedirectToAction("Edit", new { id });
                    }


                    if (commonExam)
                    {
                        var examexsiste = _context.EmploiExams.Include(e=>e.matiere).Include(e=>e.examen).FirstOrDefault(e => e.IdMatiere == e.IdMatiere);
                        examexsiste.isComunExam = true;
                        existingEmploiExam.isComunExam = true;
                        existingEmploiExam.IdSemestre = emploiExam.IdSemestre;
                        existingEmploiExam.IdSeance=emploiExam.IdSeance;
                        existingEmploiExam.isComunExam = true;
                        existingEmploiExam.IdExamen = emploiExam.IdExamen;
                        existingEmploiExam.typeEmploi = emploiExam.typeEmploi;
                        existingEmploiExam.IdMatiere=emploiExam.IdMatiere;
                        existingEmploiExam.IdJour=emploiExam.IdJour;
                        existingEmploiExam.IdNiveau = emploiExam.IdNiveau;
                        existingEmploiExam.EmploiExamEnseignants.Clear();
                        existingEmploiExam.EmploiExamVacataires.Clear();
                        existingEmploiExam.EmploiExamLocals.Clear();
                        if (user != null && user.Enseignant != null)
                        {

                            _logger.LogInformation($"L'utilisateur {(user?.NomComplet ?? "Inconnu")} a effectué une opération de Modification d'Emploi des Examen {(examexsiste?.examen?.NumeroExamen)} {examexsiste?.matiere?.NomMatiere ?? ""} Pour {(emploiExam?.niveau?.NomNiveau ?? "Inconnu")}");

                        }
                        // Update related association tables
                        foreach (var enseignantId in EmploiExamEnseignants)
                        {
                            existingEmploiExam.EmploiExamEnseignants.Add(new EmploiExamEnseignant { IdEnseignant = enseignantId });
                        }

                        foreach (var vacataireId in EmploiExamVacataires)
                        {
                            existingEmploiExam.EmploiExamVacataires.Add(new EmploiExamVacataire { IdVacataire = vacataireId });
                        }

                        foreach (var localId in EmploiExamLocals)
                        {
                            existingEmploiExam.EmploiExamLocals.Add(new EmploiExamLocal { IdLocal = localId });
                        }
                       
                        await _context.SaveChangesAsync();
                        return RedirectToAction("EmploiExam", new { IdN, IdS, IdEx, Type });
                    }

                


                    if (teacherOccupiedEN!=null)
                    {

                        string enseignantsMessage = "";
                        string matiereMessage = "";
                        string niveauMessage = " ";

                        if (teacherOccupiedEN.EmploiExamEnseignants != null)
                        {
                            foreach (var enseignant in teacherOccupiedEN.EmploiExamEnseignants)
                            {
                                if (EmploiExamEnseignants.Contains(enseignant.IdEnseignant))
                                {
                                    enseignantsMessage += enseignant.Enseignant != null
                                        ? enseignant.Enseignant.NomComplet + ", "
                                        : "inconnu, ";
                                }
                            }

                            if (teacherOccupiedEN.matiere != null)
                            {
                                matiereMessage += teacherOccupiedEN.matiere.NomMatiere != null
                                    ? teacherOccupiedEN.matiere.NomMatiere
                                    : "une matière inconnue";
                            }

                            if (teacherOccupiedEN.niveau != null)
                            {
                                niveauMessage += teacherOccupiedEN.niveau.NomNiveau != null
                                    ? teacherOccupiedEN.niveau.NomNiveau
                                    : "un niveau inconnu";
                            }

                        }


                        TempData["ExamMessage"] = $"{enseignantsMessage.TrimEnd(',', ' ')} est déjà occupé. Il surveille l'examen de {matiereMessage} avec {niveauMessage} pendant cette séance .";



                        TempData["ExamMessage"] = "L'enseignant choisi est déjà occupé dans cette séance. Veuillez vérifier les disponibilités des enseignants ou vacataires, ou trouver une autre séance.";
                        return RedirectToAction("Edit", new { id});
                    }
                    if (vacataireOccupied!=null)
                    {

                        string enseignantsMessage = "";
                        string matiereMessage = "";
                        string niveauMessage = " ";


                        if (vacataireOccupied.EmploiExamVacataires != null)
                        {
                            foreach (var vacataire in vacataireOccupied.EmploiExamVacataires)
                            {
                                if (EmploiExamVacataires.Contains(vacataire.IdVacataire))
                                {
                                    enseignantsMessage += vacataire.Vacataire != null
                                        ? vacataire.Vacataire.NomComplet + ", "
                                        : "inconnu, ";
                                }
                            }

                            if (vacataireOccupied.matiere != null)
                            {
                                matiereMessage += vacataireOccupied.matiere.NomMatiere != null
                                    ? vacataireOccupied.matiere.NomMatiere
                                    : "une matière inconnue";
                            }

                            if (vacataireOccupied.niveau != null)
                            {
                                niveauMessage += vacataireOccupied.niveau.NomNiveau != null
                                    ? vacataireOccupied.niveau.NomNiveau
                                    : "un niveau inconnu";
                            }

                        }

                        TempData["ExamMessage"] = $"{enseignantsMessage.TrimEnd(',', ' ')} est déjà occupés en surveillance d'examen. Il surveille l'examen de {matiereMessage} avec {niveauMessage} pendant cette séance .";




                        return RedirectToAction("Edit", new { id});
                    }
                    if (localOccupied!=null)
                    {
                        string localMessage = "";
                        string matiereMessage = "";
                        string niveauMessage = " ";


                        if (localOccupied.EmploiExamLocals != null)
                        {
                            foreach (var local in localOccupied.EmploiExamLocals)
                            {
                                if (EmploiExamLocals.Contains(local.IdLocal))
                                {
                                    localMessage += local.Local != null
                                        ? local.Local.NomLocal + ", "
                                        : "";
                                }
                            }

                            if (localOccupied.matiere != null)
                            {
                                matiereMessage += localOccupied.matiere.NomMatiere != null
                                    ? localOccupied.matiere.NomMatiere
                                    : "une matière inconnue";
                            }

                            if (localOccupied.niveau != null)
                            {
                                niveauMessage += localOccupied.niveau.NomNiveau != null
                                    ? localOccupied.niveau.NomNiveau
                                    : "un niveau inconnu";
                            }

                        }


                        TempData["ExamMessage"] = $"{localMessage.TrimEnd(',', ' ')} est déjà occupé. Il est réservé pour l'examen de {matiereMessage} pour {niveauMessage} pendant cette séance.";



                        return RedirectToAction("Edit", new { id});
                    }

                    // Clear existing associations
                    existingEmploiExam.IdSemestre = emploiExam.IdSemestre;
                    existingEmploiExam.IdSeance = emploiExam.IdSeance;
                    existingEmploiExam.IdExamen = emploiExam.IdExamen;
                    existingEmploiExam.typeEmploi = emploiExam.typeEmploi;
                    existingEmploiExam.IdMatiere = emploiExam.IdMatiere;
                    existingEmploiExam.IdJour = emploiExam.IdJour;
                    existingEmploiExam.IdNiveau = emploiExam.IdNiveau;
                    existingEmploiExam.EmploiExamEnseignants.Clear();
                    existingEmploiExam.EmploiExamVacataires.Clear();
                    existingEmploiExam.EmploiExamLocals.Clear();
               
                    // Update related association tables
                    foreach (var enseignantId in EmploiExamEnseignants)
                    {
                        existingEmploiExam.EmploiExamEnseignants.Add(new EmploiExamEnseignant { IdEnseignant = enseignantId });
                    }

                    foreach (var vacataireId in EmploiExamVacataires)
                    {
                        existingEmploiExam.EmploiExamVacataires.Add(new EmploiExamVacataire { IdVacataire = vacataireId });
                    }

                    foreach (var localId in EmploiExamLocals)
                    {
                        existingEmploiExam.EmploiExamLocals.Add(new EmploiExamLocal { IdLocal = localId });
                    }
                   
                    await _context.SaveChangesAsync();
                    if (user != null)
                    {

                        _logger.LogInformation($"L'utilisateur {(user?.NomComplet ?? "Inconnu")} a effectué une opération de Modification d'Emploi Examen N° {(existingEmploiExam?.examen?.NumeroExamen)} {emploiExam?.matiere?.NomMatiere ?? ""}Pour {(emploiExam?.niveau?.NomNiveau ?? "Inconnu")}");

                    }
                    return RedirectToAction("EmploiExam", new { IdN, IdS, IdEx, Type });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploiExamExists(emploiExam.IdEmploiExam))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If ModelState is not valid, reload necessary data and return to Edit view
            var enseignants = await _context.Enseignants.ToListAsync();
            var vacataires = await _context.vacataires.ToListAsync();
            var locals = await _context.Locals.ToListAsync();
            var matieres = await _context.Matieres.ToListAsync();
            var jours = await _context.Jours.ToListAsync();
            var seances = await _context.Seances.ToListAsync();
            ViewData["IdJour"] = new SelectList(jours, "IdJour", "NomJour", emploiExam.IdJour);
            ViewData["IdSeance"] = new SelectList(seances, "IdSeance", "NomSeance", emploiExam.IdSeance);
            ViewBag.Enseignants = enseignants;
            ViewBag.Vacataires = vacataires;
            ViewBag.Locals = locals;
            ViewBag.Matieres = matieres;
            ViewBag.Type = emploiExam.typeEmploi;
            return View(emploiExam);
        }



        // cette Action pour faire la suppression d un emploi 
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public IActionResult DeleteAllEmplois(int idN, int idS, int idEX,string Type)
        {
            try
            {
                    var niveauNom = _context.Niveaus
                      .Where(n => n.IdNiveau == idN)
                      .Select(n => n.NomNiveau)
                      .FirstOrDefault();
                var semestreNom = _context.semestres
                       .Where(n => n.IdSemestre == idS)
                       .Select(n => n.NomSemestre)
                       .FirstOrDefault();
                string userId = HttpContext.Session.GetString("UserId");


                if (userId != null)
                {
                    user = _userManager.Users
                      
                       .FirstOrDefault(u => u.Id == userId);
                }
                // Remove records from EmploiExamEnseignant
                var enseignantAssociations = _context.EmploiExamEnseignants
                    .Where(ee => ee.EmploiExam.IdNiveau == idN && ee.EmploiExam.IdSemestre == idS && ee.EmploiExam.IdExamen == idEX && ee.EmploiExam.typeEmploi== Type)
                    .ToList();

                _context.EmploiExamEnseignants.RemoveRange(enseignantAssociations);

                // Remove records from EmploiExamLocal
                var localAssociations = _context.EmploiExamLocals
                    .Where(el => el.EmploiExam.IdNiveau == idN && el.EmploiExam.IdSemestre == idS && el.EmploiExam.IdExamen == idEX && el.EmploiExam.typeEmploi==Type)
                    .ToList();

                _context.EmploiExamLocals.RemoveRange(localAssociations);

                // Remove records from EmploiExamVacataire
                var vacataireAssociations = _context.EmploiExamVacataires
                    .Where(ev => ev.EmploiExam.IdNiveau == idN && ev.EmploiExam.IdSemestre == idS && ev.EmploiExam.IdExamen == idEX && ev.EmploiExam.typeEmploi == Type)
                    .ToList();

                _context.EmploiExamVacataires.RemoveRange(vacataireAssociations);

                // Remove EmploiExam
                var emploiExamsToDelete = _context.EmploiExams.Include(e=>e.examen).Include(e =>e.niveau).Include(e => e.matiere)
                    .Where(e => e.IdNiveau == idN && e.IdSemestre == idS && e.IdExamen == idEX && e.typeEmploi==Type)
                    .ToList();

                _context.EmploiExams.RemoveRange(emploiExamsToDelete);
                if (user != null)
                {

                    _logger.LogInformation($"L'utilisateur {user.NomComplet} a supprimé l 'emploi  du Examen N° {emploiExamsToDelete[0].examen.NumeroExamen} Pour {emploiExamsToDelete[0].niveau.NomNiveau}");
                }
                // Save the changes to the database
                _context.SaveChanges();

                TempData["EmploiMessage"] = "l' Emploi des Examens pour ce semestre a été supprimé.";
            }
            catch (Exception ex)
            {
                TempData["EmploiMessage"] = "Une erreur s'est produite lors de la suppression des emplois .";
                // Handle the exception as needed (logging, displaying an error message, etc.).
            }

            return RedirectToAction("EmploiExam", new { IdN =idN, IdS = idS, IdEx = idEX,Type});
        }



       

        private bool EmploiExamExists(int id)
        {
            return (_context.EmploiExams?.Any(e => e.IdEmploiExam == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public async Task<IActionResult> GeneratePdf(int IdN, int IdS, int IdEx ,string Type)
        {
            var emplois = await _context.EmploiExams
               .Include(e => e.matiere)
               .ThenInclude(e => e.Enseignant)
               .Include(e => e.matiere)
               .ThenInclude(e => e.Vacataire)
               .Include(ex=>ex.examen)
               .Include(e => e.EmploiExamLocals)
                   .ThenInclude(el => el.Local)
               .Include(e => e.EmploiExamEnseignants)
                   .ThenInclude(ee => ee.Enseignant)
               .Include(e => e.EmploiExamVacataires)
                   .ThenInclude(ev => ev.Vacataire)
               .Where(e => e.IdNiveau == IdN && e.IdSemestre == IdS && e.IdExamen == IdEx)
               .ToListAsync();
            var examen = _context.Examens.FirstOrDefault(ex => ex.IdExamen == IdEx && ex.IdSemestre == IdS);

           
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
            var niveau = _context.Niveaus
                .Include(f => f.filiere).ThenInclude(d=>d.Departement)
                .FirstOrDefault(e => e.IdNiveau == IdN);
            var type = Type;

            // Créez un modèle avec les données
            var model = new EmploiExamViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                Niveau = niveau,
                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire,
                Examen = examen,
                Type = type
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdf", model);
        }
        [HttpGet] // Utiliser HttpGet au lieu de HttpPost
        public IActionResult Delete(int IdE)
        {
            string userId = HttpContext.Session.GetString("UserId");
           
            if (userId != null)
            {
                user = _userManager.Users
                   
                   .FirstOrDefault(u => u.Id == userId);
            }
            var emploiToDelete =  _context.EmploiExams.Find(IdE);
            var IdS = emploiToDelete.IdSemestre;
            var IdN = emploiToDelete.IdNiveau;
            var IdEx = emploiToDelete.IdExamen;
            var Type = emploiToDelete.typeEmploi;
            var niveauNom = _context.Niveaus
                         .Where(n => n.IdNiveau == IdN)
                         .Select(n => n.NomNiveau)
                         .FirstOrDefault();
            var nonjour = _context.Jours
                .Where(n => n.IdJour == emploiToDelete.IdJour)
                .Select(n => n.NomJour)
                .FirstOrDefault();
            var nonseance = _context.Seances
                .Where(n => n.IdSeance == emploiToDelete.IdSeance)
                .Select(n => n.NomSeance)
                .FirstOrDefault();

            //car dans Emploi QUI un Seance d exam on peut avoir plusieurs enseignant dans tout les enseignant ont meme idExam (select choix multiple)
            var EmploiExamLocal=  _context.EmploiExamLocals.Where(e=>e.IdEmploiExam== IdE).ToList();
            var EmploiExamEnseignant=  _context.EmploiExamEnseignants.Where(e => e.IdEmploiExam == IdE).ToList();
            var EmploiExamVacataire=_context.EmploiExamVacataires.Where(e => e.IdEmploiExam == IdE).ToList();
            if (EmploiExamLocal != null)
            {
                _context.EmploiExamLocals.RemoveRange(EmploiExamLocal);
            }
            if (EmploiExamEnseignant != null)
            {
                _context.EmploiExamEnseignants.RemoveRange(EmploiExamEnseignant);
            }
            if (EmploiExamVacataire != null)
            {
                _context.EmploiExamVacataires.RemoveRange(EmploiExamVacataire);
            }
            if (emploiToDelete == null)
            {

            }
            else
            {
                _context.EmploiExams.Remove(emploiToDelete);
                _context.SaveChanges();
                if (user != null )
                {

                    _logger.LogInformation($"L'utilisateur {user.NomComplet} a effectué une opération de suppression Examen N°  {emploiToDelete?.examen?.NumeroExamen} {emploiToDelete?.matiere?.NomMatiere?? ""}  dans l 'emploi des examens de {niveauNom}");

                }
            }
           

            return RedirectToAction("EmploiExam", new {IdN,IdS,IdEx,Type });
            }
           
        

    }
    }

