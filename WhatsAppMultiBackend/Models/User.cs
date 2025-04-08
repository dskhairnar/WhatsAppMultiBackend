using System.ComponentModel.DataAnnotations;

namespace WhatsAppMultiBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Role { get; set; } // "Client" or "Employee"
    }
}
