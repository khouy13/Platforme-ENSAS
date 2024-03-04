using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class MatiereNiveau
    {
        [Key]
        public int Id { get; set; }
        public int? IdNiveau { get; set; }

        [ForeignKey(nameof(IdNiveau))]
        [InverseProperty("NiveauMatieres")]
        
        public Niveau? Niveau { get; set; }
        public int? IdMatiere { get; set; }

        [ForeignKey(nameof(IdMatiere))]
        [InverseProperty("MatiereNiveaus")]
        public Matiere? Matiere { get; set; }


    }
}
