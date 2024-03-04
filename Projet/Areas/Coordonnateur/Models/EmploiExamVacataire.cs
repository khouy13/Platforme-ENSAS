using Projet.Areas.Responsable.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Projet.Areas.Coordonnateur.Models
{
    public class EmploiExamVacataire
    {
        [Key]
        public int Id { get; set; }
        public int IdEmploiExam { get; set; }
        [ForeignKey(nameof(IdEmploiExam))]
        [InverseProperty("EmploiExamVacataires")]
        public EmploiExam EmploiExam { get; set; }

        public int IdVacataire { get; set; }
        [ForeignKey(nameof(IdVacataire))]
        [InverseProperty("EmploiExamVacataires")]
        public Vacataire Vacataire { get; set; }
    }
}
