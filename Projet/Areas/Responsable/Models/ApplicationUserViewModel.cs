using Microsoft.AspNetCore.Identity;
using Projet.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet.Areas.Responsable.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        [PersonalData]
        public string? LastName { get; set; }
        public int? IdEnseignant { get; set; }
        [ForeignKey(nameof(IdEnseignant))]
        [InverseProperty("CompteEnseignat")]
        public Enseignant? Enseignant { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public int? IdVacataire { get; set; }
        [ForeignKey(nameof(IdVacataire))]
        [InverseProperty("CompteVacataire")]
        public Vacataire? Vacataire { get; set; }
        [EmailAddress]
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? UserFile { get; set; }
        public IList<string>? Roles { get; set; }
        public string NomComplet
        {
            get { return $"{LastName} {FirstName}"; }
        }
    }

}
