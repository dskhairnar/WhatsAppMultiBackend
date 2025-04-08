using System.ComponentModel.DataAnnotations;

namespace WhatsAppMultiBackend.Models
{
    public class UpdateSessionRequest
    {
        [Required]
        public required string SessionData { get; set; }
    }

    public class CreateSessionRequest
    {
        [Required]
        public required string SessionName { get; set; }
        
        [Required]
        public required string SessionData { get; set; }
    }
}