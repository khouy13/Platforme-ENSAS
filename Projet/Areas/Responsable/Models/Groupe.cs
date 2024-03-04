using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projet.Areas.Coordonnateur.Models;

namespace Projet.Areas.Responsable.Models;

public partial class Groupe
{
    [Key]
    public int IdGroupe { get; set; }

    public string NomGroup { get; set; } = null!;
    public int? IdNiveau { get; set; }
    [ForeignKey(nameof(IdNiveau))]
    [InverseProperty("Groupes")]
    public Niveau? niveau { get; set; }
    [InverseProperty(nameof(EmploiTemps.Groupe))]
    public virtual ICollection<EmploiTemps>? EmploisGroupe { get; set; }
}
