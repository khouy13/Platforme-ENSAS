using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Data;
using Projet.Models;
using System.Diagnostics;



namespace Projet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
            _context = context;
           
        }

        public IActionResult Index()
        {
           
            _logger.LogInformation("Ceci est un message d'information");
          
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}