using Microsoft.AspNetCore.Identity;

namespace MagicVilla_API.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
