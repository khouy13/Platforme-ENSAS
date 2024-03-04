using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System.Security.Claims;

namespace Projet.Areas.Chef.Controllers
{
    [Area("Chef")]
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;
        public ChefController(AppDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
       

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Departement = _context.Departements.FirstOrDefault(e => e.ApplicationUserId == userId);

            if (Departement != null)
            {
                var filieres = _context.Filieres.Include(e => e.ApplicationUser).Where(e => e.IdDepartement == Departement.IdDepartement).ToList();
                ViewBag.filiere = filieres;



                var NbEnseignant = _context.Enseignants.Where(e => e.IdDepartement == Departement.IdDepartement

                ).Count();
                ViewBag.NbEnseignant = NbEnseignant;
                var NbVacataires = _context.vacataires.Where(e => e.IdDepartement == Departement.IdDepartement).Count();
                ViewBag.NbVacataires = NbVacataires;
            }
            return View();
        }

        
    }
}
