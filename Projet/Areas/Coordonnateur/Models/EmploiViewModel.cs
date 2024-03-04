using Projet.Areas.Responsable.Models;

namespace Projet.Areas.Coordonnateur.Models
{
    public class EmploiViewModel
    {
        public List<EmploiTemps>? Emplois { get; set; }
        public List<Jour>? Jours { get; set; }
        public List<Seance>? Seances { get; set; }
        public Niveau? Niveau { get; set; }
        public Semestre? Semestre { get; set; }
        public string? NomLocal { get; set; }
        public string? NomVacataire { get; set; }
        public string? NomEnseignant { get; set; }
        public  string? nomAnneeScolaire { get; set; }
    }
}
