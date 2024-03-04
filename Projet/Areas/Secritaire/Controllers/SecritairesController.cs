using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Projet.Data;

namespace Projet.Areas.Secritaire.Controllers
{
    [Area("Secritaire")]
    public class SecritairesController : Controller
    {
        private readonly AppDbContext _context;
        public SecritairesController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            int salleCount = _context.Locals.Where(r => r.NomLocal.ToLower().Contains("salle")).Count();
            int atelierCount = _context.Locals.Where(r => r.NomLocal.ToLower().Contains("atelier")).Count();
            int amphiCount = _context.Locals.Where(r => r.NomLocal.ToLower().Contains("amphi")).Count();

            ViewBag.SalleCount = salleCount;
            ViewBag.AtelierCount = atelierCount;
            ViewBag.AmphiCount = amphiCount;
            return View();
        }
    }
}
