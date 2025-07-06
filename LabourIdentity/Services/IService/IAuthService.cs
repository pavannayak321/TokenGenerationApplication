using LabourIdentity.DTOs;

namespace LabourIdentity.Services.IService
{
    public interface IAuthService
    {
        public  Task<string> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
