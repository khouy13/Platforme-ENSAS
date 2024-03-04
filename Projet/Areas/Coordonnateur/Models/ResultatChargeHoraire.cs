using Projet.Areas.Admin.Models;

namespace Projet.Areas.Coordonnateur.Models
{
    public class ResultatChargeHoraire
    {
        public EmploiExam emploiExam { get; set; }
        public Enseignant Enseignant { get; set; }
        public Semestre? Semestre { get; set; }
        public string? Nomsemestre { get; set; }
        public string? NomComplet { get; set; }
        public  string? NomDepartement { get; set; }
        public string? GradeName { get; set; }
        public int? ChargeHoraire { get; set; }
        public string? isComuncours { get; set; }
        public string? TypeEnseignement { get; set; }
        public string? Matiere { get; set; }
        public int? SemaineDebut { get; set; }

        public int? Nombre_Total_heure { get; set; }
        public int? SemaineFin { get; set; }
    }

}
