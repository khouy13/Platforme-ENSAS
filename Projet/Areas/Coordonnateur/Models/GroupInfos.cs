namespace Projet.Areas.Coordonnateur.Models
{
    public class GroupInfos
    {
        public List<int?>? IdMatieres { get; set; }
        public bool? IsComuncours { get; set; }
        public int? NombreElements { get; set; }
        public int? SemaineDebut { get; set; }
        public int? SemaineFin { get; set; }
        public string? NomComplet { get; set; }
        public string? TypeEnseignement { get; set; }
        public string? Departement { get; set; }
        public string? GradeName { get; set; }
        public string? NomSemestre { get; set; }
        public EmploiTemps? PremierEmploi { get; set; }
        public bool IsVacataire { get; set; }
    }
}
