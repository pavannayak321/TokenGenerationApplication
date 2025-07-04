using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TokenGenerationApplication.DTOs;
using TokenGenerationApplication.service;

namespace TokenGenerationApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtService _jwtService;
        public AccountController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpGet]
        public Task<LoginResponseDTO> Get()
        {
            var token = _jwtService.Authenticate(new LoginRequestDTO
            {
                Username = "pavankumar",
                Password = "pavan123"
            });

            return token!;
        }

        [Authorize(Policy = "PavanPolicy")]
        [HttpGet("employees")]
        public IActionResult GetEmployee()
        {
            var username = User.Identity?.Name; // Comes from ClaimTypes.Name
            var subject = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Username = username,
                SubjectClaim = subject,
                Role = role
            });
        }

        // Optional endpoint to debug all claims
        [Authorize]
        [HttpGet("claims")]
        public IActionResult GetAllClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}
