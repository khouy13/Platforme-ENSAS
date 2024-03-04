
using Projet.Areas.Responsable.Models;
using Projet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Admin.Models;


public partial class Departement
{
    [Key]
    public int IdDepartement { get; set; }

    public string NomDepartementt { get; set; } = null!;
    public string? ApplicationUserId { get; set; }
    [ForeignKey(nameof(ApplicationUserId))]
    [InverseProperty("Departemets")]
    public ApplicationUser? ApplicationUser { get; set; }

    [InverseProperty(nameof(Filiere.Departement))]
    public virtual ICollection<Filiere>? Filieres { get; set; }
    [InverseProperty(nameof(Enseignant.departement))]
    public virtual ICollection<Enseignant>? Enseignants { get; set; }
    [InverseProperty(nameof(Vacataire.departement))]
    public virtual ICollection<Vacataire>? Vacataires { get; set; }
}
