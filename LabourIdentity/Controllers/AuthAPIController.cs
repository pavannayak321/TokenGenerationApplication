using LabourIdentity.DTOs;
using LabourIdentity.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabourIdentity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDTO _responseDTO;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDTO = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registerRequestDTO)
        {

            var errorMessage = await _authService.RegisterAsync(registerRequestDTO);
            if (string.IsNullOrEmpty(errorMessage))
            {
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "User registered successfully.";
                return Ok(_responseDTO);
            }
            else
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = errorMessage;
                return BadRequest(_responseDTO);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var result =await  _authService.Login(loginRequestDTO);
            if (result.User == null)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Invalid username or password.";
                return BadRequest(_responseDTO);
            }
            else
            {
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Login successful.";
                _responseDTO.Result = result;
                return Ok(_responseDTO);
            }
        }
    }
}

