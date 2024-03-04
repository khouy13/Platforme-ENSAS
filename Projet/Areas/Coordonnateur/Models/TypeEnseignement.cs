using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models;

public partial class TypeEnseignement
{
    [Key]
    public int Id { get; set; }

    public string NomEn { get; set; } = null!;
    [InverseProperty(nameof(EmploiTemps.TypeEnseignement))]
    public virtual ICollection<EmploiTemps>? EmploisTypeEnseignement { get; set; } 
}
