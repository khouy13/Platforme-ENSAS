using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;
using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;

namespace Projet.Areas.Coordonnateur.Models
{
    public class ResultatChargeHoraireVacataire
    {

        public string? GradeName { get; set; }
        public Vacataire? vacataire { get; set; }
        public string? NomComplet { get; set; }
        public Semestre? Semestre { get; set; }
        public EmploiExam emploiExam { get; set; }

        public string? NomSemestre { get; set; }
        public int? ChargeHoraire { get; set; }
        public string? NomDepartement { get; set; }

        public bool? IsVacataire { get; set; }

        public string? isComuncours { get; set; }
        public string? TypeEnseignement { get; set; }
        public string? Matiere { get; set; }
        public int? SemaineDebut { get; set; }

        public int? Nombre_Total_heure { get; set; }
        public int? SemaineFin { get; set; }  
    }
}
