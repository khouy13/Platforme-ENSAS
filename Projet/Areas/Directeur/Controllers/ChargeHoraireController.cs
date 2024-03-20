using DocumentFormat.OpenXml.Drawing;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
        public IActionResult CalculChargeHoraire(int? IdS=null, int pg = 1,  int? EId = null, int? VId = null, int? DId = null, int? GId = null)
        {
            const int pageSize = 7;
           
            ViewBag.SemestreId = IdS;

            var semestre = _context.semestres.Find(IdS);
            ViewBag.semestre = semestre;
            var resultats = new List<object>();
          
            List<Enseignant> enseignants;
            List<Vacataire> vacataires;
            List<EmploiTemps> emploiTemps = new List<EmploiTemps>();
            List<GroupInfos> GroupInfoEnseignants = new List<GroupInfos>();
            List<GroupeInfoVacataire> GroupeInfoVacataires = new List<GroupeInfoVacataire>();
           
            ViewBag.GId = GId;
            ViewBag.DId = DId;
            if (DId != null)
            {
                if (VId.HasValue || EId.HasValue)
                {
                    enseignants = _context.Enseignants.Include(e => e.departement).Include(g => g.gradeEnseigant)
                        .Where(e => e.IdEnseignant == EId && e.IdDepartement == DId)
                        .OrderBy(e=>e.NomEnseignant).ToList();
                    vacataires = _context.vacataires.Include(v => v.departement).Include(g => g.gradeEnseigant)
                        .Where(e => e.IdVacataire == VId && e.IdDepartement == DId)
                        .OrderBy(e=>e.Nom).ToList();
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

            if (IdS!=null)
            {
                foreach (var enseignant in enseignants)
                {
                    //ici on a recuperer tous les Emploi de l enseignanat
                    emploiTemps = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Enseignant).Include(t => t.TypeEnseignement)
                                        
                                            .Where(e =>e.IdEnseignant == enseignant.IdEnseignant && e.IdSemestre==IdS)
                                                        .ToList();
                        if (emploiTemps.Count() == 0)
                        {
                            GroupInfoEnseignants.Add(new GroupInfos
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
                        GroupInfoEnseignants.Add(new GroupInfos
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
                        GroupInfoEnseignants.Add(new GroupInfos
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
                        GroupInfoEnseignants.Add(new GroupInfos
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
                    NomSemestre= groupeInfoVacataire.PremierEmploi?.semestre.NomSemestre
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

        public List<GroupInfos> GetChargesGroupInfos(int IdTeacher,bool IsVacataire=false)
        {
            List<EmploiTemps> emplois;
            string NomComplet,GradeName, Departement,NomSemestre, TypeEnseignement;
            int SemaineDebut, SemaineFin;
            var CommunComparer = new CommunCoursesComparer(_context);
            var unionComparer = new UnionComparer();

            if (IsVacataire)
            {
                //ici on a recuperer tous les Emploi de l vacataire
                emplois = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Vacataire)
                                .Where(e => e.IdVacataire == IdTeacher).ToList();
                var vacataire = _context.vacataires.Where(e => e.IdVacataire == IdTeacher).First();
                NomComplet = vacataire.NomComplet;
                Departement = vacataire.departement != null ? vacataire.departement.NomDepartementt : "N/A";
                GradeName = vacataire.gradeEnseigant != null ? vacataire.gradeEnseigant.GradeName : "Grade non spécifié";
            }
            else
            {
                //ici on a recuperer tous les Emploi de l enseignanat
                emplois = _context.EmploiTemps.Include(m => m.Matiere).Include(m => m.Enseignant)
                                .Where(e => e.IdEnseignant == IdTeacher).ToList();
                var enseignant = _context.Enseignants.Where(e => e.IdEnseignant == IdTeacher).First();
                NomComplet = enseignant.NomComplet;
                Departement = enseignant.departement != null ? enseignant.departement.NomDepartementt : "N/A";
                GradeName = enseignant.gradeEnseigant != null ? enseignant.gradeEnseigant.GradeName : "Grade non spécifié";
            }

            List<GroupInfos> infos = new List<GroupInfos>();
            if (emplois.Count == 0)
            {
                infos.Add(new GroupInfos
                {
                    IsComuncours = false,
                    NombreElements = 0,
                    PremierEmploi = null,
                    NomComplet = NomComplet,
                    IsVacataire = IsVacataire,
                    NomSemestre = "",
                    TypeEnseignement = "",
                    // Exemple pour vacataire.departement.NomDepartementt
                    Departement = Departement,
                    // Exemple pour vacataire.gradeEnseigant.GradeName
                    GradeName = GradeName,
                });
            }
            else
            {
                SemaineDebut = emplois.First().SemaineDebut;
                SemaineFin = emplois.First().SemaineFin;
                NomSemestre = _context.semestres.Where(e => e.IdSemestre == emplois.First().IdSemestre).FirstOrDefault()?.NomSemestre??"";
                TypeEnseignement = _context.TypeEnseignements.Where(e => e.Id == emplois.First().IdTypeEnseignement).FirstOrDefault()?.NomEn ?? "0";

                var resultatGroupe = emplois
                               .GroupBy(et => et, CommunComparer)
                               .Select(et => new ChargesHoraires { Seances = et.ToList(), IsCommun = et.Count() > 1 })
                               .GroupBy(et => et, unionComparer)
                               .Select(et =>
                               {
                                   var ChargesGroupes = et.ToList();
                                   ChargesHoraires charge = new() { IsCommun = ChargesGroupes.First().IsCommun };
                                   foreach (var chh in ChargesGroupes)
                                   {
                                       charge.Seances.AddRange(chh.Seances);
                                   }
                                   return charge;
                               })
                               .ToList();

                foreach (var charge in resultatGroupe)
                {
                    infos.Add(new GroupInfos
                    {
                        NomComplet = NomComplet,
                        SemaineDebut = SemaineDebut,
                        SemaineFin = SemaineFin,
                        NomSemestre = NomSemestre,
                        TypeEnseignement = TypeEnseignement,
                        Departement = Departement,
                        GradeName = GradeName,
                        IsVacataire= IsVacataire,
                        IsComuncours = charge.IsCommun,
                        NombreElements = charge.IsCommun ? 1 : charge.Seances.Count,
                        PremierEmploi = charge.Seances.FirstOrDefault(),
                        IdMatieres = charge.IsCommun ? charge.Seances.Select(e => e.IdMatiere).ToList() : new List<int?>() { charge.Seances.First().IdMatiere }
                    });
                }
            }
            return infos;
        }

        [HttpGet]
        public IActionResult DownloadExcelAnnuel(int? DId,int? IdS)
        {
            List<ExportChargeViewModel> excel = new();
            var resultats = new List<object>();
            List<Enseignant> enseignants;
            List<Vacataire> vacataires;

            List<EmploiTemps> emploiTemps = new List<EmploiTemps>();
            List<GroupInfos> groupInfos = new List<GroupInfos>();

            if (DId != null)
            {
                enseignants = _context.Enseignants.Include(e => e.departement).Include(p => p.gradeEnseigant)
                    .Where(e => e.IdDepartement == DId)
                    .OrderBy(e=>e.NomEnseignant)
                    .ToList();
                vacataires = _context.vacataires.Include(v => v.departement).Include(p => p.gradeEnseigant)
                    .Where(e => e.IdDepartement == DId)
                    .OrderBy(e=>e.Nom)
                    .ToList();
            }
            else
            {
                enseignants = _context.Enseignants
                    .Include(e => e.departement).Include(p => p.gradeEnseigant)
                    .OrderBy(e=>e.NomEnseignant)
                    .ToList();
                vacataires = _context.vacataires.Include(v => v.departement)
                    .Include(p => p.gradeEnseigant)
                    .OrderBy(e=>e.Nom)
                    .ToList();
            }
            //Par semestre:
            if (IdS != null)
            {
                foreach (var enseignant in enseignants)
                {
                    groupInfos.AddRange(GetChargesGroupInfos(enseignant.IdEnseignant));
                }
                foreach (var vacataire in vacataires)
                {
                    groupInfos.AddRange(GetChargesGroupInfos(vacataire.IdVacataire, true));
                }
            }

            else {
                
                foreach (var enseignant in enseignants)
                {
                    groupInfos.AddRange(GetChargesGroupInfos(enseignant.IdEnseignant));
                }
                foreach (var vacataire in vacataires)
                {
                    groupInfos.AddRange(GetChargesGroupInfos(vacataire.IdVacataire, true));
                }
            }

            foreach (var groupInfo in groupInfos)
            {
                var MatiereNomList = groupInfo.IdMatieres?.Select(e =>
                {
                    var matiere = _context.Matieres.Where(m => m.IdMatiere == e).FirstOrDefault();
                    if (matiere == null)
                    {
                        return "";
                    }
                    return matiere.NomMatiere;
                });
                string matieres = string.Join(",", MatiereNomList?.Select(x => x.ToString()) ?? Enumerable.Empty<string>());
                var nombreTotalHeures = (groupInfo.SemaineFin??0 - groupInfo.SemaineDebut??0 + 1)*(groupInfo.NombreElements??0)*2;

                excel.Add(new ExportChargeViewModel
                {
                    Role = groupInfo.IsVacataire ? "Vacataire":"Enseignant",
                    NomComplet = groupInfo.NomComplet,
                    NomDepartement = groupInfo.Departement,
                    TypeEnseignement = groupInfo.TypeEnseignement,
                    Grade = groupInfo.GradeName,
                    //Charge Horaire
                    Nombre_de_seance_par_Semaine = groupInfo.NombreElements.ToString(),
                    SemaineDebut = groupInfo.SemaineDebut,
                    SemaineFin = groupInfo.SemaineFin,
                    Matiere = matieres,
                    isComuncours = (groupInfo.IsComuncours ?? false) ? "oui" : "non",
                    Semestre = groupInfo.NomSemestre,
                    Nombre_Total_heure = nombreTotalHeures
                });
            }
            return new ExcelResult<ExportChargeViewModel>(excel, "Sheet1", "chargeHoraireAnnuels");
        }
    }

    public class CommunCoursesComparer : IEqualityComparer<EmploiTemps>
    {
        private readonly AppDbContext _context;
        public CommunCoursesComparer(AppDbContext _context)
        {
            this._context = _context;
        }
        public bool Equals(EmploiTemps? x, EmploiTemps? y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            var e = new CommunCoursesHandler(_context);
            return e.CommonCourse(x, y)
                    && y.IdTypeEnseignement == x.IdTypeEnseignement;
        }

        public int GetHashCode([DisallowNull] EmploiTemps obj)
        {
            return 0;
        }
    }
    public class UnionComparer : IEqualityComparer<ChargesHoraires>
    {
        public bool Equals(ChargesHoraires? x, ChargesHoraires? y)
        {
            if (x == null || y == null || !x.Seances.Any() || !y.Seances.Any() || x.IsCommun || y.IsCommun)
            {
                return false;
            }
            var cdt = x.Seances.First().IdTypeEnseignement == y.Seances.First().IdTypeEnseignement
                && x.Seances.First().IdMatiere == y.Seances.First().IdMatiere 
                && x.Seances.First().SemaineDebut == y.Seances.First().SemaineDebut
                && x.Seances.First().SemaineFin == y.Seances.First().SemaineFin;
            return cdt;
        }

        public int GetHashCode([DisallowNull] ChargesHoraires obj)
        {
            return 0;
        }
    }
    public class ChargesHoraires
    {
        public List<EmploiTemps> Seances { get; set;} = new List<EmploiTemps>();
        public bool IsCommun { get; set;}
    }
}
