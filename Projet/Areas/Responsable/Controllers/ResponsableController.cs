using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet.Controllers;
using Projet.Data;
using Microsoft.Extensions.Logging;
namespace Projet.Areas.Admin.Controllers
{
    [Area("Responsable")]
    [Route("responsable")]
    [Authorize(Roles = "Admin")]

    public class ResponsableController : Controller
    {
        private readonly ILogger<ResponsableController> _logger;
        private readonly AppDbContext _context;

        public ResponsableController(AppDbContext context, ILogger<ResponsableController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("responsable")]
        [Route("")]
        [Authorize(Roles = "Admin")]
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
            return View();
        }
    }
}
