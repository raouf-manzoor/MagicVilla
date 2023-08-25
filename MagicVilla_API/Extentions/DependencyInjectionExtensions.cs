using MagicVilla_API.Repository.IRepository;
using MagicVilla_API.Repository;

namespace MagicVilla_API.Extentions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            // Register repository for managing villas.
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();

            // Register repository for managing villa numbers.
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

            // Register repository for managing users.
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
