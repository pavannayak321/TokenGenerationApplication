﻿namespace LabourIdentity.DTOs
{
    public class RegistrationRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string LastName { get;set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
