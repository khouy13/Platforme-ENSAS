using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models
{
    public class EmploiTemps
    {

        [Key]

        public int IdEmploi { get; set; }
        [Required(ErrorMessage = "La semaine de début est obligatoire.")]
        [Display(Name = "Semaine de début")]
        public int SemaineDebut { get; set; }

        [Required(ErrorMessage = "La semaine de fin est obligatoire.")]
        [Display(Name = "Semaine de fin")]
        public int SemaineFin { get; set; }


        public DateTime? Date { get; set; }

        public int? IdJour { get; set; }
        [ForeignKey(nameof(IdJour))]
        [InverseProperty("EmploisJour")]
        public Jour? Jour { get; set; }
        public int IdNiveau { get; set; }
        [ForeignKey(nameof(IdNiveau))]
        [InverseProperty("EmploisNiveau")]
        public Niveau? Niveau { get; set; }
        public int? IdSemestre { get; set; }
        [ForeignKey(nameof(IdSemestre))]
        [InverseProperty("EmploisSemestre")]
        public Semestre? semestre { get; set; }
        public int? IdEnseignant { get; set; }
        [ForeignKey(nameof(IdEnseignant))]
        public Enseignant? Enseignant { get; set; }
        [InverseProperty("EmploisEnseignat")]
        public int? IdVacataire { get; set; }
        [ForeignKey(nameof(IdVacataire))]
        public Vacataire? Vacataire { get; set; }
        [InverseProperty("EmploisVacataire")]
        public int? IdGroupe { get; set; }
        [ForeignKey(nameof(IdGroupe))]
        [InverseProperty("EmploisGroupe")]
        public Groupe? Groupe { get; set; }

        public int? IdLocal { get; set; }
        [ForeignKey(nameof(IdLocal))]
        [InverseProperty("EmploisLocal")]
        public Local? Local { get; set; }

        [Required]
        public int? IdMatiere { get; set; }
        [ForeignKey(nameof(IdMatiere))]
        [InverseProperty("EmploisMatiere")]
        public Matiere? Matiere { get; set; }
        public int? IdSeance { get; set; }
        [ForeignKey(nameof(IdSeance))]
        [InverseProperty("EmploisSeance")]
        public Seance? Seance { get; set; }
        [Required]
        public int? IdTypeEnseignement { get; set; }
        [ForeignKey(nameof(IdTypeEnseignement))]
        [InverseProperty("EmploisTypeEnseignement")]
        public TypeEnseignement? TypeEnseignement { get; set; }
        public bool? isComuncours { get; set; }

    }
}

