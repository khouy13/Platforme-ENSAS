using Microsoft.AspNetCore.Mvc;
using Projet.Data;

namespace Projet.Areas.Coordonnateur.Controllers
{

    [Area("coordonnateur")]
    public class MessageController : Controller
    {

        private readonly AppDbContext _db;

        public MessageController(AppDbContext db)
        {
           _db = db;
        }

        public IActionResult Index()
        {

            var x = _db.Messages.FirstOrDefault();
            return View(x);
        }


        public IActionResult ActiveDesactiveNotification()
        {
            var x = _db.Messages.FirstOrDefault();
            if (x != null)
            {
                if (x.IsMessageActive)
                {
                    x.IsMessageActive = false;
                    TempData["message"] = "Les notifications sont désactivées";
                }
                else
                {
                    x.IsMessageActive = true;
                    TempData["message"] = "Les notifications sont activées";
                }
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }


}
