using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models
{
    public class TypeLocal
    {
        [Key]
        public int IdTypeLocal { get; set; }
        public string Nom { get; set; }

        [InverseProperty(nameof(Local.TypeLocal))]
        public virtual ICollection<Local>? Locals { get; set; }
    }
}
