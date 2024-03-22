using System.ComponentModel.DataAnnotations;

namespace Projet.Areas.Coordonnateur.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public Boolean IsMessageActive { get; set; } = false;
        public string? EmailSender { get; set; }
        public string? EmailSenderPassword { get; set; } 

    }
}
