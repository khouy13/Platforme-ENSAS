using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;

namespace Projet.Areas.Coordonnateur.Models
{
    public class ExportChargeViewModel
    {
      
        [IncludeInReport(Order = 1)]
        public string? NomComplet { get; set; }
        [IncludeInReport(Order = 2)]
        public string? NomDepartement { get; set; }
        [IncludeInReport(Order = 3)]
        public string? Grade { get; set; }
       
       
        [IncludeInReport(Order = 4)]
        public string? Matiere { get; set; }
        [IncludeInReport(Order = 5)]
        public string? TypeEnseignement { get; set; }
        [IncludeInReport(Order = 6)]
        public int? SemaineDebut { get; set; }

        [IncludeInReport(Order = 7)]
        public int? SemaineFin { get; set; }
        [IncludeInReport(Order = 8)]
        public string? Semestre { get;set; }
        [IncludeInReport(Order = 9)]
        public string? isComuncours { get; set; }

        [IncludeInReport(Order = 10)]
        public string? Nombre_de_seance_par_Semaine{ get; set; }
        [IncludeInReport(Order = 11)]
        public int? Nombre_Total_heure { get; set; }
    }
}

