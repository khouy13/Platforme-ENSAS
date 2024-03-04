using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string GradeNomComplet { get; set; }
        [InverseProperty(nameof(Enseignant.gradeEnseigant))]
        public virtual ICollection<Enseignant>? Enseignants { get; set; }
        [InverseProperty(nameof(Vacataire.gradeEnseigant))]
        public virtual ICollection<Vacataire>? Vacataires { get; set; }
    }
}
