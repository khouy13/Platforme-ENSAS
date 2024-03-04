using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models
{
    public class EmploiExam
    {
        [Key]
        public int IdEmploiExam{get;set;}
        public int? IdExamen { get; set; }
        [ForeignKey(nameof(IdExamen))]
        [InverseProperty("EmploiExamen")]
        public Examen? examen { get; set; }
        //ExamenEnseignant
       
        public string? typeEmploi { get; set; }
       
        //IdMatiere
        public int? IdMatiere { get; set; }
        [ForeignKey(nameof(IdMatiere))]
        [InverseProperty("EmploiMatiereExam")]
        public Matiere? matiere { get; set; }
        //emploiExam
        


        public int? IdNiveau { get; set; }
        [ForeignKey(nameof(IdNiveau))]
        [InverseProperty("EmploiNiveau")]
        public Niveau? niveau { get; set; }
        public bool? isComunExam { get; set; }
        public int? IdJour { get; set; }
        [ForeignKey(nameof(IdJour))]
        [InverseProperty("EmploiExamJour")]
        public Jour? Jour { get; set; }
        public int? IdSeance { get; set; }
        [ForeignKey(nameof(IdSeance))]
        [InverseProperty("EmploiExamSeance")]
        public Seance? Seance { get; set; }
        public DateTime? DateEmploiExam { get; set; }
        public int? IdSemestre { get; set; }
        [ForeignKey(nameof(IdSemestre))]
        [InverseProperty("EmploisExamSemestre")]
        public Semestre? semestre { get; set; }
        [InverseProperty(nameof(EmploiExamLocal.EmploiExam))]
        public virtual ICollection<EmploiExamLocal>? EmploiExamLocals { get; set; }
        [InverseProperty(nameof(EmploiExamEnseignant.EmploiExam))]
        public virtual ICollection<EmploiExamEnseignant>? EmploiExamEnseignants { get; set; }
        [InverseProperty(nameof(EmploiExamVacataire.EmploiExam))]
        public virtual ICollection<EmploiExamVacataire>? EmploiExamVacataires { get; set; }

    }
}
