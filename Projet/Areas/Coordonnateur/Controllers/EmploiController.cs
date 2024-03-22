using Microsoft.AspNetCore.Mvc;
using Projet.Data;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Text;
using Projet.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Projet.Areas.Responsable.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Projet.Areas.Coordenateur.Controllers
{

    [Area("coordonnateur")]
    public class EmploiController : Controller
    {
        private ApplicationUser user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ILogger<EmploiController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CommunCoursesHandler cmH;
        private readonly IEmailSend _mail;
        public EmploiController(CommunCoursesHandler _communCoursesHandler,
            AppDbContext _context, 
            IHttpContextAccessor httpContextAccessor, 
            ILogger<EmploiController> logger, 
            UserManager<ApplicationUser> userManager,
        IEmailSend email)
        {
            this._context = _context;
            this.cmH = _communCoursesHandler;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mail=email;
        }
        
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public IActionResult TempsEmploi(int? IdN, int? IdS)
        {
            //Need Condition On IdN If Is Null Or Not
            //Check existence of Nivea with In 
            //Also For IdS
            if (IdN == null || !_context.Niveaus.Any(e => e.IdNiveau == IdN))
            {
                TempData["EmploiError"] = "Action Refusee, Choisissez Un Niveau !";
                return View();
            }
            if (IdS != null && !_context.semestres.Any(e => e.IdSemestre == IdS))
            {
                TempData["EmploiError"] = "Action Refusee, Semestre Choisit Invalid !";
                return View();
            }

            int? latestSemestreId = _context.EmploiTemps
                .Where(e => e.IdNiveau == IdN && e.IdSemestre != null)
                .OrderByDescending(e => e.IdSemestre)
                .Select(e => e.IdSemestre)
                .FirstOrDefault();

            var selectedSemesterOrDefault = IdS ?? latestSemestreId ?? 0;

            ViewBag.selectedSemester = selectedSemesterOrDefault;

            var emplois = _context.EmploiTemps
                .Include(e => e.Enseignant)
                .Include(v => v.Vacataire)
                .Include(g => g.Groupe)
                .Include(m => m.Matiere)
                .Include(l => l.Local)
                .Include(t => t.TypeEnseignement)
                .Where(e => e.IdNiveau == IdN && (IdS == null || e.IdSemestre == IdS))
                .ToList();

            //var matieres = _context.Matieres.Where(m => m.IdNiveau == IdN).ToList();
            var matieres = _context.Matieres
                .Include(e => e.MatiereNiveaus)
                .ThenInclude(e => e.Niveau)
                .ThenInclude(e => e.NiveauMatieres)
                .Where(e => e.MatiereNiveaus.Any(e => e.IdNiveau == IdN))
                .ToList();

            ViewBag.matieres = matieres;

            //Unused smest
            //ViewBag.smest = IdS;

            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;

            var semestres = _context.semestres.ToList();
            ViewBag.Semestres = semestres;

            var niveau = _context.Niveaus.FirstOrDefault(e => e.IdNiveau == IdN);
            ViewBag.niveau = niveau;

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.idNeveau = IdN;

            return View(emplois);
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public IActionResult AddSeance(int? IdN, int? IdSeance, int? IdJour, int IdS)
        {

            var Enseignants = _context.Enseignants.ToList();
            ViewBag.Enseignants = Enseignants;
            var jour = _context.Jours.Where(e => e.IdJour == IdJour).FirstOrDefault();
            var seance = _context.Seances.Where(m => m.IdSeance == IdSeance).FirstOrDefault();
            var matieres = _context.Matieres.Include(e => e.MatiereNiveaus).ThenInclude(e => e.Niveau).ThenInclude(e => e.NiveauMatieres)
                            .Where(e => e.MatiereNiveaus.Any(e => e.IdNiveau == IdN))
                            .ToList();

            ViewBag.IdSemestre = IdS;
            ViewBag.matieres = matieres;
            //It must be list
            //var jours = _context.Jours;
            var jours = _context.Jours.ToList();
            var vacataires = _context.vacataires.ToList();
            ViewBag.Vacataires = vacataires;

            var seances = _context.Seances.ToList();

            var Local = _context.Locals.ToList();
            ViewBag.Local = Local;

            var groupe = _context.Groupes.Where(m => m.IdNiveau == IdN).ToList();
            ViewBag.groupe = groupe;

            var TypEnseignant = _context.TypeEnseignements.ToList();
            ViewBag.TypEnseignant = TypEnseignant;

            ViewBag.NomJour = jour;
            ViewBag.Nomseance = seance;
            ViewBag.idNeveau = IdN;
            ViewBag.Seances = IdSeance;
            ViewBag.Jours = IdJour;
            return View();
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpPost]
        public IActionResult AddSeance(EmploiTemps emploi)
        {
            string userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                user = _userManager.Users
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }

            var IdN = emploi.IdNiveau;

            var IdSeance = emploi.IdSeance;
            var IdJour = emploi.IdJour;
            var semD = emploi.SemaineDebut;
            var semF = emploi.SemaineFin;
            var IdS = emploi.IdSemestre;
            if (semD > semF)
            {
                TempData["EmploiMessage"] = "La Semaine De Fin Doit Être Supérieure à la Semaine De Début !";
                return RedirectToAction("AddSeance", new { IdN, IdSeance, IdJour, IdS });
            }
            string nomMatiere = null;
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

            var emploiTempsList = _context.EmploiTemps.Include(e => e.Matiere)
                .ToList();

            bool commonCourse = emploiTempsList
                .Any(e=> cmH.CommonCourse(e,emploi));

            if (commonCourse)
            {
                var ListOfCommonSessions = emploiTempsList.Where(e => cmH.CommonCourse(e, emploi));

                foreach (var course in ListOfCommonSessions)
                {
                    course.isComuncours = true;
                }

                // Ajouter l'emploi du temps à la base de données pour une séance commune
                emploi.isComuncours = true;
                _context.EmploiTemps.Add(emploi);
                _context.SaveChanges();
                sendMessage(emploi);

                if (user != null && user.Enseignant != null)
                {
                    _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération d'ajout de {nonseance} du {nonjour} dans l'emploi du temps de {niveauNom}.");
                }

                TempData["EmploiMessage"] = TempData["EmploiMessage"] ??"" +"Séance commune ajoutée avec succès.";
                return RedirectToAction("TempsEmploi", new { IdN, IdS });
            }

            // ici pour recuperer Enseignant 
            var teacherOccupiedEmpoloi = _context.EmploiTemps.Include(n => n.Niveau).Include(m => m.Matiere)
                .FirstOrDefault(e =>
                    e.IdEnseignant == emploi.IdEnseignant &&
                    e.IdSeance == emploi.IdSeance &&
                    e.IdJour == emploi.IdJour &&
                    e.IdNiveau != emploi.IdNiveau &&
                    e.IdSemestre == emploi.IdSemestre &&
                    ((e.SemaineDebut <= emploi.SemaineDebut && emploi.SemaineDebut <= e.SemaineFin) ||
                    (e.SemaineDebut <= emploi.SemaineFin && emploi.SemaineFin <= e.SemaineFin))
                );

            var matchingVacataireOccupied = _context.EmploiTemps.Include(n => n.Niveau).Include(m => m.Matiere)
                .FirstOrDefault(e =>
                    e.IdVacataire == emploi.IdVacataire &&
                    e.IdSeance == emploi.IdSeance &&
                    e.IdJour == emploi.IdJour &&
                    e.IdNiveau != emploi.IdNiveau &&
                    e.IdSemestre == emploi.IdSemestre &&
                    ((e.SemaineDebut <= emploi.SemaineDebut && emploi.SemaineDebut <= e.SemaineFin) ||
                    (e.SemaineDebut <= emploi.SemaineFin && emploi.SemaineFin <= e.SemaineFin))
                );

            if (teacherOccupiedEmpoloi != null)
            {
                var enseignantOccupe = _context.Enseignants.FirstOrDefault(e => e.IdEnseignant == emploi.IdEnseignant);
                if (enseignantOccupe != null)
                {
                    TempData["EmploiMessage"] = $"L'enseignant {enseignantOccupe.NomEnseignant} {enseignantOccupe.PrenomEnseignant} est déjà occupé.Il a un cours de  {(teacherOccupiedEmpoloi.Matiere != null ? teacherOccupiedEmpoloi.Matiere.NomMatiere : "")} de S-{semD}S-{semF} avec {(teacherOccupiedEmpoloi.Niveau != null ? teacherOccupiedEmpoloi.Niveau.NomNiveau : "")} pendant cette séance";
                    return RedirectToAction("AddSeance", new { IdN, IdSeance, IdJour, IdS });

                }


            }

            if (matchingVacataireOccupied != null)
            {
                var vacataireOccupe = _context.vacataires.FirstOrDefault(e => e.IdVacataire == emploi.IdVacataire);
                if (vacataireOccupe != null)
                {
                    TempData["EmploiMessage"] = $"L'enseignant {vacataireOccupe.Prenom} {vacataireOccupe.Nom} est déjà occupé.Il a un cours de  {(matchingVacataireOccupied.Matiere != null ? matchingVacataireOccupied.Matiere.NomMatiere : "")} de S-{semD}S-{semF} avec {(matchingVacataireOccupied.Niveau != null ? teacherOccupiedEmpoloi.Niveau.NomNiveau : "")} pendant cette séance";
                    return RedirectToAction("AddSeance", new { IdN, IdSeance, IdJour, IdS });
                }

            }


            var matchingLocalRecord = _context.EmploiTemps.Include(e => e.Niveau).Include(m => m.Matiere).Include(e => e.Enseignant).FirstOrDefault(e =>
                e.IdLocal == emploi.IdLocal &&
                ((e.SemaineDebut <= emploi.SemaineDebut && emploi.SemaineDebut <= e.SemaineFin) ||
                (e.SemaineDebut <= emploi.SemaineFin && emploi.SemaineFin <= e.SemaineFin)) &&
                e.IdSeance == emploi.IdSeance &&
                e.IdJour == emploi.IdJour &&
                e.IdSemestre == emploi.IdSemestre &&
                (e.IdEnseignant != emploi.IdEnseignant || e.IdVacataire != emploi.IdVacataire)
            );

            bool existingSession = _context.EmploiTemps.Any(e =>
                e.IdJour == emploi.IdJour &&
                e.IdSeance == emploi.IdSeance &&
                e.IdNiveau == emploi.IdNiveau &&
                e.IdSemestre == emploi.IdSemestre &&
                e.IdGroupe == emploi.IdGroupe &&
                ((e.SemaineDebut <= emploi.SemaineDebut && emploi.SemaineDebut <= e.SemaineFin) ||

                 (e.SemaineDebut <= emploi.SemaineFin && emploi.SemaineFin <= e.SemaineFin))
                );

            if (matchingLocalRecord != null)
            {
                var salleOccupee = _context.Locals.FirstOrDefault(l => l.IdLocal == emploi.IdLocal);
                if (salleOccupee != null)
                {
                    TempData["EmploiMessage"] = $"{salleOccupee.NomLocal} est déjà occupé  de S-{semD} S-{semF}" +
                        $" elle est réservée pour le cours de {(matchingLocalRecord.Matiere != null ? matchingLocalRecord.Matiere.NomMatiere : "")} " +
                        $"pour le niveau {(matchingLocalRecord.Niveau != null ? matchingLocalRecord.Niveau.NomNiveau : "")} avec l'enseignant {(matchingLocalRecord.Enseignant != null ? matchingLocalRecord.Enseignant.NomComplet : "")}";
                }

                return RedirectToAction("AddSeance", new { IdN, IdSeance, IdJour, IdS });
            }
            else if (existingSession)
            {
                TempData["EmploiMessage"] = "Une séance identique existe déjà dans l'intervalle de temps spécifié.";
                return RedirectToAction("AddSeance", new { IdN, IdSeance, IdJour, IdS });
            }
            else
            {
                // Ajouter l'emploi du temps à la base de données
                _context.EmploiTemps.Add(emploi);
                _context.SaveChanges();
                sendMessage(emploi);
                if (user != null && user.Enseignant != null)
                {

                    _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération d'ajout de {nonseance} du  {nonjour} dans l'emploi du temps de {niveauNom}.");
                }
                TempData["EmploiMessage"] = "Séance ajoutée avec succès.";
                return RedirectToAction("TempsEmploi", new { IdN, IdS });
            }
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]

        [HttpGet] // Utiliser HttpGet au lieu de HttpPost
        public IActionResult Delete(int IdEmploi)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }
            var emploiToDelete = _context.EmploiTemps.Find(IdEmploi);
            var IdN = emploiToDelete.IdNiveau;
            var IdS = emploiToDelete.IdSemestre;
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
            if (emploiToDelete == null)
            {
                TempData["EmploiMessage"] = "L'emploi du temps à supprimer n'a pas été trouvé.";
            }
            else
            {
                //ici on cherche les seances communes avec cette seance 
                //si le nombre est un on le fait false
                var listOfCommonSessions = _context.EmploiTemps.ToList()
                                           .Where(e=>cmH.CommonCourse(e,emploiToDelete) && e.IdEmploi != emploiToDelete.IdEmploi);

                if (listOfCommonSessions.Count() == 1)
                {
                    foreach (var el in listOfCommonSessions)
                    {
                        el.isComuncours = false;
                    }
                }

                _context.EmploiTemps.Remove(emploiToDelete);
                _context.SaveChanges();
                sendMessage(emploiToDelete);
                TempData["EmploiMessage"] = "Seance  du temps a été supprimé avec succès.";
                if (user != null && user.Enseignant != null)
                {
                    _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération de suppression  de {nonseance} du  {nonjour} dans l 'emploi de temps de {niveauNom}");
                }
            }

            return RedirectToAction("TempsEmploi", new { IdN, IdS });
        }
        //EDIT 
        [HttpGet]
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        public IActionResult Edit(int? IdEmploi, int? IdN)
        {
            if (IdEmploi == null || !_context.EmploiTemps.Any(e => e.IdEmploi == IdEmploi))
            {
                TempData["EmploiMessage"] = "L'emploi du temps à éditer n'a pas été trouvé.";

                if (
                    (IdN == null || !_context.Niveaus.Any(e => e.IdNiveau == IdN))
                    &&
                    !_context.Niveaus.IsNullOrEmpty()
                    )
                {
                    var Niveau = _context.Niveaus.FirstOrDefault();
                    IdN = Niveau?.IdNiveau;
                    TempData["EmploiMessage"] = TempData["EmploiMessage"] + "\n\nVous Etes Dans L'emploi Du Niveau " + Niveau.NomNiveau;
                }

                return RedirectToAction("TempsEmploi", new { IdN });
            }

            var emploiToEdit = _context.EmploiTemps
                .Include(e => e.Jour)
                .Include(e => e.Seance)
                .Include(e => e.Matiere)
                .Include(e => e.Niveau)
                .First(e => e.IdEmploi == IdEmploi);

            var IdS = emploiToEdit.IdSemestre;
            ViewBag.IdSemestre = IdS;

            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            ViewData["IdJour"] = new SelectList(jours, "IdJour", "NomJour", emploiToEdit.IdJour);
            ViewData["IdSeance"] = new SelectList(seances, "IdSeance", "NomSeance", emploiToEdit.IdSeance);

            var Enseignants = _context.Enseignants.ToList();
            ViewBag.Enseignants = Enseignants;

            var vacataires = _context.vacataires.ToList();
            ViewBag.Vacataires = vacataires;
            var matieres = _context.Matieres
                .Include(e => e.MatiereNiveaus)
                .ThenInclude(e => e.Niveau)
                .ThenInclude(e => e.NiveauMatieres)
                .Where(e => e.MatiereNiveaus.Any(e => e.IdNiveau == emploiToEdit.IdNiveau))
                .ToList();

            ViewBag.matieres = matieres;
            ViewBag.Local = _context.Locals.ToList();

            var groupe = _context.Groupes.Where(m => m.IdNiveau == emploiToEdit.IdNiveau).ToList();
            ViewBag.groupe = groupe;

            var TypEnseignant = _context.TypeEnseignements.ToList();
            ViewBag.TypEnseignant = TypEnseignant;
            ViewBag.idNeveau = emploiToEdit.IdNiveau;
            return View(emploiToEdit);
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpPost]
        public IActionResult Edit(int IdEmploi, EmploiTemps newEmploi)
        {
            ViewBag.idNeveau = newEmploi.IdNiveau;
            ViewBag.IdSemestre = newEmploi.IdSemestre;
            string userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                user = _userManager.Users
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }
            var IdN = newEmploi.IdNiveau;
            var IdS = newEmploi.IdSemestre;

            var niveauNom = _context.Niveaus
                  .Where(n => n.IdNiveau == IdN)
                  .Select(n => n.NomNiveau)
                  .FirstOrDefault();

            var nonjour = _context.Jours
              .Where(n => n.IdJour == newEmploi.IdJour)
              .Select(n => n.NomJour)
              .FirstOrDefault();

            var nonseance = _context.Seances
                .Where(n => n.IdSeance == newEmploi.IdSeance)
                .Select(n => n.NomSeance)
                .FirstOrDefault();

            ViewBag.IdSemestre = IdS;

            if (ModelState.IsValid)
            {
                var semD = newEmploi.SemaineDebut;
                var semF = newEmploi.SemaineFin;

                var oldEmploi = _context.EmploiTemps
                    .FirstOrDefault(e => e.IdEmploi == IdEmploi);

                if (oldEmploi == null)
                {
                    TempData["EmploiMessage"] = "L'emploi du temps à éditer n'a pas été trouvé.";
                    return RedirectToAction("TempsEmploi", new { IdN, IdS });
                }


                bool commonCourse = _context.EmploiTemps
                .AsEnumerable()
                .Any(e =>
                    cmH.CommonCourse(e,newEmploi) && e.IdEmploi != IdEmploi
                );

                //check old data and reset the isCommonCourse
                //Check if OldEmploi has CommonCourses & handle them
                //Placed Here To Not Be Repeated
                var oldEmploiCommonCourses = _context.EmploiTemps
                        .AsEnumerable().Where(e => cmH.CommonCourse(e, oldEmploi) && e.IdEmploi != IdEmploi);

                if (oldEmploiCommonCourses.Count() == 1 && !cmH.CommonCourse(oldEmploi, newEmploi))
                {
                    foreach (var commonSession in oldEmploiCommonCourses)
                    {
                        commonSession.isComuncours = false;
                    }
                }

                if (commonCourse)
                {
                    // Mettre à jour les propriétés de l'emploi du temps existant avec les nouvelles valeurs
                    //var emploicommunexsiste = _context.EmploiTemps.FirstOrDefault(e => e.IdMatiere == existingEmploi.IdMatiere);
                    //emploicommunexsiste.isComuncours = true;

                    var commonSessionsList = _context.EmploiTemps
                                     .AsEnumerable().Where(e => cmH.CommonCourse(e,newEmploi) && e.IdEmploi!= IdEmploi);

                    foreach ( var commonSession in commonSessionsList )
                    {
                        commonSession.isComuncours = true;
                    }

                    oldEmploi.isComuncours = true;
                    oldEmploi.IdMatiere = newEmploi.IdMatiere;
                    oldEmploi.IdTypeEnseignement = newEmploi.IdTypeEnseignement;
                    oldEmploi.SemaineDebut = newEmploi.SemaineDebut;
                    oldEmploi.SemaineFin = newEmploi.SemaineFin;
                    oldEmploi.IdGroupe = newEmploi.IdGroupe;
                    oldEmploi.Date = newEmploi.Date;
                    oldEmploi.IdNiveau = newEmploi.IdNiveau;
                    oldEmploi.IdJour = newEmploi.IdJour;
                    oldEmploi.IdEnseignant = newEmploi.IdEnseignant;
                    oldEmploi.IdLocal = newEmploi.IdLocal;
                    oldEmploi.IdVacataire = newEmploi.IdVacataire;
                    if (user != null && user.Enseignant != null)
                    {
                        _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération de Modification  de {nonseance} du {nonjour} dans l 'emploi de temps de {niveauNom}");
                    }
                    _context.SaveChanges();
                    sendMessage(newEmploi);
                    TempData["EmploiMessage"] = "L'emploi du temps a été mis à jour avec succès,Cette Seance est Commune!";
                    return RedirectToAction("TempsEmploi", new { IdN, IdS });
                }
                else
                {
                    
                    oldEmploi.isComuncours = false;
                }

                // Vérification pour éviter les conflits de séances
                var localOccupied = _context.EmploiTemps
                    .Include(e => e.Enseignant)
                    .Include(e => e.Vacataire)
                    .Include(m => m.Matiere)
                    .Include(n => n.Niveau)
                    .FirstOrDefault
                    (e =>
                        e.IdEmploi != IdEmploi &&
                        newEmploi.IdLocal != null &&
                        e.IdLocal == newEmploi.IdLocal &&
                        ((e.SemaineDebut <= semD && semD <= e.SemaineFin) || (e.SemaineDebut <= semF && semF <= e.SemaineFin)) &&
                        e.IdSeance == newEmploi.IdSeance &&
                        e.IdJour == newEmploi.IdJour &&
                        e.IdSemestre == newEmploi.IdSemestre &&
                        e.IdEnseignant != newEmploi.IdEnseignant
                    );

                bool existingSession = _context.EmploiTemps
                .Any(e =>
                    e.IdEmploi != IdEmploi &&
                    e.IdJour == newEmploi.IdJour &&
                    newEmploi.IdSeance != null &&
                    e.IdSeance == newEmploi.IdSeance &&
                    e.IdNiveau == newEmploi.IdNiveau &&
                    e.IdSemestre == newEmploi.IdSemestre &&
                    ((e.SemaineDebut <= semD && semD <= e.SemaineFin) || (e.SemaineDebut <= semF && semF <= e.SemaineFin)) &&
                    e.IdGroupe == newEmploi.IdGroupe);


                var teacherOccupied = _context
                    .EmploiTemps
                    .Include(n => n.Niveau)
                    .Include(m => m.Matiere)
                    .Include(e => e.Enseignant)
                    .FirstOrDefault
                    (e =>
                        e.IdEmploi != IdEmploi &&
                        newEmploi.IdEnseignant != null &&
                        e.IdEnseignant == newEmploi.IdEnseignant &&
                        e.IdSeance == newEmploi.IdSeance &&
                        e.IdJour == newEmploi.IdJour &&
                        e.IdSemestre == newEmploi.IdSemestre &&
                        ((e.SemaineDebut <= semD && semD <= e.SemaineFin) || (e.SemaineDebut <= semF && semF <= e.SemaineFin))
                    );

                var vacataireOccupied = _context.EmploiTemps.Include(n => n.Niveau).Include(m => m.Matiere).Include(e => e.Vacataire).FirstOrDefault(e =>
                      e.IdEmploi != IdEmploi &&
                      newEmploi.IdVacataire != null &&
                      e.IdVacataire == newEmploi.IdVacataire &&
                      e.IdSeance == newEmploi.IdSeance &&
                      e.IdJour == newEmploi.IdJour &&
                      e.IdSemestre == newEmploi.IdSemestre &&
                      ((e.SemaineDebut <= semD && semD <= e.SemaineFin) ||
                      (e.SemaineDebut <= semF && semF <= e.SemaineFin))
                  );

                if (localOccupied != null)
                {
                    var salleOccupee = _context.Locals.FirstOrDefault(l => l.IdLocal == newEmploi.IdLocal);
                    if (salleOccupee != null)
                    {
                        TempData["EmploiMessage"] = $"{salleOccupee.NomLocal} est déjà occupé  de S-{semD} S-{semF}" +
                            $" elle est réservée pour le cours de {(localOccupied.Matiere != null ? localOccupied.Matiere.NomMatiere : "")} " +
                            $"pour le niveau {(localOccupied.Niveau != null ? localOccupied.Niveau.NomNiveau : "")} avec l'enseignant {(localOccupied.Enseignant != null ? localOccupied.Enseignant.NomComplet : "")}";
                    }
                    return RedirectToAction("Edit", new { IdEmploi, IdN });
                }
                else if (existingSession)
                {
                    TempData["EmploiMessage"] = "Une séance identique existe déjà dans l'intervalle de temps spécifié.";
                    return RedirectToAction("Edit", new { IdN, IdS });
                }
                else if (teacherOccupied != null)
                {
                    TempData["EmploiMessage"] = $"L'enseignant {(teacherOccupied.Enseignant != null ? teacherOccupied.Enseignant.NomComplet : "")} " +
                        $" est déjà occupé.Il a un cours de  {(teacherOccupied.Matiere != null ? teacherOccupied.Matiere.NomMatiere : "")} de S-{semD}S-{semF}" +
                        $" avec {(teacherOccupied.Niveau != null ? teacherOccupied.Niveau.NomNiveau : "")} pendant cette séance";

                    return RedirectToAction("Edit", new { IdEmploi, IdN });
                }
                else if (vacataireOccupied != null)
                {
                    TempData["EmploiMessage"] = $"Le Vacataire {(vacataireOccupied.Vacataire != null ? vacataireOccupied.Vacataire.NomComplet : "")} " +
                        $" est déjà occupé.Il a un cours de  {(vacataireOccupied.Matiere != null ? vacataireOccupied.Matiere.NomMatiere : "")} de S-{semD}S-{semF}" +
                        $" avec {(vacataireOccupied.Niveau != null ? vacataireOccupied.Niveau.NomNiveau : "")} pendant cette séance";
                    return RedirectToAction("Edit", new { IdEmploi, IdN });
                }
                else
                {
                    oldEmploi.IdMatiere = newEmploi.IdMatiere;
                    oldEmploi.IdTypeEnseignement = newEmploi.IdTypeEnseignement;
                    oldEmploi.SemaineDebut = newEmploi.SemaineDebut;
                    oldEmploi.SemaineFin = newEmploi.SemaineFin;
                    oldEmploi.IdGroupe = newEmploi.IdGroupe;
                    oldEmploi.Date = newEmploi.Date;
                    oldEmploi.IdNiveau = newEmploi.IdNiveau;
                    oldEmploi.IdSeance = newEmploi.IdSeance;
                    oldEmploi.IdSemestre = newEmploi.IdSemestre;
                    oldEmploi.IdJour = newEmploi.IdJour;
                    oldEmploi.IdEnseignant = newEmploi.IdEnseignant;
                    oldEmploi.IdLocal = newEmploi.IdLocal;
                    if (user != null && user.Enseignant != null)
                    {

                        _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a effectué une opération de Modification  de {nonseance} du {nonjour}  dans l 'emploi de temps de {niveauNom}");

                    }
                    _context.SaveChanges();
                    sendMessage(newEmploi);
                    TempData["EmploiMessage"] = "L'emploi du temps a été mis à jour avec succès.";
                    return RedirectToAction("TempsEmploi", new { IdN, IdS });
                }
            }

            //// Si la validation a échoué, réafficher la vue avec les erreurs
            //var Enseignants = _context.Enseignants.ToList();
            //ViewBag.Enseignants = Enseignants;

            //var jours = _context.Jours.ToList();
            //var seances = _context.Seances.ToList();
            //ViewData["IdJour"] = new SelectList(jours, "IdJour", "NomJour", newEmploi.IdJour);
            //ViewData["IdSeance"] = new SelectList(seances, "IdSeance", "NomSeance", newEmploi.IdSeance);

            //////var matieres = _context.Matieres.Where(m => m.IdNiveau == emploiToEdit.IdNiveau).ToList();
            //var matieres = _context.Matieres
            //    .Include(e => e.MatiereNiveaus)
            //        .ThenInclude(e => e.Niveau)
            //        .ThenInclude(e => e.NiveauMatieres)
            //    .Where(e => e.MatiereNiveaus
            //    .Any(e => e.IdNiveau == newEmploi.IdNiveau))
            //    .ToList();

            //var vacataires = _context.vacataires.ToList();
            //ViewBag.Vacataires = vacataires;

            //ViewBag.matieres = matieres;
            //ViewBag.Local = _context.Locals.ToList();
            //var groupe = _context.Groupes.Where(m => m.IdNiveau == newEmploi.IdNiveau).ToList();
            //ViewBag.groupe = groupe;
            //var TypEnseignant = _context.TypeEnseignements.ToList();
            //ViewBag.TypEnseignant = TypEnseignant;
            if (newEmploi.SemaineDebut == 0)
            {
                TempData["EmploiMessage"] = "La Semaine De Debut Est Obligatoire !";
            }
            else if (newEmploi.SemaineFin == 0)
            {
                TempData["EmploiMessage"] = "La Semaine De Fin Est Obligatoire !";
            }
            else
            {
                TempData["EmploiMessage"] = "Données nécessaires sont manquantes !";
            }

            return RedirectToAction("Edit", new { IdEmploi, IdN });
        }

        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public IActionResult GeneratePdf(int IdN, int IdS)
        {
            var emplois = _context.EmploiTemps
           .Include(e => e.Enseignant)
           .Include(g => g.Groupe).Include(e => e.Niveau)
           .Include(m => m.Matiere).Include(v => v.Vacataire)
           .Include(l => l.Local)
           .Include(t => t.TypeEnseignement)
           .Where(e => e.IdNiveau == IdN && e.IdSemestre == IdS)
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
            var niveau = _context.Niveaus
                .Include(f => f.filiere).ThenInclude(d => d.Departement)
                .FirstOrDefault(e => e.IdNiveau == IdN);

            // Créez un modèle avec les données
            var model = new EmploiViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                Niveau = niveau,
                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdf", model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize(Roles = "Directeur, Coordonnateur,Chef")]
        [HttpGet]
        public IActionResult DeleteAllEmplois(int idN, int idS)
        {
            var IdN = idN;
            // IdS ici c est Idsemestre
            var IdS = idS;
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
                   .Include(u => u.Enseignant)
                   .FirstOrDefault(u => u.Id == userId);
            }
            try
            {
                var emploisToDelete = _context.EmploiTemps
                    .Where(e => e.IdNiveau == idN && e.IdSemestre == idS)
                    .ToList();

                _context.EmploiTemps.RemoveRange(emploisToDelete);
                _context.SaveChanges();
                
                if (user != null && user.Enseignant != null)
                {

                    _logger.LogInformation($"L'utilisateur {user.Enseignant.NomComplet} a supprimé l 'emploi de temps du {semestreNom}de {niveauNom}");
                }
                TempData["EmploiMessage"] = "Tous les emplois de temps pour ce semestre ont été supprimés.";
            }
            catch (Exception ex)
            {
                TempData["EmploiMessage"] = "Une erreur s'est produite lors de la suppression des emplois de temps.";
                // Gérer l'exception selon vos besoins (journalisation, affichage d'un message d'erreur, etc.).
            }

            return RedirectToAction("TempsEmploi", new { IdN, IdS }); // Redirigez vers la vue principale ou une autre vue appropriée.
        }

        public void sendMessage(EmploiTemps emploi)
        {
            var message = _context.Messages.FirstOrDefault();

            if (message != null && message.IsMessageActive)
            {
                try
                {
                    var enseignant = _context.Enseignants.Find(emploi.IdEnseignant);
                    if (enseignant != null)
                    {
                        var emailBody = $"<html><body>Cher/Chère {enseignant.NomComplet},<br><br>Changement d'emploi du temps à noter.<br><br><a href='https://localhost:7006/'>https://localhost:7006</a><br><br>Cordialement,<br><br>Ensa safi</body></html>";

                        _mail.SendEmailAsync(enseignant.Email, "Notification", emailBody);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error occurred while sending the notification email to the instructor.");
                     TempData["MessageErreur"] = "Il y a une erreur lors de l'envoi de message de notification au professeur.";
                }
            }
        }


    }

}