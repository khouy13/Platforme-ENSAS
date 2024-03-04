namespace Projet.Areas.Coordonnateur.Models
{
    public class GroupeInfoVacataire
    {
        public int IdMatiere { get; set; }
        public int IdTypeEnseignement { get; set; }
        public bool? IsComuncours { get; set; }
        public int NombreElements { get; set; }
        public string? Departement { get; set; }
        public string? GradeName { get; set; }
        public string? NomComplet { get; set; }
        public EmploiTemps? PremierEmploi { get; set; }
    }
}
