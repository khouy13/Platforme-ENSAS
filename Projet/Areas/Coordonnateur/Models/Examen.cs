using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models
{
    public class Examen
    {
        [Key]
        public int IdExamen { get; set; }
        public DateTime DateExamen { get; set; }
        public int NumeroExamen {get;set;}
       
        public int? IdSemestre { get; set; }
        [ForeignKey(nameof(IdSemestre))]
        [InverseProperty("ExamenSemestre")]
        public Semestre? semestre { get; set; }
        //ici j ai ajouter ceci pour l utiliser dans l affichage pendant l ajout d 'un exam 
        public string NumeroExamenWithDSAndSemestre => $"DS {NumeroExamen} - {(semestre != null ? semestre.NomSemestre : "")}";
        public string NomExam => $"DS {NumeroExamen}";
        //EmploiExamen
        [InverseProperty(nameof(EmploiExam.examen))]
        public virtual ICollection<EmploiExam>? EmploiExamen { get; set; }


    }
}
