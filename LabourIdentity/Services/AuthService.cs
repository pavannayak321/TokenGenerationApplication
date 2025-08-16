using LabourIdentity.Data;
using LabourIdentity.DTOs;
using LabourIdentity.Models;
using LabourIdentity.Services.IService;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace LabourIdentity.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly RoleManager<IdentityRole>? _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser>? userManager, RoleManager<IdentityRole>? roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
                _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async  Task<bool> AssignRole(string email, string roleName)
        {
            var result = _db.ApplicationUsers.FirstOrDefault(u => u.Email == email);
            if (result != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create rle if does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                _userManager.AddToRoleAsync(result, roleName).GetAwaiter().GetResult();
                return true;
            }
            return false;
        }

        public async  Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u=>u.UserName== loginRequestDTO.UserName);
            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if(user==null || isValidUser == false)
            {
                return new LoginResponseDTO { Token = string.Empty, User = new UserDTO() };
            }
            //If User is found we need to generate the JWT Token
            var token = _jwtTokenGenerator.GenerateTokenAsync(user).Result;
            UserDTO userDto = new()
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id
            };
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO
            {
                User = userDto,
                Token = token
            };
            return loginResponseDTO;
        }

        public async Task<string> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                Name = registrationRequestDTO.Name,
                Email = registrationRequestDTO.Email,
                PhoneNumber = registrationRequestDTO.PhoneNumber,
                UserName = registrationRequestDTO.Email,
                LastName = registrationRequestDTO.LastName
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(au => au.Email == registrationRequestDTO.Email);

                    UserDTO userDto = new()
                    {
                        Name = userToReturn.Name,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber,
                        Id = userToReturn.Email
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return "Error Encountered";
        }
        public async Task<ApplicationUser> CreateOrGetUserFromTokenAsync(string acessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(acessToken);

            var email = jwt.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var givenName = jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var familyName = jwt.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;

            if (string.IsNullOrEmpty(email))
                throw new Exception("Email claim not found in token");
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return existingUser;
            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = givenName ?? "",
                LastName = familyName ?? ""
            };
            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            return newUser;
        }
    }
}
