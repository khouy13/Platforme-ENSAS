using System.ComponentModel.DataAnnotations;

namespace Projet.Areas.Responsable.Models
{
    public class MatiereGroupMatiereVM
    {
        [Required(ErrorMessage = "Le Nom Du Groupe Est Obligatoire !")]
        [Display(Name = "Nomination")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Selectionner Au moins une Matiere !")]
        [Display(Name = "Matieres")]
        public List<int> MatiereIds { get; set; } = new List<int>();
        public int? GroupeId { get; set;}
    }
}
