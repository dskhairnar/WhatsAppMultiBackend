namespace WhatsAppMultiBackend.Models
{
    public class Employee : User
    {
        public List<WhatsAppSession> AssignedSessions { get; set; } = new();
    }
}
