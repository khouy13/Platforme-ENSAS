using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Projet.Areas.Admin.Models;

public  class Enseignant
{
    [Key]
    public int IdEnseignant { get; set; }
    [Required(ErrorMessage = "Le nom de l'enseignant est requis.")]
    public string NomEnseignant { get; set; } = null!;
    [Required(ErrorMessage = "Le nom de l'enseignant est requis.")]
    public string PrenomEnseignant { get; set; } = null!;
    [Required(ErrorMessage = "Le nom de l'enseignant est requis.")]
    public string SpecialiteEnseignant { get; set; } = null!;

    [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
   
    public string? Email { get; set; }
    public int? IdDepartement { get; set; }
    [ForeignKey(nameof(IdDepartement))]
    [InverseProperty("Enseignants")]
    public Departement? departement { get; set; }
   
    [InverseProperty(nameof(EmploiTemps.Enseignant))]
    public virtual ICollection<EmploiTemps>? EmploisEnseignat { get; set; }
    //propreite calculé :
    [InverseProperty(nameof(ApplicationUser.Enseignant))]
    public virtual ICollection<ApplicationUser>? CompteEnseignat { get; set; }
    public string NomComplet
    {
        get { return $"{NomEnseignant}  {PrenomEnseignant}"; }
    }
    //IdEmploiExam

    //EnseignatMatiere
    [InverseProperty(nameof(Matiere.Enseignant))]
    public virtual ICollection<Matiere>? EnseignatMatiere { get; set; }
    [InverseProperty(nameof(EmploiExamEnseignant.Enseignant))]
    public virtual ICollection<EmploiExamEnseignant>? EmploiExamEnseignants { get; set; }

    public int? GradeId { get; set; }
    [ForeignKey(nameof(GradeId))]
    [InverseProperty("Enseignants")]
    public Grade? gradeEnseigant { get; set; }
   

    //
}
