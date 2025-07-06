﻿namespace LabourIdentity.Models
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = 60; // Default to 60 minutes
    }
}
