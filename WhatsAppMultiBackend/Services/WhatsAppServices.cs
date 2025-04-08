using Microsoft.EntityFrameworkCore;
using WhatsAppMultiBackend.Data;
using WhatsAppMultiBackend.Models;

namespace WhatsAppMultiBackend.Services
{
    public class WhatsAppService
    {
        private readonly AppDbContext _context;

        public WhatsAppService(AppDbContext context)
        {
            _context = context;
        }
        
        // Employee methods
        public async Task<List<WhatsAppSession>> GetEmployeeSessionsAsync(int employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.AssignedSessions)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
                
            return employee?.AssignedSessions.ToList() ?? new List<WhatsAppSession>();
        }
        
        public async Task<WhatsAppSession?> GetSessionByIdAsync(int sessionId)
        {
            return await _context.WhatsAppSessions
                .Include(s => s.AllowedEmployees)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }
        
        public async Task<bool> UpdateSessionDataAsync(int sessionId, string sessionData)
        {
            var session = await _context.WhatsAppSessions.FindAsync(sessionId);
            if (session == null) return false;
            
            // Update session data (this could be any WhatsApp-related data)
            // In a real implementation, this might involve calling a Node.js service
            session.QRCodeData = sessionData; // For demo purposes
            session.LastUpdated = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateQRCode(string userId)
        {
            // Simulate QR code generation (you can replace this with Node API call)
            var qrCode = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

            var session = await _context.WhatsAppSessions
                .FirstOrDefaultAsync(s => s.UserId == userId);
                
            if (session == null)
            {
                // Find the client associated with this userId
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Username == userId);
                    
                if (client == null)
                    throw new InvalidOperationException("Client not found");
                    
                session = new WhatsAppSession
                {
                    UserId = userId,
                    QRCodeData = qrCode,
                    IsLoggedIn = false,
                    LastUpdated = DateTime.UtcNow,
                    Client = client
                };
                _context.WhatsAppSessions.Add(session);
            }
            else
            {
                session.QRCodeData = qrCode;
                session.LastUpdated = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return qrCode;
        }

        public async Task<bool> MarkAsLoggedIn(string userId)
        {
            var session = await _context.WhatsAppSessions
                .FirstOrDefaultAsync(s => s.UserId == userId);
            if (session == null) return false;

            session.IsLoggedIn = true;
            session.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WhatsAppSession?> GetSession(string userId)
        {
            return await _context.WhatsAppSessions
                .Include(s => s.Client)
                .Include(s => s.AllowedEmployees)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
        
        // Additional methods for session management
        public async Task<List<WhatsAppSession>> GetAllSessionsAsync()
        {
            return await _context.WhatsAppSessions
                .Include(s => s.AllowedEmployees)
                .ToListAsync();
        }
        
        public async Task<bool> AssignSessionToEmployeeAsync(int sessionId, int employeeId)
        {
            var session = await _context.WhatsAppSessions.FindAsync(sessionId);
            var employee = await _context.Employees.FindAsync(employeeId);
            
            if (session == null || employee == null) return false;
            
            if (session.AllowedEmployees == null)
                session.AllowedEmployees = new List<Employee>();
                
            if (!session.AllowedEmployees.Any(e => e.Id == employeeId))
                session.AllowedEmployees.Add(employee);
                
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
