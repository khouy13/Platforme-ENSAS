using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models
{
    public class Semestre
    {
        [Key]
        public int IdSemestre { get; set; }
        public string NomSemestre { get; set; }
        [Required(ErrorMessage = "La semaine de début est obligatoire.")]
        [Display(Name = "Semaine de début")]
        public int SemaineDebut { get; set; }
        public DateTime DateDebut { get; set; } 
        public DateTime DateFin { get; set; }
        [Required(ErrorMessage = "La semaine de fin est obligatoire.")]
        [Display(Name = "Semaine de fin")]
        public int SemaineFin { get; set; }
        [InverseProperty(nameof(EmploiTemps.semestre))]
        public virtual ICollection<EmploiTemps>? EmploisSemestre { get; set; }

      
        [InverseProperty(nameof(Examen.semestre))]
        public virtual ICollection<Examen>? ExamenSemestre { get; set; }
        //EmploisExamSemestre
        [InverseProperty(nameof(EmploiExam.semestre))]
        public virtual ICollection<EmploiExam>? EmploisExamSemestre { get; set; }
    }
}
