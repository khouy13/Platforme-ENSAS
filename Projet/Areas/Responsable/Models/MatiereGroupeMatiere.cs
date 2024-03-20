using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class MatiereGroupeMatiere
    {
        [Key]
        public int Id { get; set; }

        public int MatiereId { get; set; }
        [ForeignKey(nameof(MatiereId))]
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        public virtual Matiere Matiere { get; set; }

        public int GroupMatiereId { get; set; }
        [ForeignKey(nameof(GroupMatiereId))]
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        public virtual GroupeMatiere GroupeMatiere { get; set; }

    }
}
