

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class MatiereCommun
    {
        [Key]
        public int Id { get; set; }

        public int MainMatiereId { get; set; }
        [ForeignKey(nameof(MainMatiereId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Matiere MainMatiere { get; set; }

        public int RelatedMatiereId { get; set; }
        [ForeignKey(nameof(RelatedMatiereId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Matiere RelatedMatiere { get; set; }
    }
}
