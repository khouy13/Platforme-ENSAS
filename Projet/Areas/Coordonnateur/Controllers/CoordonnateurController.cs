using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Directeur.Models;
using Projet.Areas.Professeur.Models;
using Projet.Data;

namespace Projet.Areas.Coordonnateur.Controllers
{
    [Area("coordonnateur")]
    public class CoordonnateurController : Controller
    {
        private readonly AppDbContext _context;
        public CoordonnateurController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int? idEnseignant = HttpContext.Session.GetInt32("IdEnseignant");

            var semestres = _context.semestres.ToList();
            var statistiquesChargeHoraire = new List<StatistiquesChargeHoraire>();

            if (idEnseignant != null)
            {
                foreach (var semestre in semestres)
                {
                    int chargeHoraire = _context.EmploiTemps
                        .Where(et => et.IdSemestre == semestre.IdSemestre && (et.IdEnseignant == idEnseignant))
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
                    .ToList();

                var emploiTempsInfosParSemestre = new Dictionary<Semestre, List<EmploiTempsInfo>>();

                foreach (var semestre in semestres)
                {
                    var emploisTempsSemestre = emploisTemps
                        .Where(et => et.IdSemestre == semestre.IdSemestre)
                        .DistinctBy(et => et.IdMatiere)
                        .ToList();

                    var emploiTempsInfos = new List<EmploiTempsInfo>();

                    foreach (var emploiTemps in emploisTempsSemestre)
                    {
                        var emploiTempsInfo = new EmploiTempsInfo
                        {
                            NomMatiere = emploiTemps.Matiere != null ? emploiTemps.Matiere.NomMatiere : "",
                            NiveauDistinct = emploiTemps.Niveau != null ? emploiTemps.Niveau.NomNiveau : "",
                            TypesEnseignement = emploiTemps.TypeEnseignement != null ? emploiTemps.TypeEnseignement.NomEn : "",
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

    }
}
