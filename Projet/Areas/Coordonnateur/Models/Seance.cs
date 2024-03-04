using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models;

public partial class Seance
{
    [Key]
    public int IdSeance { get; set; }
    public string NomSeance { get; set; }
    public TimeSpan dateDebut { get; set; }
    public TimeSpan dateFin { get; set; }
    [InverseProperty(nameof(EmploiTemps.Seance))]
    public virtual ICollection<EmploiTemps>? EmploisSeance { get; set; }
    //EmploiExamSeance
    [InverseProperty(nameof(EmploiExam.Seance))]
    public virtual ICollection<EmploiExam>? EmploiExamSeance { get; set; }
}
