using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppMultiBackend.Services;

namespace WhatsAppMultiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    public class ClientController : ControllerBase
    {
        private readonly WhatsAppService _whatsAppService;

        public ClientController(WhatsAppService whatsAppService)
        {
            _whatsAppService = whatsAppService;
        }

        [HttpGet("generate-qr")]
        public async Task<IActionResult> GenerateQRCode()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name); // username
            var qr = await _whatsAppService.GenerateQRCode(userId);
            return Ok(new { qr });
        }

        [HttpPost("mark-logged-in")]
        public async Task<IActionResult> MarkLoggedIn()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var success = await _whatsAppService.MarkAsLoggedIn(userId);
            return success ? Ok("Marked as logged in") : BadRequest("Failed");
        }

        [HttpGet("session")]
        public async Task<IActionResult> GetSession()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var session = await _whatsAppService.GetSession(userId);
            return session == null ? NotFound() : Ok(session);
        }
    }
}
