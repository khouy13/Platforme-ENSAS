using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using Projet.Areas.Coordonnateur.Models;
using Projet.Data;
using  Projet.Areas.Directeur.Models;
using Projet.Areas.Professeur.Models;
using Projet.Areas.Responsable.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Rotativa.AspNetCore;
using Projet.Migrations;

namespace Projet.Areas.professeur.Controllers
{
    [Area("Professeur")]
    public class ProfController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfController(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;


        }
       
        public IActionResult Index()
        {
            int? idEnseignant = HttpContext.Session.GetInt32("IdEnseignant");
            int? idVacataire = HttpContext.Session.GetInt32("IdVacataire");
            var semestres = _context.semestres.ToList();
            var statistiquesChargeHoraire = new List<StatistiquesChargeHoraire>();

            if (idEnseignant != null)
            {
                foreach (var semestre in semestres)
                {
                    int chargeHoraire = _context.EmploiTemps
                        .Where(et => et.IdSemestre == semestre.IdSemestre && (et.IdEnseignant == idEnseignant ))
                        .Count() * 2;

                    statistiquesChargeHoraire.Add(new StatistiquesChargeHoraire
                    {
                        Semestre = semestre,
                        ChargeHoraire = chargeHoraire
                    });
                }
            }
            else if (idVacataire != null)
            {
                foreach (var semestre in semestres)
                {
                    int chargeHoraire = _context.EmploiTemps
                        .Where(et => et.IdSemestre == semestre.IdSemestre && (et.IdVacataire == idVacataire))
                        .Count() * 2;

                    statistiquesChargeHoraire.Add(new StatistiquesChargeHoraire
                    {
                        Semestre = semestre,
                        ChargeHoraire = chargeHoraire
                    });
                }
            }

            ViewBag.StatistiquesChargeHoraire = statistiquesChargeHoraire;
            List<EmploiTemps> emploisTemps = null;
            if (idEnseignant != null)
            {
                emploisTemps = _context.EmploiTemps

                     .Include(m => m.Matiere)
                     .Include(n => n.Niveau)
                     .Include(t => t.TypeEnseignement)
                     .Where(e => e.IdEnseignant == idEnseignant)
                     .ToList(); // Exécutez la requête SQL et récupérez les données dans une liste

            }
            else if(idVacataire != null)
            {
                
                    emploisTemps = _context.EmploiTemps

                         .Include(m => m.Matiere)
                         .Include(n => n.Niveau)
                         .Include(t => t.TypeEnseignement)
                         .Where(e => e.IdVacataire==idVacataire)
                         .ToList(); // Exécutez la requête SQL et récupérez les données dans une liste

                
            }
            if (emploisTemps != null)
            {

         
                var emploiTempsInfosParSemestre = new Dictionary<Semestre, List<EmploiTempsInfo>>();

                foreach (var semestre in semestres)
                {
                    var emploisTempsSemestre = emploisTemps
                        .Where(et => et.IdSemestre == semestre.IdSemestre)
                        .DistinctBy(et => et.IdMatiere)
                        .ToList(); // Filtrer les emplois du temps par semestre et appliquer DistinctBy

                    var emploiTempsInfos = new List<EmploiTempsInfo>();

                    foreach (var emploiTemps in emploisTempsSemestre)
                    {
                        var emploiTempsInfo = new EmploiTempsInfo
                        {
                          
                            NomMatiere = emploiTemps.Matiere.NomMatiere,
                            NiveauDistinct = emploiTemps.Niveau.NomNiveau,
                            TypesEnseignement = emploiTemps.TypeEnseignement.NomEn,
                            SemaineDebut = emploiTemps.SemaineDebut,
                            SemaineFin = emploiTemps.SemaineFin
                        };

                        emploiTempsInfos.Add(emploiTempsInfo);
                    }

                    emploiTempsInfosParSemestre.Add(semestre, emploiTempsInfos);
                }
               

                ViewBag.EmploiTempsInfosParSemestre = emploiTempsInfosParSemestre;
            }

            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Directeur, Coordonnateur,Enseignant,Chef")]
        public async Task<IActionResult> EmploiProfesseur(int? IdEns = null, int? IdVac=null, int? IdS = null)
        {
            List<EmploiTemps> emplois = null;
            if(IdEns.HasValue)
            {
                emplois = _context.EmploiTemps
                        .Include(e => e.Enseignant)
                        .Include(v => v.Vacataire)
                        .Include(g => g.Groupe)
                        .Include(n => n.Niveau)
                        .Include(M => M.Matiere)
                        .Include(l => l.Local)
                        .Where(e => (e.IdEnseignant == IdEns && (IdS == null || e.IdSemestre == IdS)))
                        .ToList();
            }
            else if(IdVac.HasValue)
            {
                emplois = _context.EmploiTemps
                           .Include(e => e.Enseignant)
                           .Include(v => v.Vacataire)
                           .Include(g => g.Groupe)
                           .Include(n => n.Niveau)
                           .Include(M => M.Matiere)
                           .Include(l => l.Local)
                           .Where(e => (e.IdVacataire== IdVac && (IdS == null || e.IdSemestre == IdS)))
                           .ToList();
                                }
            bool emploiremplie = emplois?.Count != 0;
            ViewBag.remplie = emploiremplie;

            var semestres = _context.semestres.ToList();
            ViewBag.Semestres = semestres;

            int? latestSemestreId = _context.EmploiTemps
                .Where(e => (e.IdEnseignant == IdEns || e.IdVacataire == IdVac) && e.IdSemestre != null)
                .OrderByDescending(e => e.IdSemestre)
                .Select(e => e.IdSemestre)
                .FirstOrDefault();

            int selectedSemesterOrDefault = IdS ?? latestSemestreId ?? 0;
            ViewBag.selectedSemester = selectedSemesterOrDefault;

            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            var enseignant = _context.Enseignants.Find(IdEns);
            var vactaire = _context.vacataires.Find(IdVac);

            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.Enseignant = enseignant;
            ViewBag.Vacataire = vactaire;
            ViewBag.IdEns = IdEns;
            ViewBag.IdVac = IdVac;
            return View(emplois);
        }
        [HttpGet]
        [Authorize(Roles = "Directeur, Coordonnateur,Enseignant,Chef")]
        public async Task<IActionResult> EmploiSurveillance(int? IdEns=null, int? IdVac=null, int? IdS=null, int? IdEx=null)
        {
           
            var jours = _context.Jours.ToList();
            var seances = _context.Seances.ToList();
            var enseignant = _context.Enseignants.Find(IdEns);
            var vactaire = _context.vacataires.Find(IdVac);
            List<EmploiExam> emplois = new List<EmploiExam>();
          



            ViewBag.Seances = seances;
            ViewBag.Jours = jours;
            ViewBag.Enseignant = enseignant;
            ViewBag.Vacataire = vactaire;
            ViewBag.IdEns = IdEns;
            ViewBag.IdVac = IdVac;

            // Récupérez le dernier IdSemestre si IdS est null
            if (!IdS.HasValue)
            {
                IdS = await _context.EmploiExams
                    .OrderByDescending(e => e.IdSemestre)
                    .Select(e => (int?)e.IdSemestre)
                    .FirstOrDefaultAsync();
            }
            if (!IdEx.HasValue)
            {
                IdEx = await _context.Examens
                    .Where(e => e.IdSemestre == IdS)
                    .OrderByDescending(e => e.IdExamen)
                    .Select(e => (int?)e.IdExamen)
                    .FirstOrDefaultAsync();
            }
            ViewBag.selectedSemester = IdS;
            ViewBag.exammm = IdEx;
            ViewBag.idExam= IdEx;
            var exam =_context.Examens.Include(e => e.semestre).ToList();
            ViewData["Examens"] = new SelectList(exam, "IdExamen", "NumeroExamenWithDSAndSemestre", IdEx);
            var semestres = _context.semestres.ToList();
            ViewData["Semestres"] = new SelectList(semestres, "IdSemestre", "NomSemestre", IdS);
            // Récupérez le dernier IdExamen pour le dernier semestre si IdEx est null
            if (IdEns.HasValue)
            {
                emplois = await _context.EmploiExams
            .Include(e => e.matiere).Include(n => n.niveau)
            .Include(e => e.EmploiExamLocals)
                .ThenInclude(el => el.Local)
            .Include(e => e.EmploiExamEnseignants)
                .ThenInclude(ee => ee.Enseignant)
           
            .Where(e => e.IdSemestre == IdS && e.EmploiExamEnseignants.Any(ee => ee.Enseignant.IdEnseignant == IdEns))
            .ToListAsync();

            }
            else if (IdVac.HasValue)
            {
                 emplois = await _context.EmploiExams
                            .Include(e => e.matiere).Include(n => n.niveau)
                            .Include(e => e.EmploiExamLocals)
                                .ThenInclude(el => el.Local)
                          
                            .Include(e => e.EmploiExamVacataires)
                                .ThenInclude(ev => ev.Vacataire)
                            .Where(e => e.IdSemestre == IdS && e.EmploiExamVacataires.Any(ee => ee.IdVacataire == IdVac))
                            .ToListAsync();

            }
           
            bool emploiremplie = emplois.Count != 0;
            ViewBag.remplie = emploiremplie;
          
            return View(emplois);
        }
        //Empoi Exam
        [HttpGet]
        [ActionName("GeneratePdfByIds")]
        public async Task<IActionResult> GeneratePdf(int IdS, int IdEx, int? IdEns=null, int? IdVac=null)
        {


            List<EmploiExam> emplois = new List<EmploiExam>();


            ViewBag.IdEns = IdEns;
            ViewBag.IdVac = IdVac;


            ViewBag.selectedSemester = IdS;
            ViewBag.exammm = IdEx;
            var enseignant = _context.Enseignants.Find(IdEns);
            var vactaire = _context.vacataires.Find(IdVac);

            // Récupérez le dernier IdExamen pour le dernier semestre si IdEx est null
            if (IdEns.HasValue)
            {
                emplois = await _context.EmploiExams
            .Include(e => e.matiere).Include(n => n.niveau)
            .Include(e => e.EmploiExamLocals)
                .ThenInclude(el => el.Local)
            .Include(e => e.EmploiExamEnseignants)
                .ThenInclude(ee => ee.Enseignant)
                .Include(v=>v.EmploiExamVacataires).ThenInclude(v=>v.Vacataire)

            .Where(e => e.IdSemestre == IdS && e.EmploiExamEnseignants.Any(ee => ee.Enseignant.IdEnseignant == IdEns))
            .ToListAsync();

            }
            else if (IdVac.HasValue)
            {
                emplois =  await _context.EmploiExams
                           .Include(e => e.matiere).Include(n => n.niveau)
                           .Include(e => e.EmploiExamLocals)
                               .ThenInclude(el => el.Local)

                           .Include(e => e.EmploiExamVacataires)
                               .ThenInclude(ev => ev.Vacataire)
                           .Where(e => e.IdSemestre == IdS && e.EmploiExamVacataires.Any(ee => ee.IdVacataire == IdVac))
                          .ToListAsync();

            }

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



            // Créez un modèle avec les données
            var model = new EmploiExamViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                NomVacataire = vactaire?.NomComplet ?? "",
                NomEnseignant = enseignant?.NomComplet ?? "",
                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire,
                Examen = examen,
                Type = emplois[0]?.typeEmploi ?? "",
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdfAsync", model);
        }
        //GeneratePdf
        [HttpGet]
        [ActionName("GeneratePdfByParams")]
        public IActionResult GeneratePdf(int? IdEns=null, int? IdVac = null, int? IdS = null)
        {
            var emplois = new List<EmploiTemps>();

            if (IdEns.HasValue)
            {
                emplois = _context.EmploiTemps

                          .Include(g => g.Groupe)
                          .Include(m => m.Matiere)
                          .Include(l => l.Local).Include(n=>n.Niveau)
                          .Include(t => t.TypeEnseignement)
                          .Where(e => e.IdEnseignant == IdEns && e.IdSemestre == IdS)
                          .ToList();
            }
            else if (IdVac.HasValue)
            {
                emplois = _context.EmploiTemps

              .Include(g => g.Groupe)
              .Include(m => m.Matiere).Include(n=>n.Niveau)
              .Include(l => l.Local)
              .Include(t => t.TypeEnseignement).Include(e=>e.Enseignant)
              .Where(e => e.IdVacataire == IdVac && e.IdSemestre == IdS)
              .ToList();
            }
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
            var Vacataire = _context.vacataires.FirstOrDefault(l => l.IdVacataire == IdVac);


            // Créez un modèle avec les données
            var model = new EmploiViewModel
            {
                Emplois = emplois,
                Jours = jours,
                Seances = seances,
                NomEnseignant = enseignant?.NomComplet ?? "",
                NomVacataire = Vacataire?.NomComplet ?? "",
                Semestre = semestre,
                nomAnneeScolaire = nomAnneeScolaire
            };

            // Retournez la vue en tant que PDF en utilisant Rotativa
            return new ViewAsPdf("GeneratePdf1", model);
        }


        //pour EmploiSurvaiance:



    }
}
