using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Projet.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class Vacataire
    {
        [Key]
        [Required]
        public int IdVacataire { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string? Email { get; set; }
        public string? specialité { get; set; }
        [InverseProperty(nameof(EmploiTemps.Vacataire))]
        public virtual ICollection<EmploiTemps>? EmploisVacataire { get; set; }

        [InverseProperty(nameof(ApplicationUser.Vacataire))]

        public virtual ICollection<ApplicationUser>? CompteVacataire { get; set; }
        public int? IdDepartement { get; set; }
        [ForeignKey(nameof(IdDepartement))]
        [InverseProperty("Vacataires")]
        public Departement? departement { get; set; }
        public string NomComplet
        {
            get { return $"{Nom}  {Prenom}"; }
        }
      
        public virtual ICollection<EmploiExamVacataire>? EmploiExamVacataires { get; set; }
        public int? GradeId { get; set; }
        [ForeignKey(nameof(GradeId))]
        [InverseProperty("Vacataires")]
        public Grade? gradeEnseigant { get; set; }


        //MatiereVacataire
        [InverseProperty(nameof(Matiere.Vacataire))]

        public virtual ICollection<Matiere>? MatiereVacataire { get; set; }
    }
}
