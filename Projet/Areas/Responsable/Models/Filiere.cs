using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Projet.Areas.Admin.Models;
using Projet.Data;

namespace Projet.Areas.Responsable.Models;

public partial class Filiere

{
  

    [Key]
    public int IdFiliere { get; set; }

    public string NomFiliere { get; set; } = null!;
    public string? Abreviation { get; set; }
  
    
    public int? IdDepartement { get; set; }
    [ForeignKey(nameof(IdDepartement))]
    [InverseProperty("Filieres")]
    public virtual Departement? Departement { get; set; }
    [InverseProperty(nameof(Niveau.filiere))]
    public  ICollection<Niveau>? Niveaus { get; set; }
  

    public string? ApplicationUserId { get; set; }
    [ForeignKey(nameof(ApplicationUserId))]
    [InverseProperty("Filieres")]
    public ApplicationUser? ApplicationUser { get; set; }
   
}