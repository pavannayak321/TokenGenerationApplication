using Microsoft.AspNetCore.Identity;

namespace LabourIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        // You can add additional properties here if needed
        // For example, you might want to add FirstName, LastName, etc.
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
