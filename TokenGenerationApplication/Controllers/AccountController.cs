using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenGenerationApplication.Models;
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
        public Task<LoginResponse> Get()
        {
           
            var token =  _jwtService.Authenticate(new LoginRequest
            {
                Username = "pavankumar",
                Password = "pavan123"
            });
            return token;
        }
    }
}
