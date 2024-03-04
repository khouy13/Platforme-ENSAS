using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;

namespace Projet.Areas.Coordonnateur.Models
{
    public class CHargeServaianceViewModel
    {
      
        [IncludeInReport(Order = 1)]
        public string? NomComplet { get; set; }
        [IncludeInReport(Order = 2)]
        public string? NomDepartement { get; set; }
        [IncludeInReport(Order = 3)]
        public string? Grade { get; set; }
       
        [IncludeInReport(Order = 6)]
        public string? Nombre_heure_de_servaiance { get; set; }

    }
}

