using System;
using System.Collections.Generic;
using WhatsAppMultiBackend.Models;

namespace WhatsAppMultiBackend.Models
{
    public class WhatsAppSession
    {
        public int Id { get; set; }
        public string UserId { get; set; } // FK to User
        public string QRCodeData { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastUpdated { get; set; }
        
        // Navigation properties
        public Client? Client { get; set; } // The client who owns this session
        public List<Employee> AllowedEmployees { get; set; } = new(); // Employees who can access this session
    }
}
