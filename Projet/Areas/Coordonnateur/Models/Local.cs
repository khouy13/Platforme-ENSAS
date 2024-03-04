using Projet.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Coordonnateur.Models;

public partial class Local
{
    [Key]
    public int IdLocal { get; set; }

    public string NomLocal { get; set; }

    public int? CapaciteLocal { get; set; }
    public int? IdTypeLocal { get; set; }
    [ForeignKey(nameof(IdTypeLocal))]
    [InverseProperty("Locals")]
    public TypeLocal? TypeLocal { get; set; }
    public string? DescriptionLocal { get; set; } 
    [InverseProperty(nameof(EmploiTemps.Local))]
    public virtual ICollection<EmploiTemps>? EmploisLocal { get; set; }

   
    [InverseProperty(nameof(EmploiExamLocal.Local))]
    public virtual ICollection<EmploiExamLocal>? EmploiExamLocals { get; set; }

}
