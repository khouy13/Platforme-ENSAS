using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Projet.Areas.Responsable.Models;

public partial class Niveau
{
   
    [Key]
    public int IdNiveau { get; set; }

    public string NomNiveau { get; set; } = null!;
    
    public int? IdFiliere { get; set; }
    [ForeignKey(nameof(IdFiliere))]
    [InverseProperty("Niveaus")]
    public  Filiere? filiere { get; set; }

    [InverseProperty(nameof(Groupe.niveau))]
    public virtual ICollection<Groupe>? Groupes { get; set; }

    [InverseProperty(nameof(EmploiTemps.Niveau))]
    public virtual ICollection<EmploiTemps>? EmploisNiveau { get; set; }

    //EmploiNiveau
    [InverseProperty(nameof(EmploiExam.niveau))]
    public virtual ICollection<EmploiExam>? EmploiNiveau { get; set; }

    [InverseProperty(nameof(MatiereNiveau.Niveau))]
    public virtual ICollection<MatiereNiveau>? NiveauMatieres { get; set; }
}
