using DocumentFormat.OpenXml.Office2010.Excel;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using Projet.Migrations;
using System;
using System.Security.Cryptography;

namespace Projet.Areas.Directeur.Controllers
{
    [Area("Directeur")]
    public class ChargeHoraireController : Controller
    {
        private readonly AppDbContext _context;
       
        public ChargeHoraireController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult CalculChargeHoraire(int? IdS=null, int pg = 1,  int? EId = null, int? VId = null, int? DId = null, int? GId = null)
        {
            const int pageSize = 7;
           
            ViewBag.SemestreId = IdS;

            var semestre = _context.semestres.Find(IdS);
            ViewBag.semestre = semestre;
            var resultats = new List<object>();
          
            List<Enseignant> enseignants;
            List<Vacataire> vacataires;
            List<EmploiTemps> emploiTemps = new List<EmploiTemps>();
            List<GroupInfoEnseignant> GroupInfoEnseignants = new List<GroupInfoEnseignant>();
            List<GroupeInfoVacataire> GroupeInfoVacataires = new List<GroupeInfoVacataire>();
           
            ViewBag.GId = GId;
            ViewBag.DId = DId;
            if (DId != null)
            {
                if (VId.HasValue || EId.HasValue)
                {
                    enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).Where(e => e.IdEnseignant == EId && e.IdDepartement == DId).OrderBy(e=>e.NomEnseignant).ToList();
                    vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant).Where(e => e.IdVacataire == VId && e.IdDepartement == DId).OrderBy(e=>e.Nom).ToList();
                }
                else if (GId != null)
                {
                    enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).Where(e => e.IdDepartement == DId && e.GradeId == GId).OrderBy(e => e.NomEnseignant).ToList();
                    vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant).Where(e => e.IdDepartement == DId && e.GradeId == GId).OrderBy(e=>e.Nom).ToList();
                }
                else
                {
                    enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).Where(e => e.IdDepartement == DId).OrderBy(e => e.NomEnseignant).ToList();

                    vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant).Where(e => e.IdDepartement == DId).OrderBy(e=>e.Nom).ToList();
                }
            }
            else if (VId.HasValue || EId.HasValue)
            {
                enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).Where(e => e.IdEnseignant == EId).OrderBy(e => e.NomEnseignant).ToList();
                vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant).Where(e => e.IdVacataire == VId).OrderBy(e=>e.Nom).ToList();

            }
            else if (GId != null)
            {
                enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).Where(e =>  e.GradeId == GId).OrderBy(e => e.NomEnseignant).ToList();
                vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant).Where(e =>  e.GradeId == GId).OrderBy(e => e.Nom).ToList();
            }
            else
            {
                enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant).OrderBy(e => e.NomEnseignant).OrderBy(e=>e.NomEnseignant).ToList();
                vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant).OrderBy(e=>e.Nom).OrderBy(e=>e.Nom).ToList();
            }
            if(DId.HasValue)
            {
                ViewBag.vacataires = new SelectList(_context.vacataires.OrderBy(e=>e.Nom).Where(e=>e.IdDepartement==DId).OrderBy(e => e.Nom).ToList(), "IdVacataire", "NomComplet", VId);
                ViewBag.enseignants = new SelectList(_context.Enseignants.OrderBy(e=>e.NomEnseignant).Where(e => e.IdDepartement == DId).OrderBy(e => e.NomEnseignant).ToList(), "IdEnseignant", "NomComplet", EId);
            }
            else {
            ViewBag.vacataires = new SelectList(_context.vacataires.OrderBy(e=>e.Nom).ToList(), "IdVacataire", "NomComplet", VId);
            ViewBag.enseignants = new SelectList(_context.Enseignants.OrderBy(e => e.NomEnseignant).ToList(), "IdEnseignant", "NomComplet", EId);

            }
            var Departement = _context.Departements.ToList();
            ViewBag.Departements = new SelectList(Departement, "IdDepartement", "NomDepartementt", DId);
            ViewBag.semestres = new SelectList(_context.semestres.OrderBy(s=>s.SemaineDebut).ToList(), "IdSemestre", "NomSemestre",IdS);
            var Grades = _context.Grades.OrderBy(e=>e.GradeName).ToList();
            ViewBag.Grades = new SelectList(Grades, "GradeId", "GradeName", GId);



            if (IdS!=null) {
           
            foreach (var enseignant in enseignants)
            {
                //ici on a recuperer tous les Emploi de l enseignanat
                emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Enseignant).Include(t => t.TypeEnseignement)
                                        
                                        .Where(e =>e.IdEnseignant == enseignant.IdEnseignant && e.IdSemestre==IdS)
                                                    .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupInfoEnseignants.Add(new GroupInfoEnseignant
                        {
                            IsComuncours =false,
                            NombreElements =0, 
                            PremierEmploi = null,
                            NomComplet=enseignant.NomComplet,
                            Departement = enseignant.departement != null ? enseignant.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                            GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",
                        });
                    }
                    
                var resultatGroupe = emploiTemps
               .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                       .ToList();
                foreach (var groupe in resultatGroupe)
                {
                    GroupInfoEnseignants.Add(new GroupInfoEnseignant
                    {
                        IsComuncours = groupe.Key.isComuncours,
                        NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                        PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                    });
                }


            }
            foreach (var vacataire in vacataires)
            {
                //ici on a recuperer tous les Emploi de l enseignanat
                emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Vacataire).Include(t => t.TypeEnseignement)
                                        .Include(s => s.semestre)
                                        .Where(e => e.IdVacataire == vacataire.IdVacataire && e.IdSemestre == IdS)
                                                    .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupeInfoVacataires.Add(new GroupeInfoVacataire
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = vacataire.NomComplet,

                            Departement = vacataire.departement != null ? vacataire.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                            GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",
                        });
                    }

                    var resultatGroupe = emploiTemps
               .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                       .ToList();
                  foreach(var groupe in resultatGroupe) {
                        GroupeInfoVacataires.Add(
                             new GroupeInfoVacataire
                             {

                                 IsComuncours = groupe.Key.isComuncours,
                                 NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                                 PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                             });
             
                    }
                }
            }
            else
            {

                foreach (var enseignant in enseignants)
                {
                    //ici on a recuperer tous les Emploi de l enseignanat
                    emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Enseignant).Include(t => t.TypeEnseignement)

                                            .Where(e => e.IdEnseignant == enseignant.IdEnseignant )
                                                        .ToList();

                    if (emploiTemps.Count() == 0)
                    {
                        GroupInfoEnseignants.Add(new GroupInfoEnseignant
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = enseignant.NomComplet,
                            // Exemple pour vacataire.departement.NomDepartementt
                            Departement = enseignant.departement != null ? enseignant.departement.NomDepartementt : "N/A",

                        // Exemple pour vacataire.gradeEnseigant.GradeName
                        GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",

                    });
                    }

                    var resultatGroupe = emploiTemps
                   .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                           .ToList();
                    foreach (var groupe in resultatGroupe)
                    {
                        GroupInfoEnseignants.Add(new GroupInfoEnseignant
                        {
                            IsComuncours = groupe.Key.isComuncours,
                            NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                            PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                        });
                    }


                }
                foreach (var vacataire in vacataires)
                {
                    //ici on a recuperer tous les Emploi de l enseignanat
                    emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Vacataire).Include(t => t.TypeEnseignement)
                                            .Include(s => s.semestre)
                                            .Where(e => e.IdVacataire == vacataire.IdVacataire)
                                                        .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupeInfoVacataires.Add(new GroupeInfoVacataire
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = vacataire.NomComplet,
                            // Exemple pour vacataire.departement.NomDepartementt
                            Departement = vacataire.departement != null ? vacataire.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                             GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",


                    });
                    }

                    var resultatGroupe = emploiTemps
                   .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                           .ToList();
                    foreach (var groupe in resultatGroupe)
                    {
                        GroupeInfoVacataires.Add(
                             new GroupeInfoVacataire
                             {

                                 IsComuncours = groupe.Key.isComuncours,
                                 NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                                 PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                             });

                    }

                }











            }

            foreach (var groupeInfoVacataire in GroupeInfoVacataires)
            {

                resultats.Add(new ResultatChargeHoraireVacataire
                {
                    vacataire = groupeInfoVacataire.PremierEmploi?.Vacataire,
                    NomComplet= groupeInfoVacataire.NomComplet,

                    GradeName = groupeInfoVacataire.PremierEmploi?.Vacataire.gradeEnseigant != null ? groupeInfoVacataire.PremierEmploi?.Vacataire.gradeEnseigant.GradeName :groupeInfoVacataire.GradeName,
                    NomDepartement = groupeInfoVacataire.PremierEmploi?.Vacataire.departement?.NomDepartementt ?? groupeInfoVacataire.Departement,
                    //ici si le Nombre de seance par semaine :
                    ChargeHoraire = groupeInfoVacataire.NombreElements,

                    TypeEnseignement = groupeInfoVacataire.PremierEmploi?.TypeEnseignement.NomEn,
                    SemaineDebut = groupeInfoVacataire.PremierEmploi?.SemaineDebut,
                    Matiere = groupeInfoVacataire.PremierEmploi?.Matiere.NomMatiere,
                    SemaineFin = groupeInfoVacataire.PremierEmploi?.SemaineFin,
                  
                    isComuncours = groupeInfoVacataire.IsComuncours.HasValue ? (groupeInfoVacataire.IsComuncours.Value ? "oui" : "non") : "non",


                //ici on fait le calcul Nombre d heure de travail du le debut jisqu a la fin :
                Nombre_Total_heure = ((groupeInfoVacataire.PremierEmploi?.SemaineFin-groupeInfoVacataire.PremierEmploi?.SemaineDebut)+1)*groupeInfoVacataire.NombreElements*2,
                    Nomsemestre= groupeInfoVacataire.PremierEmploi?.semestre.NomSemestre
                });
            }
            foreach (var groupeInfoEnseig in GroupInfoEnseignants)
            {

                resultats.Add(new ResultatChargeHoraire
                {
                    Enseignant = groupeInfoEnseig.PremierEmploi?.Enseignant,
                    NomComplet = groupeInfoEnseig.NomComplet,
                    GradeName = groupeInfoEnseig.PremierEmploi?.Enseignant?.gradeEnseigant?.GradeName ?? groupeInfoEnseig.GradeName,
                    NomDepartement = groupeInfoEnseig.PremierEmploi?.Enseignant?.departement?.NomDepartementt ?? groupeInfoEnseig.Departement,
                    ChargeHoraire = groupeInfoEnseig.NombreElements,
                    TypeEnseignement = groupeInfoEnseig.PremierEmploi?.TypeEnseignement?.NomEn,
                    SemaineDebut = groupeInfoEnseig.PremierEmploi?.SemaineDebut,
                    SemaineFin = groupeInfoEnseig.PremierEmploi?.SemaineFin,
                    Matiere = groupeInfoEnseig.PremierEmploi?.Matiere?.NomMatiere,
                    //isComuncours = groupeInfoEnseig.IsComuncours?.ToString(),
                    isComuncours = groupeInfoEnseig.IsComuncours.HasValue ? (groupeInfoEnseig.IsComuncours.Value ? "oui" : "non") : "non",
                    Nombre_Total_heure = ((groupeInfoEnseig.PremierEmploi?.SemaineFin - groupeInfoEnseig.PremierEmploi?.SemaineDebut) + 1) * groupeInfoEnseig.NombreElements * 2,
                    Nomsemestre = groupeInfoEnseig.PremierEmploi?.semestre?.NomSemestre
                });
            }
         


            // Pagination
            var pager = new Pager(resultats.Count, pg, pageSize);
            var pagedResultats = resultats.Skip((pg - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Pager = pager;

            return View(pagedResultats);

        }




        [HttpGet]
        public IActionResult DownloadExcelAnnuel(int? DId=null,int? IdS=null)
        {
        List<ExportChargeViewModel> excel = new List<ExportChargeViewModel>();
        var resultats = new List<object>();
            List<Enseignant> enseignants;
            List<Vacataire> vacataires;

            List<EmploiTemps> emploiTemps = new List<EmploiTemps>();
            List<GroupInfoEnseignant> GroupInfoEnseignants = new List<GroupInfoEnseignant>();

            List<GroupeInfoVacataire> GroupeInfoVacataires = new List<GroupeInfoVacataire>();

             if (DId != null)
            {

                enseignants = _context.Enseignants.Include(e => e.departement).Include(p => p.gradeEnseigant).OrderBy(e=>e.NomEnseignant).Where(e => e.IdDepartement == DId).ToList();
                vacataires = _context.vacataires.Include(v => v.departement).Include(p => p.gradeEnseigant).OrderBy(e=>e.Nom).Where(e => e.IdDepartement == DId).ToList();
            }
            else
            {
                enseignants = _context.Enseignants.Include(e => e.departement).Include(p => p.gradeEnseigant).OrderBy(e=>e.NomEnseignant).ToList();
                vacataires = _context.vacataires.Include(v => v.departement).Include(p => p.gradeEnseigant).OrderBy(e=>e.Nom).ToList();
            }
            //Par semestre:
            if (IdS != null)
            {

                foreach (var enseignant in enseignants)
                {
                    //ici on a recuperer tous les Emploi de l enseignanat
                    emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Enseignant).Include(t => t.TypeEnseignement)

                                            .Where(e => e.IdEnseignant == enseignant.IdEnseignant && e.IdSemestre == IdS)
                                                        .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupInfoEnseignants.Add(new GroupInfoEnseignant
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = enseignant.NomComplet,
                            // Exemple pour vacataire.departement.NomDepartementt
                            Departement = enseignant.departement != null ? enseignant.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                            GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",

                        });
                    }

                    var resultatGroupe = emploiTemps
                   .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                           .ToList();
                    foreach (var groupe in resultatGroupe)
                    {
                        GroupInfoEnseignants.Add(new GroupInfoEnseignant
                        {
                            IsComuncours = groupe.Key.isComuncours,
                            NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                            PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                        });
                    }


                }
                foreach (var vacataire in vacataires)
                {
                    //ici on a recuperer tous les Emploi de l enseignanat
                    emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Vacataire).Include(t => t.TypeEnseignement)
                                            .Include(s => s.semestre)
                                            .Where(e => e.IdVacataire == vacataire.IdVacataire && e.IdSemestre == IdS)
                                                        .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupeInfoVacataires.Add(new GroupeInfoVacataire
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = vacataire.NomComplet,
                            // Exemple pour vacataire.departement.NomDepartementt
                            Departement = vacataire.departement != null ? vacataire.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                            GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",


                        });
                    }

                    var resultatGroupe = emploiTemps
                   .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                           .ToList();
                    foreach (var groupe in resultatGroupe)
                    {
                        GroupeInfoVacataires.Add(
                             new GroupeInfoVacataire
                             {

                                 IsComuncours = groupe.Key.isComuncours,
                                 NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                                 PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                             });

                    }

                }
            }





            else { 




            foreach (var enseignant in enseignants)
            {
                //ici on a recuperer tous les Emploi de l enseignanat
                emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Enseignant).Include(t => t.TypeEnseignement)
                                       .Include(s => s.semestre)
                                        .Where(e=> e.IdEnseignant == enseignant.IdEnseignant)
                                                    .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupInfoEnseignants.Add(new GroupInfoEnseignant
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = enseignant.NomComplet,
                            // Exemple pour vacataire.departement.NomDepartementt
                            Departement = enseignant.departement != null ? enseignant.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                            GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié",

                        });
                    }

                    var resultatGroupe = emploiTemps
               .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                       .ToList();
                foreach (var groupe in resultatGroupe)
                {
                    GroupInfoEnseignants.Add(new GroupInfoEnseignant
                    {
                        IsComuncours = groupe.Key.isComuncours,
                        NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                        PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                    });
                }




            }
            foreach (var vacataire in vacataires)
            {
                //ici on a recuperer tous les Emploi de l enseignanat
                emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Vacataire).Include(t => t.TypeEnseignement)
                                        .Include(s => s.semestre)
                                        .Where(e => e.IdVacataire == vacataire.IdVacataire)
                                                    .ToList();
                    if (emploiTemps.Count() == 0)
                    {
                        GroupeInfoVacataires.Add(new GroupeInfoVacataire
                        {
                            IsComuncours = false,
                            NombreElements = 0,
                            PremierEmploi = null,
                            NomComplet = vacataire.NomComplet,
                            // Exemple pour vacataire.departement.NomDepartementt
                            Departement = vacataire.departement != null ? vacataire.departement.NomDepartementt : "N/A",

                            // Exemple pour vacataire.gradeEnseigant.GradeName
                            GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié",


                        });
                    }

                    var resultatGroupe = emploiTemps
               .GroupBy(e => new { e.IdMatiere, e.IdTypeEnseignement, e.isComuncours })
                                       .ToList();
                    foreach (var groupe in resultatGroupe)
                    {
                        GroupeInfoVacataires.Add(
                             new GroupeInfoVacataire
                             {

                                 IsComuncours = groupe.Key.isComuncours,
                                 NombreElements = groupe.Key.isComuncours == true ? groupe.Count() / 2 : groupe.Count(),
                                 PremierEmploi = groupe.FirstOrDefault() // Assignez l'élément à afficher
                             });

                    }




                }
            }
            foreach (var groupeInfoVacataire in GroupeInfoVacataires)
            {

                resultats.Add(new ResultatChargeHoraireVacataire
                {
                    vacataire = groupeInfoVacataire.PremierEmploi?.Vacataire,

                    GradeName = groupeInfoVacataire.PremierEmploi?.Vacataire.gradeEnseigant != null ? groupeInfoVacataire.PremierEmploi?.Vacataire.gradeEnseigant.GradeName : groupeInfoVacataire.GradeName,
                    NomDepartement = groupeInfoVacataire.PremierEmploi?.Vacataire.departement?.NomDepartementt ?? groupeInfoVacataire.Departement,
                    ChargeHoraire = groupeInfoVacataire.NombreElements,
                    NomComplet = groupeInfoVacataire.NomComplet,
                    TypeEnseignement = groupeInfoVacataire.PremierEmploi?.TypeEnseignement.NomEn,
                    SemaineDebut = groupeInfoVacataire.PremierEmploi?.SemaineDebut,
                    Matiere= groupeInfoVacataire.PremierEmploi?.Matiere.NomMatiere,
                    SemaineFin = groupeInfoVacataire.PremierEmploi?.SemaineFin,
                    //isComuncours = groupeInfoVacataire.IsComuncours.ToString(),
                    isComuncours = groupeInfoVacataire.IsComuncours.HasValue ? (groupeInfoVacataire.IsComuncours.Value ? "oui" : "non") : "non",

                    Nombre_Total_heure = ((groupeInfoVacataire.PremierEmploi?.SemaineFin - groupeInfoVacataire.PremierEmploi?.SemaineDebut) + 1) * groupeInfoVacataire.NombreElements * 2,
                    Nomsemestre = groupeInfoVacataire.PremierEmploi?.semestre.NomSemestre
                });
            }
            foreach (var groupeInfoEnseig in GroupInfoEnseignants)
            {

                resultats.Add(new ResultatChargeHoraire
                {
                    Enseignant = groupeInfoEnseig.PremierEmploi?.Enseignant,

                    GradeName = groupeInfoEnseig.PremierEmploi?.Enseignant.gradeEnseigant != null ? groupeInfoEnseig.PremierEmploi?.Enseignant.gradeEnseigant.GradeName : groupeInfoEnseig.GradeName,
                    NomDepartement = groupeInfoEnseig.PremierEmploi?.Enseignant.departement?.NomDepartementt ?? groupeInfoEnseig.Departement,
                    ChargeHoraire = groupeInfoEnseig.NombreElements,
                    NomComplet = groupeInfoEnseig.NomComplet,
                    TypeEnseignement = groupeInfoEnseig.PremierEmploi?.TypeEnseignement.NomEn,
                    SemaineDebut = groupeInfoEnseig.PremierEmploi?.SemaineDebut,
                    SemaineFin = groupeInfoEnseig.PremierEmploi?.SemaineFin,
                    Matiere = groupeInfoEnseig.PremierEmploi?.Matiere.NomMatiere,
                    //isComuncours = groupeInfoEnseig.IsComuncours.ToString(),
                    isComuncours = groupeInfoEnseig.IsComuncours.HasValue ? (groupeInfoEnseig.IsComuncours.Value ? "oui" : "non") : "non",
                    Nombre_Total_heure = ((groupeInfoEnseig.PremierEmploi?.SemaineFin - groupeInfoEnseig.PremierEmploi?.SemaineDebut) + 1) * groupeInfoEnseig.NombreElements * 2,
                    Nomsemestre = groupeInfoEnseig.PremierEmploi?.semestre.NomSemestre
                });
            }


            foreach (var resultat in resultats)
            {
                if (resultat is ResultatChargeHoraire)
                {
                    var result = (ResultatChargeHoraire)resultat;
                    excel.Add(new ExportChargeViewModel
                    {
                       
                       NomComplet = result.Enseignant != null? result.Enseignant.NomComplet: result.NomComplet,
                         NomDepartement = result.NomDepartement,
                        TypeEnseignement=result.TypeEnseignement,
                        Grade = result.GradeName,
                        Nombre_de_seance_par_Semaine =result.ChargeHoraire.ToString(),
                        SemaineDebut=result.SemaineDebut,
                        SemaineFin=result.SemaineFin,
                        Matiere=result.Matiere,
                        isComuncours=result.isComuncours,
                        Semestre=result.Nomsemestre,
                        Nombre_Total_heure=result.Nombre_Total_heure,

                        

                    });
                }
                else if (resultat is ResultatChargeHoraireVacataire)
                {
                    var result = (ResultatChargeHoraireVacataire)resultat;
                    excel.Add(new ExportChargeViewModel
                    {
                       
                        NomComplet = result.vacataire != null ? result.vacataire.NomComplet : result.NomComplet,
                        NomDepartement = result.NomDepartement,
                        TypeEnseignement = result.TypeEnseignement,
                        Grade = result.GradeName,
                        Nombre_de_seance_par_Semaine = result.ChargeHoraire.ToString(),
                        SemaineDebut = result.SemaineDebut,
                        SemaineFin = result.SemaineFin,
                        Matiere = result.Matiere,
                        isComuncours = result.isComuncours,
                         Semestre = result.Nomsemestre,
                        Nombre_Total_heure = result.Nombre_Total_heure,

                    });
                }
            }
           
            return new ExcelResult<ExportChargeViewModel>(excel, "Sheet1", "chargeHoraireAnnuels");
        }

         

    }
}
