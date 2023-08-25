using MagicVilla_API.Data;
using MagicVilla_API.Models;
using Microsoft.AspNetCore.Identity;

namespace MagicVilla_API.Extentions
{
    public static class IdentityExtensions
    {
        // Microsft .Net Identity setup

        /// <summary>
        /// Configures Identity services for the application.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void ConfigureIdentityServices(this WebApplicationBuilder builder)
        {
            // Configure Entity Framework support to enable integration with the .NET Identity system.
            // This step is necessary to create and manage tables required for .NET Identity in the database.
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()

                // Configure Identity to use Entity Framework as the storage mechanism.
                // This setup enables Identity to interact with the application's database.

                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
