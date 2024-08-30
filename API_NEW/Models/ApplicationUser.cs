using Microsoft.AspNetCore.Identity;

namespace API_NEW.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
