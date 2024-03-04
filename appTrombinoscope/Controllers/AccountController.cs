using appTrombinoscope.Context;
using appTrombinoscope.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace appTrombinoscope.Controllers
{
    public class AccountController(PasswordHasher hasher,AppDbContext dbContext) : Controller
    {
        readonly PasswordHasher hasher = hasher;
        readonly AppDbContext dbContext=dbContext;
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetInt32("userId") !=null)
            {
                TempData["success"] = "vous êtes déconnecté !";
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("role");
            }
            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        public IActionResult LoginAction(string Email,string Password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrEmpty(Password))
            {
                TempData["error"] = "Données manquantes !";
                return RedirectToAction("Login");
            }

            var acccount = new Account {Email=Email,Password=Password};
            if (hasher.Login(acccount))
            {
                var user = dbContext.Accounts.Where(a=>a.Email==acccount.Email).FirstOrDefault();
                HttpContext.Session.SetInt32("userId", user!.Id);
                HttpContext.Session.SetString("role", user.Role);
                TempData["success"] = "Connecté avec succès !";
				return RedirectToAction("Index","Home");
            }

            TempData["error"] = "Courriel ou mot de passe incorrect !";
            return RedirectToAction("Login");
        }
    }
}
