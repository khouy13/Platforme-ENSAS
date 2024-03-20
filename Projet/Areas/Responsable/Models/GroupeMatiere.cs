using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class GroupeMatiere
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [InverseProperty(nameof(MatiereGroupeMatiere.GroupeMatiere))]
        public virtual ICollection<MatiereGroupeMatiere>? MatieresRelated { get; set; }
    }
}
