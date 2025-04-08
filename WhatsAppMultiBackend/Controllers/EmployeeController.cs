using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WhatsAppMultiBackend.Data;
using WhatsAppMultiBackend.Models;
using WhatsAppMultiBackend.Services;

namespace WhatsAppMultiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly WhatsAppService _whatsAppService;

        public EmployeeController(AppDbContext context, WhatsAppService whatsAppService)
        {
            _context = context;
            _whatsAppService = whatsAppService;
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetAssignedSessions()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            
            if (employee == null)
                return NotFound("Employee not found");

            var sessions = await _whatsAppService.GetEmployeeSessionsAsync(employee.Id);
            return Ok(sessions);
        }

        [HttpGet("sessions/{id}")]
        public async Task<IActionResult> GetSession(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            
            if (employee == null)
                return NotFound("Employee not found");

            var session = await _whatsAppService.GetSessionByIdAsync(id);
            
            if (session == null)
                return NotFound("Session not found");

            if (!session.AllowedEmployees.Any(e => e.Id == employee.Id))
                return Forbid("You don't have access to this session");

            return Ok(session);
        }

        [HttpPut("sessions/{id}")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            
            if (employee == null)
                return NotFound("Employee not found");

            var session = await _whatsAppService.GetSessionByIdAsync(id);
            
            if (session == null)
                return NotFound("Session not found");

            if (!session.AllowedEmployees.Any(e => e.Id == employee.Id))
                return Forbid("You don't have access to this session");

            var success = await _whatsAppService.UpdateSessionDataAsync(id, request.SessionData);
            
            if (!success)
                return BadRequest("Failed to update session");

            return NoContent();
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            // Return only clients that have sessions assigned to this employee
            var username = User.FindFirstValue(ClaimTypes.Name);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            
            if (employee == null)
                return NotFound("Employee not found");

            var sessions = await _whatsAppService.GetEmployeeSessionsAsync(employee.Id);
            var clients = sessions.Select(s => s.Client).Distinct().ToList();
            
            return Ok(clients);
        }
        
        [HttpGet("all-sessions")]
        public async Task<IActionResult> GetAllSessions()
        {
            // Admin-like functionality to view all sessions
            var sessions = await _whatsAppService.GetAllSessionsAsync();
            return Ok(sessions);
        }
        
        [HttpPost("assign-session/{sessionId}")]
        public async Task<IActionResult> AssignSession(int sessionId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            
            if (employee == null)
                return NotFound("Employee not found");
                
            var success = await _whatsAppService.AssignSessionToEmployeeAsync(sessionId, employee.Id);
            
            if (!success)
                return BadRequest("Failed to assign session");
                
            return Ok("Session assigned successfully");
        }
        
        [HttpPost("send-message/{sessionId}")]
        public async Task<IActionResult> SendMessage(int sessionId, [FromBody] SendMessageRequest message)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            
            if (employee == null)
                return NotFound("Employee not found");
                
            var session = await _whatsAppService.GetSessionByIdAsync(sessionId);
            
            if (session == null)
                return NotFound("Session not found");
                
            if (!session.AllowedEmployees.Any(e => e.Id == employee.Id))
                return Forbid("You don't have access to this session");
                
            // This would integrate with a real WhatsApp API in production
            // For now, we'll just simulate a successful message send
            return Ok(new { success = true, message = "Message sent successfully (simulated)" });
        }
    }
}