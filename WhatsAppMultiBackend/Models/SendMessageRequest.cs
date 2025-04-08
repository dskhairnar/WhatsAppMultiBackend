using System.ComponentModel.DataAnnotations;

namespace WhatsAppMultiBackend.Models
{
    public class SendMessageRequest
    {
        [Required]
        public required string PhoneNumber { get; set; }
        
        [Required]
        public required string MessageContent { get; set; }
    }
}