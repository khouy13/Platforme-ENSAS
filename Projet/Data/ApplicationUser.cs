using Microsoft.AspNetCore.Identity;
using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }
        [PersonalData]
        public string? LastName { get; set; }
        [InverseProperty(nameof(Filiere.ApplicationUser))]
        public string? ImagePath { get; set; }

        // IFormFile nous a permet d obtenir des informations sur le fichier quon aploader 
        [NotMapped] // Mark this property as not mapped to the database
        public IFormFile UserFile { get; set; }
        [InverseProperty(nameof(Filiere.ApplicationUser))]
        public virtual ICollection<Filiere>? Filieres { get; set; }
        public int? IdEnseignant { get; set; }
        [ForeignKey(nameof(IdEnseignant))]
        [InverseProperty("CompteEnseignat")]
        public Enseignant? Enseignant { get; set; }
        
        public int? IdVacataire { get; set; }
        [ForeignKey(nameof(IdVacataire))]
        [InverseProperty("CompteVacataire")]
        public Vacataire? Vacataire { get; set; }
       
         [InverseProperty(nameof(Departement.ApplicationUser))]
            public virtual ICollection<Departement>? Departemets { get; set; }
        public string NomComplet
        {
            get { return $"{LastName}  {FirstName}"; }
        }

    }

}