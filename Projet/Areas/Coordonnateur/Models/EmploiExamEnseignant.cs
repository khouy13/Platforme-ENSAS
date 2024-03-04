using Projet.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models
{
    public class EmploiExamEnseignant
    {
        [Key]
        public int Id { get; set; }
        public int IdEmploiExam { get; set; }
        [ForeignKey(nameof(IdEmploiExam))]
        [InverseProperty("EmploiExamEnseignants")]
        public EmploiExam EmploiExam { get; set; }

        public int IdEnseignant { get; set; }
        [ForeignKey(nameof(IdEnseignant))]
        [InverseProperty("EmploiExamEnseignants")]
        public Enseignant Enseignant { get; set; }
    }
}
