namespace WhatsAppMultiBackend.Models
{
    public class Client : User
    {
        public List<WhatsAppSession> Sessions { get; set; } = new();
    }
}
