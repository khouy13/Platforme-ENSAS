using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Projet.Areas.Coordonnateur.Controllers
{
    [Area("Directeur")]
    public class SurveillantController : Controller
    {
        private readonly AppDbContext _context;

        public SurveillantController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CalculChargeSurveillant(int? IdS = null, int pg = 1, int? EId = null, int? VId = null, int? DId = null ,int? GId = null)
        {
            const int pageSize = 6; // Nombre d'éléments par page
          
            ViewBag.SemestreId = IdS;
           
            var semestre = _context.semestres.Find(IdS);
            ViewBag.semestre = semestre;
            List<Enseignant> enseignants;
            List<Vacataire> vacataires;
            List<ResultatChargeHoraire> teacherWorkloads;
            List<ResultatChargeHoraireVacataire> lecturerWorkloads;
            var did = DId;
            ViewBag.DId = did;
            ViewBag.GId = GId;
            if (DId!= null)
            {
                if(VId.HasValue || EId.HasValue)
                {
                    enseignants = _context.Enseignants
                        .Include(e => e.departement)
                        .Include(g => g.gradeEnseigant)
                        .OrderBy(e=>e.NomEnseignant)
                        .Where(e => e.IdEnseignant == EId && e.IdDepartement == DId)
                        .ToList();
                    vacataires = _context.vacataires
                        .Include(v => v.departement)
                        .Include(g => g.gradeEnseigant)
                        .OrderBy(v=>v.Nom)
                        .Where(e => e.IdVacataire == VId && e.IdDepartement == DId).ToList();
                }
                else if (GId != null)
                {
                    enseignants = _context.Enseignants
                        .Include(e => e.departement)
                        .Include(g => g.gradeEnseigant)
                        .Where(e => e.IdDepartement == DId && e.GradeId == GId)
                        .OrderBy(e => e.NomEnseignant)
                        .ToList();
                    vacataires = _context.vacataires
                        .Include(v => v.departement).Include(g => g.gradeEnseigant)
                        .OrderBy(e => e.Nom)
                        .Where(e => e.IdDepartement == DId && e.GradeId == GId).ToList();
                }
                else { 
                enseignants = _context.Enseignants
                        .Include(e => e.departement).Include(g=>g.gradeEnseigant)
                        .OrderBy(e => e.NomEnseignant)
                        .Where(e => e.IdDepartement == DId).ToList();
            
                 vacataires = _context.vacataires
                        .Include(v => v.departement).Include(g=>g.gradeEnseigant)
                        .OrderBy(e => e.Nom)
                        .Where(e => e.IdDepartement == DId).ToList();
                }
            }
            else if (VId.HasValue || EId.HasValue )
            {
                enseignants = _context.Enseignants
                    .Include(e => e.departement).Include(g => g.gradeEnseigant)
                    .OrderBy(e => e.NomEnseignant)
                    .Where(e => e.IdEnseignant == EId).ToList();
                vacataires = _context.vacataires
                    .Include(v => v.departement).Include(g => g.gradeEnseigant)
                    .OrderBy(e => e.Nom)
                    .Where(e => e.IdVacataire == VId).ToList();

            }
            else if (GId != null)
            {
                enseignants = _context.Enseignants
                    .Include(e => e.departement).Include(g => g.gradeEnseigant)
                    .OrderBy(e => e.NomEnseignant)
                    .Where( e=> e.GradeId == GId).ToList();
                vacataires = _context.vacataires
                    .Include(v => v.departement).Include(g => g.gradeEnseigant)
                    .OrderBy(e => e.Nom)
                    .Where( e=>e.GradeId == GId).ToList();
            }
            else
            {
                enseignants = _context.Enseignants
                    .Include(e => e.departement).Include(g => g.gradeEnseigant)
                    .OrderBy(e => e.NomEnseignant).ToList();
                vacataires = _context.vacataires
                    .Include(v => v.departement).Include(g => g.gradeEnseigant)
                    .OrderBy(e => e.Nom).ToList();
            }

            if (DId.HasValue)
            {
                ViewBag.vacataires2 = new SelectList(_context.vacataires.Where(e=>e.IdDepartement==DId).OrderBy(e =>e.Nom).ToList(), "IdVacataire", "NomComplet", VId);
                ViewBag.enseignants2 = new SelectList(_context.Enseignants.Where(e => e.IdDepartement == DId).OrderBy(e => e.NomEnseignant).ToList(), "IdEnseignant", "NomComplet", EId);
            }
            else
            {
                ViewBag.vacataires2 = new SelectList(_context.vacataires.ToList().OrderBy(e =>e.Nom), "IdVacataire", "NomComplet", VId);
                ViewBag.enseignants2 = new SelectList(_context.Enseignants.ToList().OrderBy(e => e.NomEnseignant), "IdEnseignant", "NomComplet", EId);
            }

            var Grades = _context.Grades.OrderBy(e=>e.GradeName).ToList();
            ViewBag.Grades = new SelectList(Grades, "GradeId", "GradeName", GId);
            var Departement = _context.Departements.ToList();
            ViewBag.semestres = new SelectList(_context.semestres.ToList(), "IdSemestre", "NomSemestre", IdS);
            ViewBag.Departements = new SelectList(Departement, "IdDepartement", "NomDepartementt", DId);

            if (IdS.HasValue)
            {
                teacherWorkloads=enseignants.Select(enseignant => new ResultatChargeHoraire
                {
                    Enseignant = enseignant,
                    Semestre = semestre,
                    GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",
                    NomDepartement = enseignant.departement?.NomDepartementt ?? "N/A",
                    ChargeHoraire = _context.EmploiExamEnseignants
                        .Where(ee => ee.IdEnseignant == enseignant.IdEnseignant && ee.EmploiExam.IdSemestre==IdS)
                        .Select(ee => ee.EmploiExam.IdMatiere)  // Sélectionnez les IdMatiere
                        .Distinct()  // Filtrez les matières distinctes
                        .Count() * 2,
                }).ToList();

              lecturerWorkloads = vacataires.Select(vacataire => new ResultatChargeHoraireVacataire
                {
                    vacataire = vacataire,
                    Semestre = semestre,
                    GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",
                    NomDepartement = vacataire.departement?.NomDepartementt ?? "N/A",
                    ChargeHoraire = _context.EmploiExamVacataires
                        .Where(ee => ee.Vacataire.IdVacataire == vacataire.IdVacataire && ee.EmploiExam.IdSemestre==IdS)
                        .Select(ee => ee.EmploiExam.IdMatiere)  // Sélectionnez les IdMatiere
                        .Distinct()  // Filtrez les matières distinctes
                        .Count() * 2,
              }).ToList();
            }
            else
            {
                teacherWorkloads = enseignants.Select(enseignant => new ResultatChargeHoraire
                {
                    Enseignant = enseignant,
                    Semestre = semestre,
                    GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",
                    NomDepartement = enseignant.departement?.NomDepartementt ?? "N/A",
                        ChargeHoraire = _context.EmploiExamEnseignants
                        .Where(ee => ee.IdEnseignant == enseignant.IdEnseignant)
                    .Select(ee => ee.EmploiExam.IdMatiere)  // Sélectionnez les IdMatiere
                    .Distinct()  // Filtrez les matières distinctes
                    .Count() * 2,
                }).ToList();
                lecturerWorkloads = vacataires.Select(vacataire => new ResultatChargeHoraireVacataire
                {
                    vacataire = vacataire,
                    Semestre = semestre,
                    GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",
                    NomDepartement = vacataire.departement?.NomDepartementt ?? "N/A",
                    ChargeHoraire = _context.EmploiExamVacataires
                    .Where(ee => ee.Vacataire.IdVacataire == vacataire.IdVacataire)
                    .Select(ee => ee.EmploiExam.IdMatiere)  // Sélectionnez les IdMatiere
                    .Distinct()  // Filtrez les matières distinctes
                    .Count() * 2,
                }).ToList();
            }

            // Combinez les résultats
            var resultats = new List<object>();
            resultats.AddRange(teacherWorkloads);
            resultats.AddRange(lecturerWorkloads);

            // Pagination
            var pager = new Pager(resultats.Count, pg, pageSize);
            var pagedResultats = resultats
                .Skip((pg - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Pager = pager;
            ViewBag.DId = DId;

            return View(pagedResultats);
        }

        [HttpGet]
        public IActionResult DownloadExcelAnnuel( int? DId=null)
        {
            List<CHargeServaianceViewModel> excel = new List<CHargeServaianceViewModel>();
            var resultats = new List<object>();
            List<Enseignant> enseignants;

            List<Vacataire> vacataires;
            if (DId.HasValue)
            {
                vacataires= _context.vacataires
                .Include(v => v.departement).OrderBy(e=>e.Nom)
                .Include(p => p.gradeEnseigant).Where(p=>p.IdDepartement==DId)
                .ToList();
                enseignants= _context.Enseignants
                .Include(e => e.departement)
                .Include(p => p.gradeEnseigant).Where(p => p.IdDepartement == DId).OrderBy(n=>n.NomEnseignant)
                .ToList();
            }
            else
            {
                vacataires = _context.vacataires
                               .Include(v => v.departement)
                               .Include(p => p.gradeEnseigant).OrderBy(e => e.Nom)
                               .ToList();
                enseignants = _context.Enseignants
                .Include(e => e.departement)
                .Include(p => p.gradeEnseigant).OrderBy(e => e.NomEnseignant)
                .ToList();
            }

            var teacherWorkloads = enseignants.Select(enseignant => new ResultatChargeHoraire
            {
                Enseignant = enseignant,
               
                GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",
                NomDepartement = enseignant.departement?.NomDepartementt ?? "N/A",
                ChargeHoraire = _context.EmploiExamEnseignants
                    .Where(ee => ee.IdEnseignant == enseignant.IdEnseignant)
                    .Select(ee => ee.EmploiExam.IdMatiere)  // Sélectionnez les IdMatiere
                    .Distinct()  // Filtrez les matières distinctes
                    .Count() * 2, // Comptez le nombre de matières distinctes et multipliez par 2
            }).ToList();

            var lecturerWorkloads = vacataires.Select(vacataire => new ResultatChargeHoraireVacataire
            {
                vacataire = vacataire,
               
                GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",
                NomDepartement = vacataire.departement?.NomDepartementt ?? "N/A",
                ChargeHoraire = _context.EmploiExamVacataires
                    .Where(ee => ee.Vacataire.IdVacataire == vacataire.IdVacataire)
                    .Select(ee =>ee.EmploiExam.IdMatiere)  // Sélectionnez les IdMatiere
                    .Distinct()  // Filtrez les matières distinctes
                    .Count() * 2, // Comptez le nombre de matières distinctes et multipliez par 2

        }).ToList();
          
            resultats.AddRange(teacherWorkloads);
            resultats.AddRange(lecturerWorkloads);

            foreach (var resultat in resultats)
            {
                if (resultat is ResultatChargeHoraire)
                {
                    var result = (ResultatChargeHoraire)resultat;
                    excel.Add(new CHargeServaianceViewModel
                    {
                        NomComplet = result.Enseignant.NomComplet,
                        NomDepartement = result.NomDepartement,
                        Grade = result.GradeName,
                        

                       Nombre_heure_de_servaiance = result.ChargeHoraire.ToString()
                    });
                }
                else if (resultat is ResultatChargeHoraireVacataire)
                {
                    var result = (ResultatChargeHoraireVacataire)resultat;
                    excel.Add(new CHargeServaianceViewModel
                    {
                        NomComplet = result.vacataire.NomComplet,
                        NomDepartement = result.NomDepartement,
                        Grade = result.GradeName,
                     Nombre_heure_de_servaiance = result.ChargeHoraire.ToString()
                    });
                }
            }

   
            return new ExcelResult<CHargeServaianceViewModel>(excel, "Sheet1", "charge-Servaiances");
        }

    }
}