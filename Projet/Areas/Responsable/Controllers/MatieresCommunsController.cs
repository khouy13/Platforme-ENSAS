using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Responsable.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Controllers
{
    [Area("Responsable")]
    public class MatieresCommunsController : Controller
    {

        private readonly AppDbContext _context;
        private readonly ILogger<MatieresController> _logger;
        public MatieresCommunsController(AppDbContext context, ILogger<MatieresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Directeur")]
        async public Task<IActionResult> Index(string SearchText="")
        {
            var matiereGrouped = await _context.MatiereGroupe
                .Include(e => e.MatieresRelated)
                .ThenInclude(e => e.Matiere) 
                .Where(e => e.MatieresRelated != null)
                .ToListAsync();
            
            ViewBag.MatiereGrouped = matiereGrouped;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Directeur")]
        async public Task<IActionResult> Create()
        {
            var Matieres = await _context.Matieres.ToListAsync();
            ViewBag.Matieres = Matieres;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Directeur")]
        [ValidateAntiForgeryToken]
        async public Task<IActionResult> Create(MatiereGroupMatiereVM content)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GroupeMatiere groupe = new GroupeMatiere
                    {
                        Name = content.Name,
                    };
                    _context.MatiereGroupe.Add(groupe);
                    _context.SaveChanges();

                    int groupeId = groupe.Id;

                    foreach (int MatiereId in content.MatiereIds)
                    {
                        _context.MatiereGroupeMatieres.Add(new MatiereGroupeMatiere()
                        {
                            GroupMatiereId = groupeId,
                            MatiereId = MatiereId
                        });
                    }
                    _context.SaveChanges();
                    TempData["Message"] = "Engregitre avec succes !";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Erreur Aquise ! "+ex.Message;
                    return View(content);
                }
            }
            var Matieres = await _context.Matieres.ToListAsync();
            ViewBag.Matieres = Matieres;
            TempData["Message"] = "Les Donnees ne Sont Pas Valid !";
            return View(content);
        }

        //[HttpGet]
        //[Authorize(Roles = "Admin,Directeur")]
        //async public Task<IActionResult> Edit(int GroupId)
        //{
        //    var group = _context.MatiereGroupe.Include(e=>e.MatieresRelated).Where(e => e.Id == GroupId).FirstOrDefault();
        //    if (group == null)
        //    {
        //        return NotFound();
        //    }
        //    var vm = new MatiereGroupMatiereVM() { 
        //        GroupeId = group.Id,
        //        MatiereIds = group.MatieresRelated.Select(e=>e.Id).ToList(),
        //        Name = group.Name
        //    };

        //    var Matieres = await _context.Matieres.ToListAsync();
        //    ViewBag.Matieres = Matieres;
        //    return View(vm);
        //}

        [HttpPost]
        [Authorize(Roles = "Admin,Directeur")]
        public IActionResult Delete(int GroupId)
        {
            var matiereGroup = _context.MatiereGroupe.Where(e => e.Id == GroupId).FirstOrDefault();
            if (matiereGroup == null)
            {
                TempData["Message"] = "Le groupe des matières communes à supprimer n'était pas trouvé ! ";
                return RedirectToAction("Index");
            }
            try
            {
                _context.MatiereGroupe.Remove(matiereGroup);
                _context.SaveChanges();
                TempData["Message"] = "Le groupe des matières communes est supprime avec succes !";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Un problème est survenu lors de la suppression !";
            }
            return RedirectToAction("Index");
        }
    }
}
