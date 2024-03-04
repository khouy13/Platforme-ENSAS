using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models;

public partial class Jour
{
    [Key]
    public int IdJour { get; set; }

    public string NomJour { get; set; } = null!;
    [InverseProperty(nameof(EmploiTemps.Jour))]
    public virtual ICollection<EmploiTemps>? EmploisJour { get; set; }
    //EmploiExamJour
    [InverseProperty(nameof(EmploiExam.Jour))]
    public virtual ICollection<EmploiExam>? EmploiExamJour { get; set; }
}
