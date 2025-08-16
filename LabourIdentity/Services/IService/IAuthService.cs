using LabourIdentity.DTOs;
using LabourIdentity.Models;

namespace LabourIdentity.Services.IService
{
    public interface IAuthService
    {
        public Task<string> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<bool> AssignRole(string email, string roleName);
        public Task<ApplicationUser> CreateOrGetUserFromTokenAsync(string acessToken);
    }
}
