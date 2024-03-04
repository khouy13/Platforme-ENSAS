using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Projet.Areas.Coordonnateur.Models
{
    public class EmploiExamLocal
    {
        [Key]
        public int Id { get; set; }
        public int IdEmploiExam { get; set; }
        [ForeignKey(nameof(IdEmploiExam))]
        [InverseProperty("EmploiExamLocals")]
        public EmploiExam EmploiExam { get; set; }

        public int IdLocal { get; set; }
        [ForeignKey(nameof(IdLocal))]
        [InverseProperty("EmploiExamLocals")]
        public Local Local { get; set; }
    }
}
