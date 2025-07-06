using LabourIdentity.Models;

namespace LabourIdentity.Services.IService
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateTokenAsync(ApplicationUser  application);
    }
}
