using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Projet.Areas.Directeur.Controllers
{
    [Area("Directeur")]
    public class DirecteurController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public DirecteurController(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime currentDate = DateTime.Now;
            var formattedDate = currentDate.ToString("dd/MM/yyyy");
            ViewBag.date = formattedDate;
            var nombreTotal = _context.Enseignants.Count();
            ViewBag.nombreTotalEns = nombreTotal;
            var nbtv = _context.vacataires.Count();
            ViewBag.NbV = nbtv;
            int salleCount = _context.Locals.Where(r => r.NomLocal.ToLower().Contains("salle")).Count();
            int atelierCount = _context.Locals.Where(r => r.NomLocal.ToLower().Contains("atelier")).Count();
            int amphiCount = _context.Locals.Where(r => r.NomLocal.ToLower().Contains("amphi")).Count();

            ViewBag.SalleCount = salleCount;
            ViewBag.AtelierCount = atelierCount;
            ViewBag.AmphiCount = amphiCount;
            var semestres = _context.semestres.ToList();
            ViewBag.Semestres = semestres;

            // Créez une liste pour stocker les statistiques de chaque semestre
            var statistiquesSemestres = new List<StatistiquesSemestre>();

            foreach (var semestre in semestres)
            {
                int nombreTotalEnseignants = _context.EmploiTemps
                    .Where(et => et.IdSemestre == semestre.IdSemestre && et.IdEnseignant != null)
                    .Select(et => et.IdEnseignant)
                    .Distinct()
                    .Count();

                int nombreTotalVacataires = _context.EmploiTemps
                    .Where(et => et.IdSemestre == semestre.IdSemestre && et.IdVacataire != null)
                    .Select(et => et.IdVacataire)
                    .Distinct()
                    .Count();

                // Ajoutez les statistiques du semestre à la liste
                statistiquesSemestres.Add(new StatistiquesSemestre
                {
                    Semestre = semestre,
                    NombreEnseignants = nombreTotalEnseignants,
                    NombreVacataires = nombreTotalVacataires
                });
            }

            // Stockez la liste des statistiques des semestres dans ViewBag
            ViewBag.StatistiquesSemestres = statistiquesSemestres;

            //Coordonnateur
             var filieresAvecCoordinateurs = _context.Filieres
            .Include(f => f.ApplicationUser)  // Inclure ApplicationUser
                .ThenInclude(a => a.Enseignant)  // Ensuite, inclure Enseignant
            .ToList();

            ViewBag.filierecoordi = filieresAvecCoordinateurs;

            var filiere = _context.Filieres.Count();
            ViewBag.NBf=filiere;
            return View();
        }
    }
}
