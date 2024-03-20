using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;

namespace Projet.Areas.Responsable.Models;

public partial class Matiere
{
    [Key]
    public int IdMatiere { get; set; }

    public string NomMatiere { get; set; } = null!;

    [InverseProperty(nameof(EmploiTemps.Matiere))]
    public virtual ICollection<EmploiTemps>? EmploisMatiere { get; set; }

    [InverseProperty(nameof(EmploiExam.matiere))]
    public virtual ICollection<EmploiExam>? EmploiMatiereExam { get; set; }

    [InverseProperty(nameof(MatiereNiveau.Matiere))]
    public virtual ICollection<MatiereNiveau>? MatiereNiveaus { get; set; }

    public int? IdEnseignant { get; set; }
    [ForeignKey(nameof(IdEnseignant))]
    [InverseProperty("EnseignatMatiere")]
    public virtual Enseignant? Enseignant { get; set; }

    public int? IdVacataire { get; set; }
    [ForeignKey(nameof(IdVacataire))]
    [InverseProperty("MatiereVacataire")]
    public virtual Vacataire? Vacataire { get; set; }

    [InverseProperty(nameof(MatiereGroupeMatiere.Matiere))]
    public virtual ICollection<MatiereGroupeMatiere>? MatiereGroupeMatieres { get; set; }
}


