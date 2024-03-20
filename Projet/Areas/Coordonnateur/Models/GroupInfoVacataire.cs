namespace Projet.Areas.Coordonnateur.Models
{
    public class GroupeInfoVacataire
    {
        public List<int?>? IdMatieres { get; set; }
        public bool? IsComuncours { get; set; }
        public int NombreElements { get; set; }
        public string? TypeEnseignement { get; set; }
        public string? Departement { get; set; }
        public string? GradeName { get; set; }
        public string? NomComplet { get; set; }
        public EmploiTemps? PremierEmploi { get; set; }
    }
}
